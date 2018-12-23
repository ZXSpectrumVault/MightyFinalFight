using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXProc;

namespace WorldEditor
{
    public partial class PropertyForm : Form
    {
        public byte TileProperty; //номер редактируемого тайла

        void LoadProperty()
        {


        }

        public void InitPropertyForm(byte p)
        {
            TileProperty = p;
            switch ((p >> 6) & 3)
            {
                case 0:
                    tp_type_empty.Checked = true;
                    break;
                case 1:
                    tp_deep.Checked = true;
                    break;
                case 2:
                    tp_platform.Checked = true;
                    break;
            }
            if ((p & 15) == 0)
            {
                anim_enable.Enabled = true;
                anim_enable.Checked = false;
                anim_random.Enabled = false;
                anim_speed.Enabled = false;
                anim_text.Enabled = false;
                anim_random.Checked = false;
                anim_speed.Value = 1;
                anim_text.Text = "";
            }
            else
            {
                anim_enable.Checked = true;
                anim_random.Enabled = true;
                anim_speed.Enabled = true;
                anim_text.Enabled = true;
                anim_random.Checked = (p & 0x10) == 0 ? false : true;
                anim_speed.Value = p & 15;
                anim_text.Text = (p & 15).ToString();
            }
        }

        private void anim_enable_CheckedChanged(object sender, EventArgs e)
        {
            byte b = 0;
            if (tp_deep.Checked) b = 64;
            if (tp_platform.Checked) b = 128;
            if (anim_random.Checked) b += 16;
            TileProperty = anim_enable.Checked ? (byte)1 : (byte)0;
            TileProperty = (byte)(TileProperty + b);
            InitPropertyForm(TileProperty);
        }

        private void anim_text_TextChanged(object sender, EventArgs e)
        {
            try
            {
                byte val;
                if (anim_text.Text != "")
                {
                    val = Byte.Parse(anim_text.Text);
                }
                else
                    val = 0;

                if (val > 15) val = 15;
                byte b = 0;
                if (tp_deep.Checked) b = 64;
                if (tp_platform.Checked) b = 128;
                if (anim_random.Checked) b += 16;
                TileProperty = (byte)(b + val);
                InitPropertyForm(TileProperty);
            }
            catch
            {
                anim_text.Text = anim_speed.Value.ToString();
            }
        }

        private void anim_speed_Scroll(object sender, EventArgs e)
        {
            byte b = 0;
            if (tp_deep.Checked) b = 64;
            if (tp_platform.Checked) b = 128;
            if (anim_random.Checked) b += 16;
            TileProperty = (byte)(b + anim_speed.Value);
            InitPropertyForm(TileProperty);
        }

        public PropertyForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            LocationEditorForm.TilesetChanged = true;
            TileProperty = 0;
            if (tp_deep.Checked) TileProperty = 64;      
            if (tp_platform.Checked) TileProperty = 128;
            if (anim_enable.Checked)
            {
                if (anim_random.Checked) TileProperty += 16;
                TileProperty += (byte)anim_speed.Value;
            }
            this.Close();
        }
    }
}
