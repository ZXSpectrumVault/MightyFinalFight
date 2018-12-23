namespace WorldEditor
{
    partial class PropertyForm
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
            this.OkButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.type_group = new System.Windows.Forms.GroupBox();
            this.tp_deep = new System.Windows.Forms.RadioButton();
            this.tp_platform = new System.Windows.Forms.RadioButton();
            this.tp_type_empty = new System.Windows.Forms.RadioButton();
            this.anim_group = new System.Windows.Forms.GroupBox();
            this.anim_text = new System.Windows.Forms.TextBox();
            this.anim_random = new System.Windows.Forms.CheckBox();
            this.anim_enable = new System.Windows.Forms.CheckBox();
            this.anim_speed = new System.Windows.Forms.TrackBar();
            this.type_group.SuspendLayout();
            this.anim_group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anim_speed)).BeginInit();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(12, 228);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(71, 23);
            this.OkButton.TabIndex = 21;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(100, 228);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(71, 23);
            this.ExitButton.TabIndex = 22;
            this.ExitButton.Text = "Cancel";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // type_group
            // 
            this.type_group.Controls.Add(this.tp_deep);
            this.type_group.Controls.Add(this.tp_platform);
            this.type_group.Controls.Add(this.tp_type_empty);
            this.type_group.Location = new System.Drawing.Point(12, 12);
            this.type_group.Name = "type_group";
            this.type_group.Size = new System.Drawing.Size(157, 70);
            this.type_group.TabIndex = 26;
            this.type_group.TabStop = false;
            this.type_group.Text = "Type";
            // 
            // tp_deep
            // 
            this.tp_deep.AutoSize = true;
            this.tp_deep.Location = new System.Drawing.Point(9, 44);
            this.tp_deep.Name = "tp_deep";
            this.tp_deep.Size = new System.Drawing.Size(49, 17);
            this.tp_deep.TabIndex = 3;
            this.tp_deep.Text = "deep";
            this.tp_deep.UseVisualStyleBackColor = true;
            // 
            // tp_platform
            // 
            this.tp_platform.AutoSize = true;
            this.tp_platform.Location = new System.Drawing.Point(88, 21);
            this.tp_platform.Name = "tp_platform";
            this.tp_platform.Size = new System.Drawing.Size(62, 17);
            this.tp_platform.TabIndex = 1;
            this.tp_platform.Text = "platform";
            this.tp_platform.UseVisualStyleBackColor = true;
            // 
            // tp_type_empty
            // 
            this.tp_type_empty.AutoSize = true;
            this.tp_type_empty.Checked = true;
            this.tp_type_empty.Location = new System.Drawing.Point(9, 21);
            this.tp_type_empty.Name = "tp_type_empty";
            this.tp_type_empty.Size = new System.Drawing.Size(53, 17);
            this.tp_type_empty.TabIndex = 0;
            this.tp_type_empty.TabStop = true;
            this.tp_type_empty.Text = "empty";
            this.tp_type_empty.UseVisualStyleBackColor = true;
            // 
            // anim_group
            // 
            this.anim_group.Controls.Add(this.anim_text);
            this.anim_group.Controls.Add(this.anim_random);
            this.anim_group.Controls.Add(this.anim_enable);
            this.anim_group.Controls.Add(this.anim_speed);
            this.anim_group.Location = new System.Drawing.Point(12, 88);
            this.anim_group.Name = "anim_group";
            this.anim_group.Size = new System.Drawing.Size(157, 120);
            this.anim_group.TabIndex = 28;
            this.anim_group.TabStop = false;
            this.anim_group.Text = "Animation";
            // 
            // anim_text
            // 
            this.anim_text.Location = new System.Drawing.Point(35, 87);
            this.anim_text.Name = "anim_text";
            this.anim_text.Size = new System.Drawing.Size(86, 20);
            this.anim_text.TabIndex = 8;
            this.anim_text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.anim_text.TextChanged += new System.EventHandler(this.anim_text_TextChanged);
            // 
            // anim_random
            // 
            this.anim_random.AutoSize = true;
            this.anim_random.Location = new System.Drawing.Point(87, 23);
            this.anim_random.Name = "anim_random";
            this.anim_random.Size = new System.Drawing.Size(61, 17);
            this.anim_random.TabIndex = 7;
            this.anim_random.Text = "random";
            this.anim_random.UseVisualStyleBackColor = true;
            // 
            // anim_enable
            // 
            this.anim_enable.AutoSize = true;
            this.anim_enable.Location = new System.Drawing.Point(9, 23);
            this.anim_enable.Name = "anim_enable";
            this.anim_enable.Size = new System.Drawing.Size(64, 17);
            this.anim_enable.TabIndex = 6;
            this.anim_enable.Text = "enabled";
            this.anim_enable.UseVisualStyleBackColor = true;
            this.anim_enable.CheckedChanged += new System.EventHandler(this.anim_enable_CheckedChanged);
            // 
            // anim_speed
            // 
            this.anim_speed.Location = new System.Drawing.Point(9, 46);
            this.anim_speed.Maximum = 15;
            this.anim_speed.Minimum = 1;
            this.anim_speed.Name = "anim_speed";
            this.anim_speed.Size = new System.Drawing.Size(139, 45);
            this.anim_speed.TabIndex = 5;
            this.anim_speed.Value = 1;
            this.anim_speed.Scroll += new System.EventHandler(this.anim_speed_Scroll);
            // 
            // PropertyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 266);
            this.Controls.Add(this.anim_group);
            this.Controls.Add(this.type_group);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.OkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "PropertyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Property";
            this.type_group.ResumeLayout(false);
            this.type_group.PerformLayout();
            this.anim_group.ResumeLayout(false);
            this.anim_group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anim_speed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.GroupBox type_group;
        private System.Windows.Forms.RadioButton tp_deep;
        private System.Windows.Forms.RadioButton tp_platform;
        private System.Windows.Forms.RadioButton tp_type_empty;
        private System.Windows.Forms.GroupBox anim_group;
        private System.Windows.Forms.TextBox anim_text;
        private System.Windows.Forms.CheckBox anim_random;
        private System.Windows.Forms.CheckBox anim_enable;
        private System.Windows.Forms.TrackBar anim_speed;
    }
}