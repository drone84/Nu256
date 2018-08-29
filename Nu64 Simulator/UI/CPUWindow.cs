using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nu64.Processor;

namespace Nu64.UI
{
    public partial class CPUWindow : Form
    {
        private const int MNEMONIC_COLUMN = 22;
        private const int REGISTER_COLUMN = 34;
        private int StepCounter = 0;

        public CPUWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public static CPUWindow Instance = null;
        private Processor.CPU _cpu = null;
        private Kernel _kernel = null;

        List<string> PrintQueue = new List<string>();
        static StringBuilder lineBuffer = new StringBuilder();

        public CPU CPU
        {
            get
            {
                return this._cpu;
            }

            set
            {
                this._cpu = value;
                registerDisplay1.CPU = value;
            }
        }

        public Kernel Kernel
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


        public static void PrintChar(char c)
        {
            if (c == '\r')
            {
                PrintLine(lineBuffer.ToString());
                lineBuffer.Clear();
            }
            else if (c == '\n')
            {
                // do nothing
            }
            else
            {
                lineBuffer.Append(c);
            }
        }

        public static void PrintTab(int x)
        {
            while (lineBuffer.Length < x)
                lineBuffer.Append(" ");
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = true;
            timer1.Enabled = false;
            RefreshStatus();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
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
            UpdateStackDisplay();
            Kernel.gpu.Refresh();

            if (PrintQueue.Count > 5)
            {
                string[] lines = new string[messageText.Lines.Length + PrintQueue.Count];
                if (messageText.Lines.Length > 0)
                    Array.Copy(messageText.Lines, lines, messageText.Lines.Length);
                PrintQueue.CopyTo(lines, messageText.Lines.Length);
                messageText.Lines = lines;

                messageText.AppendText(" ");
            }
            else
            {
                for (int i = 0; i < PrintQueue.Count; i++)
                {
                    messageText.AppendText("\r\n");
                    messageText.AppendText(PrintQueue[i]);
                }
            }
            PrintQueue.Clear();
            PrintNextInstruction();
        }

        private void PrintNextInstruction()
        {
            OpCode oc = CPU.PreFetch();
            int start = CPU.GetLongPC();
            PrintPC(start);

            int end = start + oc.Length;
            for (int i = start; i < end; i++)
            {
                Print(CPU.Memory[i].ToString("X2"));
                Print(" ");
            }

            int s = CPU.ReadSignature(oc);
            PrintTab(MNEMONIC_COLUMN);
            Print(oc.ToString(s));
            //PrintTab(REGISTER_COLUMN);
            //Print(Kernel.Monitor.GetRegisterText());
            Instance.lastLine.Text = lineBuffer.ToString();
            lineBuffer.Clear();
        }

        string GetHeaderText()
        {
            StringBuilder s = new StringBuilder();
            s.Append("PC    INSTRUCTION");
            s.Append(new string(' ', REGISTER_COLUMN - s.Length));
            s.Append(Kernel.Monitor.GetRegisterHeader());
            return s.ToString();
        }

        const int MAX_LINES = 25;
        const int TRIM_LINES = 1;
        private void PrintPC(int pc1)
        {
            Print("." + pc1.ToString("X6") + "  ");
        }

        public void PrintStatus(int lastPC, int newPC)
        {
            for (int i = lastPC; i < newPC; i++)
            {
                Print(CPU.Memory[i].ToString("X2"));
                Print(" ");
            }
            PrintTab(MNEMONIC_COLUMN);
            Print(CPU.Opcode.ToString(CPU.SignatureBytes));
            PrintTab(REGISTER_COLUMN);
            Print(Kernel.Monitor.GetRegisterText());
            PrintLine();
        }

        public static void Print(string message)
        {
            lineBuffer.Append(message);
        }

        public static void PrintLine(string message)
        {
            lineBuffer.Append(message);
            PrintLine();
        }

        public static void PrintClear()
        {
            lineBuffer.Clear();
        }

        public static void PrintLine()
        {
            Instance.PrintQueue.Add(lineBuffer.ToString());
            PrintClear();
        }

        int TopOfStack = int.MinValue;
        public void UpdateStackDisplay()
        {
            if (CPU.Stack.Value > TopOfStack)
                TopOfStack = CPU.Stack.Value;

            stackText.Clear();
            stackText.AppendText("Top: $" + TopOfStack.ToString("X4") + "\r\n");
            stackText.AppendText("SP : $" + CPU.Stack.Value.ToString("X4") + "\r\n");
            stackText.AppendText("N  : " + (TopOfStack - CPU.Stack.Value).ToString().PadLeft(4) + "\r\n");
            stackText.AppendText("───────────\r\n");
            for (int i = TopOfStack; i > CPU.Stack.Value; i--)
            {
                stackText.AppendText(i.ToString("X4") + " " + CPU.Memory[i].ToString("X2") + "\r\n");
            }

        }

        const int COUNTER_STEPS = 1000;
        private void RunButton_Click(object sender, EventArgs e)
        {
            RefreshStatus();
            int counter = COUNTER_STEPS;
            CPU.DebugPause = false;
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

                PrintClear();

                int pc1 = CPU.GetLongPC();
                PrintPC(pc1);
                CPU.ExecuteNext();
                int pc2 = pc1 + CPU.Opcode.Length;
                PrintStatus(pc1, pc2);

                if (Breakpoints.Items.Contains(Kernel.CPU.GetLongPC()))
                {
                    CPU.DebugPause = true;
                    timer1.Enabled = false;
                    Breakpoints.Text = Kernel.CPU.PC.Value.ToString("X6");
                }
            }
            catch (Exception ex)
            {
                Print(ex.Message);
                CPU.Halted = true;
            }
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            //if (messageText.Lines.Length == 0)
            //    PrintLine(GetHeaderText());
            PrintTab(REGISTER_COLUMN);
            HeaderTextbox.Text = GetHeaderText();
            PrintLine(Kernel.Monitor.GetRegisterText());
            RefreshStatus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (CPU.DebugPause || CPU.Halted)
                    break;
                ExecuteStep();
            }
            RefreshStatus();
        }

        int getbp()
        {
            int bp = 0;
            try
            {
                bp = Convert.ToInt32(Breakpoints.Text, 16);
                Breakpoints.Text = bp.ToString("X6");
            }
            catch (System.FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return bp;
        }

        private void AddBPButton_Click(object sender, EventArgs e)
        {
            int bp = getbp();
            if (bp > 0)
                Breakpoints.Items.Add(bp);
        }

        private void DeleteBPButton_Click(object sender, EventArgs e)
        {
            int bp = getbp();
            Breakpoints.Items.Remove(bp);
            Breakpoints.Text = Breakpoints.Items.Count.ToString();
        }

        private void MemoryButton_Click(object sender, EventArgs e)
        {

        }

        private void JumpButton_Click(object sender, EventArgs e)
        {

        }

        private void ClearTraceButton_Click(object sender, EventArgs e)
        {
            ResetTrace();
        }

        public void ResetTrace()
        {
            StepCounter = 0;
            messageText.Clear();
        }
    }
}
