using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using ZXProc;
using System.Diagnostics;

namespace WorldEditor
{
    public partial class LocationEditorForm : Form
    {
        Bitmap[] TilesImg16 = new Bitmap[256]; //массив изображений тайлов 16x16
        Bitmap[] TilesImg32 = new Bitmap[256]; //массив изображений тайлов 32x32
        static public byte[,] Tileset = new byte[256, 37]; //массив бинарников тайлов
        Point TilesCord; //начальные координаты выделенной области в шаблоне тайлов
        static public Rectangle TilesSelectedRect = new Rectangle(0, 0, 1, 1); //координаты и размер выделенной области в шаблоне тайлов
        byte[,] TilesSelected = new byte[1, 1]; //массив номеров тайлов в выделенной области в шаблоне тайлов
        Bitmap TilesetImg = new Bitmap(512, 512); //изображение шаблона тайлов
        int MaxTilesInTileset = 255; //максимально возможное кол-во тайлов в тайлсете

        byte[,] Loc = new byte[256, 256]; //массив локации
        List<byte[,]> LocUndo = new List<byte[,]>(); //буфер отмены действий
        int LocUndoIndex; //текущий индекс в буфере отмены действий
        Bitmap LocImg; //изображение превью локации для LocPicture
        Point LastCord = new Point(-1, -1); //предыдущие координаты в превью локации
        int LocScale = 32; //размер (масштаб) тайлов в превью локации
        Size LocViewSize = new Size(16, 11); //размеры окна локации, выводимые движком
        Point LocCord = new Point(-1, -1); //текущие координаты в превью локации
        Point LocSelCord; //координаты начала выделения области в превью локации
        Point LocDrawHome = new Point(); //координата окна локации на LocPicture
        Point LocDrawCord = new Point(); //координата начала отрисовки локации на LocPicture
        Size LocEditSize = new Size(); //размеры окна редактирования
        int OneHScroll = 12; //смещение за раз по горизонтали
        int OneVScroll = 5; //смещение за раз по вертикали

        string LocationsPath = @"C:\YandexDisk\Mighty Final Fight\res\locations"; //путь к папке с локациями
        string TilesetPath = @"C:\YandexDisk\Mighty Final Fight\res\tileset"; //путь к папке с тайсетами
        string BinPath = @"C:\YandexDisk\Mighty Final Fight\bin"; //путь к папке с бинарниками
        string SourcePath = @"C:\YandexDisk\Mighty Final Fight\source"; //путь к папке с исходниками на ассемблере
        string ObjectsPath = @"C:\YandexDisk\Mighty Final Fight\res\objects"; //путь к папке с объектами
        string CurrentLocName = "New location"; //текущее имя локации
        string CurrentTilesetName = "New tileset"; //текущее имя тайлсета
        bool LocChanged = false; //признак изменения локации
        static public bool TilesetChanged = false; //признак изменения тайлсета
        bool ObjectsChanged = false; //признак изменения объектов
        CaptureFromImgForm CaptureForm; //форма захвата тайлов из внешнего источника

        static public bool ExtInsertMode = false; //режим вставки тайлов из внешнего источника
        static public bool MoveTilesMode = false; //режим перемещения тайлов в тайлсете
        static public bool CopyTilesMode = false; //режим копирования тайлов тайлсете
        static public byte[,][] TilesBuffer;
        static public Bitmap[,] TilesBufferImg;
        Point PosMoveTiles; //начальная позиция перемещаемых тайлов, для отмены перемещения

        bool isEmptyTiles; //указатель, что выбранная область не содержит тайлов
        PropertyForm PropertyEditorForm; //форма свойств тайла
        TilesetEditorForm TEditorForm; //форма редактирования тайла
        int[] TimerCounters = new int[16]; //таймеры для анимации тайлов
        int[] TimerTiles = new int[16]; //номера тайлов для анимации
        int[] RndTimerCounters = new int[16]; //таймеры для случайной анимации тайлов
        int[] RndTimerTiles = new int[16]; //номера тайлов для случайной анимации
        Random rnd = new Random(); //генератор случайных чисел

        string LaunchProc = @"C:\YandexDisk\Mighty Final Fight\run.lnk"; //путь к запускаемой программе при нажатии на Assembly Location

        //--------------------------------- Общие подпрограммы ------------------------------------

        //расчет длинны тайлов и локации
        void CalcLength()
        {
            int tiles_count = 1;
            for (int i = 1; i < 256; i++)
                if (!IsEmptyTile(Tileset, i))
                    tiles_count++;
            TilesetLength.Text = (tiles_count * 38 + 256).ToString();
            LocLength.Text = ((int)(LocSizeX.Value * LocSizeY.Value + 2)).ToString();
            TilesetCnt.Text = tiles_count + " tiles of " + MaxTilesInTileset.ToString();
            AssemblyButton.Enabled = tiles_count <= MaxTilesInTileset;
        }

        //класс сохранения настроек
        public class Settings
        {
            public string LocationsPath;
            public string CurrentLocName;
            public string TilesetPath;
            public string CurrentTilesetName;
        }

