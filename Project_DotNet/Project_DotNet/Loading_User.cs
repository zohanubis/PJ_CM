using System;
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
    public partial class Loading_User : Form
    {
        public Loading_User()
        {
            InitializeComponent();
        }

        private void label_Val_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (guna2CircleProgressBar1.Value == 99)
            {
                timer1.Stop();
                DashBoard_User p = new DashBoard_User();
                p.Show();
                this.Hide();
            }
            guna2CircleProgressBar1.Value += 1;
            label_Val.Text = (Convert.ToInt32(label_Val.Text) + 1).ToString();
        }

        private void Loading_User_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
            timer1.Start();
        }
    }
}
