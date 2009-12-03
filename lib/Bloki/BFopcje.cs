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
            int tempCount = uc.dzialania.Count;
            if (tempCount == 0)
                return;

            while (tempCount > 0)
            {
                switch (tempCount)
                {
                    case 5:
                        this.edtLval_5.Text = uc.dzialania[4].lewa.ToString();
                        this.edtRval_5a.Text = uc.dzialania[4].srodek.ToString();
                        this.edtRval_5b.Text = uc.dzialania[4].prawa.ToString();
                        this.cbDzialanie5.SelectedIndex = this.cbDzialanie1.FindString(uc.dzialania[4].dzialanie2.ToString());
                        break;
                    case 4:
                        this.edtLval_4.Text = uc.dzialania[3].lewa.ToString();
                        this.edtRval_4a.Text = uc.dzialania[3].srodek.ToString();
                        this.edtRval_4b.Text = uc.dzialania[3].prawa.ToString();
                        this.cbDzialanie4.SelectedIndex = this.cbDzialanie1.FindString(uc.dzialania[3].dzialanie2.ToString());
                        break;
                    case 3:
                        this.edtLval_3.Text = uc.dzialania[2].lewa.ToString();
                        this.edtRval_3a.Text = uc.dzialania[2].srodek.ToString();
                        this.edtRval_3b.Text = uc.dzialania[2].prawa.ToString();
                        this.cbDzialanie3.SelectedIndex = this.cbDzialanie1.FindString(uc.dzialania[2].dzialanie2.ToString());
                        break;
                    case 2:
                        this.edtLval_2.Text = uc.dzialania[1].lewa.ToString();
                        this.edtRval_2a.Text = uc.dzialania[1].srodek.ToString();
                        this.edtRval_2b.Text = uc.dzialania[1].prawa.ToString();
                        this.cbDzialanie2.SelectedIndex = this.cbDzialanie1.FindString(uc.dzialania[1].dzialanie2.ToString());
                        break;
                    case 1:
                        this.edtLval_1.Text = uc.dzialania[0].lewa.ToString();
                        this.edtRval_1a.Text = uc.dzialania[0].srodek.ToString();
                        this.edtRval_1b.Text = uc.dzialania[0].prawa.ToString();
                        this.cbDzialanie1.SelectedIndex = this.cbDzialanie1.FindString(uc.dzialania[0].dzialanie2.ToString());
                        break;
                    default:
                        break;
                }
                --tempCount;
            }
        }

        private void btnAnuluj_Click(object sender, EventArgs e)
        {
            BFopcje.ActiveForm.Close();
        }

        private void btnZapisz_Click(object sender, EventArgs e)
        {
            Działanie temp = new Działanie();
            uc.dzialania.Clear();
            
            if (edtLval_1.Text.Length > 0 && edtLval_1.Text.Contains(' ') == false)
            {
                temp.lewa = edtLval_1.Text;
                temp.srodek = edtRval_1a.Text;
                temp.prawa = edtRval_1b.Text;
                temp.dzialanie1 = label1.Text;
                temp.dzialanie2 = cbDzialanie1.SelectedItem.ToString();
                uc.dzialania.Add(temp);
            }

            if (edtLval_2.Text.Length > 0 && !edtLval_2.Text.Contains(' '))
            {
                temp.lewa = edtLval_2.Text;
                temp.srodek = edtRval_2a.Text;
                temp.prawa = edtRval_2b.Text;
                temp.dzialanie1 = label2.Text;
                temp.dzialanie2 = cbDzialanie2.SelectedItem.ToString();
                uc.dzialania.Add(temp);
            }

            if (edtLval_3.Text.Length > 0 && !edtLval_3.Text.Contains(' '))
            {
                temp.lewa = edtLval_3.Text;
                temp.srodek = edtRval_3a.Text;
                temp.prawa = edtRval_3b.Text;
                temp.dzialanie1 = label3.Text;
                temp.dzialanie2 = cbDzialanie3.SelectedItem.ToString();
                uc.dzialania.Add(temp);
            }

            if (edtLval_4.Text.Length > 0 && !edtLval_4.Text.Contains(' '))
            {
                temp.lewa = edtLval_4.Text;
                temp.srodek = edtRval_4a.Text;
                temp.prawa = edtRval_4b.Text;
                temp.dzialanie1 = label4.Text;
                temp.dzialanie2 = cbDzialanie4.SelectedItem.ToString();
                uc.dzialania.Add(temp);
            }

            if (edtLval_5.Text.Length > 0 && !edtLval_5.Text.Contains(' '))
            {
                temp.lewa = edtLval_5.Text;
                temp.srodek = edtRval_5a.Text;
                temp.prawa = edtRval_5b.Text;
                temp.dzialanie1 = label5.Text;
                temp.dzialanie2 = cbDzialanie5.SelectedItem.ToString();
                uc.dzialania.Add(temp);
            }

            uc.ReDrawText();
            this.Close();           
        }

        private void chbN1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbN1.Checked == true)
            {
                edtLval_1.Visible = true;
                cbTyp1.Visible = true;
                cbZmienneL1.Visible = false;
            }
            else
            {
                edtLval_1.Visible = false;
                cbTyp1.Visible = false;
                cbZmienneL1.Visible = true;
            }
        }

        private void chbN2_CheckedChanged(object sender, EventArgs e)
        {
            if (chbN2.Checked == true)
            {
                edtLval_2.Visible = true;
                cbTyp2.Visible = true;
                cbZmienneL2.Visible = false;
            }
            else
            {
                edtLval_2.Visible = false;
                cbTyp2.Visible = false;
                cbZmienneL2.Visible = true;
            }
        }

        private void chbN3_CheckedChanged(object sender, EventArgs e)
        {
            if (chbN3.Checked == true)
            {
                edtLval_3.Visible = true;
                cbTyp3.Visible = true;
                cbZmienneL3.Visible = false;
            }
            else
            {
                edtLval_3.Visible = false;
                cbTyp3.Visible = false;
                cbZmienneL3.Visible = true;
            }
        }

        private void chbN4_CheckedChanged(object sender, EventArgs e)
        {
            if (chbN4.Checked == true)
            {
                edtLval_4.Visible = true;
                cbTyp4.Visible = true;
                cbZmienneL4.Visible = false;
            }
            else
            {
                edtLval_4.Visible = false;
                cbTyp4.Visible = false;
                cbZmienneL4.Visible = true;
            }
        }

        private void chbN5_CheckedChanged(object sender, EventArgs e)
        {
            if (chbN5.Checked == true)
            {
                edtLval_5.Visible = true;
                cbTyp5.Visible = true;
                cbZmienneL5.Visible = false;
            }
            else
            {
                edtLval_5.Visible = false;
                cbTyp5.Visible = false;
                cbZmienneL5.Visible = true;
            }
        }

        private void chbZS1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZS1.Checked == true)
                cbZmienneS1.Visible = true;
            else
                cbZmienneS1.Visible = false;
        }

        private void chbZS2_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZS2.Checked == true)
                cbZmienneS2.Visible = true;
            else
                cbZmienneS2.Visible = false;
        }

        private void chbZS3_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZS3.Checked == true)
                cbZmienneS3.Visible = true;
            else
                cbZmienneS3.Visible = false;
        }

        private void chbZS4_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZS4.Checked == true)
                cbZmienneS4.Visible = true;
            else
                cbZmienneS4.Visible = false;
        }

        private void chbZS5_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZS5.Checked == true)
                cbZmienneS5.Visible = true;
            else
                cbZmienneS5.Visible = false;
        }

        private void chbZP1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZP1.Checked == true)
                cbZmienneP1.Visible = true;
            else
                cbZmienneP1.Visible = false;
        }

        private void chbZP2_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZP2.Checked == true)
                cbZmienneP2.Visible = true;
            else
                cbZmienneP2.Visible = false;
        }

        private void chbZP3_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZP3.Checked == true)
                cbZmienneP3.Visible = true;
            else
                cbZmienneP3.Visible = false;
        }

        private void chbZP4_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZP4.Checked == true)
                cbZmienneP4.Visible = true;
            else
                cbZmienneP4.Visible = false;
        }

        private void chbZP5_CheckedChanged(object sender, EventArgs e)
        {
            if (chbZP5.Checked == true)
                cbZmienneP5.Visible = true;
            else
                cbZmienneP5.Visible = false;
        }
    }
}
