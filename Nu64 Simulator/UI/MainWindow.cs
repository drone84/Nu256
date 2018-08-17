using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nu64
{
    public partial class MainWindow : Form
    {
        public Kernel kernel;
        public UI.DebugWindow DebugWindow;
        public Timer BootTimer = new Timer();
        public int CyclesPerTick = 35000;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BasicWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            lastKeyPressed.Text = "$" + ((UInt16)e.KeyChar).ToString("X2");
            kernel.KeyboardBuffer.Add(e.KeyChar);
        }

        private void BasicWindow_Load(object sender, EventArgs e)
        {
            kernel = new Kernel(this.gpu);

            DebugWindow = new UI.DebugWindow();
            DebugWindow.CPU = kernel.CPU;
            kernel.CPU.DebugPause = true;
            DebugWindow.Kernel = kernel;
            DebugWindow.Show();

            BootTimer.Interval = 1000;
            BootTimer.Tick += BootTimer_Tick;
            BootTimer.Enabled = true;
            //kernel.READY();
        }

        private void BootTimer_Tick(object sender, EventArgs e)
        {
            BootTimer.Enabled = false;
            kernel.Reset();
        }

        private void BasicWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Up);
                    break;
                case Keys.Down:
                    kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Down);
                    break;
                case Keys.Left:
                    kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Left);
                    break;
                case Keys.Right:
                    kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Right);
                    break;
                case Keys.Home:
                    if (e.Shift)
                        kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Clear);
                    else if (e.Control)
                        kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Home);
                    else
                    {
                        kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Esc);
                        kernel.KeyboardBuffer.Add((char)PETSCIICommandCodes.Home);
                    }
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("KeyDown: " + e.KeyCode.ToString());
                    break;
            }
        }

        private void BasicWindow_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        double cps;
        private void timer1_Tick(object sender, EventArgs e)
        {
            kernel.CPU.ExecuteCycles(CyclesPerTick);

            TimeSpan s = kernel.CPU.CycleTime;
            int c = kernel.CPU.CycleCounter;

            cps = c / s.TotalSeconds;
        }

        private void performanceTimer_Tick(object sender, EventArgs e)
        {
            timerStatus.Text = cps.ToString("N0") + " CPS";
        }
    }
}
