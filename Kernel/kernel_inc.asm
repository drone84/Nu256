.cpu "65816"

;Kernel_INC.asm
;Kernel ROM jump table
BOOT             = $F80000 ; Cold boot routine
RESTORE          = $F80004 ; Warm boot routine
BREAK            = $F80008 ; End program and return to command prompt
READY            = $F8000C ; Print prompt and wait for keyboard input
SCINIT           = $F80010 ; 
IOINIT           = $F80014 ; 
PUTC             = $F80018 ; Print a character to the currently selected channel
PUTS             = $F8001C ; Print a string to the currently selected channel
PUTB             = $F80020 ; Output a byte to the currently selected channel
PUTBLOCK         = $F80024 ; Ouput a binary block to the currently selected channel
SETLFS           = $F80028 ; Obsolete (done in OPEN)
SETNAM           = $F8002C ; Obsolete (done in OPEN)
OPEN             = $F80030 ; Open a channel for reading and/or writing. Use SETLFS and SETNAM to set the channels and filename first. 
CLOSE            = $F80034 ; Close a channel
SETIN            = $F80038 ; Set the current input channel
SETOUT           = $F8003C ; Set the current output channel
GETB             = $F80040 ; Get a byte from input channel. Return 0 if no input. Carry is set if no input.
GETBLOCK         = $F80044 ; Get a X byes from input channel. If Carry is set, wait. If Carry is clear, do not wait.
GETCH            = $F80048 ; Get a character from the input channel. Wait if Carry is set. Return 0 if no input. Carry set if no input.
GETCHE           = $F8004C ; Get a character from the input channel and echo to the screen. Wait if data is not ready.
GETS             = $F80050 ; Get a string from the input channel. NULL terminates
GETLINE          = $F80054 ; Get a line of text from input channel. CR or NULL terminates.
GETFIELD         = $F80058 ; Get a field from the input channel. Value in A, CR, or NULL terminates
TRIM             = $F8005C ; Removes spaces at beginning and end of string. 
PRINTC           = $F80060 ; Print character to screen. Handles terminal commands
PRINTS           = $F80064 ; Print string to screen. Handles terminal commands
PRINTCR          = $F80068 ; Print Carriage Return
PRINTF           = $F8006C ; Print a float value
PRINTI           = $F80070 ; Prints integer value in TEMP
PRINTH           = $F80074 ; Print Hex value in DP variable
PRINTAI          = $F80078 ; Prints integer value in A
PRINTAH          = $F8007C ; Prints hex value in A. Printed value is 2 wide if M flag is 1, 4 wide if M=0
LOCATE           = $F80080 ; 
PUSHKEY          = $F80084 ; 
PUSHKEYS         = $F80088 ; 
CSRRIGHT         = $F8008C ; 
CSRLEFT          = $F80090 ; 
CSRUP            = $F80094 ; 
CSRDOWN          = $F80098 ; 
CSRHOME          = $F8009C ; 
