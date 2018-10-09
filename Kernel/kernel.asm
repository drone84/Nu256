.cpu "65816"
.include "macros_inc.asm"
.include "simulator_inc.asm"
.include "page_00_inc.asm"
.include "page_00_data.asm"
.include "page_00_code.asm"
.include "dram_inc.asm"
.include "vicky_def.asm"
.include "super_io_def.asm"
.include "keyboard_def.asm"
.include "VIA_def.asm"
.include "SID_def.asm"
.include "RTC_def.asm"
.include "Math_def.asm"
.include "monitor.asm"

; C256 Foenix / Nu64 Kernel
; Loads to $F0:0000

;Kernel.asm
;Jump Table

.include "kernel_jumptable.asm"

* = $010400

IBOOT           ; boot the system
                CLC           ; clear the carry flag
                XCE           ; move carry to emulation flag.
                setaxl
                LDA #STACK_END   ; initialize stack pointer
                TAS
                setdp 0
                LDA #<>SCREEN_PAGE0      ; store the initial screen buffer location
                STA SCREENBEGIN
                setas
                LDA #`SCREEN_PAGE0
                STA SCREENBEGIN+2
                setaxl
                LDA #<>SCREEN_PAGE0      ; store the initial screen buffer location
                STA CURSORPOS
                setas
                LDA #`SCREEN_PAGE0
                STA CURSORPOS+2
                setaxl

                ; Set screen dimensions. There more columns in memory than
                ; are visible. A virtual line is 128 bytes, but 80 columns will be
                ; visible on screen.
                LDX #80
                STX COLS_VISIBLE
                LDY #60
                STY LINES_VISIBLE
                LDX #128
                STX COLS_PER_LINE
                LDY #64
                STY LINES_MAX

                ; Initialize the Character Color Foreground/Background LUT First
                JSL IINITCHLUT
                ; Now, clear the screen and Setup Foreground/Background Bytes, so we can see the Text on screen
                JSL ICLRSCREEN
                ; Write the Greeting Message Here, after Screen Cleared and Colored

greet           setdbr `greet_msg       ;Set data bank to ROM
                LDX #<>greet_msg
                JSL IPRINT       ; print the first line
                JSL ICOLORFLAG   ; Let's go put some Colors
                LDX #<>version_msg
                JSL IPRINT       ; print the first line
                ; Initialize Super IO Chip
                JSL IINITSUPERIO
                LDX #<>init_lpc_msg
                JSL IPRINT       ; print the Init

                ; Init the VIAs (to do some Test)
                JSL IINITVIAS
                LDX #<>init_via_msg
                JSL IPRINT       ; print the VIAs Init Message

                ; Init the RTC (Test the Interface)
                JSL IINITRTC
                LDX #<>init_rtc_msg
                JSL IPRINT       ; print the RTC Init Message

                ; Test SID (writing a bunch a Registers, so we can hear shit!)
                JSL ITESTSID
                LDX #<>test_SID_msg
                JSL IPRINT       ; print the SID Test Message

                setdp 0
                JSL ITESTMATH

                ; Init KeyBoard
                JSL IINITKEYBOARD
                LDX #<>init_kbrd_msg
                JSL IPRINT       ; print the Keybaord Init Message
                ; set the location of the cursor (top left corner of screen)
                setdp 0
                setal
                LDX #$0
                LDY #$0
                JSL ILOCATE

                ; reset keyboard buffer
                ;STZ KEY_BUFFER_RPOS
                ;STZ KEY_BUFFER_WPOS

                ; ; Copy vectors from ROM to Direct Page
                ; setaxl
                ; LDA #$FF
                ; LDX #$FF00
                ; LDY #$FF00
                ; MVP $00, $FF

                ; display boot message
;greet           setdbr `greet_msg       ;Set data bank to ROM
;                LDX #<>greet_msg
;                JSL IPRINT       ; print the first line
;                JSL IPRINT       ; print the second line
;                JSL IPRINT       ; print the third line
;                JSL IPRINTCR     ; print a blank line. Just because
                setas
                setdbr $01      ;set data bank to 1 (Kernel Variables)
greet_done      BRK             ;Terminate boot routine and go to Ready handler.

