using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace libbloki
{
    public partial class BlokObliczeniowy : Bloki
    {
        //private BOOpcje frmOpcje;

        public void DodajDzialanie(Dzialanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Add(dzialanie);
        }

        public void UsunDzialanie(Dzialanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Remove(dzialanie);
        }
        
        public BlokObliczeniowy()
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

        public void AktualizujTXT()
        {
            if (this.dzialania != null)
            {
                this.txtHint.Active = false;
                
                this.txt.Text = "";
                int i=0;
                String tempString="";
                foreach (Dzialanie d in this.dzialania)
                {
                    tempString += znacznikZmiennej + d.lewa.ToString() + znacznikZmiennej + " "
                                    + d.dzialanie1 + d.srodek.ToString() +
                                    " " + d.dzialanie2 + " " + d.prawa +"\n";
                    i++;
                    if(i>=3)
                    {
                        break;
                    }
                }
                this.txt.Text = tempString.ToString();
                

                if (this.dzialania.Count > 3) 
                {
                    tempString = "";
                    this.txt.Text += "...";
                    foreach (Dzialanie d in this.dzialania)
                    {
                        tempString += znacznikZmiennej + d.lewa.ToString() + znacznikZmiennej + " "
                                    + d.dzialanie1 + d.srodek.ToString() +
                                      " " + d.dzialanie2 + " " + d.prawa +"\n";
                    }
                    this.txtHint.Active = true;
                    txtHint.SetToolTip(txt, tempString);
                    txtHint.SetToolTip(this, tempString);

                }
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

            txt.BackColor = brush.Color;
            txt.Refresh();

            AktualizujTXT();

            Pen pn = new Pen(Color.Brown,2);
            Rectangle rect = new Rectangle(2, 2, 148, 73);
            g.DrawRectangle(pn, rect);
            g.FillRectangle(brush, rect);

            brush.Dispose();
            pn.Dispose();
            g = null;
        }

        //private void UserControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    frmOpcje = new BOOpcje(this);
        //    frmOpcje.ShowDialog(this);

        //    Pen pn = new Pen(Color.Brown, 2);
        //    Rectangle rect = new Rectangle(1, 1, 150, 75);
        //    graph.DrawRectangle(pn, rect);
        //    graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
        //    Font fnt = new Font("Verdana", 8);
        //}

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        public void Wykonaj()
        {
            for (int i = 0; i < dzialania.Count; i++)
            {
                int index = ZnajdzZmienna(dzialania[i].lewa);
                
                if (index < 0)
                    return;

                Zmienna temp = listaZmiennych[index];

                if (temp.typ == typeof(int))
                {
                    int tmpL;
                    if (dzialania[i].srodekZmienna == true)
                        tmpL = Convert.ToInt32(listaZmiennych[ZnajdzZmienna(dzialania[i].srodek)].wartosc);
                    else
                        tmpL = Convert.ToInt32(dzialania[i].srodek);

                    if (dzialania[i].dzialanie2 != null)
                    {
                        switch (dzialania[i].dzialanie2)
                        {
                            case "+":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL += Convert.ToInt32(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL += Convert.ToInt32(dzialania[i].prawa);
                                break;
                            case "-":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL -= Convert.ToInt32(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL -= Convert.ToInt32(dzialania[i].prawa);
                                break;
                            case "*":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL *= Convert.ToInt32(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL *= Convert.ToInt32(dzialania[i].prawa);
                                break;
                            case "/":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL /= Convert.ToInt32(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL /= Convert.ToInt32(dzialania[i].prawa);
                                break;
                        }
                    }

                    temp.wartosc = tmpL.ToString();
                    continue;
                }

                if (temp.typ == typeof(double))
                {
                    double tmpL;

                    if (dzialania[i].srodekZmienna == true)
                        tmpL = Convert.ToDouble(listaZmiennych[ZnajdzZmienna(dzialania[i].srodek)].wartosc);
                    else
                        tmpL = Convert.ToDouble(dzialania[i].srodek);

                    if (dzialania[i].dzialanie2 != null)
                    {
                        switch (dzialania[i].dzialanie2)
                        {
                            case "+":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL += Convert.ToDouble(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL += Convert.ToDouble(dzialania[i].prawa);
                                break;
                            case "-":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL -= Convert.ToDouble(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL -= Convert.ToDouble(dzialania[i].prawa);
                                break;
                            case "*":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL *= Convert.ToDouble(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL *= Convert.ToDouble(dzialania[i].prawa);
                                break;
                            case "/":
                                if (dzialania[i].prawaZmienna == true)
                                    tmpL /= Convert.ToDouble(listaZmiennych[ZnajdzZmienna(dzialania[i].prawa)].wartosc);
                                else
                                    tmpL /= Convert.ToDouble(dzialania[i].prawa);
                                break;
                        }
                    }

                    temp.wartosc = tmpL.ToString();
                    continue;
                }

                if (temp.typ == typeof(String))
                {
                    String tmpL = dzialania[i].srodek.ToString();

                    if (dzialania[i].dzialanie2 != null)
                    {
                        switch (dzialania[i].dzialanie2)
                        {
                            case "+":
                                tmpL += dzialania[i].prawa;
                                break;
                            case "-":
                                tmpL = tmpL.Replace(dzialania[i].prawa, "");
                                break;
                            case "*":
                                break;
                            case "/":
                                break;
                        }
                    }

                    temp.wartosc = tmpL.ToString();
                    continue;
                }
            }
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
    }
}
