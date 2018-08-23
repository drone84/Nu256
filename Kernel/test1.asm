.cpu "65816"			
.autsiz

			* = $0000

			; switch to native mode
			CLC           ; clear the carry flag
			XCE           ; move carry to emulation flag.
			REP #$30      ; set 16-bit accumulator/memory and 16-bit index registers
			
			LDA #$0000
			LDA #$0080
			LDA #$8000
			LDX #$0000
			LDX #$8000
			LDY #$1234

			.as
			.xs
			SEP #$30      ; set 16-bit accumulator/memory and 16-bit index registers
			LDX #$00
			LDX #$80

			SEC           ; set the carry flag
			XCE           ; and switch back to emulated mode
			BRK
