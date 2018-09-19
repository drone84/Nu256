; page_00.asm
; Initialization Code
;


* = 000000
                JML IBOOT           ; RESET           Jumps to the beginning of kernel ROM. ($F8:0000). 
                JML IRETURN         ; RETURN          Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
                JML KEYDOWN         ; KEYDOWN         Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
                .long $801000       ; SCREENBEGIN     Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
                .word 80            ; COLS_VISIBLE    Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
                .word 128           ; COLS_PER_LINE   Columns in memory per screen line. A virtual line can be this long. Default=128
                .word 60            ; LINES_VISIBLE   The number of rows visible on the screen. Default=25
                .word 64            ; LINES_MAX       The number of rows in memory for the screen. Default=64
                .word 0             ; CURSORPOS       The next character written to the screen will be written in this location. 
                .word 0             ; CURSORX         This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
                .word 0             ; CURSORY         This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
                .byte $0F           ; CURCOLOR        Color of next character to be printed to the screen. 
                .byte $00           ; CURATTR         Attribute of next character to be printed to the screen.
                .word STACK_END     ; STACKBOT        Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
                .word STACK_BEGIN   ; STACKTOP        Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 