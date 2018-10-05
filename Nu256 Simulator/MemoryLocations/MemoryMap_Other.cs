using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region GPU Memory
        // c# Memory Map

        public const int SCREEN_PAGE0 = 0x800000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0x802000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE2 = 0x804000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE3 = 0x806000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int CHARDATA_BEGIN = 0x808000; // 32768 Bytes Character data

        #endregion
    }
}
