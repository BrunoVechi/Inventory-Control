namespace Spareparts
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.QrcodeBox = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Gerar = new System.Windows.Forms.Button();
            this.YgQRCODE = new System.Windows.Forms.TextBox();
            this.Largura = new System.Windows.Forms.TextBox();
            this.Altura = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.QrcodeBox)).BeginInit();
            this.SuspendLayout();
            // 
            // QrcodeBox
            // 
            this.QrcodeBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.QrcodeBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.QrcodeBox.Location = new System.Drawing.Point(12, 48);
            this.QrcodeBox.Name = "QrcodeBox";
            this.QrcodeBox.Size = new System.Drawing.Size(200, 200);
            this.QrcodeBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.QrcodeBox.TabIndex = 0;
            this.QrcodeBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(218, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "YG DO MATERIAL";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(221, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 25);
            this.label9.TabIndex = 7;
            this.label9.Text = "ALTURA";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Agency FB", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(381, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "LARGURA";
            // 
            // Gerar
            // 
            this.Gerar.Font = new System.Drawing.Font("Agency FB", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Gerar.Location = new System.Drawing.Point(275, 215);
            this.Gerar.Name = "Gerar";
            this.Gerar.Size = new System.Drawing.Size(207, 33);
            this.Gerar.TabIndex = 32;
            this.Gerar.Text = "GERAR QR";
            this.Gerar.UseVisualStyleBackColor = true;
            this.Gerar.Click += new System.EventHandler(this.Gerar_Click);
            // 
            // YgQRCODE
            // 
            this.YgQRCODE.BackColor = System.Drawing.SystemColors.Window;
            this.YgQRCODE.Location = new System.Drawing.Point(331, 53);
            this.YgQRCODE.Name = "YgQRCODE";
            this.YgQRCODE.Size = new System.Drawing.Size(226, 20);
            this.YgQRCODE.TabIndex = 4;
            this.YgQRCODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Largura
            // 
            this.Largura.BackColor = System.Drawing.SystemColors.Window;
            this.Largura.Location = new System.Drawing.Point(468, 91);
            this.Largura.Name = "Largura";
            this.Largura.Size = new System.Drawing.Size(88, 20);
            this.Largura.TabIndex = 8;
            this.Largura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Altura
            // 
            this.Altura.BackColor = System.Drawing.SystemColors.Window;
            this.Altura.Location = new System.Drawing.Point(287, 91);
            this.Altura.Name = "Altura";
            this.Altura.Size = new System.Drawing.Size(88, 20);
            this.Altura.TabIndex = 33;
            this.Altura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = global::ALMOXARIFA.Properties.Resources.fundo_qr;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(568, 293);
            this.Controls.Add(this.Altura);
            this.Controls.Add(this.Gerar);
            this.Controls.Add(this.Largura);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.YgQRCODE);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.QrcodeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QR Code Generator v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.QrcodeBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox QrcodeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Gerar;
        private System.Windows.Forms.TextBox YgQRCODE;
        private System.Windows.Forms.TextBox Largura;
        private System.Windows.Forms.TextBox Altura;
    }
}