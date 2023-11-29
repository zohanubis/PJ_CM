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
    public partial class TimKiemVatTu : Form
    {
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr);
        SqlDataAdapter da_sanpham;
        DataSet ds_sanpham = new DataSet();
        DataColumn[] key = new DataColumn[1];
        private DataView dv_sanpham;
        public TimKiemVatTu()
        {
            InitializeComponent();
        }
        //Phương thức truy vấn để xem dữ liệu

        void load_cbo()
        {
            DataTable dtComboBox = new DataTable("LoaiSanPham");
            dtComboBox.Columns.Add("MALOAI");
            dtComboBox.Columns.Add("TENLOAI");

            // Thêm dòng "Tất cả vật liệu"
            DataRow allRow = dtComboBox.NewRow();
            allRow["MALOAI"] = "Loai000";
            allRow["TENLOAI"] = "Tất cả vật liệu";
            dtComboBox.Rows.Add(allRow);

            // Thêm dòng "Vật liệu còn hàng"
            DataRow availableRow = dtComboBox.NewRow();
            availableRow["MALOAI"] = "Loai001";
            availableRow["TENLOAI"] = "Vật liệu còn hàng";
            dtComboBox.Rows.Add(availableRow);

            // Thêm dòng "Vật liệu hết hàng"
            DataRow outOfStockRow = dtComboBox.NewRow();
            outOfStockRow["MALOAI"] = "Loai002";
            outOfStockRow["TENLOAI"] = "Vật liệu hết hàng";
            dtComboBox.Rows.Add(outOfStockRow);

            cbo_LocDuLieu.DataSource = dtComboBox;
            cbo_LocDuLieu.DisplayMember = "TENLOAI";
            cbo_LocDuLieu.ValueMember = "MALOAI";

        }
        void load_grid()
        {
            string strSelect = "Select * from VATLIEU";
            da_sanpham = new SqlDataAdapter(strSelect, cn);
            da_sanpham.Fill(ds_sanpham, "SanPham");
            key[0] = ds_sanpham.Tables["SanPham"].Columns[0];
            ds_sanpham.Tables["SanPham"].PrimaryKey = key;

            // Tạo DataView và lưu trữ nó trong biến toàn cục
            dv_sanpham = new DataView(ds_sanpham.Tables["SanPham"]);

            dataGridView.DataSource = ds_sanpham.Tables["SanPham"];
        }
        private void TimKiemVatTu_Load(object sender, EventArgs e)
        {
            load_cbo();
            load_grid();
            txtMaVatTu.TextChanged += txtMaVatTu_TextChanged;
            txtTenVatTu.TextChanged += txtTenVatTu_TextChanged;
        }

        private void cbo_LocDuLieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = cbo_LocDuLieu.SelectedValue.ToString();

            // Tất cả vật liệu
            if (selectedType == "Loai000")
            {
                load_grid();
            }
            // Vật liệu còn hàng
            else if (selectedType == "Loai001")
            {
                DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
                dv.RowFilter = "SOLUONG > 0";
                dataGridView.DataSource = dv.ToTable();
            }
            // Vật liệu hết hàng
            else if (selectedType == "Loai002")
            {
                DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
                dv.RowFilter = "SOLUONG <= 0";
                dataGridView.DataSource = dv.ToTable();
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                string supplierID = Convert.ToString(row.Cells["MANCC"].Value);

                // Gọi stored procedure để lấy tên Nhà cung cấp từ mã
                string supplierName = GetSupplierNameFromID(supplierID);

                txtMaVatTu.Text = Convert.ToString(row.Cells["MAVL"].Value);
                txtTenVatTu.Text = Convert.ToString(row.Cells["TENVL"].Value);
                txtSoLuong.Text = Convert.ToString(row.Cells["SOLUONG"].Value);
                txtNhaCungCap.Text = supplierName;
            }
            else
            {
                txtMaVatTu.Clear();
                txtTenVatTu.Clear();
                txtSoLuong.Clear();
            }
        }
        private string GetSupplierNameFromID(string supplierID)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("GetSupplierName", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);

                    object result = cmd.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }


        private void txtMaVatTu_TextChanged(object sender, EventArgs e)
        {
            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr))
            {
                cn.Open();

                // Tạo câu truy vấn sử dụng tham số
                string query = "SELECT * FROM VATLIEU WHERE MAVL LIKE @MaVatTu";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Thêm tham số vào câu truy vấn
                    cmd.Parameters.AddWithValue("@MaVatTu", "%" + txtMaVatTu.Text.Trim() + "%");

                    // Sử dụng SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView.DataSource = dt;
                }
            }
        }
        private void txtTenVatTu_TextChanged(object sender, EventArgs e)
       {
            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr))
            {
                cn.Open();

                // Tạo câu truy vấn sử dụng tham số
                string query = "SELECT * FROM VATLIEU WHERE TENVL LIKE @TenVatTu";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Thêm tham số vào câu truy vấn
                    cmd.Parameters.AddWithValue("@TenVatTu", "%" + txtTenVatTu.Text.Trim() + "%");

                    // Sử dụng SqlDataAdapter để lấy dữ liệu từ cơ sở dữ liệu
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Hiển thị dữ liệu trên DataGridView
                    dataGridView.DataSource = dt;
                }
            }
        }
    }
}
