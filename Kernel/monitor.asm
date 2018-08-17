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

			* = $0000

MONITOR     JML IMONITOR
MBREAK      JML IMBREAK
MSTATUS     JML IMSTATUS
MREADY      JML IMREADY
MRETURN		JML IMRETURN 
MPARSE      JML IMPARSE
;MPARSE1       JML IMPARSE1
;MEXECUTE      JML IMEXECUTE
;MASSEMBLE     JML IMASSEMBLE
;MCOMPARE      JML IMCOMPARE
;MDISASSEMBLE  JML IMDISASSEMBLE
;MFILL         JML IMFILL
;MGO           JML IMGO
;MHUNT         JML IMHUNT
;MLOAD         JML IMLOAD
;MMEMORY       JML IMMEMORY
;MREGISTERS    JML IMREGISTERS
;MSAVE         JML IMSAVE
;MTRANSFER     JML IMTRANSFER
;MVERIFY       JML IMVERIFY
;MEXIT         JML IMEXIT
;MMODIFY       JML IMMODIFY
;MDOS          JML IMDOS



			* = $F10000
IMONITOR                    ; monitor entry point. This initializes the monitor
                            ; and prints the prompt.
                            ; Make sure 16 bit mode is turned on
            CLC           ; clear the carry flag
            XCE           ; move carry to emulation flag.
            REP #$10      ; set 16-bit index registers
            SEP #$20      ; set 8 bit accumulator


IMBREAK       
IMSTATUS
IMREADY

IMRETURN
IMPARSE
