using System;
using System.Data;
using MySqlConnector;

namespace ProjetoCRUD_Miguel_Diniz
{
    // Erro de conexão com o banco (o servidor HTTP responde 503)
    public class ErroConexao : Exception
    {
        public ErroConexao(string mensagem, Exception interno) : base(mensagem, interno) { }
    }

    internal class Conexao
    {
        //ola eu sou um classe

        // A string de conexão vem da variável de ambiente MOGUS_CONEXAO.
        // Sem a variável, usa o mesmo banco de antes: localhost, MogusDB, root, senha vazia.
        // Exemplo: Server=localhost;Database=MogusDB;Uid=root;Pwd=minhasenha;CharSet=utf8mb4
        public static string DadosConexao
        {
            get
            {
                string ambiente = Environment.GetEnvironmentVariable("MOGUS_CONEXAO");
                if (!string.IsNullOrWhiteSpace(ambiente)) return ambiente;
                return "Server=localhost;Port=3306;Database=MogusDB;Uid=root;Pwd=;CharSet=utf8mb4";
            }
        }

        // Cria um parâmetro; null vira NULL no banco
        public static MySqlParameter P(string nome, object valor)
        {
            return new MySqlParameter(nome, valor ?? DBNull.Value);
        }

        //Abre uma conexão. Quem chamar deve fechar (using). Serve também para transações.
        public MySqlConnection Abrir()
        {
            try//tente rodar o código
            {
                var conn = new MySqlConnection(DadosConexao);
                conn.Open();
                return conn;
            }
            catch (MySqlException excessao)//pegue a excessão
            {
                throw new ErroConexao("Conexão não estabelecida. " + excessao.Message, excessao);
            }
        }

        // Cria o comando com os parâmetros (e a transação, quando houver)
        public static MySqlCommand Criar(MySqlConnection conn, MySqlTransaction transacao, string sql, MySqlParameter[] parametros)
        {
            var comando = new MySqlCommand(sql, conn, transacao);
            if (parametros != null) comando.Parameters.AddRange(parametros);
            return comando;
        }

        //Metodo que executa instruções que não retornam valor: INSERT, UPDATE, DELETE
        //Os valores sempre vão em parâmetros (@nome), nunca concatenados no texto do SQL
        public int ExecutarComando(string sql, params MySqlParameter[] parametros)
        {
            using (var conn = Abrir())
            using (var comando = Criar(conn, null, sql, parametros))
            {
                return comando.ExecuteNonQuery();
            }
        }

        //INSERT que devolve o id gerado (AUTO_INCREMENT)
        public long Inserir(string sql, params MySqlParameter[] parametros)
        {
            using (var conn = Abrir())
            using (var comando = Criar(conn, null, sql, parametros))
            {
                comando.ExecuteNonQuery();
                return comando.LastInsertedId;
            }
        }

        //Devolve a primeira coluna da primeira linha (ou null)
        public object ExecutarEscalar(string sql, params MySqlParameter[] parametros)
        {
            using (var conn = Abrir())
            using (var comando = Criar(conn, null, sql, parametros))
            {
                object valor = comando.ExecuteScalar();
                return valor == DBNull.Value ? null : valor;
            }
        }

        //metodo que executa instrução select, o resultado vem em um DataTable
        public DataTable ExecutarConsulta(string query, params MySqlParameter[] parametros)
        {
            using (var conn = Abrir())
            using (var comando = Criar(conn, null, query, parametros))
            using (var leitor = comando.ExecuteReader())
            {
                var dt = new DataTable();
                dt.Load(leitor);
                return dt;
            }
        }
    }
}
