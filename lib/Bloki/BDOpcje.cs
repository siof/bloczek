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
        IList<Działanie> dodaneDzialania = new List<Działanie>();

        public BDOpcje(BlokDecyzyjny usr)
        {
            InitializeComponent();

            bDec = usr;
            String temp = "";

            for (int i = 0; i < bDec.dzialania.Count; i++)
            {
                if (i != 0)
                    temp = bDec.dzialania[i].dodatkowe.ToString() + " ";

                temp += bDec.znacznikZmiennej.ToString() + bDec.dzialania[i].lewa.ToString() + bDec.znacznikZmiennej.ToString();
                temp += " " + bDec.dzialania[i].dzialanie1.ToString() + " ";

                /*if (bDec.dzialania[i].srodekZmienna == true)
                    temp += bDec.znacznikZmiennej.ToString() + bDec.dzialania[i].srodek.ToString() + bDec.znacznikZmiennej.ToString();
                else*/
                    temp += bDec.dzialania[i].srodek.ToString();

                listBox.Items.Add(temp);
            }

            if (bDec.listaZmiennych != null)
            {
                //czytaj liste zmiennych i dodaj do combo/list boxow
                for (int i = 0; i < bDec.listaZmiennych.Count; i++)
                {
                    temp = bDec.listaZmiennych[i].nazwa.ToString();
                    listBoxZmienne.Items.Add(temp);
                    comboBox1.Items.Add(temp);
                }
            }

            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }

        public BDOpcje()
        {
            InitializeComponent();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje działania to musze je usunąć z listy
            for (int i = 0; i < dodaneDzialania.Count; i++)
            {
                bDec.dzialania.Remove(dodaneDzialania[i]);
            }

            dodaneDzialania.Clear(); //na wszelki wypadek

            BDOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku 

            dodaneDzialania.Clear(); //na wszelki wypadek

            BDOpcje.ActiveForm.Close();
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
                txtBox.Text = bDec.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bDec.znacznikZmiennej;
        }

        private void WyczyscPola()
        {
            txtBox.Text = "";
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null && comboBox2.SelectedItem == null)
                return;

            String temp;
            if (comboBox1.SelectedItem.ToString() != "" && comboBox2.SelectedItem.ToString() != "" && txtBox.Text != "")
            {
                temp = "";
                Działanie noweDzialanie = new Działanie();

                if (comboBox3.Visible == true)
                {
                    if (comboBox3.SelectedItem == null)
                        return;

                    temp = comboBox3.SelectedItem.ToString() + " ";
                    noweDzialanie.dodatkowe = comboBox3.SelectedItem.ToString();
                }

                temp += bDec.znacznikZmiennej + comboBox1.SelectedItem.ToString() + bDec.znacznikZmiennej + " " + comboBox2.SelectedItem.ToString() + " " + txtBox.Text;
                noweDzialanie.lewa = comboBox1.SelectedItem.ToString();
                noweDzialanie.dzialanie1 = comboBox2.SelectedItem.ToString();
                noweDzialanie.srodek = txtBox.Text;

                listBox.Items.Add(temp);
                comboBox3.Visible = true;
                bDec.dzialania.Add(noweDzialanie);
                dodaneDzialania.Add(noweDzialanie);
            }


            
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                if (e.KeyCode == Keys.Delete)
                {
                    dodaneDzialania.Remove(bDec.dzialania.ElementAt(listBox.SelectedIndex));
                    bDec.dzialania.RemoveAt(listBox.SelectedIndex);
                    listBox.Items.Remove(listBox.SelectedItem);
                    if (listBox.Items.Count == 0)
                        comboBox3.Visible = false;
                }
            }
        }
    }
}
