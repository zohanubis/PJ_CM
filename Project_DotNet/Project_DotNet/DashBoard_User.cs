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
    public partial class DashBoard_User : Form
    {
        public DashBoard_User()
        {
            InitializeComponent();
        }
        private Form currentFormChild;
        private void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null) { currentFormChild.Close(); }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel_container.Controls.Add(childForm);
            panel_container.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void DashBoard_User_Load(object sender, EventArgs e)
        {

        }

        private void btnThongTinVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ThongTinVatTu());
            label_val.Text = btnThongTinVatTu.Text;
        }

        private void btnThemVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ThemVatTu());
            label_val.Text = btnThemVatTu.Text;
        }

        private void btnCapNhatVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CapNhatVatTu());
            label_val.Text = btnCapNhatVatTu.Text;
        }

        private void btnKiemTraVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new KiemTraVatTu());
            label_val.Text = btnKiemTraVatTu.Text;
        }

        private void btnMuaVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new MuaVatTu());
            label_val.Text = btnMuaVatTu.Text;
        }

        private void btnTimKiemVatTu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TimKiemVatTu());
            label_val.Text = btnTimKiemVatTu.Text;
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
            Login lg = new Login();
            lg.Show();
        }
    }
}