;
; IBREAK
; ROM Break handler. This pulls the registers out of the stack
; and saves them in the "CPU" direct page locations
IBREAK          setdp 0
                PLA             ; Pull .Y and stuff it in the CPUY variable
                STA CPUY
                PLA             ; Pull .X and stuff it in the CPUY variable
                STA CPUX
                PLA             ; Pull .A and stuff it in the CPUY variable
                STA CPUA
                PLA
                STA CPUDP       ; Pull Direct page
                setas
                PLA             ; Pull Data Bank (8 bits)
                STA CPUDBR
                PLA             ; Pull Flags (8 bits)
                STA CPUFLAGS
                setal
                PLA             ; Pull Program Counter (16 bits)
                STA CPUPC
                setas
                PLA             ; Pull Program Bank (8 bits)
                STA CPUPBR
                setal
                TSA             ; Get the stack
                STA CPUSTACK    ; Store the stack at immediately before the interrupt was asserted
                LDA #<>STACK_END   ; initialize stack pointer back to the bootup value
                                ;<> is "lower word"
                TAS
                JML JMP_READY   ; Run READY routine (usually BASIC or MONITOR)

IREADY          setdbr `ready_msg
                setas
                LDX #<>ready_msg
                JSL IPRINT
;
; IREADYWAIT*
;  Wait for a keypress and display it on the screen. When the RETURN key is pressed,
;  call the RETURN event handler to process the command. Since RETURN can change, use
;  the vector in Direct Page to invoke the handler.
;
;  *Does not return. Execution in your program should continue via the RETURN direct page
;  vector.
IREADYWAIT      ; Check the keyboard buffer.
                JSL IGETCHE
                BRA IREADYWAIT

IKEYDOWN        STP             ; Keyboard key pressed
IRETURN         STP

;
;IGETCHE
; Get a character from the current input chnannel and echo it to screen.
; Waits for a character to be read.
; Return:
; A: Character read
; Carry: 1 if no valid data
;
IGETCHE         JSL IGETCHW
                JSL IPUTC
                RTL

;
;IGETCHW
; Get a character from the current input chnannel.
; Waits for a character to be read.
; Return:
; A: Character read
; Carry: 1 if no valid data
;
IGETCHW         PHD
                PHX
                PHP
                setdp $0F00
                setaxl
                ; Read from the keyboard buffer
                ; If the read position and write position are the same
                ; no data is waiting.
igetchw1        LDX KEY_BUFFER_RPOS
                CPX KEY_BUFFER_WPOS
                ; If data is waiting. return it.
                ; Otherwise wait for data.
                BNE igetchw2
                ;SEC            ; In non-waiting version, set the Carry bit and return
                ;BRA igetchw_done
                ; Simulator should wait for input
                SIM_WAIT
                JMP igetchw1
igetchw2        LDA $0,D,X  ; Read the value in the keyboard buffer
                PHA
                ; increment the read position and wrap it when it reaches the end of the buffer
                TXA
                CLC
                ADC #$02
                CMP #KEY_BUFFER_SIZE
                BCC igetchw3
                LDA #$0
igetchw3        STA KEY_BUFFER_RPOS
                PLA

igetchw_done    PLP
                PLX             ; Restore the saved registers and return
                PLD
                RTL
;
; IPRINT
; Print a string, followed by a carriage return
; DBR: bank containing string
; X: address of the string in data bank
; Modifies: X
;
IPRINT          JSL IPUTS
                JSL IPRINTCR
                RTL

; IPUTS
; Print a null terminated string
; DBR: bank containing string
; X: address of the string in data bank
; Modifies: X.
;  X will be set to the location of the byte following the string
;  So you can print multiple, contiguous strings by simply calling
;  IPUTS multiple times.
IPUTS           PHA
                PHP
                setas
                setxl
iputs1          LDA $0,b,x      ; read from the string
                BEQ iputs_done
iputs2          JSL IPUTC
iputs3          INX
                JMP iputs1
iputs_done      INX
                PLP
                PLA
                RTL

;
;IPUTC
; Print a single character to a channel.
; Handles terminal sequences, based on the selected text mode
; Modifies: none
;
IPUTC           PHD
                PHP             ; stash the flags (we'll be changing M)
                setdp 0
                setas
                CMP #$0D        ; handle CR
                BNE iputc_bs
                JSL IPRINTCR
                bra iputc_done
iputc_bs        CMP #$08        ; backspace
                BNE iputc_print
                JSL IPRINTBS
                BRA iputc_done
iputc_print     STA [CURSORPOS] ; Save the character on the screen
                JSL ICSRRIGHT
iputc_done	sim_refresh
                PLP
                PLD
                RTL

;
;IPUTB
; Output a single byte to a channel.
; Does not handle terminal sequences.
; Modifies: none
;
IPUTB
                ;
                ; TODO: write to open channel
                ;
                RTL

;
; IPRINTCR
; Prints a carriage return.
; This moves the cursor to the beginning of the next line of text on the screen
; Modifies: Flags
IPRINTCR	PHX
                PHY
                PHP
                LDX #0
                LDY CURSORY
                INY
                JSL ILOCATE
                PLP
                PLY
                PLX
                RTL

;
; IPRINTBS
; Prints a carriage return.
; This moves the cursor to the beginning of the next line of text on the screen
; Modifies: Flags
IPRINTBS	PHX
                PHY
                PHP
                LDX CURSORX
                LDY CURSORY
                DEX
                JSL ILOCATE
                PLP
                PLY
                PLX
                RTL

;
;ICSRRIGHT
; Move the cursor right one space
; Modifies: none
;
ICSRRIGHT	; move the cursor right one space
                PHX
                PHB
                setal
                setxl
                setdp $0
                INC CURSORPOS
                LDX CURSORX
                INX
                CPX COLS_VISIBLE
                BCC icsr_nowrap  ; wrap if the cursor is at or past column 80
                LDX #0
                PHY
                LDY CURSORY
                INY
                JSL ILOCATE
                PLY
icsr_nowrap     STX CURSORX
                PLB
                PLX
                RTL

ISRLEFT	RTL
ICSRUP	RTL
ICSRDOWN	RTL

;ILOCATE
;Sets the cursor X and Y positions to the X and Y registers
;Direct Page must be set to 0
;Input:
; X: column to set cursor
; Y: row to set cursor
;Modifies: none
ILOCATE         PHA
                PHP
                setaxl
ilocate_scroll  ; If the cursor is below the bottom row of the screen
                ; scroll the screen up one line. Keep doing this until
                ; the cursor is visible.
                CPY LINES_VISIBLE
                BCC ilocate_scrolldone
                JSL ISCROLLUP
                DEY
                ; repeat until the cursor is visible again
                BRA ilocate_scroll
ilocate_scrolldone
                ; done scrolling store the resultant cursor positions.
                STX CURSORX
                STY CURSORY
                LDA SCREENBEGIN
ilocate_row     ; compute the row
                CPY #$0
                BEQ ilocate_right
                ; move down the number of rows in Y
ilocate_down    CLC
                ADC COLS_PER_LINE
                DEY
                BEQ ilocate_right
                BRA ilocate_down
                ; compute the column
ilocate_right   CLC
                ADC CURSORX             ; move the cursor right X columns
                STA CURSORPOS
                LDY CURSORY
ilocate_done    PLP
                PLA
                RTL
;
; ISCROLLUP
; Scroll the screen up one line
; Inputs:
;   None
; Affects:
;   None
ISCROLLUP       ; Scroll the screen up by one row
                ; Place an empty line at the bottom of the screen.
                ; TODO: use DMA to move the data
                PHA
                PHX
                PHY
                PHB
                PHP
                setaxl
                ; Set block move source to second row
                CLC
                LDA SCREENBEGIN
                TAY             ; Destination is first row
                ADC COLS_PER_LINE
                TAX             ; Source is second row
                ;TODO compute screen bottom with multiplier
                ;(once implemented)
                ; for now, should be 8064 or $1f80 bytes
                LDA #SCREEN_PAGE1-SCREEN_PAGE0-COLS_PER_LINE
                ; Move the data
                MVP $00,$00

                PLP
                PLB
                PLY
                PLX
                PLA
                RTL

;
; IPRINTH
; Prints data from memory in hexadecimal format
; Inputs:
;   X: 16-bit address of the LAST BYTE of data to print.
;   Y: Length in bytes of data to print
; Modifies:
;   X,Y, results undefined
IPRINTH         PHP
                PHA
iprinth1        setas
                LDA #0,b,x      ; Read the value to be printed
                LSR
                LSR
                LSR
                LSR
                JSL iprint_digit
                LDA #0,b,x
                JSL iprint_digit
                DEX
                DEY
                BNE iprinth1
                PLA
                PLP
                RTL

;
; iprint_digit
; This will print the low nibble in the A register.
; Inputs:
;   A: digit to print
;   x flag should be 0 (16-bit X)
; Affects:
;   P: m flag will be set to 0
iprint_digit    PHX
                setal
                AND #$0F
                TAX
                ; Use the value in AL to
                .databank ?
                LDA hex_digits,X
                JSL IPUTC       ; Print the digit
                PLX
                RTL
;
; ICLRSCREEN
; Clear the screen and set the background and foreground colors to the
; currently selected colors.
ICLRSCREEN	    PHD
                PHP
                PHA
                PHX
                setas
                setxl 			; Set 16bits
                LDX #$0000		; Only Use One Pointer
                LDA #$20		; Fill the Entire Screen with Space
iclearloop0	    STA $800000,x	;
                inx
                cpx #$2000
                bne iclearloop0
                ; Now Set the Colors so we can see the text
                LDX	#$0000		; Only Use One Pointer
                LDA #$ED		; Fill the Color Memory with Foreground: 75% Purple, Background 12.5% White
iclearloop1	    STA $802000,x	;
                inx
                cpx #$2000
                bne iclearloop1
                setxl
                setal
                PLX
                PLA
                PLP
                PLD
                RTL

;
; ICOLORFLAG
; Set the colors of the flag on the welcome screen
;
ICOLORFLAG      PHA
                PHX
                PHP
                setaxs
                LDX #$00
iclearloop2	    LDA @lgreet_clr_line1,x
                STA $802000,x
                LDA @lgreet_clr_line2,x
                STA $802080,x
                LDA @lgreet_clr_line3,x
                STA $802100,x
                LDA @lgreet_clr_line4,x
                STA $802180,x
                LDA @lgreet_clr_line5,x
                STA $802200,x
                inx
                cpx #$0E
                bne iclearloop2
                PLP
                PLX
                PLA
                RTL
;
; IINITCHLUT
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Initialize VICKY's Character Color Look-Up Table;
; Inputs:
;   None
; Affects:
;   None
IINITCHLUT		  PHD
                PHP
                PHA
                PHX
                setas
                setxs 					; Set 8bits
				        ; Setup Foreground LUT First
				        LDX	#$00
lutinitloop0	  LDA @lfg_color_lut,x		; get Local Data
                STA FG_CHAR_LUT_PTR,x	; Write in LUT Memory
                inx
                cpx #$40
                bne lutinitloop0
                ; Set Background LUT Second
                LDX	#$00
lutinitloop1	  LDA @lbg_color_lut,x		; get Local Data
                STA BG_CHAR_LUT_PTR,x	; Write in LUT Memory
                inx
                cpx #$40
                bne lutinitloop1
                setal
                setxl 					; Set 8bits
                PLX
                PLA
                PLP
                PLD
                RTL

;
; IINITSUPERIO
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Initialize SuperIO PME Registers
; Inputs:
;   None
; Affects:
;   None
IINITSUPERIO	  PHD
                PHP
                PHA
                setas			;just make sure we are in 8bit mode

                LDA #$01		;Default Value - C256 Doesn't use this IO Pin
                STA GP10_REG
                LDA #$01		;Default Value - C256 Doesn't use this IO Pin
                STA GP11_REG
                LDA #$01		;Default Value - C256 Doesn't use this IO Pin
        				LDA #$01		;Default Value - C256 Doesn't use this IO Pin
        				STA GP13_REG
        				LDA #$05		;(C256 - POT A Analog BX) Bit[0] = 1, Bit[2] = 1
        				STA GP14_REG
        				LDA #$05		;(C256 - POT A Analog BY) Bit[0] = 1, Bit[2] = 1
        				STA GP15_REG
        				LDA #$05		;(C256 - POT B Analog BX) Bit[0] = 1, Bit[2] = 1
        				STA GP16_REG
        				LDA #$05		;(C256 - POT B Analog BY) Bit[0] = 1, Bit[2] = 1
        				STA GP17_REG
        				LDA #$00		;(C256 - HEADPHONE MUTE) - Output GPIO - Push-Pull (1 - Headphone On, 0 - HeadPhone Off)
        				STA GP20_REG

                ;LDA #$00		;(C256 - FLOPPY - DS1) - TBD Later, Floppy Stuff (JIM DREW)
				        ;STA GP21_REG
				        ;LDA #$00		;(C256 - FLOPPY - DMTR1) - TBD Later, Floppy Stuff (JIM DREW)
				        ;STA GP22_REG

				        LDA #$01		;Default Value - C256 Doesn't use this IO Pin
				        STA GP24_REG
				        LDA #$05		;(C256 - MIDI IN) Bit[0] = 1, Bit[2] = 1 (Page 132 Manual)
				        STA GP25_REG
			        	LDA #$84		;(C256 - MIDI OUT) Bit[2] = 1, Bit[7] = 1 (Open Drain - To be Checked)
				        STA GP26_REG
        				LDA #$01		;Default Value - C256 Doesn't use this IO Pin
				        STA GP24_REG

				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 1) Setup as GPIO Input for now
				        STA GP30_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 4) Setup as GPIO Input for now
				        STA GP31_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 3) Setup as GPIO Input for now
				        STA GP32_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 6) Setup as GPIO Input for now
				        STA GP33_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 5) Setup as GPIO Input for now
				        STA GP34_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 8) Setup as GPIO Input for now
				        STA GP35_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 7) Setup as GPIO Input for now
				        STA GP36_REG
				        LDA #$01		;Default Value - (C256 - JP1 Fanout Pin 10) Setup as GPIO Input for now
				        STA GP37_REG

				        ;LDA #$01		;(C256 - FLOPPY - DRVDEN0) - TBD Later, Floppy Stuff (JIM DREW)
				        ;STA GP40_REG
				        ;LDA #$01		;(C256 - FLOPPY - DRVDEN1) - TBD Later, Floppy Stuff (JIM DREW)
				        ;STA GP41_REG

				        LDA #$01		;Default Value - C256 Doesn't use this IO Pin
				        STA GP42_REG

			          LDA #$01		;(C256 - INPUT PLL CLK INTERRUPT) Default Value - Will keep it as an input for now, no real usage for now
				        STA GP43_REG

				        LDA #$05		;(C256 - UART2 - RI2) - Input - Set Secondary Function
				        STA GP50_REG
				        LDA #$05		;(C256 - UART2 - DCD2) - Input - Set Secondary Function
				        STA GP51_REG
				        LDA #$05		;(C256 - UART2 - RXD2) - Input - Set Secondary Function
				        STA GP52_REG
				        LDA #$04		;(C256 - UART2 - TXD2) - Output - Set Secondary Function
				        STA GP53_REG
				        LDA #$05		;(C256 - UART2 - DSR2) - Input - Set Secondary Function
				        STA GP54_REG
				        LDA #$04		;(C256 - UART2 - RTS2) - Output - Set Secondary Function
				        STA GP55_REG
				        LDA #$05		;(C256 - UART2 - CTS2) - Input - Set Secondary Function
				        STA GP56_REG
				        LDA #$04		;(C256 - UART2 - DTR2) - Output - Set Secondary Function
				        STA GP57_REG

				        LDA #$84		;(C256 - LED1) - Open Drain - Output
				        STA GP60_REG
				        LDA #$84		;(C256 - LED2) - Open Drain - Output
				        STA GP61_REG

			        	LDA #$00		;GPIO Data Register (GP10..GP17) - Not Used
				        STA GP1_REG
				        LDA #$01		;GPIO Data Register (GP20..GP27) - Bit[0] - Headphone Mute (Enabling it)
				        STA GP2_REG
				        LDA #$00		;GPIO Data Register (GP30..GP37) - Since it is in Output mode, nothing to write here.
				        STA GP3_REG
				        LDA #$00		;GPIO Data Register (GP40..GP47)  - Not Used
				        STA GP4_REG
				        LDA #$00		;GPIO Data Register (GP50..GP57)  - Not Used
				        STA GP5_REG
				        LDA #$00		;GPIO Data Register (GP60..GP61)  - Not Used
				        STA GP6_REG

				        LDA #$01		;LED1 Output - Already setup by Vicky Init Phase, for now, I will leave it alone
				        STA LED1_REG
				        LDA #$02		;LED2 Output - However, I will setup this one, to make sure the Code works (Full On, when Code was ran)
				        STA LED2_REG
				        setal
                PLA
				        PLP
			        	PLD
                RTL



;
; IINITKEYBOARD
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Initialize the Keyboard Controler (8042) in the SuperIO.
; Inputs:
;   None
; Affects:
;   None
IINITKEYBOARD	  PHD
				        PHP
				        PHA
				        PHX
                setas				;just make sure we are in 8bit mode
                setxs 					; Set 8bits
				; Setup Foreground LUT First
				        LDX	#$00
initkb_loop1	  LDA @lSTATUS_PORT,x		; Load Status Byte
				        AND	#INPT_BUF_FULL	; Test bit $02 (if 0, Empty)
				        CMP #INPT_BUF_FULL
				        BEQ initkb_loop1

				        LDA #$0AA			;Send self test command
				        STA @lKBD_CMD_BUF,x
								;; Sent Self-Test Code and Waiting for Return value, it ought to be 0x55.
initkb_loop2	  LDA @lSTATUS_PORT,x		; Wait for test to complete
				        AND	#OUT_BUF_FULL	; Test bit $01 (0 = No Data)
				        CMP #OUT_BUF_FULL
				        BNE initkb_loop2

				        LDA @lKBD_OUT_BUF,x		;Check self test result
				        CMP #$55
				        BNE	initkb_loop_out

				        LDA #$AB			;Send test Interface command
				        STA @lKBD_DATA_BUF,x

initkb_loop3	  LDA @lSTATUS_PORT,x		; Wait for test to complete
				        AND	#OUT_BUF_FULL	; Test bit $01 (if 0, Empty)
				        CMP #OUT_BUF_FULL
			        	BNE initkb_loop3

				        LDA @lKBD_OUT_BUF,x		;Display Interface test results
				        CMP #$00			;Should be 00
				        BNE	initkb_loop_out

				        LDA #$AE			; Enable the Keyboard
				        STA @lKBD_DATA_BUF,x

initkb_loop8	  LDA @lSTATUS_PORT,x		; Wait for test to complete
				        AND	#OUT_BUF_FULL	; Test bit $01 (if 0, Empty)
				        CMP #OUT_BUF_FULL
				        BNE initkb_loop8

				        LDA @lKBD_OUT_BUF,x		; Clear the Output buffer

initkb_loop_out
                setal
                setxl 					; Set 8bits
                PLX
                PLA
				        PLP
				        PLD
                RTL
;
; IINITVIAS
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; This is more a Test than a Init - Looking for to setup a Timer to 60Hz for the music
; Inputs:
;   None
; Affects:
;   None
IINITVIAS       PHA
                setas				    ;just make sure we are in 8bit mode
                ; Init First VIA - Setup to Read JoyStick
                LDA #$E0
                STA VIA0_DDR_REG_B ; Set Dir ( JOYA )- PA0..4 In, PA5..7 output
                STA VIA0_DDR_REG_A ; Set Dir ( JOYB )- PA0..4 In, PA5..7 output
                ; Init Second VIA
                LDA #$FF
                STA VIA1_DDR_REG_B ; Set Dir( USER Port ) - PA0..7 Output
                LDA #$55
                STA VIA1_IO_REG_A ; THis is to Test if Code Worked Fine (check Value on the USER Port Connector)

                ; The Main Clock Runs @ 14.418M and with a 16Bits Counter, It can't reach 60Hz
                ; So, I will set it to get 240Hz Output. - This is just a TEST - To be removed Later
                ; The 60Hz Interrupt Click for Sound will have to come from Vicky.
                LDA #$0B
                STA VIA1_T1L_L
                LDA #$E9
                STA VIA1_T1L_H
                LDA #$41
                STA VIA1_ACR    ; Continuous interrupts, enable latching

                setal 					; Set 16bits
                PLA
                RTL

;
; IINITRTC
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Initialize the Real Time Clock
; Inputs:
;   None
                ; Affects:
                ;   None
IINITRTC        PHA
                setas				    ;just make sure we are in 8bit mode
                LDA #$00
                STA RTC_SEC     ;Set the Time to 10:10AM
                LDA #10
                STA RTC_MIN
                STA RTC_HRS
                LDA #12
                STA RTC_DAY
                LDA #04
                STA RTC_MONTH   ; April 12th, 2018 - Begining of the Project
                LDA #04
                STA RTC_MONTH   ; Thursday
                LDA #18
                STA RTC_YEAR    ; Thursday

                LDA RTC_DAY     ; Read the Day Registers
                STA RTC_DAY     ; Store it back

                setal 					; Set 16bits
                PLA
                RTL
;
; ITESTSID
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Initialize the Real Time Clock
; Inputs:
; None
ITESTSID        PHA
                setas				    ;just make sure we are in 8bit mode
                ; Left SID
                ; Voice 1
                LDA #$36              ;Left Side (Rev A of Board)
                STA SID0_V1_FREQ_LO
                LDA #$03
                STA SID0_V1_FREQ_HI   ;G1
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID0_V1_PW_LO
                LDA #$08
                STA SID0_V1_PW_HI   ;G1
                LDA #$08
                STA SID0_V1_CTRL    ; Reset
                ; Voice 2
                LDA #$0C
                STA SID0_V2_FREQ_LO
                LDA #$04
                STA SID0_V2_FREQ_HI   ;B1
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID0_V2_PW_LO
                LDA #$08
                STA SID0_V2_PW_HI   ;G1
                LDA #$08
                STA SID0_V2_CTRL    ; Reset
                ; Voice 3
                LDA #$00
                STA SID0_V3_FREQ_LO
                LDA #$08
                STA SID0_V3_FREQ_HI   ;D
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID0_V3_PW_LO
                LDA #$08
                STA SID0_V3_PW_HI   ;G1
                LDA #$08
                STA SID0_V3_CTRL    ; Reset
                ; Set the Volume to Max
                LDA #$0F
                STA SID0_MODE_VOL
                ; Enable each Voices with Triangle Wave
                LDA #$10
                STA SID0_V1_CTRL    ; Triangle
                STA SID0_V2_CTRL    ; Triangle
                STA SID0_V3_CTRL    ; Triangle

                ; Right SID
                ; Voice 1
                LDA #$36              ;Left Side (Rev A of Board)
                STA SID1_V1_FREQ_LO
                LDA #$03
                STA SID1_V1_FREQ_HI   ;G1
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID1_V1_PW_LO
                LDA #$08
                STA SID1_V1_PW_HI   ;G1
                LDA #$08
                STA SID1_V1_CTRL    ; Reset
                ; Voice 2
                LDA #$0C
                STA SID1_V2_FREQ_LO
                LDA #$04
                STA SID1_V2_FREQ_HI   ;B1
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID1_V2_PW_LO
                LDA #$08
                STA SID1_V2_PW_HI   ;G1
                LDA #$08
                STA SID1_V2_CTRL    ; Reset
                ; Voice 3
                LDA #$00
                STA SID1_V3_FREQ_LO
                LDA #$08
                STA SID1_V3_FREQ_HI   ;D
                LDA #$00              ;Left Side (Rev A of Board)
                STA SID1_V3_PW_LO
                LDA #$08
                STA SID1_V3_PW_HI   ;G1
                LDA #$08
                STA SID1_V3_CTRL    ; Reset
                ; Set the Volume to Max
                LDA #$0F
                STA SID1_MODE_VOL
                ; Enable each Voices with Triangle Wave
                LDA #$10
                STA SID1_V1_CTRL    ; Triangle
                STA SID1_V2_CTRL    ; Triangle
                STA SID1_V3_CTRL    ; Triangle

                setal 					; Set 16bits
                PLA
                RTL
;
; ITESTMATH
; Author: Stefany
; Note: We assume that A & X are 16Bits Wide when entering here.
; Verify that the Math Block Works
; Inputs:
; None
ITESTMATH       PHA
                setal 					; Set 16bits
                LDA #$1234
                STA UNSIGNED_MULT_A_LO
                LDA #$55AA
                STA UNSIGNED_MULT_B_LO
                ; Results Ought to be : $06175A88
                LDA UNSIGNED_MULT_AL_LO
                STA STEF_BLOB_BEGIN

                LDA UNSIGNED_MULT_AH_LO
                STA STEF_BLOB_BEGIN + 2
                setxl 					; Set 16bits
                setal 					; Set 16bits
                PLA
                RTL


;
;Not-implemented routines
;
IRESTORE        BRK ; Warm boot routine
ISCINIT         BRK ;
IIOINIT         BRK ;
IPUTBLOCK       BRK ; Ouput a binary block to the currently selected channel
ISETLFS         BRK ; Obsolete (done in OPEN)
ISETNAM         BRK ; Obsolete (done in OPEN)
IOPEN           BRK ; Open a channel for reading and/or writing. Use SETLFS and SETNAM to set the channels and filename first.
ICLOSE          BRK ; Close a channel
ISETIN          BRK ; Set the current input channel
ISETOUT         BRK ; Set the current output channel
IGETB           BRK ; Get a byte from input channel. Return 0 if no input. Carry is set if no input.
IGETBLOCK       BRK ; Get a X byes from input channel. If Carry is set, wait. If Carry is clear, do not wait.
IGETCH          BRK ; Get a character from the input channel. A=0 and Carry=1 if no data is wating
IGETS           BRK ; Get a string from the input channel. NULL terminates
IGETLINE        BRK ; Get a line of text from input channel. CR or NULL terminates.
IGETFIELD       BRK ; Get a field from the input channel. Value in A, CR, or NULL terminates
ITRIM           BRK ; Removes spaces at beginning and end of string.
IPRINTC         BRK ; Print character to screen. Handles terminal commands
IPRINTS         BRK ; Print string to screen. Handles terminal commands
IPRINTF         BRK ; Print a float value
IPRINTI         BRK ; Prints integer value in TEMP
IPRINTAI        BRK ; Prints integer value in A
IPRINTAH        BRK ; Prints hex value in A. Printed value is 2 wide if M flag is 1, 4 wide if M=0
IPUSHKEY        BRK ;
IPUSHKEYS       BRK ;
ICSRLEFT        BRK ;
ICSRHOME        BRK ;
ISCRREADLINE    BRK ; Loads the MCMDADDR/BCMDADDR variable with the address of the current line on the screen. This is called when the RETURN key is pressed and is the first step in processing an immediate mode command.
ISCRGETWORD     BRK ; Read a current word on the screen. A word ends with a space, punctuation (except _), or any control character (value < 32). Loads the address into CMPTEXT_VAL and length into CMPTEXT_LEN variables.

;
; Greeting message and other kernel boot data
;
KERNEL_DATA
greet_msg       .text $20, $20, $20, $20, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, "C256 FOENIX DEVELOPMENT SYSTEM",$0D
                .text $20, $20, $20, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $20, "OPEN SOURCE COMPUTER",$0D
                .text $20, $20, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $20, $20, "HARDWARE DESIGNER: STEFANY ALLAIRE",$0D
                .text $20, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $20, $20, $20, "SOFTWARE DESIGNER: TOM WILSON",$0D
                .text $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $EC, $A9, $20, $20, $20, $20, "1024KB BASIC RAM  8192K MEDIA RAM",$00

greet_clr_line1 .text $1D, $1D, $1D, $1D, $1D, $1D, $8D, $8D, $4D, $4D, $2D, $2D, $5D, $5D
greet_clr_line2 .text $1D, $1D, $1D, $1D, $1D, $8D, $8D, $4D, $4D, $2D, $2D, $5D, $5D, $5D
greet_clr_line3 .text $1D, $1D, $1D, $1D, $8D, $8D, $4D, $4D, $2D, $2D, $5D, $5D, $5D, $5D
greet_clr_line4 .text $1D, $1D, $1D, $8D, $8D, $4D, $4D, $2D, $2D, $5D, $5D, $5D, $5D, $5D
greet_clr_line5 .text $1D, $1D, $8D, $8D, $4D, $4D, $2D, $2D, $5D, $5D, $5D, $5D, $5D, $5D

fg_color_lut	  .text $00, $00, $00, $FF
                .text $00, $00, $C0, $FF
                .text $00, $C0, $00, $FF
                .text $C0, $00, $00, $FF
                .text $00, $C0, $C0, $FF
                .text $C0, $C0, $00, $FF
                .text $C0, $00, $C0, $FF
                .text $C0, $C0, $C0, $FF
                .text $00, $7F, $FF, $FF
                .text $13, $45, $8B, $FF
                .text $00, $00, $40, $FF
                .text $00, $40, $00, $FF
                .text $40, $00, $00, $FF
                .text $40, $40, $40, $FF
                .text $80, $80, $80, $FF
                .text $FF, $FF, $FF, $FF

bg_color_lut	  .text $00, $00, $00, $FF
                .text $00, $00, $C0, $FF
                .text $00, $C0, $00, $FF
                .text $C0, $00, $00, $FF
                .text $00, $40, $40, $FF
                .text $40, $40, $00, $FF
                .text $40, $00, $40, $FF
                .text $40, $40, $40, $FF
                .text $1E, $69, $D2, $FF
                .text $13, $45, $8B, $FF
                .text $00, $00, $40, $FF
                .text $00, $40, $00, $FF
                .text $40, $00, $00, $FF
                .text $20, $20, $20, $FF
                .text $80, $80, $80, $FF
                .text $FF, $FF, $FF, $FF

version_msg     .text "Degug Code Version 0.0.1 - Oct 8th, 2018", $0D, $00
init_lpc_msg    .text "Init SuperIO...", $0D, $00
init_kbrd_msg   .text "Init Keyboard...", $0D, $00
init_via_msg    .text "Init VIAs...", $0D, $00
init_rtc_msg    .text "Init RTC...", $0D, $00
test_SID_msg    .text "Testing Right & Left SID", $0D, $00

ready_msg       .null $0D,"READY."
hello_basic     .null "10 PRINT ""Hello World""",$0D
                .null "RUN",$0D
                .null "Hello World",$0D
                .null $0D,"READY."
hello_ml        .null "G 020000",$0D
                .null "HELLO WORLD",$0D
                .null $0D
                .null " PC     A    X    Y    SP   DBR DP   NVMXDIZC",$0D
                .null ";002112 0019 F0AA 0000 D6FF F8  0000 --M-----"
error_01        .null "ABORT ERROR"
hex_digits      .text "0123456789ABCDEF",0
