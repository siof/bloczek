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
        IList<Działanie> dodaneDzialania = new List<Działanie>();

        public BOOpcje(BlokObliczeniowy usr)
        {
            InitializeComponent();
            this.bObl = usr;
            
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje działania to musze je usunąć z listy
            for (int i = 0; i < dodaneDzialania.Count; i++)
            {
                bObl.dzialania.Remove(dodaneDzialania[i]);
            }

            dodaneDzialania.Clear(); //na wszelki wypadek

            BOOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //z racji że na bierząco aktualizuje dzialania to nie trzeba nic robić
            //poza wyświetleniem działań na bloku

            bObl.DodajNoweZmienne();
            bObl.ReDrawText();

            dodaneDzialania.Clear(); //na wszelki wypadek

            this.Close();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if ((txtL.Text == "" || txtL.Text == " ") && (txtS.Text != "" || txtS.Text != " "))
                return;

            String temp;
            Działanie noweDzialanie = new Działanie();

            temp = txtL.Text + " := " + txtS.Text;
            noweDzialanie.lewa = txtL.Text;

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
            dodaneDzialania.Add(noweDzialanie);
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox.SelectedItem == null)
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

            //TODO: jakiś warunek/informacja jeśli wszystkie textboxy są zajęte

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
    }
}
