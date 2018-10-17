using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Direct page
        // c# Direct page Addresses

        // * Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
        public const int BANK0_BEGIN = 0x000000; // Start of bank 0 and Direct page
        public const int CPU_REGISTERS = 0x000000; //  Byte 
        public const int CPUPC = 0x000000; // 2 Bytes Program Counter (PC)
        public const int CPUPBR = 0x000002; // 1 Byte Program Bank Register (K)
        public const int CPUA = 0x000003; // 2 Bytes Accumulator (A)
        public const int CPUX = 0x000005; // 2 Bytes X Register (X)
        public const int CPUY = 0x000007; // 2 Bytes Y Register (Y)
        public const int CPUSTACK = 0x000009; // 2 Bytes Stack Pointer (S)
        public const int CPUDP = 0x00000B; // 2 Bytes Direct Page Register (D)
        public const int CPUDBR = 0x00000D; // 1 Byte Data Bank Register (B)
        public const int CPUFLAGS = 0x00000E; // 1 Byte Flags (P)

        public const int SCREENBEGIN = 0x000010; // 2 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what VICKY is displaying, especiall if you are using mirror mode.
        public const int SCREENBANK = 0x000012; // 1 Byte Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what VICKY is displaying, especiall if you are using mirror mode.
        public const int COLS_VISIBLE = 0x000013; // 2 Bytes Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
        public const int COLS_PER_LINE = 0x000015; // 2 Bytes Columns in memory per screen line. A virtual line can be this long. Default=128
        public const int LINES_VISIBLE = 0x000017; // 2 Bytes The number of rows visible on the screen. Default=25
        public const int LINES_MAX = 0x000019; // 2 Bytes The number of rows in memory for the screen. Default=64
        public const int CURSORPOS = 0x00001B; // 2 Bytes The next character written to the screen will be written in this location. 
        public const int CURSORPOS_B = 0x00001D; // 1 Byte The next character written to the screen will be written in this location. 
        public const int MIRROR_MODE = 0x00001E; // 1 Byte 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
        public const int CURSOR_X = 0x00001F; // 2 Bytes Address of the beginning of the current text row
        public const int CURSOR_Y = 0x000021; // 2 Bytes 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
        public const int CURCOLOR = 0x000023; // 2 Bytes Color of next character to be printed to the screen. 
        public const int CURATTR = 0x000025; // 2 Bytes Attribute of next character to be printed to the screen.
        public const int STACKBOT = 0x000027; // 2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
        public const int STACKTOP = 0x000029; // 2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
        public const int READPOS = 0x00002B; // 2 Bytes Location of input buffer for parsing text
        public const int READPOS_B = 0x00002D; // 1 Byte Location of input buffer for parsing text
        public const int READEND = 0x00002E; // 2 Bytes End of the input buffer
        public const int READEND_B = 0x000030; // 1 Byte End of the input buffer
        public const int WRITEPOS = 0x000031; // 2 Bytes Address to write parsed data
        public const int WRITEPOS_B = 0x000033; // 1 Byte Address to write parsed data
        public const int KERNEL_TEMP = 0x0000C0; // 32 Bytes Temp space for kernel
        public const int USER_TEMP = 0x0000E0; // 32 Bytes Temp space for user programs
        public const int Page0_Check = 0x000100; //  Byte Expected $0000100

        public const int GAVIN_BLOCK = 0x000100; // 256 Bytes Gavin reserved
        public const int MULTIPLIER_0 = 0x000100; // 0 Byte Unsigned multiplier
        public const int M0_OPERAND_A = 0x000100; // 2 Bytes Operand A (ie: A x B)
        public const int M0_OPERAND_B = 0x000102; // 2 Bytes Operand B (ie: A x B)
        public const int M0_RESULT = 0x000104; // 4 Bytes Result of A x B
        public const int MULTIPLIER_1 = 0x000108; // 0 Byte Signed Multiplier
        public const int M1_OPERAND_A = 0x000108; // 2 Bytes Operand A (ie: A x B)
        public const int M1_OPERAND_B = 0x00010A; // 2 Bytes Operand B (ie: A x B)
        public const int M1_RESULT = 0x00010C; // 4 Bytes Result of A x B
        public const int DIVIDER_0 = 0x000110; // 0 Byte Unsigned divider
        public const int D0_OPERAND_A = 0x000110; // 2 Bytes Divider 0 Dividend ex: A in  A/B 
        public const int D0_OPERAND_B = 0x000112; // 2 Bytes Divider 0 Divisor ex B in A/B
        public const int D0_RESULT = 0x000114; // 2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
        public const int D0_REMAINDER = 0x000116; // 2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1
        public const int DIVIDER_1 = 0x000118; // 0 Byte Signed divider
        public const int D1_OPERAND_A = 0x000118; // 2 Bytes Divider 1 Dividend ex: A in  A/B 
        public const int D1_OPERAND_B = 0x00011A; // 2 Bytes Divider 1 Divisor ex B in A/B
        public const int D1_RESULT = 0x00011C; // 2 Bytes Signed quotient result of A/B ex: 7/2 = 3 r 1
        public const int D1_REMAINDER = 0x00011E; // 2 Bytes Signed remainder of A/B ex: 1 in 7/2=3 r 1
        public const int GAVIN_MISC = 0x000120; // 224 Bytes GAVIN vector controller (TBD)
        public const int VECTOR_STATE = 0x0001FF; // 1 Byte Interrupt Vector State. See VECTOR_STATE_ENUM

        public const int KEY_BUFFER = 0x000200; // 64 Bytes keyboard buffer
        public const int KEY_BUFFER_SIZE = 0x40; // 64 Bytes (constant) keyboard buffer length
        public const int KEY_BUFFER_END = 0x000240; // 2 Bytes Last byte of keyboard buffer
        public const int KEY_BUFFER_RPOS = 0x000242; // 2 Bytes keyboard buffer read position
        public const int KEY_BUFFER_WPOS = 0x000244; // 2 Bytes keyboard buffer write position

        public const int TEST_BEGIN = 0x001000; // 28672 Bytes Test/diagnostic code for prototype.
        public const int TEST_END = 0x007FFF; // 0 Byte 

        public const int STACK_BEGIN = 0x008000; // 32512 Bytes The default beginning of stack space
        public const int STACK_END = 0x00FEFF; // 0 Byte End of stack space. Everything below this is I/O space

        public const int ISR_BEGIN = 0x00FF00; //  Byte Beginning of CPU vectors in Direct page
        public const int HRESET = 0x00FF00; // 16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
        public const int HCOP = 0x00FF10; // 16 Bytes Handle the COP instruction. Program use; not used by OS
        public const int HBRK = 0x00FF20; // 16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
        public const int HABORT = 0x00FF30; // 16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
        public const int HNMI = 0x00FF40; // 32 Bytes Handle NMI
        public const int HIRQ = 0x00FF60; // 32 Bytes Handle IRQ
        public const int Unused_FF80 = 0x00FF80; // End of direct page Interrrupt handlers

        public const int VECTORS_BEGIN = 0x00FFE0; // 0 Byte Interrupt vectors
        public const int JMP_READY = 0x00FFE0; // 4 Bytes Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
        public const int VECTOR_COP = 0x00FFE4; // 2 Bytes Native COP Interrupt vector
        public const int VECTOR_BRK = 0x00FFE6; // 2 Bytes Native BRK Interrupt vector
        public const int VECTOR_ABORT = 0x00FFE8; // 2 Bytes Native ABORT Interrupt vector
        public const int VECTOR_NMI = 0x00FFEA; // 2 Bytes Native NMI Interrupt vector
        public const int VECTOR_RESET = 0x00FFEC; // 2 Bytes Unused (Native RESET vector)
        public const int VECTOR_IRQ = 0x00FFEE; // 2 Bytes Native IRQ Vector
        public const int RETURN = 0x00FFF0; // 4 Bytes RETURN key handler. Points to BASIC or MONITOR subroutine to execute when RETURN is pressed.
        public const int VECTOR_ECOP = 0x00FFF4; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EBRK = 0x00FFF6; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EABORT = 0x00FFF8; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ENMI = 0x00FFFA; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ERESET = 0x00FFFC; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EIRQ = 0x00FFFE; // 2 Bytes Emulation mode interrupt handler
        public const int VECTORS_CHECK = 0x010000; // *End of vector space
        public const int BANK0_END = 0x00FFFF; // End of Bank 00 and Direct page
        #endregion

    }
}
