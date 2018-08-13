using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public partial class CPU
    {

        /// <summary>
        ///  Sets the registers to 8 bits. Sets the emulation flag.
        /// </summary>
        public void SetEmulationMode()
        {
            Flags.Emulation = true;
            A.Length = Register.BitLengthEnum.Bits8;
            A.DiscardUpper = false;
            X.Length = Register.BitLengthEnum.Bits8;
            X.DiscardUpper = true;
            Y.Length = Register.BitLengthEnum.Bits8;
            Y.DiscardUpper = true;
        }

        /// <summary>
        /// Sets the registers to 16 bits. Clears the emulation flag.
        /// </summary>
        public void SetNativeMode()
        {

        }

        /// <summary>
        ///  Sets the width of the A, X, and Y registers based on the X and M flags. 
        /// </summary>
        public void SyncFlags()
        {
            if (Flags.Emulation)
            {
                Flags.accMemWidth = true;
                Flags.indeXregisterwidth = true;
            }
            A.Length = Flags.accMemWidth ? Register.BitLengthEnum.Bits8 : Register.BitLengthEnum.Bits16;
            X.Length = Flags.indeXregisterwidth ? Register.BitLengthEnum.Bits8 : Register.BitLengthEnum.Bits16;
            Y.Length = Flags.indeXregisterwidth ? Register.BitLengthEnum.Bits8 : Register.BitLengthEnum.Bits16;
        }

    }
}
