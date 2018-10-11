;
;ASM Declarations
;
IO_BEGIN             = $7FD800 ; Byte  Beginning of IO space
; 
; Internal VICKY Registers and Internal Memory Locations (LUTs)
; 
MASTER_CTRL_REG_L    = $7F0000 ;1 Byte  
MASTER_CTRL_REG_H    = $7F0001 ;1 Byte  
TXT_CTRL_REG         = $7F0008 ;2 Bytes 
TXT_CTRL_REG_L       = $7F0008 ;1 Byte  
TXT_CTRL_REG_H       = $7F0009 ;1 Byte  

; 
; Cursor enable and flash rate control
; 
TXT_CURSOR_CTRL      = $7F000A ;1 Byte  
TXT_CURSOR_ENABLE    = $01 ; Byte  Bit 0: Flash Enabled
TXT_CURSOR_FLASH_0   = $00 ; Byte  Bits 1-2: Cursor Flash: 0-Transparent
TXT_CURSOR_FLASH_1   = $02 ; Byte  Bits 1-2: Cursor Flash: 1-Full On
TXT_CURSOR_FLASH_2   = $04 ; Byte  Bits 1-2: Cursor Flash: 2-1Hz
TXT_CURSOR_FLASH_3   = $06 ; Byte  Bits 1-2: Cursor Flash: 3-2Hz

TXT_CURSOR_COLOR     = $7F000B ;1 Byte  
TXT_CURSOR_X_REG_L   = $7F000C ;1 Byte  
TXT_CURSOR_X_REG_H   = $7F000D ;1 Byte  
TXT_CURSOR_Y_REG_L   = $7F000E ;1 Byte  
TXT_CURSOR_Y_REG_H   = $7F000F ;1 Byte  

PME_STS_REG          = $7F1100 ;1 Byte  
PME_EN_REG           = $7F1102 ;1 Byte  
PME_STS1_REG         = $7F1104 ;1 Byte  
PME_STS2_REG         = $7F1105 ;1 Byte  
PME_STS3_REG         = $7F1106 ;1 Byte  
PME_STS4_REG         = $7F1107 ;1 Byte  
PME_STS5_REG         = $7F1108 ;1 Byte  
PME_EN1_REG          = $7F110A ;1 Byte  
PME_EN2_REG          = $7F110B ;1 Byte  
PME_EN3_REG          = $7F110C ;1 Byte  
PME_EN4_REG          = $7F110D ;1 Byte  
PME_EN5_REG          = $7F110E ;1 Byte  
SMI_STS1_REG         = $7F1110 ;1 Byte  
SMI_STS2_REG         = $7F1111 ;1 Byte  
SMI_STS3_REG         = $7F1112 ;1 Byte  
SMI_STS4_REG         = $7F1113 ;1 Byte  
SMI_STS5_REG         = $7F1114 ;1 Byte  
SMI_EN1_REG          = $7F1116 ;1 Byte  
SMI_EN2_REG          = $7F1117 ;1 Byte  
SMI_EN3_REG          = $7F1118 ;1 Byte  
SMI_EN4_REG          = $7F1119 ;1 Byte  
SMI_EN5_REG          = $7F111A ;1 Byte  
MSC_ST_REG           = $7F111C ;1 Byte  

FORCE_DISK_CHANGE    = $7F111E ;1 Byte  
FLOPPY_DATA_RATE     = $7F111F ;1 Byte  

UART1_FIFO_CTRL_SHDW = $7F1120 ;1 Byte  
UART2_FIFO_CTRL_SHDW = $7F1121 ;1 Byte  
DEV_DISABLE_REG      = $7F1122 ;1 Byte  
GP10_REG             = $7F1123 ;1 Byte  
GP11_REG             = $7F1124 ;1 Byte  
GP12_REG             = $7F1125 ;1 Byte  
GP13_REG             = $7F1126 ;1 Byte  
GP14_REG             = $7F1127 ;1 Byte  
GP15_REG             = $7F1128 ;1 Byte  
GP16_REG             = $7F1129 ;1 Byte  
GP17_REG             = $7F112A ;1 Byte  
GP20_REG             = $7F112B ;1 Byte  
GP21_REG             = $7F112C ;1 Byte  
GP22_REG             = $7F112D ;1 Byte  

