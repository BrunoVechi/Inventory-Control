using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace Spareparts
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //CARREGA GRID COM A LISTA DE KANBANS
            var list1 = new BindingList<Requisitar>(Inicial.Requisita);
            Grid2.Invoke((System.Action)(() => Grid2.DataSource = list1));
        }

        private void Confirma_Click(object sender, EventArgs e)
        { 
            string YGG = "";
            int Indice = 0;
            int indice = 0;
            bool existe = false;
            foreach (Peça x in Inicial.Peças)
            {  
                try
                {
                    if (x.yg == boxYG2.Text)
                    {
                        YGG = x.yg;

                        //SE BOX "UTILIDADES" SELECIONADO
                        if (checkedListBox1.GetItemCheckState(0) == CheckState.Checked)
                        {
                            x.Utl += Convert.ToInt32(BoxQuantid.Text);
                        }
                        // SE BOX "EXTERNO" SELECIONADO
                        if (checkedListBox1.GetItemCheckState(1) == CheckState.Checked)
                        {
                            x.Ext += Convert.ToInt32(BoxQuantid.Text);
                        }

                        x.atual += Convert.ToInt32(BoxQuantid.Text);

                        
                        //PESQUISA NA LISTA DE KANBAN
                        foreach (Requisitar y in Inicial.Requisita)
                        {
                            //ATUALIZA KANBAN 
                            if (YGG == y.YG)
                            {
                                y.local = x.local;
                                y.Descricao = x.Descricao;
                                y.Min = x.min;
                                y.Atual = x.atual;
                                y.ConsumoUTL = x.Utl;
                                y.ConsumoEXT = x.Ext;

                                indice = Indice;
                                existe = true;
                            }

                            Indice += 1;
                        }

                        // SE NESCESSARIO TIRAR ITEM DA LISTA
                        if ((x.atual > x.min) && existe)
                        {
                            try
                            {
                                Inicial.DeleteExcelRequisita();
                            }
                            catch (Exception error) { MessageBox.Show(error.Message + ". Verificar KanbanList no BD.Excel!", "ERROR"); }
                            
                            Inicial.Requisita.RemoveAt(indice);
                            
                        }

                        Inicial.requisicao = true;
                        Inicial.beepAuto = true;

                        //CARREGA GRID COM A LISTA DE KANBANS
                        var list1 = new BindingList<Requisitar>(Inicial.Requisita);
                        Grid2.Invoke((System.Action)(() => Grid2.DataSource = list1));

                        MessageBox.Show("Adicionado com sucesso!");

                        boxYG2.Clear();
                        BoxQuantid.Clear();
                        
                        
                        Inicial.cycles = 75;

                    }
                }catch (Exception f) { MessageBox.Show(f.Message); }
            }


        }


    }

}
