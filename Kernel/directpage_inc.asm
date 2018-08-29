;
;Direct Page Addresses
;
RESET            = $000000 ;4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
RETURN           = $000004 ;4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
KEYDOWN          = $000008 ;4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
SCREENBEGIN      = $00000C ;3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
SCRWIDTH         = $00000F ;2 Bytes Width of screen
SCRHEIGHT        = $000011 ;2 Bytes Height of screen
CURSORPOS        = $000013 ;3 Bytes The next character written to the screen will be written in this location. 
CURSORX          = $000016 ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURSORY          = $000018 ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURCOLOR         = $00001A ;2 Bytes Color of next character to be printed to the screen. 
CURATTR          = $00001C ;2 Bytes Attribute of next character to be printed to the screen.
STACKBOT         = $00001E ;2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
STACKTOP         = $000020 ;2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
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
MCMDLEN          = $000203 ;2 Bytes Length of string being read by the parser. This should be the screen width when in screen memory. Otherwise should be as long as the buffer used to hold the text to parse. 
MCMDPOS          = $000205 ;3 Bytes Next character being read by the command parser. 
MCMD             = $000208 ;3 Bytes Address of the command text. The first character is used to decide which function to execute
MARG1            = $00020B ;3 Bytes Address of the command arguments. 
MARG2            = $00020E ;3 Bytes Address of the command arguments. 
MARG3            = $000211 ;3 Bytes Address of the command arguments. 
MARG4            = $000214 ;3 Bytes Address of the command arguments. 
MARG5            = $000217 ;3 Bytes Address of the command arguments. 
MARG6            = $00021A ;3 Bytes Address of the command arguments. 
MARG7            = $00021D ;3 Bytes Address of the command arguments. 

BCMDADDR         = $000300 ;3 Bytes Pointer to current BASIC line on screen
BCMDLEN          = $000303 ;2 Bytes Length of the BASIC command
BCMDPOS          = $000305 ;3 Bytes Next character being read in the BASIC command

KEY_BUFFER       = $00F00 ;64 Bytes SCREEN_PAGE1
KEY_BUFFER_LEN   = $40 ;64 Bytes SCREEN_PAGE2
KEY_BUFFER_END   = $000F3F ;1 Byte  SCREEN_PAGE3
KEY_BUFFER_RPOS  = $000F40 ;2 Bytes keyboard buffer read position
KEY_BUFFER_WPOS  = $000F42 ;2 Bytes keyboard buffer write position

SCREEN_PAGE0     = $001000 ;6400 Bytes First page of display RAM. This is used at boot time to display the welcome screen and the BASIC or MONITOR command screens. 
SCREEN_PAGE1     = $002900 ;6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
SCREEN_PAGE2     = $004200 ;6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
SCREEN_PAGE3     = $005B00 ;6400 Bytes Additional page of display RAM. This can be used for page flipping or to handle multiple edit buffers. 
USER_VARIABLES   = $007400 ;0 Byte  This space is avaialble for user code and variables, up to the beginning of the stack. Make sure not to write past STACKBOT without adjusting that value.

STACK_BEGIN      = $009700 ;16384 Bytes The default beginning of stack space
STACK_END        = $00D6FF ;0 Byte  End of stack space. Everything below this is I/O space

HRESET           = $00FF00 ;16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
HCOP             = $00FF10 ;16 Bytes Handle the COP instruction
HBRK             = $00FF20 ;16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
HABORT           = $00FF30 ;16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
HNMI             = $00FF40 ;16 Bytes Handle NMI asserted. 
HIRQ             = $00FF50 ;16 Bytes Handle IRQ. Should read IRQ line from GAVIN and jump to appropriate IRQ handler.
IRQ_0            = $00FF60 ;16 Bytes Handle IRQ 0
IRQ_1            = $00FF70 ;16 Bytes Handle IRQ 1
IRQ_2            = $00FF80 ;16 Bytes Handle IRQ 2
IRQ_3            = $00FF90 ;16 Bytes Handle IRQ 3
IRQ_4            = $00FFA0 ;16 Bytes Handle IRQ 4
IRQ_5            = $00FFB0 ;16 Bytes Handle IRQ 5
IRQ_6            = $00FFC0 ;16 Bytes Handle IRQ 6
IRQ_7            = $00FFD0 ;16 Bytes Handle IRQ 7

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
; End Direct page addresses
