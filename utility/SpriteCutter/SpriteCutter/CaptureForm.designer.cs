namespace SpriteCutter
{
    partial class CaptureForm
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
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AutoCaptureButton = new System.Windows.Forms.Button();
            this.CaptureButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.SpriteType0 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.Title = "Open picture";
            // 
            // AutoCaptureButton
            // 
            this.AutoCaptureButton.Location = new System.Drawing.Point(132, 3);
            this.AutoCaptureButton.Name = "AutoCaptureButton";
            this.AutoCaptureButton.Size = new System.Drawing.Size(112, 23);
            this.AutoCaptureButton.TabIndex = 2;
            this.AutoCaptureButton.Text = "Auto capture";
            this.AutoCaptureButton.UseVisualStyleBackColor = true;
            this.AutoCaptureButton.Click += new System.EventHandler(this.AutoCaptureButton_Click);
            // 
            // CaptureButton
            // 
            this.CaptureButton.Location = new System.Drawing.Point(250, 3);
            this.CaptureButton.Name = "CaptureButton";
            this.CaptureButton.Size = new System.Drawing.Size(112, 23);
            this.CaptureButton.TabIndex = 3;
            this.CaptureButton.Text = "Capture";
            this.CaptureButton.UseVisualStyleBackColor = true;
            this.CaptureButton.Click += new System.EventHandler(this.CaptureButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(14, 3);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(112, 23);
            this.OpenButton.TabIndex = 4;
            this.OpenButton.Text = "Open picture";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // ImageBox
            // 
            this.ImageBox.Location = new System.Drawing.Point(14, 64);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(10, 10);
            this.ImageBox.TabIndex = 5;
            this.ImageBox.TabStop = false;
            this.ImageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseDown);
            this.ImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
            this.ImageBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseUp);
            // 
            // SpriteType0
            // 
            this.SpriteType0.AutoSize = true;
            this.SpriteType0.Checked = true;
            this.SpriteType0.Location = new System.Drawing.Point(14, 32);
            this.SpriteType0.Name = "SpriteType0";
            this.SpriteType0.Size = new System.Drawing.Size(137, 17);
            this.SpriteType0.TabIndex = 6;
            this.SpriteType0.TabStop = true;
            this.SpriteType0.Text = "Monochrome with mask";
            this.SpriteType0.UseVisualStyleBackColor = true;
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 86);
            this.Controls.Add(this.SpriteType0);
            this.Controls.Add(this.ImageBox);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.CaptureButton);
            this.Controls.Add(this.AutoCaptureButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CaptureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Capture sprite";
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Button AutoCaptureButton;
        private System.Windows.Forms.Button CaptureButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.RadioButton SpriteType0;
    }
}