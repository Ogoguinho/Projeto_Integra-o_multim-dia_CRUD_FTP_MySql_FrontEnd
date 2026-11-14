using ProjetoCRUD_Miguel_Diniz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms_c_
{
    internal class Usuario
    {
        private int idUsuario;

        public void set_usuarioID(int ID)
        {
            this.idUsuario = ID;
        }

        public int get_idUsuario()
        {
            return this.idUsuario;
        }
        private string nomeUsuario;

        public void set_usuarioNome(string nome)
        {
            this.nomeUsuario = nome;
        }

        public string get_nomeUsuario()
        {
            return this.nomeUsuario;
        }


        private string senhaUsuario;

        public void set_usuariosenha(string senha)
        {
            this.senhaUsuario = senha;
        }

        public string get_senhaUsuario()
        {
            return this.senhaUsuario;
        }

        private DateTime dtnUsuario;

        public void set_userDate(DateTime dataUsuario)
        {
            this.dtnUsuario = dataUsuario;
        }

        public DateTime get_userDate()
        {
            return this.dtnUsuario;
        }

        private string telefoneUsuario;

        public void set_usuariotel(string tel)
        {
            this.telefoneUsuario = tel;
        }

        public string get_etlsuario()
        {
            return this.telefoneUsuario;
        }

        private string emailUsuario;

        public void set_usuarioemail(string email)
        {
            this.emailUsuario = email;
        }

        public string get_emailUsuario()
        {
            return this.emailUsuario;
        }

        Conexao db = new Conexao();//Conectar

        public void InserirUsuario()
        {
            string inserir = $"INSERT INTO Usuario VALUES(null, '{nomeUsuario}', '{senhaUsuario}', '{dtnUsuario.ToString("yyyy/MM/dd")}', '{telefoneUsuario}', '{emailUsuario}');";
            db.ExecutarCoamando(inserir);
        }

        public void AlterarUsuario()
        {
            string alterar = $"update Usuario SET NomeUsuario = '{nomeUsuario}', SenhaUsuario = '{senhaUsuario}',  DataNascimentoUsuario = '{dtnUsuario.ToString("yyyy/MM/dd")}', TelefoneUsuario = '{telefoneUsuario}', EmailUsuario = '{emailUsuario}', where idUsuario = '{idUsuario}';";
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