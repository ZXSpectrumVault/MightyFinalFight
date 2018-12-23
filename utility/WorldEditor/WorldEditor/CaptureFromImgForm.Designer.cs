namespace WorldEditor
{
    partial class CaptureFromImgForm
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
            this.ImgName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ScreenPicture = new System.Windows.Forms.PictureBox();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // ImgName
            // 
            this.ImgName.Location = new System.Drawing.Point(9, 9);
            this.ImgName.Name = "ImgName";
            this.ImgName.ReadOnly = true;
            this.ImgName.Size = new System.Drawing.Size(231, 20);
            this.ImgName.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(240, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 19);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ScreenPicture
            // 
            this.ScreenPicture.Location = new System.Drawing.Point(9, 34);
            this.ScreenPicture.Name = "ScreenPicture";
            this.ScreenPicture.Size = new System.Drawing.Size(512, 384);
            this.ScreenPicture.TabIndex = 2;
            this.ScreenPicture.TabStop = false;
            this.ScreenPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenPicture_MouseDown);
            this.ScreenPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScreenPicture_MouseMove);
            // 
            // FileDialog
            // 
            this.FileDialog.Filter = "SCR|*.scr|All files|*.*";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(178, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(276, 424);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // CaptureFromImgForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 455);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ScreenPicture);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ImgName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CaptureFromImgForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Capture tiles from image";
            this.Activated += new System.EventHandler(this.CaptureFromImgForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.ScreenPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ImgName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox ScreenPicture;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}