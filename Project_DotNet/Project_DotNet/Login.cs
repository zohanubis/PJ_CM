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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void guna2Btn_Signin_Click(object sender, EventArgs e)
        {
            //if(txtUsername.Text == string.Empty)
            //{
            //    MessageBox.Show("Vui lòng nhập Username");
            //}
            //if(txtPassword.Text == string.Empty)
            //{
            //    MessageBox.Show("Vui lòng nhập Passoword");

            //}
            if (txtUsername.Text =="AD" && txtPassword.Text == "123")
            {
                Loading_Admin _load = new Loading_Admin();
                _load.Show();
                this.Hide();

            }
            else if(txtUsername.Text =="NV" && txtPassword.Text == "345")
            {
                Loading_User _load = new Loading_User();
                _load.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Người dùng không tồn tại");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);

        }
    }
}
