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
            A.Width = Register.BitWidthEnum.Bits8;
            A.DiscardUpper = false;
            X.Width = Register.BitWidthEnum.Bits8;
            X.DiscardUpper = true;
            Y.Width = Register.BitWidthEnum.Bits8;
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
            A.Width = Flags.accMemWidth ? Register.BitWidthEnum.Bits8 : Register.BitWidthEnum.Bits16;
            X.Width = Flags.indeXregisterwidth ? Register.BitWidthEnum.Bits8 : Register.BitWidthEnum.Bits16;
            Y.Width = Flags.indeXregisterwidth ? Register.BitWidthEnum.Bits8 : Register.BitWidthEnum.Bits16;
        }

    }
}
