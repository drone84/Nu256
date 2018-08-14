using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64;

namespace Nu64.Monitor
{
    public class Monitor : ReadyHandler
    {
        public Kernel kernel = null;

        char commmand = ' ';
        int[] args = new int[5];

        public Monitor(Kernel NewKernel)
        {
            this.kernel = NewKernel;
        }

        public void Ready()
        {
            PrintRegisters();
        }

        public void PrintRegisters(bool printHeader = true)
        {
            //  PC     A    X    Y    SP   DBR DP   NVMXDIZC
            // ;000000 0000 0000 0000 0000 00  0000 11111111

            if (printHeader)
                PrintRegisterHeader();
            kernel.PrintChar(';');
            kernel.Print(kernel.CPU.GetLongPC().ToString("X6"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.A.Value.ToString("X4"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.X.Value.ToString("X4"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.Y.Value.ToString("X4"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.Stack.Value.ToString("X4"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.DataBank.Value.ToString("X2"));
            kernel.PrintChar(' ');
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.DirectPage.Value.ToString("X4"));
            kernel.PrintChar(' ');
            kernel.Print(kernel.CPU.Flags.ToString());
            kernel.PrintLine();
        }

        public void PrintRegisterHeader()
        {
            kernel.PrintLine(" PC     A    X    Y    SP   DBR DP   NVMXDIZC");
        }

        public void PrintStoredRegisters()
        {
            //  PC     A    X    Y    SP   DBR DP   NVMXDIZC
            // ;000000 0000 0000 0000 0000 00  0000 11111111
            PrintRegisterHeader();
            kernel.PrintChar(';');
            kernel.PrintMemHex(3, MemoryMap_DirectPage.SYSPC);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap_DirectPage.SYSA);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap_DirectPage.SYSX);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap_DirectPage.SYSY);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap_DirectPage.SYSSP);
            kernel.PrintChar(' ');
            kernel.PrintMemHex(1, MemoryMap_DirectPage.SYSDBR);
            kernel.PrintChar(' ');
            kernel.PrintChar(' ');
            kernel.PrintMemHex(2, MemoryMap_DirectPage.SYSDP);
            kernel.PrintChar(' ');
            kernel.PrintMemBinary(1, MemoryMap_DirectPage.SYSP);
            kernel.PrintChar(' ');
            kernel.PrintLine();
        }

        public void ReturnPressed(int LineStart)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < kernel.gpu.Columns; i++)
            {
                s.Append(kernel.Memory[LineStart]+i);
            }

            Execute(s.ToString());
        }

        public void PrintGreeting()
        {
            kernel.PrintLine("         Machine Monitor v0.1 (dev)");
        }

        public void Execute(string Line)
        {
            commmand = ' ';
            int pos = 0;
            while (commmand == ' ' && pos < Line.Length)
            {
                commmand = Line[pos];
            }

            switch (commmand)
            {
                case '?':
                    kernel.PrintLine();
                    string s = System.IO.File.ReadAllText("Monitor\\Monitor Help.txt");
                    kernel.PrintLine(s);
                    break;
                case ' ':
                case '\0':
                    kernel.PrintLine();
                    break;
                default:
                    kernel.PrintLine();
                    kernel.PrintLine("Error (? for help)");
                    break;
            }
        }
    }
}
