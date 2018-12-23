using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXProc;

namespace WorldEditor
{
    public partial class CaptureFromImgForm : Form
    {
        public byte[][] ExtTiles = new byte[192][]; //бинарники тайлов
        public Bitmap[] ExtTilesImg = new Bitmap[192]; //миниатюры тайлов

        Point StartSel; //стартовая точка выделения области тайлов
        Point FinishSel; //финальная точка выделения области тайлов
        Rectangle SelTiles; //выделенный тайл
        Bitmap ScreenImg = new Bitmap(512, 384); //изображение скрина с тайлами

        //захват тайлсета
        void CaptureTileset()
        {
            //чтение SCR в массив
            byte[] scrfile = new byte[6912];
            BinaryReader reader = new BinaryReader(File.Open(ImgName.Text, FileMode.Open));
            for (int i = 0; i < 6912; i++)
                scrfile[i] = reader.ReadByte();
            reader.Close();
            //нарезаем тайлы
            Graphics g = Graphics.FromImage(ScreenImg);
            for (int y = 0; y < 12; y++)
                for (int x = 0; x < 16; x++)
                {
                    int n = x + y * 16;
                    ExtTiles[n] = new byte[37];
                    for (int j = 0; j < 16; j++)
                        for (int i = 0; i < 2; i++)
                        {
                            int xscr = x * 2 + i;
                            int yscr = y * 16 + j;
                            int scradr = xscr + ((yscr << 2) & 0xe0) + ((yscr & 7) * 256) + (((yscr >> 3) & 0x18) * 256);
                            ExtTiles[n][i + j * 2] = scrfile[scradr];
                        }
                    for (int j = 0; j < 2; j++)
                        for (int i = 0; i < 2; i++)
                            ExtTiles[n][32 + i + j * 2] = scrfile[6144 + (x * 2 + i) + (y * 2 + j) * 32];
                    ExtTilesImg[n] = new Bitmap(ZX.DrawTile(ExtTiles[n], 2));
                    g.DrawImage(ExtTilesImg[n], x * 32, y * 32);
                }
            ScreenPicture.Image = ScreenImg;           
        }

        void ShowSelection()
        {
            //отображение выделения области тайлов
            if (ImgName.Text == "")
                return;
            Bitmap img = new Bitmap (ScreenImg);
            Graphics g = Graphics.FromImage(img);
            int x1 = StartSel.X / 32;
            int y1 = StartSel.Y / 32;
            int x2 = FinishSel.X / 32;
            int y2 = FinishSel.Y / 32;
            if (x1 > x2)
            {
                int x = x1;
                x1 = x2;
                x2 = x;
            }
            if (y1 > y2)
            {
                int y = y1;
                y1 = y2;
                y2 = y;
            }
            SelTiles = new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
            g.DrawRectangle(new Pen(Color.Red), x1 * 32, y1 * 32, (x2 - x1 + 1) * 32 - 1, (y2 - y1 + 1) * 32 - 1);
            ScreenPicture.Image = img;
        }

        //-------------------------------------------------------------------------------------------------

        public CaptureFromImgForm()
        {
            InitializeComponent();
            FileDialog.InitialDirectory = Application.ExecutablePath;
        }

        private void CaptureFromImgForm_Activated(object sender, EventArgs e)
        {
            StartSel = new Point(-1, -1);
            SelTiles = new Rectangle(-1, -1, -1, -1);
            ScreenPicture.Image = ScreenImg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //открытие файла с картинкой
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                ImgName.Text = FileDialog.FileName;
                CaptureTileset();
            }
        }

        private void ScreenPicture_MouseDown(object sender, MouseEventArgs e)
        {
            //начало выделения области тайлов
            StartSel = e.Location;
            FinishSel = StartSel;
            ShowSelection();
        }
        
        private void ScreenPicture_MouseMove(object sender, MouseEventArgs e)
        {
            //продолжение выделения области тайлов
            if (e.Button == MouseButtons.None || StartSel.X == -1)
                return;
            int x = e.X;
            if (x > 511)
                x = 511;
            if (x < 0)
                x = 0;
            int y = e.Y;
            if (y > 383)
                y = 383;
            if (y < 0)
                y = 0;
            FinishSel = new Point(x, y);
            ShowSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LocationEditorForm.TilesBuffer = null;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SelTiles.Width == -1 || SelTiles.Height == -1)
            {
                MessageBox.Show("Please, select tiles.", "No tiles selected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LocationEditorForm.TilesBuffer = new byte[SelTiles.Width, SelTiles.Height][];
            LocationEditorForm.TilesBufferImg = new Bitmap[SelTiles.Width, SelTiles.Height];
            for (int y = 0; y < SelTiles.Height; y++)
                for (int x = 0; x < SelTiles.Width; x++)
                {
                    LocationEditorForm.TilesBuffer[x, y] = new byte[37];
                    for (int i = 0; i < 36; i++)
                        LocationEditorForm.TilesBuffer[x, y][i] = ExtTiles[SelTiles.X + x + (SelTiles.Y + y) * 16][i];
                    LocationEditorForm.TilesBufferImg[x, y] = new Bitmap(ExtTilesImg[SelTiles.X + x + (SelTiles.Y + y) * 16]);
                }
            LocationEditorForm.TilesSelectedRect = SelTiles;
            this.Close();
        }
    }
}
