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
    public partial class BFopcje : Form
    {
        public String s;
        BlokObliczeniowy uc;
        public BFopcje(BlokObliczeniowy usr)
        {
            InitializeComponent();
            this.uc = usr;
            this.edtLval_1.Text = uc.prlval[0];
            this.edtRval_1a.Text = uc.prrval_a[0].ToString();
            this.edtRval_1b.Text = uc.prrval_b[0].ToString();
            this.cbDzialanie1.SelectedIndex = this.cbDzialanie1.FindString(uc.prdzial[0].ToString());

            this.edtLval_2.Text = uc.prlval[1];
            this.edtRval_2a.Text = uc.prrval_a[1].ToString();
            this.edtRval_2b.Text = uc.prrval_b[1].ToString();
            this.cbDzialanie2.SelectedIndex = this.cbDzialanie1.FindString(uc.prdzial[1].ToString());

            this.edtLval_3.Text = uc.prlval[2];
            this.edtRval_3a.Text = uc.prrval_a[2].ToString();
            this.edtRval_3b.Text = uc.prrval_b[2].ToString();
            this.cbDzialanie3.SelectedIndex = this.cbDzialanie1.FindString(uc.prdzial[2].ToString());

            this.edtLval_4.Text = uc.prlval[3];
            this.edtRval_4a.Text = uc.prrval_a[3].ToString();
            this.edtRval_4b.Text = uc.prrval_b[3].ToString();
            this.cbDzialanie4.SelectedIndex = this.cbDzialanie1.FindString(uc.prdzial[3].ToString());

            this.edtLval_5.Text = uc.prlval[4];
            this.edtRval_5a.Text = uc.prrval_a[4].ToString();
            this.edtRval_5b.Text = uc.prrval_b[4].ToString();
            this.cbDzialanie5.SelectedIndex = this.cbDzialanie1.FindString(uc.prdzial[4].ToString());

        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            BFopcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            //uc.prText = edtLval_1.Text + " = " + edtRval_1a.Text + " " + cbDzialanie.SelectedItem.ToString() + " " + edtRval1b.Text;
            if (edtLval_1.Text.Length > 0 && edtLval_1.Text.Contains(' ') == false)
            {
                uc.prlval[0] = edtLval_1.Text;
                Int32.TryParse(edtRval_1a.Text, out uc.prrval_a[0]);
                Int32.TryParse(edtRval_1b.Text, out uc.prrval_b[0]);
                uc.prdzial[0] = cbDzialanie1.SelectedItem.ToString();
            }
            else
            {
                uc.prlval[0] = "";
                uc.prrval_a[0] = 0;
                uc.prrval_b[0] = 0;
                uc.prdzial[0] = "+";
            }
            if (edtLval_2.Text.Length > 0 && !edtLval_2.Text.Contains(' '))
            {
                uc.prlval[1] = edtLval_2.Text;
                Int32.TryParse(edtRval_2a.Text, out uc.prrval_a[1]);
                Int32.TryParse(edtRval_2b.Text, out uc.prrval_b[1]);
                uc.prdzial[1] = cbDzialanie2.SelectedItem.ToString();
            }
            else
            {
                uc.prlval[1] = "";
                uc.prrval_a[1] = 0;
                uc.prrval_b[1] = 0;
                uc.prdzial[1] = "+";
            }
            if (edtLval_3.Text.Length > 0 && !edtLval_3.Text.Contains(' '))
            {
                uc.prlval[2] = edtLval_3.Text;
                Int32.TryParse(edtRval_3a.Text, out uc.prrval_a[2]);
                Int32.TryParse(edtRval_3b.Text, out uc.prrval_b[2]);
                uc.prdzial[2] = cbDzialanie3.SelectedItem.ToString();
            }
            else
            {
                uc.prlval[2] = "";
                uc.prrval_a[2] = 0;
                uc.prrval_b[2] = 0;
                uc.prdzial[2] = "+";
            }
            if (edtLval_4.Text.Length > 0 && !edtLval_4.Text.Contains(' '))
            {
                uc.prlval[3] = edtLval_4.Text;
                Int32.TryParse(edtRval_4a.Text, out uc.prrval_a[3]);
                Int32.TryParse(edtRval_4b.Text, out uc.prrval_b[3]);
                uc.prdzial[3] = cbDzialanie4.SelectedItem.ToString();
            }
            else
            {
                uc.prlval[3] = "";
                uc.prrval_a[3] = 0;
                uc.prrval_b[3] = 0;
                uc.prdzial[3] = "+";
            }
            if (edtLval_5.Text.Length > 0 && !edtLval_5.Text.Contains(' '))
            {
                uc.prlval[4] = edtLval_5.Text;
                Int32.TryParse(edtRval_5a.Text, out uc.prrval_a[4]);
                Int32.TryParse(edtRval_5b.Text, out uc.prrval_b[4]);
                uc.prdzial[4] = cbDzialanie5.SelectedItem.ToString();
            }
            else
            {
                uc.prlval[4] = "";
                uc.prrval_a[4] = 0;
                uc.prrval_b[4] = 0;
                uc.prdzial[4] = "+";
            }
            this.Close();
            
            
        }
    }
}
