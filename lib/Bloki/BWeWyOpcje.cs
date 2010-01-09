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
    public partial class BWeWyOpcje : Form
    {
        BlokWeWy BWeWy;
        IList<Dzialanie> poprzenieDzialania = new List<Dzialanie>();

        public BWeWyOpcje()
        {
            InitializeComponent();
        }

        public BWeWyOpcje(BlokWeWy blok)
        {
            InitializeComponent();

            BWeWy = blok;
            String temp = "";

            for (int i = 0; i < BWeWy.dzialania.Count; i++)
            {
                temp = BWeWy.dzialania[i].dzialanie1 + " : ";
                temp += BWeWy.dzialania[i].srodek;
                poprzenieDzialania.Add(BWeWy.dzialania[i]);

                listBox.Items.Add(temp);
            }

            if (BWeWy.listaZmiennych != null)
            {
                //czytaj liste zmiennych i dodaj do combo/list boxow
                for (int i = 0; i < BWeWy.listaZmiennych.Count; i++)
                {
                    if (BWeWy.listaZmiennych[i].tablica == true)
                        temp = BWeWy.listaZmiennych[i].nazwa.ToString() + "[]";
                    else
                        temp = BWeWy.listaZmiennych[i].nazwa.ToString();
                    listBoxZmienne.Items.Add(temp);
                }
            }
            comboBox2.SelectedIndex = 0;
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null)
                return;

            if (comboBox1.SelectedText == "Czytaj")
            {
                txtBox.Text = BWeWy.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + BWeWy.znacznikZmiennej;
            }
            else
            {
                txtBox.Text += " " + BWeWy.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + BWeWy.znacznikZmiennej + " ";
            }
        }

        private void Dodaj_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
                return;

            String temp;
            Dzialanie noweDzialanie = new Dzialanie();

            temp = comboBox1.SelectedItem.ToString() + " : " + txtBox.Text;

            noweDzialanie.dzialanie1 = comboBox1.SelectedItem.ToString();
            noweDzialanie.srodekBWeWy = txtBox.Text;

            if (comboBox1.SelectedItem.ToString() == "Czytaj" && BWeWy.SprawdzCzyIstniejeZmienna(txtBox.Text) == false)
            {
                noweDzialanie.nowaZmienna = true;
                noweDzialanie.srodekZmienna = true;
                noweDzialanie.lewa = txtBox.Text;
                noweDzialanie.srodekBWeWy = BWeWy.znacznikZmiennej + txtBox.Text + BWeWy.znacznikZmiennej;
                listBoxZmienne.Items.Add(txtBox.Text.ToString());
                if (comboBox2.SelectedItem != null)
                    noweDzialanie.dodatkowe = comboBox2.SelectedItem.ToString();
                else
                    noweDzialanie.dodatkowe = "String";
            }

            listBox.Items.Add(temp);
            BWeWy.dzialania.Add(noweDzialanie);
            BWeWy.DodajNoweZmienne(noweDzialanie);
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje zmienne to musze je usunąć z listy
            for (int i = 0; i < BWeWy.dodaneZmienne.Count; i++)
            {
                BWeWy.listaZmiennych.Remove(BWeWy.dodaneZmienne[i]);
            }

            //przywroc poprzednia wersje dzialan:
            BWeWy.dzialania.Clear();
            for (int i = 0; i < poprzenieDzialania.Count; i++)
                BWeWy.dzialania.Add(poprzenieDzialania[i]);

            poprzenieDzialania.Clear(); //na wszelki wypadek

            BWeWyOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku

            //BWeWy.DodajNoweZmienne();

            poprzenieDzialania.Clear(); //na wszelki wypadek

            BWeWyOpcje.ActiveForm.Close();
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (BWeWy.dzialania[listBox.SelectedIndex].nowaZmienna == true)
                    {
                        String temp = BWeWy.dzialania[listBox.SelectedIndex].lewa.ToString();
                        for (int i = 0; i < listBoxZmienne.Items.Count; i++)
                        {
                            if (listBoxZmienne.Items[i].ToString() == temp)
                            {
                                listBoxZmienne.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    BWeWy.dzialania.RemoveAt(listBox.SelectedIndex);
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
            Dzialanie tmpDzial = BWeWy.dzialania[tmpInd - 1];
            String tmpString = listBox.Items[tmpInd - 1].ToString();
            listBox.Items[tmpInd - 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            BWeWy.dzialania[tmpInd - 1] = BWeWy.dzialania[tmpInd];
            BWeWy.dzialania[tmpInd] = tmpDzial;
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
            Dzialanie tmpDzial = BWeWy.dzialania[tmpInd + 1];
            String tmpString = listBox.Items[tmpInd + 1].ToString();
            listBox.Items[tmpInd + 1] = listBox.Items[tmpInd].ToString();
            listBox.Items[tmpInd] = tmpString.ToString();
            BWeWy.dzialania[tmpInd + 1] = BWeWy.dzialania[tmpInd];
            BWeWy.dzialania[tmpInd] = tmpDzial;
            listBox.SelectedIndex = tmpInd + 1;
            tmpDzial = null;
            tmpString = null;
            tmpInd = 0;
        }
    }
}
