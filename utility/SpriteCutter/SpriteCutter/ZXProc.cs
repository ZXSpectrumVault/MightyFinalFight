using System.Drawing;
using System.Collections.Generic;
using System;

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

        //фунция заливки области в двумерном массиве, возвращает прямоугольник, в который вписана закрашенная область
        static public Rectangle FillArrayArea(int[,] array, int xstart, int ystart, int value)
        {
            int zero_value = array[xstart, ystart];
            if (zero_value == value)
                return new Rectangle(xstart, ystart, 1, 1); ;
            List<Point> l = new List<Point>();
            Point p = new Point(xstart, ystart); 
            array[xstart, ystart] = value;
            int x1 = xstart;
            int y1 = ystart;
            int x2 = xstart;
            int y2 = ystart;
            l.Add(p);
            int n = 0;
            while (n < l.Count)
            {
                int x = l[n].X;
                int y = l[n].Y;
                if (x > 0)
                    if (array[x - 1, y] == zero_value)
                    {
                        l.Add(new Point(x - 1, y));
                        array[x - 1, y] = value;
                        x1 = Math.Min(x1, x - 1);
                    }
                if (x < array.GetLength(0) - 1)
                    if (array[x + 1, y] == zero_value)
                    {
                        l.Add(new Point(x + 1, y));
                        array[x + 1, y] = value;
                        x2 = Math.Max(x2, x + 1);
                    }
                if (y > 0)
                    if (array[x, y - 1] == zero_value)
                    {
                        l.Add(new Point(x, y - 1));
                        array[x, y - 1] = value;
                        y1 = Math.Min(y1, y - 1);
                    }
                if (y < array.GetLength(1) - 1)
                    if (array[x, y + 1] == zero_value)
                    {
                        l.Add(new Point(x, y + 1));
                        array[x, y + 1] = value;
                        y2 = Math.Max(y2, y + 1);
                    }
                n++;
            }
            return new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
        }
    }
}
