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
    public partial class InputBox : Form
    {
        public string ResultString;
        public InputBox(string caption, string defaulttext)
        {
            InitializeComponent();
            this.Text = caption;
            InputString.Text = defaulttext;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            ResultString = InputString.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            ResultString = "";
            this.Close();
        }
    }
}
