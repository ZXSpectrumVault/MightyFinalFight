using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using ZXProc;

namespace SpriteCutter
{
    public partial class CaptureForm : Form
    {
        private Bitmap img;
        private Point StartPoint = new Point(-1, -1);
        private Point FinishPoint = new Point(-1, -1);
        private Point MouseCord = new Point();
        private bool MouseButton;
        private string SpriteName = "";
        private int AutoNameNum;

        public CaptureForm()
        {
            InitializeComponent();
        }

        //открыть картинку
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                try
                {
                    FileStream fs = new FileStream(OpenFileDialog.FileName, FileMode.Open);
                    Image p = Image.FromStream(fs);
                    fs.Close();
                    img = new Bitmap(p);

                    //преобразование в монохром
                    if (SpriteType0.Checked)
                    {

                        //определяем самый темный пиксель на картинке
                        int min = 255;
                        Color minc = Color.White;
                        for (int y = 0; y < img.Height; y++)
                            for (int x = 0; x < img.Width; x++)
                            {
                                Color c = img.GetPixel(x, y);
                                int i = (c.R + c.G + c.B) / 3;
                                if (i < min)
                                {
                                    min = i;
                                    minc = c;
                                }
                            }

                        //заменяем самый темный пиксель на черный, все остальные на белый
                        for (int y = 0; y < img.Height; y++)
                            for (int x = 0; x < img.Width; x++)
                                if (img.GetPixel(x, y) == minc)
                                    img.SetPixel(x, y, MainForm.ColorEnable);
                                else
                                    img.SetPixel(x, y, MainForm.ColorDisable);
                    }

                    //задаем размеры CaptureForm
                    ImageBox.Size = img.Size;
                    this.Width = img.Width < 340 ? 380 : img.Width + 40;
                    this.Height = img.Height + 110;
                    ImageBox.Image = img;
                    this.Left = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
                    this.Top = (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
                }
                catch
                {
                    MessageBox.Show("Can not open this file.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //обработка ImageBox
        private void ImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (img != null)
            {
                if (e.X < 0)
                    MouseCord.X = 0;
                else if (e.X >= img.Width)
                    MouseCord.X = img.Width - 1;
                else
                    MouseCord.X = e.X;
                if (e.Y < 0)
                    MouseCord.Y = 0;
                else if (e.Y >= img.Height)
                    MouseCord.Y = img.Height - 1;
                else
                    MouseCord.Y = e.Y;

                if (MouseButton && StartPoint.X != -1)
                {
                    Bitmap b = new Bitmap(img);
                    Pen p = new Pen(Color.Red);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    Graphics g = Graphics.FromImage(b);
                    int x = MouseCord.X;
                    int y = MouseCord.Y;
                    int w = StartPoint.X - x;
                    int h = StartPoint.Y - y;
                    if (w < 0)
                    {
                        x = StartPoint.X;
                        w = MouseCord.X - x;
                    }
                    if (h < 0)
                    {
                        y = StartPoint.Y;
                        h = MouseCord.Y - y;
                    }
                    g.DrawRectangle(p, x, y, w, h);
                    ImageBox.Image = b;
                }
            }
        }

        private void ImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (img != null)
            {
                StartPoint = MouseCord;
                FinishPoint = MouseCord;
                ImageBox.Image = img;
                MouseButton = true;
            }
        }

        private void ImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (img != null)
            {
                FinishPoint = MouseCord;
                MouseButton = false;
            }
        }

        //одиночный захват спрайта
        private void CaptureButton_Click(object sender, EventArgs e)
        {
            if (StartPoint != FinishPoint && StartPoint.X != - 1 && StartPoint.Y != -1)
                CaptureSprite(StartPoint.X, StartPoint.Y, FinishPoint.X, FinishPoint.Y, false);
        }

