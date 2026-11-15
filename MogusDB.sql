-- =====================================================================
-- MogusDB - Banco de dados do projeto Mogus (CRUD em C# + Site)
-- Conexão padrão (Conexao.cs): server = localhost; database = MogusDB; uid = root; pwd =
-- Outra conexão: defina a variável de ambiente MOGUS_CONEXAO.
--
-- Todas as tabelas seguem o modelo do CRUD (Usuario / Pedido):
--   chave primária  -> id<Tabela> (ou pedidoid / comidaID, como no código C#)
--   colunas         -> <Nome><Tabela>   Ex.: NomeUsuario, PrecoComida
--   chaves externas -> fk_<Filha>_<Mãe>, únicas -> uq_<Tabela>_<Coluna>
--
-- Tabelas:
--   Usuario        -> CRUD (Usuario.cs), cadastro/login/perfil do site
--   Sessao         -> tokens de login (só o hash do token é guardado)
--   TentativaLogin -> bloqueio do e-mail após 5 senhas erradas (15 minutos)
--   Endereco       -> endereços salvos do cliente
--   Cartao         -> cartões salvos (só bandeira e 4 últimos dígitos)
--   Categoria      -> categorias do cardápio
--   Comida         -> cardápio (lanches.html)
--   CarrinhoItem   -> carrinho do cliente logado
--   Compra         -> pedido finalizado (status, pagamento, frete, total)
--   Pedido         -> itens de cada compra (Pedido.cs)
--
-- Se você já rodou uma versão antiga deste script, apague o banco antes:
--   DROP DATABASE MogusDB;
-- =====================================================================

CREATE DATABASE IF NOT EXISTS MogusDB
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

USE MogusDB;

-- ---------------------------------------------------------------------
-- Usuario
-- SenhaUsuario guarda o hash PBKDF2 (pbkdf2$iterações$sal$hash), nunca a senha.
-- DataNascimentoUsuario e CpfUsuario aceitam NULL (o CRUD antigo não pedia).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Usuario (
    idUsuario             INT           NOT NULL AUTO_INCREMENT,
    NomeUsuario           VARCHAR(100)  NOT NULL,
    SenhaUsuario          VARCHAR(255)  NOT NULL,
    DataNascimentoUsuario DATE          NULL,
    TelefoneUsuario       VARCHAR(20)   NOT NULL,
    EmailUsuario          VARCHAR(100)  NOT NULL,
    CpfUsuario            CHAR(11)      NULL,
    PRIMARY KEY (idUsuario),
    UNIQUE KEY uq_Usuario_EmailUsuario (EmailUsuario),
    UNIQUE KEY uq_Usuario_CpfUsuario (CpfUsuario)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Sessao (login do site)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Sessao (
    idSessao       INT       NOT NULL AUTO_INCREMENT,
    idUsuario      INT       NOT NULL,
    TokenSessao    CHAR(64)  NOT NULL,
    CriadaEmSessao DATETIME  NOT NULL,
    ExpiraEmSessao DATETIME  NOT NULL,
    PRIMARY KEY (idSessao),
    UNIQUE KEY uq_Sessao_TokenSessao (TokenSessao),
    CONSTRAINT fk_Sessao_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- TentativaLogin (5 erros de senha bloqueiam o e-mail por 15 minutos)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TentativaLogin (
    EmailTentativa        VARCHAR(100) NOT NULL,
    FalhasTentativa       INT          NOT NULL DEFAULT 0,
    BloqueadoAteTentativa DATETIME     NULL,
    PRIMARY KEY (EmailTentativa)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Endereco
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Endereco (
    idEndereco          INT          NOT NULL AUTO_INCREMENT,
    idUsuario           INT          NOT NULL,
    ApelidoEndereco     VARCHAR(40)  NOT NULL,
    CepEndereco         CHAR(8)      NOT NULL,
    RuaEndereco         VARCHAR(120) NOT NULL,
    NumeroEndereco      VARCHAR(10)  NOT NULL,
    ComplementoEndereco VARCHAR(60)  NULL,
    BairroEndereco      VARCHAR(80)  NOT NULL,
    CidadeEndereco      VARCHAR(80)  NOT NULL,
    UfEndereco          CHAR(2)      NOT NULL,
    PRIMARY KEY (idEndereco),
    CONSTRAINT fk_Endereco_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Cartao (nunca guarda o número completo nem o CVV)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Cartao (
    idCartao             INT          NOT NULL AUTO_INCREMENT,
    idUsuario            INT          NOT NULL,
    BandeiraCartao       VARCHAR(20)  NOT NULL,
    UltimosDigitosCartao CHAR(4)      NOT NULL,
    NomeTitularCartao    VARCHAR(100) NOT NULL,
    ValidadeCartao       CHAR(5)      NOT NULL,
    PRIMARY KEY (idCartao),
    CONSTRAINT fk_Cartao_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Categoria
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Categoria (
    idCategoria   INT          NOT NULL AUTO_INCREMENT,
    NomeCategoria VARCHAR(60)  NOT NULL,
    PRIMARY KEY (idCategoria),
    UNIQUE KEY uq_Categoria_NomeCategoria (NomeCategoria)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Comida (cardápio de lanches.html)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Comida (
    comidaID        INT            NOT NULL AUTO_INCREMENT,
    idCategoria     INT            NOT NULL,
    NomeComida      VARCHAR(100)   NOT NULL,
    PrecoComida     DECIMAL(10,2)  NOT NULL,
    DescricaoComida VARCHAR(255)   NOT NULL,
    ImagemComida    VARCHAR(255)   NOT NULL,
    PRIMARY KEY (comidaID),
    UNIQUE KEY uq_Comida_NomeComida (NomeComida),
    CONSTRAINT fk_Comida_Categoria FOREIGN KEY (idCategoria)
        REFERENCES Categoria (idCategoria)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT ck_Comida_PrecoComida CHECK (PrecoComida >= 0)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- CarrinhoItem (carrinho do cliente logado, igual em qualquer aparelho)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS CarrinhoItem (
    idCarrinhoItem INT NOT NULL AUTO_INCREMENT,
    idUsuario      INT NOT NULL,
    comidaID       INT NOT NULL,
    quantidadeItem INT NOT NULL DEFAULT 1,
    PRIMARY KEY (idCarrinhoItem),
    UNIQUE KEY uq_CarrinhoItem_Usuario_Comida (idUsuario, comidaID),
    CONSTRAINT fk_CarrinhoItem_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_CarrinhoItem_Comida FOREIGN KEY (comidaID)
        REFERENCES Comida (comidaID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT ck_CarrinhoItem_quantidadeItem CHECK (quantidadeItem > 0)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Compra (pedido finalizado do site: pedidos.html)
-- Apagar um usuário apaga também as compras dele (permite DeletarUsuario do CRUD).
-- EnderecoCompra guarda o endereço por extenso, para o histórico não mudar
-- se o cliente editar ou apagar o endereço salvo depois.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Compra (
    idCompra             INT            NOT NULL AUTO_INCREMENT,
    idUsuario            INT            NOT NULL,
    idEndereco           INT            NULL,
    idCartao             INT            NULL,
    EnderecoCompra       VARCHAR(300)   NOT NULL DEFAULT '',
    FormaPagamentoCompra VARCHAR(20)    NOT NULL DEFAULT 'Pix',
    DataCompra           DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    StatusCompra         VARCHAR(30)    NOT NULL DEFAULT 'Recebido',
    SubtotalCompra       DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    FreteCompra          DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    TotalCompra          DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    PRIMARY KEY (idCompra),
    CONSTRAINT fk_Compra_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_Compra_Endereco FOREIGN KEY (idEndereco)
        REFERENCES Endereco (idEndereco)
        ON DELETE SET NULL
        ON UPDATE CASCADE,
    CONSTRAINT fk_Compra_Cartao FOREIGN KEY (idCartao)
        REFERENCES Cartao (idCartao)
        ON DELETE SET NULL
        ON UPDATE CASCADE,
    CONSTRAINT ck_Compra_FormaPagamentoCompra CHECK (FormaPagamentoCompra IN ('Pix', 'Cartão', 'Dinheiro')),
    CONSTRAINT ck_Compra_StatusCompra CHECK (StatusCompra IN ('Recebido', 'Em preparo', 'Saiu para entrega', 'Entregue', 'Cancelado'))
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Pedido (itens de uma compra)
-- Colunas pedidoid, comidaID e quantidadeItem iguais às usadas em Pedido.cs:
--   UPDATE Pedido SET comidaID = ..., quantidadeItem = ... WHERE pedidoid = ...
--   DELETE FROM Pedido WHERE pedidoid = ...
-- PrecoUnitarioPedido guarda o preço da comida no momento da compra.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Pedido (
    pedidoid            INT            NOT NULL AUTO_INCREMENT,
    idCompra            INT            NOT NULL,
    comidaID            INT            NOT NULL,
    quantidadeItem      INT            NOT NULL DEFAULT 1,
    PrecoUnitarioPedido DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    PRIMARY KEY (pedidoid),
    CONSTRAINT fk_Pedido_Compra FOREIGN KEY (idCompra)
        REFERENCES Compra (idCompra)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_Pedido_Comida FOREIGN KEY (comidaID)
        REFERENCES Comida (comidaID)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT ck_Pedido_quantidadeItem CHECK (quantidadeItem > 0)
) ENGINE = InnoDB;

