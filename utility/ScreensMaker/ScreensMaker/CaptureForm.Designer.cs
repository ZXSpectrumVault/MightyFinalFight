namespace ScreensMaker
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
            this.ScreenFileNameButton = new System.Windows.Forms.Button();
            this.ScreenFileNameEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ScreenFileImg = new System.Windows.Forms.PictureBox();
            this.ScreenFileCancelButton = new System.Windows.Forms.Button();
            this.ScreenFileCaptureButton = new System.Windows.Forms.Button();
            this.OpenScreenFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFileImg)).BeginInit();
            this.SuspendLayout();
            // 
            // ScreenFileNameButton
            // 
            this.ScreenFileNameButton.Location = new System.Drawing.Point(480, 21);
            this.ScreenFileNameButton.Name = "ScreenFileNameButton";
            this.ScreenFileNameButton.Size = new System.Drawing.Size(44, 26);
            this.ScreenFileNameButton.TabIndex = 14;
            this.ScreenFileNameButton.Text = "...";
            this.ScreenFileNameButton.UseVisualStyleBackColor = true;
            this.ScreenFileNameButton.Click += new System.EventHandler(this.ScreenFileNameButton_Click);
            // 
            // ScreenFileNameEdit
            // 
            this.ScreenFileNameEdit.Location = new System.Drawing.Point(12, 25);
            this.ScreenFileNameEdit.Name = "ScreenFileNameEdit";
            this.ScreenFileNameEdit.Size = new System.Drawing.Size(462, 20);
            this.ScreenFileNameEdit.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Screen file name";
            // 
            // ScreenFileImg
            // 
            this.ScreenFileImg.Location = new System.Drawing.Point(12, 53);
            this.ScreenFileImg.Name = "ScreenFileImg";
            this.ScreenFileImg.Size = new System.Drawing.Size(512, 384);
            this.ScreenFileImg.TabIndex = 15;
            this.ScreenFileImg.TabStop = false;
            this.ScreenFileImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenFileImg_MouseDown);
            this.ScreenFileImg.MouseLeave += new System.EventHandler(this.ScreenFileImg_MouseLeave);
            this.ScreenFileImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScreenFileImg_MouseMove);
            this.ScreenFileImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreenFileImg_MouseUp);
            // 
            // ScreenFileCancelButton
            // 
            this.ScreenFileCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ScreenFileCancelButton.Location = new System.Drawing.Point(85, 443);
            this.ScreenFileCancelButton.Name = "ScreenFileCancelButton";
            this.ScreenFileCancelButton.Size = new System.Drawing.Size(67, 23);
            this.ScreenFileCancelButton.TabIndex = 17;
            this.ScreenFileCancelButton.Text = "Cancel";
            this.ScreenFileCancelButton.UseVisualStyleBackColor = true;
            this.ScreenFileCancelButton.Click += new System.EventHandler(this.ScreenFileCancelButton_Click);
            // 
            // ScreenFileCaptureButton
            // 
            this.ScreenFileCaptureButton.Location = new System.Drawing.Point(12, 443);
            this.ScreenFileCaptureButton.Name = "ScreenFileCaptureButton";
            this.ScreenFileCaptureButton.Size = new System.Drawing.Size(67, 23);
            this.ScreenFileCaptureButton.TabIndex = 16;
            this.ScreenFileCaptureButton.Text = "Capture";
            this.ScreenFileCaptureButton.UseVisualStyleBackColor = true;
            this.ScreenFileCaptureButton.Click += new System.EventHandler(this.ScreenFileCaptureButton_Click);
            // 
            // OpenScreenFileDialog
            // 
            this.OpenScreenFileDialog.FileName = "openFileDialog1";
            // 
            // CaptureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ScreenFileCancelButton;
            this.ClientSize = new System.Drawing.Size(536, 475);
            this.Controls.Add(this.ScreenFileCancelButton);
            this.Controls.Add(this.ScreenFileCaptureButton);
            this.Controls.Add(this.ScreenFileImg);
            this.Controls.Add(this.ScreenFileNameButton);
            this.Controls.Add(this.ScreenFileNameEdit);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CaptureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CaptureForm";
            ((System.ComponentModel.ISupportInitialize)(this.ScreenFileImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ScreenFileNameButton;
        private System.Windows.Forms.TextBox ScreenFileNameEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox ScreenFileImg;
        private System.Windows.Forms.Button ScreenFileCancelButton;
        private System.Windows.Forms.Button ScreenFileCaptureButton;
        private System.Windows.Forms.OpenFileDialog OpenScreenFileDialog;
    }
}