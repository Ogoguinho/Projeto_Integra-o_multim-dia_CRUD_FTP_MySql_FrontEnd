/*Script completamente gerado pelo Claude com assistência do Stack Overflow.*/

const CHAVE_CARRINHO = "mogus_carrinho";
const CHAVE_PEDIDOS = "mogus_pedidos";

function lerLista(chave) {
    return JSON.parse(localStorage.getItem(chave)) || [];
}

function salvarLista(chave, lista) {
    localStorage.setItem(chave, JSON.stringify(lista));
}

// "29,90" -> 29.9
function converterPreco(texto) {
    return parseFloat(texto.replace(/\./g, "").replace(",", "."));
}

// 29.9 -> "29,90"
function formatarPreco(valor) {
    return valor.toFixed(2).replace(".", ",");
}

function criarCardItem(produto, textoBotao, aoClicar) {
    const card = document.createElement("div");
    card.className = "item";

    const imagem = document.createElement("img");
    imagem.src = produto.imagem;
    imagem.alt = "";

    const info = document.createElement("div");

    const nome = document.createElement("h1");
    nome.textContent = produto.nome;

    const preco = document.createElement("h2");
    preco.textContent = formatarPreco(produto.preco);

    const descricao = document.createElement("p");
    descricao.textContent = produto.descricao;

    const botao = document.createElement("button");
    const textoDoBotao = document.createElement("h1");
    textoDoBotao.textContent = textoBotao;
    botao.appendChild(textoDoBotao);
    botao.addEventListener("click", aoClicar);

    info.append(nome, preco, descricao, botao);
    card.append(imagem, info);
    return card;
}

/* ---------- LANCHES ---------- */

function iniciarMenu() {
    const menu = document.querySelector("#menu");
    if (!menu) return;

    const itens = menu.querySelectorAll(".item");

    itens.forEach(function (item) {
        const botao = item.querySelector("button");
        const textoBotao = botao.querySelector("h1");

        botao.addEventListener("click", function () {
            const carrinho = lerLista(CHAVE_CARRINHO);
            carrinho.push({
                nome: item.querySelector("h1").textContent,
                preco: converterPreco(item.querySelector("h2").textContent),
                imagem: item.querySelector("img").getAttribute("src"),
                descricao: item.querySelector("p").textContent
            });
            salvarLista(CHAVE_CARRINHO, carrinho);

            textoBotao.textContent = "ADICIONADO!";
            setTimeout(function () {
                textoBotao.textContent = "ADICIONAR AO CARRINHO";
            }, 1200);
        });
    });

    const busca = document.querySelector("header input");
    if (busca) {
        busca.addEventListener("input", function () {
            const termo = busca.value.trim().toLowerCase();
            itens.forEach(function (item) {
                const nome = item.querySelector("h1").textContent.toLowerCase();
                item.style.display = nome.includes(termo) ? "" : "none";
            });
        });
    }
}

/* ---------- CARRINHO ---------- */

function renderizarCarrinho() {
    const lista = document.querySelector("#lista-carrinho");
    if (!lista) return;

    const carrinho = lerLista(CHAVE_CARRINHO);
    const vazio = document.querySelector("#carrinho-vazio");
    const total = document.querySelector("#total");
    const botaoFinalizar = document.querySelector("#finalizar");

    lista.innerHTML = "";
    vazio.hidden = carrinho.length > 0;
    botaoFinalizar.hidden = carrinho.length === 0;

    let soma = 0;
    carrinho.forEach(function (produto, indice) {
        soma += produto.preco;
        lista.appendChild(criarCardItem(produto, "REMOVER DO CARRINHO", function () {
            const atual = lerLista(CHAVE_CARRINHO);
            atual.splice(indice, 1);
            salvarLista(CHAVE_CARRINHO, atual);
            renderizarCarrinho();
        }));
    });

    total.textContent = formatarPreco(soma);
}

function finalizarPedido() {
    const carrinho = lerLista(CHAVE_CARRINHO);
    if (carrinho.length === 0) return;

    const pedidos = lerLista(CHAVE_PEDIDOS);
    pedidos.push({
        id: Date.now(),
        data: new Date().toLocaleString("pt-BR"),
        itens: carrinho,
        total: carrinho.reduce(function (soma, produto) {
            return soma + produto.preco;
        }, 0)
    });

    salvarLista(CHAVE_PEDIDOS, pedidos);
    salvarLista(CHAVE_CARRINHO, []);
    window.location.href = "pedidos.html";
}

function iniciarCarrinho() {
    const botaoFinalizar = document.querySelector("#finalizar");
    if (!botaoFinalizar) return;

    botaoFinalizar.addEventListener("click", finalizarPedido);
    renderizarCarrinho();
}

/* ---------- PEDIDOS ---------- */

