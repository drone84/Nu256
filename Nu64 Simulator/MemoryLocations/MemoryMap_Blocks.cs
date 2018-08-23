using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.MemoryLocations
{
    public static class MemoryMap_Blocks
    {
        public const int START_OF_DIRECT_PAGE = 0x000000;
        public const int END_OF_DIRECT_PAGE = 0xFFFF;
        /// <summary>
        /// I/O block: ports, CIAs, keyboard, etc. Basically everything that's not video.
        /// </summary>
        public const int START_OF_IO = 0xD800;
        public const int END_OF_IO = 0xFEFF;


        public const int START_OF_RAM = 0x000000;
        public const int END_OF_RAM = 0x5FFFFF;  // 8MB

        /// <summary>
        /// DMA buffers. 
        /// </summary>
        public const int START_OF_DMA = 0x600000;
        public const int END_OF_DMA = 0x6FFFFF;

        public const int START_OF_INTERRUPTS = 0xFF00;
        public const int END_OF_INTERRUPTS = 0xFFFF;

        /// <summary>
        /// GPU takes the top 1MB of RAM
        /// </summary>
        public const int START_OF_GPU = 0x700000;
        public const int END_OF_GPU = 0x7FFFFF;

        /// <summary>
        /// ROM takes 1MB in the 15-16MB block.
        /// </summary>
        public const int START_OF_ROM = 0xF00000;
        public const int END_OF_ROM = 0xFFFFFF;
    }
}
