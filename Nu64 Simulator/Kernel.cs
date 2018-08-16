using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nu64.Basic;
using Nu64.Processor;
using Nu64.Monitor;
using Nu64.Display;

namespace Nu64
{
    public class Kernel
    {
        private const int TAB_WIDTH = 4;
        public SystemBus Memory = null;
        public Processor.CPU CPU = null;
        public Gpu gpu = null;
        public RingBuffer<Char> KeyboardBuffer = new RingBuffer<char>(256);
        public ColorCodes CurrentColor = ColorCodes.Green;
        public bool ConsoleEcho = false;

        public Basic.Immediate Basic = null;
        public Monitor.Monitor Monitor = null;

        public System.Timers.Timer TickTimer = new System.Timers.Timer();

        public ReadyHandler ReadyHandler = null;

        public DeviceEnum InputDevice = DeviceEnum.Keyboard;
        public DeviceEnum OutputDevice = DeviceEnum.Screen;

        public Kernel(Gpu gpu)
        {
            Memory = new SystemBus();
            Memory.RAM = new MemoryRAM(0x800000); // 8MB RAM
            Memory.GPU = gpu;
            Memory.ROM = new MemoryRAM(0x100000); // 1MB ROM
            this.CPU = new CPU(Memory);
            this.gpu = gpu;
            gpu.LoadCharacterData(Memory.RAM);

            for(int i=MemoryMap_DirectPage.GPU_PAGE_0; i< MemoryMap_DirectPage.GPU_PAGE_1; i++)
            {
                this.Memory[i] = 0x40;
            }

            this.Basic = new Basic.Immediate(this);
            this.Monitor = new Monitor.Monitor(this);
        }

        public void Reset()
        {
            Cls();
            gpu.Refresh();

            //PrintGreeting();
            //Basic.PrintGreeting();
            //Monitor.PrintGreeting();
            //PrintCopyright();
            //ShowFlag();
            Y = 4;
            //this.PrintLine();
            this.ReadyHandler = Monitor;
            HexFile h = new HexFile(Memory, @"ROMs\kernel.hex");
            CPUTest test= new CPUTest(this);
            test.BeginTest(0xf81000);
            //test.BeginTestFast(0xf81000);

            this.TickTimer.Interval = 1000 / 60;
            this.TickTimer.Elapsed += TickTimer_Elapsed;
            this.TickTimer.Enabled = true;
        }

        private void PrintCopyright()
        {
            Y = 0;
            PrintTab(60);
            PrintLine("(c)2018 Tom Wilson");
            PrintTab(60);
            PrintLine("wilsontp@gmail.com");
        }

        private void TickTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DoConsoleEcho();
        }

        public virtual void PrintChar(char c)
        {
            if (OutputDevice == DeviceEnum.Screen
                || OutputDevice == DeviceEnum.Keyboard)
            {
                PrintCharToScreen(c);
            }

            if (OutputDevice == DeviceEnum.DebugWindow)
            {
                UI.RegisterWindow.PrintChar(c);
            }
        }

        public virtual void PrintCharToScreen(char c)
        {
            switch (c)
            {
                case (char)PETSCIICommandCodes.Up:
                    Y = Y - 1;
                    break;
                case (char)PETSCIICommandCodes.Down:
                    Y = Y + 1;
                    break;
                case (char)PETSCIICommandCodes.Left:
                    X = X - 1;
                    break;
                case (char)PETSCIICommandCodes.Right:
                    X = X + 1;
                    break;
                case (char)PETSCIICommandCodes.Home:
                    X = 0;
                    Y = 0;
                    break;
                case (char)PETSCIICommandCodes.Clear:
                    Fill(0x20);
                    X = 0;
                    Y = 0;
                    break;
                case '\x8': // backspace
                    PrintBackspace();
                    break;
                case '\t':
                    PrintTab();
                    break;
                case '\xa':
                    PrintLineFeed();
                    break;
                case '\xd':
                    PrintReturn();
                    break;
                case '\x12':
                    gpu.CurrentColor = gpu.CurrentColor | ColorCodes.Reverse;
                    break;
                case '\x92':
                    gpu.CurrentColor = gpu.CurrentColor & (int.MaxValue - ColorCodes.Reverse);
                    break;
                default:
                    Memory[MemoryMap_DirectPage.GPU_PAGE_0 + gpu.CursorPos] = (byte)c;
                    //gpu.ColorData[gpu.CursorPos] = CurrentColor;
                    AdvanceCursor();
                    break;
            }
            gpu.ResetDrawTimer();
        }

        /// <summary>
        /// Places the cursor at the specified column. The leftmost column is 0.
        /// </summary>
        /// <param name="Col"></param>
        public void PrintTab(int Col)
        {
            if (OutputDevice == DeviceEnum.Screen)
            {
                this.X = Col;
            }
            else if (OutputDevice == DeviceEnum.DebugWindow)
            {
                UI.RegisterWindow.PrintTab(Col);
            }
        }

        private void PrintBackspace()
        {
            X = X - 1;
            if (X < 0)
                Y = Y - 1;
            Memory[gpu.CursorPos] = 0x20;
        }

        private void PrintTab()
        {
            int i = TAB_WIDTH - X % TAB_WIDTH;
            while (i > 0)
            {
                PrintChar(' ');
                i--;
            }
        }

        ColorCodes _currentForeground = ColorCodes.Green | ColorCodes.LightBlue;
        public ColorCodes CurrentForeground
        {
            get { return _currentForeground; }
            protected set { _currentForeground = value; }
        }

