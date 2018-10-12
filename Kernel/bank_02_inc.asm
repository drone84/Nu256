; page_02.asm
; Video/Buffer Addresses
;
BANK2_BEGIN      = $020000 ;Start of Bank 2 (Buffers and VRAM)
MONITOR_VARS     = $020000 ;MONITOR Variables. BASIC variables may overlap this space
MCMDADDR         = $020000 ;3 Bytes Address of the current line of text being processed by the command parser. Can be in display memory or a variable in memory. MONITOR will parse up to MTEXTLEN characters or to a null character.
MCMP_TEXT        = $020003 ;3 Bytes Address of symbol being evaluated for COMPARE routine
MCMP_LEN         = $020006 ;2 Bytes Length of symbol being evaluated for COMPARE routine
MCMD             = $020008 ;4 Bytes Command. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG0            = $02000C ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG1            = $020010 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG2            = $020014 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG3            = $020018 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG4            = $02001C ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG5            = $020020 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG6            = $020024 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.
MARG7            = $020028 ;4 Bytes Argument. Can be 32-bit number, 24-bit address, 24+8 address+len, or 4 text characters.

LOADFILE_VARS    = $000300 ;
LOADFILE_NAME    = $000300 ;3 Bytes (addr) Name of file to load. Address in Data Page
LOADFILE_LEN     = $000303 ;1 Byte  Length of filename. 0=Null Terminated
LOADPBR          = $000304 ;1 Byte  First Program Bank of loaded file ($05 segment)
LOADPC           = $000305 ;2 Bytes Start address of loaded file ($05 segment)
LOADDBR          = $000307 ;1 Byte  First data bank of loaded file ($06 segment)
LOADADDR         = $000308 ;2 Bytes FIrst data address of loaded file ($06 segment)
LOADFILE_TYPE    = $00030A ;3 Bytes (addr) File type string in loaded data file. Actual string data will be in Bank 1. Valid values are BIN, PRG, P16
BLOCK_LEN        = $00030D ;2 Bytes Length of block being loaded
BLOCK_ADDR       = $00030F ;2 Bytes (temp) Address of block being loaded
BLOCK_BANK       = $000311 ;1 Byte  (temp) Bank of block being loaded
BLOCK_COUNT      = $000312 ;2 Bytes (temp) Counter of bytes read as file is loaded

TEXT_PAGE_SIZE   = $2000 ;Constant: Number of bytes in one page of video RAM
TEXT_PAGE0       = $028000 ;8192 Bytes Video RAM Character data
TEXT_PAGE1       = $02A000 ;8192 Bytes Video RAM Color Data
TEXT_PAGE2       = $02C000 ;8192 Bytes Video RAM Attribute data
TEXT_PAGE3       = $02E000 ;8192 Bytes Video RAM Additional data
