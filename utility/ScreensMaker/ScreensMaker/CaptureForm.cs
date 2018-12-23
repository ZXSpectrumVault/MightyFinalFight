using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZXProc;
using System.Drawing.Drawing2D;

namespace ScreensMaker
{
    public partial class CaptureForm : Form
    {
        byte[,][] Spr8x8 = new byte[32, 24][];
        bool isScreenLoaded = false;
        Bitmap ScreenImg = new Bitmap(512, 384);
        int x_begin = -1;
        int y_begin;
        int x_end;
        int y_end;
        bool isSelectMode = false;
        string DefaultName = "";

        public CaptureForm()
        {
            InitializeComponent();
            Graphics g = Graphics.FromImage(ScreenImg);
            g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, 512, 384);
            ScreenFileImg.Image = ScreenImg;
        }

        private void ScreenFileNameButton_Click(object sender, EventArgs e)
        {
            if (OpenScreenFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                ScreenFileNameEdit.Text = OpenScreenFileDialog.FileName;

                BinaryReader scr_file = new BinaryReader(File.Open(OpenScreenFileDialog.FileName, FileMode.Open));
                byte[] scr_buffer = new byte[6912];
                for (int i = 0; i < 6912; i++)
                    scr_buffer[i] = scr_file.ReadByte();
                scr_file.Close();

                for (int j = 0; j < 24; j++)
                    for (int i = 0; i < 32; i++)
                    {
                        Spr8x8[i, j] = new byte[9];
                        byte attr = scr_buffer[6144 + i + j * 32];
                        if (attr == 0)
                            attr = 0x40;
                        Spr8x8[i, j][0] = attr;
                        for (int k = 0; k < 8; k++)
                            Spr8x8[i, j][k + 1] = scr_buffer[i + (j & 7) * 32 + k * 256 + (j & 24) * 256];
                    }

                Graphics g = Graphics.FromImage(ScreenImg);
                MainForm main = this.Owner as MainForm;
                for (int j = 0; j < 24; j++)
                    for (int i = 0; i < 32; i++)
                    {
                        main.DrawChar(g, i, j, Spr8x8[i, j], 0);
                    }
                ScreenFileImg.Image = ScreenImg;
                x_begin = -1;
                isScreenLoaded = true;
            }
        }

        private void HideForm()
        {
            x_begin = -1;
            ScreenFileImg.Image = ScreenImg;
            this.Hide();
        }

        private void ScreenFileCancelButton_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void DrawSelection(Graphics g, Pen p)
        {
            if (x_begin != -1)
            {
                int x1 = Math.Min(x_begin, x_end);
                int y1 = Math.Min(y_begin, y_end);
                int x2 = Math.Max(x_begin, x_end);
                int y2 = Math.Max(y_begin, y_end);
                g.DrawRectangle(p, x1 * 16, y1 * 16, (x2 - x1 + 1) * 16, (y2 - y1 + 1) * 16);
            }
        }

        private void ScreenFileImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isScreenLoaded)
                return;
            int x_cord = e.X / 16;
            int y_cord = e.Y / 16;
            if (x_cord < 0)
                x_cord = 0;
            if (x_cord > 31)
                x_cord = 31;
            if (y_cord < 0)
                y_cord = 0;
            if (y_cord > 23)
                y_cord = 23;

            if (isSelectMode)
            {
                x_end = x_cord;
                y_end = y_cord;
            }

            Bitmap img = new Bitmap(ScreenImg);
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.White, 2);
            p.DashStyle = DashStyle.Dash;

            DrawSelection(g, p);

            if (!isSelectMode)
            {
                g.DrawRectangle(p, x_cord * 16, y_cord * 16, 16, 16);
            }
            ScreenFileImg.Image = img;
        }

        private void ScreenFileImg_MouseLeave(object sender, EventArgs e)
        {
            if (!isScreenLoaded)
                return;
            Bitmap img = new Bitmap(ScreenImg);
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.White, 2);
            p.DashStyle = DashStyle.Dash;
            DrawSelection(g, p);
            ScreenFileImg.Image = img;
        }

        private void ScreenFileImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isScreenLoaded)
                return;
            x_begin = e.X / 16;
            y_begin = e.Y / 16;
            x_end = x_begin;
            y_end = y_begin;
            Bitmap img = new Bitmap(ScreenImg);
            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.White, 2);
            p.DashStyle = DashStyle.Dash;
            DrawSelection(g, p);
            ScreenFileImg.Image = img;
            isSelectMode = true;
        }

        private void ScreenFileImg_MouseUp(object sender, MouseEventArgs e)
        {
            isSelectMode = false;
        }

        private void ScreenFileCaptureButton_Click(object sender, EventArgs e)
        {
            if (x_begin == -1)
                return;
            MainForm main = this.Owner as MainForm;
            int x1 = Math.Min(x_begin, x_end);
            int y1 = Math.Min(y_begin, y_end);
            int x2 = Math.Max(x_begin, x_end);
            int y2 = Math.Max(y_begin, y_end);
            x_begin = -1;

            InputBox ib = new InputBox("Enter sprite name", DefaultName);
            ib.ShowDialog();
            if (ib.ResultString == "")
                return;

            MainForm.Sprite s = main.FindSprite(ib.ResultString);
            if (s != null)
            {
                if (MessageBox.Show("Sprite with the same name already exists! \nSave this sprite?", "WARRNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            else
            {
                main.Sprites.Add(new MainForm.Sprite());
                s = main.Sprites[main.Sprites.Count - 1];
            }

            DefaultName = ib.ResultString;
            s.name = DefaultName;
            s.width = x2 - x1 + 1;
            s.height = y2 - y1 + 1;
            s.lenght = 0;
            s.data = new byte[s.width, s.height][];
            for (int j = 0; j < s.height; j++)
                for (int i = 0; i < s.width; i++)
                {
                    s.data[i, j] = new byte[9];
                    byte attr = Spr8x8[x1 + i, y1 + j][0];
                    int size = 9;
                    int sum = 0;
                    for (int k = 1; k < 9; k++)
                    {
                        s.data[i, j][k] = Spr8x8[x1 + i, y1 + j][k];
                        sum += s.data[i, j][k];
                    }
                    if (sum == 0 && (attr & 0x38) == 0)
                    {
                        attr = 0;
                        size = 1;
                    }
                    else if (sum == 0)
                    {
                        attr = (byte)(attr | 0x80);
                        size = 1;
                    }
                    else if (sum == 2040)
                    {
                        byte ink = (byte)(attr & 0x07);
                        byte paper = (byte)((attr >> 3) & 0x07);
                        attr = (byte)(0x80 + paper + ink * 8 + (attr & 0x40));
                        size = 1;
                    }
                    s.data[i, j][0] = attr;
                    s.lenght += size;
                }
            main.CreateSpriteImg(s);
            main.RefreshSpritesList();
            main.FindInSpritesList(s);
            main.CalcProjectLenght();
            main.ShowScreenPreview();
            main.ProjectChanged = true;
        }
    }
}
