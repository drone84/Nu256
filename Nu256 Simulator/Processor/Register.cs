﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.Processor
{
    public class Register
    {
        protected int _value;
        private int byteLength = 2;
        /// <summary>
        /// Forces the upper 8 bits to 0 when the register changes to 8 bit mode, or when writing or reading 
        /// the value in 8 bit mode. If this is false, the value is hidden, but preserved. If this is true, 
        /// the top 8 bits are destroyed when the width is set to 8 bits. 
        /// </summary>
        public bool DiscardUpper = false;
        string[] formatWidth = new string[] { "X", "X1", "X2", "X3", "X4" };

        //public enum int
        //{
        //    Bits8 = 1,
        //    Bits16 = 2,
        //    Bits24 = 3,
        //}

        public virtual int Value
        {
            get
            {
                if (byteLength == 1)
                    return (int)(this._value & 0xff);
                return this._value;
            }

            set
            {
                if (Width == 1)
                {
                    if (DiscardUpper)
                        this._value = (int)(value & 0xff);
                    else
                        this._value = (int)((value & 0xff) | (this._value & 0xff00));
                }
                else
                    this._value = value & 0xffff;
            }
        }

        public virtual int Low
        {
            get { return (int)(this._value & 0xff); }
            set { this.Value = (int)((this.Value & 0xff00) | (value & 0xff)); }
        }

        public virtual int High
        {
            get { return (int)((this._value & 0xff00) >> 8); }
            set { this.Value = (int)((this.Value & 0xff) | ((value & 0xff) << 8)); }
        }

        public virtual void Swap()
        {
            int l = Low;
            Low = High;
            High = l;
        }

        /// <summary>
        /// Register width in bytes. 1=8 bits, 2=16 bits
        /// </summary>
        public virtual int Width
        {
            get
            {
                return this.byteLength;
            }

            set
            {
                this.byteLength = value;
            }
        }

        public virtual int MinUnsigned
        {
            get
            {
                return 0;
            }
        }

        public virtual int MaxUnsigned
        {
            get
            {
                if (Width == 8)
                    return 0xff;
                return 0xffff;
            }
        }

        public virtual int MinSigned
        {
            get
            {
                if (Width == 8)
                    return -128;
                return -32768;
            }
        }

        public virtual int MaxSigned
        {
            get
            {
                if (Width == 8)
                    return 127;
                return 32767;
            }
        }

        public virtual void SetFromVector(int v)
        {
            throw new NotImplementedException();
        }

        public virtual bool GetZeroFlag()
        {
            return Value == 0;
        }

        public override string ToString()
        {
            switch (Width)
            {
                case 2:
                    return Value.ToString("X4");
                case 1:
                    return Value.ToString("X2");
                default:
                    return Value.ToString();
            }
        }

        /// <summary>
        /// Build a 24-bit address using this as a bank register
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual int GetLongAddress(int Address)
        {
            if (this.byteLength == 2)
                return (this.Value << 8) + Address;
            else if (this.byteLength == 1)
                return (this.Value << 16) + Address;
            else
                return this.Value;
        }

        /// <summary>
        /// Build a 24-bit address using this as a bank register
        /// </summary>
        /// <param name="Address">Register to use as address</param>
        /// <returns></returns>
        public virtual int GetLongAddress(Register Address)
        {
            if (this.byteLength == 2)
                return (this.Value << 8) + Address.Value;
            else if (this.byteLength == 1)
                return (this.Value << 16) + Address.Value;
            else
                return this.Value;
        }

        public virtual void Reset()
        {
            this.Value = 0;
            this.byteLength = 1;
        }
    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class Register16 : Register
    {
        public override int Width
        {
            get
            {
                return 2;
            }

            set
            {
                base.Width = 2;
            }
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAdddress(int address)
        {
            return (this.Value << 8) + address;
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAdddress(Register index)
        {
            return (this.Value << 8) + index.Value;
        }
    }

    /// <summary>
    /// Defines a register that is always 16 bits, such as the program counter or the Direct Page register.
    /// </summary>
    /// 
    /// <summary>
    /// 
    /// Defines an 8 bit register.
    /// </summary>
    public class Register8 : Register
    {
        public override int Width
        {
            get
            {
                return 1;
            }

            set
            {
                base.Width = 1;
            }
        }
    }

    public class RegisterBankNumber : Register8
    {
        /// <summary>
        /// Adds the 16-bit address in the register to this bank to get a 24-bit address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public virtual int GetLongAddress(Register16 Address)
        {
            return (this.Value << 16) + Address.Value;
        }

        /// <summary>
        /// Adds this bank register to a 16-bit address to form a 24-bit address
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public override int GetLongAddress(int Address)
        {
            return (this.Value << 16) + Address;
        }
    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class RegisterDirectPage : Register
    {
        public override int Width
        {
            get
            {
                return 2;
            }

            set
            {
                base.Width = 2;
            }
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public override int GetLongAddress(int address)
        {
            return this.Value + (address & 0Xff);
        }

    }

}
