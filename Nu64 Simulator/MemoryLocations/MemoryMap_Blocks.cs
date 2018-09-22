using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Memory Blocks
        // c# Memory Blocks

        public const int SRAM_START = 0x000000; // Beginning of 1MB Static RAM: Code, Data. Extra code can be stashed in DRAM but must be pulled to RAM before being used.
        public const int SRAM_END = 0x07FFFF; // End of 1MB Static RAM
        public const int SRAM_SIZE = 0x080000; // 1MB Static RAM
        public const int IO_START = 0x7F0000; // Beginning of I/O Space
        public const int IO_END = 0x7FFFFF; // End of I/O Space
        public const int IO_SIZE = 0x00FFFF; // 
        public const int DRAM_START = 0x800000; // Beginning of 8MB DRAM for graphics, sound, and offline code
        public const int DRAM_END = 0xFFFFFF; // End of 8MB DRAM 
        public const int DRAM_SIZE = 0x00FFFF; // 

        #endregion
    }
}