        //сохранение настроек
        void SaveSetting()
        {
            Settings iniSet = new Settings();
            iniSet.LocationsPath = LocationsPath;
            iniSet.CurrentLocName = CurrentLocName;
            iniSet.TilesetPath = TilesetPath;
            iniSet.CurrentTilesetName = CurrentTilesetName;
            using (Stream writer = new FileStream("setup.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, iniSet);
            }
        }

        //---------------------------------- Работа с локацией ------------------------------------

        //чтение каталога с локациями
        void ReadLocDir()
        {
            LocName.Items.Clear();
            LocName.Items.Add("New location");

            string[] files = Directory.GetFiles(LocationsPath, "*.bin");

            foreach (string file in files)
                LocName.Items.Add(Path.GetFileNameWithoutExtension(file));
            LocName.SelectedIndex = LocName.FindString(CurrentLocName);
        }

        //загрузка локации
        void LoadLocation()
        {
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                    Loc[x, y] = 0;
            if (LocName.SelectedIndex == 0)
            {
                LocSizeX.Value = 16;
                LocSizeY.Value = 11;
            }
            else
            {
                BinaryReader f = new BinaryReader(File.Open(LocationsPath + @"/" + LocName.Text + ".bin", FileMode.Open));
                LocSizeX.Value = f.ReadByte() + 1;
                LocSizeY.Value = f.ReadByte() + 1;
                for (int y = 0; y < LocSizeY.Value; y++)
                    for (int x = 0; x < LocSizeX.Value; x++)
                        Loc[x, y] = f.ReadByte();

                //загрузка информации об объектах локации
                LocObjects.Clear();
                if (f.PeekChar() > -1)
                {
                    int screens = f.ReadByte();
                    for (int i = 0; i < screens; i++)
                    {
                        ScreenObjects scr_obj = new ScreenObjects();
                        scr_obj.ObjectsOnScreen = f.ReadByte();
                        int obj_num = f.ReadByte();
                        for (int j = 0; j < obj_num; j++)
                        {
                            ObjectData obj = new ObjectData();
                            obj.Name = f.ReadString();
                            obj.XCord = f.ReadByte();
                            obj.YCord = f.ReadByte();
                            obj.YOffset = f.ReadByte();
                            obj.Direct = f.ReadBoolean();
                            scr_obj.Objects.Add(obj);
                        }
                        LocObjects.Add(scr_obj);
                    }
                }
                else
                {
                    int num = ((int)LocSizeX.Value - (16 - OneHScroll)) / OneHScroll;
                    ScreenObjects scr_obj = new ScreenObjects();
                    scr_obj.ObjectsOnScreen = 2;
                    for (int i = 0; i < num; i++)
                        LocObjects.Add(scr_obj);
                }

                f.Close();
            }
            CurrentLocName = LocName.Text;
            ShowLocation();
            LoadObjectList(XScroll.Value);

            //перезапускаем буфер Undo для новой локации
            ClearUndoBuffer();
            CalcLength();
            LocChanged = false;
        }

        //сохранение локации
        private void SaveLoc()
        {
            string n = LocName.Text;

            if (LocName.SelectedIndex == 0)
            {
                InputBox i = new InputBox("Input location name", "");

                if (i.ShowDialog() == DialogResult.OK)
                {
                    n = i.ResultString;
                    if (n == "")
                        return;
                }
                else
                    return;
                i.Dispose();
            }

            BinaryWriter f = new BinaryWriter(File.Create(LocationsPath + @"/" + n + ".bin"));
            f.Write((byte)(LocSizeX.Value - 1));
            f.Write((byte)(LocSizeY.Value - 1));
            for (int y = 0; y < LocSizeY.Value; y++)
                for (int x = 0; x < LocSizeX.Value; x++)
                    f.Write(Loc[x, y]);

            //сохранение информации об объектах локации
            f.Write((byte)(LocObjects.Count));
            foreach (ScreenObjects screen_obj in LocObjects)
            {
                f.Write((byte)(screen_obj.ObjectsOnScreen));
                f.Write((byte)(screen_obj.Objects.Count));
                foreach (ObjectData data in screen_obj.Objects)
                {
                    f.Write(data.Name);
                    f.Write(data.XCord);
                    f.Write(data.YCord);
                    f.Write(data.YOffset);
                    f.Write(data.Direct);
                }
            }

            f.Close();
            CurrentLocName = n;
            ReadLocDir();
            LocChanged = false;
        }

        //сборка локации
        void LocAssembly()
        {

            //создаём заготовки тайлсетов и локаций, создаём список уровней
            string[] tilesets = Directory.GetFiles(TilesetPath, "*-*.bin");
            List<string> levelnames = new List<string>();
            string lastname = "";
            string name = "";
            foreach (string tileset in tilesets)
            {
                BinaryWriter ft = new BinaryWriter(File.Create(TilesetPath + @"/" + Path.GetFileNameWithoutExtension(tileset) + ".tiles"));
                BinaryWriter fl = new BinaryWriter(File.Create(LocationsPath + @"/" + Path.GetFileNameWithoutExtension(tileset) + ".loc"));
                BinaryReader fot = new BinaryReader(File.Open(TilesetPath + @"/" + Path.GetFileNameWithoutExtension(tileset) + ".bin", FileMode.Open));
                BinaryReader fol = new BinaryReader(File.Open(LocationsPath + @"/" + Path.GetFileNameWithoutExtension(tileset) + ".bin", FileMode.Open));

                int xsz = fol.ReadByte() + 1;
                int ysz = fol.ReadByte();
                byte[,] loc = new byte[xsz, ysz];
                for (int j = 0; j < ysz; j++)
                    for (int i = 0; i < xsz; i++)
                        loc[i, j] = fol.ReadByte();

                for (int i = 0; i < xsz; i++)
                    fol.ReadByte();

                //чтение информации об объектах
                List<byte> ObjInfo = new List<byte>();
                if (fol.PeekChar() > -1)
                {
                    int n = fol.ReadByte();
                    for (int i = 0; i < n; i++)
                    {
                        ObjInfo.Add(fol.ReadByte());
                        int no = fol.ReadByte();
                        for (int j = 0; j < no; j++)
                        {
                            string objname = fol.ReadString();
                            byte xcord = fol.ReadByte();
                            byte ycord = fol.ReadByte();
                            byte yoffset = fol.ReadByte();
                            byte direct = 0;
                            if (fol.ReadBoolean())
                                direct = 1;
                            if (objname == "none")
                                ObjInfo.Add(0);
                            else
                                foreach (ObjectInfo obj in ObjectsInfo)
                                {
                                    if (obj.Name == objname)
                                    {
                                        ObjInfo.Add(obj.Id);
                                        ObjInfo.Add(xcord);
                                        ObjInfo.Add(ycord);
                                        ObjInfo.Add(yoffset);
                                        ObjInfo.Add(obj.Animation);
                                        ObjInfo.Add((byte)(direct + obj.Status));
                                    }
                                }
                        }
                        ObjInfo.Add(255);
                    }
                }


                byte[,] tiles = new byte[256, 37];
                for (int i = 0; i < 256; i++)
                    for (int j = 0; j < 37; j++)
                        tiles[i, j] = fot.ReadByte();

                byte num = 1;
                byte[] tiles_num = new byte[256];
                for (int i = 1; i < 256; i++)
                    if (!IsEmptyTile(tiles, i))
                    {
                        tiles_num[i] = num;
                        num++;
                    }

                fl.Write((byte)xsz);
                fl.Write((byte)ysz);
                for (int j = 0; j < ysz; j++)
                    for (int i = 0; i < xsz; i++)
                        fl.Write(tiles_num[loc[i, j]]);
                foreach (byte b in ObjInfo)
                    fl.Write(b);

                ft.Write(tiles[0, 36]);
                for (int i = 1; i < 256; i++)
                    if (tiles_num[i] != 0)
                        ft.Write(tiles[i, 36]);
                for (int i = 0; i < 256 - num; i++)
                    ft.Write((byte)0);

                ft.Write((byte)0);
                ft.Write((byte)0);
                for (int j = 0; j < 36; j++)
                    ft.Write(tiles[0, j]);
                for (int i = 1; i < 256; i++)
                {
                    if (tiles_num[i] != 0)
                    {
                        ft.Write((byte)0);
                        ft.Write((byte)0);
                        for (int j = 0; j < 36; j++)
                            ft.Write(tiles[i, j]);
                    }
                }

                ft.Close();
                fl.Close();
                fot.Close();
                fol.Close();

                name = Path.GetFileNameWithoutExtension(tileset).Split('-')[0];
                if (name != lastname)
                {
                    lastname = name;
                    levelnames.Add(name);
                }
            }
            if (name != lastname)
            {
                lastname = name;
                levelnames.Add(name);
            }

            //создаём архивы тайлсетов
            tilesets = Directory.GetFiles(TilesetPath, "*.tiles");
            string[] locations = Directory.GetFiles(LocationsPath, "*.loc");
            foreach (string tileset in tilesets)
                if (!tileset.Contains("0"))
                    Process.Start(@"..\..\..\..\..\MegaLZ\MegaLZ.exe", '"' + tileset + '"');

            System.Threading.Thread.Sleep(2000);

            //создаём сборки тайлсетов для уровней
            foreach (string levelname in levelnames)
            {
                BinaryWriter ft = new BinaryWriter(File.Create(BinPath + @"/tileset_" + levelname + ".bin"));
                tilesets = Directory.GetFiles(TilesetPath, levelname + "-*.mlz");
                long offset = tilesets.Count() * 2;
                foreach (string tileset in tilesets)
                {
                    ft.Write((byte)(offset & 255));
                    ft.Write((byte)((offset >> 8) & 255));
                    FileInfo file = new FileInfo(tileset);
                    offset += file.Length;
                }
                foreach (string tileset in tilesets)
                {
                    BinaryReader fot = new BinaryReader(File.Open(tileset, FileMode.Open), Encoding.ASCII);
                    while (fot.PeekChar() > -1)
                    {
                        byte b = fot.ReadByte();
                        ft.Write(b);
                    }
                    fot.Close();
                }
                ft.Close();
            }

            //создаём сборки локаций для уровней
            foreach (string levelname in levelnames)
            {
                BinaryWriter fl = new BinaryWriter(File.Create(BinPath + @"/location_" + levelname + ".bin"));
                locations = Directory.GetFiles(LocationsPath, levelname + "-*.loc");
                long offset = locations.Count() * 2;
                foreach (string location in locations)
                {
                    fl.Write((byte)(offset & 255));
                    fl.Write((byte)((offset >> 8) & 255));
                    FileInfo file = new FileInfo(location);
                    offset += file.Length;
                }
                foreach (string location in locations)
                {
                    BinaryReader fol = new BinaryReader(File.Open(location, FileMode.Open), Encoding.ASCII);
                    while (fol.PeekChar() > -1)
                    {
                        byte b = fol.ReadByte();
                        fl.Write(b);
                    }
                    fol.Close();
                }
                fl.Close();
            }

            //удаляем временные файлы *.loc, *.tiles и *.mlz
            string[] temps = Directory.GetFiles(TilesetPath, "*.tiles");
            foreach (string temp in temps)
                if (!temp.Contains("0"))
                {
                    File.Delete(temp);
                }
            temps = Directory.GetFiles(TilesetPath, "*.mlz");
            foreach (string temp in temps)
            {
                File.Delete(temp);
            }
            temps = Directory.GetFiles(LocationsPath, "*.loc");
            foreach (string temp in temps)
            {
                File.Delete(temp);
            }

            //создаём debug.asm
            StreamWriter debug = new StreamWriter(File.Open(SourcePath + @"/debug.asm", FileMode.Create, FileAccess.Write));
            debug.WriteLine(";debug variables, generated automatically");
            debug.WriteLine("current_level    db '" + LocName.Text[0] + "'");
            debug.WriteLine("current_loc      db " + (Int32.Parse(Convert.ToString(LocName.Text[2])) - 1).ToString());
            debug.Close();

            if (LaunchProc != "")
                Process.Start(LaunchProc);
        }

        //вывод превью локации
        void ShowLocation()
        {
            if (ExtInsertMode)
                return;

            int xoffset = XScroll.Value * OneHScroll - (LocEditSize.Width - LocViewSize.Width) / 2;
            int yoffset = YScroll.Value * OneVScroll - (LocEditSize.Height - LocViewSize.Height) / 2;

            Graphics g = Graphics.FromImage(LocImg);
            Font f = new Font("Microsoft Sans Serif", 8);
            Brush b = new SolidBrush(Color.Black);
            Brush b2 = new SolidBrush(Color.White);
            Brush b3 = new SolidBrush(Color.LightGray);
            Pen p = new Pen(Color.Gray);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            Pen p2 = new Pen(Color.Red);
            p2.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            Pen p3 = new Pen(Color.Red);

            for (int j = 0; j < LocEditSize.Height; j++)
                for (int i = 0; i < LocEditSize.Width; i++)
                {
                    int x = i + xoffset;
                    int y = j + yoffset;
                    int xdraw = LocDrawCord.X + i * LocScale;
                    int ydraw = LocDrawCord.Y + j * LocScale;
                    if (x >= 0 && x < LocSizeX.Value && y >= 0 && y < LocSizeY.Value)
                    {
                        int t = Loc[x, y];
                        if ((Tileset[t, 36] & 16) == 0 && (Tileset[t, 36] & 15) != 0)
                        {
                            t += TimerTiles[Tileset[t, 36] & 15];
                        }

                        if ((Tileset[t, 36] & 16) != 0 && (Tileset[t, 36] & 15) != 0)
                        {
                            t += RndTimerTiles[Tileset[t, 36] & 15] & 3;
                        }

                        if (LocScale == 16)
                            g.DrawImage(TilesImg16[t], xdraw, ydraw, LocScale, LocScale);
                        else
                            g.DrawImage(TilesImg32[t], xdraw, ydraw, LocScale, LocScale);
                        if (ShowLocProperty.Checked && Tileset[t, 36] != 0)
                        {
                            g.FillRectangle(b2, xdraw, ydraw, 25, 12);
                            g.DrawString(Tileset[t, 36].ToString(), f, b, xdraw, ydraw);
                        }
                        if (ShowLocGrid.Checked)
                        {
                            g.DrawLine(p, xdraw, ydraw, xdraw + LocScale, ydraw);
                            g.DrawLine(p, xdraw, ydraw, xdraw, ydraw + LocScale);
                        }
                    }
                    else
                        g.FillRectangle(b3, xdraw, ydraw, LocScale, LocScale);
                }
            g.DrawRectangle(p2, LocDrawHome.X, LocDrawHome.Y, LocViewSize.Width * LocScale - 1, LocViewSize.Height * LocScale - 1);


            //прорисовка выделенного блока тайлов
            if (LocCord.X != -1)
            {
                for (int j = 0; j < TilesSelectedRect.Height; j++)
                    for (int i = 0; i < TilesSelectedRect.Width; i++)
                    {
                        if (SetNullTiles.Checked || !IsEmptyTile(Tileset, TilesSelected[i, j]))
                        {
                            int xdraw = (i + LocCord.X - XScroll.Value * OneHScroll) * LocScale + LocDrawHome.X;
                            int ydraw = (j + LocCord.Y - YScroll.Value * OneVScroll) * LocScale + LocDrawHome.Y;
                            if (LocScale == 16)
                                g.DrawImage(TilesImg16[TilesSelected[i, j]], xdraw, ydraw, LocScale, LocScale);
                            else
                                g.DrawImage(TilesImg32[TilesSelected[i, j]], xdraw, ydraw, LocScale, LocScale);
                        }
                    }
                g.DrawRectangle(p3, (LocCord.X - XScroll.Value * OneHScroll) * LocScale + LocDrawHome.X, (LocCord.Y - YScroll.Value * OneVScroll) * LocScale + LocDrawHome.Y, TilesSelectedRect.Width * LocScale - 1, TilesSelectedRect.Height * LocScale - 1);
            }

            //прорисовка объектов
            if (ShowObjects.Checked && ObjectsList.RowCount > 0)
            {
                for (int i = 0; i < ObjectsList.RowCount; i++)
                {
                    if (ObjectsList[0, i].Value != null && ObjectsList[0, i].Value.ToString() != "none")
                    {
                        Bitmap img = ObjectsImg[FindObject(ObjectsList[0, i].Value.ToString())];
                        if (ObjectsList[4, i].Value != null && (bool)(ObjectsList[4, i].Value))
                            img = ObjectsImgMirror[FindObject(ObjectsList[0, i].Value.ToString())];
                        int xsz = img.Width * LocScale / 16;
                        int ysz = img.Height * LocScale / 16;
                        int x = Int32.Parse(ObjectsList[1, i].Value.ToString()) * LocScale / 16 + LocDrawHome.X - xsz / 2;
                        int y = (Int32.Parse(ObjectsList[2, i].Value.ToString()) - Int32.Parse(ObjectsList[3, i].Value.ToString())) * LocScale / 16 + LocDrawHome.Y - ysz;
                        g.DrawImage(img, x, y, xsz, ysz);
                        if (ObjectsList.CurrentRow != null && ObjectsList.CurrentRow.Index == i)
                            g.DrawRectangle(new Pen(Color.Red), x, y, xsz, ysz);
                    }
                }
                if (ObjectsList.CurrentRow != null && ObjectsList[0, ObjectsList.CurrentRow.Index].Value.ToString() != "none")
                {
                    int i = ObjectsList.CurrentRow.Index;
                    Bitmap img = ObjectsImg[FindObject(ObjectsList[0, i].Value.ToString())];
                    if (ObjectsList[4, i].Value != null && (bool)(ObjectsList[4, i].Value))
                        img = ObjectsImgMirror[FindObject(ObjectsList[0, i].Value.ToString())];
                    int xsz = img.Width * LocScale / 16;
                    int ysz = img.Height * LocScale / 16;
                    int x = Int32.Parse(ObjectsList[1, i].Value.ToString()) * LocScale / 16 + LocDrawHome.X - xsz / 2;
                    int y = (Int32.Parse(ObjectsList[2, i].Value.ToString()) - Int32.Parse(ObjectsList[3, i].Value.ToString())) * LocScale / 16 + LocDrawHome.Y - ysz;
                    g.DrawImage(img, x, y, xsz, ysz);
                    g.DrawRectangle(new Pen(Color.Red), x, y, xsz, ysz);
                }

            }

            LocPicture.Refresh();
        }

        //очистка и инициализация буфера Undo
        void ClearUndoBuffer()
        {
            LocUndo.Clear();
            LocUndoIndex = -1;
            AddLoc();
            LocChanged = true;
        }

        //добавить локацию в буфер Undo
        void AddLoc()
        {
            LocUndoIndex++;
            if (LocUndoIndex <= LocUndo.Count - 1)
                LocUndo.RemoveRange(LocUndoIndex, LocUndo.Count - LocUndoIndex);
            LocUndo.Add(new byte[256, 256]);
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                    LocUndo[LocUndoIndex][x, y] = Loc[x, y];
            if (LocUndoIndex >= 10000)
                MessageBox.Show("Recommended save the location and restart the application!", "UNDO/REDO BUFFER IS FULL!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            LocChanged = true;
        }

        //восстановить локацию
        void UndoLoc()
        {
            if (LocUndoIndex == 0)
                return;
            LocUndoIndex--;
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                    Loc[x, y] = LocUndo[LocUndoIndex][x, y];
            if (LocUndoIndex == 0)
                LocChanged = false;
        }

        //отменить восстановление локации
        void RedoLoc()
        {
            if (LocUndoIndex == LocUndo.Count - 1)
                return;
            LocUndoIndex++;
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                    Loc[x, y] = LocUndo[LocUndoIndex][x, y];
            LocChanged = true;
        }

        //настройка вывода локации
        void SetupDrawLoc()
        {
            if (this.WindowState == FormWindowState.Minimized)
                return;
            LocDrawHome.X = (LocPicture.Size.Width - LocViewSize.Width * LocScale) / 2;
            LocDrawHome.Y = (LocPicture.Size.Height - LocViewSize.Height * LocScale) / 2;
            LocEditSize.Width = (LocDrawHome.X / LocScale + 1) * 2 + LocViewSize.Width;
            LocEditSize.Height = (LocDrawHome.Y / LocScale + 1) * 2 + LocViewSize.Height;
            LocDrawCord.X = LocDrawHome.X - (LocEditSize.Width - LocViewSize.Width) / 2 * LocScale;
            LocDrawCord.Y = LocDrawHome.Y - (LocEditSize.Height - LocViewSize.Height) / 2 * LocScale;
            LocImg = new Bitmap(LocPicture.Width, LocPicture.Height);
            LocPicture.Image = LocImg;
        }

        //изменение размеров локации
        void ResizeLocation()
        {
            XScroll.Maximum = (int)((LocSizeX.Value - LocViewSize.Width) / OneHScroll);
            YScroll.Maximum = (int)((LocSizeY.Value - LocViewSize.Height) / OneVScroll);
            XScroll.Enabled = XScroll.Maximum != 0 ? true : false;
            YScroll.Enabled = YScroll.Maximum != 0 ? true : false;
            ShowLocation();
            CalcLength();
        }

        //---------------------------------- Работа с объектами -----------------------------------

        //класс объекта
        public class ObjectData
        {
            public string Name;
            public byte XCord;
            public byte YCord;
            public byte YOffset;
            public bool Direct;
        }

        //класс объектов на экране
        public class ScreenObjects
        {
            public byte ObjectsOnScreen;
            public List<ObjectData> Objects = new List<ObjectData>();
        }

        public class ObjectInfo
        {
            public string Name;
            public byte Id;
            public byte Animation;
            public byte Status;
        }

        List<ScreenObjects> LocObjects = new List<ScreenObjects>(); //список объектов локации
        List<Bitmap> ObjectsImg = new List<Bitmap>(); //список изображений объектов
        List<Bitmap> ObjectsImgMirror = new List<Bitmap>(); //список изображений объектов отражённых по горизонтали
        List<string> ObjectsName = new List<string>(); //список имён объектов
        int EditedObjScreen = 0;
        int xobj = 0;
        int yobj = 0;
        List<ObjectInfo> ObjectsInfo = new List<ObjectInfo>();

        //инициализация списков объектов
        void InitObjects()
        {
            ObjectsImg.Clear();
            ObjectsImgMirror.Clear();
            ObjectsName.Clear();
            ObjectsName.Add("none");
            string[] files = Directory.GetFiles(ObjectsPath, "*.png");
            foreach (string file in files)
            {
                Bitmap Img = new Bitmap(file);
                ObjectsImg.Add(Img);
                Bitmap Mirror = new Bitmap(Img);
                Mirror.RotateFlip(RotateFlipType.RotateNoneFlipX);
                ObjectsImgMirror.Add(Mirror);
                ObjectsName.Add(Path.GetFileNameWithoutExtension(file));
            }

            StreamReader f = new StreamReader(File.Open(ObjectsPath + @"/objects_info.txt", FileMode.Open));
            f.ReadLine();
            ObjectsInfo.Clear();
            while (!f.EndOfStream)
            {
                string[] s = f.ReadLine().Split(',');
                ObjectInfo obj = new ObjectInfo();
                obj.Name = s[0].Trim();
                obj.Id = Byte.Parse(s[1].Trim());
                obj.Animation = Byte.Parse(s[2].Trim());
                obj.Status = Byte.Parse(s[3].Trim());
                ObjectsInfo.Add(obj);
            }
            f.Close();
        }

        //загрузка списка объектов по номеру экрана
        void LoadObjectList(int num)
        {
            ObjectsOnScreenEdit.Text = LocObjects[num].ObjectsOnScreen.ToString();
            ObjectsList.Rows.Clear();
            int row = 0;
            foreach (ObjectData obj in LocObjects[num].Objects)
            {
                ObjectsList.Rows.Add();
                DataGridViewComboBoxCell cBoxCell = ObjectsList[0, row] as DataGridViewComboBoxCell;
                foreach (string name in ObjectsName)
                    cBoxCell.Items.Add(name);
                cBoxCell.Value = obj.Name;
                ObjectsList[1, row].Value = obj.XCord;
                ObjectsList[2, row].Value = obj.YCord;
                ObjectsList[3, row].Value = obj.YOffset;
                ObjectsList[4, row].Value = obj.Direct;
                row++;
            }
            EditedObjScreen = num;
            ObjectsChanged = false;
        }

        //сохранение списка объектов по номеру экрана
        void SaveObjectList(int num)
        {
            LocObjects[num].ObjectsOnScreen = Byte.Parse(ObjectsOnScreenEdit.Text);
            LocObjects[num].Objects.Clear();
            for (int i = 0; i < ObjectsList.Rows.Count; i++)
            {
                ObjectData obj = new ObjectData();
                obj.Name = ObjectsList[0, i].Value.ToString();
                obj.XCord = Byte.Parse(ObjectsList[1, i].Value.ToString());
                obj.YCord = Byte.Parse(ObjectsList[2, i].Value.ToString());
                obj.YOffset = Byte.Parse(ObjectsList[3, i].Value.ToString());
                if (ObjectsList[4, i].Value == null)
                    obj.Direct = false;
                else
                    obj.Direct = (bool)ObjectsList[4, i].Value;
                LocObjects[num].Objects.Add(obj);
            }
            ObjectsChanged = false;
        }

        //поиск индекса объекта по имени
        int FindObject(string name)
        {
            int i = 0;
            foreach (string n in ObjectsName)
            {
                if (n == name)
                    break;
                i++;
            }
            return i - 1;
        }

        //добавление нового объекта
        private void AddObjectButton_Click(object sender, EventArgs e)
        {
            ObjectsList.Rows.Add();
            DataGridViewComboBoxCell cBoxCell = ObjectsList[0, ObjectsList.RowCount - 1] as DataGridViewComboBoxCell;
            foreach (string name in ObjectsName)
                cBoxCell.Items.Add(name);
            ObjectsList[0, ObjectsList.RowCount - 1].Value = "none";
            ObjectsList[1, ObjectsList.RowCount - 1].Value = 128;
            ObjectsList[2, ObjectsList.RowCount - 1].Value = 128;
            ObjectsList[3, ObjectsList.RowCount - 1].Value = 0;
            ShowLocation();
            ObjectsChanged = true;
        }

        //удаление объекта
        private void DeleteObjectButton_Click(object sender, EventArgs e)
        {
            if (ObjectsList.CurrentRow != null)
            {
                ObjectsList.Rows.Remove(ObjectsList.CurrentRow);
                ObjectsChanged = true;
            }
        }

        //копирование объекта
        private void CopyObjectButton_Click(object sender, EventArgs e)
        {
            if (ObjectsList.CurrentRow != null)
            {
                ObjectsList.Rows.Add();
                DataGridViewComboBoxCell cBoxCell = ObjectsList[0, ObjectsList.RowCount - 1] as DataGridViewComboBoxCell;
                cBoxCell.Items.Add("none");
                foreach (string name in ObjectsName)
                    cBoxCell.Items.Add(name);
                for (int i = 0; i < 5; i++)
                    ObjectsList[i, ObjectsList.RowCount - 1].Value = ObjectsList[i, ObjectsList.CurrentRow.Index].Value;
                ObjectsChanged = true;
            }
        }

        //сохранение объектов
        private void button10_Click(object sender, EventArgs e)
        {
            if (ObjectsChanged)
            {
                SaveObjectList(EditedObjScreen);
                SaveLoc();
            }
        }

        //редактирование таблицы
        private void ObjectsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CheckValue();
            ShowLocation();
        }

        private void ObjectsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowLocation();
        }

