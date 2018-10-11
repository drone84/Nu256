; page_00.asm
; Initialization Code
;

* = 000000
                .fill 12, 0         ; unused_0000, 000000, 12 B, unused
                .long TEXT_PAGE0    ; SCREENBEGIN, 00000C, 3 B, Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what VICKY is displaying, especiall if you are using mirror mode.
                .word 80            ; COLS_VISIBLE, 00000F, 2 B, Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
                .word 128           ; COLS_PER_LINE, 000011, 2 B, Columns in memory per screen line. A virtual line can be this long. Default=128
                .word 60            ; LINES_VISIBLE, 000013, 2 B, The number of rows visible on the screen. Default=25
                .word 64            ; LINES_MAX, 000015, 2 B, The number of rows in memory for the screen. Default=64
                .long TEXT_PAGE0    ; CURSORPOS, 000017, 3 B, The next character written to the screen will be written in this location. 
                .long TEXT_PAGE0    ; CURSORROW, 00001A, 3 B, Address of the beginning of the current text row
                .byte 1             ; MIRROR_MODE, 00001D, 1 B, 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
                .word 0             ; CURSOR_X, 00001E, 2 B, Address of the beginning of the current text row
                .word 0             ; CURSOR_Y, 000020, 2 B, 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
                .word $0F           ; CURCOLOR, 000022, 2 B, Color of next character to be printed to the screen. 
                .word $00           ; CURATTR, 000024, 2 B, Attribute of next character to be printed to the screen.
                .word STACK_END     ; STACKBOT, 000026, 2 B, Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
                .word STACK_BEGIN   ; STACKTOP, 000028, 2 B, Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
                .fill 32, 0         ; KERNEL_TEMP, 0000D0, 32 B, Temp space for kernel
                .fill 32, 0         ; USER_TEMP, 0000F0, 32 B, Temp space for user programs
                