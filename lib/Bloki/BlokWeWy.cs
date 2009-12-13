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
        private BWeWyOpcje frmOpcje;
        //public IList<Działanie> dzialania = new List<Działanie>();

        public BlokWeWy()
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
            
            graph.DrawPolygon(pn, p);
            graph.FillPolygon(brush, p);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }

        private void BlokWeWy_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmOpcje = new BWeWyOpcje(this);
            frmOpcje.ShowDialog(this);
        }

        public void ReDrawText()
        {
            
        }
    }
}
