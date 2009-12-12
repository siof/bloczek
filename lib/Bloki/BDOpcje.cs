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

        public BDOpcje(BlokDecyzyjny usr)
        {
            bDec = usr;
            InitializeComponent();
        }

        public BDOpcje()
        {
            InitializeComponent();
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            BDOpcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            
        }

        private void listBoxZmienne_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox.SelectedItem != null)
                txtBox.Text = bDec.znacznikZmiennej + listBoxZmienne.SelectedItem.ToString() + bDec.znacznikZmiennej;
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            String temp;
            if (comboBox1.SelectedText != "" && comboBox2.SelectedText != "" && txtBox.Text != "")
            {
                temp = "";

                if (comboBox3.Visible == true)
                    temp = comboBox3.SelectedText + " ";
 
                temp += bDec.znacznikZmiennej + comboBox1.SelectedItem.ToString() + bDec.znacznikZmiennej + " " + comboBox2.SelectedItem.ToString() + " " + txtBox.Text;
                listBox.Items.Add(temp);
                comboBox3.Visible = true;
            }
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                listBox.Items.Remove(listBox.SelectedItem);
                comboBox3.Visible = false;
            }
        }
    }
}
