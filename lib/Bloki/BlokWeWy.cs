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
                foreach (Dzialanie d in this.dzialania)
                {
                    tempString += d.dzialanie1 + ": " + d.srodek.ToString() + "\n";
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
                    foreach (Dzialanie d in this.dzialania)
                    {
                        tempString += d.dzialanie1 + ": " + d.srodek.ToString() + "\n";
                    }
                    this.txtHint.Active = true;
                    txtHint.SetToolTip(txt, tempString);
                    txtHint.SetToolTip(this, tempString);
                }
            }
        }

        public void Wykonaj(Console usr)
        {
            frmConsole = usr;
            int temp = -1;
            
            for (int i = 0; i < dzialania.Count; i++)
            {
                if (dzialania[i].srodekZmienna == true)
                    temp = ZnajdzZmienna(dzialania[i].srodek);

                if (dzialania[i].dzialanie1 == "Wypisz")
                {
                    String tmpString;
                    if (dzialania[i].srodek != null)
                    {
                        tmpString = dzialania[i].srodek.ToString();
                        //najpierw podmien wszystkie zmienne
                        foreach (Zmienna zm in this.listaZmiennych)
                        {
                            if (zm.tablica == false)
                                tmpString = tmpString.Replace(znacznikZmiennej + zm.nazwa + znacznikZmiennej, zm.wartosc.ToString());
                        }
                        //nastepnie podmien wszystkie elementy tablic
                        foreach (Zmienna zm in this.listaZmiennych)
                        {
                            if (zm.tablica == true)
                            {
                                if (tmpString.Contains(zm.nazwa + '[') == true)
                                {
                                    //znajdz indexy do odczytania elementu tablicy
                                    int tmpInd1 = tmpString.IndexOf(znacznikZmiennej + zm.nazwa + '[') + 2;
                                    int tmpInd2 = 0;
                                    int tmpNumerElementu = 0;
                                    for (int j = tmpInd1; j < tmpString.Length; j++)
                                    {
                                        //if (tmpString[j] == '[')    
                                        //    tmpInd1 = j;

                                        if (tmpString[j] == ']')
                                        {
                                            tmpInd2 = j;
                                            break;
                                        }
                                    }
                                    //odczytaj ktory element tablicy wypisac
                                    if (tmpInd2 - tmpInd1 > 0)
                                        tmpNumerElementu = NumerElementuWTablicy(tmpString.Substring(tmpInd1, tmpInd2 - tmpInd1 + 1));
                                    //
                                    tmpString = tmpString.Replace(znacznikZmiennej + zm.nazwa + "[" + (tmpNumerElementu+1).ToString() + "]" + znacznikZmiennej, zm.wartosci[tmpNumerElementu].ToString());
                                }
                            }
                        }
                        frmConsole.richTextBox1.Text += tmpString + '\n';
                    }
                }
                else
                {
                    int tmpNumerEl = 0;
                    Zmienna tmpZmienna = null;
                    if (dzialania[i].srodekZmienna == true)
                    {
                        temp = ZnajdzZmienna(dzialania[i].srodek.ToString());
                        tmpZmienna = listaZmiennych[temp];
                        
                        if (tmpZmienna.tablica == true)
                            tmpNumerEl = NumerElementuWTablicy(dzialania[i].srodek);

                        tmpZmienna = null;
                    }

                    Czytaj tmpOkno = new Czytaj(listaZmiennych, temp, tmpNumerEl);
                    
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
                    
                    //listaZmiennych[i].wartosc = tmpString.ToString();
                }
            }
        }
    }
}
