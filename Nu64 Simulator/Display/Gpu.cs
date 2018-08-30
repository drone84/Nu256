using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing.Imaging;

namespace Nu64.Display
{
    public partial class Gpu : UserControl, Nu64.Common.IMappable
    {
        public event KeyPressEventHandler KeyPressed;

        public const int VRAM_SIZE = 0x100000;
        private const int REGISTER_BLOCK_SIZE = 256;

        public MemoryRAM Memory = null;

        // Video page 0 is 0x1000. Each page is 2000 bytes long.
        private int characterMatrixStart = MemoryMap_DirectPage.SCREEN_PAGE0;
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
        private int refreshTimer = 0;
        public int BlinkRate = 20;

        int columns = 80;
        int rows = 25;
        int bufferSize = 2000;
        //bool halfWidth = false;
        //int _cursorCol = 0;
        //int _cursorRow = 0;
        int cursorPos = 0;
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
            if (Memory == null)
                return 0;
            int baseAddress = Memory.ReadLong(MemoryMap_DirectPage.SCREENBEGIN);
            return baseAddress + row * Columns + col;
        }

        /// <summary>
        /// Column of the cursor position. 0 is left edge
        /// </summary>
        public int X
        {
            get
            {
                if (Memory == null)
                    return 0;

                return Memory.ReadByte(MemoryMap_DirectPage.CURSORX);
            }
            set
            {
                int x = value;
                if (x < 0)
                    x = 0;
                if (x >= Columns)
                    x = Columns - 1;
                if (Memory != null)
                    Memory.WriteByte(MemoryMap_DirectPage.CURSORX, (byte)x);
                ResetDrawTimer();
                CursorPos = GetCharPos(Y, x);
            }
        }

