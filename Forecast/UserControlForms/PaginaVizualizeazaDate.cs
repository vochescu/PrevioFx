﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Forecast.Entities;


namespace Forecast
{
    public partial class PaginaVizualizeazaDate : UserControl
    {
        private Database database = new Database();
        private String perecheValutaraSelectata;

        public PaginaVizualizeazaDate()
        {
            perecheValutaraSelectata = null;
            InitializeComponent();
            dezactiveazaButoaneDataGrid();
           
        }
        public String GetPerecheValutaraSelectata()
        {
            return perecheValutaraSelectata;
        }
        private void paginaVizualizeazaDate_Load(object sender, EventArgs e)
        {
            DataSet dataSet = database.SelectTotPerecheValutaraDataset();
            if(dataSet!= null)
            {
                perechiValutareCb.DisplayMember = "Simbol";
                perechiValutareCb.ValueMember = "Simbol";
                perechiValutareCb.DataSource = dataSet.Tables[Constante.DATASET_NAME_PERECHI_VALUTARE];
            }
            else
            {
                MessageBox.Show(Constante.DB_EROARE_SELECT_PERECHI_VALUTARE);
            }
        }

        private void vizualizareBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(perecheValutaraSelectata))
            {
                MessageBox.Show(Constante.EROARE_SELECTARE_PERECHE_VALUTARA);
            }
            else
            {
               
                updateGridViewData();
                activeazaButoaneDataGrid();
                
            }
        }

        private void adaugaBtn_Click(object sender, EventArgs e)
        {
            if(perecheValutaraSelectata!=null)
            {
                AdaugaValoriForm adaugaValoriForm = new AdaugaValoriForm(perecheValutaraSelectata);
                
                if(adaugaValoriForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Pereche valutară adaugată!");
                    updateGridViewData();
                }
            }
            else
            {
                MessageBox.Show(Constante.EROARE_SELECTARE_PERECHE_VALUTARA);
            }
            
        }

        private void editeazaBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                Valoare val = getValoareFromGridView(dataGrid.SelectedRows[0].Index);
                
                AdaugaValoriForm adaugaValoriForm = new AdaugaValoriForm(val,perecheValutaraSelectata);
                if(adaugaValoriForm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Pereche valutară modificată!");
                    updateGridViewData();
                }
            }
            else
            {
                MessageBox.Show(Constante.EROARE_SELECTARE_RAND_TABEL);
            }
        }

        private void stergeBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                Valoare val = getValoareFromGridView(dataGrid.SelectedRows[0].Index);
                database.DeleteValoriIntoDatabase(val);
                updateGridViewData();
            }
            else
            {
                MessageBox.Show(Constante.EROARE_SELECTARE_RAND_TABEL);
            }
        }

        private void updateGridViewData()
        {
            DataTable dataTable = database.SelectTotValoriDataTable(perecheValutaraSelectata);
            if(dataTable!= null)
            {
                dataGrid.DataSource = dataTable;
            }
           

        }
        private Valoare getValoareFromGridView(int index)
        {
            DateTime data = DateTime.Parse(dataGrid.Rows[index].Cells[0].Value.ToString());
            Double open = Double.Parse((dataGrid.Rows[index].Cells[1].Value.ToString()));
            Double high = Double.Parse((dataGrid.Rows[index].Cells[2].Value.ToString()));
            Double low = Double.Parse((dataGrid.Rows[index].Cells[3].Value.ToString()));
            Double close = Double.Parse((dataGrid.Rows[index].Cells[4].Value.ToString()));
            return new Valoare(data, open, high, low, close);
        }

        private void perechiValutareCb_DropDownClosed(object sender, EventArgs e)
        {
            perecheValutaraSelectata = perechiValutareCb.Text;
        }

        private void activeazaButoaneDataGrid()
        {
            adaugaBtn.Enabled = true;
            editeazaBtn.Enabled = true;
            stergeBtn.Enabled = true;
        }

        private void dezactiveazaButoaneDataGrid()
        {
            adaugaBtn.Enabled = false;
            editeazaBtn.Enabled = false;
            stergeBtn.Enabled = false;
        }
    }
}
