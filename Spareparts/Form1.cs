using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Spareparts
{
    public partial class Inicial : Form
    {   
        //DECLARA CONEXAO COM EXCEL      
        OleDbConnection _olecon;
        OleDbCommand _oleCmd;
        static String _Arquivo = @"C:\Kanban System 4.0\BD.Excel.xls";
        String _StringConexao = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;ReadOnly=False';", _Arquivo);
        String _Write;
        public static bool beepAuto = false;
        public static bool Gravando = false;      
        public static bool Refreshh = false;
        public static bool requisicao = false;
        new bool Closing = false;
        bool Empresti = false;
        bool have = false;
        bool found = false;
        public static int cycles = 0;
        int cycles2 = 0;            

        // INICIA LISTA DE CLASSE "PEÇA"
        public static List<Peça> Peças = new List<Peça>();

        //INICIA LISTA  DE CLASSE "HISTORICO" 
        public List<Historico> History = new List<Historico>();

        //INICIA LISTA  DE CLASSE "HISTORICO" 
        public List<Historico> HistoryGrid = new List<Historico>();

        //INICIA LISTA  DE CLASSE "REQUISITAR" 
        public static List<Requisitar> Requisita = new List<Requisitar>();

        //INICIA LISTA  DE CLASSE "EMPRESTIMO" 
        public List<Emprestimo> Emprestimos = new List<Emprestimo>();

        // METODO PARA EXIBIR PEÇA NOS BOXs
        public void Exibir(Peça x)
        {
            boxYG.Text = Convert.ToString(x.yg);
            boxDescricao.Text = Convert.ToString(x.Descricao);
            boxLocal.Text = Convert.ToString(x.local);
            boxMax.Text = Convert.ToString(x.max);
            boxMin.Text = Convert.ToString(x.min);
            boxAtual.Text = Convert.ToString(x.atual);
        }

        //METODO LER PLANILHA E GRAVAR NA LISTA
        void LoadExcel()
        {
            //carrega master list      
            _oleCmd.CommandText = "SELECT * FROM [MasterList$]";
            OleDbDataReader reader = _oleCmd.ExecuteReader();

            try
            {               
                while (reader.Read())
                {
                    Peça nova = new Peça(reader.GetValue(0).ToString())
                    {
                        Descricao = reader.GetValue(1).ToString(),
                        local = reader.GetValue(2).ToString(),
                        max = Convert.ToInt32(reader.GetValue(3)),
                        min = Convert.ToInt32(reader.GetValue(4)),
                        atual = Convert.ToInt32(reader.GetValue(5)),
                        Utl = Convert.ToInt32(reader.GetValue(6)),
                        Ext = Convert.ToInt32(reader.GetValue(7)),
                    };
                    Peças.Add(nova);
                }

                reader.Close();
            }
            catch(Exception e )
            {    reader.Close();
                 MessageBox.Show("Error to load BD.Excel : Masterlist " + e.Message); 
            }
            //carrega kanbanlist
            _oleCmd.CommandText = "SELECT * FROM [Requisitar$]";
            reader = _oleCmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Requisitar Novo = new Requisitar(reader.GetValue(0).ToString())
                    {                       
                        YG = reader.GetValue(1).ToString(),
                        Descricao = reader.GetValue(2).ToString(),
                        local = reader.GetValue(2).ToString(),
                        Min = Convert.ToInt32(reader.GetValue(4)),
                        Atual = Convert.ToInt32(reader.GetValue(5)),
                        ConsumoUTL = Convert.ToInt32(reader.GetValue(6)),
                        ConsumoEXT = Convert.ToInt32(reader.GetValue(7)),
                        Compras = reader.GetValue(8).ToString(),
                    };
                    Requisita.Add(Novo);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                reader.Close();
                MessageBox.Show("Error to load BD.Excel : Kanbanlist " + e.Message);
            }
        }
    //--------------------------------------------------
        //METODO GRAVAR DADOS MASTERLIST
        void WriteExcel()
        {
                    
            foreach (Peça x in Peças)
            {    
                _Write = "INSERT INTO [MasterList$] ";
                _Write += "([YG],[DESCRICAO],[LOCAL],[MAX],[MIN],[ATUAL],[UTL],[EXT]) ";
                _Write += "VALUES ";
                _Write += "(@yg,@descricao,@local,@max,@min,@atual,@utl,@ext)";

                _oleCmd.CommandText = _Write;
                _oleCmd.Parameters.Add("@yg", OleDbType.VarChar, 255).Value = x.yg;
                _oleCmd.Parameters.Add("@descricao", OleDbType.VarChar, 255).Value = x.Descricao;
                _oleCmd.Parameters.Add("@local", OleDbType.VarChar, 255).Value = x.local;
                _oleCmd.Parameters.Add("@max", OleDbType.Integer).Value = x.max;
                _oleCmd.Parameters.Add("@min", OleDbType.Integer).Value = x.min;
                _oleCmd.Parameters.Add("@atual", OleDbType.Integer).Value = x.atual;
                _oleCmd.Parameters.Add("@utl", OleDbType.Integer).Value = x.Utl;
                _oleCmd.Parameters.Add("@ext", OleDbType.Integer).Value = x.Ext;

                _oleCmd.ExecuteNonQuery();
                _oleCmd.Parameters.Clear();
            }
            _olecon.Close();
        }

        //METODO DELETAR DADOS MASTERLIST
        void DeleteExcel()
        {
            Microsoft.Office.Interop.Excel.Application _app = new Microsoft.Office.Interop.Excel.Application();
            Workbooks _books = null;
            Workbook _book = null;
            Sheets _sheets = null;
            Worksheet _sheet = null;

            //Abre o arquivo Excel
            if (_book == null)
            {
                _books = _app.Workbooks;
                _book = _books.Open(_Arquivo, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _sheets = _book.Worksheets;
            }
            foreach (Peça x in Peças)
            {
                //Pega a primeira Aba
                _sheet = (Worksheet)_sheets[1];
                _sheet.Select(Type.Missing);
                //Identifica qual o range que você deseja trabalhar
                Range range = _sheet.get_Range("A2:H2", Type.Missing);

                //Apaga a informação
                range.Delete(XlDeleteShiftDirection.xlShiftUp);


                //Limpa os objetos internos
                if (range != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                if (_sheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_sheet);
            }

            //salva o arquivo
            _book.Save();
            //Fecha o arquivo
            _book.Close(false, Type.Missing, Type.Missing);
            if (_book != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_book);
            }
            _app.Quit();
            if (_app != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_app);
            }

            _books.Close();

        }

    //-------------------------------------------------

        //METODO GRAVAR LISTA DE REQUISICAO
        void WriteExcelRequisitar()
        {

            foreach (Requisitar x in Requisita)
            {
                _Write = "INSERT INTO [Requisitar$] ";
                _Write += "([DATA],[YG],[DESCRICAO],[LOCAL],[MIN],[ATUAL],[CONSUMO UTL],[CONSUMO EXT],[EM COMPRAS]) ";
                _Write += "VALUES ";
                _Write += "(@data,@yg,@descricao,@local,@min,@atual,@consumoutl,@consumoext,@compras)";

                _oleCmd.CommandText = _Write;
                _oleCmd.Parameters.Add("@data", OleDbType.VarChar, 255).Value = x.Data;
                _oleCmd.Parameters.Add("@yg", OleDbType.VarChar, 255).Value = x.YG;
                _oleCmd.Parameters.Add("@descricao", OleDbType.VarChar, 255).Value = x.Descricao;
                _oleCmd.Parameters.Add("@local", OleDbType.VarChar, 255).Value = x.local;
                _oleCmd.Parameters.Add("@min", OleDbType.Integer).Value = x.Min;
                _oleCmd.Parameters.Add("@atual", OleDbType.Integer).Value = x.Atual;
                _oleCmd.Parameters.Add("@consumoutl", OleDbType.Integer).Value = x.ConsumoUTL;
                _oleCmd.Parameters.Add("@consumoext", OleDbType.Integer).Value = x.ConsumoEXT;
                _oleCmd.Parameters.Add("@compras", OleDbType.VarChar, 255).Value = x.Compras;

                _oleCmd.ExecuteNonQuery();
                _oleCmd.Parameters.Clear();
            }
            _olecon.Close();

        }

        //METODO DELETAR LISTA REQUISICOES
        public static void DeleteExcelRequisita()
        {
            Microsoft.Office.Interop.Excel.Application _app2 = new Microsoft.Office.Interop.Excel.Application();
            Workbooks _books2 = null;
            Workbook _book2 = null;
            Sheets _sheets2 = null;
            Worksheet _sheet2 = null;

            //Abre o arquivo Excel
            if (_book2 == null)
            {
                _books2 = _app2.Workbooks;
                _book2 = _books2.Open(_Arquivo, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _sheets2 = _book2.Worksheets;
            }
           
            foreach (Requisitar x in Requisita) 
            {
                //Pega a Aba
                _sheet2 = (Worksheet)_sheets2[4];
                _sheet2.Select(Type.Missing);
                //Identifica qual o range que você deseja trabalhar
                Range range = _sheet2.get_Range("A2:I2", Type.Missing);

                //Apaga a informação
                range.Delete(XlDeleteShiftDirection.xlShiftUp);


                //Limpa os objetos internos
                if (range != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                if (_sheet2 != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_sheet2);
            }
                       
            //salva o arquivo
            _book2.Save();
            //Fecha o arquivo
            _book2.Close(false, Type.Missing, Type.Missing);
            if (_book2 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_book2);
            }
            _app2.Quit();
            if (_app2 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_app2);
            }

            _books2.Close();

        }

    //------------------------------------------------

        //METODO GRAVAR LISTA NO HISTORICO
        void WriteExcelHistory()
        {

            foreach (Historico x in History)
            {
                _Write = "INSERT INTO [Historico$] ";
                _Write += "([DATA],[YG],[DESCRICAO],[LOCAL],[CONSUMO],[ATUAL],[MIN]) ";
                _Write += "VALUES ";
                _Write += "(@data,@yg,@descricao,@local,@consumo,@atual,@min)";

                _oleCmd.CommandText = _Write;
                _oleCmd.Parameters.Add("@data", OleDbType.VarChar, 255).Value = x.Data;
                _oleCmd.Parameters.Add("@yg", OleDbType.VarChar, 255).Value = x.YG;
                _oleCmd.Parameters.Add("@descricao", OleDbType.VarChar, 255).Value = x.Descricao;
                _oleCmd.Parameters.Add("@local", OleDbType.VarChar, 255).Value = x.local;
                _oleCmd.Parameters.Add("@consumo", OleDbType.VarChar, 255).Value = x.consumo;
                _oleCmd.Parameters.Add("@atual", OleDbType.Integer).Value = x.Atual;
                _oleCmd.Parameters.Add("@min", OleDbType.Integer).Value = x.min;              
               
                _oleCmd.ExecuteNonQuery();
                _oleCmd.Parameters.Clear();
            }
            _olecon.Close();
            History.Clear();           
        }     

        //METODO GRAVAR LISTA DE EMPRESTIMO
        void WriteExcelEmprestimo()
        {

            foreach (Emprestimo x in Emprestimos)
            {
                _Write = "INSERT INTO [Emprestimo$] ";
                _Write += "([DATA],[YG],[DESCRICAO],[LOCAL],[CONSUMO],[ATUAL],[MIN],[CENTRO DE CUSTO]) ";
                _Write += "VALUES ";
                _Write += "(@data,@yg,@descricao,@local,@consumo,@atual,@min,@centrocusto)";

                _oleCmd.CommandText = _Write;
                _oleCmd.Parameters.Add("@data", OleDbType.VarChar, 255).Value = x.data;
                _oleCmd.Parameters.Add("@yg", OleDbType.VarChar, 255).Value = x.yg;
                _oleCmd.Parameters.Add("@descricao", OleDbType.VarChar, 255).Value = x.Descricao;
                _oleCmd.Parameters.Add("@local", OleDbType.VarChar, 255).Value = x.local;
                _oleCmd.Parameters.Add("@consumo", OleDbType.Integer).Value = x.consumo;
                _oleCmd.Parameters.Add("@atual", OleDbType.Integer).Value = x.atual;
                _oleCmd.Parameters.Add("@min", OleDbType.Integer).Value = x.min;
                _oleCmd.Parameters.Add("@centrocusto", OleDbType.Integer).Value = x.CentroCusto;

                _oleCmd.ExecuteNonQuery();
                _oleCmd.Parameters.Clear();
            }
            _olecon.Close();
            Emprestimos.Clear();
        }   
    
        // METODO AUTOMATICO
        void AutoMode()
        {                  
            //METODO PARA GRAVAR NO TEXTBOX A PARTIR DE OUTRO THREAD
            void YGBox(string value)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(YGBox), new object[] { value });
                    return;
                }
                boxYG.Text = value;
            }

            while (true)
            {             
                //CASO YG CORRETO         
                if (checkAuto.CheckState == CheckState.Checked)
                {
                    foreach (Peça x in Peças)
                    {
                        if (x.yg == boxYG.Text && Closing == false)
                        {
                            cycles = 0;
                            
                            //SUBTRAI 1 NO ATUAL
                            x.atual -= 1;
                            x.Utl -= 1;
                            YGBox("");
                            beepAuto = true;

                            //ADICIONA ITEM A LISTA DO HISTORICO
                            DateTime date = DateTime.Now;
                            string DATE = date.ToString();

                            Historico novo = new Historico(DATE)
                            {
                                YG = x.yg,
                                Descricao = x.Descricao,
                                local = x.local,
                                consumo = "UTL",
                                Atual = x.atual,
                                min = x.min,
                            };
                            History.Add(novo);
                            RefreshGrid(novo);
                           
                            // SE ITEM ESTIVER NA QUANTIA MINIMA
                            if (x.atual <= x.min)
                            {
                                int Indice = 0;
                                int indice = 0;
                                bool remove = false;

                                foreach (Requisitar y in Requisita)
                                {
                                    have = false;
                                    //SE ITEM EXISTIR NA LISTA DE REQUISITA
                                    if (x.yg == y.YG)
                                    {
                                        have = true;
                                        // SE ITEM ESTIVER NA QUANTIA MINIMA
                                        if (x.atual <= x.min)
                                        {
                                            y.local = x.local;
                                            y.Descricao = x.Descricao;
                                            y.Min = x.min;
                                            y.Atual = x.atual;
                                            y.ConsumoUTL = x.Utl;
                                            y.ConsumoEXT = x.Ext;
                                        }

                                        if (x.atual > x.min)
                                        {
                                            indice = Indice;
                                            remove = true;
                                        }
                                    }

                                    Indice += 1;
                                }
                                //SE NESCESSARIO RETIRAR ITEM DA LISTA DE REQUISICOES
                                if (remove)
                                {
                                    Requisita.RemoveAt(indice);
                                }

                                //se item nao existir na lista
                                if (have == false)
                                {
                                    Requisitar nova = new Requisitar(DATE)
                                    {
                                        YG = x.yg,
                                        Descricao = x.Descricao,
                                        local = x.local,
                                        Atual = x.atual,
                                        Min = x.min,
                                        ConsumoUTL = x.Utl,
                                        ConsumoEXT = x.Ext,
                                    };

                                    Requisita.Add(nova);
                                }

                                requisicao = true;
                            }
                        }                      
                    }

                    //CASO YG ERRADO
                    cycles2 += 1;
                    if (cycles2 >= 4 && Closing == false)
                    {
                        YGBox("");
                        cycles2 = 0;
                    }
                }

                //CASO YG CORRETO - MODO EMPRESTIMO         
                if (CheckEmprest.CheckState == CheckState.Checked)
                {
                    foreach (Peça x in Peças)
                    {                      
                        if (x.yg == boxYG.Text && Closing == false)
                        {
                            cycles = 0; 

                            //SUBTRAI 1 NO ATUAL
                            x.atual -= 1;
                            x.Ext -= 1;
                            YGBox("");
                            beepAuto = true;

                            DateTime date = DateTime.Now;
                            string DATE = date.ToString();

                            //ADICIONA ITEM A LISTA DE EMPRESTIMO                          
                          try
                          {
                                Emprestimo Novo = new Emprestimo(DATE)
                                {
                                    yg = x.yg,
                                    Descricao = x.Descricao,
                                    local = x.local,
                                    consumo = -1,
                                    atual = x.atual,
                                    min = x.min,
                                    CentroCusto = Convert.ToInt32(boxCentroCusto.Text),
                                };
                                Emprestimos.Add(Novo);
                                Empresti = true;                            

                            //ADICIONA ITEM A LISTA DO HISTORICO                        
                            Historico novo = new Historico(DATE)
                            {
                                YG = x.yg,
                                Descricao = x.Descricao,
                                local = x.local,
                                consumo = "EXT",
                                Atual = x.atual,
                                min = x.min,
                            };
                            History.Add(novo);
                            RefreshGrid(novo);

                            }
                            catch (Exception)
                          {
                             x.atual += 1;
                             MessageBox.Show("OBRIGATORIO: Centro de Custo!","ERRO AO GRAVAR");
                          }

                            // SE ITEM ESTIVER NA QUANTIA MINIMA
                            if (x.atual <= x.min)
                            {
                                int Indice = 0;
                                int indice = 0;
                                bool remove = false;

                                foreach (Requisitar y in Requisita)
                                {
                                    have = false;
                                    //SE ITEM EXISTIR NA LISTA DE REQUISITA
                                    if (x.yg == y.YG)
                                    {
                                        have = true;
                                        // SE ITEM ESTIVER NA QUANTIA MINIMA
                                        if (x.atual <= x.min)
                                        {
                                            y.local = x.local;
                                            y.Descricao = x.Descricao;
                                            y.Min = x.min;
                                            y.Atual = x.atual;
                                            y.ConsumoUTL = x.Utl;
                                            y.ConsumoEXT = x.Ext;
                                        }

                                        if (x.atual > x.min)
                                        {
                                            indice = Indice;
                                            remove = true;
                                        }
                                    }

                                    Indice += 1;
                                }
                                //SE NESCESSARIO RETIRAR ITEM DA LISTA DE REQUISICOES
                                if (remove)
                                {
                                    Requisita.RemoveAt(indice);
                                }

                                //se item nao existir na lista
                                if (have == false)
                                {
                                    Requisitar nova = new Requisitar(DATE)
                                    {
                                        YG = x.yg,
                                        Descricao = x.Descricao,
                                        local = x.local,
                                        Atual = x.atual,
                                        Min = x.min,
                                        ConsumoUTL = x.Utl,
                                        ConsumoEXT = x.Ext,
                                    };

                                    Requisita.Add(nova);
                                }

                                requisicao = true;
                            }
                        }    
                    }
                   
                    //CASO YG ERRADO - MODO EMPRESTIMO
                    cycles2 += 1;
                    if (cycles2 >= 4 && Closing == false)
                    {
                        YGBox("");
                        cycles2 = 0;
                    }
                }

                //CONTA UM TEMPO ANTES DE ATUALIZAR BD.EXCEL
                if (beepAuto || Refreshh || requisicao)
                {
                    cycles += 1;
                }
                
                Thread.Sleep(200);            
                                
            }
        }

        //METODO ATUALIZAR BD.EXCEL NEW TASK=>
        void AtualizaAuto()
        {
            {  // TAREFA ASYNC
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {                           
                        //ATUALIZA BD.EXCEL NO MODO AUTO
                        if (cycles >= 150 && Gravando == false && (beepAuto || Refreshh || requisicao))
                        {
                            Gravando = true;
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;                         
                            DeleteExcel();
                            _olecon.Open();
                            WriteExcel();
                            beepAuto = false;
                            //CASO NAO SEJA NENHUMA ATUALIZACAO DE ITEM
                            if (Refreshh == false)
                            {
                                _olecon.Open();
                                WriteExcelHistory();                               
                            }
                            //CASO ITEM ATINJA QUANTIA MINIMA
                            if (requisicao)
                            {
                                DeleteExcelRequisita();
                                _olecon.Open();
                                WriteExcelRequisitar();
                                requisicao = false;
                            }
                            //CASO ITEM SEJA PARA OUTRO SETOR
                            if (Empresti)
                            {
                                _olecon.Open();
                                WriteExcelEmprestimo();
                                Empresti = false;
                            }
                            Gravando = false;
                            Refreshh = false;
                            cycles = 0;
                            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                        }                                                       
                        
                        Thread.Sleep(1000);
                    }
                });

            }
        }

        //METODO ATUALIZA DATAGRID
        void RefreshGrid(Historico x)
        {
            HistoryGrid.Reverse();
            HistoryGrid.Add(x);
            if (HistoryGrid.Count >= 10)
            {
                HistoryGrid.RemoveAt(0);
            }
            HistoryGrid.Reverse();          
           
            var list = new BindingList<Historico>(HistoryGrid);
            Grid1.Invoke((System.Action)(() => Grid1.DataSource = list));
        }

        //======== MAIN ========
        public Inicial()
        {
            //INICIALIZA CONEXAO COM PLANILHA      
            try
            {
                _olecon = new OleDbConnection(_StringConexao);
                _olecon.Open();
                _oleCmd = new OleDbCommand();
                _oleCmd.Connection = _olecon;
                _oleCmd.CommandType = CommandType.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to Connect to BD.Excel: " + ex.Message);
            }
            //CARREGA LISTA DE PEÇAS COM PLANILHA DO EXCEL
            LoadExcel();
            _olecon.Close();

            //INICIALIZA JANELA 
            InitializeComponent();

            //THREAD PARA MODO AUTOMATICO
            Thread Auto = new Thread(AutoMode);
            Auto.Start();             

            //ATUALIZACAO DE BD.EXCEL AUTO
            AtualizaAuto();
        }
    
         //SELEÇOES DOS CHECKBOXs  
        private void checkAuto_CheckedChanged(object sender, EventArgs e)
        {
          
            CheckEmprest.CheckState = CheckState.Unchecked;
            checkManual.CheckState = CheckState.Unchecked;          

            boxYG.Clear();
            boxDescricao.Clear();
            boxLocal.Clear();
            boxMax.Clear();
            boxMin.Clear();
            boxAtual.Clear();
            boxCentroCusto.Clear();         

            boxYG.Select();
            boxDescricao.Visible = false;
            boxLocal.Visible = false;
            boxMax.Visible = false;
            boxMin.Visible = false;
            boxAtual.Visible = false;
            label3.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;         
            boxCentroCusto.Visible = false;
            label9.Visible = false;
            AtualizarButton.Visible = false;
            CadastrarButton.Visible = false;         
            Grid1.Visible = true;
            label10.Visible = true;            

        }
        private void CheckEmprest_CheckedChanged(object sender, EventArgs e)
        {
            checkAuto.CheckState = CheckState.Unchecked;
            checkManual.CheckState = CheckState.Unchecked;           

            boxYG.Clear();
            boxDescricao.Clear();
            boxLocal.Clear();
            boxMax.Clear();
            boxMin.Clear();
            boxAtual.Clear();
            boxCentroCusto.Clear();          

            boxYG.Select();
            boxDescricao.Visible = false;
            boxLocal.Visible = false;
            boxMax.Visible = false;
            boxMin.Visible = false;
            boxAtual.Visible = false;
            label3.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;           
            boxCentroCusto.Visible = true;
            label9.Visible = true;
            AtualizarButton.Visible = false;
            CadastrarButton.Visible = false;       
            Grid1.Visible = true;
            label10.Visible = true;
        }
        private void CheckManual_CheckedChanged(object sender, EventArgs e)
        {
            CheckEmprest.CheckState = CheckState.Unchecked;
            checkAuto.CheckState = CheckState.Unchecked;   

            boxYG.Clear();
            boxDescricao.Clear();
            boxLocal.Clear();
            boxMax.Clear();
            boxMin.Clear();
            boxAtual.Clear();
            boxCentroCusto.Clear();         

            boxYG.Select();
            boxDescricao.Visible = true;
            boxLocal.Visible = true;
            boxMax.Visible = true;
            boxMin.Visible = true;
            boxAtual.Visible = true;
            label3.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            boxCentroCusto.Visible = false;
            label9.Visible = false;
            AtualizarButton.Visible = true;
            CadastrarButton.Visible = true;    
            Grid1.Visible = false;
            label10.Visible = false;

        }       

        //DUPLO CLICK NO TEXTBOX DO YG
        private void BoxYG_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            found = false;     
            foreach (Peça x in Peças)
            {
                if (x.yg == boxYG.Text)
                {
                    Exibir(x);
                    found = true;
                }         
            } 
            if (found == false)
            {
                boxDescricao.Clear();
                boxLocal.Clear();
                boxMax.Clear();
                boxMin.Clear();
                boxAtual.Clear();            
            }
           
        }
        //BOTAO ATUALIZAR
        private void AtualizarButton_Click(object sender, EventArgs e)
        {
            foreach (Peça x in Peças)
            {
                if (x.yg == boxYG.Text)
                {
                    try
                    {
                        x.Descricao = boxDescricao.Text;
                        x.local = boxLocal.Text;
                        x.max = Convert.ToInt32(boxMax.Text);
                        x.min = Convert.ToInt32(boxMin.Text);
                        x.atual = Convert.ToInt32(boxAtual.Text);

                        MessageBox.Show("Atualizado!");

                        boxYG.Clear();
                        boxDescricao.Clear();
                        boxLocal.Clear();
                        boxMax.Clear();
                        boxMin.Clear();
                        boxAtual.Clear();
                    }
                    catch (Exception B) { MessageBox.Show("ERROR: " + B.Message); }
                }

            }
            cycles = 0;
            Refreshh = true;
            beepAuto = true;

        }
        //BOTAO CADASTRAR
        private void CadastrarButton_Click(object sender, EventArgs e)
        {
            try
            {
                Peça nova = new Peça(boxYG.Text)
                {
                    Descricao = boxDescricao.Text,
                    local = boxLocal.Text,
                    max = Convert.ToInt32(boxMax.Text),
                    min = Convert.ToInt32(boxMin.Text),
                    atual = Convert.ToInt32(boxAtual.Text),
                };

                Peças.Add(nova);

                MessageBox.Show("Cadastrado com sucesso!");

                boxYG.Clear();
                boxDescricao.Clear();
                boxLocal.Clear();
                boxMax.Clear();
                boxMin.Clear();
                boxAtual.Clear();
            }
            catch (Exception A) { MessageBox.Show("ERROR: " + A.Message); }
            
            cycles = 0;
            Refreshh = true;
            beepAuto = true;

        }       
    
        // AO FECHAR JANELA 
        private void Inicial_FormClosed(object sender, FormClosedEventArgs e)
        {
            Closing = true;

            if (Gravando || beepAuto || Refreshh || requisicao || Empresti)
            {
                while (Gravando)
                {
                    Thread.Sleep(1000);
                }
                
            }         

        }
        //ABRIR JANELA - KANBANS
        private void buttonJanela2_Click(object sender, EventArgs e)
        {            
            Form2 kanban = new Form2();
            kanban.Show();
        }
        //ABRIR JANELA - QR CODE
        private void QrCode_Click(object sender, EventArgs e)
        {
            Form3 qrcode = new Form3();
            qrcode.Show();
        }

    }
    
}