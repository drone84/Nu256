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
        public UI.RegisterWindow RegisterWindow;

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

            RegisterWindow = new UI.RegisterWindow();
            RegisterWindow.registerDisplay1.CPU = kernel.CPU;
            RegisterWindow.Show();

            kernel.READY();
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
    }
}
