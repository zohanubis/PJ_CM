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

        //void load_cbo()
        //{
        //    DataTable dtComboBox = new DataTable("LoaiSanPham");
        //    dtComboBox.Columns.Add("MALOAI");
        //    dtComboBox.Columns.Add("TENLOAI");

        //    // Thêm dòng "Tất cả vật liệu"
        //    DataRow allRow = dtComboBox.NewRow();
        //    allRow["MALOAI"] = "Loai000";
        //    allRow["TENLOAI"] = "Tất cả vật liệu";
        //    dtComboBox.Rows.Add(allRow);

        //    // Thêm dòng "Vật liệu còn hàng"
        //    DataRow availableRow = dtComboBox.NewRow();
        //    availableRow["MALOAI"] = "Loai001";
        //    availableRow["TENLOAI"] = "Vật liệu còn hàng";
        //    dtComboBox.Rows.Add(availableRow);

        //    // Thêm dòng "Vật liệu hết hàng"
        //    DataRow outOfStockRow = dtComboBox.NewRow();
        //    outOfStockRow["MALOAI"] = "Loai002";
        //    outOfStockRow["TENLOAI"] = "Vật liệu hết hàng";
        //    dtComboBox.Rows.Add(outOfStockRow);

        //    cbo_LocDuLieu.DataSource = dtComboBox;
        //    cbo_LocDuLieu.DisplayMember = "TENLOAI";
        //    cbo_LocDuLieu.ValueMember = "MALOAI";

        //}
        void load_grid()
        {
            string strSelect = "Select * from VATLIEU";
            da_sanpham = new SqlDataAdapter(strSelect, cn);
            da_sanpham.Fill(ds_sanpham, "SanPham");
            key[0] = ds_sanpham.Tables["SanPham"].Columns[0];
            ds_sanpham.Tables["SanPham"].PrimaryKey = key;

            dv_sanpham = new DataView(ds_sanpham.Tables["SanPham"]);

            dataGridView.DataSource = ds_sanpham.Tables["SanPham"];
        }
        private void TimKiemVatTu_Load(object sender, EventArgs e)
        {
            load_grid();
            load_cbo_NhaCungCap();
            txtMaVatTu.TextChanged += txtMaVatTu_TextChanged;
            txtTenVatTu.TextChanged += txtTenVatTu_TextChanged;
        }

        private void cbo_LocDuLieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string selectedType = cbo_LocDuLieu.SelectedValue.ToString();

            //// Tất cả vật liệu
            //if (selectedType == "Loai000")
            //{
            //    load_grid();
            //}
            //// Vật liệu còn hàng
            //else if (selectedType == "Loai001")
            //{
            //    DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
            //    dv.RowFilter = "SOLUONG > 0";
            //    dataGridView.DataSource = dv.ToTable();
            //}
            //// Vật liệu hết hàng
            //else if (selectedType == "Loai002")
            //{
            //    DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
            //    dv.RowFilter = "SOLUONG <= 0";
            //    dataGridView.DataSource = dv.ToTable();
            //}
            //// Lọc theo nhà cung cấp nếu có lựa chọn từ cbo_NhaCungCap
            //if (cbo_NhaCungCap.SelectedValue != null && selectedType != "Loai000")
            //{
            //    DataView dv = new DataView(ds_sanpham.Tables["SanPham"]);
            //    dv.RowFilter = $"MANCC = '{cbo_NhaCungCap.SelectedValue}'";
            //    dataGridView.DataSource = dv.ToTable();
            //}
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           // DataBingding(ds_sanpham.Tables["SanPham"]);
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
        //void DataBingding(DataTable pDT)
        //{
        //    txtMaVatTu.DataBindings.Clear();
        //    txtTenVatTu.DataBindings.Clear();
        //    txtSoLuong.DataBindings.Clear();

        //    txtMaVatTu.DataBindings.Add("Text", pDT, "MAVL");
        //    txtTenVatTu.DataBindings.Add("Text", pDT, "TENVL");
        //    txtSoLuong.DataBindings.Add("Text", pDT, "SOLUONG");
        //}
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                string supplierID = Convert.ToString(row.Cells["MANCC"].Value);

                // Gọi stored procedure để lấy tên Nhà cung cấp từ mã
                string supplierName = GetSupplierNameFromID(supplierID);

                txtMaVatTu.Text = Convert.ToString(row.Cells["MAVL"].Value);
                txtTenVatTu.Text = Convert.ToString(row.Cells["TENVL"].Value);
                //txtSoLuong.Text = Convert.ToString(row.Cells["SOLUONG"].Value);
                //txtNhaCungCap.Text = supplierName;
            }
            else
            {
                txtMaVatTu.Clear();
                txtTenVatTu.Clear();
                //txtSoLuong.Clear();
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
        private string GetMaNhaCungCap(string tenNhaCungCap)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr))
            {
                cn.Open();

                // Tạo câu truy vấn sử dụng tham số
                string query = "SELECT MANCC FROM NHACUNGCAP WHERE TENNCC = @TenNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // Thêm tham số vào câu truy vấn
                    cmd.Parameters.AddWithValue("@TenNhaCungCap", tenNhaCungCap);

                    // Lấy mã nhà cung cấp từ tên nhà cung cấp
                    object result = cmd.ExecuteScalar();

                    return result != null ? result.ToString() : string.Empty;
                }
            }
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
            if(selectedNCC == "Loai000")
            {
                load_grid();
            }
            dataGridView.DataSource = dv.ToTable();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
