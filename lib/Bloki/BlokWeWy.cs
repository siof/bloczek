using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libbloki
{
    public partial class BlokWeWy : Bloki
    {
        //private BWeWyOpcje frmOpcje;
        private Console frmConsole;

        //public IList<Działanie> dzialania = new List<Działanie>();

        public BlokWeWy()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        public BlokWeWy(Console usr)
        {
            InitializeComponent();
            graph = CreateGraphics();
            frmConsole = usr;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);
            switch (tryb)
            {
                case tryby.normal: brush.Color = Color.Wheat; break;
                case tryby.zaznaczony: brush.Color = Color.Orange; break;
                case tryby.aktualny: brush.Color = Color.Red; break;
            }

            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(20, 1, 170, 75);
            Point[] p = new Point[4];
            p[0].X = 20; p[0].Y = 2;
            p[1].X = 172; p[1].Y = 2;
            p[2].X = 152; p[2].Y = 75;
            p[3].X = 2; p[3].Y = 75;

            txt.BackColor = brush.Color;
            txt.Refresh();

            AktualizujTXT();
            
            graph.DrawPolygon(pn, p);
            graph.FillPolygon(brush, p);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        //private void BlokWeWy_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    frmOpcje = new BWeWyOpcje(this);
        //    frmOpcje.ShowDialog(this);
        //}

        public void ReDrawText()
        {
            
        }

        private void txt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseDoubleClick(e);
        }

        private void txt_MouseDown(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseDown(e);
        }

        private void txt_MouseMove(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseMove(e);
        }

        private void txt_MouseUp(object sender, MouseEventArgs e)
        {
            sender = this;
            this.OnMouseUp(e);
        }

        public void AktualizujTXT()
        {
            if (this.dzialania != null)
            {
                this.txtHint.Active = false;

                this.txt.Text = "";
                int i = 0;
                String tempString = "";
                foreach (Działanie d in this.dzialania)
                {
                    tempString += d.dzialanie1 + ": \"" + d.srodek.ToString() + "\"\n";
                    i++;
                    if (i >= 3)
                    {
                        break;
                    }
                }
                this.txt.Text = tempString.ToString();


                if (this.dzialania.Count > 3)
                {
                    tempString = "";
                    this.txt.Text += "...";
                    foreach (Działanie d in this.dzialania)
                    {
                        tempString += d.dzialanie1 + ": \"" + d.srodek.ToString() + "\"\n";
                    }
                    this.txtHint.Active = true;
                    txtHint.SetToolTip(txt, tempString);
                    txtHint.SetToolTip(this, tempString);

                }
            }
        }

        public void Wykonaj()
        {
            int temp = -1;
            for (int i = 0; i < dzialania.Count; i++)
            {
                if (dzialania[i].srodekZmienna == true)
                    temp = ZnajdzZmienna(dzialania[i].srodek);

                if (dzialania[i].dzialanie1 == "Wypisz")
                {
                    if (dzialania[i].srodekZmienna == true)
                    {
                        frmConsole.richTextBox1.Text += listaZmiennych[temp].wartosc + '\n';
                    }
                    else
                    {
                        frmConsole.richTextBox1.Text += dzialania[i].srodek + '\n';
                    }
                }
                else
                {
                    String tmpString = "";
                    Czytaj tmpOkno = new Czytaj(tmpString);
                    tmpOkno.label1.Text = "Podaj " + dzialania[i].srodek + ":";

                    if (listaZmiennych[temp].typ == typeof(int))
                        tmpOkno.maskedTextBox1.Mask = "000000000000000";
                    else
                    {
                        if (listaZmiennych[temp].typ == typeof(double))
                            tmpOkno.maskedTextBox1.Mask = "000000000.0000";
                        else
                            tmpOkno.maskedTextBox1.Mask = "";
                    }

                    tmpOkno.ShowDialog();

                    listaZmiennych[i].wartosc = tmpString;
                }
            }
        }
    }
}
