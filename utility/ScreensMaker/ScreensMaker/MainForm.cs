using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXProc;

namespace ScreensMaker
{
    public partial class MainForm : Form
    {
        string Version = "Screen Maker v1.0";
        public class Sprite
        {
            public string name; //имя спрайта
            public int height; //ширина спрайта в символах
            public int width; //высота спрайта в символах
            public byte[,][] data; //данные спрайта
            public Bitmap img; //изображение спрайта
            public int lenght; //размер спрайта в байтах
        }
        public List<Sprite> Sprites = new List<Sprite>();
        public class ScreenSprite
        {
            public Sprite sprite; //спрайт
            public int x_cord; //координата X
            public int y_cord; //координата Y
            public int attr = 0; //мастер-атрибут
        }
        public class Screen
        {
            public string name; //имя экрана
            public List<ScreenSprite> sprites = new List<ScreenSprite>();
            public bool visible = true;
        }
        List<Screen> Screens = new List<Screen>();

        Bitmap PreviewSprite = new Bitmap(222, 171);
        Bitmap PreviewScreen = new Bitmap(512, 384);
        string ProjectPath = ""; //путь к текущему проекту
        public bool ProjectChanged = false;

        public MainForm()
        {
            InitializeComponent();
            ShowSpritePreview(null);
            Graphics g = Graphics.FromImage(PreviewScreen);
            g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, PreviewScreen.Width, PreviewScreen.Height);
            ScreenImg.Image = PreviewScreen;
            LoadSetup();
            if (ProjectPath != "")
            {
                LoadProject();
            }
            ShowProjectName();
        }

        //чтение настроек
        void LoadSetup()
        {
            if (File.Exists("setup.bin"))
            {
                BinaryReader setup = new BinaryReader(File.Open("setup.bin", FileMode.Open));
                ProjectPath = setup.ReadString();
                setup.Close();
            }
        }

        //сохранение настроек
        void SaveSetup()
        {
            BinaryWriter setup = new BinaryWriter(File.Open("setup.bin", FileMode.Create, FileAccess.Write));
            setup.Write(ProjectPath);
            setup.Close();
        }

        //вывод в заголовок программы назнание проекта
        void ShowProjectName()
        {
            String s = Version + "   ";
            if (ProjectPath == "")
                s += "-=New project=-";
            else
                s += "-=" + Path.GetFileNameWithoutExtension(ProjectPath) + "=-";
            this.Text = s;
            CalcProjectLenght();
        }

        //загрузка проекта
        void LoadProject()
        {
            try
            {
                if (ProjectPath == "")
                {
                    if (OpenProjectDialog.ShowDialog() == DialogResult.Cancel)
                        return;
                    ProjectPath = OpenProjectDialog.FileName;
                }
                BinaryReader proj = new BinaryReader(File.Open(ProjectPath, FileMode.Open));

                //загружаем спрайты
                int spr_count = proj.ReadInt32();
                Sprites.Clear();
                for (int i = 0; i < spr_count; i++)
                {
                    Sprite spr = new Sprite();
                    spr.name = proj.ReadString();
                    spr.lenght = proj.ReadInt32();
                    spr.width = proj.ReadInt32();
                    spr.height = proj.ReadInt32();
                    spr.data = new byte[spr.width, spr.height][];
                    for (int y = 0; y < spr.height; y++)
                        for (int x = 0; x < spr.width; x++)
                        {
                            spr.data[x, y] = new byte[9];
                            for (int j = 0; j < 9; j++)
                                spr.data[x, y][j] = proj.ReadByte();
                        }
                    CreateSpriteImg(spr);
                    Sprites.Add(spr);
                }

                //загружаем скрины
                int scr_count = proj.ReadInt32();
                Screens.Clear();
                for (int i = 0; i < scr_count; i++)
                {
                    Screen scr = new Screen();
                    scr.name = proj.ReadString();
                    scr.visible = proj.ReadBoolean();
                    int scrspr_count = proj.ReadInt32();
                    for (int j = 0; j < scrspr_count; j++)
                    {
                        ScreenSprite scrspr = new ScreenSprite();
                        scrspr.sprite = FindSprite(proj.ReadString());
                        scrspr.x_cord = proj.ReadInt32();
                        scrspr.y_cord = proj.ReadInt32();
                        scrspr.attr = proj.ReadInt32();
                        scr.sprites.Add(scrspr);
                    }
                    Screens.Add(scr);
                }

                proj.Close();
                RefreshSpritesList();
                RefreshScreensList();
                ProjectChanged = false;
                ShowProjectName();
            }
            catch
            {

            }
        }

