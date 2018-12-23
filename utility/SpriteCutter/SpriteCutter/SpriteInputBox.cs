using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpriteCutter
{
    public partial class SpriteInputBox : Form
    {
        public string ResultString = "";
        public byte ResultType = 0;
        public SpriteInputBox(string defaulttext)
        {
            InitializeComponent();
            InputString.Text = defaulttext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            ResultString = InputString.Text;
            foreach (Control c in this.Controls)
            {
                if (c.Name.Contains("SpriteType"))
                {
                    RadioButton r = (RadioButton)c;
                    if (r.Checked)
                    {
                        string num = new string(new char[]{r.Name[r.Name.Length - 1]});
                        ResultType = byte.Parse(num);
                        break;
                    }
                }
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
