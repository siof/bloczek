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
 |

*/

namespace libbloki
{
    public partial class NaroznikLG : UserControl
    {
        Graphics graph;

        public NaroznikLG()
        {
            InitializeComponent();
            graph = CreateGraphics();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            int temp = this.Width - 1;
            Graphics graph = pe.Graphics;
            Pen pn = new Pen(Color.Black, 1);
            Point[] p = new Point[3];
            p[0].X = 0; p[0].Y = 0;
            p[1].X = temp; p[1].Y = 0;
            p[2].X = 0; p[2].Y = temp;

            graph.DrawLine(pn, p[0], p[1]);
            graph.DrawLine(pn, p[0], p[2]);
        }
    }
}