        /// <summary>
        /// Row of cursor position. 0 is top of the screen
        /// </summary>
        public int Y
        {
            get
            {
                if (Memory == null)
                    return 0;

                return Memory.ReadByte(MemoryMap_DirectPage.CURSORY);
            }
            set
            {
                int y  = value;
                if (y < 0)
                    y = 0;
                if (y >= Rows)
                    y = Rows - 1;
                if (Memory != null)
                    Memory.WriteByte(MemoryMap_DirectPage.CURSORY, (byte)y);
                ResetDrawTimer();
                CursorPos = GetCharPos(y, X);
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
            //this.SetBufferSize(25, 40);
            this.Paint += new PaintEventHandler(Gpu_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000 / 60;
            this.VisibleChanged += new EventHandler(FrameBufferControl_VisibleChanged);
            this.DoubleBuffered = true;

            var info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            var tot = info.TotalPhysicalMemory;
            var fre = info.AvailablePhysicalMemory;

            tot = (tot / 1024 / 1024 / 1024);
            fre = (fre / 1024 / 1024);
            string line1 = "**** Nu64 BASIC TEST PLATFORM ****";
            string line2 = tot.ToString() + "GB RAM SYSTEM " + fre.ToString() + " MEGABYTES FREE";
            int adjust = columns / 2 - line1.Length / 2;

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

        public void LoadCharacterData(MemoryRAM newVRAM)
        {
            this.Memory = newVRAM;
            LoadCharacterSet("ASCII-PET", @"Resources\FOENIX-CHARACTER-ASCII.bin", 0, CharacterSet.CharTypeCodes.ASCII_PET, CharacterSet.SizeCodes.Size8x8);
            //LoadCharacterSet("PETSCII_GRAPHICS", @"Resources\PETSCII.901225-01.bin", 0, CharacterSet.CharTypeCodes.PETSCII_GRAPHICS, CharacterSet.SizeCodes.Size8x8);
            //LoadCharacterSet("PETSCII_TEXT", @"Resources\PETSCII.901225-01.bin", 4096, CharacterSet.CharTypeCodes.PETSCII_TEXT, CharacterSet.SizeCodes.Size8x8);
        }

        private Font GetBestFont()
        {
            Font useFont = null;
            float rowHeight = this.ClientRectangle.Height / (float)Rows;
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
            refreshTimer = 0;
            CursorState = true;
        }


        public int Columns
        {
            get
            {
                return this.columns;
            }
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }
        }

        public int BufferSize
        {
            get
            {
                return bufferSize;
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
                if (Memory == null)
                    return 0;
                return Memory.ReadWord(MemoryMap_DirectPage.CURSORPOS);
            }

            set
            {
                if (Memory == null)
                    return;
                Memory.WriteWord(MemoryMap_DirectPage.CURSORPOS, value);
            }
        }


        /// <summary>
        /// Draw the frame buffer to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Gpu_Paint(object sender, PaintEventArgs e)
        {
            //DrawVectorText(e.Graphics);
            DrawBitmapText(e.Graphics);
        }

        private void DrawVectorText(Graphics g)
        {

            //float x;
            //float y;

            //if (TextFont == null)
            //    TextFont = GetBestFont();
            //SizeF charSize = MeasureFont(TextFont, g);
            //float charWidth = charSize.Width / MEASURE_STRING.Length;
            //float charHeight = charSize.Height;
            //float Col80 = charWidth * columns;
            //float Row25 = charWidth * rows;

            //if (halfWidth)
            //{
            //    g.ScaleTransform(0.5f, 1.0f);
            //}
            //float ScaleX = this.ClientRectangle.Width / Col80;
            //float ScaleY = this.ClientRectangle.Height / Row25;
            //g.ScaleTransform(ScaleX, ScaleY);

            //g.Clear(Color.Blue);

            //if (VRAM == null)
            //    return;

            //int col = 0, row = 0;
            //for (int i = 0; i < BufferSize; i++)
            //{
            //    x = col * charWidth;
            //    y = row * charHeight;

            //    if ((ColorData[i] & ColorCodes.Reverse) == 0)
            //        g.DrawString(CharacterData[i].ToString(), TextFont, TextBrush, x, y, StringFormat.GenericTypographic);
            //    else
            //    {
            //        g.FillRectangle(CursorBrush, x, y, charWidth, charHeight);
            //        g.DrawString(CharacterData[i].ToString(),
            //            TextFont,
            //            InvertedBrush,
            //            x, y,
            //            StringFormat.GenericTypographic);
            //    }

            //    col++;
            //    if (col >= Columns)
            //    {
            //        col = 0;
            //        row++;
            //    }
            //}

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

        private void DrawBitmapText(Graphics controlGraphics)
        {
            Graphics g = Graphics.FromImage(frameBuffer);

            float x;
            float y;

            ColorMap[] colorMap = new ColorMap[1];
            colorMap[0] = new ColorMap();
            colorMap[0].OldColor = Color.White;
            colorMap[0].NewColor = Color.LightGreen;
            ImageAttributes attr = new ImageAttributes();
            attr.SetRemapTable(colorMap);

            //if (TextFont == null)
            //    TextFont = GetBestFont();
            //SizeF charSize = MeasureFont(TextFont, g);
            float charWidth = 8; //charSize.Width / MEASURE_STRING.Length;
            float charHeight = 8; //charSize.Height;
            float Col80 = charWidth * columns;
            float Row25 = charWidth * rows;

            //if (halfWidth)
            //{
            //    g.ScaleTransform(0.5f, 1.0f);
            //}
            //float ScaleX = this.ClientRectangle.Width / Col80;
            //float ScaleY = this.ClientRectangle.Height / Row25;
            //g.ScaleTransform(ScaleX, ScaleY);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            g.Clear(Color.Blue);

            if (Memory == null)
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

            //this._cursorCol = Memory.ReadByte(MemoryMap_DirectPage.CURSORX);
            //this._cursorRow = Memory.ReadByte(MemoryMap_DirectPage.CURSORY);

            int col = 0, row = 0;
            for (int i = 0; i < BufferSize; i++)
            {
                x = col * charWidth;
                y = row * charHeight;

                byte c = Memory.ReadByte(characterMatrixStart + i);
                Bitmap bmp = CharacterSetSlots[0].Bitmaps[c];
                RectangleF rect = new RectangleF((int)x, (int)y, bmp.Width, bmp.Height);
                //g.DrawImage(bmp, rect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, attr);
                g.DrawImage(bmp, rect);

                col++;
                if (col >= Columns)
                {
                    col = 0;
                    row++;
                }
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
            if (refreshTimer-- > 0)
                return;

            this.Refresh();

            CursorState = !CursorState;
            refreshTimer = BlinkRate;
        }

        void FrameBufferControl_VisibleChanged(object sender, EventArgs e)
        {
            timer.Enabled = this.Visible;
        }

        private void FrameBuffer_SizeChanged(object sender, global::System.EventArgs e)
        {
            TextFont = GetBestFont();
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

        public virtual void SetScreenSize(int Rows, int Columns)
        {
            //CharacterData = new char[Rows * Columns];
            //characterMatrixStart = REGISTER_BLOCK_SIZE;
            //ColorData = new ColorCodes[Rows * Columns];
            //colorMatrixStart = REGISTER_BLOCK_SIZE + CharacterData.Length;

            this.columns = Columns;
            this.rows = Rows;
            this.bufferSize = Columns * Rows;

            //TextFont = GetBestFont();
            //MEASURE_STRING = new string('W', Columns);
        }

        public byte ReadByte(int Address)
        {
            if (Address >= 0 && Address < REGISTER_BLOCK_SIZE)
            {
                return GetGPURegister(Address);
            }
            //else if (Address >= characterMatrixStart && Address < (characterMatrixStart + CharacterData.Length))
            //{
            //    return (byte)CharacterData[Address - characterMatrixStart];
            //}
            //else if (Address >= colorMatrixStart && Address < (colorMatrixStart + ColorData.Length))
            //{
            //    return (byte)ColorData[Address - colorMatrixStart];
            //}
            return 0;
        }

        /// <summary>
        /// return the GPU registers: start of text page, start of color page, start of character data, 
        /// number of columns, number of rows, graphics mode, etc. 
        /// </summary>
        /// <param name="Address">Address to read</param>
        /// <returns></returns>
        public byte GetGPURegister(int Address)
        {
            return 0;
        }

        public void SetGPURegister(int Address, byte Data)
        {

        }

        public void WriteByte(int Address, byte Data)
        {
            if (Address >= 0 && Address < REGISTER_BLOCK_SIZE)
            {
                SetGPURegister(Address, Data);
            }
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
            cs.Load(Filename, Offset, Memory, characterSetStart + CharacterSetSlots.Count * CharacterSet.SlotSize, CharSize);
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
