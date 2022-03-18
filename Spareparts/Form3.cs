using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Spareparts
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        //METODO GERAR QR
         public Bitmap GerarQRCode(int width, int height, string text)
        {
            try
            {
                var bw = new ZXing.BarcodeWriter();
                var encOptions = new ZXing.Common.EncodingOptions() { Width = width, Height = height, Margin = 0 };
                bw.Options = encOptions;
                bw.Format = ZXing.BarcodeFormat.QR_CODE;
                var resultado = new Bitmap(bw.Write(text));
                return resultado;
            }
            catch
            {
                throw;
            }
        }

        private void Gerar_Click(object sender, EventArgs e)
        {
            if (Largura.Text == string.Empty || label9.Text == string.Empty || YgQRCODE.Text == string.Empty)
            {
                MessageBox.Show("Informações inválidas. Complete as informações para gerar o QRCode...");
                YgQRCODE.Focus();
                return;
            }
            try
            {
                int largura = Convert.ToInt32(Largura.Text);
                int altura = Convert.ToInt32(Altura.Text);
                QrcodeBox.Image = GerarQRCode(largura, altura, YgQRCODE.Text);

                //SALVA IMAGEM EM UM DIRETORIO
                SaveFileDialog saveFile = new SaveFileDialog();               
                saveFile.Filter = "Imagem  Arquivos(*.Jpg, *.Png, *.Tiff, *.Bmp, *.Gif) | * .jpg; *.Png; *.Tiff; *.Bmp; *.Gif ";
                saveFile.InitialDirectory = @"C:\\";
                saveFile.Title = "Salvar QR Code";
                saveFile.AddExtension = true;
                saveFile.DefaultExt = "bmp";
                saveFile.FilterIndex = 2;
                saveFile.RestoreDirectory = true;

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string fName = saveFile.FileName;
                    QrcodeBox.Image.Save(fName,ImageFormat.Bmp);                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
