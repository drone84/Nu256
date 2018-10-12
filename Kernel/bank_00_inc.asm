; page_00.asm
; Direct Page Addresses
;
;* Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
BANK0_BEGIN      = $000000 ;Start of bank 0 and Direct page
CPU_REGISTERS    = $000000 ; Byte  
CPUPC            = $000000 ;2 Bytes Program Counter (PC)
CPUPBR           = $000002 ;1 Byte  Program Bank Register (K)
CPUA             = $000003 ;2 Bytes Accumulator (A)
CPUX             = $000005 ;2 Bytes X Register (X)
CPUY             = $000007 ;2 Bytes Y Register (Y)
CPUSTACK         = $000009 ;2 Bytes Stack Pointer (S)
CPUDP            = $00000B ;2 Bytes Direct Page Register (D)
CPUDBR           = $00000D ;1 Byte  Data Bank Register (B)
CPUFLAGS         = $00000E ;1 Byte  Flags (P)
SCREENBEGIN      = $000010 ;3 Bytes Start of screen in video RAM. This is the upper-left corrner of the current video page being written to. This may not be what VICKY is displaying, especiall if you are using mirror mode.
COLS_VISIBLE     = $000013 ;2 Bytes Columns visible per screen line. A virtual line can be longer than displayed, up to COLS_PER_LINE long. Default = 80
COLS_PER_LINE    = $000015 ;2 Bytes Columns in memory per screen line. A virtual line can be this long. Default=128
LINES_VISIBLE    = $000017 ;2 Bytes The number of rows visible on the screen. Default=25
LINES_MAX        = $000019 ;2 Bytes The number of rows in memory for the screen. Default=64
CURSORPOS        = $00001B ;3 Bytes The next character written to the screen will be written in this location. 
CURSORROW        = $00001E ;3 Bytes Address of the beginning of the current text row
MIRROR_MODE      = $000021 ;1 Byte  1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
CURSOR_X         = $000022 ;2 Bytes Address of the beginning of the current text row
CURSOR_Y         = $000024 ;2 Bytes 1=Mirror Mode enabled. Reserve 32K (somewhere) in SRAM for a display mirror. 0=Disable Mirror Mode and write directly to VICKY. 
CURCOLOR         = $000026 ;2 Bytes Color of next character to be printed to the screen. 
CURATTR          = $000028 ;2 Bytes Attribute of next character to be printed to the screen.
STACKBOT         = $00002A ;2 Bytes Lowest location the stack should be allowed to write to. If SP falls below this value, the runtime should generate STACK OVERFLOW error and abort.
STACKTOP         = $00002C ;2 Bytes Highest location the stack can occupy. If SP goes above this value, the runtime should generate STACK OVERFLOW error and abort. 
KERNEL_TEMP      = $0000C0 ;32 Bytes Temp space for kernel
USER_TEMP        = $0000E0 ;32 Bytes Temp space for user programs
Page0_Check      = $000100 ; Byte  Expected $0000100

GAVIN_BLOCK      = $000100 ;256 Bytes Gavin reserved
MULTIPLIER_0     = $000100 ;0 Byte  Unsigned multiplier
M0_OPERAND_A     = $000100 ;2 Bytes Operand A (ie: A x B)
M0_OPERAND_B     = $000102 ;2 Bytes Operand B (ie: A x B)
M0_RESULT        = $000104 ;4 Bytes Result of A x B
MULTIPLIER_1     = $000108 ;0 Byte  Signed Multiplier
M1_OPERAND_A     = $000108 ;2 Bytes Operand A (ie: A x B)
M1_OPERAND_B     = $00010A ;2 Bytes Operand B (ie: A x B)
M1_RESULT        = $00010C ;4 Bytes Result of A x B
DIVIDER_0        = $000110 ;0 Byte  Unsigned divider
D0_OPERAND_A     = $000110 ;2 Bytes Divider 0 Dividend ex: A in  A/B 
D0_OPERAND_B     = $000112 ;2 Bytes Divider 0 Divisor ex B in A/B
D0_RESULT        = $000114 ;2 Bytes Quotient result of A/B ex: 7/2 = 3 r 1
D0_REMAINDER     = $000116 ;2 Bytes Remainder of A/B ex: 1 in 7/2=3 r 1
DIVIDER_1        = $000118 ;0 Byte  Signed divider
D1_OPERAND_A     = $000118 ;2 Bytes Divider 1 Dividend ex: A in  A/B 
D1_OPERAND_B     = $00011A ;2 Bytes Divider 1 Divisor ex B in A/B
D1_RESULT        = $00011C ;2 Bytes Signed quotient result of A/B ex: 7/2 = 3 r 1
D1_REMAINDER     = $00011E ;2 Bytes Signed remainder of A/B ex: 1 in 7/2=3 r 1
GAVIN_MISC       = $000120 ;224 Bytes GAVIN vector controller (TBD)
VECTOR_STATE     = $0001FF ;1 Byte  Interrupt Vector State. See VECTOR_STATE_ENUM

