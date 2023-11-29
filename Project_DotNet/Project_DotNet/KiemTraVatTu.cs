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
    public partial class KiemTraVatTu : Form
    {
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr);
        SqlDataAdapter da_sanpham;
        DataSet ds_sanpham = new DataSet();
        DataColumn[] key = new DataColumn[1];
        public KiemTraVatTu()
        {
            InitializeComponent();

        }
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
            dataGridView.DataSource = ds_sanpham.Tables["SanPham"];
        }
        private void KiemTraVatTu_Load(object sender, EventArgs e)
        {
            load_cbo();
            load_grid();
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
    }
}
