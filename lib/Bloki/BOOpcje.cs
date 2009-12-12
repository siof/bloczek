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

        public BOOpcje(BlokObliczeniowy usr)
        {
            InitializeComponent();
            this.bObl = usr;
            
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            BOOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            if (txtL.Text == "" || txtL.Text == " ")
                return;

            String temp;

            temp = txtL.Text;

            if (txtS.Text != "" || txtS.Text != " ")
            {
                temp += " := " + txtS.Text;

                if (cbDzialanie.SelectedText != "" && (txtP.Text != "" || txtP.Text != " "))
                    temp += " " + cbDzialanie.SelectedText + " " + txtP.Text;
            }

            listBox.Items.Add(temp);
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
    }
}