        //проверка и коррекция введённых значений
        void CheckValue()
        {
            int row = ObjectsList.CurrentRow.Index;
            if (Int32.Parse(ObjectsList[1, row].Value.ToString()) < 0)
                ObjectsList[1, row].Value = 0;
            if (Int32.Parse(ObjectsList[1, row].Value.ToString()) > 255)
                ObjectsList[1, row].Value = 255;
            if (Int32.Parse(ObjectsList[2, row].Value.ToString()) < 0)
                ObjectsList[2, row].Value = 0;
            if (Int32.Parse(ObjectsList[2, row].Value.ToString()) > 159)
                ObjectsList[2, row].Value = 159;
            if (Int32.Parse(ObjectsList[3, row].Value.ToString()) < 0)
                ObjectsList[3, row].Value = 0;
            if (Int32.Parse(ObjectsList[3, row].Value.ToString()) > 255)
                ObjectsList[3, row].Value = 255;
            if (Int32.Parse(ObjectsList[1, row].Value.ToString()) == 0)
                ObjectsList[4, row].Value = false;
            if (Int32.Parse(ObjectsList[1, row].Value.ToString()) == 255)
                ObjectsList[4, row].Value = true;
            ObjectsChanged = true;
        }

        //перемещение объекта вверх по списку
        private void MoveUpObjectButton_Click(object sender, EventArgs e)
        {
            if (ObjectsList.CurrentRow != null && ObjectsList.CurrentRow.Index != 0)
            {
                int i = ObjectsList.CurrentRow.Index - 1;
                ObjectsList.Rows.Insert(i);
                DataGridViewComboBoxCell cBoxCell = ObjectsList[0, i] as DataGridViewComboBoxCell;
                foreach (string name in ObjectsName)
                    cBoxCell.Items.Add(name);
                for (int j = 0; j < 5; j++ )
                    ObjectsList[j, i].Value = ObjectsList[j, i + 2].Value;
                ObjectsList.Rows.RemoveAt(i + 2);
                DataGridViewCell cell = ObjectsList.Rows[i].Cells[1];
                ObjectsList.CurrentCell = cell;
                ObjectsList.CurrentCell.Selected = true;
                ObjectsChanged = true;
            }
        }

