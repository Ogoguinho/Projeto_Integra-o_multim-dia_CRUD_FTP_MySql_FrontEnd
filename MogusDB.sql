-- =====================================================================
-- MogusDB - Banco de dados do projeto Mogus (CRUD em C# + Site)
-- Conexão usada pelo CRUD (Conexao.cs): server = localhost; database = MogusDB; uid = root; pwd =
--
-- Tabelas:
--   Usuario  -> usada pelo CRUD (Usuario.cs) e pelo login/cadastro do site (login.html)
--   Comida   -> cardápio do site (lanches.html), referenciada por Pedido.comidaID
--   Compra   -> "pedido" do site (pedidos.html): data, status e total do carrinho finalizado
--   Pedido   -> itens de cada compra, usada pelo CRUD (Pedido.cs): pedidoid, comidaID, quantidadeItem
-- =====================================================================

CREATE DATABASE IF NOT EXISTS MogusDB
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_general_ci;

USE MogusDB;

-- ---------------------------------------------------------------------
-- Usuario
-- Ordem das colunas igual ao INSERT do CRUD:
--   INSERT INTO Usuario VALUES(null, nome, senha, dataNascimento, telefone, email)
-- DataNascimentoUsuario aceita NULL porque o cadastro do site não pede essa informação.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Usuario (
    idUsuario             INT           NOT NULL AUTO_INCREMENT,
    NomeUsuario           VARCHAR(100)  NOT NULL,
    SenhaUsuario          VARCHAR(255)  NOT NULL,
    DataNascimentoUsuario DATE          NULL,
    TelefoneUsuario       VARCHAR(20)   NOT NULL,
    EmailUsuario          VARCHAR(100)  NOT NULL,
    PRIMARY KEY (idUsuario),
    UNIQUE KEY uq_Usuario_EmailUsuario (EmailUsuario)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Comida (cardápio de lanches.html)
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Comida (
    comidaID        INT            NOT NULL AUTO_INCREMENT,
    NomeComida      VARCHAR(100)   NOT NULL,
    PrecoComida     DECIMAL(10,2)  NOT NULL,
    DescricaoComida VARCHAR(255)   NOT NULL,
    ImagemComida    VARCHAR(255)   NOT NULL,
    PRIMARY KEY (comidaID),
    UNIQUE KEY uq_Comida_NomeComida (NomeComida)
) ENGINE = InnoDB;

-- ---------------------------------------------------------------------
-- Compra (pedido do site: data, status e total do carrinho finalizado)
-- Apagar um usuário apaga também as compras dele (permite DeletarUsuario do CRUD).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Compra (
    idCompra     INT            NOT NULL AUTO_INCREMENT,
    idUsuario    INT            NOT NULL,
    DataCompra   DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    StatusCompra VARCHAR(30)    NOT NULL DEFAULT 'Recebido',
    TotalCompra  DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    PRIMARY KEY (idCompra),
    CONSTRAINT fk_Compra_Usuario FOREIGN KEY (idUsuario)
        REFERENCES Usuario (idUsuario)
        ON DELETE CASCADE
        ON UPDATE CASCADE
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

-- ---------------------------------------------------------------------
-- Carga inicial: cardápio exibido em lanches.html
-- ---------------------------------------------------------------------
INSERT IGNORE INTO Comida (comidaID, NomeComida, PrecoComida, DescricaoComida, ImagemComida) VALUES
(1, 'the mogus',          29.90, 'Pão brioche com gergelim, hambúrguer de 120g, queijo cheddar, picles, alface, cebola roxa e a maionese da casa.', 'images/lanches/Hamburger-Recipe-7.jpg'),
(2, 'bacon mogus',        33.90, 'Pão brioche, hambúrguer de 120g, queijo cheddar, bacon, alface e a maionese da casa.',                          'images/lanches/images (2).jpg'),
(3, 'the chicken mogus',  27.90, 'Pão brioche, 120g de filé de frango empanado, alface, ketchup, bacon e a maionese da casa.',                    'images/lanches/2017-07-27-smart-burguer-cocoricrok-creditos-wellington-nemeth.webp'),
(4, 'the mogus-o-fish',   33.90, 'Pão brioche, 90g de filé de peixe empanado, alface, tomate e molho tártaro da casa.',                           'images/lanches/R0425-SP-hamburguer-de-peixe-empanado-1200x675-1.webp'),
(5, 'the doublegus',      29.90, 'Pão brioche, dois hambúrgueres de 80g, queijo cheddar, alface, tomate e bacon.',                                'images/lanches/doublemogus.jpg'),
(6, 'the fries',          12.90, 'Porção de 80g de batatas fritas.',                                                                              'images/lanches/Crispy-Oven-Fries15-2.jpg'),
(7, 'the supreme fries',  18.90, 'Porção de 90g de batatas fritas, com queijo, bacon, cebolinha e a maionese da casa.',                           'images/lanches/supremefries.jpg');
