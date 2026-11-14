using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ProjetoCRUD_Miguel_Diniz
{
    internal class Conexao
    {
        //ola eu sou um classe

        MySqlConnection conn;//Executa Insert Update e Delete,"é uma variavel da classe"

        public void conectarBD()
        {
            try//tente rodar o código
            {
                string dadosConexao = "Persist security info = false; server = localhost; database = MogusDB; uid = root; pwd=";
                //^Dados: Persistencia de segurança: 0 --> nova vez que o usuário logar as info. não vao continuar logados; acessa o nome do servidor; id de user: raiz do código, como esta descrito; senha vazia;

                conn = new MySqlConnection(dadosConexao);
                //Terminamos de instanciar  a classe com a string de parametro

                conn.Open();
                //Abrimos a conexão
            }

            catch (MySqlException excessao)//pegue a excessão
            {
                throw new Exception("Conexão não estabelecida." + excessao.Message);
                //Manda a mensagem ao usuário+ atributo padrão "Message" do mysql
            }
        }

        //Metodo que executa instruções que não retornam valor: INSERT, UPDATE, DELETE

        public void ExecutarCoamando(string sql)
        {
            conectarBD();//Inicializa a conexão

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, conn);//conn é o obj de conexão, "sql" é o comando em string
                comando.ExecuteNonQuery(); //Executa a instrução fornecida
            }
            catch (MySqlException excessao)
            {
                throw new Exception("Instrução não executada. " + $"\n{sql}\n" + excessao.Message);//Mesmo de antes
            }
            finally//Sempre será executado, com ou sem erro;
            {
                conn.Close();//fecha a conexão
            }
        }

        //metodo que executa instrução select

        public DataTable ExecutarConsulta(string query)
        {
            //"MySqlDataAdapter" -> Essa classe executa o select, esse resultado será exibido através de datatable

            conectarBD();

            try
            {
                DataTable dt = new DataTable();
                MySqlDataAdapter dados = new MySqlDataAdapter(query, conn);
                dados.Fill(dt);
                return dt;
            }

            catch (MySqlException ex)
            {
                throw new Exception($"Erro ao executar o comando SELECT: {ex.Message}");
            }
        }
    }
}
