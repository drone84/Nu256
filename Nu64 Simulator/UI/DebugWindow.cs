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
    public partial class DebugWindow : Form
    {
        private const int REGISTER_COLUMN = 26;
        private const int MNEMONIC_COLUMN = 14;

        public DebugWindow()
        {
            InitializeComponent();
            instance = this;
        }

        public static DebugWindow instance = null;
        private Processor.CPU _cpu = null;
        private Kernel _kernel = null;

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
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            int pc1 = CPU.GetLongPC();
            CPU.ExecuteNext();
            int pc2 = pc1 + CPU.Opcode.Length;
            PrintStatus(pc1, pc2);

            PrintNextInstruction();
        }

        private void PrintNextInstruction()
        {
            OpCode oc = CPU.PreFetch();
            int start = CPU.GetLongPC();
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
        }

        string GetHeaderText()
        {
            StringBuilder s = new StringBuilder();
            s.Append(new string(' ', REGISTER_COLUMN));
            s.Append(Kernel.Monitor.GetRegisterHeader());
            return s.ToString();
        }

        const int MAX_LINES = 25;
        const int TRIM_LINES = 1;
        public void PrintStatus(int lastPC, int newPC)
        {
            if (messageText.Lines.Length == 0)
                PrintLine(GetHeaderText());
            else if (messageText.Lines.Length > MAX_LINES)
            {
                string[] tmp = new string[MAX_LINES - TRIM_LINES + 1];
                tmp[0] = GetHeaderText();
                Array.Copy(messageText.Lines, messageText.Lines.Length - MAX_LINES + TRIM_LINES, tmp, 1, tmp.Length - 1);
                messageText.Lines = tmp;
            }


            PrintClear();
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
            UpdateStackDisplay();
        }

        public static void Print(string message)
        {
            lineBuffer.Append(message);
            instance.lastLine.Text = lineBuffer.ToString();
        }

        public static void PrintLine(string message)
        {
            lineBuffer.AppendLine(message);
            PrintLine();
        }

        public static void PrintClear()
        {
            lineBuffer.Clear();
        }

        public static void PrintLine()
        {
            instance.messageText.AppendText(lineBuffer.ToString());
            lineBuffer.Clear();
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

        private void RunButton_Click(object sender, EventArgs e)
        {
            CPU.DebugPause = false;
            while(! CPU.DebugPause && !CPU.Halted)
            {
                StepButton_Click(sender, e);
                Kernel.gpu.Refresh();
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}
