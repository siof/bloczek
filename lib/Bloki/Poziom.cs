using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
rysuje:
  _

*/

namespace libbloki
{
    public partial class Poziom : UserControl
    {
        Graphics graph;

        public Poziom()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        public Poziom(Panel parent)
        {
            InitializeComponent();
            graph = CreateGraphics();
            Parent = parent;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            int temp = this.Width;
            Graphics graph = pe.Graphics;
            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[2];
            p[0].X = 0; p[0].Y = 0;
            p[1].X = temp; p[1].Y = 0;

            graph.DrawLine(pn, p[0], p[1]);
        }
    }
}
