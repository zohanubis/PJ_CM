using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_DotNet
{
    public partial class NhanVien : Form
    {
        DBConnect db = new DBConnect();
        DataTable dt_users;

        public NhanVien()
        {
            InitializeComponent();
            string sql = "SELECT * FROM USERS";
            dt_users = db.getDataTable(sql);
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                if (db.Kt_TrungKhoa(txtUserName.Text) == true)
                {
                    DataRow row = dt_users.NewRow();
                    row["USERROLE"] = txtRole.Text;
                    row["_NAME"] = txtTen.Text;
                    row["DOB"] = txtDob.Text;
                    row["MOBILE"] = Int64.Parse(txtPhone.Text);
                    row["EMAIL"] = txtEmail.Text;
                    row["USERNAME"] = txtUserName.Text;
                    row["MAUSERS"] = txtUserID.Text;
                    row["PASS"] = txtPassWord.Text;
                    dt_users.Rows.Add(row);
                    int kq = db.updateDatabase(dt_users);
                    if (kq > 0)
                        MessageBox.Show("Đăng Ký Thành Công");
                    else
                        MessageBox.Show("Đăng Ký Không Thành Công");
                }
                else
                {
                    MessageBox.Show("UserName Phải khác !!!!");
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)  // 2627 là mã lỗi cho việc vi phạm ràng buộc duy nhất (unique constraint)
                {
                    MessageBox.Show("Địa chỉ email đã tồn tại. Vui lòng nhập lại.");
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thực hiện INSERT. Chi tiết lỗi: " + ex.Message);
                }
            }
        }
        public void Clear()
        {
            txtUserName.Clear();
            txtPassWord.Clear();
            txtTen.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtUserID.Clear();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
