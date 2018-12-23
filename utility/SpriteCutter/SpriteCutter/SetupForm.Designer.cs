namespace SpriteCutter
{
    partial class SetupForm
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
            this.ColorDialog = new System.Windows.Forms.ColorDialog();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.FolderDialogPath = new System.Windows.Forms.FolderBrowserDialog();
            this.PathBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PathButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.EnableColorButton = new System.Windows.Forms.Button();
            this.DisableColorButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MaskColorButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectionColorButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ColliderColorButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.BinButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.BinBox = new System.Windows.Forms.TextBox();
            this.FolderDialogBin = new System.Windows.Forms.FolderBrowserDialog();
            this.SourceButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.SourceBox = new System.Windows.Forms.TextBox();
            this.FolderDialogSource = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(99, 231);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(195, 231);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Cancel";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // PathBox
            // 
            this.PathBox.Location = new System.Drawing.Point(74, 23);
            this.PathBox.Name = "PathBox";
            this.PathBox.Size = new System.Drawing.Size(260, 20);
            this.PathBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Work path";
            // 
            // PathButton
            // 
            this.PathButton.Location = new System.Drawing.Point(340, 24);
            this.PathButton.Name = "PathButton";
            this.PathButton.Size = new System.Drawing.Size(24, 19);
            this.PathButton.TabIndex = 5;
            this.PathButton.Text = "...";
            this.PathButton.UseVisualStyleBackColor = true;
            this.PathButton.Click += new System.EventHandler(this.PathButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Enable color";
            // 
            // EnableColorButton
            // 
            this.EnableColorButton.Location = new System.Drawing.Point(83, 112);
            this.EnableColorButton.Name = "EnableColorButton";
            this.EnableColorButton.Size = new System.Drawing.Size(25, 23);
            this.EnableColorButton.TabIndex = 7;
            this.EnableColorButton.UseVisualStyleBackColor = true;
            this.EnableColorButton.Click += new System.EventHandler(this.EnableColorButton_Click);
            // 
            // DisableColorButton
            // 
            this.DisableColorButton.Location = new System.Drawing.Point(83, 149);
            this.DisableColorButton.Name = "DisableColorButton";
            this.DisableColorButton.Size = new System.Drawing.Size(25, 23);
            this.DisableColorButton.TabIndex = 9;
            this.DisableColorButton.UseVisualStyleBackColor = true;
            this.DisableColorButton.Click += new System.EventHandler(this.DisableColorButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Disable color";
            // 
            // MaskColorButton
            // 
            this.MaskColorButton.Location = new System.Drawing.Point(83, 188);
            this.MaskColorButton.Name = "MaskColorButton";
            this.MaskColorButton.Size = new System.Drawing.Size(25, 23);
            this.MaskColorButton.TabIndex = 11;
            this.MaskColorButton.UseVisualStyleBackColor = true;
            this.MaskColorButton.Click += new System.EventHandler(this.MaskColorButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Mask color";
            // 
            // SelectionColorButton
            // 
            this.SelectionColorButton.Location = new System.Drawing.Point(216, 112);
            this.SelectionColorButton.Name = "SelectionColorButton";
            this.SelectionColorButton.Size = new System.Drawing.Size(25, 23);
            this.SelectionColorButton.TabIndex = 13;
            this.SelectionColorButton.UseVisualStyleBackColor = true;
            this.SelectionColorButton.Click += new System.EventHandler(this.SelectionColorButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(133, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Selection color";
            // 
            // ColliderColorButton
            // 
            this.ColliderColorButton.Location = new System.Drawing.Point(216, 149);
            this.ColliderColorButton.Name = "ColliderColorButton";
            this.ColliderColorButton.Size = new System.Drawing.Size(25, 23);
            this.ColliderColorButton.TabIndex = 15;
            this.ColliderColorButton.UseVisualStyleBackColor = true;
            this.ColliderColorButton.Click += new System.EventHandler(this.ColliderColorButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(133, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Collider color";
            // 
            // BinButton
            // 
            this.BinButton.Location = new System.Drawing.Point(340, 50);
            this.BinButton.Name = "BinButton";
            this.BinButton.Size = new System.Drawing.Size(24, 19);
            this.BinButton.TabIndex = 18;
            this.BinButton.Text = "...";
            this.BinButton.UseVisualStyleBackColor = true;
            this.BinButton.Click += new System.EventHandler(this.BinButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Bin path";
            // 
            // BinBox
            // 
            this.BinBox.Location = new System.Drawing.Point(74, 49);
            this.BinBox.Name = "BinBox";
            this.BinBox.Size = new System.Drawing.Size(260, 20);
            this.BinBox.TabIndex = 16;
            // 
            // SourceButton
            // 
            this.SourceButton.Location = new System.Drawing.Point(340, 76);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(24, 19);
            this.SourceButton.TabIndex = 21;
            this.SourceButton.Text = "...";
            this.SourceButton.UseVisualStyleBackColor = true;
            this.SourceButton.Click += new System.EventHandler(this.SourceButton_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Source path";
            // 
            // SourceBox
            // 
            this.SourceBox.Location = new System.Drawing.Point(74, 75);
            this.SourceBox.Name = "SourceBox";
            this.SourceBox.Size = new System.Drawing.Size(260, 20);
            this.SourceBox.TabIndex = 19;
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 263);
            this.Controls.Add(this.SourceButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.SourceBox);
            this.Controls.Add(this.BinButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BinBox);
            this.Controls.Add(this.ColliderColorButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SelectionColorButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.MaskColorButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DisableColorButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EnableColorButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PathButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PathBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.SaveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog ColorDialog;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.FolderBrowserDialog FolderDialogPath;
        private System.Windows.Forms.TextBox PathBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button PathButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button EnableColorButton;
        private System.Windows.Forms.Button DisableColorButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button MaskColorButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button SelectionColorButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ColliderColorButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BinButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox BinBox;
        private System.Windows.Forms.FolderBrowserDialog FolderDialogBin;
        private System.Windows.Forms.Button SourceButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SourceBox;
        private System.Windows.Forms.FolderBrowserDialog FolderDialogSource;
    }
}