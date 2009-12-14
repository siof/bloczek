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
        //private BDOpcje frmOpcje;
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

        //private void BlokDecyzyjny_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    frmOpcje = new BDOpcje(this);
        //    frmOpcje.ShowDialog(this);
        //}

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

        public bool Wykonaj()
        {
            int lZm, pZm;
            bool aktualnyStan = false;
            
            for (int i = 0; i < dzialania.Count; i++)
            {
                bool tymczasowyStan = false;

                pZm = -1;
                lZm = ZnajdzZmienna(dzialania[i].lewa);
                
                if (dzialania[i].srodekZmienna == true)
                    pZm = ZnajdzZmienna(dzialania[i].srodek);

                if (listaZmiennych[lZm].typ == typeof(int))
                {
                    int tmp = Convert.ToInt32(listaZmiennych[lZm].wartosc);
                    int tmp2 = 0;

                    if (pZm != -1)
                    {
                        tmp2 = Convert.ToInt32(listaZmiennych[pZm].wartosc);
                    }
                    else
                    {
                        tmp2 = Convert.ToInt32(dzialania[i].srodek);
                    }

                    switch (dzialania[i].dzialanie1)
                    {
                        case "=":
                            if (tmp == tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;
                        
                        case ">" :
                            if (tmp > tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case "<":
                            if (tmp < tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case ">=":
                            if (tmp >= tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;
                        
                        case "<=":
                            if (tmp <= tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;
                        
                        case "<>":
                            if (tmp != tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;
                    }
                }

                if (listaZmiennych[lZm].typ == typeof(double))
                {
                    double tmp = Convert.ToDouble(listaZmiennych[lZm].wartosc);
                    double tmp2 = 0;

                    if (pZm != -1)
                    {
                        tmp2 = Convert.ToDouble(listaZmiennych[pZm].wartosc);
                    }
                    else
                    {
                        tmp2 = Convert.ToDouble(dzialania[i].srodek);
                    }

                    switch (dzialania[i].dzialanie1)
                    {
                        case "=":
                            if (tmp == tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case ">":
                            if (tmp > tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case "<":
                            if (tmp < tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case ">=":
                            if (tmp >= tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case "<=":
                            if (tmp <= tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case "<>":
                            if (tmp != tmp2)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;
                    }
                }

                if (listaZmiennych[lZm].typ == typeof(String))
                {
                    String tmp = listaZmiennych[lZm].wartosc.ToString();
                    String tmp2 = "";

                    if (pZm != -1)
                        tmp2 = listaZmiennych[pZm].wartosc.ToString();
                    else
                        tmp2 = dzialania[i].srodek.ToString();

                    switch (dzialania[i].dzialanie1)
                    {
                        case "=":
                            if (tmp.Equals(tmp2) == true)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        case "<>":
                            if (tmp.Equals(tmp2) == false)
                                tymczasowyStan = true;
                            else
                                tymczasowyStan = false;
                            break;

                        default:
                            tymczasowyStan = false;
                            break;
                    }
                }

                if (dzialania[i].dodatkowe != null)
                {
                    switch (dzialania[i].dodatkowe)
                    {
                        case "AND":
                            if (aktualnyStan == true && tymczasowyStan == true)
                                aktualnyStan = true;
                            else
                                aktualnyStan = false;
                            break;

                        case "OR":
                            if (aktualnyStan == false && tymczasowyStan == false)
                                aktualnyStan = false;
                            else
                                aktualnyStan = true;
                            break;

                        default:
                            aktualnyStan = tymczasowyStan;
                            break;
                    }
                }
                else
                    aktualnyStan = tymczasowyStan;
            }

            return aktualnyStan;
        }
    }
}
