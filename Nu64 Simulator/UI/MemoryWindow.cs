using Nu64.Common;
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
    public partial class MemoryWindow : Form
    {
        public static MemoryWindow Instance = null;
        public IMappable Memory = null;
        public int StartAddress = 0;
        public int EndAddress = 0xFF;

        public MemoryWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void MemoryWindow_Load(object sender, EventArgs e)
        {

        }

        public void RefreshMemoryView()
        {
            StringBuilder s = new StringBuilder();
            if (Memory == null)
                return;
            MemoryText.Clear();
            for (int i = StartAddress; i <= EndAddress; i += 0x10)
            {
                s.Append(">");
                s.Append(i.ToString("X6"));
                s.Append("  ");
                for (int j = 0; j < 16; j++)
                {
                    s.Append(Memory.ReadByte(i + j).ToString("X2"));
                    if (j == 7 || j == 15)
                        s.Append(" ");
                }

                for (int j = 0; j < 16; j++)
                {
                    int c = Memory.ReadByte(i + j);
                    if (c < 32 || c > 127)
                        s.Append(".");
                    else
                        s.Append((char)c);
                    if (j == 7 || j == 15)
                        s.Append(" ");
                }
                s.AppendLine();
            }
            MemoryText.AppendText(s.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshMemoryView();
            timer1.Enabled = false;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            RefreshMemoryView();
        }

        private void StartAddressText_Validated(object sender, EventArgs e)
        {
            try
            {
                int len = EndAddress - StartAddress;
                StartAddress = Convert.ToInt32(StartAddressText.Text, 16);
                EndAddress = StartAddress + len;
                EndAddressText.Text = EndAddress.ToString("X6");
            }
            catch (System.FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void EndAddressText_Validated(object sender, EventArgs e)
        {
            try
            {
                StartAddress = Convert.ToInt32(StartAddressText.Text, 16);
            }
            catch (System.FormatException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
