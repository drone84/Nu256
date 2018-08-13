using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64;

namespace Nu64.Processor
{
    public class OpCode
    {
        public byte Value;
        public string Mnemonic;
        public AddressModes AddressMode;
        public delegate void ExecuteDelegate(byte Instruction, AddressModes AddressMode, int Signature);
        public event ExecuteDelegate ExecuteOp;
        public int Length8Bit;
        public Register ActionRegister = null;

        public OpCode(byte Value, string Mnemonic, int Length8Bit, Register ActionRegister, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            this.Length8Bit = Length8Bit;
            this.ActionRegister = ActionRegister;
            this.Mnemonic = Mnemonic;
            this.AddressMode = Mode;
            this.ExecuteOp += newDelegate;

            System.Diagnostics.Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public OpCode(byte Value, string Mnemonic, int Length, AddressModes Mode, ExecuteDelegate newDelegate)
        {
            this.Value = Value;
            this.Length8Bit = Length;
            this.Mnemonic = Mnemonic;
            this.AddressMode = Mode;
            this.ExecuteOp += newDelegate;

            System.Diagnostics.Debug.WriteLine("public const int " + Mnemonic + "_" + Mode.ToString() + "=0x" + Value.ToString("X2") + ";");
        }

        public void Execute(int SignatureBytes)
        {
            if (ExecuteOp == null)
                throw new NotImplementedException("Tried to execute " + this.Mnemonic + " but it is not implemented.");

            ExecuteOp(Value, AddressMode, SignatureBytes);
        }

        public int Length
        {
            get
            {
                if (ActionRegister == null)
                    return Length8Bit;

                if (ActionRegister.Length == Register.BitLengthEnum.Bits16)
                    return Length8Bit + 1;

                return Length8Bit;
            }
        }

        public override string ToString()
        {
            return this.Mnemonic + " " + this.AddressMode.ToString();
        }

    }
}
