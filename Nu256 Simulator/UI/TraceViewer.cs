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

        public int Scrollback
        {
            get
            {
                return CPUTrace.TRACE_STEPS_MIN - vScrollBar1.Maximum;
            }

            set
            {
                if (value >= 0 && value < CPUTrace.TRACE_STEPS_MIN)
                {
                    vScrollBar1.Value = vScrollBar1.Maximum - value;
                }
                this.Refresh();
            }
        }


        private Processor.CPU cpu = null;
        public Processor.CPU CPU
        {
            get
            {
                return this.cpu;
            }

            set
            {
                this.cpu = value;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = CPUTrace.TRACE_STEPS_MIN;
                vScrollBar1.Value = vScrollBar1.Maximum;
            }
        }

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
            r.Y = this.ClientRectangle.Height - r.Height;

            if (cpu != null && cpu.Trace != null && cpu.TraceEnabled == true)
            {

                cpu.TraceWait = true;
                int i = cpu.Trace.Count - Scrollback - 1;
                while(i >= 0 && r.Y > headerSize.Height)
                {
                    g.DrawString(cpu.Trace[i--], this.Font, textBrush, r.Location);
                    r.Y -= r.Height;
                }
                cpu.TraceWait = false;
            }
        }

        internal void Clear()
        {

        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
