;Kernel_INC.asm
;Kernel ROM jump table

BOOT             = $010000 ; Cold boot routine
RESTORE          = $010004 ; Warm boot routine
BREAK            = $010008 ; End program and return to command prompt
READY            = $01000C ; Print prompt and wait for keyboard input
SCINIT           = $010010 ; 
IOINIT           = $010014 ; 
PUTC             = $010018 ; Print a character to the currently selected channel
PUTS             = $01001C ; Print a string to the currently selected channel
PUTB             = $010020 ; Output a byte to the currently selected channel
PUTBLOCK         = $010024 ; Ouput a binary block to the currently selected channel
SETLFS           = $010028 ; Obsolete (done in OPEN)
SETNAM           = $01002C ; Obsolete (done in OPEN)
OPEN             = $010030 ; Open a channel for reading and/or writing. Use SETLFS and SETNAM to set the channels and filename first. 
CLOSE            = $010034 ; Close a channel
SETIN            = $010038 ; Set the current input channel
SETOUT           = $01003C ; Set the current output channel
GETB             = $010040 ; Get a byte from input channel. Return 0 if no input. Carry is set if no input.
GETBLOCK         = $010044 ; Get a X byes from input channel. If Carry is set, wait. If Carry is clear, do not wait.
GETCH            = $010048 ; Get a character from the input channel. A=0 and Carry=1 if no data is wating 
GETCHW           = $01004C ; Get a character from the input channel. Waits until data received. A=0 and Carry=1 if no data is wating 
GETCHE           = $010050 ; Get a character from the input channel and echo to the screen. Wait if data is not ready.
GETS             = $010054 ; Get a string from the input channel. NULL terminates
GETLINE          = $010058 ; Get a line of text from input channel. CR or NULL terminates.
GETFIELD         = $01005C ; Get a field from the input channel. Value in A, CR, or NULL terminates
TRIM             = $010060 ; Removes spaces at beginning and end of string. 
PRINTC           = $010064 ; Print character to screen. Handles terminal commands
PRINTS           = $010068 ; Print string to screen. Handles terminal commands
PRINTCR          = $01006C ; Print Carriage Return
PRINTF           = $010070 ; Print a float value
PRINTI           = $010074 ; Prints integer value in TEMP
PRINTH           = $010078 ; Print Hex value in DP variable
PRINTAI          = $01007C ; Prints integer value in A
PRINTAH          = $010080 ; Prints hex value in A. Printed value is 2 wide if M flag is 1, 4 wide if M=0
LOCATE           = $010084 ; 
PUSHKEY          = $010088 ; 
PUSHKEYS         = $01008C ; 
CSRRIGHT         = $010090 ; 
CSRLEFT          = $010094 ; 
CSRUP            = $010098 ; 
CSRDOWN          = $01009C ; 
CSRHOME          = $0100A0 ; 
SCROLLUP         = $0100A4 ; Scroll the screen up one line. Creates an empty line at the bottom.
SCRREADLINE      = $0100A8 ; Loads the MCMDADDR/BCMDADDR variable with the address of the current line on the screen. This is called when the RETURN key is pressed and is the first step in processing an immediate mode command.
SCRGETWORD       = $0100AC ; Read a current word on the screen. A word ends with a space, punctuation (except _), or any control character (value < 32). Loads the address into CMPTEXT_VAL and length into CMPTEXT_LEN variables.
CLRSCREEN        = $0100B0 ; Clear the screen
INITCHLUT        = $0100B4 ; Init character look-up table
INITSUPERIO      = $0100B8 ; Init Super-IO chip
INITKEYBOARD     = $0100BC ; Init keyboard