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
    public partial class LiniaPoz : Bloki
    {
        //private Point klikoffset;
        //private bool klik;
        //Graphics graph;

        public LiniaPoz()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        //private void Linia_MouseDown(object sender, MouseEventArgs e)
        //{
        //    klik = true;
        //    klikoffset.X = e.X;
        //    klikoffset.Y = e.Y;
        //}

        //private void Linia_MouseUp(object sender, MouseEventArgs e)
        //{
        //    klik = false;
        //}

        //private void Linia_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (klik)
        //    {
        //        this.Left = e.X + this.Left - klikoffset.X;
        //        this.Top = e.Y + this.Top - klikoffset.Y;
        //    }
        //}

        protected override void OnPaint(PaintEventArgs pe)
        {
           
            Graphics graph = pe.Graphics;
            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[2];
            p[0].X = 2; p[0].Y = 2;
            p[1].X = this.Width -2; p[1].Y = 2;
            pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            graph.DrawLine(pn,p[0],p[1]);
        }

        private void LiniaPoz_Resize(object sender, EventArgs e)
        {
            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[2];
            p[0].X = 2; p[0].Y = 2;
            p[1].X = this.Width -2; p[1].Y = 2;
            pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            graph.DrawLine(pn, p[0], p[1]);
            pn.Dispose();
        }
    }
}
