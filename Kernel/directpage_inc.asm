;
;Direct Page Addresses
;
;* Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
BANK0_BEGIN      = $000000 ;Start of bank 0 and Direct page
DIRECT_PAGE      = $000000 ;Start of bank 0 and Direct page
RESET            = $000000 ;4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
RETURN           = $000004 ;4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
KEYDOWN          = $000008 ;4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
SCREENBEGIN      = $00000C ;3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
COLS_VISIBLE     = $00000F ;2 Bytes Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
COLS_PER_LINE    = $000011 ;2 Bytes Columns in memory per screen line. A virtual line can be this long. Default=128
LINES_VISIBLE    = $000013 ;2 Bytes The number of rows visible on the screen. Default=25
LINES_MAX        = $000015 ;2 Bytes The number of rows in memory for the screen. Default=64
CURSORPOS        = $000017 ;3 Bytes The next character written to the screen will be written in this location. 
CURSORX          = $00001A ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURSORY          = $00001C ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURCOLOR         = $00001E ;2 Bytes Color of next character to be printed to the screen. 
CURATTR          = $000020 ;2 Bytes Attribute of next character to be printed to the screen.
STACKBOT         = $000022 ;2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
STACKTOP         = $000024 ;2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
TEMP             = $0000E0 ;16 Bytes Temp storage for kernel routines

GAVIN_BLOCK      = $000100 ;256 Bytes Gavin reserved, overlaps debugging registers at $1F0
MULTIPLIER_0     = $000100 ;0 Byte  Unsigned multiplier
M0_OPERAND_A     = $000100 ;2 Bytes Operand A (ie: A x B)
M0_OPERAND_B     = $000102 ;2 Bytes Operand B (ie: A x B)
M0_RESULT        = $000104 ;4 Bytes Result of A x B
MULTIPLIER_1     = $000108 ;0 Byte  Signed Multiplier
M1_OPERAND_A     = $000108 ;2 Bytes Operand A (ie: A x B)
M1_OPERAND_B     = $00010A ;2 Bytes Operand B (ie: A x B)
M1_RESULT        = $00010C ;4 Bytes Result of A x B
DIVIDER_0        = $000108 ;0 Byte  Unsigned divider
D0_OPERAND_A     = $000108 ;2 Bytes Divider 0 Dividend ex: A in  A/B 
D0_OPERAND_B     = $00010A ;2 Bytes Divider 0 Divisor ex B in A/B
D0_RESULT        = $00010C ;2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
D0_REMAINDER     = $00010E ;2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1
DIVIDER_1        = $000110 ;0 Byte  Signed divider
D1_OPERAND_A     = $000110 ;2 Bytes Divider 1 Dividend ex: A in  A/B 
D1_OPERAND_B     = $000112 ;2 Bytes Divider 1 Divisor ex B in A/B
D1_RESULT        = $000114 ;2 Bytes Signed quotient result of A/B ex: 7/2 = 3 r 1
D1_REMAINDER     = $000116 ;2 Bytes Signed remainder of A/B ex: 1 in 7/2=3 r 1
VECTOR_STATE     = $0001FF ;1 Byte  Interrupt Vector State. See VECTOR_STATE_ENUM



CPUPC            = $0001F0 ;2 Bytes Debug registers. When BRK is executed, Interrupt service routine will populate this block with the CPU registers. 
CPUPBR           = $0001F2 ;1 Byte  Program Bank Register (K)
CPUDBR           = $0001F3 ;1 Byte  Data Bank Register (B)
CPUA             = $0001F4 ;2 Bytes Accumulator (A)
CPUX             = $0001F6 ;2 Bytes X Register
CPUY             = $0001F8 ;2 Bytes Y Index Register
CPUSTACK         = $0001FA ;2 Bytes Stack (S)
CPUDP            = $0001FC ;2 Bytes Direct Page Register (D)
CPUFLAGS         = $0001FE ;1 Byte  Flags (P)

