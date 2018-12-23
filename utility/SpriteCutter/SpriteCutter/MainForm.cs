using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ZXProc;

namespace SpriteCutter
{
    public partial class MainForm : Form
    {
        //класс инициализации
        public class Settings
        {
            public string WorkPath;
            public string BinPath;
            public string SourcePath;
            public int ColorEnable;
            public int ColorDisable;
            public int ColorMask;
            public int ColorSelection;
            public int ColorCollider;
        }


        //класс спрайта
        public class Sprite
        {
            public string Name; //имя спрайта
            public byte Type; //тип спрайта
            public Size SpriteSize; //размеры спрайта
            public int[,] PixelLayer; //слой пикселей
            public int[,] AttrLayer; //слой атрибутов
        }

        //класс композиции
        public class Composition
        {
            public Sprite ElemSprite; //элемент (спрайт) композиции
            public Point Offset; //смещение элемента (спрайта) композиции
            public bool View;//показывать элемент при редактировании
        }

        //класс кадра
        public class Frame
        {
            public List<Composition> Compositions; //список спрайтов в композиции
            public Rectangle DamageCollider; //коллайдер повреждений
            public Rectangle HitCollider; //коллайдер ударов
            public int Time; //время проигрывания кадра во фреймах
            public string Event; //название события
        }

        //класс анимации
        public class Animation
        {
            public string Name; //имя анимации
            public List<Frame> Frames; //список кадров
            public bool Loop; //анимация зациклена
        }


        static public string WorkPath = "/";
        static public string BinPath = "/";
        static public string SourcePath = "/";
        static public Color ColorEnable = Color.Black;
        static public Color ColorDisable = Color.White;
        static public Color ColorMask = Color.LawnGreen;
        static public Color ColorSelection = Color.Red;
        static public Color ColorCollider = Color.Blue;

        static public string ProjectVersion = "v1.0"; //версия проекта
        static public string ProjectName = @"C:\YandexDisk\Mighty Final Fight\res\sprites\mff.scproj"; //путь и имя проекта

        static public List<Sprite> Sprites = new List<Sprite>(); //список спрайтов
        static public List<Animation> Animations = new List<Animation>(); //список анимаций

        Bitmap PreviewSpriteImg = new Bitmap(128, 128);

        

        //-------------------------------------------- Общие --------------------------------------------

        int PageAdres = 0;//0xc000;

        class UsageSprite
        {
            public string Name;
            public int Adres;
            public int Page;
        }

		static public bool ProjectChanged = false; //изменения в проекте 
		
