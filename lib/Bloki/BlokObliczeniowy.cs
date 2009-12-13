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
        private BOOpcje frmOpcje;

        public void DodajDzialanie(Działanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Add(dzialanie);
        }

        public void UsunDzialanie(Działanie dzialanie)
        {
            if (dzialanie != null)
                dzialania.Remove(dzialanie);
        }
        
        public BlokObliczeniowy()
        {
            InitializeComponent();
            graph = CreateGraphics();

            txt.Visible = true;
            txt.ReadOnly = true;
            txt.Left = this.Left + 10;
            txt.Top = this.Top + 10;
            txt.Height = this.Height - 20;
            txt.Width = this.Width - 20;
            txt.Parent = this;
            txt.ScrollBars = RichTextBoxScrollBars.Both;            
            txt.BringToFront();
            txt.Enabled = false;
            txt.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(UserControl1_MouseDoubleClick);
            this.Controls.Add(txt);
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

            Pen pn = new Pen(Color.Brown,2);
            Rectangle rect = new Rectangle(2, 2, 148, 73);
            g.DrawRectangle(pn, rect);
            g.FillRectangle(brush, rect);

            brush.Dispose();
            pn.Dispose();
            g = null;
        }

        private void UserControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmOpcje = new BOOpcje(this);
            frmOpcje.ShowDialog(this);

            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            graph.DrawRectangle(pn, rect);
            graph.FillRectangle(new SolidBrush(Color.Wheat), rect);
            Font fnt = new Font("Verdana", 8);
            
            ReDrawText();
        }
        
        public void ReDrawText()
        {
            
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        private void Wykonaj()
        {
            for (int i = 0; i < dzialania.Count; i++)
            {
                int index = ZnajdzZmienna(dzialania[i].lewa);
                
                if (index < 0)
                    return;

                Zmienna temp = listaZmiennych[index];

                if (temp.typ == typeof(int))
                {
                    int tmpL = Convert.ToInt32(dzialania[i].srodek);

                    if (dzialania[i].dzialanie2 == null)
                        return;

                    switch (dzialania[i].dzialanie2)
                    {
                        case "+":
                            tmpL += Convert.ToInt32(dzialania[i].prawa);
                            break;
                        case "-":
                            tmpL -= Convert.ToInt32(dzialania[i].prawa);
                            break;
                        case "*":
                            tmpL *= Convert.ToInt32(dzialania[i].prawa);
                            break;
                        case "/":
                            tmpL /= Convert.ToInt32(dzialania[i].prawa);
                            break;
                    }

                    temp.wartosc = tmpL.ToString();
                    return;
                }

                if (temp.typ == typeof(double))
                {
                    double tmpL = Convert.ToDouble(dzialania[i].srodek);

                    if (dzialania[i].dzialanie2 == null)
                        return;

                    switch (dzialania[i].dzialanie2)
                    {
                        case "+":
                            tmpL += Convert.ToDouble(dzialania[i].prawa);
                            break;
                        case "-":
                            tmpL -= Convert.ToDouble(dzialania[i].prawa);
                            break;
                        case "*":
                            tmpL *= Convert.ToDouble(dzialania[i].prawa);
                            break;
                        case "/":
                            tmpL /= Convert.ToDouble(dzialania[i].prawa);
                            break;
                    }

                    temp.wartosc = tmpL.ToString();
                    return;
                }

                if (temp.typ == typeof(String))
                {
                    String tmpL = dzialania[i].srodek.ToString();

                    if (dzialania[i].dzialanie2 == null)
                        return;

                    switch (dzialania[i].dzialanie2)
                    {
                        case "+":
                            tmpL += dzialania[i].prawa;
                            break;
                        case "-":
                            tmpL.Replace(dzialania[i].prawa, "");
                            break;
                        case "*":
                            break;
                        case "/":
                            break;
                    }

                    temp.wartosc = tmpL.ToString();
                    return;
                }
            }
        }
    }
}
