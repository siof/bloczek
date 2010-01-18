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
    public partial class BOOpcje : Form
    {
        BlokObliczeniowy bObl;
        IList<Dzialanie> poprzenieDzialania = new List<Dzialanie>();

        public BOOpcje(BlokObliczeniowy blok)
        {
            InitializeComponent();

            if (blok == null)
            {
                MessageBox.Show("BOOpcje: konstruktor: blok nie istnieje");
                this.Close();
                return;
            }

            bObl = blok;
            String temp = "";

            for (int i = 0; i < bObl.dzialania.Count; i++)
            {
                temp = bObl.znacznikZmiennej + bObl.dzialania[i].lewa + bObl.znacznikZmiennej + " := ";

                if (bObl.dzialania[i].srodekZmienna == true)
                    temp += bObl.znacznikZmiennej + bObl.dzialania[i].srodek + bObl.znacznikZmiennej;
                else
                    temp += bObl.dzialania[i].srodek;

                if (bObl.dzialania[i].dzialanie2 != null)
                {
                    temp += " " + bObl.dzialania[i].dzialanie2 + " ";

                    if (bObl.dzialania[i].prawaZmienna == true)
                        temp += bObl.dzialania[i].znacznikZmiennej + bObl.dzialania[i].prawa + bObl.znacznikZmiennej;
                    else
                        temp += bObl.dzialania[i].prawa;
                }

                poprzenieDzialania.Add(bObl.dzialania[i]);

                listBox.Items.Add(temp);
            }
            if (bObl.listaZmiennych != null)
            {
                //czytaj liste zmiennych i dodaj do combo/list boxow
                for (int i = 0; i < bObl.listaZmiennych.Count; i++)
                {
                    if (bObl.listaZmiennych[i].tablica == true)
                        temp = bObl.listaZmiennych[i].nazwa.ToString() + "[]";
                    else
                        temp = bObl.listaZmiennych[i].nazwa.ToString();
                    listBoxZmienne.Items.Add(temp);
                }
            }
            comboBox2.SelectedIndex = 0;
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje zmienne to musze je usunąć z listy 
            for (int i = 0; i < bObl.dodaneZmienne.Count; i++)
            {
                bObl.listaZmiennych.Remove(bObl.dodaneZmienne[i]);
            }

            //przywroc poprzednia wersje dzialan:
            bObl.dzialania.Clear();
            for (int i = 0; i < poprzenieDzialania.Count; i++)
                bObl.dzialania.Add(poprzenieDzialania[i]);

            poprzenieDzialania.Clear(); //na wszelki wypadek
            bObl.dodaneZmienne.Clear(); //na wszelki wypadek

            BOOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku

            bObl.AktualizujTXT();

            poprzenieDzialania.Clear(); //na wszelki wypadek
            bObl.dodaneZmienne.Clear();

            this.Close();
        }

        private void WyczyscPola()
        {
            txtL.Text = "";
            txtP.Text = "";
            txtS.Text = "";
        }

        private bool CzyZmiennaJestWLB(String zmienna)
        {
            if (zmienna == null || listBoxZmienne.Items.Count == 0)
                return false;

            String temp = zmienna.ToString();

            if (temp.Contains(bObl.znacznikZmiennej) == true)
                temp = temp.Replace(bObl.znacznikZmiennej, "");

            if (temp.Contains('[') == true && temp.Contains(']') == true)
            {
                int tmpInd1 = temp.IndexOf('[') + 1;
                int tmpInd2 = temp.IndexOf(']');

                temp = temp.Remove(tmpInd1, tmpInd2 - tmpInd1);
            }

            for (int i = 0; i < listBoxZmienne.Items.Count; i++)
            {
                if (listBoxZmienne.Items[i].ToString() == temp.ToString())
                    return true;
            }

            return false;
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if ((txtL.Text == "" || txtL.Text == " ") && (txtS.Text != "" || txtS.Text != " "))
                return;

            if (cbDzialanie != null && cbDzialanie.Text == "/" && (txtP.Text == "0" || txtP.Text == "0.0"))
            {
                MessageBox.Show("Proba dzielenia przez 0");
                return;
            }

            String temp;
            Dzialanie noweDzialanie = new Dzialanie();

            temp = txtL.Text + " := " + txtS.Text;
            noweDzialanie.lewa = txtL.Text;

            if (bObl.SprawdzCzyIstniejeZmienna(txtL.Text) == false)
            {
                noweDzialanie.lewa = noweDzialanie.lewa.Replace(bObl.znacznikZmiennej, "");
                noweDzialanie.nowaZmienna = true;
                if (comboBox2.SelectedItem != null)
                    noweDzialanie.dodatkowe = comboBox2.SelectedItem.ToString();
                else
                    noweDzialanie.dodatkowe = "String";

                if (CzyZmiennaJestWLB(txtL.Text) == false)
                {
                    String tmpString = noweDzialanie.lewa.ToString();
                    if (tmpString.Contains('[') == true && tmpString.Contains(']') == true)
                    {
                        int tmpInd1 = tmpString.IndexOf('[') + 1;
                        int tmpInd2 = tmpString.IndexOf(']');

                        tmpString = tmpString.Remove(tmpInd1, tmpInd2 - tmpInd1);
                    }
                    listBoxZmienne.Items.Add(tmpString);        
                }
            }

            if (txtS.Text != "" && txtS.Text != " ")
            {
                noweDzialanie.dzialanie1 = ":=";
                noweDzialanie.srodek = txtS.Text;

                if (cbDzialanie.SelectedItem != null && cbDzialanie.SelectedItem.ToString() != "" && (txtP.Text != "" || txtP.Text != " "))
                {
                    temp += " " + cbDzialanie.SelectedItem.ToString() + " " + txtP.Text;
                    noweDzialanie.dzialanie2 = cbDzialanie.SelectedItem.ToString();
                    noweDzialanie.prawa = txtP.Text;
                }

                listBox.Items.Add(temp);
                bObl.dzialania.Add(noweDzialanie);
            }
            bObl.DodajNoweZmienne(noweDzialanie);
            WyczyscPola();
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null)
                return;

            if (txtL.Text == "" || txtL.Text == " ")
            {
                txtL.Text = bObl.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bObl.znacznikZmiennej;

                return;
            }

            if (txtS.Text == "" || txtS.Text == " ")
            {
                txtS.Text = bObl.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bObl.znacznikZmiennej;

                return;
            }

            if (txtP.Text == "" || txtP.Text == " ")
            {
                txtP.Text = bObl.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bObl.znacznikZmiennej;

                return;
            }
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    bObl.dzialania.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
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
            Dzialanie tmpDzial = bObl.dzialania[tmpInd - 1];
            String tmpString = listBox.Items[tmpInd - 1].ToString();
            listBox.Items[tmpInd - 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            bObl.dzialania[tmpInd - 1] = bObl.dzialania[tmpInd];
            bObl.dzialania[tmpInd] = tmpDzial;
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
            Dzialanie tmpDzial = bObl.dzialania[tmpInd + 1];
            String tmpString = listBox.Items[tmpInd + 1].ToString();
            listBox.Items[tmpInd + 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            bObl.dzialania[tmpInd + 1] = bObl.dzialania[tmpInd];
            bObl.dzialania[tmpInd] = tmpDzial;
            listBox.SelectedIndex = tmpInd + 1;
            tmpDzial = null;
            tmpString = null;
            tmpInd = 0;
        }
    }
}
