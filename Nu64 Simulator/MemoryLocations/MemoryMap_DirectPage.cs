﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public static class MemoryMap_DirectPage
    {
        #region Direct page
        // c# Direct page Addresses

        // * Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
        public const int BANK0_BEGIN = 0x000000; // Start of bank 0 and Direct page
        public const int DIRECT_PAGE = 0x000000; // Start of bank 0 and Direct page
        public const int RESET = 0x000000; // 4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
        public const int RETURN = 0x000004; // 4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
        public const int KEYDOWN = 0x000008; // 4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
        public const int SCREENBEGIN = 0x00000C; // 3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
        public const int COLS_VISIBLE = 0x00000F; // 2 Bytes Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
        public const int COLS_PER_LINE = 0x000011; // 2 Bytes Columns in memory per screen line. A virtual line can be this long. Default=128
        public const int LINES_VISIBLE = 0x000013; // 2 Bytes The number of rows visible on the screen. Default=25
        public const int LINES_MAX = 0x000015; // 2 Bytes The number of rows in memory for the screen. Default=64
        public const int CURSORPOS = 0x000017; // 3 Bytes The next character written to the screen will be written in this location. 
        public const int CURSORX = 0x00001A; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURSORY = 0x00001C; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURCOLOR = 0x00001E; // 2 Bytes Color of next character to be printed to the screen. 
        public const int CURATTR = 0x000020; // 2 Bytes Attribute of next character to be printed to the screen.
        public const int STACKBOT = 0x000022; // 2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
        public const int STACKTOP = 0x000024; // 2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
        public const int TEMP = 0x0000E0; // 16 Bytes Temp storage for kernel routines

        public const int GAVIN_BLOCK = 0x000100; // 256 Bytes Gavin reserved, overlaps debugging registers at $1F0
        public const int MULTIPLIER_0 = 0x000100; // 0 Byte Unsigned multiplier
        public const int M0_OPERAND_A = 0x000100; // 2 Bytes Operand A (ie: A x B)
        public const int M0_OPERAND_B = 0x000102; // 2 Bytes Operand B (ie: A x B)
        public const int M0_RESULT = 0x000104; // 4 Bytes Result of A x B
        public const int MULTIPLIER_1 = 0x000108; // 0 Byte Signed Multiplier
        public const int M1_OPERAND_A = 0x000108; // 2 Bytes Operand A (ie: A x B)
        public const int M1_OPERAND_B = 0x00010A; // 2 Bytes Operand B (ie: A x B)
        public const int M1_RESULT = 0x00010C; // 4 Bytes Result of A x B
        public const int DIVIDER_0 = 0x000108; // 0 Byte Unsigned divider
        public const int D0_OPERAND_A = 0x000108; // 2 Bytes Divider 0 Dividend ex: A in  A/B 
        public const int D0_OPERAND_B = 0x00010A; // 2 Bytes Divider 0 Divisor ex B in A/B
        public const int D0_RESULT = 0x00010C; // 2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
        public const int D0_REMAINDER = 0x00010E; // 2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1
        public const int DIVIDER_1 = 0x000110; // 0 Byte Signed divider
        public const int D1_OPERAND_A = 0x000110; // 2 Bytes Divider 1 Dividend ex: A in  A/B 
        public const int D1_OPERAND_B = 0x000112; // 2 Bytes Divider 1 Divisor ex B in A/B
        public const int D1_RESULT = 0x000114; // 2 Bytes Signed quotient result of A/B ex: 7/2 = 3 r 1
        public const int D1_REMAINDER = 0x000116; // 2 Bytes Signed remainder of A/B ex: 1 in 7/2=3 r 1

        public const int CPUPC = 0x0001F0; // 2 Bytes Debug registers. When BRK is executed, Interrupt service routine will populate this block with the CPU registers. 
        public const int CPUPBR = 0x0001F2; // 1 Byte Program Bank Register (K)
        public const int CPUDBR = 0x0001F3; // 1 Byte Data Bank Register (B)
        public const int CPUA = 0x0001F4; // 2 Bytes Accumulator (A)
        public const int CPUX = 0x0001F6; // 2 Bytes X Register
        public const int CPUY = 0x0001F8; // 2 Bytes Y Index Register
        public const int CPUSTACK = 0x0001FA; // 2 Bytes Stack (S)
        public const int CPUDP = 0x0001FC; // 2 Bytes Direct Page Register (D)
        public const int CPUFLAGS = 0x0001FE; // 1 Byte Flags (P)

        public const int MCMDADDR = 0x000200; // 3 Bytes Address of the current line of text being processed by the MONITOR command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
        public const int MCMDLEN = 0x000203; // 2 Bytes Length of string being read by the parser. This should be the screen width when in screen memory. Otherwise should be as long as the buffer used to hold the text to parse. 
        public const int MCMDPOS = 0x000205; // 3 Bytes Next character being read by the command parser. 
        public const int MCMD = 0x000208; // 3 Bytes Address of the command text. The first character is used to decide which function to execute
        public const int MARG1 = 0x00020B; // 3 Bytes Address of the command arguments. 
        public const int MARG2 = 0x00020E; // 3 Bytes Address of the command arguments. 
        public const int MARG3 = 0x000211; // 3 Bytes Address of the command arguments. 
        public const int MARG4 = 0x000214; // 3 Bytes Address of the command arguments. 
        public const int MARG5 = 0x000217; // 3 Bytes Address of the command arguments. 
        public const int MARG6 = 0x00021A; // 3 Bytes Address of the command arguments. 
        public const int MARG7 = 0x00021D; // 3 Bytes Address of the command arguments. 

        public const int BCMDADDR = 0x000300; // 3 Bytes Pointer to current BASIC line on screen
        public const int BCMDLEN = 0x000303; // 2 Bytes Length of the BASIC command
        public const int BCMDPOS = 0x000305; // 3 Bytes Next character being read in the BASIC command

        public const int KEY_BUFFER = 0x00F00; // 64 Bytes SCREEN_PAGE1
        public const int KEY_BUFFER_LEN = 0x40; // 64 Bytes SCREEN_PAGE2
        public const int KEY_BUFFER_END = 0x000F3F; // 1 Byte SCREEN_PAGE3
        public const int KEY_BUFFER_RPOS = 0x000F40; // 2 Bytes keyboard buffer read position
        public const int KEY_BUFFER_WPOS = 0x000F42; // 2 Bytes keyboard buffer write position

        public const int SCREEN_PAGE0 = 0x001000; // 8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0x003000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE2 = 0x005000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE3 = 0x007000; // 8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_END = 0x009000; // This space is avaialble for user code and variables, up to the beginning of the stack. Do not write past STACK_BEGIN

        public const int STACK_BEGIN = 0x009800; // 16384 Bytes The default beginning of stack space
        public const int STACK_END = 0x00D7FF; // 0 Byte End of stack space. Everything below this is I/O space

        public const int IO_BEGIN = 0x00D800; //  Byte Beginning of IO space
        public const int IO_GAVIN = 0x00D800; // 1024 Bytes GAVIN I/O space
        public const int IO_SUPERIO = 0x00DC00; // 1024 Bytes SuperIO I/O space
        public const int IO_VICKY = 0x00E000; // 1024 Bytes VICKY I/O space
        public const int IO_BEATRIX = 0x00E400; // 1024 Bytes BEATRIX I/O space
        public const int IO_RTC = 0x00E800; // 1024 Bytes RTC I/O space
        public const int IO_CIA = 0x00EC00; // 4864 Bytes CIA I/O space
        public const int IO_END = 0x00FF00; // *End of I/O space

        public const int ISR_BEGIN = 0x00FF00; //  Byte Beginning of CPU vectors in Direct page
        public const int HRESET = 0x00FF00; // 16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
        public const int HCOP = 0x00FF10; // 16 Bytes Handle the COP instruction
        public const int HBRK = 0x00FF20; // 16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
        public const int HABORT = 0x00FF30; // 16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
        public const int HNMI = 0x00FF40; // 80 Bytes Handle NMI asserted. 
        public const int HIRQ = 0x00FF90; // 80 Bytes Handle IRQ. Should read IRQ line from GAVIN and jump to appropriate IRQ handler.
        public const int ISR_END = 0x00FFE0; // *End of vector space

        public const int VECTORS_BEGIN = 0x00FFE0; // 0 Byte Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
        public const int JMP_READY = 0x00FFE0; // 4 Bytes Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
        public const int VECTOR_COP = 0x00FFE4; // 2 Bytes Native interrupt vector
        public const int VECTOR_BRK = 0x00FFE6; // 2 Bytes Native interrupt vector
        public const int VECTOR_ABORT = 0x00FFE8; // 2 Bytes Native interrupt vector
        public const int VECTOR_NMI = 0x00FFEA; // 2 Bytes Native interrupt vector
        public const int VECTOR_RESET = 0x00FFEC; // 2 Bytes Native interrupt vector
        public const int VECTOR_IRQ = 0x00FFEE; // 2 Bytes Native interrupt vector


        public const int VECTOR_ECOP = 0x00FFF4; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EBRK = 0x00FFF6; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EABORT = 0x00FFF8; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ENMI = 0x00FFFA; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_ERESET = 0x00FFFC; // 2 Bytes Emulation mode interrupt handler
        public const int VECTOR_EIRQ = 0x00FFFE; // 2 Bytes Emulation mode interrupt handler
        public const int VECTORS_END = 0x010000; // *End of vector space
        #endregion

    }
}