GP24_REG             = $7F112F ;1 Byte  
GP25_REG             = $7F1130 ;1 Byte  
GP26_REG             = $7F1131 ;1 Byte  
GP27_REG             = $7F1132 ;1 Byte  
GP30_REG             = $7F1133 ;1 Byte  
GP31_REG             = $7F1134 ;1 Byte  
GP32_REG             = $7F1135 ;1 Byte  
GP33_REG             = $7F1136 ;1 Byte  
GP34_REG             = $7F1137 ;1 Byte  
GP35_REG             = $7F1138 ;1 Byte  
GP36_REG             = $7F1139 ;1 Byte  
GP37_REG             = $7F113A ;1 Byte  
GP40_REG             = $7F113B ;1 Byte  
GP41_REG             = $7F113C ;1 Byte  
GP42_REG             = $7F113D ;1 Byte  
GP43_REG             = $7F113E ;1 Byte  
GP50_REG             = $7F113F ;1 Byte  
GP51_REG             = $7F1140 ;1 Byte  
GP52_REG             = $7F1141 ;1 Byte  
GP53_REG             = $7F1142 ;1 Byte  
GP54_REG             = $7F1143 ;1 Byte  
GP55_REG             = $7F1144 ;1 Byte  
GP56_REG             = $7F1145 ;1 Byte  
GP57_REG             = $7F1146 ;1 Byte  
GP60_REG             = $7F1147 ;1 Byte  
GP61_REG             = $7F1148 ;1 Byte  

GP1_REG              = $7F114B ;1 Byte  
GP2_REG              = $7F114C ;1 Byte  
GP3_REG              = $7F114D ;1 Byte  
GP4_REG              = $7F114E ;1 Byte  
GP5_REG              = $7F114F ;1 Byte  
GP6_REG              = $7F1150 ;1 Byte  

FAN1_REG             = $7F1156 ;1 Byte  
FAN2_REG             = $7F1157 ;1 Byte  
FAN_CTRL_REG         = $7F1158 ;1 Byte  
FAN1_TACH_REG        = $7F1159 ;1 Byte  
FAN2_TACH_REG        = $7F115A ;1 Byte  
FAN1_PRELOAD_REG     = $7F115B ;1 Byte  
FAN2_PRELOAD_REG     = $7F115C ;1 Byte  
LED1_REG             = $7F115D ;1 Byte  
LED2_REG             = $7F115E ;1 Byte  
KEYBOARD_SCAN_CODE   = $7F115F ;1 Byte  
                    ; 3552 bytes available
; 
; Color Lookup Tables
; 
FG_CHAR_LUT_PTR      = $7F1F40 ;64 Bytes 
BG_CHAR_LUT_PTR      = $7F1F80 ;64 Bytes 
GRPH_LUT0_PTR        = $7F2000 ;1024 Bytes 
GRPH_LUT1_PTR        = $7F2400 ;1024 Bytes 
GRPH_LUT2_PTR        = $7F2800 ;1024 Bytes 
GRPH_LUT3_PTR        = $7F2C00 ;1024 Bytes 
GRPH_LUT4_PTR        = $7F3000 ;1024 Bytes 
GRPH_LUT5_PTR        = $7F3400 ;1024 Bytes 
GRPH_LUT6_PTR        = $7F3800 ;1024 Bytes 
GAMMA_LUT_PTR        = $7F3C00 ;1024 Bytes 
                    ; 38912 bytes available
IO_GAVIN             = $7FD800 ;1024 Bytes GAVIN I/O space
IO_SUPERIO           = $7FDC00 ;1024 Bytes SuperIO I/O space
IO_VICKY             = $7FE000 ;1024 Bytes VICKY I/O space
IO_BEATRIX           = $7FE400 ;1024 Bytes BEATRIX I/O space
IO_RTC               = $7FE800 ;1024 Bytes RTC I/O space
IO_CIA               = $7FEC00 ;4864 Bytes CIA I/O space
                    ; 255 bytes available
IO_END               = $7FFFFF ; Byte  *End of I/O space
;
; End of Bank 7F
;