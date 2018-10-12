.cpu "65816"

;Cmd   Command      Params
;A     ASSEMBLE     [Start] [Assembly code]
;C     COMPARE      Start1 Start2 [Len (1 if blank)]
;D     DISASSEMBLE  Start [End]
;F     FILL         Start End Byte
;G     GO           [Address]
;J                  [Address]
;H     HUNT (find)  Start End Byte [Byte]...
;L     LOAD         "File" [Device] [Start]
;M     MEMORY       [Start] [End]
;R     REGISTERS    Register [Value]  (A 1234, F 00100011)
;;                  PC A X Y SP DBR DP NVMXDIZC
;S     SAVE         "File" Device Start End
;T     TRANSFER     Start End Destination
;V     VERIFY       "File" [Device] [Start]
;X     EXIT
;>     MODIFY       Start Byte [Byte]...
;@     DOS          [Command] Returns drive status if no params.
;?     HELP         Display a short help screen 

;Monitor.asm
;Jump Table
* = $018000
MONITOR         JML IMONITOR
MSTATUS         JML IMSTATUS
MREADY          JML IMREADY
MRETURN         JML IMRETURN
MPARSE          JML IMPARSE
MPARSE1         JML IMPARSE1
MEXECUTE        JML IMEXECUTE
MASSEMBLE       JML IMASSEMBLE
MASSEMBLEA      JML IMASSEMBLEA
MCOMPARE        JML IMCOMPARE
MDISASSEMBLE    JML IMDISASSEMBLE
MFILL           JML IMFILL

MJUMP           JML IMJUMP
MHUNT           JML IMHUNT
MLOAD           JML IMLOAD
MMEMORY         JML IMMEMORY
MREGISTERS      JML IMREGISTERS
MSAVE           JML IMSAVE
MTRANSFER       JML IMTRANSFER
MVERIFY         JML IMVERIFY
MEXIT           JML IMEXIT
MMODIFY         JML IMMODIFY
MDOS            JML IMDOS

;
; IMONITOR
; monitor entry point. This initializes the monitor
; and prints the prompt.
; Make sure 16 bit mode is turned on
;
IMONITOR        CLC           ; clear the carry flag
                XCE           ; move carry to emulation flag.
                LDA #STACK_END ; Reset the stack
                TAS 
                JML IMREADY 
                
;
; IMREADY
; Print the status prompt, then wait for input
;
IMREADY         ;set the READY handler to jump here instead of BASIC
                setdbr `JMP_READY
                setaxl 
                LDA #<>IMREADY
                STA JMP_READY+1
                setas
                LDA #`IMREADY
                STA JMP_READY+3

                ;set the RETURN vector and then wait for keyboard input
                setal
                LDA #<>IMRETURN
                STA RETURN+1
                setas
                LDA #`IMRETURN
                STA RETURN+3

                JML IMSTATUS

