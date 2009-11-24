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
  
 |

*/

namespace libbloki
{
    public partial class Pion : UserControl
    {
        Graphics graph;

        public Pion()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        public Pion(Panel parent)
        {
            InitializeComponent();
            graph = CreateGraphics();
            Parent = parent;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            int temp = this.Height;
            Graphics graph = pe.Graphics;
            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[2];
            p[0].X = 0; p[0].Y = 0;
            p[1].X = 0; p[1].Y = temp;

            graph.DrawLine(pn, p[0], p[1]);
        }
    }
}
