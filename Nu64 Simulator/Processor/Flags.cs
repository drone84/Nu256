using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class Flags : Register8
    {
        //flags
        public bool Negative;
        public bool oVerflow;
        public bool Break;
        public bool accMemWidth;
        public bool indeXregisterwidth;
        public bool Decimal;
        public bool Irqdisable;
        public bool Zero;
        public bool Carry;
        public bool Emulation;

        public void SwapCE()
        {
            bool temp = Emulation;
            Emulation = Carry;
            Carry = temp;
        }

        public override int Value
        {
            get
            {
                if (Emulation)
                    return GetFlags(
                        Negative,
                        oVerflow,
                        Break,
                        false,
                        Decimal,
                        Irqdisable,
                        Zero,
                        Carry);
                else
                    return GetFlags(
                        Negative,
                        oVerflow,
                        accMemWidth,
                        indeXregisterwidth,
                        Decimal,
                        Irqdisable,
                        Zero,
                        Carry);
            }

            set
            {
                SetFlags(value);
            }
        }

        public UInt16 GetFlags(params bool[] flags)
        {
            UInt16 bits = 0;
            for (int i = 0; i < flags.Length; i++)
            {
                bits = (UInt16)(bits << 1);
                if (flags[i])
                    bits = (UInt16)(bits | 1);
            }

            return bits;
        }

        public void SetFlags(int value)
        {
            Negative = (value & 128) != 0;
            oVerflow = (value & 64) != 0;
            if (!Emulation)
                accMemWidth = (value & 32) != 0;
            else
                Break = (value & 32) != 0;
            indeXregisterwidth = (value & 16) != 0;
            Decimal = (value & 8) != 0;
            Irqdisable = (value & 4) != 0;
            Zero = (value & 2) != 0;
            Carry = (value & 1) != 0;
        }

        public override string ToString()
        {
            //NVMXDIZC
            if (Emulation)
                return (Negative ? "N" : "-")
                    + (oVerflow ? "V" : "-")
                    + (Break ? "B" : "-")
                    + "-"
                    + (Decimal ? "D" : "-")
                    + (Irqdisable ? "I" : "-")
                    + (Zero ? "Z" : "-")
                    + (Carry ? "C" : "-")
                    + " " + (Emulation ? "E" : " ");
            else
                return (Negative ? "N" : "-")
                    + (oVerflow ? "V" : "-")
                    + (accMemWidth ? "M" : "-")
                    + (indeXregisterwidth ? "X" : "-")
                    + (Decimal ? "D" : "-")
                    + (Irqdisable ? "I" : "-")
                    + (Zero ? "Z" : "-")
                    + (Carry ? "C" : "-")
                    + " " + (Emulation ? "E" : " ");
        }



        public void SetZ(int Val)
        {
            Zero = Val == 0;
        }

        public void SetZ(Register X)
        {
            Zero = X.Value == 0;
        }

        public void SetN(Register X)
        {
            if (X.Length == BitLengthEnum.Bits16)
                Negative = ((int)this.Value & 0x8000) == 0x8000;
            else
                Negative = (X.Value & 0x80) == 0x80;
        }

        public void SetN(int Value, BitLengthEnum Width)
        {
            if (Width == BitLengthEnum.Bits8 && (Value & 0x80) != 0)
                Negative = true;
            else if (Width == BitLengthEnum.Bits16 && (Value & 0x8000) != 0)
                Negative = true;
        }

        public void SetNZ(Register X)
        {
            Zero = X.Value == 0;
            SetN(X);
        }

        public void SetNZ(int Value, Register.BitLengthEnum Width)
        {
            Zero = Value == 0;
            SetN(Value, Width);
        }
    }
}
