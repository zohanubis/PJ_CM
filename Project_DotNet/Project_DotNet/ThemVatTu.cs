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
    public partial class ThemVatTu : Form
    {
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr);
        SqlDataAdapter da_sanpham;
        DataSet ds_sanpham = new DataSet();
        DataColumn[] key = new DataColumn[1];
        public ThemVatTu()
        {
            InitializeComponent();
        }

        private void ThemVatTu_Load(object sender, EventArgs e)
        {
            load_grid();
            load_cbo_NhaCungCap();
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

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
        void load_cbo_NhaCungCap()
        {
            DataTable dtComboBox = new DataTable("LoaiNhaCungCap");
            dtComboBox.Columns.Add("MANCC");
            dtComboBox.Columns.Add("TENNCC");

            // Thêm dòng "Tất cả nhà cung cấp"
            DataRow allNCCRow = dtComboBox.NewRow();
            allNCCRow["MANCC"] = "Loai000"; // Chọn một MANCC phù hợp
            allNCCRow["TENNCC"] = "Tất cả nhà cung cấp";
            dtComboBox.Rows.Add(allNCCRow);

            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr))
            {
                cn.Open();

                // Tạo câu truy vấn để lấy dữ liệu từ bảng Nhà cung cấp
                string query = "SELECT MANCC, TENNCC FROM NHACUNGCAP";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtComboBox); // Đổ dữ liệu vào DataTable
                }
            }

            cbo_NhaCungCap.DataSource = dtComboBox;
            cbo_NhaCungCap.DisplayMember = "TENNCC";
            cbo_NhaCungCap.ValueMember = "MANCC";
        }

        private void cbo_NhaCungCap_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedNCC = cbo_NhaCungCap.SelectedValue.ToString();

            // Lọc lại dữ liệu theo nhà cung cấp khi có sự thay đổi trong cbo_NhaCungCap
            DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
            if (selectedNCC != "Loai000")
            {
                dv.RowFilter = $"MANCC = '{selectedNCC}'";
            }
            if (selectedNCC == "Loai000")
            {
                load_grid();
            }
            dataGridView.DataSource = dv.ToTable();
        }

        private void btnMuaVatTu_Click(object sender, EventArgs e)
        {
            DataRow newrow = ds_sanpham.Tables["SanPham"].NewRow();
            newrow["MAVL"] = txtMaVatTu.Text;
            newrow["TENVL"] = txtTenVatTu.Text;
            newrow["GIANHAP"] = float.Parse(txtGiaNhap.Text);
            newrow["GIABAN"] = float.Parse(txtGiaBan.Text);
            newrow["SOLUONG"] = int.Parse(txtSoLuong.Text);
            newrow["MALOAI"] = txtMaLoai.Text;
            newrow["MANCC"] = cbo_NhaCungCap.SelectedValue.ToString();
            try
            {
                ds_sanpham.Tables["SanPham"].Rows.Add(newrow);
            }
            catch
            {
                MessageBox.Show("Mã sản phẩm không được trùng!");
            }
            SqlCommandBuilder cb = new SqlCommandBuilder(da_sanpham);
            da_sanpham.Update(ds_sanpham, "SanPham");
            load_grid();

            MessageBox.Show("Thêm Thành Công");

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow dr = ds_sanpham.Tables["SanPham"].Rows.Find(txtMaVatTu.Text);
            if (dr != null)
                dr.Delete();
            SqlCommandBuilder cb = new SqlCommandBuilder(da_sanpham);
            da_sanpham.Update(ds_sanpham, "SanPham");
            MessageBox.Show("Xóa Thành Công");
            load_grid();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            load_grid();
        }

        private void dataGridView_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
