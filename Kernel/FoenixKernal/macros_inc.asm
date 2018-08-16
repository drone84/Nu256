; Sets 8-bit accumulator
setas           .macro
                SEP #$20
                .as
                .endm
                
                ; Sets 16-bit accumulator
setal           .macro
                REP #$20
                .al
                .endm
                
setxs           .macro
                SEP #$10
                .xs
                .endm
                
                ; Sets 16-bit accumulator
setxl           .macro
                REP #$10
                .xl
                .endm
