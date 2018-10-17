﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Imaging;
using Nu256.Common;
using Nu256.Simulator.MemoryLocations;

namespace Nu256.Simulator.Display
{
    public partial class Gpu : UserControl, IMappable
    {
        public event KeyPressEventHandler KeyPressed;

        public const int VRAM_SIZE = 0x100000;
        private const int REGISTER_BLOCK_SIZE = 256;
        const int MAX_TEXT_COLS = 128;
        const int MAX_TEXT_LINES = 64;
        const int SCREEN_PAGE_SIZE = 128 * 64;

        private int length = 128 * 64 * 4; //Text mode uses 32K, 4 planes of 8K each.

        [Browsable(false)]
        public int StartAddress
        {
            get
            {
                if (VRAM == null)
                    return -1;

                return VRAM.ReadLong(MemoryMap.SCREENBEGIN);
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int EndAddress
        {
            get
            {
                if (StartAddress < 0)
                    return -1;
                return StartAddress + length - 1;
            }
        }

        public MemoryRAM VRAM = null;

        // Video page 0 is 0x1000. Each page is 8192 (128x64) bytes long.
        private int characterMatrixStart = MemoryLocations.MemoryMap.TEXT_PAGE0;
        //private int colorMatrixStart = 6096;
        //private int attributeStart = 8096;
        // character bitmaps are stored in the reserved video memory space.
        private int characterSetStart = 0x7F0000;

        public List<CharacterSet> CharacterSetSlots = new List<CharacterSet>();

        private Bitmap frameBuffer = new Bitmap(640, 200, PixelFormat.Format32bppArgb);

        /// <summary>
        /// number of frames to wait to refresh the screen.
        /// One frame = 1/60 second.
        /// </summary>
        public int RefreshTimer = 0;
        public int BlinkRate = 20;

        //        int columns = 80;
        //        int LINES = 25;
        //        int bufferSize = 2000;
        public ColorCodes CurrentColor = ColorCodes.White;

        Font TextFont = SystemFonts.DefaultFont;
        Brush TextBrush = new SolidBrush(Color.LightBlue);
        Brush BorderBrush = new SolidBrush(Color.LightBlue);
        Brush InvertedBrush = new SolidBrush(Color.Blue);
        Brush CursorBrush = new SolidBrush(Color.LightBlue);

        static string MEASURE_STRING = new string('W', 80);

        Timer timer = new Timer();
        bool CursorEnabled = true;
        bool CursorState = true;

        /// <summary>
        /// Screen character data. Data is addressed as Data[i].
        /// </summary>
        //public char[] CharacterData = null;

        /// <summary>
        /// Screen color data. Upper nibble is background color. Lower nibble is foreground color. 
        /// </summary>
        //public ColorCodes[] ColorData = null;

        private int GetCharPos(int row, int col)
        {
            if (VRAM == null)
                return 0;
            int baseAddress = VRAM.ReadLong(MemoryMap.SCREENBEGIN);
            return baseAddress + row * COLS_PER_LINE + col;
        }

        /// <summary>
        /// Column of the cursor position. 0 is left edge
        /// </summary>
        [Browsable(false)]
        public int X
        {
            get
            {
                if (VRAM == null)
                    return 0;

                return VRAM.ReadByte(MemoryMap.CURSOR_X);
            }
            set
            {
                int x = value;
                if (x < 0)
                    x = 0;
                if (x >= ColumnsVisible)
                    x = ColumnsVisible - 1;
                if (VRAM != null)
                    VRAM.WriteByte(MemoryMap.CURSOR_X, (byte)x);
                ResetDrawTimer();
                CursorPos = GetCharPos(Y, x);
            }
        }

        /// <summary>
        /// Row of cursor position. 0 is top of the screen
        /// </summary>
        [Browsable(false)]
        public int Y
        {
            get
            {
                if (VRAM == null)
                    return 0;

                return VRAM.ReadByte(MemoryMap.CURSOR_Y);
            }
            set
            {
                int y = value;
                if (y < 0)
                    y = 0;
                if (y >= LinesVisible)
                    y = LinesVisible - 1;
                if (VRAM != null)
                    VRAM.WriteByte(MemoryMap.CURSOR_Y, (byte)y);
                ResetDrawTimer();
                CursorPos = GetCharPos(y, X);
            }
        }

        [Browsable(false)]
        public int ColumnsVisible
        {
            get
            {
                if (VRAM == null)
                    return 0;

                return VRAM.ReadByte(MemoryMap.COLS_VISIBLE);
            }
            set
            {
                if (VRAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_COLS)
                    i = MAX_TEXT_COLS;
                VRAM.WriteWord(MemoryMap.COLS_VISIBLE, i);
            }
        }

        [Browsable(false)]
        public int LinesVisible
        {
            get
            {
                if (VRAM == null)
                    return 0;
                return VRAM.ReadByte(MemoryMap.LINES_VISIBLE);
            }
            set
            {
                if (VRAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_LINES)
                    i = MAX_TEXT_LINES;
                VRAM.WriteWord(MemoryMap.LINES_VISIBLE, i);
            }
        }

        public int COLS_PER_LINE
        {
            get
            {
                if (VRAM == null)
                    return 0;

                return VRAM.ReadByte(MemoryMap.COLS_PER_LINE);
            }
            set
            {
                if (VRAM == null)
                    return;

                int i = value;
                if (i < 0)
                    i = 0;
                if (i > MAX_TEXT_COLS)
                    i = MAX_TEXT_COLS;
                VRAM.WriteWord(MemoryMap.COLS_PER_LINE, i);
            }
        }


        public Gpu()
        {
            InitializeComponent();

            this.Load += new EventHandler(Gpu_Load);
        }

        void Gpu_Load(object sender, EventArgs e)
        {
            //TextFont = GetBestFont();

            this.SetScreenSize(25, 80);
            this.Paint += new PaintEventHandler(Gpu_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000 / 60;
            this.VisibleChanged += new EventHandler(FrameBufferControl_VisibleChanged);
            this.DoubleBuffered = true;

            X = 0;
            Y = 0;

            if (DesignMode)
            {
                timer.Enabled = false;
            }
            else
            {
                if (ParentForm == null)
                    return;
                int htarget = 480;
                int topmargin = ParentForm.Height - ClientRectangle.Height;
                int sidemargin = ParentForm.Width - ClientRectangle.Width;
                ParentForm.Height = htarget + topmargin;
                ParentForm.Width = (int)Math.Ceiling(htarget * 1.6) + sidemargin;
            }
        }

        public void LoadCharacterData(MemoryRAM RAM)
        {
            this.VRAM = RAM;
            LoadCharacterSet("ASCII-PET", @"Resources\FOENIX-CHARACTER-ASCII.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);
            //LoadCharacterSet("ASCII-PET", @"Resources\FOENIX-CHARACTER-ASCII.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);
            //LoadCharacterSet("PETSCII_GRAPHICS", @"Resources\PETSCII.901225-01.bin", 0, CharacterSet.CharTypeCodes.PETSCII_GRAPHICS, CharacterSet.SizeCodes.Size8x8);
            //LoadCharacterSet("PETSCII_TEXT", @"Resources\PETSCII.901225-01.bin", 4096, CharacterSet.CharTypeCodes.PETSCII_TEXT, CharacterSet.SizeCodes.Size8x8);
        }

        private Font GetBestFont()
        {
            Font useFont = null;
            float rowHeight = this.ClientRectangle.Height / (float)LinesVisible;
            if (rowHeight < 8)
                rowHeight = 8;

            var fonts = new[]
            {
                "C64 Pro Mono",
                "Consolas",
                //"Classic Console",
                //"Glass TTY VT220",
                "Lucida Console",
            };

#if DEBUGx
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            // Get the array of FontFamily objects.
            var fontFamilies = installedFontCollection.Families;

            // The loop below creates a large string that is a comma-separated
            // list of all font family names.

            int count = fontFamilies.Length;
            for (int j = 0; j < count; ++j)
            {
                System.Diagnostics.Debug.WriteLine("Font: " + fontFamilies[j].Name);
            }
#endif

            foreach (var f in fonts)
            {
                using (Font fontTester = new Font(
                        f,
                        rowHeight,
                        FontStyle.Regular,
                        GraphicsUnit.Pixel))
                {
                    if (fontTester.Name == f)
                    {
                        useFont = new Font(f, rowHeight, FontStyle.Regular, GraphicsUnit.Pixel);
                        break;
                    }
                    else
                    {
                    }
                }
            }
            if (useFont == null)
                useFont = new Font(this.Font, FontStyle.Regular);

            Graphics g = this.CreateGraphics();
            SizeF fs = MeasureFont(useFont, g);
            float ratio = rowHeight / fs.Height;
            float newSize = rowHeight * ratio;
            useFont = new Font(useFont.FontFamily, newSize, FontStyle.Regular, GraphicsUnit.Pixel);

            return useFont;
        }

        public void ResetDrawTimer()
        {
            RefreshTimer = 0;
            CursorState = true;
        }


        public int BufferSize
        {
            get
            {
                return MAX_TEXT_COLS * MAX_TEXT_LINES;
            }
        }

        /// <summary>
        /// Memory offset of the cursor position on the screen. The top-left corner is the first memory location
        /// of the screen. 
        /// </summary>
        [Browsable(false)]
        public int CursorPos
        {
            get
            {
                if (VRAM == null)
                    return 0;
                return VRAM.ReadWord(MemoryMap.CURSORPOS);
            }

            set
            {
                if (VRAM == null)
                    return;
                VRAM.WriteWord(MemoryMap.CURSORPOS, value);
            }
        }

        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Gpu_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            if (VRAM == null)
            {
                g.DrawString("VRAM Not initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            if (StartAddress < VRAM.StartAddress || (StartAddress + Length) > VRAM.EndAddress)
            {
                g.DrawString("StartAddress(" + StartAddress.ToString() + ") or Length(" + Length.ToString() + ") invalid", this.Font, TextBrush, 0, 0);
                return;
            }
            if (ColumnsVisible < 1 || ColumnsVisible > 128)
            {
                g.DrawString("ColumnsVisible invalid:" + ColumnsVisible.ToString(), this.Font, TextBrush, 0, 0);
                return;
            }
            if (LinesVisible < 1)
            {
                g.DrawString("LinesVisible invalid:" + LinesVisible.ToString(), this.Font, TextBrush, 0, 0);
                return;
            }
            characterMatrixStart = StartAddress;
            //DrawVectorText(e.Graphics);
            DrawBitmapText(e.Graphics);
        }

        private void DrawVectorText(Graphics g)
        {
            float x;
            float y;

            if (TextFont == null)
                TextFont = GetBestFont();
            SizeF charSize = MeasureFont(TextFont, g);
            float charWidth = charSize.Width / MEASURE_STRING.Length;
            float charHeight = charSize.Height;
            float RightCol = charWidth * ColumnsVisible;
            float BottomRow = charWidth * LinesVisible;

            float ScaleX = this.ClientRectangle.Width / RightCol;
            float ScaleY = this.ClientRectangle.Height / BottomRow;
            g.ResetTransform();
            g.ScaleTransform(ScaleX, ScaleY);

            if (VRAM == null)
                return;

            int col = 0, row = 0;
            for (int i = 0; i < BufferSize; i++)
            {
                if (col < ColumnsVisible)
                {
                    x = col * charWidth;
                    y = row * charHeight;

                    char c = (char)VRAM.ReadByte(i);
                    g.DrawString(c.ToString(),
                        this.Font,
                        TextBrush,
                        x, y,
                        StringFormat.GenericTypographic);
                }

                col++;
                if (col >= COLS_PER_LINE)
                {
                    col = 0;
                    row++;
                }
            }

            //if (CursorState && CursorEnabled)
            //{
            //    x = X * charWidth;
            //    y = Y * charHeight;
            //    g.FillRectangle(CursorBrush, x, y, charWidth, charHeight);
            //    g.DrawString(CharacterData[GetCharPos(Y, X)].ToString(),
            //        TextFont,
            //        InvertedBrush,
            //        x, y,
            //        StringFormat.GenericTypographic);
            //}
        }

        int lastWidth = 0;
        private void DrawBitmapText(Graphics controlGraphics)
        {
            if (lastWidth != ColumnsVisible
                && ColumnsVisible > 0
                && LinesVisible > 0)
            {
                frameBuffer = new Bitmap(8 * ColumnsVisible, 8 * LinesVisible, PixelFormat.Format32bppArgb);
                lastWidth = ColumnsVisible;
            }

            Graphics g = Graphics.FromImage(frameBuffer);
            if (VRAM == null)
            {
                controlGraphics.Clear(Color.Blue);
                controlGraphics.DrawString("VRAM not initialized", this.Font, TextBrush, 0, 0);
                return;
            }
            if (CharacterSetSlots.Count == 0)
            {
                controlGraphics.Clear(Color.Blue);
                controlGraphics.DrawString("Character ROM not initialized", this.Font, TextBrush, 0, 0);
                return;
            }

            float x;
            float y;

            ColorMap[] colorMap = new ColorMap[1];
            colorMap[0] = new ColorMap();
            colorMap[0].OldColor = Color.White;
            colorMap[0].NewColor = Color.LightGreen;
            ImageAttributes attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);

            float charWidth = 8;
            float charHeight = 8;

            g.CompositingQuality = global::System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = global::System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.Clear(Color.Blue);

            //this._cursorCol = Memory.ReadByte(MemoryMap_DirectPage.CURSOR_X);
            //this._cursorRow = Memory.ReadByte(MemoryMap_DirectPage.CURSOR_Y);

            int col = 0, line = 0;
            int screenStart = characterMatrixStart - VRAM.StartAddress;
            int lineStart = screenStart;
            for (line = 0; line < LinesVisible; line++)
            {
                int addr = lineStart;
                for (col = 0; col < ColumnsVisible; col++)
                {
                    x = col * charWidth;
                    y = line * charHeight;
                    byte c = VRAM.ReadByte(addr++);
                    Bitmap bmp = CharacterSetSlots[0].Bitmaps[c];
                    RectangleF rect = new RectangleF((int)x, (int)y, bmp.Width, bmp.Height);
                    //g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                    g.DrawImage(bmp, rect);
                }
                lineStart += COLS_PER_LINE;
            }

            if (CursorState && CursorEnabled)
            {
                x = X * charWidth;
                y = Y * charHeight;
                g.FillRectangle(CursorBrush, x, y, charWidth, charHeight);
                //g.DrawString(CharacterData[GetCharPos(Y, X)].ToString(),
                //    TextFont,
                //    InvertedBrush,
                //    x, y,
                //    StringFormat.GenericTypographic);
            }

            controlGraphics.DrawImage(frameBuffer, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        private SizeF MeasureFont(Font font, Graphics g)
        {
            return g.MeasureString(MEASURE_STRING, font, int.MaxValue, StringFormat.GenericTypographic);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (RefreshTimer-- > 0)
            {
                if (Form.ActiveForm == this.ParentForm)
                    Refresh();
                return;
            }

            this.Refresh();
            CursorState = !CursorState;
            RefreshTimer = BlinkRate;
        }

        void FrameBufferControl_VisibleChanged(object sender, EventArgs e)
        {
            timer.Enabled = this.Visible;
        }

        private void FrameBuffer_SizeChanged(object sender, global::System.EventArgs e)
        {
            //TextFont = GetBestFont();
        }

        private void FrameBuffer_KeyPress(object sender, KeyPressEventArgs e)
        {
            TerminalKeyEventArgs args = new TerminalKeyEventArgs(e.KeyChar);
            KeyPressed?.Invoke(this, args);
        }

        public byte ReadByte()
        {
            return 0;
        }

        public virtual void SetScreenSize(int Lines, int Columns)
        {
            this.ColumnsVisible = Columns;
            this.LinesVisible = Lines;
        }

        public byte ReadByte(int Address)
        {
            if (VRAM == null)
                return 0;
            return VRAM.ReadByte(Address - VRAM.StartAddress);
        }

        /// return the GPU registers: start of text page, start of color page, start of character data, 
        /// number of columns, number of LINES, graphics mode, etc. 
        /// </summary>
        /// <param name="Address">Address to read</param>
        /// <returns></returns>
        public byte ReadGPURegister(int Address)
        {
            return 0;
        }

        public void WriteGPURegister(int Address, byte Data)
        {

        }

        public void WriteByte(int Address, byte Data)
        {
            if (VRAM == null)
                return;
            VRAM.WriteByte(Address - VRAM.StartAddress, Data);
            //else if (Address >= characterMatrixStart && Address < (characterMatrixStart + CharacterData.Length))
            //{
            //    CharacterData[Address - characterMatrixStart] = (char)Data;
            //}
            //else if (Address >= colorMatrixStart && Address < (colorMatrixStart + ColorData.Length))
            //{
            //    ColorData[Address - colorMatrixStart] = (ColorCodes)Data;
            //}
        }

        /// <summary>
        /// Loads a character set into RAM and adds it to the character set table.
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Filename"></param>
        public CharacterSet LoadCharacterSet(string Name, string Filename, int Offset, CharacterSet.CharTypeCodes CharType, CharacterSet.SizeCodes CharSize)
        {
            CharacterSet cs = new CharacterSet();
            cs.Load(Filename, Offset, VRAM, MemoryLocations.MemoryMap.CHARDATA_BEGIN & 0xffff, CharSize);
            CharacterSetSlots.Add(cs);
            cs.CharType = CharType;
            return cs;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }
    }
}
