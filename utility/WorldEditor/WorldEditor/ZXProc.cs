using System.Drawing;

namespace ZXProc
{
    class ZX
    {
        //функция возвращает цвет, соответствующий цвету ZX Spectrum [0..15]
        static public Color GetColor(int zxcolor)
        {
            int bright = (zxcolor & 8) == 0 ? 192 : 255;
            int r = (zxcolor & 2) == 0 ? 0 : bright;
            int g = (zxcolor & 4) == 0 ? 0 : bright;
            int b = (zxcolor & 1) == 0 ? 0 : bright;
            return Color.FromArgb(r, g, b);
        }

        //рисование тайла
        static public Bitmap DrawTile(byte[] tile, int scale)
        {
            Bitmap img = new Bitmap(scale * 16, scale * 16);
            Graphics g = Graphics.FromImage(img);
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    byte attr = tile[32 + (x / 8) + (y / 8) * 2];
                    int bright = (attr & 64) == 0 ? 0 : 8;
                    Color ink = GetColor((attr & 7) + bright);
                    Color paper = GetColor(((attr >> 3) & 7) + bright);
                    Color pixel = ((tile[(x / 8) + y * 2] << (x % 8)) & 128) == 0 ? paper : ink;
                    Brush b = new SolidBrush(pixel);
                    g.FillRectangle(b, x * scale, y * scale, scale, scale);
                }
            return img;
        }
    }

    class My
    {
        //функция ограничивает значение переменной типа Int
        static public int LimitRange(int value, int min, int max)
        {
            if (value < min)
                value = min;
            if (value > max)
                value = max;
            return value;
        }

        //функция создает прямоугольник из координат
        static public Rectangle MakeRect(int x1, int y1, int x2, int y2)
        {
            Rectangle r = new Rectangle();
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                int temp = y1;
                y1 = y2;
                y2 = temp;
            }
            r.X = x1;
            r.Y = y1;
            r.Width = x2 - x1 + 1;
            r.Height = y2 - y1 + 1;
            return r;
        }

        //функция возвращает прямоугольник умноженный на значение
        static public Rectangle MulRect(Rectangle rect, int mul)
        {
            Rectangle r = new Rectangle();
            r.X = rect.X * mul;
            r.Y = rect.Y * mul;
            r.Width = rect.Width * mul - 1;
            r.Height = rect.Height * mul - 1;
            return r;
        }
    }
}
