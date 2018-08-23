;
; Interrupt Handlers
;
* = $FFFF00     ; HRESET
                JML BOOT
* = $FFFF10     ; HCOP  
                JMP HBRK
* = $FFFF20     ; HBRK  - Handle BRK interrupt
RHBRK           setaxl
                PHB 
                PHD
                PHA
                PHX
                PHY
                JML BREAK

* = $FFFF30     ; HABORT
                
* = $FFFF40     ; HNMI  

* = $FFFF50     ; IRQ handler. 
RHIRQ           setaxl
                PHB
                PHD
                PHA
                PHX
                PHY
                ;
                ; todo: look up IRQ triggered and do stuff
                ;
                PLY
                PLX
                PLA
                PLD
                PLB
                RTI

* = $FFFFE0                     ; Native mode vectors
ROM_VECTORS     ; Initial CPU Vectors. These will be copied to the top of Direct Page
                ; during system boot
JUMP_READY      JML READY
RVECTOR_COP     .word $FF10     ; FFE4
RVECTOR_BRK     .word $FF20     ; FFE6
RVECTOR_ABORT   .word $FF30     ; FFE8
RVECTOR_NMI     .word $FF40     ; FFEA
                .word $0000     ; FFEC
RVECTOR_IRQ     .word $FF50     ; FFEE

                .word $0000     ; FFF0
                .word $0000     ; FFF2

RVECTOR_ECOP    .word $FF10     ; FFF4
RVECTOR_EBRK    .word $FF20     ; FFF6
RVECTOR_EABORT  .word $FF30     ; FFF8
RVECTOR_ENMI    .word $FF40     ; FFFA
RVECTOR_ERESET  .word $FF00     ; FFFC
RVECTOR_EIRQ    .word $FF50     ; FFFE
