using ProjetoCRUD_Miguel_Diniz;
using System;
using System.Data;

namespace Forms_c_
{
    public class Usuario
    {
        // Propriedades públicas que combinam exatamente com o JSON enviado pelo site
        public int idUsuario { get; set; }
        public string nomeUsuario { get; set; }
        public string senhaUsuario { get; set; }
        public DateTime dtnUsuario { get; set; }
        public string telefoneUsuario { get; set; }
        public string emailUsuario { get; set; }

        Conexao db = new Conexao();

        public void InserirUsuario()
        {
            // O cadastro do site não envia data de nascimento: nesse caso grava NULL
            string dataNascimento = dtnUsuario == default(DateTime) ? "NULL" : $"'{dtnUsuario:yyyy/MM/dd}'";
            string inserir = $"INSERT INTO Usuario VALUES(null, '{nomeUsuario}', '{senhaUsuario}', {dataNascimento}, '{telefoneUsuario}', '{emailUsuario}');";
            db.ExecutarCoamando(inserir);
        }

        public void AlterarUsuario()
        {
            string alterar = $"UPDATE Usuario SET NomeUsuario = '{nomeUsuario}', SenhaUsuario = '{senhaUsuario}', DataNascimentoUsuario = '{dtnUsuario:yyyy/MM/dd}', TelefoneUsuario = '{telefoneUsuario}', EmailUsuario = '{emailUsuario}' WHERE idUsuario = {idUsuario};";
            db.ExecutarCoamando(alterar);
        }

        public void DeletarUsuario()
        {
            string deletar = $"DELETE FROM Usuario WHERE idUsuario = {idUsuario};";
            db.ExecutarCoamando(deletar);
        }

        public DataTable ListarAlunos()
        {
            string Listar = $"SELECT * FROM MogusDB.Usuario;";
            return db.ExecutarConsulta(Listar);
        }
    }
}