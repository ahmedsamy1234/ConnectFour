﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace The_Box_v0._1.Forms
{
    public partial class PlayForm : Form
    {
        public PlayForm()
        {
            InitializeComponent();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Dialog dlg = new Dialog();
            DialogResult dResult;
            dResult = dlg.ShowDialog();
            if (dResult == DialogResult.OK)
            {
                listBox1.Items.Add(dlg.RoomName);
            }
        }
    }
}
