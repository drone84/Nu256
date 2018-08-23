; Set 8-bit accumulator
setas           .macro
                SEP #$20
                .as
                .endm
                
; Set 16-bit accumulator
setal           .macro
                REP #$20
                .al
                .endm
; Set 8 bit index registers               
setxs           .macro
                SEP #$10
                .xs
                .endm
                
; Set 16-bit index registers
setxl           .macro
                REP #$10
                .xl
                .endm

; Set the direct page. 
; Note: This uses the accumulator and leaves A set to 16 bits. 
setdp           .macro                
                PHA
		PHP
                setal
                LDA #\1         ; set DP to page 0
                TCD             ; and get character back
                .dpage \1
		PLP
                PLA
                .endm 

setdbr          .macro          ; Set the B (Data bank) register 
                PHA
                PHP
                setas
		LDA #\1         
                PHA
                PLB
                .databank \1
                PLP
                PLA
                .endm 

pushap          .macro
                PHA
                PHP
                .endm
pullap          .macro
                PLP
                PLA
                .endm

pushreg         .macro
                PHA
                PHX
                PHY
                PHP
                .endm
pullreg         .macro
                PLP
                PLY
                PLX
                PLA
                .endm

                        
                