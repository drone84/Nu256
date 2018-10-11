; page_02.asm
; Video/Buffer Addresses
;
;* Addresses are the byte AFTER the block. Use this to confirm block locations and check for overlaps
BANK2_BEGIN      = $020000 ;Start of Bank 2 (Buffers and VRAM)
TEXT_PAGE_SIZE   = $2000 ;Constant: Number of bytes in one page of video RAM
TEXT_PAGE0       = $028000 ;8192 Bytes Video RAM Character data
TEXT_PAGE1       = $02A000 ;8192 Bytes Video RAM Color Data
TEXT_PAGE2       = $02C000 ;8192 Bytes Video RAM Attribute data
TEXT_PAGE3       = $02E000 ;8192 Bytes Video RAM Additional data
;
;