using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nu256.Simulator.Processor;

namespace Nu256.Simulator.UI
{
    public partial class CPUWindow : Form
    {
        private const int MNEMONIC_COLUMN = 22;
        private const int REGISTER_COLUMN = 34;
        private int StepCounter = 0;
        Processor.Breakpoints breakpoints = null;

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public static CPUWindow Instance = null;
        private Processor.CPU cpu = null;
        private NuSystem _kernel = null;

        public CPU CPU
        {
            get
            {
                return this.cpu;
            }

            set
            {
                this.cpu = value;
                if (cpu != null)
                {
                    registerDisplay1.CPU = cpu;
                    this.breakpoints = cpu.Breakpoints;
                    traceViewer1.Trace = cpu.Trace;
                }
            }
        }

        public NuSystem Kernel
        {
            get
            {
                return this._kernel;
            }

            set
            {
                this._kernel = value;
            }
        }


        private void PauseButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            timer1.Enabled = false;
            RefreshStatus();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            CPUTraceCheckBox.Checked = true;
            CPU.DebugPause = true;
            timer1.Enabled = false;

            int steps = 1;
            int.TryParse(stepsInput.Text, out steps);
            Kernel.CPU.DebugPause = false;
            while (!Kernel.CPU.DebugPause && steps-- > 0)
            {
                ExecuteStep();
                Application.DoEvents();
            }
            RefreshStatus();
            Kernel.CPU.DebugPause = true;
        }

        private void RefreshStatus()
        {
            this.Text = "Debug: " + StepCounter.ToString();
            traceViewer1.Refresh();
            UpdateStackDisplay();
            Kernel.gpu.Refresh();
        }

        int TopOfStack = 0xd6ff;
        public void UpdateStackDisplay()
        {
            if (CPU.Stack.Value > TopOfStack)
                TopOfStack = CPU.Stack.Value;

            stackText.Clear();
            stackText.AppendText("Top: $" + TopOfStack.ToString("X4") + "\r\n");
            stackText.AppendText("SP : $" + CPU.Stack.Value.ToString("X4") + "\r\n");
            stackText.AppendText("N  : " + (TopOfStack - CPU.Stack.Value).ToString().PadLeft(4) + "\r\n");
            stackText.AppendText("───────────\r\n");

            int i = TopOfStack;
            if (CPU.Stack.Value == 0)
                i = 0;
            else if (CPU.Stack.Value - i > 1000)
                i = CPU.Stack.Value - 1000;
            while (i > CPU.Stack.Value)
            {
                stackText.AppendText(i.ToString("X4") + " " + CPU.Memory[i].ToString("X2") + "\r\n");
                i--;
            }

        }

        const int COUNTER_STEPS = 1000;
        private void RunButton_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            cpu.DebugPause = false;
            cpu.Run();
            timer1.Enabled = true;
        }

        private void locationInput_TextChanged(object sender, EventArgs e)
        {
        }

        private void stepsInput_Enter(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
                return;

            tb.SelectAll();
        }

        public void ExecuteStep()
        {
            try
            {
                StepCounter++;

                CPU.ExecuteNext();
                int pc = CPU.GetLongPC();
                if (breakpoints.ContainsKey(pc))
                {
                    CPU.DebugPause = true;
                    timer1.Enabled = false;
                    BPCombo.Text = breakpoints.GetHex(pc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                CPU.Halt();
            }
        }

        private void CPUWindow_Load(object sender, EventArgs e)
        {
            //if (messageText.Lines.Length == 0)
            //    PrintLine(GetHeaderText());
            //PrintLine(Kernel.Monitor.GetRegisterText());
            ClearTrace();
            RefreshStatus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshStatus();
        }

        private void RefreshBreakpoints()
        {
            BPCombo.Items.Clear();
            foreach (string s in breakpoints.Values)
            {
                BPCombo.Items.Add(s);
            }
            BPLabel.Text = breakpoints.Count.ToString() + " Breakpoints";
        }

        private void AddBPButton_Click(object sender, EventArgs e)
        {
            breakpoints.Add(BPCombo.Text);
            BPCombo.Text = breakpoints.Format(BPCombo.Text);
            RefreshBreakpoints();
        }

        private void DeleteBPButton_Click(object sender, EventArgs e)
        {
            if (BPCombo.Text != "")
                breakpoints.Remove(BPCombo.Text);
            if (breakpoints.Count == 0)
                BPCombo.Text = "";
            else
                BPCombo.Text = breakpoints.Values[0];
            RefreshBreakpoints();
        }

        private void MemoryButton_Click(object sender, EventArgs e)
        {

        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            int pc = breakpoints.GetIntFromHex(locationInput.Text);
            CPU.SetLongPC(pc);
            ClearTrace();
            CPU.ExecuteNext();
        }

        private void ClearTraceButton_Click(object sender, EventArgs e)
        {
            ClearTrace();
        }

        public void ClearTrace()
        {
            StepCounter = 0;
            traceViewer1.Clear();
        }

        private void CPUTraceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CPU.TraceEnabled = CPUTraceCheckBox.Checked;
        }
    }
}
