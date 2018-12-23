namespace WorldEditor
{
    partial class TilesetEditorForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.XposText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.YposText = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CaptureColor = new System.Windows.Forms.CheckBox();
            this.ColorsCheck = new System.Windows.Forms.CheckBox();
            this.BrightCheck = new System.Windows.Forms.CheckBox();
            this.RedoButton = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ScaleBar = new System.Windows.Forms.TrackBar();
            this.ShowGrid = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.PalettePanel = new System.Windows.Forms.Panel();
            this.P7 = new System.Windows.Forms.Label();
            this.I7 = new System.Windows.Forms.Label();
            this.P6 = new System.Windows.Forms.Label();
            this.I6 = new System.Windows.Forms.Label();
            this.P5 = new System.Windows.Forms.Label();
            this.I5 = new System.Windows.Forms.Label();
            this.P4 = new System.Windows.Forms.Label();
            this.I4 = new System.Windows.Forms.Label();
            this.P3 = new System.Windows.Forms.Label();
            this.I3 = new System.Windows.Forms.Label();
            this.P2 = new System.Windows.Forms.Label();
            this.I2 = new System.Windows.Forms.Label();
            this.P1 = new System.Windows.Forms.Label();
            this.I1 = new System.Windows.Forms.Label();
            this.P0 = new System.Windows.Forms.Label();
            this.I0 = new System.Windows.Forms.Label();
            this.AttrColor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ScrollPanel = new System.Windows.Forms.Panel();
            this.ViewImg = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBar)).BeginInit();
            this.PalettePanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ScrollPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ViewImg)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.XposText,
            this.toolStripStatusLabel2,
            this.YposText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 476);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(567, 22);
            this.statusStrip1.TabIndex = 31;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "X pos";
            // 
            // XposText
            // 
            this.XposText.AutoSize = false;
            this.XposText.Name = "XposText";
            this.XposText.Size = new System.Drawing.Size(32, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel2.Text = "Y pos";
            // 
            // YposText
            // 
            this.YposText.AutoSize = false;
            this.YposText.Name = "YposText";
            this.YposText.Size = new System.Drawing.Size(32, 17);
            // 
            // CaptureColor
            // 
            this.CaptureColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureColor.Appearance = System.Windows.Forms.Appearance.Button;
            this.CaptureColor.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CaptureColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.CaptureColor.Location = new System.Drawing.Point(14, 339);
            this.CaptureColor.Name = "CaptureColor";
            this.CaptureColor.Size = new System.Drawing.Size(52, 49);
            this.CaptureColor.TabIndex = 55;
            this.CaptureColor.Text = "Capture color";
            this.CaptureColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.CaptureColor, "Press \"C\" on tileset.");
            this.CaptureColor.UseVisualStyleBackColor = true;
            // 
            // ColorsCheck
            // 
            this.ColorsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorsCheck.AutoSize = true;
            this.ColorsCheck.Location = new System.Drawing.Point(15, 8);
            this.ColorsCheck.Name = "ColorsCheck";
            this.ColorsCheck.Size = new System.Drawing.Size(55, 17);
            this.ColorsCheck.TabIndex = 53;
            this.ColorsCheck.Text = "Color&s";
            this.toolTip1.SetToolTip(this.ColorsCheck, "Press \"S\"");
            this.ColorsCheck.UseVisualStyleBackColor = true;
            this.ColorsCheck.CheckedChanged += new System.EventHandler(this.ColorsCheck_CheckedChanged);
            // 
            // BrightCheck
            // 
            this.BrightCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrightCheck.AutoSize = true;
            this.BrightCheck.Location = new System.Drawing.Point(13, 314);
            this.BrightCheck.Name = "BrightCheck";
            this.BrightCheck.Size = new System.Drawing.Size(53, 17);
            this.BrightCheck.TabIndex = 52;
            this.BrightCheck.Text = "&Bright";
            this.toolTip1.SetToolTip(this.BrightCheck, "Press \"B\"");
            this.BrightCheck.UseVisualStyleBackColor = true;
            this.BrightCheck.CheckedChanged += new System.EventHandler(this.BrightCheck_CheckedChanged);
            // 
            // RedoButton
            // 
            this.RedoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RedoButton.Location = new System.Drawing.Point(75, 8);
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(60, 23);
            this.RedoButton.TabIndex = 56;
            this.RedoButton.Text = "Redo";
            this.toolTip1.SetToolTip(this.RedoButton, "\"Ctrl\" + \"Y\"");
            this.RedoButton.UseVisualStyleBackColor = true;
            this.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UndoButton.Location = new System.Drawing.Point(9, 8);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(60, 23);
            this.UndoButton.TabIndex = 55;
            this.UndoButton.Text = "Undo";
            this.toolTip1.SetToolTip(this.UndoButton, "\"Ctrl\" + \"Z\"");
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(185, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Scale";
            this.toolTip1.SetToolTip(this.label3, "Press \"+\" or \"-\"");
            // 
            // ScaleBar
            // 
            this.ScaleBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ScaleBar.AutoSize = false;
            this.ScaleBar.Location = new System.Drawing.Point(225, 12);
            this.ScaleBar.Maximum = 16;
            this.ScaleBar.Minimum = 2;
            this.ScaleBar.Name = "ScaleBar";
            this.ScaleBar.Size = new System.Drawing.Size(166, 22);
            this.ScaleBar.SmallChange = 2;
            this.ScaleBar.TabIndex = 52;
            this.ScaleBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.ScaleBar, "Press \"+\" or \"-\"");
            this.ScaleBar.Value = 9;
            this.ScaleBar.Scroll += new System.EventHandler(this.ScaleBar_Scroll);
            // 
            // ShowGrid
            // 
            this.ShowGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowGrid.AutoSize = true;
            this.ShowGrid.Checked = true;
            this.ShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGrid.Location = new System.Drawing.Point(408, 8);
            this.ShowGrid.Name = "ShowGrid";
            this.ShowGrid.Size = new System.Drawing.Size(75, 17);
            this.ShowGrid.TabIndex = 53;
            this.ShowGrid.Text = "Show &Grid";
            this.toolTip1.SetToolTip(this.ShowGrid, "Press \"G\"");
            this.ShowGrid.UseVisualStyleBackColor = true;
            this.ShowGrid.CheckedChanged += new System.EventHandler(this.ShowGrid_CheckedChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(498, 408);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(58, 56);
            this.SaveButton.TabIndex = 72;
            this.SaveButton.Text = "Save changes";
            this.toolTip1.SetToolTip(this.SaveButton, "\"Ctrl\" + \"S\"");
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // PalettePanel
            // 
            this.PalettePanel.Controls.Add(this.P7);
            this.PalettePanel.Controls.Add(this.I7);
            this.PalettePanel.Controls.Add(this.P6);
            this.PalettePanel.Controls.Add(this.I6);
            this.PalettePanel.Controls.Add(this.P5);
            this.PalettePanel.Controls.Add(this.I5);
            this.PalettePanel.Controls.Add(this.P4);
            this.PalettePanel.Controls.Add(this.I4);
            this.PalettePanel.Controls.Add(this.P3);
            this.PalettePanel.Controls.Add(this.I3);
            this.PalettePanel.Controls.Add(this.P2);
            this.PalettePanel.Controls.Add(this.I2);
            this.PalettePanel.Controls.Add(this.P1);
            this.PalettePanel.Controls.Add(this.I1);
            this.PalettePanel.Controls.Add(this.P0);
            this.PalettePanel.Controls.Add(this.I0);
            this.PalettePanel.Controls.Add(this.CaptureColor);
            this.PalettePanel.Controls.Add(this.AttrColor);
            this.PalettePanel.Controls.Add(this.ColorsCheck);
            this.PalettePanel.Controls.Add(this.BrightCheck);
            this.PalettePanel.Controls.Add(this.label2);
            this.PalettePanel.Controls.Add(this.label1);
            this.PalettePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.PalettePanel.Location = new System.Drawing.Point(486, 0);
            this.PalettePanel.Name = "PalettePanel";
            this.PalettePanel.Size = new System.Drawing.Size(81, 476);
            this.PalettePanel.TabIndex = 53;
            // 
            // P7
            // 
            this.P7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P7.Location = new System.Drawing.Point(45, 281);
            this.P7.Name = "P7";
            this.P7.Size = new System.Drawing.Size(21, 21);
            this.P7.TabIndex = 71;
            this.P7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P7.Click += new System.EventHandler(this.P7_Click);
            // 
            // I7
            // 
            this.I7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I7.Location = new System.Drawing.Point(13, 281);
            this.I7.Name = "I7";
            this.I7.Size = new System.Drawing.Size(21, 21);
            this.I7.TabIndex = 70;
            this.I7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I7.Click += new System.EventHandler(this.I7_Click);
            // 
            // P6
            // 
            this.P6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P6.Location = new System.Drawing.Point(45, 256);
            this.P6.Name = "P6";
            this.P6.Size = new System.Drawing.Size(21, 21);
            this.P6.TabIndex = 69;
            this.P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P6.Click += new System.EventHandler(this.P6_Click);
            // 
            // I6
            // 
            this.I6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I6.Location = new System.Drawing.Point(13, 256);
            this.I6.Name = "I6";
            this.I6.Size = new System.Drawing.Size(21, 21);
            this.I6.TabIndex = 68;
            this.I6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I6.Click += new System.EventHandler(this.I6_Click);
            // 
            // P5
            // 
            this.P5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P5.Location = new System.Drawing.Point(45, 231);
            this.P5.Name = "P5";
            this.P5.Size = new System.Drawing.Size(21, 21);
            this.P5.TabIndex = 67;
            this.P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P5.Click += new System.EventHandler(this.P5_Click);
            // 
            // I5
            // 
            this.I5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I5.Location = new System.Drawing.Point(13, 231);
            this.I5.Name = "I5";
            this.I5.Size = new System.Drawing.Size(21, 21);
            this.I5.TabIndex = 66;
            this.I5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I5.Click += new System.EventHandler(this.I5_Click);
            // 
            // P4
            // 
            this.P4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P4.Location = new System.Drawing.Point(45, 206);
            this.P4.Name = "P4";
            this.P4.Size = new System.Drawing.Size(21, 21);
            this.P4.TabIndex = 65;
            this.P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P4.Click += new System.EventHandler(this.P4_Click);
            // 
            // I4
            // 
            this.I4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I4.Location = new System.Drawing.Point(13, 206);
            this.I4.Name = "I4";
            this.I4.Size = new System.Drawing.Size(21, 21);
            this.I4.TabIndex = 64;
            this.I4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I4.Click += new System.EventHandler(this.I4_Click);
            // 
            // P3
            // 
            this.P3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P3.Location = new System.Drawing.Point(45, 181);
            this.P3.Name = "P3";
            this.P3.Size = new System.Drawing.Size(21, 21);
            this.P3.TabIndex = 63;
            this.P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P3.Click += new System.EventHandler(this.P3_Click);
            // 
            // I3
            // 
            this.I3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I3.Location = new System.Drawing.Point(13, 181);
            this.I3.Name = "I3";
            this.I3.Size = new System.Drawing.Size(21, 21);
            this.I3.TabIndex = 62;
            this.I3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I3.Click += new System.EventHandler(this.I3_Click);
            // 
            // P2
            // 
            this.P2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P2.Location = new System.Drawing.Point(45, 156);
            this.P2.Name = "P2";
            this.P2.Size = new System.Drawing.Size(21, 21);
            this.P2.TabIndex = 61;
            this.P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P2.Click += new System.EventHandler(this.P2_Click);
            // 
            // I2
            // 
            this.I2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I2.Location = new System.Drawing.Point(13, 156);
            this.I2.Name = "I2";
            this.I2.Size = new System.Drawing.Size(21, 21);
            this.I2.TabIndex = 60;
            this.I2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I2.Click += new System.EventHandler(this.I2_Click);
            // 
            // P1
            // 
            this.P1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P1.Location = new System.Drawing.Point(45, 131);
            this.P1.Name = "P1";
            this.P1.Size = new System.Drawing.Size(21, 21);
            this.P1.TabIndex = 59;
            this.P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P1.Click += new System.EventHandler(this.P1_Click);
            // 
            // I1
            // 
            this.I1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I1.Location = new System.Drawing.Point(13, 131);
            this.I1.Name = "I1";
            this.I1.Size = new System.Drawing.Size(21, 21);
            this.I1.TabIndex = 58;
            this.I1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I1.Click += new System.EventHandler(this.I1_Click);
            // 
            // P0
            // 
            this.P0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.P0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.P0.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.P0.Location = new System.Drawing.Point(45, 106);
            this.P0.Name = "P0";
            this.P0.Size = new System.Drawing.Size(21, 21);
            this.P0.TabIndex = 57;
            this.P0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.P0.Click += new System.EventHandler(this.P0_Click);
            // 
            // I0
            // 
            this.I0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.I0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.I0.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.I0.Location = new System.Drawing.Point(13, 106);
            this.I0.Name = "I0";
            this.I0.Size = new System.Drawing.Size(21, 21);
            this.I0.TabIndex = 56;
            this.I0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.I0.Click += new System.EventHandler(this.I0_Click);
            // 
            // AttrColor
            // 
            this.AttrColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AttrColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AttrColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AttrColor.Location = new System.Drawing.Point(15, 37);
            this.AttrColor.Name = "AttrColor";
            this.AttrColor.Size = new System.Drawing.Size(47, 44);
            this.AttrColor.TabIndex = 54;
            this.AttrColor.Text = "ink";
            this.AttrColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Ink";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Paper";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RedoButton);
            this.panel1.Controls.Add(this.UndoButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ScaleBar);
            this.panel1.Controls.Add(this.ShowGrid);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 433);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(486, 43);
            this.panel1.TabIndex = 73;
            // 
            // ScrollPanel
            // 
            this.ScrollPanel.AutoScroll = true;
            this.ScrollPanel.Controls.Add(this.ViewImg);
            this.ScrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScrollPanel.Location = new System.Drawing.Point(0, 0);
            this.ScrollPanel.Name = "ScrollPanel";
            this.ScrollPanel.Size = new System.Drawing.Size(486, 433);
            this.ScrollPanel.TabIndex = 74;
            // 
            // ViewImg
            // 
            this.ViewImg.Location = new System.Drawing.Point(3, 3);
            this.ViewImg.Name = "ViewImg";
            this.ViewImg.Size = new System.Drawing.Size(118, 118);
            this.ViewImg.TabIndex = 3;
            this.ViewImg.TabStop = false;
            this.ViewImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewImg_MouseDown);
            this.ViewImg.MouseLeave += new System.EventHandler(this.ViewImg_MouseLeave);
            this.ViewImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewImg_MouseMove);
            this.ViewImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewImg_MouseUp);
            // 
            // TilesetEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 498);
            this.Controls.Add(this.ScrollPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.PalettePanel);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(583, 536);
            this.Name = "TilesetEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tileset Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TilesetEditorForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TilesetEditorForm_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleBar)).EndInit();
            this.PalettePanel.ResumeLayout(false);
            this.PalettePanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ScrollPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ViewImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel XposText;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel YposText;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel PalettePanel;
        private System.Windows.Forms.Label P7;
        private System.Windows.Forms.Label I7;
        private System.Windows.Forms.Label P6;
        private System.Windows.Forms.Label I6;
        private System.Windows.Forms.Label P5;
        private System.Windows.Forms.Label I5;
        private System.Windows.Forms.Label P4;
        private System.Windows.Forms.Label I4;
        private System.Windows.Forms.Label P3;
        private System.Windows.Forms.Label I3;
        private System.Windows.Forms.Label P2;
        private System.Windows.Forms.Label I2;
        private System.Windows.Forms.Label P1;
        private System.Windows.Forms.Label I1;
        private System.Windows.Forms.Label P0;
        private System.Windows.Forms.Label I0;
        private System.Windows.Forms.CheckBox CaptureColor;
        private System.Windows.Forms.Label AttrColor;
        private System.Windows.Forms.CheckBox ColorsCheck;
        private System.Windows.Forms.CheckBox BrightCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button RedoButton;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar ScaleBar;
        private System.Windows.Forms.CheckBox ShowGrid;
        private System.Windows.Forms.Panel ScrollPanel;
        private System.Windows.Forms.PictureBox ViewImg;
    }
}