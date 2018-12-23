namespace ScreensMaker
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.SpritesList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpriteCaptureButton = new System.Windows.Forms.Button();
            this.SpriteDeleteButton = new System.Windows.Forms.Button();
            this.SpriteRenameButton = new System.Windows.Forms.Button();
            this.SpriteImg = new System.Windows.Forms.PictureBox();
            this.SpritesDownButton = new System.Windows.Forms.Button();
            this.SpritesUpButton = new System.Windows.Forms.Button();
            this.ScreenImg = new System.Windows.Forms.PictureBox();
            this.ScreenSpriteList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.ScreenSpriteUpButton = new System.Windows.Forms.Button();
            this.ScreenSpriteDownButton = new System.Windows.Forms.Button();
            this.ScreenSpriteDeleteButton = new System.Windows.Forms.Button();
            this.ScreenSpriteAddButton = new System.Windows.Forms.Button();
            this.ScreensDeleteButton = new System.Windows.Forms.Button();
            this.ScreensAddButton = new System.Windows.Forms.Button();
            this.ScreensUpButton = new System.Windows.Forms.Button();
            this.ScreensDownButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ScreensList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.ScreenRenameButton = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusXCord = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusYCord = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSpriteLenght = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusScreenLenght = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusTotalLenght = new System.Windows.Forms.ToolStripStatusLabel();
            this.CheckScreens = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SpritesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpriteImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenSpriteList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreensList)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sprites";
            // 
            // SpritesList
            // 
            this.SpritesList.AllowUserToAddRows = false;
            this.SpritesList.AllowUserToDeleteRows = false;
            this.SpritesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SpritesList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.SpritesList.Location = new System.Drawing.Point(14, 48);
            this.SpritesList.MultiSelect = false;
            this.SpritesList.Name = "SpritesList";
            this.SpritesList.RowHeadersVisible = false;
            this.SpritesList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SpritesList.Size = new System.Drawing.Size(223, 430);
            this.SpritesList.TabIndex = 1;
            this.SpritesList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SpritesList_CellEndEdit);
            this.SpritesList.CurrentCellChanged += new System.EventHandler(this.SpritesList_CurrentCellChanged);
            this.SpritesList.DoubleClick += new System.EventHandler(this.SpritesList_DoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 140;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Size";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 60;
            // 
            // SpriteCaptureButton
            // 
            this.SpriteCaptureButton.Location = new System.Drawing.Point(14, 661);
            this.SpriteCaptureButton.Name = "SpriteCaptureButton";
            this.SpriteCaptureButton.Size = new System.Drawing.Size(67, 23);
            this.SpriteCaptureButton.TabIndex = 2;
            this.SpriteCaptureButton.Text = "Capture";
            this.SpriteCaptureButton.UseVisualStyleBackColor = true;
            this.SpriteCaptureButton.Click += new System.EventHandler(this.SpriteCaptureButton_Click);
            // 
            // SpriteDeleteButton
            // 
            this.SpriteDeleteButton.Location = new System.Drawing.Point(87, 661);
            this.SpriteDeleteButton.Name = "SpriteDeleteButton";
            this.SpriteDeleteButton.Size = new System.Drawing.Size(67, 23);
            this.SpriteDeleteButton.TabIndex = 3;
            this.SpriteDeleteButton.Text = "Delete";
            this.SpriteDeleteButton.UseVisualStyleBackColor = true;
            this.SpriteDeleteButton.Click += new System.EventHandler(this.SpriteDeleteButton_Click);
            // 
            // SpriteRenameButton
            // 
            this.SpriteRenameButton.Location = new System.Drawing.Point(160, 661);
            this.SpriteRenameButton.Name = "SpriteRenameButton";
            this.SpriteRenameButton.Size = new System.Drawing.Size(67, 23);
            this.SpriteRenameButton.TabIndex = 4;
            this.SpriteRenameButton.Text = "Rename";
            this.SpriteRenameButton.UseVisualStyleBackColor = true;
            this.SpriteRenameButton.Click += new System.EventHandler(this.SpriteRenameButton_Click);
            // 
            // SpriteImg
            // 
            this.SpriteImg.Location = new System.Drawing.Point(14, 484);
            this.SpriteImg.Name = "SpriteImg";
            this.SpriteImg.Size = new System.Drawing.Size(222, 171);
            this.SpriteImg.TabIndex = 5;
            this.SpriteImg.TabStop = false;
            // 
            // SpritesDownButton
            // 
            this.SpritesDownButton.Location = new System.Drawing.Point(243, 80);
            this.SpritesDownButton.Name = "SpritesDownButton";
            this.SpritesDownButton.Size = new System.Drawing.Size(45, 26);
            this.SpritesDownButton.TabIndex = 9;
            this.SpritesDownButton.Text = "down";
            this.SpritesDownButton.UseVisualStyleBackColor = true;
            this.SpritesDownButton.Click += new System.EventHandler(this.SpritesDownButton_Click);
            // 
            // SpritesUpButton
            // 
            this.SpritesUpButton.Location = new System.Drawing.Point(244, 48);
            this.SpritesUpButton.Name = "SpritesUpButton";
            this.SpritesUpButton.Size = new System.Drawing.Size(44, 26);
            this.SpritesUpButton.TabIndex = 10;
            this.SpritesUpButton.Text = "up";
            this.SpritesUpButton.UseVisualStyleBackColor = true;
            this.SpritesUpButton.Click += new System.EventHandler(this.SpritesUpButton_Click);
            // 
            // ScreenImg
            // 
            this.ScreenImg.Location = new System.Drawing.Point(313, 47);
            this.ScreenImg.Name = "ScreenImg";
            this.ScreenImg.Size = new System.Drawing.Size(512, 384);
            this.ScreenImg.TabIndex = 12;
            this.ScreenImg.TabStop = false;
            this.ScreenImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenImg_MouseDown);
            this.ScreenImg.MouseLeave += new System.EventHandler(this.ScreenImg_MouseLeave);
            this.ScreenImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScreenImg_MouseMove);
            this.ScreenImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreenImg_MouseUp);
            // 
            // ScreenSpriteList
            // 
            this.ScreenSpriteList.AllowUserToAddRows = false;
            this.ScreenSpriteList.AllowUserToDeleteRows = false;
            this.ScreenSpriteList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScreenSpriteList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column3,
            this.Column4});
            this.ScreenSpriteList.Location = new System.Drawing.Point(244, 455);
            this.ScreenSpriteList.MultiSelect = false;
            this.ScreenSpriteList.Name = "ScreenSpriteList";
            this.ScreenSpriteList.RowHeadersVisible = false;
            this.ScreenSpriteList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ScreenSpriteList.Size = new System.Drawing.Size(285, 200);
            this.ScreenSpriteList.TabIndex = 13;
            this.ScreenSpriteList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScreenSpriteList_CellEndEdit);
            this.ScreenSpriteList.CurrentCellChanged += new System.EventHandler(this.ScreenSpriteList_CurrentCellChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 140;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "XCord";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 40;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "YCord";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 40;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Attr";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Screen Sprites";
            // 
            // ScreenSpriteUpButton
            // 
            this.ScreenSpriteUpButton.Location = new System.Drawing.Point(535, 455);
            this.ScreenSpriteUpButton.Name = "ScreenSpriteUpButton";
            this.ScreenSpriteUpButton.Size = new System.Drawing.Size(44, 26);
            this.ScreenSpriteUpButton.TabIndex = 18;
            this.ScreenSpriteUpButton.Text = "up";
            this.ScreenSpriteUpButton.UseVisualStyleBackColor = true;
            this.ScreenSpriteUpButton.Click += new System.EventHandler(this.ScreenSpriteUpButton_Click);
            // 
            // ScreenSpriteDownButton
            // 
            this.ScreenSpriteDownButton.Location = new System.Drawing.Point(534, 487);
            this.ScreenSpriteDownButton.Name = "ScreenSpriteDownButton";
            this.ScreenSpriteDownButton.Size = new System.Drawing.Size(45, 26);
            this.ScreenSpriteDownButton.TabIndex = 17;
            this.ScreenSpriteDownButton.Text = "down";
            this.ScreenSpriteDownButton.UseVisualStyleBackColor = true;
            this.ScreenSpriteDownButton.Click += new System.EventHandler(this.ScreenSpriteDownButton_Click);
            // 
            // ScreenSpriteDeleteButton
            // 
            this.ScreenSpriteDeleteButton.Location = new System.Drawing.Point(317, 661);
            this.ScreenSpriteDeleteButton.Name = "ScreenSpriteDeleteButton";
            this.ScreenSpriteDeleteButton.Size = new System.Drawing.Size(67, 23);
            this.ScreenSpriteDeleteButton.TabIndex = 20;
            this.ScreenSpriteDeleteButton.Text = "Delete";
            this.ScreenSpriteDeleteButton.UseVisualStyleBackColor = true;
            this.ScreenSpriteDeleteButton.Click += new System.EventHandler(this.ScreenSpriteDeleteButton_Click);
            // 
            // ScreenSpriteAddButton
            // 
            this.ScreenSpriteAddButton.Location = new System.Drawing.Point(244, 661);
            this.ScreenSpriteAddButton.Name = "ScreenSpriteAddButton";
            this.ScreenSpriteAddButton.Size = new System.Drawing.Size(67, 23);
            this.ScreenSpriteAddButton.TabIndex = 19;
            this.ScreenSpriteAddButton.Text = "Add";
            this.ScreenSpriteAddButton.UseVisualStyleBackColor = true;
            this.ScreenSpriteAddButton.Click += new System.EventHandler(this.ScreenSpriteAddButton_Click);
            // 
            // ScreensDeleteButton
            // 
            this.ScreensDeleteButton.Location = new System.Drawing.Point(658, 661);
            this.ScreensDeleteButton.Name = "ScreensDeleteButton";
            this.ScreensDeleteButton.Size = new System.Drawing.Size(67, 23);
            this.ScreensDeleteButton.TabIndex = 26;
            this.ScreensDeleteButton.Text = "Delete";
            this.ScreensDeleteButton.UseVisualStyleBackColor = true;
            this.ScreensDeleteButton.Click += new System.EventHandler(this.ScreensDeleteButton_Click);
            // 
            // ScreensAddButton
            // 
            this.ScreensAddButton.Location = new System.Drawing.Point(585, 661);
            this.ScreensAddButton.Name = "ScreensAddButton";
            this.ScreensAddButton.Size = new System.Drawing.Size(67, 23);
            this.ScreensAddButton.TabIndex = 25;
            this.ScreensAddButton.Text = "Add";
            this.ScreensAddButton.UseVisualStyleBackColor = true;
            this.ScreensAddButton.Click += new System.EventHandler(this.ScreensAddButton_Click);
            // 
            // ScreensUpButton
            // 
            this.ScreensUpButton.Location = new System.Drawing.Point(805, 455);
            this.ScreensUpButton.Name = "ScreensUpButton";
            this.ScreensUpButton.Size = new System.Drawing.Size(44, 26);
            this.ScreensUpButton.TabIndex = 24;
            this.ScreensUpButton.Text = "up";
            this.ScreensUpButton.UseVisualStyleBackColor = true;
            this.ScreensUpButton.Click += new System.EventHandler(this.ScreensUpButton_Click);
            // 
            // ScreensDownButton
            // 
            this.ScreensDownButton.Location = new System.Drawing.Point(804, 487);
            this.ScreensDownButton.Name = "ScreensDownButton";
            this.ScreensDownButton.Size = new System.Drawing.Size(45, 26);
            this.ScreensDownButton.TabIndex = 23;
            this.ScreensDownButton.Text = "down";
            this.ScreensDownButton.UseVisualStyleBackColor = true;
            this.ScreensDownButton.Click += new System.EventHandler(this.ScreensDownButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(582, 439);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Screens";
            // 
            // ScreensList
            // 
            this.ScreensList.AllowUserToAddRows = false;
            this.ScreensList.AllowUserToDeleteRows = false;
            this.ScreensList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScreensList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn1});
            this.ScreensList.Location = new System.Drawing.Point(585, 455);
            this.ScreensList.MultiSelect = false;
            this.ScreensList.Name = "ScreensList";
            this.ScreensList.RowHeadersVisible = false;
            this.ScreensList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ScreensList.Size = new System.Drawing.Size(213, 200);
            this.ScreensList.TabIndex = 21;
            this.ScreensList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScreensList_CellEndEdit);
            this.ScreensList.CurrentCellChanged += new System.EventHandler(this.ScreensList_CurrentCellChanged);
            this.ScreensList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreensList_MouseUp);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 152;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Visible";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Screen view";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.compileSaveToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(883, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // compileSaveToolStripMenuItem
            // 
            this.compileSaveToolStripMenuItem.Name = "compileSaveToolStripMenuItem";
            this.compileSaveToolStripMenuItem.Size = new System.Drawing.Size(114, 20);
            this.compileSaveToolStripMenuItem.Text = "Compile and Save";
            this.compileSaveToolStripMenuItem.Click += new System.EventHandler(this.compileSaveToolStripMenuItem_Click);
            // 
            // OpenProjectDialog
            // 
            this.OpenProjectDialog.Filter = "Screen Maker project|*.smproj";
            // 
            // SaveProjectDialog
            // 
            this.SaveProjectDialog.Filter = "Screen Maker project|*.smproj";
            // 
            // ScreenRenameButton
            // 
            this.ScreenRenameButton.Location = new System.Drawing.Point(731, 661);
            this.ScreenRenameButton.Name = "ScreenRenameButton";
            this.ScreenRenameButton.Size = new System.Drawing.Size(67, 23);
            this.ScreenRenameButton.TabIndex = 30;
            this.ScreenRenameButton.Text = "Rename";
            this.ScreenRenameButton.UseVisualStyleBackColor = true;
            this.ScreenRenameButton.Click += new System.EventHandler(this.ScreenRenameButton_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.StatusXCord,
            this.toolStripStatusLabel3,
            this.StatusYCord,
            this.toolStripStatusLabel2,
            this.StatusSpriteLenght,
            this.toolStripStatusLabel4,
            this.StatusScreenLenght,
            this.toolStripStatusLabel5,
            this.StatusTotalLenght});
            this.Status.Location = new System.Drawing.Point(0, 690);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(883, 22);
            this.Status.TabIndex = 31;
            this.Status.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "X pos";
            // 
            // StatusXCord
            // 
            this.StatusXCord.AutoSize = false;
            this.StatusXCord.Name = "StatusXCord";
            this.StatusXCord.Size = new System.Drawing.Size(32, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel3.Text = "Y pos";
            // 
            // StatusYCord
            // 
            this.StatusYCord.AutoSize = false;
            this.StatusYCord.Name = "StatusYCord";
            this.StatusYCord.Size = new System.Drawing.Size(32, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(74, 17);
            this.toolStripStatusLabel2.Text = "Sprite lenght";
            // 
            // StatusSpriteLenght
            // 
            this.StatusSpriteLenght.AutoSize = false;
            this.StatusSpriteLenght.Name = "StatusSpriteLenght";
            this.StatusSpriteLenght.Size = new System.Drawing.Size(64, 17);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel4.Text = "Screen lenght";
            // 
            // StatusScreenLenght
            // 
            this.StatusScreenLenght.AutoSize = false;
            this.StatusScreenLenght.Name = "StatusScreenLenght";
            this.StatusScreenLenght.Size = new System.Drawing.Size(64, 17);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabel5.Text = "Total lenght";
            // 
            // StatusTotalLenght
            // 
            this.StatusTotalLenght.AutoSize = false;
            this.StatusTotalLenght.Name = "StatusTotalLenght";
            this.StatusTotalLenght.Size = new System.Drawing.Size(64, 17);
            // 
            // CheckScreens
            // 
            this.CheckScreens.AutoSize = true;
            this.CheckScreens.Checked = true;
            this.CheckScreens.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.CheckScreens.Location = new System.Drawing.Point(753, 439);
            this.CheckScreens.Name = "CheckScreens";
            this.CheckScreens.Size = new System.Drawing.Size(15, 14);
            this.CheckScreens.TabIndex = 32;
            this.CheckScreens.UseVisualStyleBackColor = true;
            this.CheckScreens.CheckedChanged += new System.EventHandler(this.CheckScreens_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 712);
            this.Controls.Add(this.CheckScreens);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.ScreenRenameButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ScreensDeleteButton);
            this.Controls.Add(this.ScreensAddButton);
            this.Controls.Add(this.ScreensUpButton);
            this.Controls.Add(this.ScreensDownButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ScreensList);
            this.Controls.Add(this.ScreenSpriteDeleteButton);
            this.Controls.Add(this.ScreenSpriteAddButton);
            this.Controls.Add(this.ScreenSpriteUpButton);
            this.Controls.Add(this.ScreenSpriteDownButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ScreenSpriteList);
            this.Controls.Add(this.ScreenImg);
            this.Controls.Add(this.SpritesUpButton);
            this.Controls.Add(this.SpritesDownButton);
            this.Controls.Add(this.SpriteImg);
            this.Controls.Add(this.SpriteRenameButton);
            this.Controls.Add(this.SpriteDeleteButton);
            this.Controls.Add(this.SpriteCaptureButton);
            this.Controls.Add(this.SpritesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Screens Maker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SpritesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpriteImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenSpriteList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScreensList)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView SpritesList;
        private System.Windows.Forms.Button SpriteCaptureButton;
        private System.Windows.Forms.Button SpriteDeleteButton;
        private System.Windows.Forms.Button SpriteRenameButton;
        private System.Windows.Forms.PictureBox SpriteImg;
        private System.Windows.Forms.Button SpritesDownButton;
        private System.Windows.Forms.Button SpritesUpButton;
        private System.Windows.Forms.PictureBox ScreenImg;
        private System.Windows.Forms.DataGridView ScreenSpriteList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ScreenSpriteUpButton;
        private System.Windows.Forms.Button ScreenSpriteDownButton;
        private System.Windows.Forms.Button ScreenSpriteDeleteButton;
        private System.Windows.Forms.Button ScreenSpriteAddButton;
        private System.Windows.Forms.Button ScreensDeleteButton;
        private System.Windows.Forms.Button ScreensAddButton;
        private System.Windows.Forms.Button ScreensUpButton;
        private System.Windows.Forms.Button ScreensDownButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView ScreensList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compileSaveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenProjectDialog;
        private System.Windows.Forms.SaveFileDialog SaveProjectDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button ScreenRenameButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel StatusXCord;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel StatusYCord;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusSpriteLenght;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel StatusScreenLenght;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel StatusTotalLenght;
        private System.Windows.Forms.CheckBox CheckScreens;
    }
}

