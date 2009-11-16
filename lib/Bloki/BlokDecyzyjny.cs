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
    public partial class BlokDecyzyjny : UserControl
    {
        Graphics graph;

        private Point klikoffset;
        private bool klik;

        public BlokDecyzyjny()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font fnt = new Font("Verdana", 8);
            Graphics g = pe.Graphics;
            Pen pn = new Pen(Color.Brown, 2);
           // Rectangle rect = new Rectangle(1, 1, 150, 75);
            Point[] p = new Point[4];
            p[0].X = 92; p[0].Y = 2;
            p[1].X = 177; p[1].Y = 37;
            p[2].X = 92; p[2].Y = 75;
            p[3].X = 19; p[3].Y = 37;

            g.DrawPolygon(pn, p);
            g.FillPolygon(new SolidBrush(Color.Wheat), p);
            g.DrawString("TAK", fnt, new SolidBrush(Color.Black), 170, 20);
            g.DrawString("NIE", fnt, new SolidBrush(Color.Black), 2, 20);
        }

        private void BlokDecyzyjny_MouseDown(object sender, MouseEventArgs e)
        {
            klik = true;
            klikoffset.X = e.X;
            klikoffset.Y = e.Y;
        }

        private void BlokDecyzyjny_MouseUp(object sender, MouseEventArgs e)
        {
            klik = false;
        }

        private void BlokDecyzyjny_MouseMove(object sender, MouseEventArgs e)
        {
            if (klik)
            {
                this.Left = e.X + this.Left - klikoffset.X;
                this.Top = e.Y + this.Top - klikoffset.Y;
            }
        }
    }
}
