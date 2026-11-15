using ProjetoCRUD_Miguel_Diniz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms_c_
{
    internal class Pedido
    {

        Conexao db = new Conexao();

        private int comidaID;

        public void set_comidaID(int comidaID)
        {
            this.comidaID = comidaID;
        }

        public int get_comidaID()
        {
            return this.comidaID;
        }

        private int pedidoid;

        public void set_pedidooID(int ID)
        {
            this.pedidoid = ID;
        }

        public int get_idUsuario()
        {
            return this.pedidoid;
        }

        private int quantidadeItem;

        public void set_quantidadeItem(int quantidade)
        {
            this.quantidadeItem = quantidade;
        }

        public int get_quantidade()
        {
            return this.quantidadeItem;
        }

        public DataTable ListarPedidos()
        {
            string Listar = $"SELECT * FROM MogusDB.Pedido;";
            return db.ExecutarConsulta(Listar);
        }

        public void AlterarPedido()
        {
            string alterar = $"UPDATE Pedido SET comidaID = {comidaID}, quantidadeItem = {quantidadeItem} WHERE pedidoid = {pedidoid};";
            db.ExecutarCoamando(alterar);
        }

        public void DeletarPedido()
        {
            string deletar = $"DELETE FROM Pedido WHERE pedidoid = {pedidoid};";
            db.ExecutarCoamando(deletar);
        }
    }
}
