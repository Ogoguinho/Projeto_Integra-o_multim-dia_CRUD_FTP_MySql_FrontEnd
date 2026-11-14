using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_c_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoginEntrar_Click(object sender, EventArgs e)
        {
            Form2 tela = new Form2();
            tela.ShowDialog();
        }


    }
}

/*using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_c_
{
    public partial class Form1 : Form // Substitua pelo nome do seu Form inicial, se necessário
    {
        private HttpListener _listener;
        private bool _isListening;

        public Form1()
        {
            InitializeComponent();

            // Inicia o servidor HTTP em background assim que a aplicação abre
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

                // Roda em uma thread separada para não congelar a interface do Windows Forms
                _ = Task.Run(async () =>
                {
                    while (_isListening)
                    {
                        try
                        {
                            var context = await _listener.GetContextAsync();
                            var request = context.Request;
                            var response = context.Response;

                            // Configurações de CORS para aceitar requisições locais do navegador
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
                                // Lê o JSON enviado pelo JavaScript no corpo da requisição
                                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                                var corpoJson = await reader.ReadToEndAsync();

                                // Converte o texto JSON em um objeto da classe Usuario
                                var usuario = JsonSerializer.Deserialize<Usuario>(corpoJson);
                                string mensagemRetorno = "";

                                if (usuario != null)
                                {
                                    // Identifica qual rota foi acionada e executa o método MySQL correspondente
                                    if (request.Url?.AbsolutePath == "/api/usuario/inserir")
                                    {
                                        usuario.InserirUsuario();
                                        mensagemRetorno = "Usuário inserido no MySQL com sucesso!";
                                    }
                                    else if (request.Url?.AbsolutePath == "/api/usuario/alterar")
                                    {
                                        usuario.AlterarUsuario();
                                        mensagemRetorno = "Usuário alterado no MySQL com sucesso!";
                                    }
                                    else if (request.Url?.AbsolutePath == "/api/usuario/deletar")
                                    {
                                        usuario.DeletarUsuario();
                                        mensagemRetorno = "Usuário deletado do MySQL com sucesso!";
                                    }

                                    // Atualiza a interface gráfica do Windows Forms de forma segura com Invoke
                                    this.Invoke(new Action(() =>
                                    {
                                        MessageBox.Show(mensagemRetorno, "Banco de Dados MySQL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }));
                                }

                                // Responde de volta para o JavaScript confirmando o sucesso
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
                            Console.WriteLine($"Erro no servidor HTTP: {ex.Message}");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao iniciar o servidor na porta 5000: {ex.Message}\nVerifique se a porta já está em uso.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Garante que o servidor será desligado corretamente ao fechar o app
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _isListening = false;
            _listener?.Stop();
            base.OnFormClosed(e);
        }
    }
}*/