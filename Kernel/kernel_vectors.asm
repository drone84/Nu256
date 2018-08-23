;
; Interrupt Handlers
;
* = $FFFF00     ; HRESET
                JML BOOT
* = $FFFF10     ; HCOP  
                JMP HBRK
* = $FFFF20     ; HBRK  - Handle BRK interrupt
RHBRK           setal
                LDA #STACK_END ; Reset the stack pointer to its location at boot
                TAS 
                JML READY

* = $FFFF30     ; HABORT
                
* = $FFFF40     ; HNMI  

* = $FFFF50     ; IRQ handler. 
RHIRQ           PHD
                PHA
                PHX
                PHY
                PHP
                ;
                ; todo: look up IRQ triggered and do stuff
                ;
                PLP
                PLY
                PLX
                PLA
                PLD
                RTI

* = $FFFFE4                     ; Native mode vectors
ROM_VECTORS     ; Initial CPU Vectors. These will be copied to the top of Direct Page
                ; during system boot
RVECTOR_COP     .word $FF00     ; FFE4
RVECTOR_BRK     .word $FF10     ; FFE6
RVECTOR_ABORT   .word $FF20     ; FFE8
RVECTOR_NMI     .word $FF30     ; FFEA
                .word $0000     ; FFEC
RVECTOR_IRQ     .word $FF40     ; FFEE

                .word $0000     ; FFF0
                .word $0000     ; FFF2

RVECTOR_ECOP    .word $FF50     ; FFF4
RVECTOR_EBRK    .word $FF10     ; FFF6
RVECTOR_EABORT  .word $FF60     ; FFF8
RVECTOR_ENMI    .word $FF70     ; FFFA
RVECTOR_ERESET  .word $FF80     ; FFFC
RVECTOR_EIRQ    .word $FF90     ; FFFE
