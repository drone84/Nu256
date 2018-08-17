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
                setal
                PHA
                LDA #\1         ; set DP to page 0
                TCD             ; and get character back
                .dpage \1
                PLA
                .endm 

setdb           .macro          ; Set the B (Data bank) register 
                PHP
                setal
                PHA
                LDA #\1         
                PHA
                PLB
                .databank \1
                PLA
                PLP
                .endm 
                                