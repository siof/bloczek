﻿using System;
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
        strzalkaLeftRight _zakonczenie;

        public strzalkaLeftRight zakonczenie
        {
            get { return _zakonczenie; }
            set { _zakonczenie = value; }
        }

        public LiniaPoz(strzalkaLeftRight zakonczenie)
        {
            InitializeComponent();
            graph = CreateGraphics();
            this.zakonczenie = zakonczenie;
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
            switch (zakonczenie)
            {
                case strzalkaLeftRight.left:
                            p[1].X = 2; p[1].Y = 2;
                            p[0].X = this.Width - 2; p[0].Y = 2;
                            pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            break;
                case strzalkaLeftRight.right:
                            p[0].X = 2; p[0].Y = 2;
                            p[1].X = this.Width - 2; p[1].Y = 2;
                            pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            break;

                default:
                            p[0].X = 2; p[0].Y = 2;
                            p[1].X = this.Width - 2; p[1].Y = 2;
                            pn.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                            break;

            }

            switch (tryb)
            {
                case tryby.zaznaczony:
                    pn.Color = Color.Red;
                    break;
                default:
                    pn.Color = Color.Black;
                    break;
            }

            graph.DrawLine(pn,p[0],p[1]);
            pn.Dispose();
        }

        private void LiniaPoz_Resize(object sender, EventArgs e)
        {
            if (graph == null)
                graph = CreateGraphics();

            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[2];
            switch (zakonczenie)
            {
                case strzalkaLeftRight.left:
                    p[1].X = 2; p[1].Y = 2;
                    p[0].X = this.Width - 2; p[0].Y = 2;
                    pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    break;
                case strzalkaLeftRight.right:
                    p[0].X = 2; p[0].Y = 2;
                    p[1].X = this.Width - 2; p[1].Y = 2;
                    pn.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    break;

                default:
                    p[0].X = 2; p[0].Y = 2;
                    p[1].X = this.Width - 2; p[1].Y = 2;
                    pn.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                    break;

            }

            switch (tryb)
            {
                case tryby.zaznaczony:
                    pn.Color = Color.Red;
                    break;
                default:
                    pn.Color = Color.Black;
                    break;
            }

            graph.DrawLine(pn, p[0], p[1]);
            pn.Dispose();
        }
    }
}