function renderizarPedidos() {
    const lista = document.querySelector("#lista-pedidos");
    if (!lista) return;

    const pedidos = lerLista(CHAVE_PEDIDOS);
    const vazio = document.querySelector("#pedidos-vazio");

    lista.innerHTML = "";
    vazio.hidden = pedidos.length > 0;

    pedidos.slice().reverse().forEach(function (pedido) {
        const card = document.createElement("div");
        card.className = "pedido";

        const titulo = document.createElement("h2");
        titulo.textContent = "Pedido de " + pedido.data;

        const itens = document.createElement("ul");
        pedido.itens.forEach(function (produto) {
            const linha = document.createElement("li");
            linha.textContent = produto.nome + " - R$ " + formatarPreco(produto.preco);
            itens.appendChild(linha);
        });

        const total = document.createElement("p");
        total.textContent = "Total: R$ " + formatarPreco(pedido.total);

        const status = document.createElement("p");
        status.textContent = "Status: Recebido";

        card.append(titulo, itens, total, status);
        lista.appendChild(card);
    });
}

/* ---------- EXPORTAR JSON (BLOB + DOWNLOAD) ---------- */

// Transforma qualquer objeto/lista em um arquivo .json e dispara o download no navegador.
function baixarJson(objeto, nomeArquivo) {
    const texto = JSON.stringify(objeto, null, 2);
    const blob = new Blob([texto], { type: "application/json" });
    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = nomeArquivo;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    URL.revokeObjectURL(url);
}

function iniciarExportacao() {
    const botao = document.querySelector("#exportar-json");
    if (!botao) return;

    botao.hidden = lerLista(CHAVE_PEDIDOS).length === 0;

    botao.addEventListener("click", function () {
        const pedidos = lerLista(CHAVE_PEDIDOS);
        if (pedidos.length === 0) return;

        baixarJson(pedidos, "mogus_pedidos.json");
    });
}

/* ---------- CONEXÃO COM O C# (FETCH) ---------- */

const URL_API = "http://localhost:5000/api";

// Envia um objeto como JSON (POST) para o aplicativo Windows Forms.
// Exemplo: enviarParaServidor("usuario", "cadastrar", { ... })
async function enviarParaServidor(entidade, acao, objeto, elementoStatus) {
    const rota = URL_API + "/" + entidade + "/" + acao;

    if (elementoStatus) {
        elementoStatus.textContent = "Enviando comando '" + acao + "' para o aplicativo Windows Forms...";
    }

    try {
        const resposta = await fetch(rota, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(objeto)
        });

        if (resposta.ok) {
            const resultado = await resposta.json();
            if (elementoStatus) {
                elementoStatus.textContent = "Sucesso! Resposta do C#: " + resultado.mensagem;
            }
            return resultado;
        }

        if (elementoStatus) {
            elementoStatus.textContent = "Erro no servidor C#. Status: " + resposta.status;
        }
    } catch (erro) {
        console.error(erro);
        if (elementoStatus) {
            elementoStatus.textContent = "Falha de conexão. Certifique-se de que o projeto Windows Forms está aberto e rodando (F5).";
        }
    }

    return null;
}

function iniciarCadastro() {
    const formulario = document.querySelector("#formCadastro");
    if (!formulario) return;

    const status = document.querySelector("#status");

    formulario.addEventListener("submit", function (evento) {
        evento.preventDefault();

        // Mesmos nomes de propriedades da classe Usuario.cs
        const usuario = {
            idUsuario: 0,
            nomeUsuario: formulario.querySelector("#inputNome").value,
            senhaUsuario: formulario.querySelector("#inputSenha").value,
            telefoneUsuario: formulario.querySelector("#inputTel").value,
            emailUsuario: formulario.querySelector("#inputEmail").value
        };

        enviarParaServidor("usuario", "cadastrar", usuario, status);
    });
}

/* ---------- TEMA ---------- */

const CHAVE_TEMA = "mogus_tema";

function aplicarTema(modo) {
    const escuro = modo === "escuro";
    document.body.classList.toggle("dark-mode", escuro);

    const botao = document.querySelector("#tema");
    if (botao) {
        botao.textContent = escuro ? "MODO CLARO" : "MODO ESCURO";
    }
}

function iniciarTema() {
    aplicarTema(localStorage.getItem(CHAVE_TEMA) || "claro");

    const botao = document.querySelector("#tema");
    if (!botao) return;

    botao.addEventListener("click", function () {
        const novo = document.body.classList.contains("dark-mode") ? "claro" : "escuro";
        localStorage.setItem(CHAVE_TEMA, novo);
        aplicarTema(novo);
    });
}

/* ---------- INICIALIZAÇÃO ---------- */

document.addEventListener("DOMContentLoaded", function () {
    iniciarTema();
    iniciarMenu();
    iniciarCarrinho();
    renderizarPedidos();
    iniciarExportacao();
    iniciarCadastro();

});
