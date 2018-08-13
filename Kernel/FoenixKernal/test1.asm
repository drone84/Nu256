SuperCPU
LONGA   ON
LONGI   ON
*       = $C000
start
; switch to native mode
LONGA   OFF
        CLC           ; clear the carry flag
        XCE           ; move carry to emulation flag.
        REP #$30      ; set 16-bit accumulator/memory and 16-bit index registers
LONGA   ON
        LDA #$0000
        LDA #$0080
        LDA #$8000
        LDX #$0000
        LDX #$8000
        LDY #$1234
LONGI   OFF
LONGA   OFF
        SEP #$30      ; set 16-bit accumulator/memory and 16-bit index registers
        LDX #$00
        LDX #$80
; switch back to emulated mode
        SEC           ; set the carry flag
        XCE           ; and switch back to emulated mode
        BRK