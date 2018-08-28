;
;Direct Page Addresses
;
RESET            = $000000 ;4 Bytes Jumps to the beginning of kernel ROM. ($F8:0000). 
RETURN           = $000004 ;4 Bytes Called when the RETURN key is pressed in the immediate mode screen. This will process a command in MONITOR, execute a BASIC command, or add a BASIC program line.
KEYDOWN          = $000008 ;4 Bytes Custom keyboard handler. This defaults to the kernel keypress handler, but you can redirect this to your own routines. Make sure to JML to the original address at the end of your custom routine. Use this to make F-Key macros or custom keyboard commands. 
KB_READPOS       = $00000C ;2 Bytes Keyboard buffer next write position. 
KB_WRITEPOS      = $00000E ;2 Bytes Keyboard buffer next read position. When KEYRP = KEYWP, the buffer is empty. When KEYWP = KEYRP-1, buffer is full.
SCREENBEGIN      = $000010 ;3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what's being displayed by VICKY. Update this if you change VICKY's display page. 
SCRWIDTH         = $000013 ;2 Bytes Width of screen
SCRHEIGHT        = $000015 ;2 Bytes Height of screen
CURSORPOS        = $000017 ;3 Bytes The next character written to the screen will be written in this location. 
CURSORX          = $00001A ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURSORY          = $00001C ;2 Bytes This is where the blinking cursor sits. Do not edit this direectly. Call LOCATE to update the location and handle moving the cursor correctly. 
CURCOLOR         = $00001E ;2 Bytes Color of next character to be printed to the screen. 
CURATTR          = $000020 ;2 Bytes Attribute of next character to be printed to the screen.
STACKBOT         = $000022 ;2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
STACKTOP         = $000024 ;2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
TEMP             = $0000E0 ;16 Bytes Temp storage for kernel routines
CPUPC            = $0000F0 ;2 Bytes CPU Program Counter. Stored by BRK. Stores CPU state after ML routine is finished running. These values are also loaded back into the CPU on a BASIC SYS command or MONITOR GO command.
CPUPBR           = $0000F2 ;1 Byte  Program Bank
CPUDP            = $0000F3 ;2 Bytes Direct Page
CPUFLAGS         = $0000F5 ;1 Byte  Flags
CPUA             = $0000F6 ;2 Bytes Accumulator
CPUX             = $0000F8 ;2 Bytes X Index
CPUY             = $0000FA ;2 Bytes Y Index
CPUDBR           = $0000FC ;1 Byte  Data Bank
CPUSTACK         = $0000FD ;2 Bytes Stack Pointer

MCMDADDR         = $000100 ;3 Bytes Address of the current line of text being processed by the MONITOR command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
MCMDLEN          = $000103 ;2 Bytes Length of string being read by the parser. This should be the screen width when in screen memory. Otherwise should be as long as the buffer used to hold the text to parse. 
MCMDPOS          = $000105 ;3 Bytes Next character being read by the command parser. 
MCMD             = $000108 ;3 Bytes Address of the command text. The first character is used to decide which function to execute
MARG1            = $00010B ;3 Bytes Address of the command arguments. 
MARG2            = $00010E ;3 Bytes Address of the command arguments. 
MARG3            = $000111 ;3 Bytes Address of the command arguments. 
MARG4            = $000114 ;3 Bytes Address of the command arguments. 
MARG5            = $000117 ;3 Bytes Address of the command arguments. 
MARG6            = $00011A ;3 Bytes Address of the command arguments. 
MARG7            = $00011D ;3 Bytes Address of the command arguments. 

BCMDADDR         = $000100 ;3 Bytes Pointer to current BASIC line on screen
BCMDLEN          = $000103 ;2 Bytes Length of the BASIC command
BCMDPOS          = $000105 ;3 Bytes Next character being read in the BASIC command

KEY_BUFFER       = $000FC0 ;64 Bytes Keyboard Buffer
KEY_BUFFER_END   = $000FFF ;64 Bytes End of keyboard buffer

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
