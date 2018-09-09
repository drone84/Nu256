.cpu "65816"
.include "macros_inc.asm"
.include "simulator_inc.asm"
.include "directpage_inc.asm"
.include "monitor.asm"
.include "kernel_bank_FF.asm"


; C256 Foenix / Nu64 Kernel
; Loads to $F0:0000

* = $F80000

;Kernel.asm
;Jump Table
;Kernel.asm
;Jump Table
BOOT            JML IBOOT
RESTORE         JML IRESTORE
BREAK           JML IBREAK
READY           JML IREADY
SCINIT          JML ISCINIT
IOINIT          JML IIOINIT
PUTC            JML IPUTC
PUTS            JML IPUTS
PUTB            JML IPUTB
PUTBLOCK        JML IPUTBLOCK
SETLFS          JML ISETLFS
SETNAM          JML ISETNAM
OPEN            JML IOPEN
CLOSE           JML ICLOSE
SETIN           JML ISETIN
SETOUT          JML ISETOUT
GETB            JML IGETB
GETBLOCK        JML IGETBLOCK
GETCH           JML IGETCH
GETCHW          JML IGETCHW
GETCHE          JML IGETCHE
GETS            JML IGETS
GETLINE         JML IGETLINE
GETFIELD        JML IGETFIELD
TRIM            JML ITRIM
PRINTC          JML IPRINTC
PRINTS          JML IPRINTS
PRINTCR         JML IPRINTCR
PRINTF          JML IPRINTF
PRINTI          JML IPRINTI
PRINTH          JML IPRINTH
PRINTAI         JML IPRINTAI
PRINTAH         JML IPRINTAH
LOCATE          JML ILOCATE
PUSHKEY         JML IPUSHKEY
PUSHKEYS        JML IPUSHKEYS
CSRRIGHT        JML ICSRRIGHT
CSRLEFT         JML ICSRLEFT
CSRUP           JML ICSRUP
CSRDOWN         JML ICSRDOWN
CSRHOME         JML ICSRHOME
SCROLLUP        JML ISCROLLUP

* = $F80400
IBOOT           ; boot the system
                CLC           ; clear the carry flag
                XCE           ; move carry to emulation flag.
                setaxl
                LDA #STACK_END   ; initialize stack pointer 
                TAS 
                setdp 0
                LDA #$1000      ; store the initial screen buffer location
                STA SCREENBEGIN
                setas
                LDA #$00
                STA SCREENBEGIN+2
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
                setal
                ; set the location of the cursor (top left corner of screen)
                LDX #$0
                LDY #$0
                JSL ILOCATE
                ; reset keyboard buffer
                STZ KEY_BUFFER_RPOS
                STZ KEY_BUFFER_WPOS
                ; Copy vectors from ROM to Direct Page
                setaxl 
                LDA #$FF
                LDX #$FF00
                LDY #$FF00
                MVP $00, $FF 
                
                ; display boot message 
greet           setdbr `greet_msg       ;Set data bank to ROM
                LDX #<>greet_msg
                JSL IPRINT       ; print the first line
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
                BCS IREADYWAIT
                JSL IPUTC
                JMP IREADYWAIT
                
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
IPRINTH         BRK ; Print Hex value in DP variable
IPRINTAI        BRK ; Prints integer value in A
IPRINTAH        BRK ; Prints hex value in A. Printed value is 2 wide if M flag is 1, 4 wide if M=0
IPUSHKEY        BRK ; 
IPUSHKEYS       BRK ; 
ICSRLEFT        BRK ; 
ICSRHOME        BRK ; 
                
;
; Greeting message and other kernel boot data
;
* = $F8F000                
greet_msg       .text "  ///// FOENIX 256 DEVELOPMENT SYSTEM",$0D
greet_msg1      .text " /////  OPEN SOURCE COMPUTER",$0D
greet_msg2      .null "/////   8192KB SYSTEM 8128KB FREE"
ready_msg       .null $0D,"READY."
;ready_msg       .null " PC     A    X    Y    SP   DBR DP   NVMXDIZC",$0D
;                .null ";F81000 0000 0000 0000 D6FF F8  0000 ------Z-"
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

