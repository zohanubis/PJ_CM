using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Project_DotNet
{
    public partial class CapNhatVatTu : Form
    {
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr);
        SqlDataAdapter da_sanpham;
        DataSet ds_sanpham = new DataSet();
        DataColumn[] key = new DataColumn[1];
        public CapNhatVatTu()
        {
            InitializeComponent();
        }
        void load_grid()
        {
            //ds_sanpham.Tables["SanPham"].Clear(); // Xóa dữ liệu cũ
            string strSelect = "Select * from VATLIEU";
            da_sanpham = new SqlDataAdapter(strSelect, cn);
            da_sanpham.Fill(ds_sanpham, "SanPham");
            key[0] = ds_sanpham.Tables["SanPham"].Columns[0];
            ds_sanpham.Tables["SanPham"].PrimaryKey = key;
            dataGridView.DataSource = ds_sanpham.Tables["SanPham"];
        }

        private void CapNhatVatTu_Load(object sender, EventArgs e)
        {
            load_grid();
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            try
            {
                row = dataGridView.Rows[e.RowIndex];

                txtMaVatTu.Text = Convert.ToString(row.Cells["MAVL"].Value);
                txtTenVatTu.Text = Convert.ToString(row.Cells["TENVL"].Value);
                txtSoLuong.Text = Convert.ToString(row.Cells["SOLUONG"].Value);
                txtGiaNhap.Text = Convert.ToString(row.Cells["GIANHAP"].Value);
                txtGiaBan.Text = Convert.ToString(row.Cells["GIABAN"].Value);
                txtMaLoai.Text = Convert.ToString(row.Cells["MALOAI"].Value);
            }
            catch
            {
                txtMaVatTu.Clear();
                txtTenVatTu.Clear();
                txtGiaBan.Clear();
                txtGiaBan.Clear();
                txtSoLuong.Clear();
            }
        }

        private void btnCapNhatVatTu_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = ds_sanpham.Tables["SanPham"].Rows.Find(txtMaVatTu.Text);
                if (dr != null)
                {
                    // Cập nhật giá trị mới từ TextBox vào dòng DataSet
                    dr["TENVL"] = txtTenVatTu.Text;
                    dr["GIANHAP"] = float.Parse(txtGiaNhap.Text);
                    dr["GIABAN"] = float.Parse(txtGiaBan.Text);
                    dr["SOLUONG"] = int.Parse(txtSoLuong.Text);
                    dr["MALOAI"] = txtMaLoai.Text;

                    // Cập nhật cơ sở dữ liệu
                    SqlCommandBuilder cb = new SqlCommandBuilder(da_sanpham);
                    da_sanpham.Update(ds_sanpham, "SanPham");

                    MessageBox.Show("Cập nhật thành công");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vật liệu cần cập nhật");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                load_grid(); 
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            load_grid();

        }
    }
}