        //сохранение проекта
        void SaveProject()
        {
            if (ProjectPath == "")
            {
                if (SaveProjectDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                ProjectPath = SaveProjectDialog.FileName;
            }
            BinaryWriter proj = new BinaryWriter(File.Open(ProjectPath, FileMode.Create, FileAccess.Write));
            proj.Write(Sprites.Count());
            foreach (Sprite spr in Sprites)
            {
                proj.Write(spr.name);
                proj.Write(spr.lenght);
                proj.Write(spr.width);
                proj.Write(spr.height);
                for (int y = 0; y < spr.height; y++)
                    for (int x = 0; x < spr.width; x++)
                        for (int i = 0; i < 9; i++)
                            proj.Write(spr.data[x, y][i]);
            }
            proj.Write(Screens.Count());
            foreach (Screen scr in Screens)
            {
                proj.Write(scr.name);
                proj.Write(scr.visible);
                proj.Write(scr.sprites.Count());
                foreach (ScreenSprite scrspr in scr.sprites)
                {
                    proj.Write(scrspr.sprite.name);
                    proj.Write(scrspr.x_cord);
                    proj.Write(scrspr.y_cord);
                    proj.Write(scrspr.attr);
                }
            }
            proj.Close();
            MessageBox.Show("Project saved!", "Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowProjectName();
            ProjectChanged = false;
        }

        //закрытие программы
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetup();
            if (ProjectChanged)
            {
                if (MessageBox.Show("Save project?", "Project was changed!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    SaveProject();
            }
        }

        //вызов сохранения проекта
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        //новый проект
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectChanged)
            {
                DialogResult r = MessageBox.Show("Save project?", "Project was changed!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (r == DialogResult.Cancel)
                    return;
                if (r == DialogResult.Yes)
                    SaveProject();
            }
            Sprites.Clear();
            Screens.Clear();
            RefreshSpritesList();
            RefreshScreensList();
            ProjectPath = "";
            ShowProjectName();
        }

        //открыть проект
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectChanged)
            {
                DialogResult r = MessageBox.Show("Save project?", "Project was changed!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (r == DialogResult.Cancel)
                    return;
                if (r == DialogResult.Yes)
                    SaveProject();
            }
            string last_project = ProjectPath;
            ProjectPath = "";
            LoadProject();
            if (ProjectPath == "")
                ProjectPath = last_project;
        }

        //расчёт размера скомпилированного проекта
        int spr_sum = 0;
        int scr_sum = 0;
        public void CalcProjectLenght()
        {
            spr_sum = 0;
            scr_sum = 0;
            foreach (Sprite spr in Sprites)
                spr_sum += spr.lenght + 4;
            foreach (Screen scr in Screens)
            {
                foreach (ScreenSprite scr_spr in scr.sprites)
                    scr_sum += 4;
                scr_sum += 3;
            }
            StatusSpriteLenght.Text = spr_sum.ToString();
            StatusScreenLenght.Text = scr_sum.ToString();
            StatusTotalLenght.Text = (spr_sum + scr_sum).ToString();
        }

        //компилирование проекта
        private void compileSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
            string name = Path.GetFileNameWithoutExtension(ProjectPath);
            BinaryWriter bin = new BinaryWriter(File.Open(Path.GetDirectoryName(ProjectPath) + "/" + name + ".bin", FileMode.Create, FileAccess.Write));
            StreamWriter asm = new StreamWriter(File.Open(Path.GetDirectoryName(ProjectPath) + "/" + name + ".asm", FileMode.Create, FileAccess.Write));
            asm.WriteLine(";" + name + " sprites & screens names, generated automatically");
            asm.WriteLine();
            asm.WriteLine(";screns adres offset");
            asm.WriteLine(Path.GetFileNameWithoutExtension(ProjectPath) + "_offset = " + spr_sum.ToString());
            asm.WriteLine();
            
            //таблица адресов спрайтов
            asm.WriteLine(";sprites nums");
            int adr = Sprites.Count * 2;
            int num = 0;
            foreach (Sprite spr in Sprites)
            {
                asm.WriteLine(spr.name + " = " + num.ToString());
                bin.Write((ushort)adr);
                adr += spr.lenght + 2;
                num++;
            }

            //данные спрайтов
            foreach (Sprite spr in Sprites)
            {
                bin.Write((byte)spr.width);
                bin.Write((byte)spr.height);
                for (int y = 0; y < spr.height; y++)
                    for (int x = 0; x < spr.width; x++)
                        if (spr.data[x, y][0] == 0)
                            bin.Write((byte)0);
                        else if ((spr.data[x, y][0] & 0x80) != 0)
                            bin.Write((byte)(((spr.data[x, y][0] >> 3) & 0x07) + (spr.data[x, y][0] & 0xf8)));
                        else
                            for (int i = 0; i < 9; i++)
                                bin.Write(spr.data[x, y][i]);
            }

            //таблица адресов экранов
            asm.WriteLine();
            asm.WriteLine(";screens nums");
            adr = Screens.Count * 2;
            num = 0;
            foreach (Screen scr in Screens)
            {
                asm.WriteLine(scr.name + " = " + num.ToString());
                bin.Write((ushort)adr);
                adr += scr.sprites.Count * 4 + 1;
                num++;
            }

            //данные экранов
            foreach (Screen scr in Screens)
            {
                foreach (ScreenSprite scr_spr in scr.sprites)
                {
                    int i = 0;
                    for (i = 0; i < Sprites.Count; i++)
                        if (scr_spr.sprite.name == Sprites[i].name)
                            break;
                    bin.Write((byte)i);
                    bin.Write((byte)scr_spr.x_cord);
                    bin.Write((byte)scr_spr.y_cord);
                    bin.Write((byte)scr_spr.attr);
                }
                bin.Write((byte)0xff);
            }
        
            bin.Close();
            asm.Close();
        }

