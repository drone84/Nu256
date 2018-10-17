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
                setdp 0
                setdbr `MONITOR_DATA

                PRINT MREGISTERS_MSG
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
                setdp 0
                setdbr `MONITOR_VARS
                setaxl 

                ; Set the location for the parsed output
                LDA #<>MCMD
                STA WRITEPOS
                ; Get the start of the current line and copy that
                ; to the parse variables 
                LDX #0
                LDY CURSOR_Y
                JSL ILOCATE  
                LDA CURSORPOS
                STA READPOS   
                setas 
                ; Set the DBR to the screen bank 
                LDA CURSORPOS_B
                PHA
                PLB 
                STA READPOS_B ; Set bank # for read buffer
                STA WRITEPOS_B  ; Set the bank # for the parsed output

                ; read the command from the command line 
imparse_1       NOP
                setas
                JSL IMPARSECMD
                JSL IPRINTCR
                CMP #'?'
                BNE imparse_cmd_error
                PHD
                setdbr `MHELP_MSG
                print MHELP_MSG
                PLD
                BRA imparse_done

imparse_cmd_error 
                setdbr `MMERROR
                PRINT MMERROR

imparse_done                
                PLP
                RTL

;
; IMPARSECMD 
; Reads the current command on the command line 
; and places the first 4 characters in MCMD. 
; unused bytes in MCMD will be filled with 0.                
; Input: 
;  Y - address in Data Bank to read 
;  DBR - Bank of data to read. Should be the bank that contains
;   MONITOR_VARS
;  READPOS - Location to read 
;  READEND - The next address after the end of the input buffer (READPOS + length)
;  MCMD - Starting address of parsed data.
; Modifies:
;  A - Returns character read or 0 if read has passed the buffer length
;  READPOS - This will advance as the text is read 
;  X,Y,D,B,P - undefined. Save values you need to keep 
; Output: none. 
; Memory: 
;  MCMD - will be set to command text 
IMPARSECMD      .dpage 0
                .databank `MONITOR_VARS
                setaxl
                ;Clear the output location 
                STZ MCMD
                STZ MCMD+2
                ; Read the first character on the command line 
                LDA READPOS
                CLC
                ADC COLS_VISIBLE
                STA READEND 
                setas 
                JSL IMREADCHAR
impcmd_found    STA MCMD
                RTL 
                
;
; IMREADCHAR
;  Read from the address pointed to by READPOS. 
;  Returns the value in A. 
;  Sets Carry if reading has passed the end of the buffer. 
;  READPOS will be advanced to the next character to be read. 
; Input: 
;  DBR - Bank of data to read
;  READPOS - Location to read 
;  READEND - The next address after the end of the input buffer (READPOS + length)
;  P - m flag must be 1 (Short A)
; Modifies:
;  A - Returns character read or 0 if read has passed the buffer length.
;  P - Carry will be set if the read has passed the end of the buffer. 
;  READPOS - Will be set to the next character to be read. 
;
IMREADCHAR      ; First, test whether we have passed the end of the buffer. 
                LDA READPOS
                CMP READEND
                BCC imrc_1
                LDA 0
                SEC 
                BRA imrc_ns_done
                
                ; Read the character at (READPOS)
imrc_1          LDA (READPOS)
                INC READPOS 
imrc_ns_done    RTL        
                
;
; IMREADCHAR_ID
;  Read the characterr pointed to by Y in the data bank. 
;  Returns the value in A
;  Returns 0 if the value is not an identifier character. (A-Z, a-z, 0-9, _)
;         
IMREADCHAR_ID   LDA (READPOS)
                CMP #'0'
                BCC imcrid_loop
                CMP #'9'+1
                BCC imrcid_done
                CMP #'A'
                BCC imcrid_loop
                CMP #'Z'+1
                BCC imrcid_done
                CMP #'a'
                BCC imcrid_loop
                CMP #'z'+1
                BCC imrcid_done
                CMP '_'
                BEQ imrcid_done
                ; Character is not valid
                ; Increment the read counter and try again
imcrid_loop     INC READPOS 
                LDA READPOS
                CMP READEND
                BCC IMREADCHAR_ID
imrcid_0        LDA #0                  ; 
imrcid_done     RTL        
                
                
IMWRITEARG4     
                
                
                
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
MREGISTERS_MSG  .null $0D," PC     A    X    Y    SP   DBR DP   NVMXDIZC"
MHELP_MSG       .text "Cmd   Command      Params",$0D
                .text "A     ASSEMBLE     [Start] [Assembly code]",$0D
                .text "C     COMPARE      Start1 Start2 [Len (1 if blank)]",$0D
                .text "D     DISASSEMBLE  Start [End]",$0D
                .text "F     FILL         Start End Byte",$0D
                .text "G     GO           [Address]",$0D
                .text "J                  [Address]",$0D
                .text "H     HUNT (find)  Start End Byte [Byte]...",$0D
                .text "L     LOAD         ""File"" [Device] [Start]",$0D
                .text "M     MEMORY       [Start] [End]",$0D
                .text "R     REGISTERS    Register [Value]  (A 1234, F 00100011)",$0D
                .text ";                  PC A X Y SP DBR DP NVMXDIZC",$0D
                .text "S     SAVE         ""File"" Device Start End",$0D
                .text "T     TRANSFER     Start End Destination",$0D
                .text "V     VERIFY       ""File"" [Device] [Start]",$0D
                .text "X     EXIT",$0D
                .text ">     MODIFY       Start Byte [Byte]...",$0D
                .text "@     DOS          [Command] Returns drive status if no params.",$0D
                .text "?     HELP         Display a short help screen ",$0D
                .null 
