;MONITOR_INC.asm
;MONITOR ROM jump table
MONITOR          = $018000 ; Cold boot routine
MBREAK           = $018004 ; Warm boot routine
MSTATUS          = $018008 ; Print status message
MREADY           = $01800C ; Prints status message and waits for input
MRETURN          = $018010 ; Handle RETURN key (ie: execute command)
MPARSE           = $018014 ; Parse the current command line
MPARSE1          = $018018 ; Parse one word on the current command line
MEXECUTE         = $01801C ; Execute the current command line (requires MCMD and MARG1-MARG8 to be populated)
MASSEMBLE        = $018020 ; Assemble a line of text. 
MASSEMBLEA       = $018024 ; Assemble a line of text. 
MCOMPARE         = $018028 ; Compare memory. len=1
MDISASSEMBLE     = $01802C ; Disassemble memory. end=1 instruction
MFILL            = $018030 ; Fill memory with specified value. Start and end must be in the same bank. 

MJUMP            = $018038 ; Execute from spefified address
MHUNT            = $01803C ; Hunt (find) value in memory
MLOAD            = $018040 ; Load data from disk. Device=1 (internal floppy) Start=Address in file
MMEMORY          = $018044 ; View memory
MREGISTERS       = $018048 ; View/edit registers
MSAVE            = $01804C ; Save memory to disk
MTRANSFER        = $018050 ; Transfer (copy) data in memory
MVERIFY          = $018054 ; Verify memory and file on disk
MEXIT            = $018058 ; Exit monitor and return to BASIC command prompt
MMODIFY          = $01805C ; Modify memory
MDOS             = $018060 ; Execute DOS command 
