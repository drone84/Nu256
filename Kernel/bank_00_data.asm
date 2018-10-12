; page_00.asm
; Initialization Code
;

* = BANK0_BEGIN      = $000000 ;Start of bank 0 and Direct page

                .word      0                ; CPUPC, 000000, 2 B, Program Counter (PC)
                .byte      0                ; CPUPBR, 000002, 1 B, Program Bank Register (K)
                .word      0                ; CPUA, 000003, 2 B, Accumulator (A)
                .word      0                ; CPUX, 000005, 2 B, X Register (X)
                .word      0                ; CPUY, 000007, 2 B, Y Register (Y)
                .word      0                ; CPUSTACK, 000009, 2 B, Stack Pointer (S)
                .word      0                ; CPUDP, 00000B, 2 B, Direct Page Register (D)
                .byte      0                ; CPUDBR, 00000D, 1 B, Data Bank Register (B)
                .byte      0                ; CPUFLAGS, 00000E, 1 B, Flags (P)

                .long      TEXT_PAGE0       ; SCREENBEGIN, 000010, 3 B, Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what VICKY is displaying, especiall if you are using mirror mode.
                .word      80               ; COLS_VISIBLE, 000013, 2 B, Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
                .word      128              ; COLS_PER_LINE, 000015, 2 B, Columns in memory per screen line. A virtual line can be this long. Default=128
                .word      60               ; LINES_VISIBLE, 000017, 2 B, The number of rows visible on the screen. Default=25
                .word      64               ; LINES_MAX, 000019, 2 B, The number of rows in memory for the screen. Default=64
                .long      TEXT_PAGE0       ; CURSORPOS, 00001B, 3 B, The next character written to the screen will be written in this location. 
                .long      TEXT_PAGE0       ; CURSORROW, 00001E, 3 B, Address of the beginning of the current text row
                .byte      1                ; MIRROR_MODE, 000021, 1 B, 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
                .word      0                ; CURSOR_X, 000022, 2 B, Address of the beginning of the current text row
                .word      0                ; CURSOR_Y, 000024, 2 B, 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
                .word      $0F              ; CURCOLOR, 000026, 2 B, Color of next character to be printed to the screen. 
                .word      $00              ; CURATTR, 000028, 2 B, Attribute of next character to be printed to the screen.
                .word      STACK_END        ; STACKBOT, 00002A, 2 B, Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
                .word      STACK_BEGIN      ; STACKTOP, 00002C, 2 B, Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
                .fill 32,  0                ; KERNEL_TEMP, 0000C0, 32 B, Temp space for kernel
                .fill 32,  0                ; USER_TEMP, 0000E0, 32 B, Temp space for user programs
                