        //перемещение объекта вниз по списку
        private void MoveDownObjectButton_Click(object sender, EventArgs e)
        {
            if (ObjectsList.CurrentRow != null && ObjectsList.CurrentRow.Index < ObjectsList.Rows.Count - 1)
            {
                int i = ObjectsList.CurrentRow.Index + 2;
                ObjectsList.Rows.Insert(i);
                DataGridViewComboBoxCell cBoxCell = ObjectsList[0, i] as DataGridViewComboBoxCell;
                foreach (string name in ObjectsName)
                    cBoxCell.Items.Add(name);
                for (int j = 0; j < 5; j++)
                    ObjectsList[j, i].Value = ObjectsList[j, i - 2].Value;
                ObjectsList.Rows.RemoveAt(i - 2);
                DataGridViewCell cell = ObjectsList.Rows[i - 1].Cells[1];
                ObjectsList.CurrentCell = cell;
                ObjectsList.CurrentCell.Selected = true;
                ObjectsChanged = true;
            }
        }

        //----------------------------------- Работа с тайлами ------------------------------------

        //чтение каталога с тайлсетами

        //загрузка тайлсета
        void LoadTileset()
        {
            //очистка тайлсета
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 32; j++)
                    Tileset[i, j] = 0;
                for (int j = 32; j < 36; j++)
                    Tileset[i, j] = 56;
                Tileset[i, 36] = 0;
            }

