using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libbloki
{
    public partial class BDOpcje : Form
    {
        BlokDecyzyjny bDec;
        IList<Dzialanie> poprzenieDzialania = new List<Dzialanie>();

        public BDOpcje(BlokDecyzyjny blok)
        {
            InitializeComponent();

            if (blok == null)
            {
                MessageBox.Show("BDOpcje: konstruktor: blok nie istnieje");
                this.Close();
                return;
            }

            bDec = blok;
            String temp = "";

            for (int i = 0; i < bDec.dzialania.Count; i++)
            {
                if (i != 0)
                    temp = bDec.dzialania[i].dodatkowe.ToString() + " ";

                temp += bDec.znacznikZmiennej.ToString() + bDec.dzialania[i].lewa.ToString() + bDec.znacznikZmiennej.ToString();
                temp += " " + bDec.dzialania[i].dzialanie1.ToString() + " ";

                if (bDec.dzialania[i].srodekZmienna == true)
                    temp += bDec.znacznikZmiennej.ToString() + bDec.dzialania[i].srodek.ToString() + bDec.znacznikZmiennej.ToString();
                else
                    temp += bDec.dzialania[i].srodek.ToString();

                poprzenieDzialania.Add(bDec.dzialania[i]);

                listBox.Items.Add(temp);
            }

            if (bDec.listaZmiennych != null)
            {
                //czytaj liste zmiennych i dodaj do combo/list boxow
                for (int i = 0; i < bDec.listaZmiennych.Count; i++)
                {
                    if (bDec.listaZmiennych[i].tablica == true)
                        temp = bDec.listaZmiennych[i].nazwa.ToString() + "[]";
                    else
                        temp = bDec.listaZmiennych[i].nazwa.ToString();
                    listBoxZmienne.Items.Add(temp);
                }
            }

            if (bDec.dzialania.Count > 0)
                comboBox3.Visible = true;

            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        public BDOpcje()
        {
            InitializeComponent();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //przywroc poprzednia wersje dzialan:
            bDec.dzialania.Clear();
            for (int i = 0; i < poprzenieDzialania.Count; i++)
                bDec.dzialania.Add(poprzenieDzialania[i]);

            poprzenieDzialania.Clear(); //na wszelki wypadek

            BDOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku 

            poprzenieDzialania.Clear(); //na wszelki wypadek

            BDOpcje.ActiveForm.Close();
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
            {
                if (txtBoxL.Text != "")
                    txtBoxP.Text = bDec.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bDec.znacznikZmiennej;
                else
                    txtBoxL.Text = bDec.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bDec.znacznikZmiennej;
            }
        }

        private void WyczyscPola()
        {
            txtBoxP.Text = "";
            txtBoxL.Text = "";
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
                return;

            String temp;
            if (txtBoxL.Text != "" && comboBox2.SelectedItem.ToString() != "" && txtBoxP.Text != "")
            {
                temp = "";
                Dzialanie noweDzialanie = new Dzialanie();

                if (comboBox3.Visible == true)
                {
                    if (comboBox3.SelectedItem == null)
                        return;

                    temp = comboBox3.SelectedItem.ToString() + " ";
                    noweDzialanie.dodatkowe = comboBox3.SelectedItem.ToString();
                }

                temp += txtBoxL.Text + " " + comboBox2.SelectedItem.ToString() + " " + txtBoxP.Text;
                noweDzialanie.lewa = txtBoxL.Text;
                noweDzialanie.dzialanie1 = comboBox2.SelectedItem.ToString();
                noweDzialanie.srodek = txtBoxP.Text;

                listBox.Items.Add(temp);
                comboBox3.Visible = true;
                bDec.dzialania.Add(noweDzialanie);
                WyczyscPola();
            }            
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox.SelectedItem != null && listBox.SelectedIndex >= 0)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    bDec.dzialania.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                    if (listBox.Items.Count == 0)
                        comboBox3.Visible = false;
                }
            }
        }

        private void btnDoGory_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null)
                return;

            if (listBox.SelectedIndex == 0)
                return;

            int tmpInd = listBox.SelectedIndex;
            Dzialanie tmpDzial = bDec.dzialania[tmpInd - 1];
            String tmpString = listBox.Items[tmpInd - 1].ToString();
            listBox.Items[tmpInd - 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            bDec.dzialania[tmpInd - 1] = bDec.dzialania[tmpInd];
            bDec.dzialania[tmpInd] = tmpDzial;
            listBox.SelectedIndex = tmpInd - 1;
            tmpDzial = null;
            tmpString = null;
            tmpInd = 0;
        }

        private void btnNaDol_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem == null)
                return;

            if (listBox.SelectedIndex == listBox.Items.Count - 1)
                return;

            int tmpInd = listBox.SelectedIndex;
            Dzialanie tmpDzial = bDec.dzialania[tmpInd + 1];
            String tmpString = listBox.Items[tmpInd + 1].ToString();
            listBox.Items[tmpInd + 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            bDec.dzialania[tmpInd + 1] = bDec.dzialania[tmpInd];
            bDec.dzialania[tmpInd] = tmpDzial;
            listBox.SelectedIndex = tmpInd + 1;
            tmpDzial = null;
            tmpString = null;
            tmpInd = 0;
        }
    }
}
