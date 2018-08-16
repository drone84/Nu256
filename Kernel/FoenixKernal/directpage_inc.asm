;ASM declare
DPRESET        = $000000 ;4 Bytes 
RETURN         = $000100 ;4 Bytes 
KEYDOWN        = $000104 ;4 Bytes 
KEYWP          = $000108 ;2 Bytes 
KEYRP          = $00010A ;2 Bytes 
SCREENBEGIN    = $00010C ;3 Bytes Start of screen in video RAM
CURSORPOS      = $00010F ;3 Bytes Cursor position in video RAM. SCREENBEGIN is upper left corner
CURSORX        = $000112 ;2 Bytes cursor column
CURSORY        = $000114 ;2 Bytes cursor row
CURCOLOR       = $000116 ;2 Bytes 
CURATTR        = $000118 ;2 Bytes 
STACKBOT       = $00011A ;2 Bytes 
STACKTOP       = $00011C ;2 Bytes 

SYSPBR         = $000200 ;1 Byte  Program Bank
SYSPC          = $000201 ;2 Bytes Program Counter
SYSP           = $000203 ;1 Byte  Flags
SYSA           = $000204 ;2 Bytes Accumulator
SYSX           = $000206 ;2 Bytes X Index
SYSY           = $000208 ;2 Bytes Y Index
SYSDBR         = $00020A ;1 Byte  Data Bank
SYSSP          = $00020B ;2 Bytes Stack Pointer

MTEXTADDR      = $000210 ;3 Bytes 
MPOS           = $000213 ;3 Bytes 
MTLEN          = $000216 ;2 Bytes 
MCMD           = $000218 ;3 Bytes 
MARG1          = $00021B ;3 Bytes 
MARG2          = $00021E ;3 Bytes 
MARG3          = $000221 ;3 Bytes 
MARG4          = $000224 ;3 Bytes 
MARG5          = $000227 ;3 Bytes 
MARG6          = $00022A ;3 Bytes 
MARG7          = $00022D ;3 Bytes 
MARG8          = $000230 ;3 Bytes 

BLINE          = $000200 ;3 Bytes Pointer to current BASIC line on screen

GPU_PAGE_0     = $001000 ;6400 Bytes 
GPU_PAGE_1     = $002900 ;6400 Bytes 
GPU_PAGE_2     = $004200 ;6400 Bytes 
GPU_PAGE_3     = $005B00 ;6400 Bytes 
GPU_END        = $0073FF ;1 Byte  

STACK_BEGIN    = $009700 ;16384 Bytes 
STACK_END      = $00D6FF ;1 Byte  

VECTOR_COP     = $00FFE4 ;2 Bytes 
VECTOR_BRK     = $00FFE6 ;2 Bytes 
VECTOR_ABORT   = $00FFE8 ;2 Bytes 
VECTOR_NMI     = $00FFEA ;2 Bytes 
VECTOR_RESET   = $00FFFC ;2 Bytes 
VECTOR_IRQ     = $00FFEE ;2 Bytes 

VECTOR_ECOP    = $00FFF4 ;2 Bytes 
VECTOR_EBRK    = $00FFF6 ;2 Bytes 
VECTOR_EABORT  = $00FFF8 ;2 Bytes 
VECTOR_ENMI    = $00FFFA ;2 Bytes 
VECTOR_ERESET  = $00FFFC ;2 Bytes 
VECTOR_EIRQ    = $00FFFE ;2 Bytes 


















































































































































































































































































































































































































































































































































































































































































































































































































































































































































































