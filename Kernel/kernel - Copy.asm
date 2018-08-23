.cpu "65816"
.include "macros_inc.asm"
.include "directpage_inc.asm"
.include "monitor_inc.asm"
.include "kernel_vectors.asm"

; C256 Foenix / Nu64 Kernel
; Loads to $F0:0000

* = $F80000

BOOT        JML IBOOT
;RESTORE       JML IRESTORE
READY         JML IREADY
;SCINIT        JML ISCINIT
;IOINIT        JML IIOINIT
;PUTC          JML IPUTC
;PUTS          JML IPUTS
;PUTB          JML IPUTB
;OPEN          JML IOPEN
;CLOSE         JML ICLOSE
;SETIN         JML ISETIN
;SETOUT        JML ISETOUT
;GETCH         JML IGETCH
;GETCHE        JML IGETCHE
;GETCHW        JML IGETCHW
;GETS          JML IGETS
;GETLINE       JML IGETLINE
;GETFIELD      JML IGETFIELD
;GETB          JML IGETB
;TRIM          JML ITRIM
;SPUTCH        JML ISPUTCH
;SPUTS         JML ISPUTS
;SGETCH        JML ISGETCH
;SGETS         JML ISGETS
;LOCATE        JML ILOCATE
;PRINTI4       JML IPRINTI4
;PRINTI        JML IPRINTI
;PRINTH        JML IPRINTH
;PRINTH2       JML IPRINTH2
;PRINTH3       JML IPRINTH3
;PRINTH4       JML IPRINTH4
;TYPE          JML ITYPE

* = $F81000
IRESET          JMP IBOOT

IBOOT           ; boot the system
                CLC           ; clear the carry flag
                XCE           ; move carry to emulation flag.
                setxl
                setal
                LDA #$0000      ; init direct page
                TCD
                setal           
                LDA #$1000      ; store the initial screen buffer location
                STA SCREENBEGIN
                setas
                LDA #$00
                STA SCREENBEGIN+2
                setal           
                LDA #$1000      ; store the initial cursor position
                STA CURSORPOS
                setas
                LDA #$00
                STA CURSORPOS+2
                setas
                LDA #80         ; Set screen dimensions
                STA SCRWIDTH
                LDA #25
                STA SCRHEIGHT
                
                ; display boot message 
greet           setdbr `greet_msg       ;Set data bank to ROM
                LDX #<>greet_msg
                JSL IPRINT       ; print the first line
                JSL IPRINT       ; print the second line
                JSL IPRINT       ; print the third line
                JSL IPRINTCR     ; print a blank line. Just because
                setas
                setdbr $01      ;set data bank to 1 (Kernel Variables) 
greet_done      NOP             ;halt the CPU

IREADY          setdbr `ready_msg
                LDX #<>ready_msg
                JSL IPRINT
                STP 
                
IKEYDOWN        STP             ; Keyboard key pressed
IRETURN         STP

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
iputs1          LDA $0,b,x ; read from the string
                BEQ iputs_done
		CMP #$0D   ; handle CR
		BNE iputs2
		JSL IPRINTCR
		BRA iputs3
iputs2          JSL IPUTC
iputs3          INX
                JMP iputs1
iputs_done      INX
                PLP
                PLA
                RTL

;
;IPUTC
; Print a single character
; Handles terminal sequences, based on the selected text mode
; Modifies: none
;
IPUTC           PHD
		PHP             ; stash the flags (we'll be changing M)
                setdp 0
		setas
                STA [CURSORPOS] ; Save the character on the screen                
                JSL ICSRRIGHT
                PLP
                PLD
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
                CPX SCRWIDTH
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
                          
ISRLEFT		RTL
ICSRUP		RTL
ICSRDOWN	RTL

;ILOCATE
;Sets the cursor X and Y positions to the X and Y registers
;Direct Page must be set to 0
;Modifies: none
ILOCATE         PHA
                PHP 
                setal
                setxl 
		STX CURSORX
		STY CURSORY 
                LDA SCREENBEGIN
                
ilocate_down    CLC
                ADC SCRWIDTH
                DEY 
		BEQ ilocate_right
                JMP ilocate_down
ilocate_right   ADC CURSORX             ; move the cursor right X columns
                STA CURSORPOS
		LDY CURSORY
ilocate_done    PLP
                PLA
                RTL

;
; Greeting message and other kernel boot data
;
* = $F8F000                
greet_msg       .text "  ///// NU64 DEVELOPMENT SYSTEM",$00
greet_msg1      .text " /////  NU64 BASIC (Not Implemented)",$00
greet_msg2      .text "/////   Machine Monitor v0.1 (dev)",$00
ready_msg       .text "READY.",$00
error_01        .text "ABORT ERROR",$00