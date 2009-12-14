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
    public partial class BlokDecyzyjny : Bloki
    {
        private BDOpcje frmOpcje;
        //public IList<Działanie> dzialania = new List<Działanie>();

        public BlokDecyzyjny()
        {
            InitializeComponent();
            graph = CreateGraphics();
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
            Font fnt = new Font("Verdana", 8);
            Graphics g = pe.Graphics;
            SolidBrush brush = new SolidBrush(Color.Black);
            switch (tryb)
            {
                case tryby.normal: brush.Color = Color.Wheat; break;
                case tryby.zaznaczony: brush.Color = Color.Orange; break;
                case tryby.aktualny: brush.Color = Color.Red; break;
            }

            Pen pn = new Pen(Color.Brown, 2);
           // Rectangle rect = new Rectangle(1, 1, 150, 75);
            Point[] p = new Point[4];
            p[0].X = 92; p[0].Y = 2;
            p[1].X = 177; p[1].Y = 37;
            p[2].X = 92; p[2].Y = 75;
            p[3].X = 19; p[3].Y = 37;

            txt.BackColor = brush.Color;
            txt.Refresh();

            AktualizujTXT();

            g.DrawPolygon(pn, p);
            g.FillPolygon(brush, p);
            g.DrawString("TAK", fnt, new SolidBrush(Color.Black), 170, 20);
            g.DrawString("NIE", fnt, new SolidBrush(Color.Black), 2, 20);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        private void BlokDecyzyjny_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmOpcje = new BDOpcje(this);
            frmOpcje.ShowDialog(this);
        }

        public void ReDrawText()
        {

        }

        private void txt_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        private void txt_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void txt_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void txt_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        public void AktualizujTXT()
        {
            if (this.dzialania != null)
            {
                this.txtHint.Active = false;

                this.txt.Text = "";
                int j,i = 0;
                String tempString = "";
                if (this.dzialania.Count > 2)
                {
                    j = 1;
                }
                else
                {
                    j = 2;
                }
                foreach (Działanie d in this.dzialania)
                {
                    tempString += d.dodatkowe +" " + d.lewa.ToString() +" "+ d.dzialanie1 +" "+ d.srodek.ToString() + "\n";
                    i++;
                    if (i >=j)
                    {
                        break;
                    }
                }
                this.txt.Text = tempString.ToString();


                if (this.dzialania.Count > 2)
                {
                    tempString = "";
                    this.txt.Text += "...";
                    foreach (Działanie d in this.dzialania)
                    {
                        tempString += d.dodatkowe + " " + d.lewa.ToString() +" "+ d.dzialanie1 +" "+ d.srodek.ToString() + "\n";
                    }
                    this.txtHint.Active = true;
                    txtHint.SetToolTip(txt, tempString);
                    txtHint.SetToolTip(this, tempString);

                }
            }
        }
    }
}
