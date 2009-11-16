using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libbloki;
using System.Threading;

namespace Okienka
{
    public partial class Form1 : Form
    {
        private static int numer = 0;
        private bool klik = false;
        private bool ctrl = false;
        private int ile = 0;
        private IList<BlokObliczeniowy> tabBO =new List<BlokObliczeniowy>();
        private Graphics g;
        BlokObliczeniowy temp_BO;

        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            
        }

        private int ZnajdzBlok(String nazwa)
        {
            int i;
            for (i = 0; i < ile + 1; i++)
                if (tabBO[i].Name.Equals(nazwa) == true) break;

            return i;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            klik = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "BO_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBO);

                    tabBO.Add(temp);
                    Form1.ActiveForm.Controls.Add(tabBO.Last());
                    
                    ile++;
                if (ctrl != true)
                {
                    klik = false;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void blokObliczeniowy1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void blokObliczeniowy1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int temp = ZnajdzBlok(((BlokObliczeniowy)sender).Name);
                tabBO[temp].Dispose();
            }
        }

        private void UsunBO(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                tabBO.Remove((BlokObliczeniowy)sender);
                ((BlokObliczeniowy)sender).Dispose();
                ile--;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrl = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                ctrl = false;
                klik = false;
            }
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                    BlokObliczeniowy temp = new BlokObliczeniowy();
                    temp.Left = ((MouseEventArgs)e).X;
                    temp.Top = ((MouseEventArgs)e).Y;
                    temp.Name = "BO_" + numer;
                    temp.KeyDown += new KeyEventHandler(UsunBO);

                    tabBO.Add(temp);
                    //flowLayoutPanel1.Controls.Add(tabBO.Last());
                    
                    ile++;
                if (ctrl != true)
                {
                    klik = false;
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (klik == true)
            {
                BlokObliczeniowy temp = new BlokObliczeniowy();
                temp.Left = ((MouseEventArgs)e).X;
                temp.Top = ((MouseEventArgs)e).Y;
                temp.Name = "BO_" + numer;
                temp.KeyDown += new KeyEventHandler(UsunBO);

                numer++;
                tabBO.Add(temp);
                panel1.Controls.Add(tabBO.Last());

                /*if (ile > 0)
                {
                    tabBO[ile - 1].prNext = tabBO[ile].Name;
                    tabBO[ile - 1].prNext_ref = tabBO[ile];
                    tabBO[ile - 1].Update();



                    foreach (BlokObliczeniowy bo in panel1.Controls)
                    {
                        if (bo.prNext.Length > 0)
                        {
                            Pen p = new Pen(Color.Black, 1);
                            p.EndCap = LineCap.ArrowAnchor;

                            g.DrawLine(p, bo.Location.X + 75, bo.Location.Y + 75, bo.prNext_ref.Location.X + 75, bo.prNext_ref.Location.Y);
                        }
                    }
                }
                else
                {
                    temp_BO = (BlokObliczeniowy)panel1.Controls[0];
                }
                */

                ile++;
                if (ctrl != true)
                {
                    klik = false;
                }
            }
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control c in panel1.Controls)
            {
                c.Update();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            if (ile > 0)
            {
                Form1.ActiveForm.Text = temp_BO.Name;
                /*if (temp_BO.prNext.Length > 0)
                {
                    temp_BO = temp_BO.prNext_ref;
                } */  
            }
        }
    }
}
