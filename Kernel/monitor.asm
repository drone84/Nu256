.cpu "65816"

;ShorT Command                            Params
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

* = $F90000

;Monitor.asm
;Jump Table
MONITOR         JML IMONITOR
MBREAK          JML IMBREAK
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


* = $F90400
IMONITOR        ; monitor entry point. This initializes the monitor
                ; and prints the prompt.
                ; Make sure 16 bit mode is turned on
                CLC           ; clear the carry flag
                XCE           ; move carry to emulation flag.
                REP #$10      ; set 16-bit index registers
                SEP #$20      ; set 8 bit accumulator



IMBREAK         BRK ; Warm boot routine
IMSTATUS        BRK ; Print status message
IMREADY         BRK ; Prints status message and waits for input
IMRETURN        BRK ; Handle RETURN key (ie: execute command)
IMPARSE         BRK ; Parse the current command line
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

; 
; MMESSAGES
; MONITOR messages and responses.                
MMESSAGES
MMERROR         .text                 