        //инициализация формы
        public MainForm()
        {
            InitializeComponent();

            //загрузка настроек
            if (File.Exists("setup.xml"))
            {
                using (Stream stream = new FileStream("setup.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings iniSet = (Settings)serializer.Deserialize(stream);
                    WorkPath = iniSet.WorkPath;
                    BinPath = iniSet.BinPath;
                    SourcePath = iniSet.SourcePath;
                    ColorEnable = Color.FromArgb(iniSet.ColorEnable);
                    ColorDisable = Color.FromArgb(iniSet.ColorDisable);
                    ColorMask = Color.FromArgb(iniSet.ColorMask);
                    ColorSelection = Color.FromArgb(iniSet.ColorSelection);
                    ColorCollider = Color.FromArgb(iniSet.ColorCollider);
                }
            }

            InitMonochromePalette();
            LoadProject();
        }

        //преобразование байта со знаком в int
        int SignByte(byte b)
        {
            return b < 128 ? b : -1 * (256 - b);
        }

        //функция возвращает младший байт int
        byte lbyte(int i)
        {
            return (byte)(i & 0xff);
        }

        //функция возвращает старший байт int
        byte hbyte(int i)
        {
            return (byte)((i >> 8) & 0xff);
        }

        //сохранение проекта
        bool SaveProject()
        {
            try
            {
                using (BinaryWriter proj = new BinaryWriter(File.Open(ProjectName, FileMode.Create, FileAccess.Write)))
                {
                    proj.Write(ProjectVersion);

                    //спрайты
                    proj.Write(Sprites.Count);
                    foreach (Sprite s in Sprites)
                    {
                        proj.Write(s.Name);
                        proj.Write(s.Type);
                        proj.Write(s.SpriteSize.Width);
                        proj.Write(s.SpriteSize.Height);

                        //монохромный спрайт с маской
                        if (s.Type == 0)
                        {                       
                            for (int y = 0; y < s.SpriteSize.Height; y++)
                                for (int x = 0; x < s.SpriteSize.Width; x++)
                                    proj.Write(s.PixelLayer[x, y]);
                        }
                    }

                    //анимации
                    proj.Write(Animations.Count);
                    foreach (Animation a in Animations)
                    {
                        proj.Write(a.Name);

                        //кадры
                        proj.Write(a.Frames.Count);
                        foreach (Frame f in a.Frames)
                        {

                            //композиции
                            proj.Write(f.Compositions.Count);
                            foreach (Composition c in f.Compositions)
                            {
                                proj.Write(c.ElemSprite.Name);
                                proj.Write(c.Offset.X);
                                proj.Write(c.Offset.Y);
                                proj.Write(c.View);
                            }
                            proj.Write(f.DamageCollider.X);
                            proj.Write(f.DamageCollider.Y);
                            proj.Write(f.DamageCollider.Width);
                            proj.Write(f.DamageCollider.Height);
                            proj.Write(f.HitCollider.X);
                            proj.Write(f.HitCollider.Y);
                            proj.Write(f.HitCollider.Width);
                            proj.Write(f.HitCollider.Height);
                            proj.Write(f.Time);
                            if (f.Event == null)
                                f.Event = "";
                            proj.Write(f.Event);
                        }
                        proj.Write(a.Loop);
                    }
                }
				ProjectChanged = false;
                return true;
            }
            catch
            {
                MessageBox.Show("Project not saved.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        bool LoadProject()
        {
            try
            {
                using (BinaryReader proj = new BinaryReader(File.Open(ProjectName, FileMode.Open)))
                {
                    ProjectVersion = proj.ReadString();

                    //спрайты
                    Sprites.Clear();
                    int spr_count = proj.ReadInt32();
                    for (int i = 0; i < spr_count; i++)
                    {
                        Sprite s = new Sprite();
                        s.Name = proj.ReadString();
                        s.Type = proj.ReadByte();
                        s.SpriteSize.Width = proj.ReadInt32();
                        s.SpriteSize.Height = proj.ReadInt32();

                        //монохромный спрайт с маской
                        if (s.Type == 0)
                        {
                            s.PixelLayer = new int[s.SpriteSize.Width, s.SpriteSize.Height];
                            for (int y = 0; y < s.SpriteSize.Height; y++)
                                for (int x = 0; x < s.SpriteSize.Width; x++)
                                    s.PixelLayer[x, y] = proj.ReadInt32();
                        }
                        Sprites.Add(s);
                    }

                    //анимации
                    Animations.Clear();
                    int anim_count = proj.ReadInt32();
                    for (int i = 0; i < anim_count; i++)
                    {
                        Animation a = new Animation();
                        a.Name = proj.ReadString(); 

                        //кадры
                        int frame_count = proj.ReadInt32();
                        a.Frames = new List<Frame>();
                        for (int j = 0; j < frame_count; j++)
                        {
                            Frame f = new Frame();

                            //композиции
                            int comp_count = proj.ReadInt32();
                            f.Compositions = new List<Composition>();
                            for (int k = 0; k < comp_count; k++)
                            {
                                Composition c = new Composition();
                                c.ElemSprite = FindSprite(proj.ReadString());
                                c.Offset.X = proj.ReadInt32();
                                c.Offset.Y = proj.ReadInt32();
                                c.View = proj.ReadBoolean();
                                f.Compositions.Add(c);
                            }
                            f.DamageCollider.X = proj.ReadInt32();
                            f.DamageCollider.Y = proj.ReadInt32();
                            f.DamageCollider.Width = proj.ReadInt32();
                            f.DamageCollider.Height = proj.ReadInt32();
                            f.HitCollider.X = proj.ReadInt32();
                            f.HitCollider.Y = proj.ReadInt32();
                            f.HitCollider.Width = proj.ReadInt32();
                            f.HitCollider.Height = proj.ReadInt32();
                            f.Time = proj.ReadInt32();
                            f.Event = proj.ReadString();
                            a.Frames.Add(f);
                        }
                        a.Loop = proj.ReadBoolean();
                        Animations.Add(a);
                    }
                }

                //вывод списков спрайтов и анимаций
                ShowSpriteList();
                ShowAnimList();
				ProjectChanged = false;
                return true;
            }
            catch
            {
                MessageBox.Show("Project not loaded.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //вызов сохранения проекта
        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveProject())
                MessageBox.Show("Project saved.", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

		//сохранение спрайта при закрытии формы
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!ProjectChanged)
                return;
			if (MessageBox.Show("Save changes?", "Project has been modified!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                return;
            SaveProject();
        }

        //форма настроек
        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupForm sf = new SetupForm();
            sf.ShowDialog();
        }

        //хот-кеи
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //выполнение undo
            if (e.Control && e.KeyCode == Keys.Z)
                UndoLayer();
            //выполнение redo
            if (e.Control && e.KeyCode == Keys.Y)
                RedoLayer();
            //выполнение сохранения
            if (e.Control && e.KeyCode == Keys.S)
                if (SaveProject())
                    MessageBox.Show("Project saved.", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //компиляция спрайтов и анимаций в бинарники
        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AnimationList.RowCount == 0)
                return;

            // InputBox ib = new InputBox("Animations name", AnimationFilter.Text);
            // if (ib.ShowDialog() == DialogResult.Cancel)
            // return;
            //BinaryWriter bin_file = new BinaryWriter(File.Open(BinPath + ib.ResultString + ".bin", FileMode.Create, FileAccess.Write));

            //находим имена всех персонажей
            List<string> pers_name = new List<string>();
            string last_name = "";
            foreach (Animation a in Animations)
            {
                string[] current_name = a.Name.Split('_');
                if (last_name != current_name[0])
                {
                    last_name = current_name[0];
                    pers_name.Add(last_name);
                }
            }

            //сохраняем аминации всех персонажей
            foreach (string pers in pers_name)
            {
                AnimationFilter.Text = pers;
                List<string> Events = new List<string>();
                BinaryWriter bin_file = new BinaryWriter(File.Open(BinPath + pers + ".bin", FileMode.Create, FileAccess.Write));
                //вычисляем адрес начала спрайтов, строим таблицу адресов анимаций и список событий
                int AnimAdres = PageAdres + AnimationList.RowCount * 2;

                //StreamWriter anim_file = new StreamWriter(File.Open(SourcePath + ib.ResultString + "_anim.asm", FileMode.Create, FileAccess.Write));
                //anim_file.WriteLine(";" + ib.ResultString + " animation names, generated automatically");
                //StreamWriter events_file = new StreamWriter(File.Open(SourcePath + ib.ResultString + "_event.asm", FileMode.Create, FileAccess.Write));
                //events_file.WriteLine(";" + ib.ResultString + " evenst, generated automatically");
                //events_file.WriteLine(ib.ResultString + "_events");

                StreamWriter anim_file = new StreamWriter(File.Open(SourcePath + pers + "_anim.asm", FileMode.Create, FileAccess.Write));          
                anim_file.WriteLine(";" + pers + " animation names, generated automatically");
                StreamWriter events_file = new StreamWriter(File.Open(SourcePath + pers + "_event.asm", FileMode.Create, FileAccess.Write));
                events_file.WriteLine(";" + pers + " evenst, generated automatically");
                events_file.WriteLine(pers + "_events");

                for (int i = 0; i < AnimationList.RowCount; i++)
                {
                    bin_file.Write((ushort)AnimAdres);
                    anim_file.WriteLine((string)AnimationList[0, i].Value + " = " + i.ToString());
                    Animation a = FindAnimation((string)AnimationList[0, i].Value);
                    foreach (Frame f in a.Frames)
                    {
                        //AnimAdres += 11 + f.Compositions.Count * 5;
                        AnimAdres += 3 + f.Compositions.Count * 5;
                        //проверяем, есть-ли событие в списке событий
                        if (f.Event != "" && f.Event != null)
                        {
                            bool isExists = false;
                            foreach (string s in Events)
                                if (s == f.Event)
                                {
                                    isExists = true;
                                    break;
                                }
                            //если нет, добавляем событие в список
                            if (!isExists)
                            {
                                Events.Add(f.Event);
                                events_file.WriteLine("          dw " + f.Event);
                            }
                        }
                    }
                    AnimAdres++;
                }
                //выравнивание на чётный адрес
                bool ShiftAdres = false;
                if ((AnimAdres & 1) != 0)
                {
                    AnimAdres++;
                    ShiftAdres = true;
                }
                int SpriteAdres = AnimAdres;
                int SpritePage = 0;
                anim_file.Close();
                events_file.Close();

                //строим список адресов и страниц для используемых спрайтов
                List<UsageSprite> UsageSprites = new List<UsageSprite>();
                for (int i = 0; i < AnimationList.RowCount; i++)
                {
                    Animation a = FindAnimation((string)AnimationList[0, i].Value);
                    foreach (Frame f in a.Frames)
                        foreach (Composition c in f.Compositions)
                        {
                            //проверяем, есть-ли уже этот спрайт в списке
                            bool isExists = false;
                            foreach (UsageSprite u in UsageSprites)
                                if (c.ElemSprite.Name == u.Name)
                                {
                                    isExists = true;
                                    break;
                                }
                            //если нет, то добавляем
                            if (!isExists)
                            {
                                int spr_len = 0;
                                //монохромный спрайт с маской
                                if (c.ElemSprite.Type == 0)
                                    spr_len = (c.ElemSprite.SpriteSize.Width + 7) / 8 * c.ElemSprite.SpriteSize.Height * 2 + 2;
                                //монохромный без маски
                                else if (c.ElemSprite.Type == 1)
                                    spr_len = (c.ElemSprite.SpriteSize.Width + 7) / 8 * c.ElemSprite.SpriteSize.Height + 2;
                                //с атрибутами zx с маской
                                else if (c.ElemSprite.Type == 1)
                                    spr_len = (c.ElemSprite.SpriteSize.Width + 7) / 8 * (c.ElemSprite.SpriteSize.Height + 7) / 8 * 17 + 2;
                                //if (SpriteAdres + spr_len > PageAdres + 0x1fff)
                                if (SpriteAdres + spr_len > PageAdres + 0x3fff)
                                {
                                    SpriteAdres = PageAdres;
                                    SpritePage++;
                                }
                                UsageSprite u = new UsageSprite();
                                u.Name = c.ElemSprite.Name;
                                u.Adres = SpriteAdres;
                                u.Page = SpritePage;
                                UsageSprites.Add(u);
                                SpriteAdres += spr_len;
                            }
                        }
                }

                //сохраняем анимации
                for (int i = 0; i < AnimationList.RowCount; i++)
                {
                    Animation a = FindAnimation((string)AnimationList[0, i].Value);
                    foreach (Frame f in a.Frames)
                    {
                        bin_file.Write((byte)f.Time);
                        //bin_file.Write((byte)(f.DamageCollider.X & 0xff));
                        //bin_file.Write((byte)(f.DamageCollider.Y & 0xff));
                        //bin_file.Write((byte)(f.DamageCollider.Width & 0xff));
                        //bin_file.Write((byte)(f.DamageCollider.Height & 0xff));
                        //bin_file.Write((byte)(f.HitCollider.X & 0xff));
                        //bin_file.Write((byte)(f.HitCollider.Y & 0xff));
                        //bin_file.Write((byte)(f.HitCollider.Width & 0xff));
                        //bin_file.Write((byte)(f.HitCollider.Height & 0xff));
                        byte j = 0;
                        if (f.Event == "")
                            bin_file.Write(j);
                        else
                        {
                            for (j = 0; j < Events.Count; j++)
                                if (Events[j] == f.Event)
                                    break;
                            bin_file.Write((byte)(j + 1));
                        }
                        bin_file.Write((byte)f.Compositions.Count);
                        foreach (Composition c in f.Compositions)
                        {
                            //ищем спрайт в списке используемых спрайтов
                            foreach (UsageSprite u in UsageSprites)
                                if (u.Name == c.ElemSprite.Name)
                                {
                                    bin_file.Write((byte)((u.Page & 0x0f) | ((c.ElemSprite.Type << 4) & 0xf0)));
                                    bin_file.Write((byte)(c.Offset.X & 0xff));
                                    bin_file.Write((byte)(c.Offset.Y & 0xff));
                                    bin_file.Write((ushort)u.Adres);
                                    break;
                                }
                        }
                    }
                    //добавляем конец или цикл анимации
                    if (a.Loop)
                        bin_file.Write((byte)255);
                    else
                        bin_file.Write((byte)0);
                }

                //выравнивание на чётный адрес
                if (ShiftAdres)
                    bin_file.Write((byte)0);

                //сохраняем спрайты
                int current_page = 0;
                int current_adres = AnimAdres;
                foreach (UsageSprite u in UsageSprites)
                {
                    if (current_page != u.Page)
                    {
                        AddOffsetBytetabInFile(current_adres, bin_file);
                        bin_file.Close();
                        current_page = u.Page;
                        //bin_file = new BinaryWriter(File.Open(BinPath + ib.ResultString + "_" + current_page + ".bin", FileMode.Create, FileAccess.Write));
                        bin_file = new BinaryWriter(File.Open(BinPath + pers + "_" + current_page + ".bin", FileMode.Create, FileAccess.Write));
                        current_adres = PageAdres;
                    }

                    Sprite s = FindSprite(u.Name);
                    bin_file.Write((byte)s.SpriteSize.Width);
                    bin_file.Write((byte)s.SpriteSize.Height);
                    current_adres += 2;

                    //монохромный спрайт с маской
                    if (s.Type == 0)
                    {
                        for (int j = 0; j < s.SpriteSize.Height; j++)
                            for (int i = 0; i < (s.SpriteSize.Width + 7) / 8; i++)
                            {
                                byte m = 0;
                                byte b = 0;                           
                                for (int k = 0; k < 8; k++)
                                    if (i * 8 + k < s.SpriteSize.Width)
                                    {
                                        if (s.PixelLayer[i * 8 + k, j] == 1)
                                            m = (byte)(m | (128 >> k));
                                        else if (s.PixelLayer[i * 8 + k, j] == 2)
                                        {
                                            m = (byte)(m | (128 >> k));
                                            b = (byte)(b | (128 >> k));
                                        }
                                    }
                                bin_file.Write(m);
                                bin_file.Write(b);
                                current_adres += 2;
                            }
                    }
                }
                //AddOffsetBytetabInFile(current_adres, bin_file);
                bin_file.Close(); 
            }
            AnimationFilter.Text = "";
            SaveProject();
            MessageBox.Show("Project compiled & saved.", "Compiled.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //добавление в файл таблицы сдвигов байта
        void AddOffsetBytetabInFile(int current_adr, BinaryWriter file)
        {
            //доводим размер файла до 8Кб и записываем таблицу сдвигов байта
            while (current_adr < 0xe000)
            {
                file.Write((byte)0);
                current_adr++;
            }
            //прямой байт
            for (int s = 0; s < 8; s++)
            {
                for (int b = 0; b < 256; b++)
                    file.Write((byte)((b >> s) & 0xff));
                for (int b = 0; b < 256; b++)
                    file.Write((byte)(((b * 256) >> s) & 0xff));
            }
            //зеркальный байт
            for (int s = 0; s < 8; s++)
            {
                for (int b = 0; b < 256; b++)
                {
                    int m = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        m = m * 2;
                        if (((1 << i) & b) != 0)
                            m++;
                    }
                    file.Write((byte)(((m * 256) >> s) & 0xff));
                }
                for (int b = 0; b < 256; b++)
                {
                    int m = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        m = m * 2;
                        if (((1 << i) & b) != 0)
                            m++;
                    }
                    file.Write((byte)((m >> s) & 0xff));
                }
            }
        }

        //------------------------------------------- Спрайты -------------------------------------------

        string PrevSpriteName = ""; //предыдущее имя анимации
        Bitmap SpriteImg;

        //показать список спрайтов
        public void ShowSpriteList()
        {
            int i = -1;
            if (SpriteList.CurrentRow != null)
                i = SpriteList.CurrentRow.Index;
            SpriteList.Rows.Clear();
            foreach (Sprite s in Sprites)
                if (s.Name.Contains(SpriteFilter.Text) || SpriteFilter.Text == "")
                    SpriteList.Rows.Add(s.Name, s.SpriteSize.Width.ToString() + " x " + s.SpriteSize.Height.ToString());
            if (i >= SpriteList.RowCount)
                i = SpriteList.RowCount - 1;
            if (i >= 0)
            {
                DataGridViewCell cell = SpriteList.Rows[i].Cells[0];
                SpriteList.CurrentCell = cell;
                SpriteList.CurrentCell.Selected = true;
            }
            ShowSpritePreview();
        }

        //вывод превью спрайта
        void ShowSpritePreview()
        {
            if (SpriteList.CurrentRow == null)
            {
                SprView.Image = null;
                return;
            }

            //подсветка строк
            for (int y = 0; y < SpriteList.Rows.Count; y++)
                for (int x = 0; x < SpriteList.Columns.Count; x++)
                    if (y == SpriteList.CurrentRow.Index)
                        SpriteList[x, y].Style.BackColor = Color.Cyan;
                    else
                    {
                        Sprite spr = FindSprite((string)SpriteList[0, y].Value);
                        if (spr.SpriteSize.Width > 64 || spr.SpriteSize.Height > 64)
                            SpriteList[x, y].Style.BackColor = Color.Yellow;
                        else
                            SpriteList[x, y].Style.BackColor = Color.White;
                    }

            Graphics g = Graphics.FromImage(PreviewSpriteImg);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.FillRectangle(new SolidBrush(ColorMask), 0, 0, 128, 128);
            Sprite s = FindSprite((string)(SpriteList[0, SpriteList.CurrentRow.Index].Value));
            int xsz = s.SpriteSize.Width;
            int ysz = s.SpriteSize.Height;
            SpriteImg = new Bitmap(xsz, ysz);
            int xoffset = 64 - xsz;
            int yoffset = 64 - ysz;

            //монохромный спрайт с маской
            if (s.Type == 0)
            {
                for (int y = 0; y < ysz; y++)
                    for (int x = 0; x < xsz; x++)
                    {
                        Color c = Color.FromArgb(0, 0, 0, 0);
                        if (s.PixelLayer[x, y] == 1)
                            c = ColorEnable;
                        if (s.PixelLayer[x, y] == 2)
                            c = ColorDisable;
                        SpriteImg.SetPixel(x, y, c);
                    }
            }

            g.DrawImage(SpriteImg, xoffset, yoffset, xsz * 2, ysz * 2);
            SprView.Image = PreviewSpriteImg;         
        }

        //поиск спрайта по имени
        public static Sprite FindSprite(string name)
        {
            foreach (Sprite s in Sprites)
                if (s.Name == name)
                    return s;
            return null;
        }

        //изменение фильтра спрайтов
        void SpriteFilter_TextChanged(object sender, EventArgs e)
        {
            ShowSpriteList();
            SpriteButtonUp.Enabled = SpriteFilter.Text == "";
            SpriteButtonDown.Enabled = SpriteFilter.Text == "";
        }

        //очистка фильтра спрайтов
        void ClearFilter_Click(object sender, EventArgs e)
        {
            SpriteFilter.Text = "";
            SpriteFilter.Focus();
        }

        //выбор спрайта
        private void SpriteList_CurrentCellChanged(object sender, EventArgs e)
        {
            ShowSpritePreview();
        }

        //перемещение спрайта вверх по списку
        private void SpriteButtonUp_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;
            int i = SpriteList.CurrentRow.Index;
            if (i == 0)
                return;
            Sprite spr = FindSprite((string)SpriteList[0, i].Value);
            Sprites.Remove(spr);
            Sprites.Insert(i - 1, spr);
            DataGridViewCell cell = SpriteList.Rows[i - 1].Cells[0];
            SpriteList.CurrentCell = cell;
            SpriteList.CurrentCell.Selected = true;
            ShowSpriteList();
			ProjectChanged = true;
        }

        //перемещение спрайта вниз по списку
        private void SpriteButtonDown_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;
            int i = SpriteList.CurrentRow.Index;
            if (i >= SpriteList.RowCount - 1)
                return;
            Sprite spr = FindSprite((string)SpriteList[0, i].Value);
            Sprites.Remove(spr);
            Sprites.Insert(i + 1, spr);
            DataGridViewCell cell = SpriteList.Rows[i + 1].Cells[0];
            SpriteList.CurrentCell = cell;
            SpriteList.CurrentCell.Selected = true;
            ShowSpriteList();
			ProjectChanged = true;
        }

        //захват спрайта
        private void CaptureButton_Click(object sender, EventArgs e)
        {
            //проверяем чтобы форма была уже не открыта
            foreach (Form f in Application.OpenForms)
            {
                if (f is CaptureForm)
                {
                    return;
                }
            }
            CaptureForm cf = new CaptureForm();
            cf.Owner = this;
            cf.Show();
        }

        //экспорт спрайта
        private void ExportSprButtom_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;
            ExportSpriteDialog.FileName = (string)SpriteList[0, SpriteList.CurrentRow.Index].Value;
            if (ExportSpriteDialog.ShowDialog() == DialogResult.OK)
            {
                SpriteImg.Save(ExportSpriteDialog.FileName);
            }
        }

        //создать новый спрайт
        private void NewSpriteButton_Click(object sender, EventArgs e)
        {
            if (PrevSpriteName == "" && SpriteList.CurrentRow != null)
                PrevSpriteName = (string)SpriteList[0, SpriteList.CurrentRow.Index].Value;
            SpriteInputBox i = new SpriteInputBox(PrevSpriteName);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the sprite can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindSprite(i.ResultString) != null)
                {
                    MessageBox.Show("Sprite with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Sprite spr = new Sprite();
                spr.Name = i.ResultString;
                spr.Type = i.ResultType;
                
                //монохромный спрайт с маской
                if (spr.Type == 0)
                {
                    spr.SpriteSize = new Size(1, 1);
                    spr.PixelLayer = new int[1, 1];
                }

                AddInSpriteList(spr);    
            }          
        }

        //удаление спрайта
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;

            //проверяем, не используется ли этот спрайт в анимациях
            Sprite spr = FindSprite((string)SpriteList[0, SpriteList.CurrentRow.Index].Value);
            foreach (Animation a in Animations)
                foreach (Frame f in a.Frames)
                    foreach (Composition c in f.Compositions)
                        if (c.ElemSprite == spr)
                        {
                            if (!AnimationFilter.Text.Contains(a.Name) || AnimationFilter.Text != "")
                                AnimationFilter.Text = "";
                            ShowAnimList();
                            for (int i = 0; i < AnimationList.RowCount; i++)
                                if ((string)AnimationList[0, i].Value == a.Name)
                                {
                                    DataGridViewCell cell = AnimationList.Rows[i].Cells[0];
                                    AnimationList.CurrentCell = cell;
                                    AnimationList.CurrentCell.Selected = true;
                                    break;
                                }
                            ShowFrameList();
                            for (int i = 0; i < FrameList.RowCount; i++)
                            {
                                DataGridViewCell cell = FrameList.Rows[i].Cells[0];
                                FrameList.CurrentCell = cell;
                                FrameList.CurrentCell.Selected = true;
                                ShowCompositionList();
                                for (int j = 0; j < CompositionList.RowCount; j++)
                                    if ((string)CompositionList[0, j].Value == c.ElemSprite.Name)
                                    {
                                        DataGridViewCell cell2 = CompositionList.Rows[j].Cells[0];
                                        CompositionList.CurrentCell = cell2;
                                        CompositionList.CurrentCell.Selected = true;
                                        MessageBox.Show("This sprite is used in some composition!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                            }
                        }

            if (MessageBox.Show("Are you sure?", "Delete sprite!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            
            Sprites.Remove(spr);
            ShowSpriteList();
			ProjectChanged = true;
        }

        //переименование спрайта
        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;
            Sprite spr = FindSprite((string)SpriteList[0, SpriteList.CurrentRow.Index].Value);
            InputBox i = new InputBox("New sprite name", spr.Name);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the sprite can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindSprite(i.ResultString) != null)
                {
                    MessageBox.Show("Sprite with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string old_name = spr.Name;
                spr.Name = i.ResultString;

                //удаление фильтра, если новое имя не входит в него
                if (!spr.Name.Contains(SpriteFilter.Text))
                    SpriteFilter.Text = "";

                //переименование наименования спрайта в списке композиций
                ShowSpriteList();
                if (CompositionList.RowCount > 0)
                {
                    for (int j = 0; j < CompositionList.RowCount; j++)
                        if ((string)CompositionList[0, j].Value == old_name)
                            CompositionList[0, j].Value = spr.Name;
                }
				ProjectChanged = true;
            }
        }

        //копирование спрайта
        private void CopySpriteButton_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;
            Sprite spr = FindSprite((string)SpriteList[0, SpriteList.CurrentRow.Index].Value);
            InputBox i = new InputBox("Sprite name", spr.Name);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the sprite can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindSprite(i.ResultString) != null)
                {
                    MessageBox.Show("Sprite with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //клон спрайта
                Sprite new_spr = new Sprite();
                new_spr.Name = i.ResultString;
                new_spr.Type = spr.Type;
                new_spr.SpriteSize = new Size(spr.SpriteSize.Width, spr.SpriteSize.Height);

                //монохромный спрайт с маской
                if (new_spr.Type == 0)
                {
                    new_spr.PixelLayer = new int[spr.SpriteSize.Width, spr.SpriteSize.Height];
                    for (int y = 0; y < spr.SpriteSize.Height; y++)
                        for (int x = 0; x < spr.SpriteSize.Width; x++)
                            new_spr.PixelLayer[x, y] = spr.PixelLayer[x, y];
                }
                AddInSpriteList(new_spr);
            }
        }

        //добавления нового спрайта в список спрайтов
        public void AddInSpriteList(Sprite spr)
        {
            Sprites.Add(spr);
            PrevSpriteName = spr.Name;

            //удаление фильтра, если новое имя не входит в него
            if (!spr.Name.Contains(SpriteFilter.Text))
                SpriteFilter.Text = "";

            ShowSpriteList();
            DataGridViewCell cell = SpriteList.Rows[SpriteList.RowCount - 1].Cells[0];
            SpriteList.CurrentCell = cell;
            SpriteList.CurrentCell.Selected = true;
			ProjectChanged = true;
        }

        //добавление нового кадра и композиции с выбранным спрайтом
        private void SpriteList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AddFrame())
            {
                AddCompositionSprite();
            }
        }

        //создать спрайт тени
        private void MakeShadowButton_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;

            Sprite spr = FindSprite((string)SpriteList[0, SpriteList.CurrentRow.Index].Value);
            //тени строим только для монохромного спрайта с маской
            if (spr.Type == 0)
            {
                Sprite shadow = new Sprite();
                int[,] shlayer = new int[spr.SpriteSize.Width, (spr.SpriteSize.Height + 7) / 8];
                for (int y = 0; y < spr.SpriteSize.Height; y++)
                    for (int x = 0; x < spr.SpriteSize.Width; x++)
                        if (spr.PixelLayer[x, y] == 1)
                            shlayer[x, y / 8] = 1;
                shadow.PixelLayer = shlayer;
                shadow.Name = spr.Name + "_shadow";
                shadow.Type = 0;
                shadow.SpriteSize.Width = spr.SpriteSize.Width;
                shadow.SpriteSize.Height = (spr.SpriteSize.Height + 7) / 8;
                int index = 0;
                foreach (Sprite s in Sprites)
                {
                    if (s.Name == spr.Name)
                        break;
                    index++;
                }
                Sprites.Insert(index, shadow);
                ProjectChanged = true;
            }
            ShowSpriteList();         
        }

        //добавляем тень к спрайту
        private void AddShadowButton_Click(object sender, EventArgs e)
        {
            if (SpriteList.CurrentRow == null)
                return;

            if (MessageBox.Show("Are you sure?", "Add shadow in sprite!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            Sprite spr = FindSprite((string)SpriteList[0, SpriteList.CurrentRow.Index].Value);
            //тени строим только для монохромного спрайта с маской
            if (spr.Type == 0)
            {
                int[,] newlayer = new int[spr.SpriteSize.Width, spr.SpriteSize.Height];
                for (int y = 0; y < spr.SpriteSize.Height; y++)
                    for (int x = 0; x < spr.SpriteSize.Width; x++)
                        if (spr.PixelLayer[x, y] == 1)
                            newlayer[x, spr.SpriteSize.Height - spr.SpriteSize.Height / 8 + y / 8] = 1;
                for (int y = 0; y < spr.SpriteSize.Height; y++)
                    for (int x = 0; x < spr.SpriteSize.Width; x++)
                        if (spr.PixelLayer[x, y] != 0)
                            newlayer[x, y] = spr.PixelLayer[x, y];
                spr.PixelLayer = newlayer;
                ProjectChanged = true;
            }
            ShowSpriteList();
        }

        string old_spr_name;

        //начало ручного редактирования списка спрайтов
        private void SpriteList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            old_spr_name = (string)SpriteList[0, e.RowIndex].Value;
        }

        //завершение ручного редактирования списка спрайтов
        private void SpriteList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string new_spr_name = (string)SpriteList[0, e.RowIndex].Value;

            if (new_spr_name == null)
            {
                MessageBox.Show("The name of the sprite can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpriteList[0, e.RowIndex].Value = old_spr_name;
                return;
            }

            Sprite s = FindSprite(old_spr_name);
            if (old_spr_name != new_spr_name)
            {
                if (FindSprite(new_spr_name) != null)
                {
                    MessageBox.Show("Sprite with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SpriteList[0, e.RowIndex].Value = old_spr_name;
                    return;
                }
                s.Name = new_spr_name;

                //удаление фильтра, если новое имя не входит в него
                if (!new_spr_name.Contains(SpriteFilter.Text))
                {
                    SpriteFilter.Text = "";
                    ShowSpriteList();
                }

                //переименование наименования спрайта в списке композиций             
                if (CompositionList.RowCount > 0)
                {
                    for (int j = 0; j < CompositionList.RowCount; j++)
                        if ((string)CompositionList[0, j].Value == old_spr_name)
                            CompositionList[0, j].Value = new_spr_name;
                }

                ProjectChanged = true;
            }
        }

        //------------------------------------------ Анимация -------------------------------------------

        string PrevAnimName = ""; //предыдущее имя анимации
        int CurrentFrameIndex = 0; //индекс текущего кадра анимации при её воспроизведении

        //показать список анимаций
        void ShowAnimList()
        {
            int i = -1;
            if (AnimationList.CurrentRow != null)
                i = AnimationList.CurrentRow.Index;
            AnimationList.Rows.Clear();
            foreach (Animation a in Animations)
                if (a.Name.Contains(AnimationFilter.Text) || AnimationFilter.Text == "")
                    AnimationList.Rows.Add(a.Name, a.Loop);
            if (i >= AnimationList.RowCount)
                i = AnimationList.RowCount - 1;
            if (i >= 0)
            {
                DataGridViewCell cell = AnimationList.Rows[i].Cells[0];
                AnimationList.CurrentCell = cell;
                AnimationList.CurrentCell.Selected = true;
            }
        }

        //поиск анимации по имени
        Animation FindAnimation(string name)
        {
            foreach (Animation anim in Animations)
                if (anim.Name == name)
                    return anim;
            return null;
        }

        //новая анимация
        void NewAnimButton_Click(object sender, EventArgs e)
        {
            if (PrevAnimName == "" && AnimationList.CurrentRow != null)
                PrevAnimName = (string)AnimationList[0, AnimationList.CurrentRow.Index].Value;
            InputBox i = new InputBox("Animation Name", PrevAnimName);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the animation can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindAnimation(i.ResultString) != null)
                {
                    MessageBox.Show("Animation with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                PrevAnimName = i.ResultString;
                Animation anim = new Animation();
                anim.Name = i.ResultString;
                anim.Frames = new List<Frame>();
                anim.Loop = false;
                Animations.Add(anim);

                //удаление фильтра, если новое имя не входит в него
                if (!anim.Name.Contains(AnimationFilter.Text))
                    AnimationFilter.Text = "";

                ShowAnimList();
                DataGridViewCell cell = AnimationList.Rows[AnimationList.RowCount - 1].Cells[0];
                AnimationList.CurrentCell = cell;
                AnimationList.CurrentCell.Selected = true;
				ProjectChanged = true;
            }
        }

        string old_anim_name;

        //начало ручного редактирования списка анимаций
        private void AnimationList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            old_anim_name = (string)AnimationList[0, e.RowIndex].Value;
        }

        //завершение ручного редактирования списка анимаций
        void AnimationList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string new_anim_name = (string)AnimationList[0, e.RowIndex].Value;

            if (new_anim_name == null)
            {
                MessageBox.Show("The name of the animation can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AnimationList[0, e.RowIndex].Value = old_anim_name;
                return;
            }

            Animation a = FindAnimation(old_anim_name);
            if (old_anim_name != new_anim_name)
            {
                if (FindAnimation(new_anim_name) != null)
                {
                    MessageBox.Show("Animation with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AnimationList[0, e.RowIndex].Value = old_anim_name;
                    return;
                }
                a.Name = new_anim_name;

                //удаление фильтра, если новое имя не входит в него
                if (!new_anim_name.Contains(AnimationFilter.Text))
                {
                    AnimationFilter.Text = "";
                    ShowAnimList();
                }
                ProjectChanged = true;
            }

            if (e.ColumnIndex == 1)
            {
                a.Loop = (bool)AnimationList[1, e.RowIndex].Value;
				ProjectChanged = true;
            }
        }

        //выбор анимации
        private void AnimationList_CurrentCellChanged(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow != null)
            {
                for (int j = 0; j < AnimationList.Rows.Count; j++)
                    for (int i = 0; i < AnimationList.Columns.Count; i++)
                        if (j == AnimationList.CurrentRow.Index)
                            AnimationList[i, j].Style.BackColor = Color.Cyan;
                        else
                            AnimationList[i, j].Style.BackColor = Color.White;
            }
            ShowFrameList();
        }

        //перемещение анимации вверх по списку
        void AnimUpButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            int i = AnimationList.CurrentRow.Index;
            if (i == 0)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, i].Value);
            Animations.Remove(anim);
            Animations.Insert(i - 1, anim);
            DataGridViewCell cell = AnimationList.Rows[i - 1].Cells[0];
            AnimationList.CurrentCell = cell;
            AnimationList.CurrentCell.Selected = true;
            ShowAnimList();
			ProjectChanged = true;
        }

