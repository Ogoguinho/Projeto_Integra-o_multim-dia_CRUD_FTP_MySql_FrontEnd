using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_c_
{
    public partial class Form1 : Form
    {
        private HttpListener _listener;
        private bool _isListening;

        public Form1()
        {
            InitializeComponent();
            IniciarServidorHttp();
        }

        private void IniciarServidorHttp()
        {
            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add("http://localhost:5000/");
                _listener.Start();
                _isListening = true;

                _ = Task.Run(async () =>
                {
                    while (_isListening)
                    {
                        try
                        {
                            var context = await _listener.GetContextAsync();
                            var request = context.Request;
                            var response = context.Response;

                            response.AddHeader("Access-Control-Allow-Origin", "*");
                            response.AddHeader("Access-Control-Allow-Methods", "POST, OPTIONS");
                            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

                            if (request.HttpMethod == "OPTIONS")
                            {
                                response.StatusCode = (int)HttpStatusCode.OK;
                                response.OutputStream.Close();
                                continue;
                            }

                            if (request.HttpMethod == "POST")
                            {
                                string corpoJson = "";
                                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                                {
                                    corpoJson = await reader.ReadToEndAsync();
                                }

                                string mensagemRetorno = "";
                                bool erroNoBanco = false;
                                var path = request.Url?.AbsolutePath ?? "";

                                try
                                {
                                    // --- ROTAS DE USUÁRIO ---
                                    if (path.StartsWith("/api/usuario/"))
                                    {
                                        var usuario = JsonSerializer.Deserialize<Usuario>(corpoJson);
                                        if (usuario != null)
                                        {
                                            if (path == "/api/usuario/cadastrar") { usuario.InserirUsuario(); mensagemRetorno = "Usuário cadastrado com sucesso!"; }
                                            else if (path == "/api/usuario/alterar") { usuario.AlterarUsuario(); mensagemRetorno = "Usuário alterado com sucesso!"; }
                                            else if (path == "/api/usuario/deletar") { usuario.DeletarUsuario(); mensagemRetorno = "Usuário deletado com sucesso!"; }
                                        }
                                    }
                                    // --- ROTAS DE PEDIDO ---
                                    else if (path.StartsWith("/api/pedido/"))
                                    {
                                        var pedido = JsonSerializer.Deserialize<Pedido>(corpoJson);
                                        if (pedido != null)
                                        {
                                            if (path == "/api/pedido/alterar") { pedido.AlterarPedido(); mensagemRetorno = "Pedido alterado com sucesso!"; }
                                            else if (path == "/api/pedido/deletar") { pedido.DeletarPedido(); mensagemRetorno = "Pedido deletado com sucesso!"; }
                                        }
                                    }
                                }
                                catch (Exception exBanco)
                                {
                                    // Responde com erro para o fetch não ficar esperando para sempre
                                    erroNoBanco = true;
                                    mensagemRetorno = "Erro ao executar a operação: " + exBanco.Message;
                                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                }

                                if (!string.IsNullOrEmpty(mensagemRetorno) && !erroNoBanco)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        MessageBox.Show(mensagemRetorno, "Banco de Dados MySQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }));
                                }

                                var respostaObj = new { mensagem = mensagemRetorno };
                                var buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(respostaObj));

                                response.ContentType = "application/json";
                                response.ContentLength64 = buffer.Length;
                                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                response.StatusCode = (int)HttpStatusCode.NotFound;
                            }

                            response.OutputStream.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro: {ex.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o servidor na porta 5000: {ex.Message}");
            }
        }

        private void btnLoginEntrar_Click(object sender, EventArgs e)
        {
            FormCRUD tela = new FormCRUD();
            tela.ShowDialog();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _isListening = false;
            _listener?.Stop();
            base.OnFormClosed(e);
        }
    }
}