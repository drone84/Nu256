using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.Processor
{
    public class CPUStep
    {
        public static int[] columns = new int[] { 8, 14, 14, 8, 5, 5, 5, 5, 4, 5, 10 };

        public string Address;
        public string Data;
        public string Instruction;
        public string PC;
        public string A;
        public string X;
        public string Y;
        public string SP;
        public string DBR;
        public string DP;
        public string Flags;

        public static string Header
        {
            get
            {
                StringBuilder s = new StringBuilder();
                s.Append(" Addr".PadRight(columns[0]));
                s.Append("Instruction".PadRight(columns[1]));
                s.Append("".PadRight(columns[2]));
                s.Append(" PC".PadRight(columns[3]));
                s.Append("A".PadRight(columns[4]));
                s.Append("X".PadRight(columns[5]));
                s.Append("Y".PadRight(columns[6]));
                s.Append("SP".PadRight(columns[7]));
                s.Append("DBR".PadRight(columns[8]));
                s.Append("DP".PadRight(columns[9]));
                s.Append("NVMXDIZC E".PadRight(columns[10]));

                return s.ToString();
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(".");
            s.Append(Address.PadRight(columns[0]));
            s.Append(Data.PadRight(columns[1]));
            s.Append(Instruction.PadRight(columns[2]));
            s.Append(";");
            s.Append(PC.PadRight(columns[3]));
            s.Append(A.PadRight(columns[4]));
            s.Append(X.PadRight(columns[5]));
            s.Append(Y.PadRight(columns[6]));
            s.Append(SP.PadRight(columns[7]));
            s.Append(DBR.PadRight(columns[8]));
            s.Append(DP.PadRight(columns[9]));
            s.Append(Flags.PadRight(columns[10]));

            return s.ToString();
        }
    }
}
