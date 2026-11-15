namespace Forms_c_
{
    partial class FormCRUD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bTUsusarios = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bTUsusarios
            // 
            this.bTUsusarios.BackColor = System.Drawing.Color.Navy;
            this.bTUsusarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bTUsusarios.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bTUsusarios.Location = new System.Drawing.Point(12, 12);
            this.bTUsusarios.Name = "bTUsusarios";
            this.bTUsusarios.Size = new System.Drawing.Size(387, 426);
            this.bTUsusarios.TabIndex = 1;
            this.bTUsusarios.Text = "ÁREA DE USUÁRIO";
            this.bTUsusarios.UseVisualStyleBackColor = false;
            this.bTUsusarios.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(405, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(387, 426);
            this.button1.TabIndex = 2;
            this.button1.Text = "ÁREA DE PEDIDOS";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormCRUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bTUsusarios);
            this.Name = "FormCRUD";
            this.Text = "FormCRUD";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bTUsusarios;
        private System.Windows.Forms.Button button1;
    }
}