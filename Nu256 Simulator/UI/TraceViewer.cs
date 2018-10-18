using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nu256.Simulator.Processor;

namespace Nu256.Simulator.UI
{
    public partial class TraceViewer : UserControl
    {
        public TraceViewer()
        {
            InitializeComponent();
        }

        public CPUTrace Trace = null;

        private void TraceViewer_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(this.ForeColor);
            SolidBrush textBrush = new SolidBrush(this.ForeColor);
            Graphics g = e.Graphics;

            string header = CPUStep.Header;
            SizeF headerSize = g.MeasureString(header, this.Font);
            RectangleF r = new RectangleF(0, 2, this.ClientRectangle.Width, headerSize.Height);

            g.Clear(BackColor);
            g.DrawLine(pen, r.Left, r.Top, r.Right, r.Top);
            g.DrawString(header, this.Font, textBrush, r.Location);
            pen.Width = 2;
            g.DrawLine(pen, r.Left, r.Bottom, r.Right, r.Bottom);
            r.Y += r.Height + 2;

            if (Trace != null)
            {
                for (int i = 0; i < Trace.Count; i++)
                {
                    g.DrawString(Trace[i], this.Font, textBrush, r.Location);
                    r.Y += r.Height;
                }
            }
        }

        internal void Clear()
        {

        }
    }
}
