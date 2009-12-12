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

        public BWeWyOpcje()
        {
            InitializeComponent();
        }

        public BWeWyOpcje(BlokWeWy blok)
        {
            BWeWy = blok;
            InitializeComponent();
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox.SelectedItem == null)
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
            temp = comboBox1.SelectedItem.ToString() + " : " + txtBox.Text;
            listBox.Items.Add(temp);
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            BWeWyOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {

        }
    }
}
