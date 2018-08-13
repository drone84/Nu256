using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class Register
    {
        protected int _value;
        private BitLengthEnum bitLength = BitLengthEnum.Bits16;
        /// <summary>
        /// Forces the upper 8 bits to 0 when the register changes to 8 bit mode, or when writing or reading 
        /// the value in 8 bit mode. If this is false, the value is hidden, but preserved. If this is true, 
        /// the top 8 bits are destroyed when the width is set to 8 bits. 
        /// </summary>
        public bool DiscardUpper = false;

        public enum BitLengthEnum
        {
            Bits8 = 1,
            Bits16 = 2,
            Bits24 = 3,
        }

        public virtual int Value
        {
            get
            {
                if (bitLength == BitLengthEnum.Bits8)
                    return (int)(this._value & 0xff);
                return this._value;
            }

            set
            {
                if (Length == BitLengthEnum.Bits8)
                {
                    if (DiscardUpper)
                        this._value = (int)(value & 0xff);
                    else
                        this._value = (int)((value & 0xff) | (this._value & 0xff00));
                }
                else
                    this._value = value;
            }
        }

        public virtual int Low
        {
            get { return (int)(this._value & 0xff); }
            set { this.Value = (int)((this.Value & 0xff00) | (value & 0xff)); }
        }

        public virtual int High
        {
            get { return (int)(this._value & 0xff00 >> 8); }
            set { this.Value = (int)((this.Value & 0xff) | (value & 0xff << 8)); }
        }

        public virtual void Swap()
        {
            int l = Low;
            Low = High;
            High = l;
        }

        public virtual BitLengthEnum Length
        {
            get
            {
                return this.bitLength;
            }

            set
            {
                this.bitLength = value;
            }
        }

        public virtual int Bytes
        {
            get
            {
                if (this.bitLength == BitLengthEnum.Bits16)
                    return 2;
                return 1;
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
            switch (Length)
            {
                case Register.BitLengthEnum.Bits16:
                    return "$" + Value.ToString("X4");
                case Register.BitLengthEnum.Bits8:
                    return "$" + Value.ToString("X2");
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
            if (this.bitLength == BitLengthEnum.Bits16)
                return (this.Value << 8) + Address;
            else if (this.bitLength == BitLengthEnum.Bits8)
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
            if (this.bitLength == BitLengthEnum.Bits16)
                return (this.Value << 8) + Address.Value;
            else if (this.bitLength == BitLengthEnum.Bits8)
                return (this.Value << 16) + Address.Value;
            else
                return this.Value;
        }
    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class Register16 : Register
    {
        public override BitLengthEnum Length
        {
            get
            {
                return BitLengthEnum.Bits16;
            }

            set
            {
                base.Length = BitLengthEnum.Bits16;
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
            return this.Value << 8 + address;
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAdddress(Register index)
        {
            return this.Value << 8 + index.Value;
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
        public override BitLengthEnum Length
        {
            get
            {
                return BitLengthEnum.Bits8;
            }

            set
            {
                base.Length = BitLengthEnum.Bits8;
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
        public virtual int GetLongAddress(int Address)
        {
            return this.Value << 16 + Address;
        }
    }

    /// <summary>
    /// A register that is always 16 bits, such as the Direct Page register
    /// </summary>
    public class RegisterDirectPage : Register
    {
        public override BitLengthEnum Length
        {
            get
            {
                return BitLengthEnum.Bits16;
            }

            set
            {
                base.Length = BitLengthEnum.Bits16;
            }
        }

        /// <summary>
        /// Get a direct page address. Offsets the register's value by 8 bits, then adds 
        /// the supplied address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int GetLongAddress(int address)
        {
            return this.Value << 8 + address;
        }

    }

}