        //автоматический захват спрайтов
        private void AutoCaptureButton_Click(object sender, EventArgs e)
        {
            if (img != null)
            {
                StartPoint.X = -1;
                StartPoint.Y = -1;

                //предварительный просмотр выбора спрайтов
                int[,] i = new int[img.Width + 4, img.Height + 4];
                for (int y = 0; y < img.Height; y++)
                    for (int x = 0; x < img.Width; x++)
                        i[x + 2, y + 2] = img.GetPixel(x, y).R == MainForm.ColorDisable.R && img.GetPixel(x, y).G == MainForm.ColorDisable.G && img.GetPixel(x, y).B == MainForm.ColorDisable.B ? 0 : 1;

                for (int y = 0; y < i.GetLength(1); y++)
                    for (int x = 0; x < i.GetLength(0); x++)
                        if ((i[x, y] & 1) == 1)
                        {
                            i[x + 1, y] |= 2;
                            i[x - 1, y] |= 2;
                            i[x, y + 1] |= 2;
                            i[x, y - 1] |= 2;
                        }

                My.FillArrayArea(i, 0, 0, -1);                    
                for (int y = 1; y < img.Height + 1; y++)
                    for (int x = 1; x < img.Width + 1; x++)
                        if (i[x, y] != -1)
                            i[x, y] = 0;
  
                Bitmap b = new Bitmap(img);
                Graphics g = Graphics.FromImage(b);
                Pen p = new Pen(Color.Red);
                List<Rectangle> l = new List<Rectangle>();
                for (int y = 0; y < i.GetLength(1); y++)
                    for (int x = 0; x < i.GetLength(0); x++)
                        if (i[x, y] == 0)
                        {
                            Rectangle r = My.FillArrayArea(i, x, y, 1);
                            r.X --;
                            r.Y --;
                            r.Width --;
                            r.Height --;
                            l.Add(r);
                            g.DrawRectangle(p, r);
                        }
                ImageBox.Image = b;

                //сохранение спрайтов
                if (MessageBox.Show("Capture this sprites?", "Capture", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    InputBox ib = new InputBox("Name prefix for sprites", SpriteName);
                    if (ib.ShowDialog() == DialogResult.OK)
                    {
                        SpriteName = ib.ResultString;
                        AutoNameNum = 0;
                        foreach (Rectangle r in l)
                        {
                            CaptureSprite(r.X, r.Y, r.X + r.Width, r.Y + r.Height, true);
                            AutoNameNum++;
                        }
                    }
                }
                ImageBox.Image = img;
            }
        }


        //процедура захвата спрайта
        private void CaptureSprite(int x1, int y1, int x2, int y2, bool autoname)
        {
            if (x1 > x2)
            {
                int i = x1;
                x1 = x2;
                x2 = i;
            }
            if (y1 > y2)
            {
                int i = y1;
                y1 = y2;
                y2 = i;
            }
            int xsz = x2 - x1 + 1;
            int ysz = y2 - y1 + 1;
            int[,] spr = new int[xsz + 10, ysz + 2];
            int j = 0;
            for (int y = y1; y <= y2; y++)
            {
                int i = 0;
                for (int x = x1; x <= x2; x++)
                {
                    spr[i, j] = img.GetPixel(x, y).R == MainForm.ColorDisable.R && img.GetPixel(x, y).G == MainForm.ColorDisable.G && img.GetPixel(x, y).B == MainForm.ColorDisable.B ? 0 : 1;
                    i++;
                }
                j++;
            }

            //перемещаем спрайт в левый верхний угол и проверяем наличие изображения
            bool e = false;
            for (int i = 0; i < xsz; i++)
            {
                for (int y = 0; y < ysz; y++)
                    if (spr[0, y] == 1)
                        e = true;
                if (e)
                    break;
                for (int y = 0; y < ysz; y++)
                    for (int x = 0; x < xsz - 1; x++)
                    {
                        spr[x, y] = spr[x + 1, y];
                        spr[x + 1, y] = 0;
                    }
            }

            //в спрайте нет пикселей
            if (!e)
            {
                MessageBox.Show("Sprite is empty.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            e = false;
            for (int i = 0; i < xsz; i++)
            {
                for (int x = 0; x < xsz; x++)
                    if (spr[x, 0] == 1)
                        e = true;
                if (e)
                    break;
                for (int y = 0; y < ysz - 1; y++)
                    for (int x = 0; x < xsz; x++)
                    {
                        spr[x, y] = spr[x, y + 1];
                        spr[x, y + 1] = 0;
                    }
            }

            //двигаем спрайт на один пиксель вправо и вниз
            for (int y = ysz - 2; y >= 0; y--)
                for (int x = xsz - 2; x >= 0; x--)
                {
                    spr[x + 1, y + 1] = spr[x, y];
                    spr[x, y] = 0;
                }
            
            //уточняем реальные размеры спрайта
            xsz += 10;
            ysz += 2;
            for (int y = ysz - 1; y >= 0; y--)
            {
                e = false;
                for (int x = 0; x < xsz; x++)
                {
                    if (spr[x, y] == 1)
                    {
                        e = true;
                    }
                }
                if (e)
                {
                    ysz = y + 2;
                    break;
                }
            }

            for (int x = xsz - 1; x >= 0; x--)
            {
                e = false;
                for (int y = 0; y < ysz; y++)
                    if (spr[x, y] == 1)
                        e = true;
                if (e)
                {
                    xsz = x + 2;
                    break;
                }
            }

            //создаем маску спрайта
            int[,] msk = new int[xsz + 2, ysz + 2];
            for (int y = 0; y < ysz; y++)
                for (int x = 0; x < xsz; x++)
                    if (spr[x, y] == 1)
                    {
                        msk[x + 1, y + 1] = 1;
                        msk[x + 1, y] = 1;
                        msk[x + 1, y + 2] = 1;
                        msk[x, y + 1] = 1;
                        msk[x + 2, y + 1] = 1;
                    }

            My.FillArrayArea(msk, 0, 0, 2);

            //сохранение спрайта
            MainForm.Sprite s = new MainForm.Sprite();
            if (!autoname)
            {
                InputBox i = new InputBox("Sprite name", SpriteName);
                if (i.ShowDialog() != DialogResult.OK)
                    return;
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the sprite can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (MainForm.FindSprite(i.ResultString) != null)
                {
                    MessageBox.Show("Sprite with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SpriteName = i.ResultString;
                s.Name = SpriteName;
            }
            //автоматическое именование спрайтов
            else
            {
                while (MainForm.FindSprite(SpriteName + IntToDecStr(AutoNameNum)) != null)
                    AutoNameNum++;
                s.Name = SpriteName + IntToDecStr(AutoNameNum);
            }

            s.SpriteSize = new Size(xsz, ysz);

            //монохромный спрайт с маской
            if (SpriteType0.Checked)
            {
                s.Type = 0;
                s.PixelLayer = new int[xsz, ysz];
                for (int y = 0; y < ysz; y++)
                    for (int x = 0; x < xsz; x++)
                    {
                        if (msk[x + 1, y + 1] != 2)
                            s.PixelLayer[x, y] = spr[x, y] == 0 ? 2 : 1;
                    }
            }
      
            MainForm main = this.Owner as MainForm;
            main.AddInSpriteList(s);
        }

        //функция преобразования числа в строку с нулями в начале
        private string IntToDecStr(int i)
        {
            string s = i.ToString();
            while (s.Length < 4)
                s = "0" + s;
            return s;
        }

        
    }
}
