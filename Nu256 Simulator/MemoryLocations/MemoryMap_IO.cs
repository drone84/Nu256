using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.MemoryLocations
{
    public static partial class MemoryMap
    {
        #region Internal I/O
        // c# Declarations

        public const int IO7F_BEGIN = 0x7FD800; //  Byte Beginning of IO space
                                                // 
                                                // Internal VICKY Registers and Internal Memory Locations (LUTs)
                                                // 
        public const int MASTER_CTRL_REG_L = 0x7F0000; // 1 Byte 
        public const int MASTER_CTRL_REG_H = 0x7F0001; // 1 Byte 
        public const int TXT_CTRL_REG = 0x7F0008; // 2 Bytes 
        public const int TXT_CTRL_REG_L = 0x7F0008; // 1 Byte 
        public const int TXT_CTRL_REG_H = 0x7F0009; // 1 Byte 

        // 
        // Cursor enable and flash rate control
        // 
        public const int TXT_CURSOR_CTRL = 0x7F000A; // 1 Byte 
        public const int TXT_CURSOR_ENABLE = 0x01; //  Byte Bit 0: Flash Enabled





        public const int TXT_CURSOR_COLOR = 0x7F000B; // 1 Byte 
        public const int TXT_CURSOR_X_REG_L = 0x7F000C; // 1 Byte 
        public const int TXT_CURSOR_X_REG_H = 0x7F000D; // 1 Byte 
        public const int TXT_CURSOR_Y_REG_L = 0x7F000E; // 1 Byte 
        public const int TXT_CURSOR_Y_REG_H = 0x7F000F; // 1 Byte 

        public const int PME_STS_REG = 0x7F1100; // 1 Byte 
        public const int PME_EN_REG = 0x7F1102; // 1 Byte 
        public const int PME_STS1_REG = 0x7F1104; // 1 Byte 
        public const int PME_STS2_REG = 0x7F1105; // 1 Byte 
        public const int PME_STS3_REG = 0x7F1106; // 1 Byte 
        public const int PME_STS4_REG = 0x7F1107; // 1 Byte 
        public const int PME_STS5_REG = 0x7F1108; // 1 Byte 
        public const int PME_EN1_REG = 0x7F110A; // 1 Byte 
        public const int PME_EN2_REG = 0x7F110B; // 1 Byte 
        public const int PME_EN3_REG = 0x7F110C; // 1 Byte 
        public const int PME_EN4_REG = 0x7F110D; // 1 Byte 
        public const int PME_EN5_REG = 0x7F110E; // 1 Byte 
        public const int SMI_STS1_REG = 0x7F1110; // 1 Byte 
        public const int SMI_STS2_REG = 0x7F1111; // 1 Byte 
        public const int SMI_STS3_REG = 0x7F1112; // 1 Byte 
        public const int SMI_STS4_REG = 0x7F1113; // 1 Byte 
        public const int SMI_STS5_REG = 0x7F1114; // 1 Byte 
        public const int SMI_EN1_REG = 0x7F1116; // 1 Byte 
        public const int SMI_EN2_REG = 0x7F1117; // 1 Byte 
        public const int SMI_EN3_REG = 0x7F1118; // 1 Byte 
        public const int SMI_EN4_REG = 0x7F1119; // 1 Byte 
        public const int SMI_EN5_REG = 0x7F111A; // 1 Byte 
        public const int MSC_ST_REG = 0x7F111C; // 1 Byte 

        public const int FORCE_DISK_CHANGE = 0x7F111E; // 1 Byte 
        public const int FLOPPY_DATA_RATE = 0x7F111F; // 1 Byte 

        public const int UART1_FIFO_CTRL_SHDW = 0x7F1120; // 1 Byte 
        public const int UART2_FIFO_CTRL_SHDW = 0x7F1121; // 1 Byte 
        public const int DEV_DISABLE_REG = 0x7F1122; // 1 Byte 
        public const int GP10_REG = 0x7F1123; // 1 Byte 
        public const int GP11_REG = 0x7F1124; // 1 Byte 
        public const int GP12_REG = 0x7F1125; // 1 Byte 
        public const int GP13_REG = 0x7F1126; // 1 Byte 
        public const int GP14_REG = 0x7F1127; // 1 Byte 
        public const int GP15_REG = 0x7F1128; // 1 Byte 
        public const int GP16_REG = 0x7F1129; // 1 Byte 
        public const int GP17_REG = 0x7F112A; // 1 Byte 
        public const int GP20_REG = 0x7F112B; // 1 Byte 
        public const int GP21_REG = 0x7F112C; // 1 Byte 
        public const int GP22_REG = 0x7F112D; // 1 Byte 

        public const int GP24_REG = 0x7F112F; // 1 Byte 
        public const int GP25_REG = 0x7F1130; // 1 Byte 
        public const int GP26_REG = 0x7F1131; // 1 Byte 
        public const int GP27_REG = 0x7F1132; // 1 Byte 
        public const int GP30_REG = 0x7F1133; // 1 Byte 
        public const int GP31_REG = 0x7F1134; // 1 Byte 
        public const int GP32_REG = 0x7F1135; // 1 Byte 
        public const int GP33_REG = 0x7F1136; // 1 Byte 
        public const int GP34_REG = 0x7F1137; // 1 Byte 
        public const int GP35_REG = 0x7F1138; // 1 Byte 
        public const int GP36_REG = 0x7F1139; // 1 Byte 
        public const int GP37_REG = 0x7F113A; // 1 Byte 
        public const int GP40_REG = 0x7F113B; // 1 Byte 
        public const int GP41_REG = 0x7F113C; // 1 Byte 
        public const int GP42_REG = 0x7F113D; // 1 Byte 
        public const int GP43_REG = 0x7F113E; // 1 Byte 
        public const int GP50_REG = 0x7F113F; // 1 Byte 
        public const int GP51_REG = 0x7F1140; // 1 Byte 
        public const int GP52_REG = 0x7F1141; // 1 Byte 
        public const int GP53_REG = 0x7F1142; // 1 Byte 
        public const int GP54_REG = 0x7F1143; // 1 Byte 
        public const int GP55_REG = 0x7F1144; // 1 Byte 
        public const int GP56_REG = 0x7F1145; // 1 Byte 
        public const int GP57_REG = 0x7F1146; // 1 Byte 
        public const int GP60_REG = 0x7F1147; // 1 Byte 
        public const int GP61_REG = 0x7F1148; // 1 Byte 

        public const int GP1_REG = 0x7F114B; // 1 Byte 
        public const int GP2_REG = 0x7F114C; // 1 Byte 
        public const int GP3_REG = 0x7F114D; // 1 Byte 
        public const int GP4_REG = 0x7F114E; // 1 Byte 
        public const int GP5_REG = 0x7F114F; // 1 Byte 
        public const int GP6_REG = 0x7F1150; // 1 Byte 

        public const int FAN1_REG = 0x7F1156; // 1 Byte 
        public const int FAN2_REG = 0x7F1157; // 1 Byte 
        public const int FAN_CTRL_REG = 0x7F1158; // 1 Byte 
        public const int FAN1_TACH_REG = 0x7F1159; // 1 Byte 
        public const int FAN2_TACH_REG = 0x7F115A; // 1 Byte 
        public const int FAN1_PRELOAD_REG = 0x7F115B; // 1 Byte 
        public const int FAN2_PRELOAD_REG = 0x7F115C; // 1 Byte 
        public const int LED1_REG = 0x7F115D; // 1 Byte 
        public const int LED2_REG = 0x7F115E; // 1 Byte 
        public const int KEYBOARD_SCAN_CODE = 0x7F115F; // 1 Byte 
                                                        // 3552 bytes available
                                                        // 
                                                        // Color Lookup Tables
                                                        // 
        public const int FG_CHAR_LUT_PTR = 0x7F1F40; // 64 Bytes 
        public const int BG_CHAR_LUT_PTR = 0x7F1F80; // 64 Bytes 
        public const int GRPH_LUT0_PTR = 0x7F2000; // 1024 Bytes 
        public const int GRPH_LUT1_PTR = 0x7F2400; // 1024 Bytes 
        public const int GRPH_LUT2_PTR = 0x7F2800; // 1024 Bytes 
        public const int GRPH_LUT3_PTR = 0x7F2C00; // 1024 Bytes 
        public const int GRPH_LUT4_PTR = 0x7F3000; // 1024 Bytes 
        public const int GRPH_LUT5_PTR = 0x7F3400; // 1024 Bytes 
        public const int GRPH_LUT6_PTR = 0x7F3800; // 1024 Bytes 
        public const int GAMMA_LUT_PTR = 0x7F3C00; // 1024 Bytes 
                                                   // 38912 bytes available
        public const int IO_GAVIN = 0x7FD800; // 1024 Bytes GAVIN I/O space
        public const int IO_SUPERIO = 0x7FDC00; // 1024 Bytes SuperIO I/O space
        public const int IO_VICKY = 0x7FE000; // 1024 Bytes VICKY I/O space
        public const int IO_BEATRIX = 0x7FE400; // 1024 Bytes BEATRIX I/O space
        public const int IO_RTC = 0x7FE800; // 1024 Bytes RTC I/O space
        public const int IO_CIA = 0x7FEC00; // 4864 Bytes CIA I/O space
                                            // 255 bytes available
        public const int IO7F_END = 0x7FFFFF; // *End of I/O space
                                              //
                                              // End of Bank 7F
        #endregion

    }
}
