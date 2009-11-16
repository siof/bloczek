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
    public partial class BlokWeWy : UserControl
    {
        private Point klikoffset;
        private bool klik;
        Graphics graph;

        public BlokWeWy()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        private void BlokWeWy_MouseDown(object sender, MouseEventArgs e)
        {
            klik = true;
            klikoffset.X = e.X;
            klikoffset.Y = e.Y;
        }

        private void BlokWeWy_MouseUp(object sender, MouseEventArgs e)
        {
            klik = false;
        }

        private void BlokWeWy_MouseMove(object sender, MouseEventArgs e)
        {
            if (klik)
            {
                this.Left = e.X + this.Left - klikoffset.X;
                this.Top = e.Y + this.Top - klikoffset.Y;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 16);
            Graphics g = pe.Graphics;
            Pen pn = new Pen(Color.Brown, 2);
            Rectangle rect = new Rectangle(20, 1, 170, 75);
            Point[] p = new Point[4];
            p[0].X = 20; p[0].Y = 2;
            p[1].X = 172; p[1].Y = 2;
            p[2].X = 152; p[2].Y = 75;
            p[3].X = 2; p[3].Y = 75;
            
            graph.DrawPolygon(pn, p);
            graph.FillPolygon(new SolidBrush(Color.Wheat), p);
        }
    }
}