MCMDADDR         = $000200 ;3 Bytes Address of the current line of text being processed by the MONITOR command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
MCMP_TEXT        = $000203 ;3 Bytes Address of symbol being evaluated for COMPARE routine
MCMP_LEN         = $000206 ;2 Bytes Length of symbol being evaluated for COMPARE routine
MCMD             = $000208 ;3 Bytes Address of the current command/function string
MCMD_LEN         = $00020B ;2 Bytes Length of the current command/function string
MARG1            = $00020D ;3 Bytes Address of the command arguments. 
MARG1_LEN        = $000210 ;2 Bytes Length of the argument 
MARG2            = $000212 ;3 Bytes Address of the command arguments. 
MARG2_LEN        = $000215 ;2 Bytes Length of the argument 
MARG3            = $000217 ;3 Bytes Address of the command arguments. 
MARG3_LEN        = $00021A ;2 Bytes Length of the argument 
MARG4            = $00021C ;3 Bytes Address of the command arguments. 
MARG4_LEN        = $00021F ;2 Bytes Length of the argument 
MARG5            = $000221 ;3 Bytes Address of the command arguments. 
MARG5_LEN        = $000224 ;2 Bytes Length of the argument 
MARG6            = $000226 ;3 Bytes Address of the command arguments. 
MARG6_LEN        = $000229 ;2 Bytes Length of the argument 
MARG7            = $00022B ;3 Bytes Address of the command arguments. 
MARG7_LEN        = $00022E ;2 Bytes Length of the argument 

KEY_BUFFER       = $00F00 ;64 Bytes KEY_BUFFER
KEY_BUFFER_SIZE  = $40 ;64 Bytes KEY_BUFFER_SIZE
KEY_BUFFER_END   = $000F3F ;1 Byte  KEY_BUFFER_END
KEY_BUFFER_RPOS  = $000F40 ;2 Bytes KEY_BUFFER_RPOS
KEY_BUFFER_WPOS  = $000F42 ;2 Bytes KEY_BUFFER_WPOS

SCREEN_PAGE0     = $001000 ;8192 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
SCREEN_PAGE1     = $003000 ;8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
SCREEN_PAGE2     = $005000 ;8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
SCREEN_PAGE3     = $007000 ;8192 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
SCREEN_END       = $009000 ;End of display memory

USER_VARIABLES   = $009000 ;2048 Bytes This space is free for user data in Direct Page
USER_VARIABLES_E = $009800 ;*End of user free space

STACK_BEGIN      = $009800 ;16384 Bytes The default beginning of stack space
STACK_END        = $00D7FF ;0 Byte  End of stack space. Everything below this is I/O space

IO_BEGIN         = $00D800 ; Byte  Beginning of IO space
IO_GAVIN         = $00D800 ;1024 Bytes GAVIN I/O space
IO_SUPERIO       = $00DC00 ;1024 Bytes SuperIO I/O space
IO_VICKY         = $00E000 ;1024 Bytes VICKY I/O space
IO_BEATRIX       = $00E400 ;1024 Bytes BEATRIX I/O space
IO_RTC           = $00E800 ;1024 Bytes RTC I/O space
IO_CIA           = $00EC00 ;4864 Bytes CIA I/O space
IO_END           = $00FF00 ;*End of I/O space

ISR_BEGIN        = $00FF00 ; Byte  Beginning of CPU vectors in Direct page
HRESET           = $00FF00 ;16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
HCOP             = $00FF10 ;16 Bytes Handle the COP instruction. Program use; not used by OS
HBRK             = $00FF20 ;16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
HABORT           = $00FF30 ;16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
INT_TABLE        = $00FF40 ;96 Bytes Interrupt vectors for GAVIN interrupt handler
ISR_END          = $00FFA0 ;*End of vector space

VECTORS_BEGIN    = $00FFE0 ;0 Byte  Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
JMP_READY        = $00FFE0 ;4 Bytes Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
VECTOR_COP       = $00FFE4 ;2 Bytes Native interrupt vector
VECTOR_BRK       = $00FFE6 ;2 Bytes Native interrupt vector
VECTOR_ABORT     = $00FFE8 ;2 Bytes Native interrupt vector
VECTOR_NMI       = $00FFEA ;2 Bytes Native interrupt vector
VECTOR_RESET     = $00FFEC ;2 Bytes Native interrupt vector
VECTOR_IRQ       = $00FFEE ;2 Bytes Native interrupt vector


VECTOR_ECOP      = $00FFF4 ;2 Bytes Emulation mode interrupt handler
VECTOR_EBRK      = $00FFF6 ;2 Bytes Emulation mode interrupt handler
VECTOR_EABORT    = $00FFF8 ;2 Bytes Emulation mode interrupt handler
VECTOR_ENMI      = $00FFFA ;2 Bytes Emulation mode interrupt handler
VECTOR_ERESET    = $00FFFC ;2 Bytes Emulation mode interrupt handler
VECTOR_EIRQ      = $00FFFE ;2 Bytes Emulation mode interrupt handler
VECTORS_END      = $010000 ;*End of vector space
; 
