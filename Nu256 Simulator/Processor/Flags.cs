﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Processor
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

        /// <summary>
        /// Swap the Carry and Emulation flags. Used by the XCE instruction.
        /// This sets the CPU's emulation mode based on the Carry flag. When
        /// in emulation mode, the CPU can only access 64KB of RAM, cannot use
        /// the PBR and DBR registers, and can only store 8-bit values in A, X, and Y.
        /// <para>To set the CPU to emulation mode, call SEC XCE</para>
        /// <para>To set the CPU to native mode, call CLC XCE</para>
        /// </summary>
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

        public virtual int CarryBit
        {
            get
            {
                return Carry ? 1 : 0;
            }
            set
            {
                Carry = value != 0;
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
            if (X.Width == 2)
                Negative = ((int)this.Value & 0x8000) == 0x8000;
            else
                Negative = (X.Value & 0x80) == 0x80;
        }

        public void SetN(int Value, int Width)
        {
            if (Width == 1 && (Value & 0x80) != 0)
                Negative = true;
            else if (Width == 2 && (Value & 0x8000) != 0)
                Negative = true;
        }

        public void SetNZ(Register Reg)
        {
            Zero = Reg.Value == 0;
            SetN(Reg);
        }

        public void SetNZ(int Value, int Width)
        {
            Zero = Value == 0;
            SetN(Value, Width);
        }

        public override void Reset()
        {
            Negative = false;
            oVerflow = false;
            Break = false;
            accMemWidth = false;
            indeXregisterwidth = false;
            Decimal = false;
            Irqdisable = false;
            Zero = false;
            Carry = false;
            Emulation = false;
        }

    }
}
