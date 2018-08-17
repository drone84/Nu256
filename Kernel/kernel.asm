.cpu "65816"
.include "macros_inc.asm"
.include "directpage_inc.asm"
.include "monitor_inc.asm"

; C256 Foenix / Nu64 Kernel
; Loads to $F0:0000

* = $F80000

BOOT        JML IBOOT
;RESTORE       JML IRESTORE
;READY         JML IREADY
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
greet           setdb $F8       ;Set data bank to ROM
                
                LDX #<>greet_msg
                JSL IPUTS       ; print the string
                LDX #<>greet_msg1
                JSL IPUTS       ; print the string
                LDX #<>greet_msg2
                JSL IPUTS       ; print the string
                ; LDY #40

                setas
                LDA #$01        ;set data bank to 1 (Kernel Variables)
                PHA
                PLB

waitloop	NOP
		JMP waitloop

                STP
                
greet_done      BRK             ;halt the CPU

IKEYDOWN        BRK             ; Keyboard key pressed

IRETURN         BRK

IPUTC           ; Print  character
                ; A: character to print
                PHP             ; stash the flags (we'll be changing M)
                PHD
                setdp 0
                setas
                STA [CURSORPOS] ; Save the character on the screen                
                JSL ICSRRIGHT
                PLD
                PLP
                RTL

ICSRRIGHT	; move the cursor right one space
                PHB
                setal 
                setxl
                PHX
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
                PLX
                PLB
                RTL
                          
ISRLEFT		RTL
ICSRUP		RTL
ICSRDOWN	RTL

ILOCATE         ;Sets the cursor X and Y positions to the X and Y registers
                PHB 
                setal
                setxl 
                PHA
                PHY
                LDA SCREENBEGIN
                STY CURSORY
                STX CURSORX
                
ilocate_down    CPY SCRWIDTH            ; move the cursor down Y rows
                BCC ilocate_right
                CLC
                ADC SCRWIDTH
                DEY 
                JMP ilocate_down
ilocate_right   ADC CURSORX             ; move the cursor right X columns
                STA CURSORPOS
                
ilocate_done    PLY
                PLA
                PLB
                RTL
                
IPUTS           ; Print a null terminated string
                ; DBR: bank containing string
                ; X: address of the string in data bank
                ; Y: maximum number of characters to write
                ; Modifies: X,Y
                PHP
                PHA
                setas
                setxl 
iputs1          LDA $0,b,x ; read from the string
                BEQ iputs_done
                JSL IPUTC
                INX
                DEY
                BEQ iputs_done
                jmp iputs1

iputs_done      PLA
                PLP
                RTL
                
greet_msg       .text "**///// NU64 DEVELOPMENT SYSTEM",$0d,$00
greet_msg1      .text "*/////* NU64 BASIC (Not Implemented)",$0d,$00
greet_msg2      .text "/////** Machine Monitor v0.1 (dev)",$0d,$00

