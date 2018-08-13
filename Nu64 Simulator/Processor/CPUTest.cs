using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu64.Processor
{
    class CPUTest : ReadyHandler
    {
        Kernel kernel = null;
        Processor.CPU CPU = null;

        byte[] TestProg = {
            // Switch to native mode. This should be done by the kernel,
            // but we don't have a boot routine yet.
            0x18,             // CLC          Clear carry in preparation to...
            0xFB,             // XCE          Switch to native mode
            0xE2, 0x30,       // REP 30
            0xC2, 0x30,       // REP 30
            
            // test LDA immediate, zero and Negative flags
            0xA9, 0x00, 0X00, // LDA #$0000   Zero should be set
            0xA9, 0x00, 0x80, // LDA #$8000   Negative should be set 
            0xA9, 0xFF, 0x00, // LDA #$00FF   
            0xA9, 0xFF, 0xFF, // LDA #$FFFF   Negative should be set 
            
            // test LDX and LDY with 16-bit values
            0xA2, 0x00, 0x00, // LDX #$0000   Zero should be set
            0xA2, 0x00, 0x80, // LDX #$8000   Negative should be set
            0xA0, 0x34, 0x12, // LDY #$1234   

            // Switch to 8-bit mode and test load instructions
            0xE2, 0x30,       // SEP #$30     Set 8-bit A and Index registers
            0xA9, 0x00,       // LDA #$00     
            0xA9, 0xFF,       // LDA #$FF
            0xA2, 0x00,       // LDX #$00                               
            0xA2, 0xFF,       // LDX #$FF                             
            0xA0, 0x00,       // LDY #$00     
            0xA0, 0xFF,       // LDY #$FF     
            // Return to the OS
            0xDB,             // STP          Stops the CPU
            };

        public CPUTest(Kernel newKernel)
        {
            this.kernel = newKernel;
            this.CPU = kernel.CPU;
        }

        public void BeginTest()
        {
            sbyte test1 = -1;
            byte test2 = (byte)test1;

            int pc = 0xc000;
            for (int i = 0; i < TestProg.Length; i++)
            {
                kernel.MemoryMap[pc] = TestProg[i];
                pc++;
            }
            kernel.MemoryMap.WriteWord(MemoryMap_DirectPage.VECTOR_RESET, 0xc000);
            kernel.MemoryMap.WriteWord(MemoryMap_DirectPage.VECTOR_BRK, 0xc000);
            kernel.CPU.Stack.Value = MemoryMap_DirectPage.END_OF_STACK;

            kernel.OutputDevice = DeviceEnum.DebugWindow;
            kernel.CPU.PC.Value = 0xc000;
            kernel.PrintTab(10);
            kernel.Monitor.PrintRegisterHeader();
            kernel.PrintTab(10);
            kernel.Monitor.PrintRegisters(false);
            while (!CPU.Halted)
            {
                int p1 = CPU.GetLongPC();

                CPU.ExecuteNext();

                int pc2 = CPU.GetLongPC();
                for (int i = p1; i < pc2; i++)
                {
                    kernel.PrintMemHex(1, i);
                    kernel.Print(" ");
                }
                kernel.PrintTab(10);
                kernel.Monitor.PrintRegisters(false);
            }

            kernel.OutputDevice = DeviceEnum.Screen;
            kernel.ReadyHandler = kernel.Monitor;
            kernel.READY();
        }

        public void Ready()
        {
            BeginTest();
        }

        public void ReturnPressed(int LineStart)
        {
            kernel.PrintLine();
        }

        public void PrintGreeting()
        {
            kernel.PrintLine("Simulator Performance Test. Executing for one second.");
        }

    }
}

