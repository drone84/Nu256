using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nu64.UI
{
    public partial class RegisterWindow : Form
    {
        public RegisterWindow()
        {
            InitializeComponent();

            instance = this;
        }

        public static RegisterWindow instance = null;
        public static void Print(string message)
        {
            instance.messageText.AppendText(message);
        }

        public static void PrintLine(string message)
        {
            Print(message);
            Print("\r\n");
        }

        static StringBuilder lineBuffer = new StringBuilder();
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
    }
}