;
; IMSTATUS
; Prints the regsiter status
; Reads the saved register values at CPU_REGISTERS
;
; PC     A    X    Y    SP   DBR DP   NVMXDIZC
; 000000 0000 0000 0000 0000 00  0000 00000000       
;
; Arguments: none
; Modifies: A,X,Y
IMSTATUS        ; Print the MONITOR prompt (registers header)
                setdbr `MONITOR_DATA

                PRINT mregisters_msg
                setxl
                setas
                setdbr $0
                LDA #';'
                JSL IPUTC
                
                ; print Program Counter
                LDY #3
                LDX #CPUPC+2
                JSL IPRINTH
                
                ; print A register
                setas 
                LDA #' '
                JSL IPUTC
                LDY #2
                LDX #CPUA+1
                JSL IPRINTH
                
                ; print X register
                setas 
                LDA #' '
                JSL IPUTC
                LDY #2
                LDX #CPUX+1
                JSL IPRINTH
                
                ; print Y register
                setas 
                LDA #' '
                JSL IPUTC
                LDY #2
                LDX #CPUY+1
                JSL IPRINTH
                
                ; print Stack Pointer
                setas 
                LDA #' '
                JSL IPUTC
                LDY #2
                LDX #CPUSTACK+1
                JSL IPRINTH
                
                ; print DBR
                setas 
                LDA #' '
                JSL IPUTC
                LDY #1
                LDX #CPUDBR
                JSL IPRINTH
                
                ; print Direct Page
                setas 
                LDA #' '
                JSL IPUTC
                JSL IPUTC
                LDY #2
                LDX #CPUDP+1
                JSL IPRINTH
                
                ; print Flags
                setas 
                LDA #' '
                JSL IPUTC
                LDY #1
                LDX #CPUFLAGS
                JSL IPRINTH
                
                JSL IPRINTCR
                
                JML IREADYWAIT

;
; IMRETURN 
; Handles the RETURN key.
; This will process the line of text under the cursor, spliting arguments
; and placing them in the correct variables in Direct Page
; It will then execute the command.                 
IMRETURN        JSL IMPARSE 
                RTL 
;
; IMPARSE
; Parse the current command line. 
; Places the command in MCMD  
; Places the numeric arguments in MARG1-MARG8
; If the command has string arguments, the value in MARG will be the address+length of the string
; Registers affected: All. Save any register data before calling this procedure. 
IMPARSE         PHP
                setaxl 
                setdp 0
                
                ; MCMP_LEN stores the number of bytes parsed 
                ; if this passes the edge of the screen, complete the parse operation. 
                LDA #0
                STA MCMP_LEN
                ; Get the start of the current line and copy that
                ; to the parse variables 
                LDX #0
                LDY CURSOR_Y
                JSL ILOCATE  
                LDA CURSORPOS
                STA MCMP_TEXT
                STA MCMDADDR
                setas 
                LDA CURSORPOS+2
                STA MCMP_TEXT+2
                STA MCMDADDR+2
                ; Set the DBR to the screen bank 
                PHA
                PLB 
                
                JSL IPRINTCR
                PRINT MMERROR
                
                PLP
                RTL

;
; IMPARSECMD 
; Reads the current command on the command line 
; and places the first 4 characters in MCMD. 
; unused bytes in MCMD will be filled with 0.                
; Input: DBR - bank of text to parse 
; Modifies: All.
; Output: none. 
; Memory: 
;   MCMP_TEXT - starting address of text to parse
;   MCMD - will be set to command text 
IMPARSECMD      setaxl
                LDY MCMP_TEXT,B
                setas 
                JSL IMSEEK_TEXT
impcmd_found    STA MCMD
                RTL 
;
; IMSEEK_TEXT
; Advance the Y register until the address in the data bank 
; points to a non-whitespace character value (whitespace is anything <= $20)       
;         
IMSEEK_TEXT     
                LDA 0,b,y
                CMP #$20
                BCS IMSEEK_TEXT_DONE
                INY
                ; 
                LDX 
                INX 
                CPX COLS_VISIBLE
                BCS IMSEEK_TEXT
                        
                
                

IMPARSE1        BRK ; Parse one word on the current command line
IMEXECUTE       BRK ; Execute the current command line (requires MCMD and MARG1-MARG8 to be populated)
IMASSEMBLE      BRK ; Assemble a line of text. 
IMASSEMBLEA     BRK ; Assemble a line of text. 
IMCOMPARE       BRK ; Compare memory. len=1
IMDISASSEMBLE   BRK ; Disassemble memory. end=1 instruction
IMFILL          BRK ; Fill memory with specified value. Start and end must be in the same bank. 
IMGO            BRK ; Execute from specified address
IMJUMP          BRK ; Execute from spefified address
IMHUNT          BRK ; Hunt (find) value in memory
IMLOAD          BRK ; Load data from disk. Device=1 (internal floppy) Start=Address in file
IMMEMORY        BRK ; View memory
IMREGISTERS     BRK ; View/edit registers
IMSAVE          BRK ; Save memory to disk
IMTRANSFER      BRK ; Transfer (copy) data in memory
IMVERIFY        BRK ; Verify memory and file on disk
IMEXIT          BRK ; Exit monitor and return to BASIC command prompt
IMMODIFY        BRK ; Modify memory 
IMDOS           BRK ; Execute DOS command 

; 
; MMESSAGES
; MONITOR messages and responses.                
MONITOR_DATA     
MMERROR         .null "?ERROR"
mregisters_msg  .null $0D," PC     A    X    Y    SP   DBR DP   NVMXDIZC"
mhelp_msg       .null "Cmd   Command      Params",$0D
                .null "A     ASSEMBLE     [Start] [Assembly code]",$0D
                .null "C     COMPARE      Start1 Start2 [Len (1 if blank)]",$0D
                .null "D     DISASSEMBLE  Start [End]",$0D
                .null "F     FILL         Start End Byte",$0D
                .null "G     GO           [Address]",$0D
                .null "J                  [Address]",$0D
                .null "H     HUNT (find)  Start End Byte [Byte]...",$0D
                .null "L     LOAD         "File" [Device] [Start]",$0D
                .null "M     MEMORY       [Start] [End]",$0D
                .null "R     REGISTERS    Register [Value]  (A 1234, F 00100011)",$0D
                .null ""                  PC A X Y SP DBR DP NVMXDIZC",$0D
                .null "S     SAVE         "File" Device Start End",$0D
                .null "T     TRANSFER     Start End Destination",$0D
                .null "V     VERIFY       "File" [Device] [Start]",$0D
                .null "X     EXIT",$0D
                .null ">     MODIFY       Start Byte [Byte]...",$0D
                .null "@     DOS          [Command] Returns drive status if no params.",$0D
                .null "?     HELP         Display a short help screen ",$0D
                
       