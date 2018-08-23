using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    public class CPUPins
    {
        // Pins
        /// <summary>
        /// Pause the CPU to allow slow I/O or memory operations. When true, the CPU will not execute 
        /// the next instruction.
        /// </summary>
        public bool Ready = false;
        /// <summary>
        /// When high, the CPU is being reset. The CPU will not execute
        /// instructions while reset is high. Once reset goes low (false),
        /// the CPU will read the reset interrupt vector from memory, set the 
        /// Program Counter to the address in the vector, and begin executing 
        /// instructions
        /// </summary>
        public bool Reset = false;
        /// <summary>
        /// When NMI is high, control will shift to the NMI vector. 
        /// </summary>
        public bool NMI = false;
        /// <summary>
        /// When high, the CPU is reading interrupt/reset vectors
        /// </summary>
        public bool VectorPull = false;
        /// <summary>
        /// Aborts the current instruction. Control is shifted to the Abort vector.
        /// </summary>
        public bool Abort = false;
    }
}