            //загрузка образов тайлов
            if (LocName.SelectedIndex != 0)
            {
                BinaryReader f = new BinaryReader(File.Open(TilesetPath + @"/" + LocName.Text + ".bin", FileMode.Open));
                for (int i = 0; i < 256; i++)
                    for (int j = 0; j < 37; j++)
                        Tileset[i, j] = f.ReadByte();
                f.Close();
            }

            CurrentTilesetName = LocName.Text;
            DrawTileset();
            ShowLocation();
            CalcLength();
            TilesetChanged = false;
        }

        //сохранение тайлсета
        void SaveTileset()
        {
            //новый тайлсет или сохранение в имеющийся
            string n = LocName.Text;
            if (LocName.SelectedIndex == 0)
            {
                InputBox i = new InputBox("Input tileset name", "");

                if (i.ShowDialog() == DialogResult.OK)
                {
                    n = i.ResultString;
                    if (n == "")
                        return;
                }
                else
                    return;
                i.Dispose();
            }

            //сохранение образов тайлов
            BinaryWriter f = new BinaryWriter(File.Create(TilesetPath + @"/" + n + ".bin"));
            for (int i = 0; i < 256; i++)
                for (int j = 0; j < 37; j++)
                       f.Write((byte)(Tileset[i, j]));
            f.Close();

            CurrentTilesetName = n;
            TilesetChanged = false;
        }

        //функция проверки, является ли указанный тайл пустым
        bool IsEmptyTile(byte[,] array, int tile_num)
        {
            bool e = true;
            for (int i = 0; i < 32; i++)
                if (array[tile_num, i] != 0)
                    e = false;
            for (int i = 32; i < 36; i++)
                if (array[tile_num, i] != 56)
                    e = false;
            if (array[tile_num, 36] != 0)
                e = false;
            return e;
        }

        //прорисовка тайлсета
        void DrawTileset()
        {
            for (int i = 0; i < 256; i++)
            {
                TilesImg16[i] = new Bitmap(16, 16);
                TilesImg32[i] = new Bitmap(32, 32);
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        byte attr = Tileset[i, 32 + (x / 8) + (y / 8) * 2];
                        byte ink = (byte)((attr & 7) + ((attr >> 3) & 8));
                        byte paper = (byte)((attr >> 3) & 15);
                        int bit = x & 7;
                        Color pixel = ZX.GetColor((byte)((Tileset[i, (x / 8) + y * 2] << bit) & 128) == 0 ? paper : ink);
                        TilesImg16[i].SetPixel(x, y, pixel);
                        TilesImg32[i].SetPixel(x * 2, y * 2, pixel);
                        TilesImg32[i].SetPixel(x * 2 + 1, y * 2, pixel);
                        TilesImg32[i].SetPixel(x * 2, y * 2 + 1, pixel);
                        TilesImg32[i].SetPixel(x * 2 + 1, y * 2 + 1, pixel);
                    }
                }
            }
            ShowTileset();
        }

        //вывод тайлсета
        void ShowTileset()
        {
            Graphics g = Graphics.FromImage(TilesetImg);
            Font f = new Font("Microsoft Sans Serif", 8);
            Brush b = new SolidBrush(Color.Black);
            Brush b2 = new SolidBrush(Color.White);
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                {
                    g.DrawImage(TilesImg32[x + y * 16], x * 32, y * 32);
                    if (ShowTileProperty.Checked && Tileset[x + y * 16, 36] != 0)
                    {
                        g.FillRectangle(b2, x * 32, y * 32, 25, 12);
                        g.DrawString(Tileset[x + y * 16, 36].ToString(), f, b, x * 32, y * 32);
                    }
                }
            if (ShowTilesetGrid.Checked)
            {
                Pen p = new Pen(Color.Gray);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                for (int i = 0; i < 16; i++)
                {
                    g.DrawLine(p, i * 32, 0, i * 32, 512);
                    g.DrawLine(p, 0, i * 32, 512, i * 32);
                }
            }
            TilesetPicture.Image = TilesetImg;
            CalcLength();
        }

        //редактирование тайла
        void EditTileset()
        {
            if (TEditorForm == null || TEditorForm.IsDisposed)
            {
                TEditorForm = new TilesetEditorForm();
                Point p = new Point(this.Location.X + (this.Width - TEditorForm.Width) / 2, this.Location.Y + (this.Height - TEditorForm.Height) / 2);
                TEditorForm.Location = p;
            }
            TEditorForm.InitTilesetEditor();
            TEditorForm.ShowDialog();
            DrawTileset();
            ShowLocation();
        }

        //редактирование свойств тайла
        void PropertyTile()
        {
            if (PropertyEditorForm == null || PropertyEditorForm.IsDisposed)
            {
                PropertyEditorForm = new PropertyForm();
                Point p = new Point(this.Location.X + (this.Width - PropertyEditorForm.Width) / 2, this.Location.Y + (this.Height - PropertyEditorForm.Height) / 2);
                PropertyEditorForm.Location = p;
            }
            PropertyEditorForm.InitPropertyForm(Tileset[TilesSelected[0, 0], 36]);
            PropertyEditorForm.ShowDialog();
            Tileset[TilesSelected[0, 0], 36] = PropertyEditorForm.TileProperty;
            ShowTileset();
            ShowLocation();
        }

        //экспорт тайлсета в scr
        void TilesetExport(int part)
        {
            byte[] scr = new byte[6912];
            for (int i = 6144; i < 6912; i++)
                scr[i] = 56;

            int offset = 0;
            int j_end = 12;
            if (part == 2)
            {
                offset = 192;
                j_end = 4;
            }

            for (int j = 0; j < j_end; j++)
                for (int i = 0; i < 16; i++)
                {
                    int c = 0;
                    for (int y = 0; y < 16; y++)
                        for (int x = 0; x < 2; x++)
                        {
                            int ycord = j * 16 + y;
                            int adr = (ycord & 0x07) * 256 + ((ycord << 2) & 0xe0) + ((ycord >> 3) & 0x18) * 256 + i * 2 + x;
                            scr[adr] = Tileset[i + j * 16 + offset, c];
                            c++;
                        }
                    for (int y = 0; y < 2; y++)
                        for (int x = 0; x < 2; x++)
                        {
                            int ycord = j * 2 + y;
                            int adr = ycord * 32 + i * 2 + x + 6144;
                            scr[adr] = Tileset[i + j * 16 + offset, c];
                            c++;
                        }
                }
            ExportDialog.Title = "Part " + part.ToString() +" of 2";
            if (ExportDialog.ShowDialog() != DialogResult.Cancel)
            {
                BinaryWriter f = new BinaryWriter(File.Create(ExportDialog.FileName));
                for (int i = 0; i < 6912; i++)
                    f.Write(scr[i]);
                f.Close();
            }
            ExportDialog.FileName = "";
        }

        //вывод статистики использования тайлов
        void ShowUsageStatistics()
        {
            Graphics g = Graphics.FromImage(TilesetImg);
            Font f = new Font("Microsoft Sans Serif", 8);
            Brush b = new SolidBrush(Color.Black);
            Brush b2 = new SolidBrush(Color.Yellow);
            for (int j = 0; j < 16; j++)
                for (int i = 0; i < 16; i++)
                {
                    int count = 0;
                    for (int y = 0; y < LocSizeY.Value; y++)
                        for (int x = 0; x < LocSizeX.Value; x++)
                            if (Loc[x, y] == i + j * 16)
                                count++;
                    if (count > 0)
                    {
                        g.FillRectangle(b2, i * 32, j * 32, 32, 12);
                        g.DrawString(count.ToString(), f, b, i * 32, j * 32);
                    }
                }
            TilesetPicture.Image = TilesetImg;
        }

        //копирование в буфер выделенных тайлов из тайлсета
        void CopyTiles(Rectangle rect)
        {
            TilesBuffer = new byte[rect.Width, rect.Height][];
            TilesBufferImg = new Bitmap[rect.Width, rect.Height];
            for (int j = 0; j < rect.Height; j++)
                for (int i = 0; i < rect.Width; i++)
                {
                    TilesBuffer[i, j] = new byte[37];
                    for (int k = 0; k < 37; k++)
                        TilesBuffer[i, j][k] = Tileset[rect.X + i + (rect.Y + j) * 16, k];
                    TilesBufferImg[i, j] = new Bitmap(TilesImg32[rect.X + i + (rect.Y + j) * 16]);
                }
        }

        //вставка тайлов в тайлсет из буфера
        void PasteTiles(Point point)
        {
            for (int j = 0; j < TilesBuffer.GetLength(1); j++)
                for (int i = 0; i < TilesBuffer.GetLength(0); i++)
                    for (int k = 0; k < 37; k++)
                        Tileset[point.X + i + (point.Y + j) * 16, k] = TilesBuffer[i, j][k];
            DrawTileset();
        }

        //удаление выделенных тайлов из тайлсета
        void DeleteTiles(Rectangle rect)
        {
            for (int j = 0; j < rect.Height; j++)
                for (int i = 0; i < rect.Width; i++)
                {
                    for (int k = 0; k < 32; k++)
                        Tileset[rect.X + i + (rect.Y + j) * 16, k] = 0;
                    for (int k = 32; k < 36; k++)
                        Tileset[rect.X + i + (rect.Y + j) * 16, k] = 56;
                    Tileset[rect.X + i + (rect.Y + j) * 16, 36] = 0;
                }
            DrawTileset();
        }

        //--------------------------------------- Интерфейс ---------------------------------------

		//загрузка настроек
        public LocationEditorForm()
        {
            InitializeComponent();
            if (File.Exists("setup.xml"))
            {
                using (Stream stream = new FileStream("setup.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings iniSet = (Settings)serializer.Deserialize(stream);
                    LocationsPath = iniSet.LocationsPath;
                    CurrentLocName = iniSet.CurrentLocName;
                    TilesetPath = iniSet.TilesetPath;
                    CurrentTilesetName = iniSet.CurrentTilesetName;
                }
            }
            InitObjects();
            LocSizeX.Increment = OneHScroll;
            LocSizeY.Increment = OneVScroll;
            SetupDrawLoc();
            ReadLocDir();
            LocPicture.MouseWheel += new MouseEventHandler(LocPicture_MouseWheel);
            ResizeLocation();
        }

        //зум превью локации
        void LocPicture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                LocScale = 32;
            else
                LocScale = 16;
            SetupDrawLoc();
            ShowLocation();
        }

        //изменение размеров локации
        private void LocSizeX_ValueChanged(object sender, EventArgs e)
        {
            ResizeLocation();
        }

        private void LocSizeY_ValueChanged(object sender, EventArgs e)
        {
            ResizeLocation();
        }

        //работа с LocPicture
        private void LocPicture_MouseEnter(object sender, EventArgs e)
        {
            LocPicture.Focus();
        }

        private void LocPicture_MouseMove(object sender, MouseEventArgs e)
        {
            xobj = e.X; //костыль, потом переделаю, наверное
            yobj = e.Y;

            //режим тайлсета
            if (TabControl.SelectedIndex == 0)
            {
                if (ExtInsertMode || MoveTilesMode || CopyTilesMode)
                    return;

                int x = (e.X - LocDrawHome.X) / LocScale + XScroll.Value * OneHScroll;
                int y = (e.Y - LocDrawHome.Y) / LocScale + YScroll.Value * OneVScroll;
                if (e.X - LocDrawHome.X < 0)
                    x--;
                if (e.Y - LocDrawHome.Y < 0)
                    y--;

                //выход за пределы локации
                if (x < 0 || x >= LocSizeX.Value || y < 0 || y >= LocSizeY.Value)
                {
                    LocPosX.Text = "";
                    LocPosY.Text = "";
                    LastCord.X = -1;
                    LastCord.Y = -1;
                    LocCord.X = -1;
                    LocCord.Y = -1;
                    ShowLocation();
                    return;
                }

                if (LastCord.X != x || LastCord.Y != y)
                {
                    LocPosX.Text = x.ToString();
                    LocPosY.Text = y.ToString();

                    int xr = My.LimitRange(x, 0, (int)LocSizeX.Value - TilesSelectedRect.Width);
                    int yr = My.LimitRange(y, 0, (int)LocSizeY.Value - TilesSelectedRect.Height);

                    LastCord.X = xr;
                    LastCord.Y = yr;
                    LocCord.X = xr;
                    LocCord.Y = yr;

                    //зажата кнопка мыши
                    if (e.Button == MouseButtons.Left)
                        LocPicture_MouseDown(sender, e);

                    //продолжение выделения области в локации
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (x < 0)
                            x = 0;
                        if (x > LocSizeX.Value - 1)
                            x = (int)LocSizeX.Value - 1;
                        if (y < 0)
                            y = 0;
                        if (y > LocSizeY.Value - 1)
                            y = (int)LocSizeY.Value - 1;
                        LocCord = LocSelCord;
                        if (x < LocCord.X)
                        {
                            LocCord.X = x;
                            x = LocSelCord.X;
                        }
                        if (y < LocCord.Y)
                        {
                            LocCord.Y = y;
                            y = LocSelCord.Y;
                        }
                        TilesSelectedRect.Width = x - LocCord.X + 1;
                        TilesSelectedRect.Height = y - LocCord.Y + 1;
                        TilesSelected = new byte[TilesSelectedRect.Width, TilesSelectedRect.Height];
                        for (int j = 0; j < TilesSelectedRect.Height; j++)
                            for (int i = 0; i < TilesSelectedRect.Width; i++)
                                TilesSelected[i, j] = Loc[LocCord.X + i, LocCord.Y + j];
                        ShowLocation();
                    }
                    else
                        ShowLocation();
                }
            }
        }

        private void LocPicture_MouseDown(object sender, MouseEventArgs e)
        {
            //режим тайлсета
            if (TabControl.SelectedIndex == 0)
            {
                if (ExtInsertMode || LocCord.X == -1)
                    return;

                //вставка в локацию выделенного блока тайлов
                if (e.Button == MouseButtons.Left)
                {
                    for (int j = 0; j < TilesSelectedRect.Height; j++)
                        for (int i = 0; i < TilesSelectedRect.Width; i++)
                            if (SetNullTiles.Checked || !IsEmptyTile(Tileset, TilesSelected[i, j]))
                                Loc[LocCord.X + i, LocCord.Y + j] = TilesSelected[i, j];
                    ShowLocation();
                }
                //начало выделения области в локации
                if (e.Button == MouseButtons.Right)
                {
                    TilesetPicture.Image = TilesetImg;
                    LocSelCord.X = LocCord.X;
                    LocSelCord.Y = LocCord.Y;
                    TilesSelectedRect.Width = 1;
                    TilesSelectedRect.Height = 1;
                    TilesSelected = new byte[1, 1];
                    TilesSelected[0, 0] = Loc[LocCord.X, LocCord.Y];
                    ShowLocation();

                    //выделение тайла в тайлсете
                    Bitmap img = new Bitmap(TilesetImg);
                    Graphics g = Graphics.FromImage(img);
                    Pen p = new Pen(Color.Red);
                    TilesCord.X = TilesSelected[0, 0] % 16;
                    TilesCord.Y = TilesSelected[0, 0] / 16;
                    g.DrawRectangle(p, TilesCord.X * 32, TilesCord.Y * 32, 31, 31);
                    TilesetPicture.Image = img;
                }
            }
        }

        private void LocPicture_MouseUp(object sender, MouseEventArgs e)
        {
            //режим тайлсета
            if (TabControl.SelectedIndex == 0)
            {
                if (e.Button != MouseButtons.Right)
                    AddLoc();
            }
        }

        private void LocPicture_MouseLeave(object sender, EventArgs e)
        {
            //режим тайлсета
            if (TabControl.SelectedIndex == 0)
            {
                LocPosX.Text = "";
                LocPosY.Text = "";
                LastCord.X = -1;
                LastCord.Y = -1;
                LocCord.X = -1;
                LocCord.Y = -1;
                ShowLocation();
            }
        }

        private void LocPicture_DoubleClick(object sender, EventArgs e)
        {
            //режим объектов
            if (TabControl.SelectedIndex == 1)
            {
                if (ObjectsList.CurrentRow != null)
                {
                    int row = ObjectsList.CurrentRow.Index;
                    int s = LocScale / 16;
                    int x = (xobj - LocDrawHome.X) / s;
                    int y = (yobj - LocDrawHome.Y) / s;
                    ObjectsList[1, row].Value = x.ToString();
                    ObjectsList[2, row].Value = y.ToString();
                    CheckValue();
                }
            }
        }

        //работа с TilesetPicture
        private void TilesetPicture_MouseMove(object sender, MouseEventArgs e)
        {
            //отображение буфера тайлов
            if (ExtInsertMode || CopyTilesMode || MoveTilesMode)
            {
                int xsz = TilesSelectedRect.Width;
                int ysz = TilesSelectedRect.Height;
                int x = My.LimitRange(e.X / 32, 0, 16 - xsz);
                int y = My.LimitRange(e.Y / 32, 0, 16 - ysz);

                Bitmap img = new Bitmap(TilesetImg);
                Graphics g = Graphics.FromImage(img);
                isEmptyTiles = true;
                for (int j = 0; j < ysz; j++)
                    for (int i = 0; i < xsz; i++)
                    {
                        g.DrawImage(TilesBufferImg[i, j], (x + i) * 32, (y + j) * 32);
                        if (!IsEmptyTile(Tileset, (x + i) + (y + j) * 16))
                            isEmptyTiles = false;
                    }
                Pen p = isEmptyTiles ? new Pen(Color.Green) : new Pen(Color.Red);
                g.DrawRectangle(p, x * 32, y * 32, xsz * 32 - 1, ysz * 32 - 1);
                TilesetPicture.Image = img;
            }
            //продолжение выбора области в тайлсете
            else if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int x = My.LimitRange(e.X / 32, 0, 15);
                int y = My.LimitRange(e.Y / 32, 0, 15);

                if (TilesCord.X != x || TilesCord.Y != y)
                {
                    int x2 = TilesCord.X;
                    int y2 = TilesCord.Y;

                    if (x > x2)
                    {
                        x2 = x;
                        x = TilesCord.X;
                    }
                    if (y > y2)
                    {
                        y2 = y;
                        y = TilesCord.Y;
                    }

                    Bitmap img = new Bitmap(TilesetImg);
                    Graphics g = Graphics.FromImage(img);
                    Pen p = new Pen(Color.Red);
                    g.DrawRectangle(p, x * 32, y * 32, (x2 - x + 1) * 32 - 1, (y2 - y + 1) * 32 - 1);
                    TilesetPicture.Image = img;
                    TilesSelectedRect.X = x;
                    TilesSelectedRect.Y = y;
                    TilesSelectedRect.Width = x2 - x + 1;
                    TilesSelectedRect.Height = y2 - y + 1;
                }
            }
        }

        private void TilesetPicture_MouseDown(object sender, MouseEventArgs e)
        {
            TilesCord.X = e.X / 32;
            TilesCord.Y = e.Y / 32;
            //режим вставки тайлов из внешнего источника
            if (ExtInsertMode || CopyTilesMode || MoveTilesMode)
            {
                //отмена вставки внешних тайлов
                if (e.Button == MouseButtons.Right)
                {
                    //восстановление тайлов при отмене перемещения
                    if (MoveTilesMode)
                        PasteTiles(PosMoveTiles);
                    ExtInsertMode = false;
                    CopyTilesMode = false;
                    MoveTilesMode = false;
                    TilesetPicture.Image = TilesetImg;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (!isEmptyTiles)
                    {
                        if (MessageBox.Show("Do you want to continue?", "Selected area is not empty!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return;
                    }
                    int xsz = TilesSelectedRect.Width;
                    int ysz = TilesSelectedRect.Height;
                    int x = My.LimitRange(e.X / 32, 0, 16 - xsz);
                    int y = My.LimitRange(e.Y / 32, 0, 16 - ysz);

                    for (int j = 0; j < ysz; j++)
                        for (int i = 0; i < xsz; i++)
                            for (int k = 0; k < 37; k++)
                                Tileset[(x + i) + (y + j) * 16, k] = TilesBuffer[i, j][k];

                    //перерасчёт номеров тайлов в локации после перемещения
                    if (MoveTilesMode)
                    {  
                        for (int yl = 0; yl < 256; yl++)
                            for (int xl = 0; xl < 256; xl++)
                            {
                                bool done = false;
                                for (int j = 0; j < ysz; j++)
                                    for (int i = 0; i < xsz; i++)
                                        if (Loc[xl, yl] == PosMoveTiles.X + i + (PosMoveTiles.Y + j) * 16 && !done && Loc[xl, yl] != 0)
                                        {
                                            Loc[xl, yl] = (byte)(x + i + (y + j) * 16);
                                            done = true;
                                        }
                            }
                        ClearUndoBuffer();
                    }

                    CalcLength();
                    DrawTileset();
                    ExtInsertMode = false;
                    CopyTilesMode = false;
                    MoveTilesMode = false;
                    TilesetChanged = true;
                }
            }
            else
            //начало выбора области в тайлсете
            {
                Bitmap img = new Bitmap(TilesetImg);
                Graphics g = Graphics.FromImage(img);
                Pen p = new Pen(Color.Red);
                g.DrawRectangle(p, TilesCord.X * 32, TilesCord.Y * 32, 31, 31);
                TilesetPicture.Image = img;
                TilesSelectedRect.X = TilesCord.X;
                TilesSelectedRect.Y = TilesCord.Y;
                TilesSelectedRect.Width = 1;
                TilesSelectedRect.Height = 1;
            }
        }

        private void TilesetPicture_MouseUp(object sender, MouseEventArgs e)
        {
            //окончание выбора области в тайлсете
            TilesSelected = new byte[TilesSelectedRect.Width, TilesSelectedRect.Height];
            for (int y = 0; y < TilesSelectedRect.Height; y++)
                for (int x = 0; x < TilesSelectedRect.Width; x++)
                    TilesSelected[x, y] = (byte)(x + TilesSelectedRect.X + (y + TilesSelectedRect.Y) * 16);
        }

        private void TilesetPicture_MouseLeave(object sender, EventArgs e)
        {
            //режим вставки тайлов из внешнего источника
            if (ExtInsertMode)
            {
                TilesetPicture.Image = TilesetImg;
            }
        }

        //навигация по локации
        private void XScroll_Scroll(object sender, ScrollEventArgs e)
        {
            if (ObjectsChanged)
            {
                SaveObjectList(EditedObjScreen);
                SaveLoc();
            }
            LoadObjectList(XScroll.Value);
            ShowLocation();
        }

        private void YScroll_Scroll(object sender, ScrollEventArgs e)
        {
            ShowLocation();
        }

        //выбор загрузки локации
        private void LocName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTileset();
            LoadLocation();       
        }

        //выбор сохранения локации
        private void SaveLoc_Click(object sender, EventArgs e)
        {
            SaveLoc();
        }

        //выбор сохранения тайлсета
        private void SaveTilesetButton_Click(object sender, EventArgs e)
        {
            SaveTileset();
        }

        //закрытие редактора
        private void LocationEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSetting();
            if (LocChanged)
                if (MessageBox.Show("Save location?", "Location has been changed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    SaveLoc();
            if (TilesetChanged)
                if (MessageBox.Show("Save tileset?", "Tileset has been changed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    SaveTileset();
        }

        //хот-кеи
        private void LocationEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (LocName.Focused == true)
                return;

            //редактор тайлов
            if (e.KeyCode == Keys.E)
                PropertyTile();

            //отмена изменений
            if (e.Control && e.KeyCode == Keys.Z)
            {
                UndoLoc();
                ShowLocation();
            }
            //возврат изменений
            if (e.Control && e.KeyCode == Keys.Y)
            {
                RedoLoc();
                ShowLocation();
            }
            //свдиг локации
            if (e.KeyCode == Keys.Right && XScroll.Value < XScroll.Maximum && !ObjectsList.Focused)
            {
                XScroll.Value++;
                ShowLocation();
            }
            if (e.KeyCode == Keys.Left && XScroll.Value > XScroll.Minimum && !ObjectsList.Focused)
            {
                XScroll.Value--;
                ShowLocation();
            }
            if (e.KeyCode == Keys.Down && YScroll.Value < YScroll.Maximum && !ObjectsList.Focused)
            {
                YScroll.Value++;
                ShowLocation();
            }
            if (e.KeyCode == Keys.Up && YScroll.Value > YScroll.Minimum && !ObjectsList.Focused)
            {
                YScroll.Value--;
                ShowLocation();
            }
        }

        //разрешение хот-кеев
        private void LocationEditorForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void LocName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void LocSizeX_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void LocSizeY_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void LocPicture_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        //сдвиги локации 
        private void LocUp_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 255; y++)
                for (int x = 0; x < 256; x++)
                {
                    Loc[x, y] = Loc[x, y + 1];
                    Loc[x, y + 1] = 0;
                }
            ShowLocation();
            AddLoc();
        }

        private void LocDown_Click(object sender, EventArgs e)
        {
            for (int y = 255; y > 0; y--)
                for (int x = 0; x < 256; x++)
                {
                    Loc[x, y] = Loc[x, y - 1];
                    Loc[x, y - 1] = 0;
                }
            ShowLocation();
            AddLoc();
        }

        private void LocLeft_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 255; x++)
                {
                    Loc[x, y] = Loc[x + 1, y];
                    Loc[x + 1, y] = 0;
                }
            ShowLocation();
            AddLoc();
        }

        private void LocRight_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < 256; y++)
                for (int x = 255; x > 0; x--)
                {
                    Loc[x, y] = Loc[x - 1, y];
                    Loc[x - 1, y] = 0;
                }
            ShowLocation();
            AddLoc();
        }

        //отображение сетки на локации
        private void ShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            ShowLocation();
        }

        //отображение сетки на тайлсете
        private void ShowTilesetGrid_CheckedChanged(object sender, EventArgs e)
        {
            ShowTileset();
        }

        //отображение свойств тайлов на тайлсете
        private void ShowTileProperty_CheckedChanged(object sender, EventArgs e)
        {
            ShowTileset();
        }

        //вызов редкатирования свойств тайла
        private void TilesetPicture_DoubleClick(object sender, EventArgs e)
        {
            PropertyTile();
        }

        //вызов редкатирования тайлсета
        private void button1_Click(object sender, EventArgs e)
        {
            EditTileset();
        }

		//начало перемещения тайлов на шаблоне
        private void button5_Click(object sender, EventArgs e)
        {
            PosMoveTiles = new Point(TilesSelectedRect.X, TilesSelectedRect.Y);
            CopyTiles(TilesSelectedRect);
            DeleteTiles(TilesSelectedRect);
            MoveTilesMode = true;
        }

		//удаление тайлов
        private void button3_Click(object sender, EventArgs e)
        {
            if (TilesCord.X != -1 && TilesCord.Y != -1)
            {
                string caption = "This tile found in location ";
                string message = "Do you really want delete the tile?";

                if (TilesSelectedRect.Height != 1 || TilesSelectedRect.Width != 1)
                {
                    caption = "Some of this tiles found in location ";
                    message = "Do you really want delete the tiles?";
                }

                //подсчёт тайлов в локации
                int count = 0;
                for (int j = 0; j < TilesSelectedRect.Height; j++)
                    for (int i = 0; i < TilesSelectedRect.Width; i++)
                    {
                        int tile = TilesSelected[i, j];
                        if (tile != 0)
                        {
                            for (int y = 0; y < LocSizeY.Value; y++)
                                for (int x = 0; x < LocSizeX.Value; x++)
                                    if (Loc[x, y] == tile)
                                        count++;
                        }
                    }
                if (count != 0)
                {
                    if (MessageBox.Show(message, caption + count.ToString() + " times!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                }

                DeleteTiles(TilesSelectedRect);
                ShowLocation();
                TilesetChanged = true;
            }
        }

        //начало копирования тайла
        private void button4_Click(object sender, EventArgs e)
        {
            CopyTiles(TilesSelectedRect);
            CopyTilesMode = true;
        }

        //захват тайлов из внешней картинки
        private void button6_Click(object sender, EventArgs e)
        {
            if (CaptureForm == null || CaptureForm.IsDisposed)
                CaptureForm = new CaptureFromImgForm();
            CaptureForm.ShowDialog();
            if (TilesBuffer != null)
                ExtInsertMode = true;
        }

        //вызов сборки локации
        private void button7_Click(object sender, EventArgs e)
        {
            LocAssembly();
        }

        //обработка таймеров тайлов
        private void TileTimer_Tick(object sender, EventArgs e)
        {
            //инициализация для случайной анимации
            if ((rnd.Next() & 63) == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (RndTimerTiles[i] == 4)
                    {
                        RndTimerTiles[i] = 0;
                    }
                }
            }

            //счётчики анимации
            for (int i = 0; i < 16; i++)
            {
                TimerCounters[i]++;
                if (TimerCounters[i] == i + 1)
                {
                    TimerCounters[i] = 0;
                    TimerTiles[i]++;
                    if (TimerTiles[i] == 4)
                    {
                        TimerTiles[i] = 0;
                    }
                }
            }

            //счётчики случайной анимации
            for (int i = 0; i < 16; i++)
            {
                RndTimerCounters[i]++;
                if (RndTimerCounters[i] == i + 1)
                {
                    RndTimerCounters[i] = 0;
                    if (RndTimerTiles[i] != 4)
                    {
                        RndTimerTiles[i]++;
                    }
                }
            }

            if (ExtInsertMode || CopyTilesMode || MoveTilesMode)
                return;
            ShowLocation();
        }

        //отображение свойств тайлов в локации
        private void ShowLocProperty_CheckedChanged(object sender, EventArgs e)
        {
            ShowLocation();
        }

        //вызов экспорта тайлсета
        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tileset has been exported in two SCR files.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            TilesetExport(1);
            TilesetExport(2);
        }

        //включение отображения статистики использования тайлов
        private void button9_MouseDown(object sender, MouseEventArgs e)
        {
            ShowUsageStatistics();
        }

        //выключение отображения статистики использования тайлов
        private void button9_MouseUp(object sender, MouseEventArgs e)
        {
            ShowTileset();
        }

        //свойства тайла
        private void button2_Click(object sender, EventArgs e)
        {
            PropertyTile();
        }

        //изменение размеров формы редактора
        private void LocationEditorForm_Resize(object sender, EventArgs e)
        {
            SetupDrawLoc();
        }

        //отображение объектов
        private void ShowObjects_CheckedChanged(object sender, EventArgs e)
        {
            ShowLocation();
        }

    }
}