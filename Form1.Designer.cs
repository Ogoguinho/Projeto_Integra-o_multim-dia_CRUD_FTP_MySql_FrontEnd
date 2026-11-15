namespace Forms_c_
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLoginUsername = new System.Windows.Forms.TextBox();
            this.txtLoginUserPswd = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnLoginEntrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLoginUsername
            // 
            this.txtLoginUsername.Location = new System.Drawing.Point(220, 137);
            this.txtLoginUsername.Name = "txtLoginUsername";
            this.txtLoginUsername.Size = new System.Drawing.Size(356, 20);
            this.txtLoginUsername.TabIndex = 0;
            // 
            // txtLoginUserPswd
            // 
            this.txtLoginUserPswd.Location = new System.Drawing.Point(222, 215);
            this.txtLoginUserPswd.Name = "txtLoginUserPswd";
            this.txtLoginUserPswd.Size = new System.Drawing.Size(356, 20);
            this.txtLoginUserPswd.TabIndex = 1;
            // 
            // lblSenha
            // 
            this.lblSenha.AutoSize = true;
            this.lblSenha.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenha.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblSenha.Location = new System.Drawing.Point(273, 174);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(88, 29);
            this.lblSenha.TabIndex = 2;
            this.lblSenha.Text = "Senha:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUsername.Location = new System.Drawing.Point(273, 87);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(208, 29);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Nome de Usuário:";
            // 
            // btnLoginEntrar
            // 
            this.btnLoginEntrar.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnLoginEntrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoginEntrar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLoginEntrar.Location = new System.Drawing.Point(305, 336);
            this.btnLoginEntrar.Name = "btnLoginEntrar";
            this.btnLoginEntrar.Size = new System.Drawing.Size(200, 40);
            this.btnLoginEntrar.TabIndex = 4;
            this.btnLoginEntrar.Text = "Entar";
            this.btnLoginEntrar.UseVisualStyleBackColor = false;
            this.btnLoginEntrar.Click += new System.EventHandler(this.btnLoginEntrar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLoginEntrar);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblSenha);
            this.Controls.Add(this.txtLoginUserPswd);
            this.Controls.Add(this.txtLoginUsername);
            this.Name = "Form1";
            this.Text = "Login-Projeto_CRUD_Miguel-Freitas_Miguel-Diniz";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLoginUsername;
        private System.Windows.Forms.TextBox txtLoginUserPswd;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Button btnLoginEntrar;
    }
}