        //----------------------------------------------------- спрайты -----------------------------------------------------

        Sprite CurrentSprite = null;

        //изменение объекта текущего спрайта
        private void SpritesList_CurrentCellChanged(object sender, EventArgs e)
        {
            CurrentSprite = null;
            if (SpritesList.CurrentCell == null)
                return;
            CurrentSprite = Sprites[SpritesList.CurrentCell.RowIndex];
            ShowSpritePreview(CurrentSprite);
        }

        //поиск спрайта по имени в списке Sprites
        public Sprite FindSprite(string name)
        {
            foreach (Sprite s in Sprites)
            {
                if (s.name == name)
                    return s;
            }
            return null;
        }

        //найти спрайт в списке и установить на него курсор
        public void FindInSpritesList(Sprite spr)
        {
            int i = 0;
            foreach (Sprite s in Sprites)
            {
                if (s.name == spr.name)
                    break;
                i++;
            }
            SpritesList.CurrentCell = SpritesList.Rows[i].Cells[0];
            SpritesList.CurrentCell.Selected = true;
        }

        //обновление списка спрайтов
        public void RefreshSpritesList()
        {
            int i = -1;
            if (SpritesList.CurrentRow != null)
                i = SpritesList.CurrentRow.Index;
            SpritesList.Rows.Clear();
            foreach (Sprite s in Sprites)
            {
                SpritesList.Rows.Add(s.name, s.width.ToString() + " x " + s.height.ToString());
            }

            if (i >= SpritesList.RowCount)
                i = SpritesList.RowCount - 1;
            if (i >= 0)
            {
                SpritesList.CurrentCell = SpritesList.Rows[i].Cells[0];
                SpritesList.CurrentCell.Selected = true;
            }
            ShowSpritePreview(CurrentSprite);
        }

