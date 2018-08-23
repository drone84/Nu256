using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public static class MemoryMap_DirectPage
    {
        //C# Declare


        public const int RESET = 0x0000; // 4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
        public const int RETURN = 0x0004; // 4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
        public const int KEYDOWN = 0x0008; // 4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
        public const int KEYWP = 0x000C; // 2 Bytes Keyboard buffer next write position. 
        public const int KEYRP = 0x000E; // 2 Bytes Keyboard buffer next read position. When KEYRP = KEYWP, the buffer is empty. When KEYWP = KEYRP-1, buffer is full.
        public const int SCREENBEGIN = 0x0010; // 3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
        public const int SCRWIDTH = 0x0013; // 2 Bytes Width of screen
        public const int SCRHEIGHT = 0x0015; // 2 Bytes Height of screen
        public const int CURSORPOS = 0x0017; // 3 Bytes The next character written to the screen will be written in this location. 
        public const int CURSORX = 0x001A; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURSORY = 0x001C; // 2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
        public const int CURCOLOR = 0x001E; // 2 Bytes Color of next character to be printed to the screen. 
        public const int CURATTR = 0x0020; // 2 Bytes Attribute of next character to be printed to the screen.
        public const int STACKBOT = 0x0022; // 2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
        public const int STACKTOP = 0x0024; // 2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
        public const int CPUPC = 0x00F0; // 2 Bytes CPU Program Counter. Stored by BRK. Stores CPU state after ML routine is finished running. These values are also loaded back into the CPU on a BASIC SYS command or MONITOR GO command.
        public const int CPUPBR = 0x00F2; // 1 Byte Program Bank
        public const int CPUDP = 0x00F3; // 2 Bytes Direct Page
        public const int CPUFLAGS = 0x00F5; // 1 Byte Flags
        public const int CPUA = 0x00F6; // 2 Bytes Accumulator
        public const int CPUX = 0x00F8; // 2 Bytes X Index
        public const int CPUY = 0x00FA; // 2 Bytes Y Index
        public const int CPUDBR = 0x00FC; // 1 Byte Data Bank
        public const int CPUSTACK = 0x00FD; // 2 Bytes Stack Pointer

        public const int MCMDADDR = 0x0100; // 3 Bytes Address of the current line of text being processed by the MONITOR command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
        public const int MCMDLEN = 0x0103; // 2 Bytes Length of string being read by the parser. This should be the screen width when in screen memory. Otherwise should be as long as the buffer used to hold the text to parse. 
        public const int MCMDPOS = 0x0105; // 3 Bytes Next character being read by the command parser. 
        public const int MCMD = 0x0108; // 3 Bytes Address of the command text. The first character is used to decide which function to execute
        public const int MARG1 = 0x010B; // 3 Bytes Address of the command arguments. 
        public const int MARG2 = 0x010E; // 3 Bytes Address of the command arguments. 
        public const int MARG3 = 0x0111; // 3 Bytes Address of the command arguments. 
        public const int MARG4 = 0x0114; // 3 Bytes Address of the command arguments. 
        public const int MARG5 = 0x0117; // 3 Bytes Address of the command arguments. 
        public const int MARG6 = 0x011A; // 3 Bytes Address of the command arguments. 
        public const int MARG7 = 0x011D; // 3 Bytes Address of the command arguments. 


        public const int BCMDADDR = 0x100; // 3 Bytes Pointer to current BASIC line on screen




        public const int SCREEN_PAGE0 = 0x1000; // 6400 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
        public const int SCREEN_PAGE1 = 0x2900; // 6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE2 = 0x4200; // 6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int SCREEN_PAGE3 = 0x5B00; // 6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
        public const int USER_VARIABLES = 0x7400; // 0 Byte This space is avaialble for user code and variables, up to the beginning of the stack. Make sure not to write past STACKBOT without adjusting that value.

        public const int STACK_BEGIN = 0x009700; // 16384 Bytes The default beginning of stack space
        public const int STACK_END = 0x00D6FF; // 0 Byte End of stack space. Everything below this is I/O space

        public const int VECTOR_COP = 0x00FFE4; // 2 Bytes 
        public const int VECTOR_BRK = 0x00FFE6; // 2 Bytes 
        public const int VECTOR_ABORT = 0x00FFE8; // 2 Bytes 
        public const int VECTOR_NMI = 0x00FFEA; // 2 Bytes 
        public const int VECTOR_RESET = 0x00FFFC; // 2 Bytes 
        public const int VECTOR_IRQ = 0x00FFEE; // 2 Bytes 

        public const int VECTOR_ECOP = 0x00FFF4; // 2 Bytes 
        public const int VECTOR_EBRK = 0x00FFF6; // 2 Bytes 
        public const int VECTOR_EABORT = 0x00FFF8; // 2 Bytes 
        public const int VECTOR_ENMI = 0x00FFFA; // 2 Bytes 
        public const int VECTOR_ERESET = 0x00FFFC; // 2 Bytes 
        public const int VECTOR_EIRQ = 0x00FFFE; // 2 Bytes 

    }
}
