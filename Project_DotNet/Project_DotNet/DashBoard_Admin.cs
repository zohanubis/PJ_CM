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
    public partial class DashBoard_Admin : Form
    {
        public DashBoard_Admin()
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
        private void btnSignOut_Click(object sender, EventArgs e)
        {
            this.Close();
            Login lg = new Login();
            lg.Show();
        }

        private void DashBoard_Admin_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox_val_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null) { currentFormChild.Close(); }
            label_val.Text = btnDashBoard.Text;
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            if (currentFormChild != null) { currentFormChild.Close(); }
            label_val.Text = btnDashBoard.Text;

        }

        private void btnDSNhanVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new NhanVien());
            label_val.Text = btnDSNhanVien.Text;
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
            label_val.Text = btnKiemTraVatTu.Text;
        }
    }
}
