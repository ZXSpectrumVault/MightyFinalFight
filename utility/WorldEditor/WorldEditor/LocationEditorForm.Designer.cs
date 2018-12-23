namespace WorldEditor
{
    partial class LocationEditorForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.TilesetCnt = new System.Windows.Forms.Label();
            this.LocSizeX = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LocSizeY = new System.Windows.Forms.NumericUpDown();
            this.LocPicture = new System.Windows.Forms.PictureBox();
            this.YScroll = new System.Windows.Forms.VScrollBar();
            this.XScroll = new System.Windows.Forms.HScrollBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LocPosX = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LocPosY = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TilesetLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.LocLength = new System.Windows.Forms.ToolStripStatusLabel();
            this.LocName = new System.Windows.Forms.ComboBox();
            this.SaveLocButton = new System.Windows.Forms.Button();
            this.LocLeft = new System.Windows.Forms.Button();
            this.LocRight = new System.Windows.Forms.Button();
            this.LocDown = new System.Windows.Forms.Button();
            this.LocUp = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ShowLocGrid = new System.Windows.Forms.CheckBox();
            this.ShowTilesetGrid = new System.Windows.Forms.CheckBox();
            this.SaveTilesetButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.TileTimer = new System.Windows.Forms.Timer(this.components);
            this.ShowTileProperty = new System.Windows.Forms.CheckBox();
            this.ShowLocProperty = new System.Windows.Forms.CheckBox();
            this.ExportDialog = new System.Windows.Forms.SaveFileDialog();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ShowObjects = new System.Windows.Forms.CheckBox();
            this.SetNullTiles = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ObjectsOnScreenEdit = new System.Windows.Forms.TextBox();
            this.CopyObjectButton = new System.Windows.Forms.Button();
            this.MoveUpObjectButton = new System.Windows.Forms.Button();
            this.MoveDownObjectButton = new System.Windows.Forms.Button();
            this.DeleteObjectButton = new System.Windows.Forms.Button();
            this.AddObjectButton = new System.Windows.Forms.Button();
            this.ObjectsList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button10 = new System.Windows.Forms.Button();
            this.AssemblyButton = new System.Windows.Forms.Button();
            this.TilesetPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LocSizeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocSizeY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocPicture)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsList)).BeginInit();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Location";
            // 
            // TilesetCnt
            // 
            this.TilesetCnt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TilesetCnt.AutoSize = true;
            this.TilesetCnt.Location = new System.Drawing.Point(6, 521);
            this.TilesetCnt.Name = "TilesetCnt";
            this.TilesetCnt.Size = new System.Drawing.Size(38, 13);
            this.TilesetCnt.TabIndex = 3;
            this.TilesetCnt.Text = "Tileset";
            // 
            // LocSizeX
            // 
            this.LocSizeX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocSizeX.Location = new System.Drawing.Point(54, 523);
            this.LocSizeX.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LocSizeX.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.LocSizeX.Name = "LocSizeX";
            this.LocSizeX.ReadOnly = true;
            this.LocSizeX.Size = new System.Drawing.Size(60, 20);
            this.LocSizeX.TabIndex = 5;
            this.LocSizeX.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.LocSizeX.ValueChanged += new System.EventHandler(this.LocSizeX_ValueChanged);
            this.LocSizeX.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LocSizeX_PreviewKeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 525);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "X size";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 551);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Y size";
            // 
            // LocSizeY
            // 
            this.LocSizeY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocSizeY.Location = new System.Drawing.Point(54, 549);
            this.LocSizeY.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.LocSizeY.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.LocSizeY.Name = "LocSizeY";
            this.LocSizeY.ReadOnly = true;
            this.LocSizeY.Size = new System.Drawing.Size(60, 20);
            this.LocSizeY.TabIndex = 7;
            this.LocSizeY.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.LocSizeY.ValueChanged += new System.EventHandler(this.LocSizeY_ValueChanged);
            this.LocSizeY.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LocSizeY_PreviewKeyDown);
            // 
            // LocPicture
            // 
            this.LocPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LocPicture.Location = new System.Drawing.Point(15, 34);
            this.LocPicture.Name = "LocPicture";
            this.LocPicture.Padding = new System.Windows.Forms.Padding(3);
            this.LocPicture.Size = new System.Drawing.Size(512, 463);
            this.LocPicture.TabIndex = 9;
            this.LocPicture.TabStop = false;
            this.LocPicture.DoubleClick += new System.EventHandler(this.LocPicture_DoubleClick);
            this.LocPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LocPicture_MouseDown);
            this.LocPicture.MouseEnter += new System.EventHandler(this.LocPicture_MouseEnter);
            this.LocPicture.MouseLeave += new System.EventHandler(this.LocPicture_MouseLeave);
            this.LocPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LocPicture_MouseMove);
            this.LocPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LocPicture_MouseUp);
            this.LocPicture.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LocPicture_PreviewKeyDown);
            // 
            // YScroll
            // 
            this.YScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.YScroll.LargeChange = 1;
            this.YScroll.Location = new System.Drawing.Point(530, 34);
            this.YScroll.Name = "YScroll";
            this.YScroll.Padding = new System.Windows.Forms.Padding(3);
            this.YScroll.Size = new System.Drawing.Size(17, 463);
            this.YScroll.TabIndex = 10;
            this.YScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.YScroll_Scroll);
            // 
            // XScroll
            // 
            this.XScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.XScroll.LargeChange = 1;
            this.XScroll.Location = new System.Drawing.Point(15, 500);
            this.XScroll.Maximum = 2;
            this.XScroll.Name = "XScroll";
            this.XScroll.Padding = new System.Windows.Forms.Padding(3);
            this.XScroll.Size = new System.Drawing.Size(515, 17);
            this.XScroll.TabIndex = 11;
            this.XScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.XScroll_Scroll);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.LocPosX,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.LocPosY,
            this.toolStripStatusLabel4,
            this.TilesetLength,
            this.toolStripStatusLabel5,
            this.LocLength});
            this.statusStrip1.Location = new System.Drawing.Point(0, 630);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1091, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "X pos";
            // 
            // LocPosX
            // 
            this.LocPosX.AutoSize = false;
            this.LocPosX.Name = "LocPosX";
            this.LocPosX.Size = new System.Drawing.Size(32, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel2.Text = "Y pos";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // LocPosY
            // 
            this.LocPosY.AutoSize = false;
            this.LocPosY.Name = "LocPosY";
            this.LocPosY.Size = new System.Drawing.Size(32, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel4.Text = "Tileset length";
            // 
            // TilesetLength
            // 
            this.TilesetLength.AutoSize = false;
            this.TilesetLength.Name = "TilesetLength";
            this.TilesetLength.Size = new System.Drawing.Size(64, 17);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(90, 17);
            this.toolStripStatusLabel5.Text = "Location length";
            // 
            // LocLength
            // 
            this.LocLength.AutoSize = false;
            this.LocLength.Name = "LocLength";
            this.LocLength.Size = new System.Drawing.Size(64, 17);
            // 
            // LocName
            // 
            this.LocName.FormattingEnabled = true;
            this.LocName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.LocName.Location = new System.Drawing.Point(66, 6);
            this.LocName.Name = "LocName";
            this.LocName.Size = new System.Drawing.Size(452, 21);
            this.LocName.TabIndex = 13;
            this.LocName.SelectedIndexChanged += new System.EventHandler(this.LocName_SelectedIndexChanged);
            this.LocName.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LocName_PreviewKeyDown);
            // 
            // SaveLocButton
            // 
            this.SaveLocButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveLocButton.Location = new System.Drawing.Point(459, 594);
            this.SaveLocButton.Name = "SaveLocButton";
            this.SaveLocButton.Size = new System.Drawing.Size(88, 23);
            this.SaveLocButton.TabIndex = 14;
            this.SaveLocButton.Text = "Save Loc";
            this.SaveLocButton.UseVisualStyleBackColor = true;
            this.SaveLocButton.Click += new System.EventHandler(this.SaveLoc_Click);
            // 
            // LocLeft
            // 
            this.LocLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocLeft.Location = new System.Drawing.Point(197, 546);
            this.LocLeft.Name = "LocLeft";
            this.LocLeft.Size = new System.Drawing.Size(23, 23);
            this.LocLeft.TabIndex = 15;
            this.LocLeft.UseVisualStyleBackColor = true;
            this.LocLeft.Click += new System.EventHandler(this.LocLeft_Click);
            // 
            // LocRight
            // 
            this.LocRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocRight.Location = new System.Drawing.Point(238, 546);
            this.LocRight.Name = "LocRight";
            this.LocRight.Size = new System.Drawing.Size(23, 23);
            this.LocRight.TabIndex = 16;
            this.LocRight.UseVisualStyleBackColor = true;
            this.LocRight.Click += new System.EventHandler(this.LocRight_Click);
            // 
            // LocDown
            // 
            this.LocDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocDown.Location = new System.Drawing.Point(218, 567);
            this.LocDown.Name = "LocDown";
            this.LocDown.Size = new System.Drawing.Size(23, 23);
            this.LocDown.TabIndex = 17;
            this.LocDown.UseVisualStyleBackColor = true;
            this.LocDown.Click += new System.EventHandler(this.LocDown_Click);
            // 
            // LocUp
            // 
            this.LocUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocUp.Location = new System.Drawing.Point(218, 525);
            this.LocUp.Name = "LocUp";
            this.LocUp.Size = new System.Drawing.Size(23, 23);
            this.LocUp.TabIndex = 18;
            this.LocUp.UseVisualStyleBackColor = true;
            this.LocUp.Click += new System.EventHandler(this.LocUp_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Location shift";
            // 
            // ShowLocGrid
            // 
            this.ShowLocGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowLocGrid.AutoSize = true;
            this.ShowLocGrid.Location = new System.Drawing.Point(447, 546);
            this.ShowLocGrid.Name = "ShowLocGrid";
            this.ShowLocGrid.Size = new System.Drawing.Size(71, 17);
            this.ShowLocGrid.TabIndex = 20;
            this.ShowLocGrid.Text = "show grid";
            this.ShowLocGrid.UseVisualStyleBackColor = true;
            this.ShowLocGrid.CheckedChanged += new System.EventHandler(this.ShowGrid_CheckedChanged);
            // 
            // ShowTilesetGrid
            // 
            this.ShowTilesetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowTilesetGrid.AutoSize = true;
            this.ShowTilesetGrid.Location = new System.Drawing.Point(429, 545);
            this.ShowTilesetGrid.Name = "ShowTilesetGrid";
            this.ShowTilesetGrid.Size = new System.Drawing.Size(71, 17);
            this.ShowTilesetGrid.TabIndex = 21;
            this.ShowTilesetGrid.Text = "show grid";
            this.ShowTilesetGrid.UseVisualStyleBackColor = true;
            this.ShowTilesetGrid.CheckedChanged += new System.EventHandler(this.ShowTilesetGrid_CheckedChanged);
            // 
            // SaveTilesetButton
            // 
            this.SaveTilesetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveTilesetButton.Location = new System.Drawing.Point(429, 566);
            this.SaveTilesetButton.Name = "SaveTilesetButton";
            this.SaveTilesetButton.Size = new System.Drawing.Size(88, 23);
            this.SaveTilesetButton.TabIndex = 24;
            this.SaveTilesetButton.Text = "Save Tileset";
            this.SaveTilesetButton.UseVisualStyleBackColor = true;
            this.SaveTilesetButton.Click += new System.EventHandler(this.SaveTilesetButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(6, 537);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "Edit tileset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(100, 537);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 23);
            this.button3.TabIndex = 27;
            this.button3.Text = "Delete tiles";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(194, 537);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 23);
            this.button4.TabIndex = 28;
            this.button4.Text = "Copy tile";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(100, 566);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 23);
            this.button5.TabIndex = 29;
            this.button5.Text = "Move tiles";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(194, 566);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 23);
            this.button6.TabIndex = 30;
            this.button6.Text = "Capture from";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // TileTimer
            // 
            this.TileTimer.Enabled = true;
            this.TileTimer.Interval = 20;
            this.TileTimer.Tick += new System.EventHandler(this.TileTimer_Tick);
            // 
            // ShowTileProperty
            // 
            this.ShowTileProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowTileProperty.AutoSize = true;
            this.ShowTileProperty.Location = new System.Drawing.Point(429, 524);
            this.ShowTileProperty.Name = "ShowTileProperty";
            this.ShowTileProperty.Size = new System.Drawing.Size(92, 17);
            this.ShowTileProperty.TabIndex = 32;
            this.ShowTileProperty.Text = "show property";
            this.ShowTileProperty.UseVisualStyleBackColor = true;
            this.ShowTileProperty.CheckedChanged += new System.EventHandler(this.ShowTileProperty_CheckedChanged);
            // 
            // ShowLocProperty
            // 
            this.ShowLocProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowLocProperty.AutoSize = true;
            this.ShowLocProperty.Location = new System.Drawing.Point(316, 523);
            this.ShowLocProperty.Name = "ShowLocProperty";
            this.ShowLocProperty.Size = new System.Drawing.Size(92, 17);
            this.ShowLocProperty.TabIndex = 33;
            this.ShowLocProperty.Text = "show property";
            this.ShowLocProperty.UseVisualStyleBackColor = true;
            this.ShowLocProperty.CheckedChanged += new System.EventHandler(this.ShowLocProperty_CheckedChanged);
            // 
            // ExportDialog
            // 
            this.ExportDialog.Filter = "Spectrum screen|*.scr";
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(288, 537);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(88, 23);
            this.button8.TabIndex = 34;
            this.button8.Text = "Export tileset";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Location = new System.Drawing.Point(288, 566);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(88, 23);
            this.button9.TabIndex = 35;
            this.button9.Text = "Statistics";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button9_MouseDown);
            this.button9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button9_MouseUp);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(6, 566);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "Tile property";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ShowObjects
            // 
            this.ShowObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowObjects.AutoSize = true;
            this.ShowObjects.Checked = true;
            this.ShowObjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowObjects.Location = new System.Drawing.Point(316, 546);
            this.ShowObjects.Name = "ShowObjects";
            this.ShowObjects.Size = new System.Drawing.Size(88, 17);
            this.ShowObjects.TabIndex = 42;
            this.ShowObjects.Text = "show objects";
            this.ShowObjects.UseVisualStyleBackColor = true;
            this.ShowObjects.CheckedChanged += new System.EventHandler(this.ShowObjects_CheckedChanged);
            // 
            // SetNullTiles
            // 
            this.SetNullTiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetNullTiles.AutoSize = true;
            this.SetNullTiles.Checked = true;
            this.SetNullTiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SetNullTiles.Location = new System.Drawing.Point(447, 523);
            this.SetNullTiles.Name = "SetNullTiles";
            this.SetNullTiles.Size = new System.Drawing.Size(92, 17);
            this.SetNullTiles.TabIndex = 34;
            this.SetNullTiles.Text = "set empty tiles";
            this.SetNullTiles.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(364, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 44;
            this.label5.Text = "objects on screen";
            // 
            // ObjectsOnScreenEdit
            // 
            this.ObjectsOnScreenEdit.Location = new System.Drawing.Point(461, 6);
            this.ObjectsOnScreenEdit.Name = "ObjectsOnScreenEdit";
            this.ObjectsOnScreenEdit.Size = new System.Drawing.Size(56, 20);
            this.ObjectsOnScreenEdit.TabIndex = 43;
            this.ObjectsOnScreenEdit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CopyObjectButton
            // 
            this.CopyObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyObjectButton.Location = new System.Drawing.Point(188, 566);
            this.CopyObjectButton.Name = "CopyObjectButton";
            this.CopyObjectButton.Size = new System.Drawing.Size(88, 23);
            this.CopyObjectButton.TabIndex = 41;
            this.CopyObjectButton.Text = "Copy object";
            this.CopyObjectButton.UseVisualStyleBackColor = true;
            this.CopyObjectButton.Click += new System.EventHandler(this.CopyObjectButton_Click);
            // 
            // MoveUpObjectButton
            // 
            this.MoveUpObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveUpObjectButton.Location = new System.Drawing.Point(364, 510);
            this.MoveUpObjectButton.Name = "MoveUpObjectButton";
            this.MoveUpObjectButton.Size = new System.Drawing.Size(42, 23);
            this.MoveUpObjectButton.TabIndex = 40;
            this.MoveUpObjectButton.Text = "up";
            this.MoveUpObjectButton.UseVisualStyleBackColor = true;
            this.MoveUpObjectButton.Click += new System.EventHandler(this.MoveUpObjectButton_Click);
            // 
            // MoveDownObjectButton
            // 
            this.MoveDownObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MoveDownObjectButton.Location = new System.Drawing.Point(364, 537);
            this.MoveDownObjectButton.Name = "MoveDownObjectButton";
            this.MoveDownObjectButton.Size = new System.Drawing.Size(42, 23);
            this.MoveDownObjectButton.TabIndex = 39;
            this.MoveDownObjectButton.Text = "down";
            this.MoveDownObjectButton.UseVisualStyleBackColor = true;
            this.MoveDownObjectButton.Click += new System.EventHandler(this.MoveDownObjectButton_Click);
            // 
            // DeleteObjectButton
            // 
            this.DeleteObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteObjectButton.Location = new System.Drawing.Point(94, 566);
            this.DeleteObjectButton.Name = "DeleteObjectButton";
            this.DeleteObjectButton.Size = new System.Drawing.Size(88, 23);
            this.DeleteObjectButton.TabIndex = 38;
            this.DeleteObjectButton.Text = "Delete object";
            this.DeleteObjectButton.UseVisualStyleBackColor = true;
            this.DeleteObjectButton.Click += new System.EventHandler(this.DeleteObjectButton_Click);
            // 
            // AddObjectButton
            // 
            this.AddObjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddObjectButton.Location = new System.Drawing.Point(0, 566);
            this.AddObjectButton.Name = "AddObjectButton";
            this.AddObjectButton.Size = new System.Drawing.Size(88, 23);
            this.AddObjectButton.TabIndex = 37;
            this.AddObjectButton.Text = "Add object";
            this.AddObjectButton.UseVisualStyleBackColor = true;
            this.AddObjectButton.Click += new System.EventHandler(this.AddObjectButton_Click);
            // 
            // ObjectsList
            // 
            this.ObjectsList.AllowUserToAddRows = false;
            this.ObjectsList.AllowUserToDeleteRows = false;
            this.ObjectsList.AllowUserToResizeColumns = false;
            this.ObjectsList.AllowUserToResizeRows = false;
            this.ObjectsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ObjectsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column6});
            this.ObjectsList.Location = new System.Drawing.Point(6, 6);
            this.ObjectsList.MultiSelect = false;
            this.ObjectsList.Name = "ObjectsList";
            this.ObjectsList.RowHeadersVisible = false;
            this.ObjectsList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ObjectsList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ObjectsList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ObjectsList.Size = new System.Drawing.Size(352, 554);
            this.ObjectsList.TabIndex = 36;
            this.ObjectsList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ObjectsList_CellClick);
            this.ObjectsList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ObjectsList_CellEndEdit);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "object";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 124;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "xcord";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 50;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "ycord";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 50;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "yoffset";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.Width = 50;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "direct";
            this.Column6.Name = "Column6";
            this.Column6.Width = 50;
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Location = new System.Drawing.Point(553, 6);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(531, 621);
            this.TabControl.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.ShowTileProperty);
            this.tabPage1.Controls.Add(this.ShowTilesetGrid);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.SaveTilesetButton);
            this.tabPage1.Controls.Add(this.TilesetPicture);
            this.tabPage1.Controls.Add(this.button8);
            this.tabPage1.Controls.Add(this.TilesetCnt);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(523, 595);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tiles";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.button10);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.CopyObjectButton);
            this.tabPage2.Controls.Add(this.ObjectsOnScreenEdit);
            this.tabPage2.Controls.Add(this.MoveUpObjectButton);
            this.tabPage2.Controls.Add(this.ObjectsList);
            this.tabPage2.Controls.Add(this.MoveDownObjectButton);
            this.tabPage2.Controls.Add(this.AddObjectButton);
            this.tabPage2.Controls.Add(this.DeleteObjectButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(523, 595);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Objects";
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Location = new System.Drawing.Point(423, 566);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(88, 23);
            this.button10.TabIndex = 45;
            this.button10.Text = "Save Objects";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // AssemblyButton
            // 
            this.AssemblyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AssemblyButton.Location = new System.Drawing.Point(365, 594);
            this.AssemblyButton.Name = "AssemblyButton";
            this.AssemblyButton.Size = new System.Drawing.Size(88, 23);
            this.AssemblyButton.TabIndex = 45;
            this.AssemblyButton.Text = "Assembly";
            this.AssemblyButton.UseVisualStyleBackColor = true;
            this.AssemblyButton.Click += new System.EventHandler(this.button7_Click);
            // 
            // TilesetPicture
            // 
            this.TilesetPicture.Location = new System.Drawing.Point(6, 6);
            this.TilesetPicture.Name = "TilesetPicture";
            this.TilesetPicture.Size = new System.Drawing.Size(512, 512);
            this.TilesetPicture.TabIndex = 2;
            this.TilesetPicture.TabStop = false;
            this.TilesetPicture.DoubleClick += new System.EventHandler(this.TilesetPicture_DoubleClick);
            this.TilesetPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TilesetPicture_MouseDown);
            this.TilesetPicture.MouseLeave += new System.EventHandler(this.TilesetPicture_MouseLeave);
            this.TilesetPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TilesetPicture_MouseMove);
            this.TilesetPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TilesetPicture_MouseUp);
            // 
            // LocationEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 652);
            this.Controls.Add(this.AssemblyButton);
            this.Controls.Add(this.SaveLocButton);
            this.Controls.Add(this.ShowObjects);
            this.Controls.Add(this.XScroll);
            this.Controls.Add(this.ShowLocProperty);
            this.Controls.Add(this.SetNullTiles);
            this.Controls.Add(this.YScroll);
            this.Controls.Add(this.LocPicture);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ShowLocGrid);
            this.Controls.Add(this.LocUp);
            this.Controls.Add(this.LocSizeX);
            this.Controls.Add(this.LocName);
            this.Controls.Add(this.LocDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LocRight);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.LocLeft);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.LocSizeY);
            this.Controls.Add(this.label2);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1107, 690);
            this.Name = "LocationEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "World editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LocationEditorForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LocationEditorForm_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.LocationEditorForm_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.LocationEditorForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.LocSizeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocSizeY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocPicture)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectsList)).EndInit();
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TilesetPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TilesetCnt;
        private System.Windows.Forms.NumericUpDown LocSizeX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown LocSizeY;
        private System.Windows.Forms.PictureBox LocPicture;
        private System.Windows.Forms.VScrollBar YScroll;
        private System.Windows.Forms.HScrollBar XScroll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel LocPosX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel LocPosY;
        private System.Windows.Forms.ComboBox LocName;
        private System.Windows.Forms.Button SaveLocButton;
        private System.Windows.Forms.Button LocLeft;
        private System.Windows.Forms.Button LocRight;
        private System.Windows.Forms.Button LocDown;
        private System.Windows.Forms.Button LocUp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel TilesetLength;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel LocLength;
        private System.Windows.Forms.CheckBox ShowLocGrid;
        private System.Windows.Forms.CheckBox ShowTilesetGrid;
        private System.Windows.Forms.Button SaveTilesetButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Timer TileTimer;
        private System.Windows.Forms.CheckBox ShowTileProperty;
        private System.Windows.Forms.CheckBox ShowLocProperty;
        private System.Windows.Forms.SaveFileDialog ExportDialog;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox SetNullTiles;
        private System.Windows.Forms.DataGridView ObjectsList;
        private System.Windows.Forms.Button MoveUpObjectButton;
        private System.Windows.Forms.Button MoveDownObjectButton;
        private System.Windows.Forms.Button DeleteObjectButton;
        private System.Windows.Forms.Button AddObjectButton;
        private System.Windows.Forms.Button CopyObjectButton;
        private System.Windows.Forms.CheckBox ShowObjects;
        private System.Windows.Forms.TextBox ObjectsOnScreenEdit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.Button AssemblyButton;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.PictureBox TilesetPicture;
    }
}

