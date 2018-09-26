using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nu64.CharEdit
{
    public partial class EditControl : UserControl
    {
        byte[] clipData = null;
        byte[] FontData = null;
        byte[] characterData = new byte[16];
        Panel[,] grid = null;
        Bitmap characterBitmap = null;
        int CharIndex = 0;
        int Columns = 8;
        int Rows = 0;

        public event EventHandler CharacterSaved;

        Brush textBrush = new SolidBrush(Color.LightGreen);

        public EditControl()
        {
            InitializeComponent();
        }

        internal void LoadCharacter(byte[] FontData, int selectedIndex, int bytesPerCharacter)
        {
            this.FontData = FontData;
            this.CharIndex = selectedIndex;
            Rows = bytesPerCharacter;

            LoadCharacter();
        }

        private void LoadCharacter()
        {
            characterData = new byte[Rows];
            int pos = CharIndex * Rows;
            for (int i = 0; i < Rows; i++)
            {
                characterData[i] = FontData[pos + i];
            }
            DisplayCharacter();
        }

        private void DisplayCharacter()
        {
            int x = 0;
            int y = 0;

            if (grid == null)
            {
                grid = new Panel[Rows, Columns];
                for (y = 0; y < Rows; y++)
                {
                    for (x = 0; x < Columns; x++)
                    {
                        Panel p = new Panel();
                        p.Left = x * 32;
                        p.Top = y * 32;
                        p.Width = 32;
                        p.Height = 32;
                        p.BorderStyle = BorderStyle.FixedSingle;
                        p.Click += P_Click;
                        grid[y, x] = p;
                        characterPanel.Controls.Add(p);
                    }
                }
            }

            //Graphics g = Graphics.FromImage(characterBitmap);
            for (y = 0; y < Rows; y++)
            {
                byte b = characterData[y];
                x = 0;
                for (int bit = 128; bit > 0;)
                {
                    if ((b & bit) > 0)
                        grid[y, x].BackColor = characterPanel.ForeColor;
                    else
                        grid[y, x].BackColor = characterPanel.BackColor;
                    x += 1;
                    bit = bit >> 1;
                }
            }
        }

        private void P_Click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            if (p == null)
                return;

            if (p.BackColor == characterPanel.BackColor)
                p.BackColor = characterPanel.ForeColor;
            else
                p.BackColor = characterPanel.BackColor;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveCharacter();

            int j = CharIndex * Rows;
            for (int i = 0; i < Rows; i++)
            {
                FontData[j + i] = characterData[i];
            }

            CharacterSaved?.Invoke(this, new EventArgs());
        }

        private void SaveCharacter()
        {
            for (int y = 0; y < Rows; y++)
            {
                int row = 0;
                for (int x = 0; x < Columns; x++)
                {
                    int bit = (int)Math.Pow(2, (Columns - x - 1));
                    if (grid[y, x].BackColor == characterPanel.ForeColor)
                        row = row | bit;
                }
                characterData[y] = (byte)row;
            }
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = Columns - 1; x > 0; x--)
                {
                    grid[y, x].BackColor = grid[y, x - 1].BackColor;
                }
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            LoadCharacter();
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns - 1; x++)
                {
                    grid[y, x].BackColor = grid[y, x + 1].BackColor;
                }
            }

        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            SaveCharacter();
            clipData = new byte[Rows];
            characterData.CopyTo(clipData, 0);
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            if (clipData == null)
                return;

            clipData.CopyTo(characterData, 0);
            DisplayCharacter();
        }
    }
}
