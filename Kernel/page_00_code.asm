;
; Page 0 Code: interrupt handlers and vectors 
;

;
; Interrupt Handlers
;
* = HRESET     ; HRESET
RHRESET         CLC
                XCE
                JML BOOT
                
* = HCOP       ; HCOP  
RHCOP           setaxl
                PHB 
                PHD
                PHA
                PHX
                PHY
                JML BREAK
                
* = HBRK       ; HBRK  - Handle BRK interrupt
RHBRK           setaxl
                PHB 
                PHD
                PHA
                PHX
                PHY
                JML BREAK

* = HABORT     ; HABORT
RHABORT         setaxl
                PHB 
                PHD
                PHA
                PHX
                PHY
                JML BREAK
                
* = HNMI       ; HNMI  
 RHNMI          setaxl
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
                
* = HIRQ       ; IRQ handler. 
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

;                
; Interrupt Vectors
;
* = VECTORS_BEGIN                
JUMP_READY      JML IMONITOR     ; Kernel READY routine. Rewrite this address to jump to a custom kernel.
RVECTOR_COP     .word HCOP     ; FFE4
RVECTOR_BRK     .word HBRK     ; FFE6
RVECTOR_ABORT   .word HABORT   ; FFE8
RVECTOR_NMI     .word HNMI     ; FFEA
                .word $0000    ; FFEC
RVECTOR_IRQ     .word HIRQ     ; FFEE

RRETURN         JML IRETURN

RVECTOR_ECOP    .word HCOP     ; FFF4
RVECTOR_EBRK    .word HBRK     ; FFF6
RVECTOR_EABORT  .word HABORT   ; FFF8
RVECTOR_ENMI    .word HNMI     ; FFFA
RVECTOR_ERESET  .word HRESET   ; FFFC
RVECTOR_EIRQ    .word HIRQ     ; FFFE
