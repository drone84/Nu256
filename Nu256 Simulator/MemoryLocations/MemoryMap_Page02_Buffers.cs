using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Page $02
        // c#  Video/Buffer Addresses
        //
        public const int BANK2_BEGIN = 0x020000; // Start of Bank 2 (Buffers and VRAM)
        public const int MONITOR_VARS = 0x020000; // MONITOR Variables. BASIC variables may overlap this space
        public const int MCMDADDR = 0x020000; // 2 Bytes Address of the current line of text being processed by the command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
        public const int MCMDADDR_B = 0x020002; // 1 Byte  Address of the current line of text being processed by the command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
        public const int MCMP_TEXT = 0x020003; // 2 Bytes Address of symbol being evaluated for COMPARE routine
        public const int MCMP_TEXT_B = 0x020005; // 1 Byte  Address of symbol being evaluated for COMPARE routine
        public const int MCMP_LEN = 0x020006; // 2 Bytes Length of symbol being evaluated for COMPARE routine
        public const int MCMD = 0x020008; // 4 Bytes Command. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG0 = 0x02000C; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG1 = 0x020010; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG2 = 0x020014; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG3 = 0x020018; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG4 = 0x02001C; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG5 = 0x020020; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG6 = 0x020024; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
        public const int MARG7 = 0x020028; // 4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.

        public const int LOADFILE_VARS = 0x020100; // 
        public const int LOADFILE_NAME_ADDR = 0x020100; // 2 Bytes (addr) Name of file to load. Address in Data Page
        public const int LOADFILE_NAME_ADDR_B = 0x020102; // 1 Byte  (addr) Name of file to load. Address in Data Page
        public const int LOADFILE_LEN = 0x020103; // 1 Byte  Length of filename. 0=Null Terminated
        public const int LOADPBR = 0x020104; // 1 Byte  First Program Bank of loaded file ($05 segment)
        public const int LOADPC = 0x020105; // 2 Bytes Start address of loaded file ($05 segment)
        public const int LOADDBR = 0x020107; // 1 Byte  First data bank of loaded file ($06 segment)
        public const int LOADADDR = 0x020108; // 2 Bytes FIrst data address of loaded file ($06 segment)
        public const int LOADFILE_TYPE = 0x02010A; // 2 Bytes (addr) File type string in loaded data file. Actual string data will be in Bank 1. Valid values are BIN, PRG, P16
        public const int LOADFILE_TYPE_B = 0x02010C; // 1 Byte  (addr) File type string in loaded data file. Actual string data will be in Bank 1. Valid values are BIN, PRG, P16
        public const int BLOCK_LEN = 0x02010D; // 2 Bytes Length of block being loaded
        public const int BLOCK_ADDR = 0x02010F; // 2 Bytes (temp) Address of block being loaded
        public const int BLOCK_BANK = 0x020111; // 1 Byte  (temp) Bank of block being loaded
        public const int BLOCK_COUNT = 0x020112; // 2 Bytes (temp) Counter of bytes read as file is loaded

        public const int TEXT_PAGE_SIZE = 0x2000; // Constant: Number of bytes in one page of video RAM
        public const int TEXT_PAGE0 = 0x028000; // 8192 Bytes Video RAM Character data
        public const int TEXT_PAGE1 = 0x02A000; // 8192 Bytes Video RAM Color Data
        public const int TEXT_PAGE2 = 0x02C000; // 8192 Bytes Video RAM Attribute data
        public const int TEXT_PAGE3 = 0x02E000; // 8192 Bytes Video RAM Additional data
                                                // End Video/Buffer Addresses
        #endregion

    }
}
