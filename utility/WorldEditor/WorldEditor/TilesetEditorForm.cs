using System;
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
    public partial class TilesetEditorForm : Form
    {

        class ScreenUndoData
        {
            public bool pix; //пискели или аттрибуты
            public int x; //координата x
            public int y; //координата y
            public int prev; //предыдущие значение
            public int now; //текущее значение
        }

        int[,] PixelLayer = new int[256, 256]; //слой пикселей
        int[,] AttrLayer = new int[32, 32]; //слой атрибутов
        int[,] PrevPixelLayer = new int[256, 256]; //предыдущий слой пикселей (для Undo)
        int[,] PrevAttrLayer = new int[32, 32]; //предыдущий слой атрибутов (для Undo)
        int PixSize = 2; //размер пикселя
        int XCord; //текущая координата X
        int YCord; //текущая координата Y
        bool MouseLeft = false;
        bool MouseRight = false;
        int Ink = 0; // текущий цвет чернил
        int Paper = 7; // текущий цвет бумаги
        Point LastTile = new Point(-1, -1); //координаты предыдущего тайла
        Bitmap Img = new Bitmap(4096, 4096); //Bitmap для PictureBox
        List<List<ScreenUndoData>> ScreenUndoBuffer = new List<List<ScreenUndoData>>(); //буфер Undo
        int ScreenUndoIndex = -1; //индекс undo
        bool isPixSet = false; //был установлен пиксель
        bool _ScreenChanged; //экран был изменен
        bool ScreenChanged 
        {
            get {return _ScreenChanged; }
            set 
            {
                _ScreenChanged = value;
                SaveButton.Enabled = value;
            }
        }


        //--------------------------------- Общие подпрограммы ------------------------------------

        //инициализация редактора
        public void InitTilesetEditor()
        {
            LoadTileset();
            ClearUndoBuffer();
            ChangeScale();
            ShowScreen();
            ShowPalette();
        }

        //загрузка тайлсета в слои PixelLayer & AttrLayer редактора
        void LoadTileset()
        {
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    for (int j = 0; j < 16; j++)
                        for (int i = 0; i < 2; i++)
                            for (int k = 0; k < 8; k++)
                                if (((LocationEditorForm.Tileset[x + y * 16, i + j * 2] << k) & 0x80) != 0)
                                    PixelLayer[x * 16 + i * 8 + k, y * 16 + j] = 1;
                                else
                                    PixelLayer[x * 16 + i * 8 + k, y * 16 + j] = 0;
                    for (int j = 0; j < 2; j++)
                        for (int i = 0; i < 2; i++)
                            AttrLayer[x * 2 + i, y * 2 + j] = LocationEditorForm.Tileset[x + y * 16, 32 + i + j * 2];
                }
        }

        //сохранение слоёв PixelLayer & AttrLayer в Tileset
        void SaveTileset()
        {
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    for (int j = 0; j < 16; j++)
                        for (int i = 0; i < 2; i++)
                        {
                            byte b = 0;
                            for (int k = 0; k < 8; k++)
                            {
                                b *= 2;
                                if (PixelLayer[x * 16 + i * 8 + k, y * 16 + j] != 0)
                                    b++;
                            }
                            LocationEditorForm.Tileset[x + y * 16, i + j * 2] = b;
                        }
                    for (int j = 0; j < 2; j++)
                        for (int i = 0; i < 2; i++)
                            LocationEditorForm.Tileset[x + y * 16, 32 + i + j * 2] = (byte)(AttrLayer[x * 2 + i, y * 2 + j]);
                }
            LocationEditorForm.TilesetChanged = true;
        }

        //вывод экрана
        void ShowScreen()
        {
            Graphics g = Graphics.FromImage(Img);
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                    ShowTile(x, y); 
            ViewImg.Refresh();
        }

        //вывод тайла
        void ShowTile(int x, int y)
        {
            Graphics g = Graphics.FromImage(Img);
            for (int j = y * 16; j < y * 16 + 16; j++)
                for (int i = x * 16; i < x * 16 + 16; i++)
                {
                    int bright = (AttrLayer[i / 8, j / 8] >> 3) & 0x08;
                    Color ink = ZX.GetColor((AttrLayer[i / 8, j / 8] & 0x07) + bright);
                    Color paper = ZX.GetColor(((AttrLayer[i / 8, j / 8] >> 3) & 0x07) + bright);
                    Color c = PixelLayer[i, j] == 0 ? paper : ink;
                    Brush b = new SolidBrush(c);
                    g.FillRectangle(b, i * PixSize, j * PixSize, PixSize, PixSize);

                }
            if (ShowGrid.Checked)
            {
                Pen p = new Pen(Color.Gray);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawLine(p, x * 16 * PixSize, y * 16 * PixSize, x * 16 * PixSize, (y + 1) * 16 * PixSize);
                g.DrawLine(p, x * 16 * PixSize, y * 16 * PixSize, (x + 1) * 16 * PixSize, y * 16 * PixSize);

                p = new Pen(Color.LightGray);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawLine(p, (x * 16 + 8) * PixSize, y * 16 * PixSize, (x * 16 + 8) * PixSize, (y + 1) * 16 * PixSize);
                g.DrawLine(p, x * 16 * PixSize, (y * 16 + 8) * PixSize, (x + 1) * 16 * PixSize, (y * 16 + 8) * PixSize);
            }
        }

        //вывод палитры
        void ShowPalette()
        {
            int b = 0;
            if (BrightCheck.Checked)
                b = 8;
            for (int i = 0; i < 8; i++)
            {      
                Color c = ZX.GetColor((byte)(i + b));
                if (!ColorsCheck.Checked)
                    c = Color.LightGray;
                var p = PalettePanel.Controls["I" + i.ToString()];
                p.BackColor = c;
                if (i == Ink)
                    p.Text = "V";
                else
                    p.Text = "";
                if (i < 4 && ColorsCheck.Checked)
                    p.ForeColor = Color.White;
                else
                    p.ForeColor = Color.Black;
                p = PalettePanel.Controls["P" + i.ToString()];
                p.BackColor = c;
                if (i == Paper)
                    p.Text = "V";
                else
                    p.Text = "";
                if (i < 4 && ColorsCheck.Checked)
                    p.ForeColor = Color.White;
                else
                    p.ForeColor = Color.Black;
            }
            if (!ColorsCheck.Checked)
            {
                AttrColor.BackColor = Color.LightGray;
                AttrColor.ForeColor = Color.Black;
            }
            else
            {
                AttrColor.BackColor = ZX.GetColor((byte)(Paper + b));
                AttrColor.ForeColor = ZX.GetColor((byte)(Ink + b));
            }
        }

        //захват значений аттрибута по координатам XCord и YCord
        void CaptureColors()
        {
            int i = AttrLayer[XCord / 8, YCord / 8];
            Ink = i & 7;
            Paper = (i >> 3) & 7;
            BrightCheck.Checked = (i & 64) == 0 ? false : true;
            ColorsCheck.Checked = true;
            CaptureColor.Checked = false;
            ShowPalette();
        }

        //инвертирование цветов аттрибута и его пикселей
        void InvertColors()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    PixelLayer[XCord / 8 * 8 + x, YCord / 8 * 8 + y] = 1 - PixelLayer[XCord / 8 * 8 + x, YCord / 8 * 8 + y];
            int i = AttrLayer[XCord / 8, YCord / 8];
            int ink = i & 7;
            int paper = (i >> 3) & 7;
            int bright = (i & 64) == 0 ? 0 : 1;
            AttrLayer[XCord / 8, YCord / 8] = paper + ink * 8 + bright * 64;
            isPixSet = true;
            ScreenChanged = true;
            CaptureColors();
            ShowTile(XCord / 16, YCord / 16);
        }

        //изменение масштаба тайлсета
        void ChangeScale()
        {
            PixSize = ScaleBar.Value;
            if (PixSize < 2)
                PixSize = 2;
            if (PixSize > 16)
                PixSize = 16;
            Size s = new Size(PixSize * 256, PixSize * 256);
            ViewImg.Size = s;
            ShowScreen();
        }

        //нарисовать пиксель на экран по координатам XCord и YCord
        void SetPixel()
        {
            int i = 0;
            if (MouseLeft)
                i = 1;
            PixelLayer[XCord, YCord] = i;
            if (ColorsCheck.Checked)
            {
                int c = Ink + Paper * 8;
                if (BrightCheck.Checked)
                    c += 64;
                AttrLayer[XCord / 8, YCord / 8] = c;
            }

            ShowTile(XCord / 16, YCord / 16);
            isPixSet = true;
        }

        //сохранение текущих массивов пикселей и атрибутов в предыдущие
        void SavePixAttrArray()
        {
            for (int j = 0; j < 256; j++)
                for (int i = 0; i < 256; i++)
                    PrevPixelLayer[i, j] = PixelLayer[i, j];
            for (int j = 0; j < 32; j++)
                for (int i = 0; i < 32; i++)
                    PrevAttrLayer[i, j] = AttrLayer[i, j];
        }

        //очистка и инициализация буфера Undo
        void ClearUndoBuffer()
        {
            ScreenUndoBuffer.Clear();
            ScreenUndoIndex = -1;
            ScreenChanged = false;
            SavePixAttrArray();
            UndoButton.Enabled = false;
            RedoButton.Enabled = false;
        }

        //добавить изменения экрана в буфер Undo
        void AddScreenChanges()
        {
            ScreenUndoIndex++;
            if (ScreenUndoIndex <= ScreenUndoBuffer.Count - 1)
                ScreenUndoBuffer.RemoveRange(ScreenUndoIndex, ScreenUndoBuffer.Count - ScreenUndoIndex);
            ScreenUndoBuffer.Add(new List<ScreenUndoData>());
            for (int j = 0; j < 256; j++)
                for (int i = 0; i < 256; i++)
                    if (PrevPixelLayer[i, j] != PixelLayer[i, j])
                    {
                        ScreenUndoData s = new ScreenUndoData();
                        s.pix = true;
                        s.x = i;
                        s.y = j;
                        s.prev = PrevPixelLayer[i, j];
                        s.now = PixelLayer[i, j];
                        ScreenUndoBuffer[ScreenUndoIndex].Add(s);
                    }
            for (int j = 0; j < 32; j++)
                for (int i = 0; i < 32; i++)
                    if (PrevAttrLayer[i, j] != AttrLayer[i, j])
                    {
                        ScreenUndoData s = new ScreenUndoData();
                        s.pix = false;
                        s.x = i;
                        s.y = j;
                        s.prev = PrevAttrLayer[i, j];
                        s.now = AttrLayer[i, j];
                        ScreenUndoBuffer[ScreenUndoIndex].Add(s);
                    }
            SavePixAttrArray();
            UndoButton.Enabled = true;
            RedoButton.Enabled = false;
            isPixSet = false;
            ScreenChanged = true;
        }

        //восстановить экран
        void UndoScreen()
        {
            if (ScreenUndoIndex == -1)
                return;

            foreach (ScreenUndoData s in ScreenUndoBuffer[ScreenUndoIndex])
            {
                if (s.pix)
                    PixelLayer[s.x, s.y] = s.prev;
                else
                    AttrLayer[s.x, s.y] = s.prev;
            }
            SavePixAttrArray();
            ScreenChanged = true;
            ScreenUndoIndex--;

            if (ScreenUndoIndex == -1)
            {
                ScreenChanged = false;
                UndoButton.Enabled = false;
            }
            RedoButton.Enabled = true;
            ShowScreen();
        }

        //отменить восстановление экрана
        void RedoScreen()
        {
            if (ScreenUndoIndex == ScreenUndoBuffer.Count - 1)
                return;

            ScreenUndoIndex++;
            foreach (ScreenUndoData s in ScreenUndoBuffer[ScreenUndoIndex])
            {
                if (s.pix)
                    PixelLayer[s.x, s.y] = s.now;
                else
                    AttrLayer[s.x, s.y] = s.now;
            }

            UndoButton.Enabled = true;
            if (ScreenUndoIndex == ScreenUndoBuffer.Count - 1)
                RedoButton.Enabled = false;

            SavePixAttrArray();
            ScreenChanged = true;
            ShowScreen();
        }

        //--------------------------------------- Интерфейс ---------------------------------------

        public TilesetEditorForm()
        {
            InitializeComponent();
            ViewImg.Image = Img;
        }

        //работа с Img
        private void ViewImg_MouseMove(object sender, MouseEventArgs e)
        {
            XCord = e.X / PixSize;
            if (XCord < 0)
                XCord = 0;
            if (XCord > 255)
                XCord = 255;
            YCord = e.Y / PixSize;
            if (YCord < 0)
                YCord = 0;
            if (YCord > 255)
                YCord = 255;
            XposText.Text = XCord.ToString();
            YposText.Text = YCord.ToString();

            if (MouseLeft || MouseRight)
                SetPixel();

            if (LastTile.X != -1 && LastTile.Y != -1)
                ShowTile(LastTile.X, LastTile.Y);
            int xtile = XCord / 16;
            int ytile = YCord / 16;
            Graphics g = Graphics.FromImage(Img);
            Pen p = new Pen(Color.Red);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(p, xtile * PixSize * 16, ytile * PixSize * 16, PixSize * 16 - 1, PixSize * 16 - 1);
            ViewImg.Refresh();
            LastTile.X = xtile;
            LastTile.Y = ytile;
        }

        private void ViewImg_MouseLeave(object sender, EventArgs e)
        {
            XposText.Text = "";
            YposText.Text = "";
            ShowTile(LastTile.X, LastTile.Y);
            LastTile.X = -1;
            LastTile.Y = -1;
            MouseLeft = false;
            MouseRight = false;
            ViewImg.Refresh();
        }

        private void ViewImg_MouseDown(object sender, MouseEventArgs e)
        {
            //захват значений аттрибута
            if (CaptureColor.Checked)
            {
                CaptureColors();
                return;
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                    MouseLeft = true;
                else
                    MouseRight = true;
                SetPixel();
                ViewImg.Refresh();
            }
        }

        private void ViewImg_MouseUp(object sender, MouseEventArgs e)
        {
            MouseLeft = false;
            MouseRight = false;
            if (isPixSet)
                AddScreenChanges();
        }


        //изменение масштаба
        private void ScaleBar_Scroll(object sender, EventArgs e)
        {
            ChangeScale();
        }

        //изменение видимости сетки
        private void ShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            ShowScreen();
        }

        //изменения палитры
        private void ColorsCheck_CheckedChanged(object sender, EventArgs e)
        {
            ShowPalette();
        }

        private void BrightCheck_CheckedChanged(object sender, EventArgs e)
        {
            ShowPalette();
        }

        //выбор цвета в палитре
        private void I0_Click(object sender, EventArgs e)
        {
            Ink = 0;
            ShowPalette();
        }

        private void I1_Click(object sender, EventArgs e)
        {
            Ink = 1;
            ShowPalette();
        }

        private void I2_Click(object sender, EventArgs e)
        {
            Ink = 2;
            ShowPalette();
        }

        private void I3_Click(object sender, EventArgs e)
        {
            Ink = 3;
            ShowPalette();
        }

        private void I4_Click(object sender, EventArgs e)
        {
            Ink = 4;
            ShowPalette();
        }

        private void I5_Click(object sender, EventArgs e)
        {
            Ink = 5;
            ShowPalette();
        }

        private void I6_Click(object sender, EventArgs e)
        {
            Ink = 6;
            ShowPalette();
        }

        private void I7_Click(object sender, EventArgs e)
        {
            Ink = 7;
            ShowPalette();
        }

        private void P0_Click(object sender, EventArgs e)
        {
            Paper = 0;
            ShowPalette();
        }

        private void P1_Click(object sender, EventArgs e)
        {
            Paper = 1;
            ShowPalette();
        }

        private void P2_Click(object sender, EventArgs e)
        {
            Paper = 2;
            ShowPalette();
        }

        private void P3_Click(object sender, EventArgs e)
        {
            Paper = 3;
            ShowPalette();
        }

        private void P4_Click(object sender, EventArgs e)
        {
            Paper = 4;
            ShowPalette();
        }

        private void P5_Click(object sender, EventArgs e)
        {
            Paper = 5;
            ShowPalette();
        }

        private void P6_Click(object sender, EventArgs e)
        {
            Paper = 6;
            ShowPalette();
        }

        private void P7_Click(object sender, EventArgs e)
        {
            Paper = 7;
            ShowPalette();
        }

        //хот-кеи
        private void TilesetEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            //захват цвета
            if (e.KeyCode == Keys.C && XCord >=0 && XCord < 256 && YCord >=0 && YCord < 256)
                CaptureColors();

            //инвертирование пикселей и цветов в атрибуте
            if (e.KeyCode == Keys.I && XCord >= 0 && XCord < 256 && YCord >= 0 && YCord < 256)
                InvertColors();

            //масштаб
            if ((e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)  && ScaleBar.Value > 2)
            {
                ScaleBar.Value -= 1;
                ChangeScale();
            }
            if ((e.KeyCode == Keys.Oemplus || e.KeyCode == Keys.Add) && ScaleBar.Value < 16)
            {
                ScaleBar.Value += 1;
                ChangeScale();
            }

            //отмена изменений
            if (e.Control && e.KeyCode == Keys.Z)
                UndoScreen();

            //возврат изменений
            if (e.Control && e.KeyCode == Keys.Y)
                RedoScreen();

            //сохранение тайлсета
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveTileset();
                ScreenChanged = false;
            }
        }

        //вызов отмены изменений
        private void UndoButton_Click(object sender, EventArgs e)
        {
            UndoScreen();
        }

        //вызов возврата изменений
        private void RedoButton_Click(object sender, EventArgs e)
        {
            RedoScreen();
        }

        //сохранение изменений в тайлах
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveTileset();
            ScreenChanged = false;
        }

        //закрытие редактора
        private void TilesetEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ScreenChanged)
                if (MessageBox.Show("Save changes?", "Tileset has been changed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    SaveTileset();
            ScreenChanged = false;
        }        
    }
}
