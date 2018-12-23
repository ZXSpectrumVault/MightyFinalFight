using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpriteCutter
{
    public partial class SetupForm : Form
    {
        public SetupForm()
        {
            InitializeComponent();
            PathBox.Text = MainForm.WorkPath.Remove(MainForm.WorkPath.Length - 1);
            BinBox.Text = MainForm.BinPath.Remove(MainForm.BinPath.Length - 1);
            SourceBox.Text = MainForm.SourcePath.Remove(MainForm.SourcePath.Length - 1);
            EnableColorButton.BackColor = MainForm.ColorEnable;
            DisableColorButton.BackColor = MainForm.ColorDisable;
            MaskColorButton.BackColor = MainForm.ColorMask;
            SelectionColorButton.BackColor = MainForm.ColorSelection;
            ColliderColorButton.BackColor = MainForm.ColorCollider;
        }

        private void PathButton_Click(object sender, EventArgs e)
        {
            FolderDialogPath.SelectedPath = PathBox.Text;
            if (FolderDialogPath.ShowDialog() == DialogResult.OK)
            {
                PathBox.Text = FolderDialogPath.SelectedPath;
            }
        }

        private void EnableColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = EnableColorButton.BackColor;
            ColorDialog.ShowDialog();
            EnableColorButton.BackColor = ColorDialog.Color;
        }

        private void DisableColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = DisableColorButton.BackColor;
            ColorDialog.ShowDialog();
            DisableColorButton.BackColor = ColorDialog.Color;
        }

        private void MaskColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = MaskColorButton.BackColor;
            ColorDialog.ShowDialog();
            MaskColorButton.BackColor = ColorDialog.Color;
        }

        private void SelectionColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = SelectionColorButton.BackColor;
            ColorDialog.ShowDialog();
            SelectionColorButton.BackColor = ColorDialog.Color;
        }

        private void ColliderColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ColliderColorButton.BackColor;
            ColorDialog.ShowDialog();
            ColliderColorButton.BackColor = ColorDialog.Color;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            MainForm.WorkPath = PathBox.Text + "/";
            MainForm.BinPath = BinBox.Text + "/";
            MainForm.SourcePath = SourceBox.Text + "/";
            MainForm.ColorEnable = EnableColorButton.BackColor;
            MainForm.ColorDisable = DisableColorButton.BackColor;
            MainForm.ColorMask = MaskColorButton.BackColor;
            MainForm.ColorSelection = SelectionColorButton.BackColor;
            MainForm.ColorCollider = ColliderColorButton.BackColor;

            //сохраняем настройки в файл
            MainForm.Settings iniSet = new MainForm.Settings();
            iniSet.WorkPath = PathBox.Text + "/";
            iniSet.BinPath = BinBox.Text + "/";
            iniSet.SourcePath = SourceBox.Text + "/";
            iniSet.ColorEnable = EnableColorButton.BackColor.ToArgb();
            iniSet.ColorDisable = DisableColorButton.BackColor.ToArgb();
            iniSet.ColorMask = MaskColorButton.BackColor.ToArgb();
            iniSet.ColorSelection = SelectionColorButton.BackColor.ToArgb();
            iniSet.ColorCollider = ColliderColorButton.BackColor.ToArgb();
            using (Stream writer = new FileStream("setup.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MainForm.Settings));
                serializer.Serialize(writer, iniSet);
            }
            this.Close();
        }

        private void BinButton_Click(object sender, EventArgs e)
        {
            FolderDialogBin.SelectedPath = BinBox.Text;
            if (FolderDialogBin.ShowDialog() == DialogResult.OK)
            {
                BinBox.Text = FolderDialogBin.SelectedPath;
            }
        }

        private void SourceButton_Click(object sender, EventArgs e)
        {
            FolderDialogSource.SelectedPath = SourceBox.Text;
            if (FolderDialogSource.ShowDialog() == DialogResult.OK)
            {
                SourceBox.Text = FolderDialogSource.SelectedPath;
            }
        }
    }
}
