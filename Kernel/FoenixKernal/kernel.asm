.cpu "65816"
.include "macros_inc.asm"
.include "directpage_inc.asm"
.include "monitor_inc.asm"

; C256 Foenix / Nu64 Kernel
; Loads to $F0:0000

* = $F00000

BOOT        JML IBOOT
RESET       JML IRESET
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

* = $F01000
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
                
                ; display boot message
greet           LDA #$F0        ;Set data bank to $F0
                PHA             ;
                PLB             ;
                .databank $F0
                LDX #<>greet_msg
                LDY #40
                JSR IPUTS       ; print the string

                setas
                LDA #$01        ;set data bank to 1 (Kernel Variables)
                PHA
                PLB
                STP
                
greet_done      BRK             ;halt the CPU

IKEYDOWN        BRK             ; Keyboard key pressed

IRETURN         BRK

IPUTC           ; Print  character
                ; A: character to print
                PHP             ; stash the flags (we'll be changing M)
                PHD
                setas
                PHA
                setal
                LDA #$100       ; set DP to page 0
                TCD             ; and get character back
                .dpage $100
                setas
                PLA
                STA [CURSORPOS] ; Save the character on the screen                
                INC CURSORPOS   ; Move the cursor to the next position
                INC CURSORX     ; 
                PLD
                PLP
                RTS
                
IPUTS           ; Print a null terminated string
                ; DBR: bank containing string
                ; X: address of the string in data bank
                ; Y: maximum number of characters to write
                ; Modifies: X,Y
                PHA
                PHP
                setas
iputs1          LDA $0,b,x ; read from the string
                BEQ iputs_done
                JSR IPUTC
                INX
                DEY
                BEQ iputs_done
                jmp iputs1

iputs_done      PLP
                PLA
                RTS
                
greet_msg       .text "**///// NU64 DEVELOPMENT SYSTEM",$0d,$00
greet_msg1      .text "*/////* NU64 BASIC (Not Implemented)",$0d,$00
greet_msg2      .text "/////** Machine Monitor v0.1 (dev)",$0d,$00