        //прорисовка спрайта 8x8
        public void DrawChar(Graphics g, int x, int y, byte[] ch, int master_attr)
        {
            if (ch[0] == 0)
                return;
            Brush ColorInk;            
            Brush ColorPaper;
            Brush ColorPix;
            if (master_attr == 0)
            {
                ColorInk = new SolidBrush(ZX.GetColor((ch[0] & 7) + ((ch[0] >> 3) & 8)));
                ColorPaper = new SolidBrush(ZX.GetColor(((ch[0] >> 3) & 15)));
            }
            else
            {
                ColorInk = new SolidBrush(ZX.GetColor((master_attr & 7) + ((master_attr >> 3) & 8)));
                ColorPaper = new SolidBrush(ZX.GetColor(((master_attr >> 3) & 15)));
            }
            if ((ch[0] & 0x80) == 0)
            {
                for (int j = 0; j < 8; j++)
                    for (int i = 0; i < 8; i++)
                    {
                        ColorPix = ((ch[j + 1] << i) & 128) == 0 ? ColorPaper : ColorInk;
                        g.FillRectangle(ColorPix, x * 16 + i * 2, y * 16 + j * 2, 2, 2);
                    }
            }
            else
            {
                g.FillRectangle(ColorPaper, x * 16, y * 16, 16, 16);
            }
        }

        //создать изображение спрайта
        public void CreateSpriteImg(Sprite spr)
        {
            spr.img = new Bitmap(spr.width * 16, spr.height * 16);
            Graphics g = Graphics.FromImage(spr.img);
            for (int y = 0; y < spr.height; y++)
                for (int x = 0; x < spr.width; x++)
                    DrawChar(g, x, y, spr.data[x, y], 0);
        }

