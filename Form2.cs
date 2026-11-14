using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms_c_
{
    public partial class Form2 : Form
    {
        Usuario usuario = new Usuario();

        

        public Form2()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        public void Limpar()
        {
            txtEmail.Text = string.Empty;
            txtID.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtTelefone.Text = string.Empty;

            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            usuario.AlterarUsuario();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataGridView1.Rows[e.RowIndex].Cells["idUsuario"].Value.ToString();
            txtNome.Text = dataGridView1.Rows[e.RowIndex].Cells["nomeUsuario"].Value.ToString();
            dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells["Data_Nascimneto"].Value.ToString());
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["emailUsuario"].Value.ToString();
            txtTelefone.Text = dataGridView1.Rows[e.RowIndex].Cells["telefoneUsuario"].Value.ToString();
            txtSenha.Text = dataGridView1.Rows[e.RowIndex].Cells["senhaUsuario"].Value.ToString();
        }
    }
}