        //перемещение анимации вниз по списку
        void AnimDownButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            int i = AnimationList.CurrentRow.Index;
            if (i >= AnimationList.RowCount - 1)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, i].Value);
            Animations.Remove(anim);
            Animations.Insert(i + 1, anim);
            DataGridViewCell cell = AnimationList.Rows[i + 1].Cells[0];
            AnimationList.CurrentCell = cell;
            AnimationList.CurrentCell.Selected = true;
            ShowAnimList();
			ProjectChanged = true;
        }

        //изменение фильтра анимаций
        void AnimationFilter_TextChanged(object sender, EventArgs e)
        {
            ShowAnimList();
            AnimUpButton.Enabled = AnimationFilter.Text == "";
            AnimDownButton.Enabled = AnimationFilter.Text == "";
        }

        //очистка фильтра анимаций
        void ClearAnimationFilter_Click(object sender, EventArgs e)
        {
            AnimationFilter.Text = "";
            AnimationFilter.Focus();
        }

        //удаление анимации
        void DelAnimButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            if (MessageBox.Show("Are you sure?", "Delete animation!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Animations.Remove(anim);
            ShowAnimList();
			ProjectChanged = true;
        }

        //переименование анимации
        void RenameAnimButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            InputBox i = new InputBox("New animation name", anim.Name);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the animation can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindAnimation(i.ResultString) != null)
                {
                    MessageBox.Show("Animation with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                anim.Name = i.ResultString;

                //удаление фильтра, если новое имя не входит в него
                if (!anim.Name.Contains(AnimationFilter.Text))
                    AnimationFilter.Text = "";

                ShowAnimList();
				ProjectChanged = true;
            }
        }

        //копирование анимации
        void CopyAnimButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            InputBox i = new InputBox("Animation name", anim.Name);
            if (i.ShowDialog() == DialogResult.OK)
            {
                if (i.ResultString == "")
                {
                    MessageBox.Show("The name of the animation can't be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (FindAnimation(i.ResultString) != null)
                {
                    MessageBox.Show("Animation with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //клон анимации
                Animation new_anim = new Animation();
                new_anim.Name = i.ResultString;
                new_anim.Frames = new List<Frame>();
                foreach (Frame f in anim.Frames)
                {
                    Frame f_new = new Frame();
                    List<Composition> new_compositions = new List<Composition>();
                    foreach (Composition c in f.Compositions)
                    {
                        Composition c_new = new Composition();
                        c_new.ElemSprite = c.ElemSprite;
                        c_new.Offset = new Point(c.Offset.X, c.Offset.Y);
                        c_new.View = c.View;
                        new_compositions.Add(c_new);
                    }
                    f_new.Compositions = new_compositions;
                    Rectangle dcldr = new Rectangle(f.DamageCollider.X, f.DamageCollider.Y, f.DamageCollider.Width, f.DamageCollider.Height);
                    f_new.DamageCollider = dcldr;
                    Rectangle hcldr = new Rectangle(f.HitCollider.X, f.HitCollider.Y, f.HitCollider.Width, f.HitCollider.Height);
                    f_new.HitCollider = hcldr;
                    f_new.Time = f.Time;
                    f_new.Event = f.Event;
                    new_anim.Frames.Add(f_new);
                }
                new_anim.Loop = anim.Loop;   
                Animations.Add(new_anim);
				
				//удаление фильтра, если новое имя не входит в него
                if (!anim.Name.Contains(AnimationFilter.Text))
                    AnimationFilter.Text = "";
				
                ShowAnimList();
                DataGridViewCell cell = AnimationList.Rows[AnimationList.RowCount - 1].Cells[0];
                AnimationList.CurrentCell = cell;
                AnimationList.CurrentCell.Selected = true;
				ProjectChanged = true;
            }
        }

        //воспроизведение анимации
        private void PlayAnimationCheck_CheckedChanged(object sender, EventArgs e)
        {
            if ((PlayAnimationCheck.Checked && FrameList.RowCount == 0) || AnimationList.RowCount == 0)
            {
                PlayAnimationCheck.Checked = false;
                return;
            }

            foreach (Control c in this.Controls)
            {
                c.Enabled = !PlayAnimationCheck.Checked;
            }
            PlayAnimationCheck.Enabled = true;
            AnimationTimer.Interval = 100;
            AnimationTimer.Enabled = PlayAnimationCheck.Checked;
            if (!PlayAnimationCheck.Checked)
            {
                CurrentFrameIndex = 0;
                ShowFrameList();
            }
            else
            {
                DataGridViewCell cell = FrameList.Rows[0].Cells[0];
                FrameList.CurrentCell = cell;
                FrameList.CurrentCell.Selected = true;
            }
        }

        //таймер анимации
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            AnimationTimer.Enabled = false;
            if (CurrentFrameIndex == FrameList.RowCount)
            {
                CurrentFrameIndex = 0;
                if (!FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Loop)
                {
                    PlayAnimationCheck.Checked = false;
                    return;
                }
            }
            Frame f = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[CurrentFrameIndex];
            int scale = ScaleBar.Value; 
            Graphics g = Graphics.FromImage(CompositionImg);
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            Brush bEnable = new SolidBrush(ColorEnable);
            Brush bDisable = new SolidBrush(ColorDisable);
            g.FillRectangle(new SolidBrush(ColorMask), 0, 0, 128 * scale, 128 * scale);

            foreach (Composition c in f.Compositions)
            {
                //монохромный спрайт с маской
                if (c.ElemSprite.Type == 0)
                {
                    for (int y = 0; y < c.ElemSprite.SpriteSize.Height; y++)
                        for (int x = 0; x < c.ElemSprite.SpriteSize.Width; x++)
                            if (x + c.Offset.X + 64 >= 0 && x + c.Offset.X + 64 < 128 && y + c.Offset.Y + 96 >= 0 && y + c.Offset.Y + 96 < 128)
                                if (c.ElemSprite.PixelLayer[x, y] != 0)
                                {
                                    Brush b = c.ElemSprite.PixelLayer[x, y] == 1 ? bEnable : bDisable;
                                    g.FillRectangle(b, (x + c.Offset.X + 64) * scale, (y + c.Offset.Y + 96) * scale, scale, scale);
                                }
                }
            }

            CompositionView.Image = CompositionImg;
            CurrentFrameIndex++;
            AnimationTimer.Interval = 20 * f.Time;
            AnimationTimer.Enabled = true;
        }

        //-------------------------------------------- Кадры --------------------------------------------

        //показать список кадров
        void ShowFrameList()
        {
            int i = -1;
            if (FrameList.CurrentRow != null)
                i = FrameList.CurrentRow.Index;
            FrameList.Rows.Clear();
            if (AnimationList.CurrentRow == null)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            if (anim == null)
                return;
            foreach (Frame f in anim.Frames)
                FrameList.Rows.Add(f.DamageCollider.X.ToString(), f.DamageCollider.Y.ToString(), f.DamageCollider.Width.ToString(), f.DamageCollider.Height.ToString(), f.HitCollider.X.ToString(), f.HitCollider.Y.ToString(), f.HitCollider.Width.ToString(), f.HitCollider.Height.ToString(), f.Time.ToString(), f.Event);
            if (i >= FrameList.RowCount)
                i = FrameList.RowCount - 1;
            if (i >= 0)
            {
                DataGridViewCell cell = FrameList.Rows[i].Cells[0];
                FrameList.CurrentCell = cell;
                FrameList.CurrentCell.Selected = true;
            }
        }

        //добавление нового кадра
        bool AddFrame()
        {
            if (AnimationList.CurrentRow == null)
            {
                MessageBox.Show("Select animation!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Frame f = new Frame();
            f.Compositions = new List<Composition>();
            f.DamageCollider = new Rectangle();
            f.HitCollider = new Rectangle();
            f.Time = 5;
            anim.Frames.Add(f);
            ShowFrameList();
            DataGridViewCell cell = FrameList.Rows[FrameList.RowCount - 1].Cells[0];
            FrameList.CurrentCell = cell;
            FrameList.CurrentCell.Selected = true;
            ProjectChanged = true;
            return true;
        }


        //новый кадр
        void NewFrameButton_Click(object sender, EventArgs e)
        {
            AddFrame();
        }

        //удаление кадра
        void DelFrameButton_Click(object sender, EventArgs e)
        {
            if (AnimationList.CurrentRow == null)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            if (anim.Frames.Count == 0)
                return;
            if (MessageBox.Show("Are you sure?", "Delete Frame!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            anim.Frames.RemoveAt(FrameList.CurrentRow.Index);
            ShowFrameList();
			ProjectChanged = true;
        }

        //ручное редактирование списка кадров
        void FrameList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Frame frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index];
            try
            {

                frame.DamageCollider.X = Int32.Parse((string)(FrameList[0, e.RowIndex].Value));
                frame.DamageCollider.Y = Int32.Parse((string)(FrameList[1, e.RowIndex].Value));
                frame.DamageCollider.Width = Int32.Parse((string)(FrameList[2, e.RowIndex].Value));
                frame.DamageCollider.Height = Int32.Parse((string)(FrameList[3, e.RowIndex].Value));
                frame.HitCollider.X = Int32.Parse((string)(FrameList[4, e.RowIndex].Value));
                frame.HitCollider.Y = Int32.Parse((string)(FrameList[5, e.RowIndex].Value));
                frame.HitCollider.Width = Int32.Parse((string)(FrameList[6, e.RowIndex].Value));
                frame.HitCollider.Height = Int32.Parse((string)(FrameList[7, e.RowIndex].Value));
                frame.Time = Int32.Parse((string)(FrameList[8, e.RowIndex].Value));
                frame.Event = (string)FrameList[9, e.RowIndex].Value;
				ProjectChanged = true;
            }
            catch
            {
                FrameList[0, e.RowIndex].Value = frame.DamageCollider.X.ToString();
                FrameList[1, e.RowIndex].Value = frame.DamageCollider.Y.ToString();
                FrameList[2, e.RowIndex].Value = frame.DamageCollider.Width.ToString();
                FrameList[3, e.RowIndex].Value = frame.DamageCollider.Height.ToString();
                FrameList[4, e.RowIndex].Value = frame.HitCollider.X.ToString();
                FrameList[5, e.RowIndex].Value = frame.HitCollider.Y.ToString();
                FrameList[6, e.RowIndex].Value = frame.HitCollider.Width.ToString();
                FrameList[7, e.RowIndex].Value = frame.HitCollider.Height.ToString();
                FrameList[8, e.RowIndex].Value = frame.Time.ToString();
                FrameList[9, e.RowIndex].Value = frame.Event;
                MessageBox.Show("Wrong value!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //выбор кадра
        private void FrameList_CurrentCellChanged(object sender, EventArgs e)
        {
            if (FrameList.CurrentRow != null)
            {
                for (int j = 0; j < FrameList.Rows.Count; j++)
                    for (int i = 0; i < FrameList.Columns.Count; i++)
                        if (j == FrameList.CurrentRow.Index)
                            FrameList[i, j].Style.BackColor = Color.Cyan;
                        else
                            FrameList[i, j].Style.BackColor = Color.White;
            }
            ShowCompositionList();

            if (CompositionList.RowCount > 0)
            {
                DataGridViewCell cell = CompositionList.Rows[CompositionList.RowCount - 1].Cells[0];
                CompositionList.CurrentCell = cell;
                CompositionList.CurrentCell.Selected = true;
            }

        }

        //перемещение кадра вверх по списку
        void FrameUpButton_Click(object sender, EventArgs e)
        {
            if (FrameList.CurrentRow == null)
                return;
            int i = FrameList.CurrentRow.Index;
            if (i == 0)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Frame frame = anim.Frames[i];
            anim.Frames.Remove(frame);
            anim.Frames.Insert(i - 1, frame);
            DataGridViewCell cell = FrameList.Rows[i - 1].Cells[0];
            FrameList.CurrentCell = cell;
            FrameList.CurrentCell.Selected = true;
            ShowFrameList();
			ProjectChanged = true;
        }

        //перемещение кадра вниз по списку
        void FrameDownButton_Click(object sender, EventArgs e)
        {
            if (FrameList.CurrentRow == null)
                return;
            int i = FrameList.CurrentRow.Index;
            if (i >= FrameList.RowCount - 1)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Frame frame = anim.Frames[i];
            anim.Frames.Remove(frame);
            anim.Frames.Insert(i + 1, frame);
            DataGridViewCell cell = FrameList.Rows[i + 1].Cells[0];
            FrameList.CurrentCell = cell;
            FrameList.CurrentCell.Selected = true;
            ShowFrameList();
			ProjectChanged = true;
        }

        //----------------------------------------- Композиции ------------------------------------------

        //показать список композиций
        void ShowCompositionList()
        {
            int i = -1;
            if (CompositionList.CurrentRow != null)
                i = CompositionList.CurrentRow.Index;
            CompositionList.Rows.Clear();
            if (FrameList.CurrentRow == null)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Frame frame = anim.Frames[FrameList.CurrentRow.Index];
            if (anim == null)
                return;
            foreach (Composition c in frame.Compositions)
                CompositionList.Rows.Add(c.ElemSprite.Name, c.Offset.X.ToString(), c.Offset.Y.ToString(), c.View);
            if (i >= CompositionList.RowCount)
                i = CompositionList.RowCount - 1;
            if (i >= 0)
            {
                DataGridViewCell cell = CompositionList.Rows[i].Cells[0];
                CompositionList.CurrentCell = cell;
                CompositionList.CurrentCell.Selected = true;
            }
        }

        //добавить спрайт в композицию
        void AddCompositionSprite()
        {
            if (FrameList.CurrentRow == null)
            {
                MessageBox.Show("Select frame!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (SpriteList.CurrentRow == null)
            {
                MessageBox.Show("Select sprite!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Frame frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index];
            Composition c = new Composition();
            Sprite s = FindSprite((string)(SpriteList[0, SpriteList.CurrentRow.Index].Value));
            c.ElemSprite = s;
            c.Offset.X = (s.SpriteSize.Width / 2) * -1;
            c.Offset.Y = s.SpriteSize.Height * -1;
            c.View = true;
            frame.Compositions.Add(c);
            ShowCompositionList();
            DataGridViewCell cell = CompositionList.Rows[CompositionList.RowCount - 1].Cells[0];
            CompositionList.CurrentCell = cell;
            CompositionList.CurrentCell.Selected = true;
            ProjectChanged = true;
        }

        //новый элемент(спрайт) композиции
        void AddCompSprite_Click(object sender, EventArgs e)
        {
            AddCompositionSprite();
        }

        //удаление элемента(спрайта) композиции
        void DelCompSprite_Click(object sender, EventArgs e)
        {
            if (FrameList.CurrentRow == null)
                return;
            Frame frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index];
            if (frame.Compositions.Count == 0)
                return;
            if (MessageBox.Show("Are you sure?", "Delete composition sprite!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            frame.Compositions.RemoveAt(CompositionList.CurrentRow.Index);
            ShowCompositionList();
			ProjectChanged = true;
        }

        //ручное редактирование списка элементов(спрайтов) композиции
        void CompositionList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            try
            {
                composition.Offset.X = My.LimitRange(Int32.Parse((string)CompositionList[1, e.RowIndex].Value), -64, 64 - FindSprite((string)CompositionList[0, e.RowIndex].Value).SpriteSize.Width);
                composition.Offset.Y = My.LimitRange(Int32.Parse((string)CompositionList[2, e.RowIndex].Value), -96, 32 - FindSprite((string)CompositionList[0, e.RowIndex].Value).SpriteSize.Height);

                composition.View = (bool)CompositionList[3, e.RowIndex].Value;
                InitCompositions();
                ProjectChanged = true;
            }
            catch
            {
                MessageBox.Show("Wrong value!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CompositionList[1, e.RowIndex].Value = composition.Offset.X.ToString();
                CompositionList[2, e.RowIndex].Value = composition.Offset.Y.ToString();          
            }
        }

        //клик на списке композиций
        private void CompositionList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (CompositionList.CurrentRow != null)
            {
                if (e.ColumnIndex == 3 && e.RowIndex == CompositionList.CurrentRow.Index)
                {
                    if (FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index].View == (bool)CompositionList[3, e.RowIndex].EditedFormattedValue)
                        return;
                    FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index].View = (bool)CompositionList[3, e.RowIndex].EditedFormattedValue;
                    InitCompositions();
                    if ((bool)CompositionList[3, e.RowIndex].Value != (bool)CompositionList[3, e.RowIndex].EditedFormattedValue)
                    {
                        CompositionList[3, e.RowIndex].Value = (bool)CompositionList[3, e.RowIndex].EditedFormattedValue;
                        ProjectChanged = true;
                    }
                }
            }
        }

        //выбор комозиции
        void CompositionList_CurrentCellChanged(object sender, EventArgs e)
        {
            

            if (CompositionList.CurrentRow != null)
            {
                for (int j = 0; j < CompositionList.Rows.Count; j++)
                    for (int i = 0; i < CompositionList.Columns.Count; i++)
                        if (j == CompositionList.CurrentRow.Index)
                            CompositionList[i, j].Style.BackColor = Color.Cyan;
                        else
                            CompositionList[i, j].Style.BackColor = Color.White;
            }
            InitCompositions();
        }

        //сдвиг элемента (спрайта) композиции вверх, вниз, влево и вправо
        private void ShiftUpButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            if (composition.Offset.Y < -95)
                return;
            composition.Offset.Y--;
            CompositionList[2, CompositionList.CurrentRow.Index].Value = composition.Offset.Y.ToString();
            InitCompositions();
        }

        private void ShiftDownButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            if (composition.Offset.Y > 31 - FindSprite((string)CompositionList[0, CompositionList.CurrentRow.Index].Value).SpriteSize.Height)
                return;
            composition.Offset.Y++;
            CompositionList[2, CompositionList.CurrentRow.Index].Value = composition.Offset.Y.ToString();
            InitCompositions();
        }

        private void ShiftLeftButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            if (composition.Offset.X < -63)
                return;
            composition.Offset.X--;
            CompositionList[1, CompositionList.CurrentRow.Index].Value = composition.Offset.X.ToString();
            InitCompositions();
        }

        private void ShiftRightButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            if (composition.Offset.X > 63 - FindSprite((string)CompositionList[0, CompositionList.CurrentRow.Index].Value).SpriteSize.Width)
                return;
            composition.Offset.X++;
            CompositionList[1, CompositionList.CurrentRow.Index].Value = composition.Offset.X.ToString();
            InitCompositions();
        }

        //перемещение композиции вверх по списку
        private void CompositionUpButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            int i = CompositionList.CurrentRow.Index;
            if (i == 0)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Composition comp = anim.Frames[FrameList.CurrentRow.Index].Compositions[i];
            anim.Frames[FrameList.CurrentRow.Index].Compositions.Remove(comp);
            anim.Frames[FrameList.CurrentRow.Index].Compositions.Insert(i - 1, comp);
            DataGridViewCell cell = CompositionList.Rows[i - 1].Cells[0];
            CompositionList.CurrentCell = cell;
            CompositionList.CurrentCell.Selected = true;
            ShowCompositionList();
            ProjectChanged = true;
        }

        //перемещение композиции вниз по списку
        private void CompositionDownButton_Click(object sender, EventArgs e)
        {
            if (CompositionList.CurrentRow == null)
                return;
            int i = CompositionList.CurrentRow.Index;
            if (i >= CompositionList.RowCount - 1)
                return;
            Animation anim = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value);
            Composition comp = anim.Frames[FrameList.CurrentRow.Index].Compositions[i];
            anim.Frames[FrameList.CurrentRow.Index].Compositions.Remove(comp);
            anim.Frames[FrameList.CurrentRow.Index].Compositions.Insert(i + 1, comp);
            DataGridViewCell cell = CompositionList.Rows[i + 1].Cells[0];
            CompositionList.CurrentCell = cell;
            CompositionList.CurrentCell.Selected = true;
            ShowCompositionList();
            ProjectChanged = true;
        }

        //---------------------------------- Редактирование композиции ----------------------------------

        class CLayer
        {
            public int Type;
            public int[,] PixelLayer;
            public int[,] AttrLayer;
            public bool View;
        }

        bool _CanEdit = false; //разрешено редактрирование слоя
        bool CanEdit
        {
            get { return _CanEdit; }
            set
            {
                _CanEdit = value;
                if (CompositionList.CurrentRow != null)
                {
                    Composition c = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
                    if (value && CompositionList.CurrentRow.Index == CompositionList.RowCount - 1 && c.View)
                    {
                        ModesPanel.Enabled = true;
                        MakeAutomaskButton.Enabled = true;
                        switch (c.ElemSprite.Type)
                        {
                            case 0:
                                MonochromePalettePanel.Visible = true;
                                break;
                            case 1:
                                ZXPalettePanel.Visible = true;
                                break;
                        }
                    }
                    else
                    {
                        ModesPanel.Enabled = false;
                        MakeAutomaskButton.Enabled = false;
                        MonochromePalettePanel.Visible = false;
                        ZXPalettePanel.Visible = false;
                        ShowCompositions(0, 0, 128, 128);
                    }
                }
            }
        }

        List<CLayer> CLayers = new List<CLayer>(); //список отображаемых слоёв
        Brush[,] PrevFrameLayerBrush = new Brush[128, 128]; //массив для битмапа предыдущего кадра
        Bitmap CompositionImg = new Bitmap(2048, 2048);
        CLayer CurrentLayer;
        Point StartCord = new Point(-1, -1);
        Point CurrentCord = new Point(0, 0);
        int[,] LayerSelectionData; //данные выбранной области
        bool LayerRangeSelected = false; //область выбрана
        int LayerSelectionType; //тип выбранной области
        Rectangle CleanRect; // область, которую необходимо перерисовать

        int MButtonLeft; //значение цвета для левой кнопки мыши
        int MButtonRight; //значение цвета для правой кнопки мыши

        List<CLayer> UndoBuffer = new List<CLayer>(); //буфер для undo/redo
        int UndoIndex; //индекс буфера

        //инициализация композиции
        void InitCompositions()
        {
            if (CompositionList.CurrentRow == null)
            {
                CanEdit = false;
                CompositionView.Image = null;
                return;
            }

            //загрузка массива PrevFrameLayer
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                    PrevFrameLayerBrush[x, y] = null;
            if (ViewPrevFrameCheck.Checked && FrameList.CurrentRow.Index > 0)
            {
                Frame prev_frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index - 1];
                foreach (Composition c in prev_frame.Compositions)
                {
                    //монохромный спрайт с маской
                    if (c.ElemSprite.Type == 0)
                    {
                        Brush bEnable = new SolidBrush(Color.FromArgb((ColorMask.R * 6 + ColorEnable.R) / 7, (ColorMask.G * 6 + ColorEnable.G) / 7, (ColorMask.B * 6 + ColorEnable.B) / 7));
                        Brush bDisable = new SolidBrush(Color.FromArgb((ColorMask.R * 6 + ColorDisable.R) / 7, (ColorMask.G * 6 + ColorDisable.G) / 7, (ColorMask.B * 6 + ColorDisable.B) / 7));
                        for (int y = 0; y < c.ElemSprite.SpriteSize.Height; y++)
                            for (int x = 0; x < c.ElemSprite.SpriteSize.Width; x++)
                                if (x + c.Offset.X + 64 >= 0 && x + c.Offset.X + 64 < 128 && y + c.Offset.Y + 96 >= 0 && y + c.Offset.Y + 96 < 128 && c.ElemSprite.PixelLayer[x, y] != 0)
                                    PrevFrameLayerBrush[x + c.Offset.X + 64, y + c.Offset.Y + 96] = c.ElemSprite.PixelLayer[x, y] == 1 ? bEnable : bDisable;
                    }
                }

            }

            //загрузка слоёв
            CLayers.Clear();
            Frame frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index];
            foreach (Composition c in frame.Compositions)
            {
                CLayer layer = new CLayer();
                layer.Type = c.ElemSprite.Type;
                layer.View = c.View;
                //монохромный спрайт с маской
                if (layer.Type == 0)
                {
                    int[,] a = new int[128, 128];
                    for (int y = 0; y < c.ElemSprite.SpriteSize.Height; y++)
                        for (int x = 0; x < c.ElemSprite.SpriteSize.Width; x++)
                            if (x + c.Offset.X + 64 >= 0 && x + c.Offset.X + 64 < 128 && y + c.Offset.Y + 96 >= 0 && y + c.Offset.Y + 96 < 128)
                                a[x + c.Offset.X + 64, y + c.Offset.Y + 96] = c.ElemSprite.PixelLayer[x, y];
                    layer.PixelLayer = a;
                    CLayers.Add(layer);
                }
            }
            ShowCompositions(0, 0, 128, 128);
            CurrentLayer = CLayers[CLayers.Count - 1];
            CanEdit = (CurrentLayer.View && CompositionList.CurrentRow.Index == CompositionList.RowCount - 1);
            ClearUndoBuffer();
        }

        //вывод изображения композиции
        void ShowCompositions(int xstart, int ystart, int width, int height)
        {
            if (CompositionList.RowCount == 0)
                return;
            int scale = ScaleBar.Value; 
            Graphics g = Graphics.FromImage(CompositionImg);
			g.SmoothingMode = SmoothingMode.HighSpeed;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            Brush bMainMask = new SolidBrush(ColorMask);
            Brush bMonoMainEnable = new SolidBrush(ColorEnable);
            Brush bMonoMainDisable = new SolidBrush(ColorDisable);
            Brush bMonoBehindEnable = new SolidBrush(Color.FromArgb((ColorMask.R + ColorEnable.R * 2) / 3, (ColorMask.G + ColorEnable.G * 2) / 3, (ColorMask.B + ColorEnable.B * 2) / 3));
            Brush bMonoBehindDisable = new SolidBrush(Color.FromArgb((ColorMask.R + ColorDisable.R * 2) / 3, (ColorMask.G + ColorDisable.G * 2) / 3, (ColorMask.B + ColorDisable.B * 2) / 3));

            //прорисовка кадра
            g.FillRectangle(bMainMask, xstart * scale, ystart * scale, width * scale, height * scale);
            for (int y = ystart; y < ystart + height; y++)
                for (int x = xstart; x < xstart + width; x++)
                {
                    Brush current_brush = ViewPrevFrameCheck.Checked ? PrevFrameLayerBrush[x, y] : null;
                    for (int i = CLayers.Count - 1; i >= 0; i--)
                    {
                        if (CLayers[i].PixelLayer[x, y] != 0 && CLayers[i].View)
                        {
                            //монохромный спрайт с маской
                            if (CLayers[i].Type == 0)
                            {
                                if (i == CLayers.Count - 1)
                                    current_brush = CLayers[i].PixelLayer[x, y] == 1 ? bMonoMainEnable : bMonoMainDisable;
                                else
                                    current_brush = CLayers[i].PixelLayer[x, y] == 1 ? bMonoBehindEnable : bMonoBehindDisable;
                            }
                            break;
                        }

                    }
                    if (current_brush != null)
                        g.FillRectangle(current_brush, x * scale, y * scale, scale, scale);
                }

            //сетка
            if (ShowGrid.Checked)
            {
                Pen p_grid = new Pen(Color.LightGray);
                for (int i = xstart; i < xstart + width; i++)
                    g.DrawLine(p_grid, i * scale, ystart * scale, i * scale, (ystart + height) * scale);
                for (int i = ystart; i < ystart + height; i++)
                    g.DrawLine(p_grid, xstart, i * scale, (xstart + width) * scale, i * scale);
            }

            //центр композиции
            Pen p_center = new Pen(ColorSelection);
            p_center.DashStyle = DashStyle.Dash;
            g.DrawLine(p_center, 64 * scale, 0, 64 * scale, 128 * scale);
            g.DrawLine(p_center, 0, 96 * scale, 128 * scale, 96 * scale);

            if (CanEdit)
            {

                //выделение области для режимов копирования и вырезания
                if (!LayerRangeSelected && StartCord.X != -1 && StartCord.Y != -1)
                {
                    g.DrawRectangle(new Pen(ColorSelection), My.MulRect(My.MakeRect(StartCord.X, StartCord.Y, CurrentCord.X, CurrentCord.Y), scale));
                }

                //отображение выделенной области для режимов копирования и вырезания
                if (LayerRangeSelected)
                {
                    //монохромный спрайт с маской
                    if (CurrentLayer.Type == 0)
                    {
                        Brush bEnable = new SolidBrush(ColorEnable);
                        Brush bDisable = new SolidBrush(ColorDisable);
                        Brush bMask = new SolidBrush(ColorMask);
                        g.FillRectangle(bMask, CurrentCord.X * scale, CurrentCord.Y * scale, LayerSelectionData.GetLength(0) * scale, LayerSelectionData.GetLength(1) * scale);
                        for (int y = 0; y < LayerSelectionData.GetLength(1); y++)
                            for (int x = 0; x < LayerSelectionData.GetLength(0); x++)
                            {
                                if (LayerSelectionData[x, y] == 1)
                                    g.FillRectangle(bEnable, (x + CurrentCord.X) * scale, (y + CurrentCord.Y) * scale, scale, scale);
                                else if (LayerSelectionData[x, y] == 2)
                                    g.FillRectangle(bDisable, (x + CurrentCord.X) * scale, (y + CurrentCord.Y) * scale, scale, scale);
                            }
                        g.DrawRectangle(new Pen(ColorSelection), My.MulRect(new Rectangle(CurrentCord.X, CurrentCord.Y, LayerSelectionData.GetLength(0), LayerSelectionData.GetLength(1)), scale));
                    }
                }


                //отображение ластика
                if (EraserMode.Checked && CurrentCord.X != -1 && CurrentCord.Y != -1)
                {
                    Color c = ColorMask;
                    //монохромный спрайт с маской
                    if (CurrentLayer.Type == 0)
                    {
                        if (MButtonRight == 1)
                            c = ColorEnable;
                        else if (MButtonRight == 2)
                            c = ColorDisable;
                    }

                    Brush bEraser = new SolidBrush(c);
                    Brush bSelection = new SolidBrush(ColorSelection);
                    g.FillRectangle(bSelection, (CurrentCord.X - 2) * scale, (CurrentCord.Y - 2) * scale, 5 * scale, 5 * scale);
                    g.FillRectangle(bSelection, (CurrentCord.X - 1) * scale, (CurrentCord.Y - 3) * scale, 3 * scale, 7 * scale);
                    g.FillRectangle(bSelection, (CurrentCord.X - 3) * scale, (CurrentCord.Y - 1) * scale, 7 * scale, 3 * scale);
                    g.FillRectangle(bEraser, (CurrentCord.X - 2) * scale + 1, (CurrentCord.Y - 2) * scale + 1, 5 * scale - 2, 5 * scale - 2);
                    g.FillRectangle(bEraser, (CurrentCord.X - 1) * scale + 1, (CurrentCord.Y - 3) * scale + 1, 3 * scale - 2, 7 * scale - 2);
                    g.FillRectangle(bEraser, (CurrentCord.X - 3) * scale + 1, (CurrentCord.Y - 1) * scale + 1, 7 * scale - 2, 3 * scale - 2);
                }
            }

            CompositionView.Width = 128 * scale;
            CompositionView.Height = 128 * scale;
            CompositionView.Image = CompositionImg;

        }

        //изменение масштаба
        private void ScaleBar_Scroll(object sender, EventArgs e)
        {
            ShowCompositions(0, 0, 128, 128);
        }

        //изменения режима фона (с предыдущим кадром)
        private void ViewPrevFrameCheck_CheckedChanged(object sender, EventArgs e)
        {
            ShowCompositions(0, 0, 128, 128);
        }

        //сохранение слоя со смещениями
        void SaveLayer()
        {
            if (CompositionList.CurrentRow == null)
                return;
            CLayer layer = CLayers[CompositionList.CurrentRow.Index];
            Composition composition = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index].Compositions[CompositionList.CurrentRow.Index];
            Sprite sprite = FindSprite((string)CompositionList[0, CompositionList.CurrentRow.Index].Value);     

            //определяем размеры спрайта
            int x1 = 128;
            int y1 = 128;
            int x2 = 0;
            int y2 = 0;
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                    if (layer.PixelLayer[x, y] != 0)
                    {
                        x1 = Math.Min(x1, x);
                        y1 = Math.Min(y1, y);
                        x2 = Math.Max(x2, x);
                        y2 = Math.Max(y2, y);
                    }

            //пустой спрайт
            if (x1 == 128 && y1 == 128)
            {
                x1 = 64 + composition.Offset.X;
                y1 = 96 + composition.Offset.Y;
                x2 = x1;
                y2 = y1;
            }

            //сохранение смещений и спрайта
            composition.Offset.X = x1 - 64;
            composition.Offset.Y = y1 - 96;
            CompositionList[1, CompositionList.CurrentRow.Index].Value = (x1 - 64).ToString();
            CompositionList[2, CompositionList.CurrentRow.Index].Value = (y1 - 96).ToString();

            //монохромный спрайт с маской
            if (sprite.Type == 0)
            {
                if (sprite.SpriteSize.Width != x2 - x1 + 1 || sprite.SpriteSize.Height != y2 - y1 + 1)
                {
                    sprite.SpriteSize.Width = x2 - x1 + 1;
                    sprite.SpriteSize.Height = y2 - y1 + 1;
                    sprite.PixelLayer = new int[sprite.SpriteSize.Width, sprite.SpriteSize.Height];
                }              
                for (int y = 0; y < sprite.SpriteSize.Height; y++)
                    for (int x = 0; x < sprite.SpriteSize.Width; x++)
                        sprite.PixelLayer[x, y] = layer.PixelLayer[x + x1, y + y1];
            }
            ShowSpriteList();
            ProjectChanged = true;
        }

        //переключение режима сетки
        private void ShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            ShowCompositions(0, 0, 128, 128);
        }

        //рисование точки в режиме карандаша
        void DrawPenMode(int x, int y, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                return;
            int i = e.Button == MouseButtons.Left ? MButtonLeft : MButtonRight;
            CurrentLayer.PixelLayer[x, y] = i;
            ShowCompositions(x, y, 1, 1);
        }

        //стирание области в режиме ластика
        void DrawEraserMode(int x, int y, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            for (int j = -3; j < 4; j++)
                for (int i = -3; i < 4; i++)
                {
                    int xa = Math.Abs(i);
                    int ya = Math.Abs(j);
                    if (x + i >= 0 && x + i < 128 && y + j >= 0 && y + j < 128 && !((xa == 3 && ya == 3) || (xa == 2 && ya == 3) || (xa == 3 && ya == 2)))
                        CurrentLayer.PixelLayer[x + i, y + j] = MButtonRight;
                }
            ShowCompositions(My.LimitRange(x - 3, 0, 128 - 7), My.LimitRange(y - 3, 0, 128 - 7), 7, 7);
        }

        //заливка области
        void FillAreaMode(int x, int y, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
                return;
            int i = e.Button == MouseButtons.Left ? MButtonLeft : MButtonRight;
            Rectangle r = My.FillArrayArea(CurrentLayer.PixelLayer, x, y, i);
            ShowCompositions(r.X, r.Y, r.Width, r.Height);
        }

        //работа с CompositionView
        private void CompositionView_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CanEdit)
                return;

            int scale = ScaleBar.Value; 
            int x = e.X / scale;
            int y = e.Y / scale;

            //начало выбора области для режимов копирования и вырезания
            if ((CopyMode.Checked || CutMode.Checked) && !LayerRangeSelected && e.Button == MouseButtons.Left)
            {
                StartCord = new Point(x, y);
                CurrentCord = new Point(x, y);
                ShowCompositions(0, 0, 128, 128);
            }

            //отмена выбранной области для режимов копирования и вырезания
            if ((CopyMode.Checked || CutMode.Checked) && e.Button == MouseButtons.Right)
            {
                StartCord = new Point(-1, -1);
                CurrentCord = new Point(-1, -1);
                LayerRangeSelected = false;
                if (CutMode.Checked)
                    SaveLayer();
                ShowCompositions(0, 0, 128, 128);
            }

            //вставка фрагмента выбранной области для режимов копирования и вырезания
            if ((CopyMode.Checked || CutMode.Checked) && LayerRangeSelected && e.Button == MouseButtons.Left)
            {
                if (LayerSelectionType == CurrentLayer.Type)
                {
                    for (int j = 0; j < LayerSelectionData.GetLength(1); j++)
                        for (int i = 0; i < LayerSelectionData.GetLength(0); i++)
                            CurrentLayer.PixelLayer[x + i, y + j] = LayerSelectionData[i, j];
                    if (CutMode.Checked)
                    {
                        StartCord = new Point(-1, -1);
                        CurrentCord = new Point(-1, -1);
                        LayerRangeSelected = false;
                    }
                    SaveLayer();
                    AddLayerInUndoBuffer();
                    ShowCompositions(0, 0, 128, 128);
                }
                else
                    MessageBox.Show("Current layer is another type!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //выбор цвета в режиме пипетки
            if (CaptureMode.Checked)
            {
                if (CurrentLayer.Type == 0)
                {
                    if (e.Button == MouseButtons.Left)
                        MButtonLeft = CurrentLayer.PixelLayer[x, y];
                    else if (e.Button == MouseButtons.Right)
                        MButtonRight = CurrentLayer.PixelLayer[x, y];
                    for (int i = 0; i < 3; i++)
                    {
                        var ink = MonochromePalettePanel.Controls["mwm_I" + i.ToString()];
                        var paper = MonochromePalettePanel.Controls["mwm_P" + i.ToString()];
                        if (i == MButtonLeft)
                            ink.Text = "V";
                        else
                            ink.Text = "";
                        if (i == MButtonRight)
                            paper.Text = "V";
                        else
                            paper.Text = "";
                    }
                }
            }

            //начало рисования в режиме карандаша
            if (PenMode.Checked)
                DrawPenMode(x, y, e);

            //начало стирания области в режиме ластика
            if (EraserMode.Checked)
                DrawEraserMode(x, y, e);

            //начало заливки области
            if (FillMode.Checked)
                FillAreaMode(x, y, e);
        }

        private void CompositionView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!CanEdit)
                return;

            int scale = ScaleBar.Value; 
            int x = My.LimitRange(e.X / scale, 0, 127);
            int y = My.LimitRange(e.Y / scale, 0 ,127);

            //продолжение выбора области для режимов копирования и вырезания
            if ((CopyMode.Checked || CutMode.Checked) && !LayerRangeSelected && (CurrentCord.X != x || CurrentCord.Y != y) && e.Button == MouseButtons.Left)
            {
                CurrentCord = new Point(x, y);
                ShowCompositions(0, 0, 128, 128);
            }

            //перерасчёт текущих координат для выбранной области с учётом ее размеров
            if ((CopyMode.Checked || CutMode.Checked) && LayerRangeSelected && (CurrentCord.X != x || CurrentCord.Y != y))
            {
                CurrentCord.X = My.LimitRange(x, 0, 128 - LayerSelectionData.GetLength(0));
                CurrentCord.Y = My.LimitRange(y, 0, 128 - LayerSelectionData.GetLength(1));
                ShowCompositions(0, 0, 128, 128);
            }

            //продолжение рисования в режиме карандаша
            if (PenMode.Checked && (CurrentCord.X != x || CurrentCord.Y != y))
            {
                DrawPenMode(x, y, e);
                CurrentCord = new Point(x, y);
            }

            //обновление CompositionView в режиме ластика
            if (EraserMode.Checked && (CurrentCord.X != x || CurrentCord.Y != y))
            {
                CurrentCord.X = x;
                CurrentCord.Y = y;
                ShowCompositions(CleanRect.X, CleanRect.Y, CleanRect.Width, CleanRect.Height);
                CleanRect.Width = 7;
                CleanRect.Height = 7;
                CleanRect.X = My.LimitRange(x - 3, 0, 128 - CleanRect.Width);
                CleanRect.Y = My.LimitRange(y - 3, 0, 128 - CleanRect.Height);
                DrawEraserMode(x, y, e);
            }

            //продолжение заливки области
            if (FillMode.Checked && (CurrentCord.X != x || CurrentCord.Y != y))
            {
                FillAreaMode(x, y, e);
                CurrentCord = new Point(x, y);
            }
        }

        private void CompositionView_MouseUp(object sender, MouseEventArgs e)
        {
            if (!CanEdit)
                return;

            //окончание выбора области для режимов копирования и вырезания
            if ((CopyMode.Checked || CutMode.Checked) && !LayerRangeSelected && e.Button == MouseButtons.Left && StartCord.X != -1 && StartCord.Y != -1)
            {
                LayerRangeSelected = true;
                Rectangle r = My.MakeRect(StartCord.X, StartCord.Y, CurrentCord.X, CurrentCord.Y);
                LayerSelectionData = new int[r.Width, r.Height];
                LayerSelectionType = CurrentLayer.Type;
                for (int j = 0; j < r.Height; j++)
                    for (int i = 0; i < r.Width; i++)
                    {
                        LayerSelectionData[i, j] = CurrentLayer.PixelLayer[r.X + i, r.Y + j];
                        if (CutMode.Checked)
                            CurrentLayer.PixelLayer[r.X + i, r.Y + j] = 0;
                    }
                CurrentCord.X = My.LimitRange(CurrentCord.X, 0, 128 - r.Width);
                CurrentCord.Y = My.LimitRange(CurrentCord.Y, 0, 128 - r.Height);
                if (CutMode.Checked)
                {
                    SaveLayer();
                    AddLayerInUndoBuffer();
                }
                ShowCompositions(0, 0, 128, 128);
            }

            //окончание рисования в режиме карандаша, ластика или заливки
            if (PenMode.Checked || EraserMode.Checked || FillMode.Checked)
            {
                SaveLayer();
                AddLayerInUndoBuffer();
            }
        }

        private void CompositionView_MouseLeave(object sender, EventArgs e)
        {
            if (!CanEdit)
                return;

            //скрытие ластика при выходе кусрора из CompositionView
            if (EraserMode.Checked)
            {
                CurrentCord.X = -1;
                CurrentCord.Y = -1;
                ShowCompositions(0, 0, 128, 128);
            }
        }

        //выбор инструментов
        void SelectTool(object sender, EventArgs e)
        {
            StartCord = new Point(-1, -1);
            CurrentCord = new Point(-1, -1);
            LayerRangeSelected = false;
            ShowCompositions(0, 0, 128, 128);
        }

        //поиск спрайта в SpriteList
        private void CompositionList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string n = (string)CompositionList[0, e.RowIndex].Value;
            if (SpriteFilter.Text != "" && !SpriteFilter.Text.Contains(n))
            {
                SpriteFilter.Text = "";
                ShowSpriteList();
            }
            for (int i = 0; i < SpriteList.RowCount; i++)
                if ((string)SpriteList[0, i].Value == n)
                {
                    DataGridViewCell cell = SpriteList.Rows[i].Cells[0];
                    SpriteList.CurrentCell = cell;
                    SpriteList.CurrentCell.Selected = true;
                    return;
                }
        }

        //очистка и инициализация буфера Undo
        void ClearUndoBuffer()
        {
            UndoBuffer.Clear();
            UndoIndex = -1;
            AddLayerInUndoBuffer();
            UndoButton.Enabled = false;
            RedoButton.Enabled = false;
        }

        //добавить слой в UndoBuffer
        void AddLayerInUndoBuffer()
        {
            UndoIndex++;
            if (UndoIndex <= UndoBuffer.Count - 1)
                UndoBuffer.RemoveRange(UndoIndex, UndoBuffer.Count - UndoIndex);
            CLayer l = new CLayer();
            l.Type = CurrentLayer.Type;
            l.View = CurrentLayer.View;
            if (CurrentLayer.PixelLayer != null)
            {
                l.PixelLayer = new int[CurrentLayer.PixelLayer.GetLength(0), CurrentLayer.PixelLayer.GetLength(1)];
                for (int y = 0; y < CurrentLayer.PixelLayer.GetLength(1); y++)
                    for (int x = 0; x < CurrentLayer.PixelLayer.GetLength(0); x++)
                        l.PixelLayer[x, y] = CurrentLayer.PixelLayer[x, y];
            }
            if (CurrentLayer.AttrLayer != null)
            {
                l.AttrLayer = new int[CurrentLayer.AttrLayer.GetLength(0), CurrentLayer.AttrLayer.GetLength(1)];
                for (int y = 0; y < CurrentLayer.AttrLayer.GetLength(1); y++)
                    for (int x = 0; x < CurrentLayer.AttrLayer.GetLength(0); x++)
                        l.AttrLayer[x, y] = CurrentLayer.AttrLayer[x, y];
            }
            UndoBuffer.Add(l);
            if (UndoIndex >= 10000)
                MessageBox.Show("Recommended save the sprites and restart the application!", "UNDO/REDO BUFFER IS FULL!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            UndoButton.Enabled = true;
            RedoButton.Enabled = false;
        }

        //восстановить слой
        void UndoLayer()
        {
            if (UndoIndex == 0)
                return;
            UndoIndex--;

            CurrentLayer.Type = UndoBuffer[UndoIndex].Type;
            CurrentLayer.View = UndoBuffer[UndoIndex].View;
            if (UndoBuffer[UndoIndex].PixelLayer != null)
            {
                CurrentLayer.PixelLayer = new int[UndoBuffer[UndoIndex].PixelLayer.GetLength(0), UndoBuffer[UndoIndex].PixelLayer.GetLength(1)];
                for (int y = 0; y < UndoBuffer[UndoIndex].PixelLayer.GetLength(1); y++)
                    for (int x = 0; x < UndoBuffer[UndoIndex].PixelLayer.GetLength(0); x++)
                        CurrentLayer.PixelLayer[x, y] = UndoBuffer[UndoIndex].PixelLayer[x, y];
            }
            if (UndoBuffer[UndoIndex].AttrLayer != null)
            {
                CurrentLayer.AttrLayer = new int[UndoBuffer[UndoIndex].AttrLayer.GetLength(0), UndoBuffer[UndoIndex].AttrLayer.GetLength(1)];
                for (int y = 0; y < UndoBuffer[UndoIndex].AttrLayer.GetLength(1); y++)
                    for (int x = 0; x < UndoBuffer[UndoIndex].AttrLayer.GetLength(0); x++)
                        CurrentLayer.AttrLayer[x, y] = UndoBuffer[UndoIndex].AttrLayer[x, y];
            }
            ShowCompositions(0, 0, 128, 128);
            if (UndoIndex == 0)
                UndoButton.Enabled = false;
            RedoButton.Enabled = true;
            SaveLayer();
        }

        //отменить восстановление слоя
        void RedoLayer()
        {
            if (UndoIndex == UndoBuffer.Count - 1)
                return;
            UndoIndex++;

            CurrentLayer.Type = UndoBuffer[UndoIndex].Type;
            CurrentLayer.View = UndoBuffer[UndoIndex].View;
            if (UndoBuffer[UndoIndex].PixelLayer != null)
            {
                CurrentLayer.PixelLayer = new int[UndoBuffer[UndoIndex].PixelLayer.GetLength(0), UndoBuffer[UndoIndex].PixelLayer.GetLength(1)];
                for (int y = 0; y < UndoBuffer[UndoIndex].PixelLayer.GetLength(1); y++)
                    for (int x = 0; x < UndoBuffer[UndoIndex].PixelLayer.GetLength(0); x++)
                        CurrentLayer.PixelLayer[x, y] = UndoBuffer[UndoIndex].PixelLayer[x, y];
            }
            if (UndoBuffer[UndoIndex].AttrLayer != null)
            {
                CurrentLayer.AttrLayer = new int[UndoBuffer[UndoIndex].AttrLayer.GetLength(0), UndoBuffer[UndoIndex].AttrLayer.GetLength(1)];
                for (int y = 0; y < UndoBuffer[UndoIndex].AttrLayer.GetLength(1); y++)
                    for (int x = 0; x < UndoBuffer[UndoIndex].AttrLayer.GetLength(0); x++)
                        CurrentLayer.AttrLayer[x, y] = UndoBuffer[UndoIndex].AttrLayer[x, y];
            }
            ShowCompositions(0, 0, 128, 128);
            if (UndoIndex == UndoBuffer.Count - 1)
                RedoButton.Enabled = false;
            UndoButton.Enabled = true;
            SaveLayer();
        }

        //выполнение undo
        private void UndoButton_Click(object sender, EventArgs e)
        {
            UndoLayer();
        }

        //выполнение redo
        private void RedoButton_Click(object sender, EventArgs e)
        {
            RedoLayer();
        }

        //сделать автомаску
        private void MakeAutomaskButton_Click(object sender, EventArgs e)
        {
            if (FrameList.CurrentRow == null)
                return;
            Frame frame = FindAnimation((string)AnimationList[0, AnimationList.CurrentRow.Index].Value).Frames[FrameList.CurrentRow.Index];
            if (frame.Compositions.Count == 0)
                return;
            if (MessageBox.Show("Are you sure?", "Make automask for this sprite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            int[,] m = new int[128, 128];
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                    if (CurrentLayer.PixelLayer[x, y] == 1)
                    {
                        m[x, y] = 2;
                        if (x > 0)
                            m[x - 1, y] = 2;
                        if (x < 127)
                            m[x + 1, y] = 2;
                        if (y > 0)
                            m[x, y - 1] = 2;
                        if (y < 127)
                            m[x, y + 1] = 2;
                    }
            int xstart = 0;
            int ystart = 0;
            if (m[xstart, ystart] != 0)
                xstart = 127;
            if (m[xstart, ystart] != 0)
                ystart = 127;
            if (m[xstart, ystart] != 0)
                xstart = 0;
            if (m[xstart, ystart] != 0)
            {
                MessageBox.Show("Sorry, can't make automask.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            My.FillArrayArea(m, xstart, ystart, 1);
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                {
                    if (m[x, y] == 1)
                        m[x, y] = 0;
                    else if (m[x, y] == 0)
                        m[x, y] = 2;
                    if (CurrentLayer.PixelLayer[x, y] == 1)
                        m[x, y] = 1;
                }
            CurrentLayer.PixelLayer = m;
            ShowCompositions(0, 0, 128, 128);
            AddLayerInUndoBuffer();
            SaveLayer();
        }

        //-------------------------------- Монохромная палитра с маской ---------------------------------

        void InitMonochromePalette()
        {
            mwm_I0.BackColor = ColorMask;
            mwm_P0.BackColor = ColorMask;
            mwm_I1.BackColor = ColorEnable;
            mwm_P1.BackColor = ColorEnable;
            mwm_I2.BackColor = ColorDisable;
            mwm_P2.BackColor = ColorDisable;
            MButtonLeft = 1;
            MButtonRight = 0;
            mwm_I1.Text = "V";
            mwm_P0.Text = "V";
        }

        private void mwm_I0_Click(object sender, EventArgs e)
        {
            mwm_I0.Text = "V";
            mwm_I1.Text = "";
            mwm_I2.Text = "";
            MButtonLeft = 0;
        }

        private void mwm_I1_Click(object sender, EventArgs e)
        {
            mwm_I0.Text = "";
            mwm_I1.Text = "V";
            mwm_I2.Text = "";
            MButtonLeft = 1;
        }

        private void mwm_I2_Click(object sender, EventArgs e)
        {
            mwm_I0.Text = "";
            mwm_I1.Text = "";
            mwm_I2.Text = "V";
            MButtonLeft = 2;
        }

        private void mwm_P0_Click(object sender, EventArgs e)
        {
            mwm_P0.Text = "V";
            mwm_P1.Text = "";
            mwm_P2.Text = "";
            MButtonRight = 0;
        }

        private void mwm_P1_Click(object sender, EventArgs e)
        {
            mwm_P0.Text = "";
            mwm_P1.Text = "V";
            mwm_P2.Text = "";
            MButtonRight = 1;
        }

        private void mwm_P2_Click(object sender, EventArgs e)
        {
            mwm_P0.Text = "";
            mwm_P1.Text = "";
            mwm_P2.Text = "V";
            MButtonRight = 2;
        }


        //------------------------------------------ Интерфейс ------------------------------------------
        

        private void ShowCollider_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void ShowShadow_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        

        //компиляция спрайтов
        /*private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //удаление предыдущих файлов
            File.Delete(SourcePath + @"/spritenames.asm");
            File.Delete(BinPath + @"/sprheaders.bin");
            string[] files = Directory.GetFiles(BinPath, "spritedata*.bin");
            foreach (string file in files)
            {
                File.Delete(file);
            }

            //создаем файлы sprheaders.bin и spritenames.asm
            BinaryWriter sprheaders = new BinaryWriter(File.Open(BinPath + @"/sprheaders.bin", FileMode.Create));
            StreamWriter spritenames = new StreamWriter(File.Open(SourcePath + @"/spritenames.asm", FileMode.Create));
            spritenames.WriteLine(";sprite names, generated automatically");

            for (int i = 0; i < SpriteList.Items.Count; i++)
            {
                string spritename = SpriteList.Items[i].ToString();
                spritenames.WriteLine(spritename + " = " + i);
                BinaryReader sprite = new BinaryReader(File.Open(WorkPath + spritename + ".spr", FileMode.Open));
                byte[] temp = new byte[6];
                for (int j = 0; j < 6; j++)
                {
                    temp[j] = sprite.ReadByte();
                }
                int xsize = (sprite.ReadByte() - 1) / 8 + 1;
                int ysize = sprite.ReadByte();
                sprheaders.Write(temp[0]);
                int offset = temp[0] < 128 ? temp[0] : temp[0] - 256;
                byte r = (byte)((-(xsize * 4 + offset + xsize * 4)) & 0xff);
                sprheaders.Write(r);
                for (int j = 1; j < 6; j++)
                {
                    sprheaders.Write(temp[j]);
                }               
                sprheaders.Write((byte)xsize);
                sprheaders.Write((byte)ysize);
                //чтение спрайта в массив
                byte[,] sprarray = new byte[xsize * 8, ysize];
                byte[,] shadowarray = new byte[xsize * 8, ysize / 8 + 1];
                for (int y = 0; y < ysize; y++)
                {
                    for (int x = 0; x < xsize * 8; x++)
                    {
                        sprarray[x, y] = 2;
                    }
                }
                for (int y = 0; y < ysize; y++)
                    for (int x = 0; x < xsize; x++)
                    {
                        byte m = sprite.ReadByte();
                        byte s = sprite.ReadByte();
                        for (int j = 0; j < 8; j++)
                        {
                            sprarray[x * 8 + j, y] = (s & 128 >> j) == 0 ? (byte)0 : (byte)1;
                            if ((m & 128 >> j) != 0)
                            {
                                sprarray[x * 8 + j, y] = 2;
                            }
                        }
                    }
                sprite.Close();
                for (int y = 0; y < ysize; y++)
                {
                    for (int x = 0; x < xsize * 8; x++)
                    {
                        if (sprarray[x, y] == 1)
                        {
                            shadowarray[x, y / 8] = 1;
                        }
                    }
                }
                //запись данных спрайта в spritedata*.bin
                for (int j = 0; j < 2; j++)
                {
                    //рачет размера спрайта и поиск подходящего spritedata
                    int spritesize = xsize * ysize * 2 + xsize * ysize / 8;
                    int spriteadres = 0xc000;
                    string romname = "";
                    DirectoryInfo dir = new DirectoryInfo(BinPath);
                    FileInfo[] roms = dir.GetFiles("spritedata*.bin");
                    foreach (FileInfo rom in roms)
                    {
                        if (rom.Length + spritesize - 1 < 0x4000)
                        {
                            romname = BinPath + rom.Name;
                            spriteadres += (int)rom.Length;
                            break;
                        }
                    }
                    if (romname == "")
                    {
                        romname = BinPath + "spritedata" + roms.Count() + ".bin";
                    }
                    //вычисляем номер ROM
                    string shortname = Path.GetFileNameWithoutExtension(romname);
                    int romnum = Int32.Parse(shortname.Substring(10, shortname.Length - 10));
                    //записываем данные спрайта в ROM
                    sprheaders.Write((byte)romnum);
                    sprheaders.Write(lbyte(spriteadres));
                    sprheaders.Write(hbyte(spriteadres));
                    BinaryWriter sprdata = new BinaryWriter(File.Open(romname, FileMode.Append, FileAccess.Write));
                    //тень спрайта
                    for (int y = 0; y < ysize / 8; y++)
                    {
                        for (int x = 0; x < xsize; x++)
                        {
                            byte b = 0;
                            for (int bit = 0; bit < 8; bit++)
                            {
                                if (shadowarray[x * 8 + bit, y] == 1)
                                {
                                    b = (byte)(b | (128 >> bit));
                                }
                            }
                            sprdata.Write(b);
                            sprdata.Write((byte)0);
                        }
                    }
                    //данные спрайта
                    for (int y = 0; y < ysize; y++)
                    {
                        for (int x = 0; x < xsize; x++)
                        {
                            byte m = 0;
                            byte d = 0;
                            for (int bit = 0; bit < 8; bit++)
                            {
                                if (sprarray[x * 8 + bit, y] == 0)
                                {
                                    m = (byte)(m | (128 >> bit));
                                    d = (byte)(d | (128 >> bit));
                                }
                                if (sprarray[x * 8 + bit, y] == 1)
                                {
                                    m = (byte)(m | (128 >> bit));
                                }
                            }
                            sprdata.Write(m);
                            sprdata.Write(d);
                        }
                    }
                    sprdata.Close();
                    //зеркалируем тень
                    for (int y = 0; y < ysize / 8; y++)
                    {
                        for (int x = 0; x < xsize * 4; x++)
                        {
                            byte b = shadowarray[x, y];
                            shadowarray[x, y] = shadowarray[xsize * 8 - x - 1, y];
                            shadowarray[xsize * 8 - x - 1, y] = b;
                        }
                    }
                    //зеркалируем спрайт
                    for (int y = 0; y < ysize; y++)
                    {
                        for (int x = 0; x < xsize * 4; x++)
                        {
                            byte b = sprarray[x, y];
                            sprarray[x, y] = sprarray[xsize * 8 - x - 1, y];
                            sprarray[xsize * 8 - x - 1, y] = b;
                        }
                    }
                }
                sprheaders.Write((byte)0);
                // sprheaders.Write((byte)0);
            }
            sprheaders.Close();
            spritenames.Close();
            MessageBox.Show("Sprites compiled!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }*/

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

    }
}