        //показать превью спрайта
        void ShowSpritePreview(Sprite spr)
        {
            Graphics g = Graphics.FromImage(PreviewSprite);
            g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, PreviewSprite.Width, PreviewSprite.Height);
            if (spr != null)
            {
                float k = Math.Min((float)PreviewSprite.Width / (float)spr.img.Width, (float)PreviewSprite.Height / (float)spr.img.Height);
                int xsz = (int)(spr.img.Width * k);
                int ysz = (int)(spr.img.Height * k);
                int x = PreviewSprite.Width / 2 - xsz / 2;
                int y = PreviewSprite.Height / 2 - ysz / 2;
                g.DrawImage(spr.img, x, y, xsz, ysz);
            }
            SpriteImg.Image = PreviewSprite;
        }

        private void SpriteCaptureButton_Click(object sender, EventArgs e)
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

        //удаление спрайта
        private void SpriteDeleteButton_Click(object sender, EventArgs e)
        {
            if (CurrentSprite != null)
            {
                //ищем, используется ли этот спрайт в каких-либо экранах
                string scr_name = "";
                foreach (Screen scr in Screens)
                {
                    if (scr_name != "")
                        break;
                    foreach (ScreenSprite scr_spr in scr.sprites)
                        if (CurrentSprite.name == scr_spr.sprite.name)
                        {
                            scr_name = scr.name;
                            break;
                        }
                }
                if (scr_name != "")
                {
                    MessageBox.Show("This sprite used in screen '" + scr_name + '"', "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Sprites.Remove(CurrentSprite);
                RefreshSpritesList();
                CalcProjectLenght();
                ProjectChanged = true;
            }
        }

        //начало переименования спрайта
        private void SpriteRenameButton_Click(object sender, EventArgs e)
        {
            if (CurrentSprite == null)
                return;
            SpritesList.CurrentCell = SpritesList.Rows[SpritesList.CurrentRow.Index].Cells[0];
            SpritesList.BeginEdit(true);
        }

        //завершение переименования спрайта
        private void SpritesList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string new_name = SpritesList[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (new_name == CurrentSprite.name)
                return;
            if (FindSprite(new_name) != null)
            {
                MessageBox.Show("Sprite with same name already exsist!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SpritesList[e.ColumnIndex, e.RowIndex].Value = CurrentSprite.name;
            }
            else
            {
                CurrentSprite.name = new_name;
                RefreshScreensList();
                ProjectChanged = true;
            }
        }

        //перемещение спрайта вверх по списку
        private void SpritesUpButton_Click(object sender, EventArgs e)
        {
            if (CurrentSprite == null)
                return;
            int i = SpritesList.CurrentRow.Index;
            if (i == 0)
                return;
            Sprites.Remove(CurrentSprite);
            Sprites.Insert(i - 1, CurrentSprite);
            SpritesList.CurrentCell = SpritesList.Rows[i - 1].Cells[0];
            SpritesList.CurrentCell.Selected = true;
            RefreshSpritesList();
            ProjectChanged = true;
        }

        //перемещение спрайта вниз по списку
        private void SpritesDownButton_Click(object sender, EventArgs e)
        {
            if (CurrentSprite == null)
                return;
            int i = SpritesList.CurrentRow.Index;
            if (i >= SpritesList.RowCount - 1)
                return;
            Sprites.Remove(CurrentSprite);
            Sprites.Insert(i + 1, CurrentSprite);
            SpritesList.CurrentCell = SpritesList.Rows[i + 1].Cells[0];
            SpritesList.CurrentCell.Selected = true;
            RefreshSpritesList();
            ProjectChanged = true;
        }

        //добавление текущего спрайта в список экранных спрайтов
        private void SpritesList_DoubleClick(object sender, EventArgs e)
        {
            AddScreenSprite();
        }

        //------------------------------------------------ экранные спрайты -------------------------------------------------

        ScreenSprite CurrentScreenSprite = null;
        int LastXCord;
        int LastYCord;
        int XOffset;
        int YOffset;
        bool DragMode;

        //изменение объекта текущего экранного спрайта
        private void ScreenSpriteList_CurrentCellChanged(object sender, EventArgs e)
        {
            CurrentScreenSprite = null;
            if (ScreenSpriteList.CurrentCell != null && CurrentScreen != null && CurrentScreen.visible)
            {
                CurrentScreenSprite = CurrentScreen.sprites[ScreenSpriteList.CurrentCell.RowIndex];
            }
            ShowScreenPreview();
        }

        //добавление нового экранного спрайта
        void AddScreenSprite()
        {
            if (CurrentScreen == null)
            {
                MessageBox.Show("Select screen!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CurrentSprite == null)
            {
                MessageBox.Show("Select sprite!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ScreenSprite scr_spr = new ScreenSprite();
            scr_spr.sprite = CurrentSprite;
            scr_spr.x_cord = 16 - CurrentSprite.width / 2;
            scr_spr.y_cord = 12 - CurrentSprite.height / 2;
            CurrentScreen.sprites.Add(scr_spr);
            RefreshScreenSpriteList();
            ScreenSpriteList.CurrentCell = ScreenSpriteList.Rows[ScreenSpriteList.RowCount - 1].Cells[0]; ;
            ScreenSpriteList.CurrentCell.Selected = true;
            CalcProjectLenght();
            ProjectChanged = true;
        }

        //обновление списка экранных спрайтов
        void RefreshScreenSpriteList()
        {
            int i = -1;
            if (ScreenSpriteList.CurrentRow != null)
                i = ScreenSpriteList.CurrentRow.Index;
            ScreenSpriteList.Rows.Clear();
            foreach (ScreenSprite s in CurrentScreen.sprites)
            {
                ScreenSpriteList.Rows.Add(s.sprite.name, s.x_cord, s.y_cord, s.attr);
            }

            if (i >= ScreenSpriteList.RowCount)
                i = ScreenSpriteList.RowCount - 1;
            if (i >= 0)
            {
                ScreenSpriteList.CurrentCell = ScreenSpriteList.Rows[i].Cells[0];
                ScreenSpriteList.CurrentCell.Selected = true;
            }
            ShowScreenPreview();
        }

        //добавление текущего спрайта в список экранных спрайтов
        private void ScreenSpriteAddButton_Click(object sender, EventArgs e)
        {
            AddScreenSprite();
        }

        //удаление экранного спрайта
        private void ScreenSpriteDeleteButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreenSprite != null)
            {
                CurrentScreen.sprites.Remove(CurrentScreenSprite);
                RefreshScreenSpriteList();
                CalcProjectLenght();
                ProjectChanged = true;
            }
        }

        //перемещение экраного спрайта вверх по списку
        private void ScreenSpriteUpButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreenSprite == null)
                return;
            int i = ScreenSpriteList.CurrentRow.Index;
            if (i == 0)
                return;
            CurrentScreen.sprites.Remove(CurrentScreenSprite);
            CurrentScreen.sprites.Insert(i - 1, CurrentScreenSprite);
            ScreenSpriteList.CurrentCell = ScreenSpriteList.Rows[i - 1].Cells[0];
            ScreenSpriteList.CurrentCell.Selected = true;
            RefreshScreenSpriteList();
            ProjectChanged = true;
        }

        //перемещение экраного спрайта вниз по списку
        private void ScreenSpriteDownButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreenSprite == null)
                return;
            int i = ScreenSpriteList.CurrentRow.Index;
            if (i >= ScreenSpriteList.RowCount - 1)
                return;
            CurrentScreen.sprites.Remove(CurrentScreenSprite);
            CurrentScreen.sprites.Insert(i + 1, CurrentScreenSprite);
            ScreenSpriteList.CurrentCell = ScreenSpriteList.Rows[i + 1].Cells[0];
            ScreenSpriteList.CurrentCell.Selected = true;
            RefreshScreenSpriteList();
            ProjectChanged = true;
        }

        //редактирование свойств экранного спрайта
        private void ScreenSpriteList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int x = Int32.Parse(ScreenSpriteList[1, e.RowIndex].Value.ToString());
                int y = Int32.Parse(ScreenSpriteList[2, e.RowIndex].Value.ToString());
                int a = Int32.Parse(ScreenSpriteList[3, e.RowIndex].Value.ToString());
                if (x < 0)
                {
                    x = 0;
                    ScreenSpriteList[1, e.RowIndex].Value = x;
                }
                if (x + CurrentScreenSprite.sprite.width - 1 > 31)
                {
                    x = 32 - CurrentScreenSprite.sprite.width;
                    ScreenSpriteList[1, e.RowIndex].Value = x;
                }
                if (y < 0)
                {
                    y = 0;
                    ScreenSpriteList[2, e.RowIndex].Value = y;
                }
                if (y + CurrentScreenSprite.sprite.height - 1 > 23)
                {
                    y = 24 - CurrentScreenSprite.sprite.height;
                    ScreenSpriteList[2, e.RowIndex].Value = y;
                }
                CurrentScreenSprite.x_cord = x;
                CurrentScreenSprite.y_cord = y;
                CurrentScreenSprite.attr = a;
                ShowScreenPreview();
                ProjectChanged = true;
            }
            catch
            {
                ScreenSpriteList[1, e.RowIndex].Value = CurrentScreenSprite.x_cord;
                ScreenSpriteList[2, e.RowIndex].Value = CurrentScreenSprite.y_cord;
                ScreenSpriteList[3, e.RowIndex].Value = CurrentScreenSprite.attr;
            }
        }
 
        //----------------------------------------------------- экраны -----------------------------------------------------

        Screen CurrentScreen = null;

        //изменение объекта текущего экрана
        private void ScreensList_CurrentCellChanged(object sender, EventArgs e)
        {
            CurrentScreen = null;
            if (ScreensList.CurrentCell == null)
            {
                ScreenSpriteList.Rows.Clear();
                return;
            }
            CurrentScreen = Screens[ScreensList.CurrentCell.RowIndex];
            RefreshScreenSpriteList();
        }

        //добавление нового экрана
        string DefaultScreenName = "";
        private void ScreensAddButton_Click(object sender, EventArgs e)
        {
            InputBox ib = new InputBox("Enter screen name", DefaultScreenName);
            ib.ShowDialog();
            if (ib.ResultString != "")
            {
                if (FindScreen(ib.ResultString) != null)
                {
                    MessageBox.Show("Screen with the same name already exists!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Screen scr = new Screen();
                    scr.name = ib.ResultString;
                    Screens.Add(scr);
                    DefaultScreenName = scr.name;
                    RefreshScreensList();
                    ScreensList.CurrentCell = ScreensList.Rows[ScreensList.RowCount - 1].Cells[0];
                    ScreensList.CurrentCell.Selected = true;
                    CalcProjectLenght();
                    ProjectChanged = true;
                }
            }
        }

        //обновление списка экранов
        public void RefreshScreensList()
        {
            int i = -1;
            if (ScreensList.CurrentRow != null)
                i = ScreensList.CurrentRow.Index;
            ScreensList.Rows.Clear();
            foreach (Screen s in Screens)
            {
                ScreensList.Rows.Add(s.name, s.visible);
            }

            if (i >= ScreensList.RowCount)
                i = ScreensList.RowCount - 1;
            if (i >= 0)
            {
                ScreensList.CurrentCell = ScreensList.Rows[i].Cells[0];
                ScreensList.CurrentCell.Selected = true;
            }
            ViewCheckScreens();
            ShowScreenPreview();
        }

        //поиск экрана по имени в списке Screens
        Screen FindScreen(string name)
        {
            foreach (Screen s in Screens)
            {
                if (s.name == name)
                    return s;
            }
            return null;
        }

        //удаление экрана
        private void ScreensDeleteButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreen != null)
            {
                if (CurrentScreen.sprites.Count > 0)
                {
                    if (MessageBox.Show("Are you sure?", "This screen has attached sprites.", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                        return;
                }
                Screens.Remove(CurrentScreen);
                RefreshScreensList();
                CalcProjectLenght();
                ProjectChanged = true;
            }
        }

        //начало переименования экрана
        private void ScreenRenameButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreen == null)
                return;
            ScreensList.CurrentCell = ScreensList.Rows[ScreensList.CurrentRow.Index].Cells[0];
            ScreensList.BeginEdit(true);
        }

        //завершение переименования экрана
        private void ScreensList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string new_name = ScreensList[0, e.RowIndex].Value.ToString();
            if (new_name == CurrentScreen.name)
                return;
            if (FindScreen(new_name) != null)
            {
                MessageBox.Show("Screen with same name already exsist!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ScreensList[0, e.RowIndex].Value = CurrentScreen.name;
            }
            else
            {
                CurrentScreen.name = new_name;
                ProjectChanged = true;
            }
        }

        //перемещение экрана вверх по списку
        private void ScreensUpButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreen == null)
                return;
            int i = ScreensList.CurrentRow.Index;
            if (i == 0)
                return;
            Screens.Remove(CurrentScreen);
            Screens.Insert(i - 1, CurrentScreen);
            ScreensList.CurrentCell = ScreensList.Rows[i - 1].Cells[0];
            ScreensList.CurrentCell.Selected = true;
            RefreshScreensList();
            ProjectChanged = true;
        }

        //перемещение экрана вниз по списку
        private void ScreensDownButton_Click(object sender, EventArgs e)
        {
            if (CurrentScreen == null)
                return;
            int i = ScreensList.CurrentRow.Index;
            if (i >= ScreensList.RowCount - 1)
                return;
            Screens.Remove(CurrentScreen);
            Screens.Insert(i + 1, CurrentScreen);
            ScreensList.CurrentCell = ScreensList.Rows[i + 1].Cells[0];
            ScreensList.CurrentCell.Selected = true;
            RefreshScreensList();
            ProjectChanged = true;
        }

        //показать превью экрана
        public void ShowScreenPreview()
        {
            Graphics g = Graphics.FromImage(PreviewScreen);
            if (Screens.Count == 0)
            {
                g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, PreviewScreen.Width, PreviewScreen.Height);
            }
            else
            {
                g.FillRectangle(new SolidBrush(Color.Black), 0, 0, 512, 384);
                foreach (Screen scr in Screens)
                    if (scr.visible)
                        foreach (ScreenSprite scr_spr in scr.sprites)
                        {
                            int master_attr = scr_spr.attr;
                            int x_cord = scr_spr.x_cord;
                            int y_cord = scr_spr.y_cord;
                            for (int y = 0; y < scr_spr.sprite.height; y++)
                                for (int x = 0; x < scr_spr.sprite.width; x++)
                                    DrawChar(g, x_cord + x, y_cord + y, scr_spr.sprite.data[x, y], master_attr);
                        }
                if (CurrentScreenSprite != null)
                {
                    Pen p = new Pen(Color.White, 2);
                    p.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(p, CurrentScreenSprite.x_cord * 16, CurrentScreenSprite.y_cord * 16, CurrentScreenSprite.sprite.width * 16, CurrentScreenSprite.sprite.height * 16);
                }
            }
            ScreenImg.Image = PreviewScreen;
        }

        //чекбокс видимости экрана
        private void ScreensList_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentScreen != null && ScreensList.CurrentCell.ColumnIndex == 1)
            {
                CurrentScreen.visible = (bool)ScreensList[1, ScreensList.CurrentRow.Index].EditedFormattedValue;
                if (!CurrentScreen.visible)
                    CurrentScreenSprite = null;
                else
                    RefreshScreenSpriteList();
                ShowScreenPreview();
                ViewCheckScreens();
                ProjectChanged = true;
            }
        }

        private void ScreenImg_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / 16;
            int y = e.Y / 16;
            if (DragMode)
            {
                x -= XOffset;
                y -= YOffset;
                if (x < 0)
                    x = 0;
                if (x + CurrentScreenSprite.sprite.width - 1 > 31)
                    x = 32 - CurrentScreenSprite.sprite.width;
                if (y < 0)
                    y = 0;
                if (y + CurrentScreenSprite.sprite.height - 1 > 23)
                    y = 24 - CurrentScreenSprite.sprite.height;
            }
            StatusXCord.Text = x.ToString();
            StatusYCord.Text = y.ToString();
            if (DragMode && (x != LastXCord || y != LastYCord))
            {
                LastXCord = x;
                LastYCord = y;
                CurrentScreenSprite.x_cord = x;
                CurrentScreenSprite.y_cord = y;
                StatusXCord.Text = x.ToString();
                StatusYCord.Text = y.ToString();
                ShowScreenPreview();
            }
        }

        private void ScreenImg_MouseLeave(object sender, EventArgs e)
        {
            StatusXCord.Text = "";
            StatusYCord.Text = "";
        }

        private void ScreenImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentScreenSprite != null)
            {
                int x = e.X / 16;
                int y = e.Y / 16;
                //проверка выбора текущего экранного спрайта
                if (x >= CurrentScreenSprite.x_cord && x < CurrentScreenSprite.x_cord + CurrentScreenSprite.sprite.width && y >= CurrentScreenSprite.y_cord && y < CurrentScreenSprite.y_cord + CurrentScreenSprite.sprite.height)
                {
                    LastXCord = x;
                    LastYCord = y;
                    XOffset = x - CurrentScreenSprite.x_cord;
                    YOffset = y - CurrentScreenSprite.y_cord;
                    DragMode = true;
                }
                else
                {
                    //поиск подходящего экранного спрайта
                    for (int i = Screens.Count - 1; i >= 0; i--)
                        for (int j = Screens[i].sprites.Count - 1; j >= 0; j--)
                            if (Screens[i].visible)
                            {
                                ScreenSprite scr_spr = Screens[i].sprites[j];
                                if (x >= scr_spr.x_cord && x < scr_spr.x_cord + scr_spr.sprite.width && y >= scr_spr.y_cord && y < scr_spr.y_cord + scr_spr.sprite.height)
                                {
                                    ScreensList.CurrentCell = ScreensList.Rows[i].Cells[0];
                                    ScreensList.CurrentCell.Selected = true;
                                    ScreenSpriteList.CurrentCell = ScreenSpriteList.Rows[j].Cells[0];
                                    ScreenSpriteList.CurrentCell.Selected = true;
                                    LastXCord = x;
                                    LastYCord = y;
                                    XOffset = x - CurrentScreenSprite.x_cord;
                                    YOffset = y - CurrentScreenSprite.y_cord;
                                    DragMode = true;
                                    return;
                                }
                            }
                    DragMode = false;
                }
            }
        }

        private void ScreenImg_MouseUp(object sender, MouseEventArgs e)
        {
            if (DragMode)
            {
                RefreshScreenSpriteList();
                ProjectChanged = true;
                DragMode = false;
            }
        }

        bool NoCheckScreens = false;

        //установка статуса чекбокса видимости экранов
        void ViewCheckScreens()
        {
            NoCheckScreens = true;
            int i = 0;
            foreach (Screen scr in Screens)
                if (scr.visible)
                    i++;
            if (i == 0)
                CheckScreens.CheckState = CheckState.Unchecked;
            else if (i == Screens.Count)
                CheckScreens.CheckState = CheckState.Checked;
            else
                CheckScreens.CheckState = CheckState.Indeterminate;
            NoCheckScreens = false;
        }

        //общее переключение видимости экранов
        private void CheckScreens_CheckedChanged(object sender, EventArgs e)
        {
            if (NoCheckScreens)
                return;
            foreach (Screen scr in Screens)
                scr.visible = CheckScreens.Checked;
            RefreshScreensList();
            ProjectChanged = true;
        }
        
    }
}