KEY_BUFFER       = $000200 ;64 Bytes keyboard buffer
KEY_BUFFER_SIZE  = $40     ;64 Bytes (constant) keyboard buffer length
KEY_BUFFER_END   = $000240 ;1 Byte  Last byte of keyboard buffer
KEY_BUFFER_RPOS  = $000241 ;2 Bytes keyboard buffer read position
KEY_BUFFER_WPOS  = $000243 ;2 Bytes keyboard buffer write position

TEST_BEGIN       = $001000 ;28672 Bytes Test/diagnostic code for prototype.
TEST_END         = $007FFF ;0 Byte  

STACK_BEGIN      = $008000 ;32512 Bytes The default beginning of stack space
STACK_END        = $00FEFF ;0 Byte  End of stack space. Everything below this is I/O space

ISR_BEGIN        = $00FF00 ; Byte  Beginning of CPU vectors in Direct page
HRESET           = $00FF00 ;16 Bytes Handle RESET asserted. Reboot computer and re-initialize the kernel.
HCOP             = $00FF10 ;16 Bytes Handle the COP instruction. Program use; not used by OS
HBRK             = $00FF20 ;16 Bytes Handle the BRK instruction. Returns to BASIC Ready prompt.
HABORT           = $00FF30 ;16 Bytes Handle ABORT asserted. Return to Ready prompt with an error message.
HNMI             = $00FF40 ;32 Bytes Handle NMI
HIRQ             = $00FF60 ;32 Bytes Handle IRQ
Unused_FF80      = $00FF80 ;End of direct page Interrrupt handlers

VECTORS_BEGIN    = $00FFE0 ;0 Byte  Interrupt vectors
JMP_READY        = $00FFE0 ;4 Bytes Jumps to ROM READY routine. Modified whenever alternate command interpreter is loaded. 
VECTOR_COP       = $00FFE4 ;2 Bytes Native COP Interrupt vector
VECTOR_BRK       = $00FFE6 ;2 Bytes Native BRK Interrupt vector
VECTOR_ABORT     = $00FFE8 ;2 Bytes Native ABORT Interrupt vector
VECTOR_NMI       = $00FFEA ;2 Bytes Native NMI Interrupt vector
VECTOR_RESET     = $00FFEC ;2 Bytes Unused (Native RESET vector)
VECTOR_IRQ       = $00FFEE ;2 Bytes Native IRQ Vector
RETURN           = $00FFF0 ;4 Bytes RETURN key handler. Points to BASIC or MONITOR subroutine to execute when RETURN is pressed.
VECTOR_ECOP      = $00FFF4 ;2 Bytes Emulation mode interrupt handler
VECTOR_EBRK      = $00FFF6 ;2 Bytes Emulation mode interrupt handler
VECTOR_EABORT    = $00FFF8 ;2 Bytes Emulation mode interrupt handler
VECTOR_ENMI      = $00FFFA ;2 Bytes Emulation mode interrupt handler
VECTOR_ERESET    = $00FFFC ;2 Bytes Emulation mode interrupt handler
VECTOR_EIRQ      = $00FFFE ;2 Bytes Emulation mode interrupt handler
VECTORS_CHECK    = $010000 ;*End of vector space
BANK0_END        = $00FFFF ;End of Bank 00 and Direct page
; 
