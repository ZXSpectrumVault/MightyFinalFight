using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


namespace MLZShell
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string f in OpenDialog.FileNames)
                {
                    Process.Start(@"..\..\..\..\MegaLZ.exe", '"' + f + '"');
                }

            }
        }
    }
}
