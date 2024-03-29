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
    public partial class BlokSTOP : Bloki
    {
        //Graphics graph;

        //private Point klikoffset;
        //private bool klik;

        public BlokSTOP()
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
            Rectangle rect = new Rectangle(1, 1, 150, 75);
            g.DrawEllipse(pn, rect);
            g.FillEllipse(brush, rect);
            g.DrawString("STOP", fnt, new SolidBrush(Color.Black), 40, 25);
        }

        /*private void BlokSTOP_MouseDown(object sender, MouseEventArgs e)
        {
            klik = true;
            klikoffset.X = e.X;
            klikoffset.Y = e.Y;
        }

        private void BlokSTOP_MouseMove(object sender, MouseEventArgs e)
        {
            if (klik)
            {
                this.Left = e.X + this.Left - klikoffset.X;
                this.Top = e.Y + this.Top - klikoffset.Y;
            }
        }

        private void BlokSTOP_MouseUp(object sender, MouseEventArgs e)
        {
            klik = false;
        }*/

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // przezroczyste tlo
        }
    }
}