        ColorCodes _currentBackground = ColorCodes.Black;
        public ColorCodes CurrentBackground
        {
            get { return _currentBackground; }
            protected set { _currentBackground = value; }
        }

        public virtual void PrintChars(char[] Chars)
        {
            for (int i = 0; i < Chars.Length; i++)
                PrintChar(Chars[i]);
        }

        public void Scroll1()
        {
            int addr = MemoryMap_DirectPage.GPU_PAGE_0;
            for (int c = 0; c < gpu.BufferSize - gpu.Columns; c++)
            {
                for (int col = 0; col < gpu.Columns; col++)
                {
                    Memory[addr + c] = Memory[addr + c + gpu.Columns];
                    //gpu.ColorData[c] = gpu.ColorData[c + gpu.Columns];
                }
            }

            for (int c = gpu.BufferSize - gpu.Columns; c < gpu.BufferSize; c++)
            {
                Memory[addr + c] = 0x20;
                //gpu.ColorData[c] = _currentForeground;
            }
        }

        public void AdvanceCursor()
        {
            if (X < Columns - 1)
                X += 1;
            else
                PrintLine();
        }

        public void PrintLineFeed()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\n');
            }
            else
            {
                if (Y < Rows - 1)
                    Y += 1;
                else
                {
                    Scroll1();
                    Y = Rows - 1;
                }
            }
        }

        public void PrintReturn()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\r');
            }
            else
            {
                X = 0;
            }
        }

        public void PrintLine()
        {
            if (OutputDevice != DeviceEnum.Screen)
            {
                PrintChar('\r');
                PrintChar('\n');
            }
            else
            {
                PrintReturn();
                PrintLineFeed();
            }
        }

        public virtual void PrintLine(string s)
        {
            Print(s);
            PrintReturn();
            PrintLineFeed();
        }

        public virtual void Print(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                PrintChar(s[i]);
            }
        }

        /// <summary>
        /// Moves the cursor on the screen. This is zero-based
        /// </summary>
        /// <param name="Row">Row number, top of screen is 0</param>
        /// <param name="Col">Column, left side of screen is 0</param>
        public virtual void Locate(int Row, int Col)
        {
            Y = Row;
            X = Col;

            if (Row < 0)
                Row = 0;
            if (Row >= Rows)
                Row = Rows - 1;
            if (Col < 0)
                Col = 0;
            if (Col >= Columns)
                Col = Columns - 1;
        }

        public virtual void Cls()
        {
            Fill(0x20);
            Locate(0, 0);
        }

        public virtual void Fill(byte c)
        {
            for (int i = 0; i < gpu.BufferSize; i++)
            {
                Memory[MemoryMap_DirectPage.GPU_PAGE_0 + i] = c;
                //gpu.ColorData[i] = _currentForeground;
            }
        }

        public int GetCharPos(int row, int col)
        {
            return row * Columns + col;
        }

        public int GetStartOfLine()
        {
            return GetCharPos(Y, 0);
        }

        public void READY()
        {
            if (ReadyHandler != null)
                ReadyHandler.Ready();
            ConsoleEcho = true;
        }

        public void ReturnPressed(int pos)
        {
            PrintReturn();
            if (ReadyHandler != null)
                ReadyHandler.ReturnPressed(GetStartOfLine());
        }

        public int X
        {
            get { return gpu.X; }
            set { gpu.X = value; }
        }

        public int Y
        {
            get { return gpu.Y; }
            set { gpu.Y = value; }
        }

        public int Rows
        {
            get { return gpu.Rows; }
        }

        public int Columns
        {
            get { return gpu.Columns; }
        }

        public void DoConsoleEcho()
        {
            if (!ConsoleEcho)
                return;
            if (KeyboardBuffer.Count == 0)
                return;

            char c = KeyboardBuffer.Read();
            int pos = GetStartOfLine();
            switch (c)
            {
                case '\r':
                    ReturnPressed(pos);
                    break;
                default:
                    PrintChar(c);
                    break;
            }
        }

        public void PrintMemBinary(int Bytes, int Address)
        {
            for (int i = Bytes - 1; i >= 0; i--)
            {
                int b = Peek(Address + i);
                for (int j = 0; j < 8; j++)
                {
                    if ((b & 0x80) != 0)
                        Print("1");
                    else
                        Print("0");
                    b = b << 1;
                }
            }
        }

        public void PrintMemHex(int Bytes, int Address)
        {
            for (int i = Bytes - 1; i >= 0; i--)
            {
                byte b = Peek(Address + i);
                Print(b.ToString("X2"));
            }
        }

        public byte Peek(int bank, int Address)
        {
            return Memory[bank, Address];
        }

        public byte Peek(int Address)
        {
            return Memory[Address];
        }

        public void PrintGreeting()
        {
            PrintLine("         Nu64 Programming Simulator");
        }

        public void ShowFlag()
        {
            Locate(0, 0);
            //PrintLine(" \xec\xa0\xa8\xa8\xa0\xed");
            //PrintLine(" \xa0\xa6\xa6\xa6\xa6\xa0");
            //PrintLine(" \xdf\xa0\xf9\xf9\xa0\xa9");
            PrintLine(" \xf2\xee\xee\xee\xee\xf3\xa8");
            PrintLine(" \xA1NU64\xef\xa6");
            PrintLine(" \xf0\xa2\xa2\xa2\xa2\xf1\xa6");
            PrintLine("  \xf9\xf9\xf9\xf9\xf9\xf9");
        }

    }
}
