using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64
{
    public static class MemoryMap_DirectPage
    {
        public const int RESET = 0X000000;

        public const int RETURN = 0x000100;
        public const int KEYDOWN = 0x000104;
        public const int KEYWP = 0x000108;
        public const int KEYRP = 0x000109;
        public const int CURSORX = 0x00010A;
        public const int CURSORY = 0x00010B;
        public const int CURCOLOR = 0x00010C;
        public const int CURATTR = 0x00010D;

        public const int SYSPC = 0x200;     // 2 bytes
        public const int SYSPBR = 0x202;    // 1 byte
        public const int SYSP = 0x203;      // 1 byte flags 
        public const int SYSA = 0x204;      // 2 bytes
        public const int SYSX = 0x206;      // 2 bytes
        public const int SYSY = 0x208;      // 2 bytes
        public const int SYSDBR = 0x20A;    // 2 bytes
        public const int SYSDP = 0x20C;     // 2 bytes
        public const int SYSSP = 0x20E;     // 2 bytes

        // Machine Monitor variables. May overlap BASIC variables
        public const int MTEXTADDR = 0x210; // 3 bytes
        public const int MPOS = 0x213;      // 3 bytes
        public const int MTLEN = 0x216;     // 2 bytes
        public const int MCMD = 0x218;      // 1 byte
        // Arguments. Usage varies by command type
        public const int MARG1 = 0x219;     // 3 bytes
        public const int MARG2 = 0x21C;     // 3 bytes
        public const int MARG3 = 0x21F;     // 3 bytes
        public const int MARG4 = 0x222;     // 3 bytes
        public const int MARG5 = 0x225;     // 3 bytes
        public const int MARG6 = 0x228;     // 3 bytes
        public const int MARG7 = 0x22B;     // 3 bytes
        public const int MARG8 = 0x22E;     // 3 bytes

        // BASIC variables, may overlap Monitor variables
        public const int BLINE = 0x200;     // 3 bytes 

        /// Video frame buffer
        public const int GPU_PAGE_0 = 0x001000;
        public const int GPU_PAGE_1 = 0x003000;
        public const int GPU_PAGE_2 = 0x005000;
        public const int GPU_PAGE_3 = 0x007000;

        // vector handlers
        public const int HANDLE_RESET = 0xFF00;
        /* 
        18          CLC       ; CLEAR THE CARRY FLAG
        FB          XCE       ; MOVE CARRY TO EMULATION FLAG.
        C2 30       REP #$30  ; SET 16-BIT ACCUMULATOR/MEMORY AND 16-BIT INDEX REGISTERS
        5C 00 00 00 JMP KBOOT ; Jump to the kernel boot handler
        */
        public const int HANDLE_COP = 0xFF10;
        public const int HANDLE_BRK = 0xFF20;
        // Push Flags
        // Push K
        // Push A
        // Push X
        // Push Y
        // JMP KDUMPREG

        public const int HANDLE_ABORT = 0xFF30;
        public const int HANDLE_NMI = 0xFF40;
        public const int HANDLE_IRQ = 0xFF50;

        /*
        00FFE4 00FFF4 COP
        00FFE6 00FFFE BRK
        00FFE8 00FFF8 ABORT
        00FFEA 00FFFA NMI
                00FFFC RESET
        00FFEE 00FFFE IRQ
        */
        public const int VECTOR_COP = 0xFFE4;
        public const int VECTOR_BRK = 0xFFE6;
        public const int VECTOR_ABORT = 0xFFE8;
        public const int VECTOR_NMI = 0xFFEA;
        public const int VECTOR_RESET = 0xFFFC;
        public const int VECTOR_IRQ = 0xFFEE;

        public const int VECTOR_ECOP = 0xFFF4;
        public const int VECTOR_EBRK = 0xFFF6;
        public const int VECTOR_EABORT = 0xFFF8;
        public const int VECTOR_ENMI = 0xFFFA;
        public const int VECTOR_ERESET = 0xFFFC;
        public const int VECTOR_EIRQ = 0xFFFE;

        public const int START_OF_STACK = 0;
        public const int END_OF_STACK = 0xD7FF;

    }
}
