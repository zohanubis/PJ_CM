﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_DotNet
{
    public partial class Loading_Admin : Form
    {
        public Loading_Admin()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(guna2CircleProgressBar1.Value == 99)
            {
                timer1.Stop();
                DashBoard_Admin p = new DashBoard_Admin();
                p.Show();
                this.Hide();
            }
            guna2CircleProgressBar1.Value += 1;
            label_Val.Text = (Convert.ToInt32(label_Val.Text)+1).ToString();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            timer1.Start();
        }

        private void label_Val_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
