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
    public partial class MuaVatTu : Form
    {
        DBConnect db = new DBConnect();
        DataTable dt;
        int DonGia = 0;
        int TongTien = 0;
        public MuaVatTu()
        {
            InitializeComponent();
        }
        public void LoadListBox()
        {
            List<string> lst = new List<string>();
            string sql = "SELECT * FROM dbo.GetAvailableVatLieu()";
            lst = db.loadListBoxSell(sql);

            // Xóa hết danh sách cũ
            listBox_VatTu.Items.Clear();

            // Thêm danh sách mới vào ListBox
            foreach (string s in lst)
            {
                listBox_VatTu.Items.Add(s);
            }
        }
        private void btnMuaVatTu_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSoLuongMua.Text, out int quantity) && quantity >= 0)
            {
                int tongTienMSP = DonGia * quantity;
                TongTien += tongTienMSP;

                int n = dataGridView_GioHang.Rows.Add();
                dataGridView_GioHang.Rows[n].Cells[0].Value = txtMaHD.Text;
                dataGridView_GioHang.Rows[n].Cells[1].Value = txtTenVatTu.Text;
                dataGridView_GioHang.Rows[n].Cells[2].Value = txtGiaBan.Text;
                dataGridView_GioHang.Rows[n].Cells[3].Value = quantity;
                dataGridView_GioHang.Rows[n].Cells[4].Value = tongTienMSP.ToString();
                dataGridView_GioHang.Rows[n].Cells[5].Value = txtMaKhachHang.Text;
                dataGridView_GioHang.Rows[n].Cells[6].Value = txtTenKhachHang.Text;

                txtThanhTien.Text = TongTien.ToString();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ.");
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            InHoaDon();
        }

        private void btnXoaKhoiGioHang_Click(object sender, EventArgs e)
        {
            if (dataGridView_GioHang.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView_GioHang.SelectedRows[0];
                int removedPrice = int.Parse(selectedRow.Cells[4].Value.ToString());
                TongTien -= removedPrice;
                txtThanhTien.Text = TongTien.ToString();
                dataGridView_GioHang.Rows.Remove(selectedRow);
            }
        }

        private void txtThanhTien_TextChanged(object sender, EventArgs e)
        {

        }

        private void MuaVatTu_Load(object sender, EventArgs e)
        {
            LoadListBox();
        }

        private void listView_VatTu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView_VatTu_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void listBox_VatTu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_VatTu.SelectedIndex != -1)
            {
                string selected = listBox_VatTu.SelectedItem.ToString();
                string sql = "SELECT MAVL, TENVL, GIABAN FROM VATLIEU WHERE TENVL = @TENVL";
                using (SqlCommand cmd = new SqlCommand(sql, db.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@TENVL", selected);
                    db.OpenConnection();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMaVatTu.Text = reader["MAVL"].ToString();
                        txtTenVatTu.Text = reader["TENVL"].ToString();
                        txtGiaBan.Text = reader["GIABAN"].ToString();
                        DonGia = (int)float.Parse(reader["GIABAN"].ToString());

                    }
                    else
                    {
                        MessageBox.Show("Vật liệu không tồn tại");
                        txtGiaBan.Clear();
                        txtMaVatTu.Clear();
                        txtTenVatTu.Clear();
                        txtGiaBan.Clear();
                    }
                    reader.Close();
                    db.CloseConnection();
                }
            }
        }

        private void txtSoLuongMua_TextChanged(object sender, EventArgs e)
        {
            txtThanhTien.Clear();
            if (int.TryParse(txtSoLuongMua.Text, out int quantity) && quantity >= 0)
            {
                int tongTienSP = DonGia * quantity;
                txtThanhTien.Text = tongTienSP.ToString();
            }
        }
        private void InHoaDon()
        {
            pddHoaDon.Document = pdHoaDon;
            pddHoaDon.ShowDialog();
        }
        private void pdHoaDon_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            string tenCuaHang = "HÓA ĐƠN VẬT LIỆU XÂY DỰNG";
            string diaChiCH = "140 Lê Trọng Tân, Tây Thạnh, TP.HCM";
            string phone = "0779.139.003";

            var w = pdHoaDon.DefaultPageSettings.PaperSize.Width;
            if (dataGridView_GioHang != null && dataGridView_GioHang.Rows.Count > 0)
            {

                // In thông tin từ DataGridView
                foreach (DataGridViewRow row in dataGridView_GioHang.Rows)
                {
                    // Check if the cells are not null
                    if (row.Cells.Count >= 7 && row.Cells[0].Value != null)
                    {
                        string maHD = row.Cells[0].Value.ToString();
                        string maKhachHang = row.Cells[5].Value.ToString();
                        string tenKhachHang = row.Cells[6].Value.ToString();
                        string thanhTien = txtThanhTien.Text;

                        // In thông tin từng hàng vào hóa đơn
                        // Cửa Hàng
                        #region header
                        e.Graphics.DrawString(tenCuaHang.ToUpper(),
                            new Font("Courier New", 12, FontStyle.Bold),
                            Brushes.Black, new Point(10, 20));
                        // Số Hóa Đơn
                        e.Graphics.DrawString(
                            string.Format("Số Hóa Đơn : {0}", maHD),
                            new Font("Courier New", 12, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(w / 2 + 200, 20)
                            );
                        // Địa chỉ, phone
                        e.Graphics.DrawString(
                            string.Format("{0} - {1}", diaChiCH, phone),
                            new Font("Courier New", 8, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(100, 45)
                            );
                        // Ngày xuất hóa đơn
                        e.Graphics.DrawString(
                           string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm")),
                           new Font("Courier New", 12, FontStyle.Bold),
                           Brushes.Black,
                           new PointF(w / 2 + 200, 45)
                           );
                        Pen blackPen = new Pen(Color.Black, 1);
                        var y = 70;

                        Point p1 = new Point(10, y);
                        Point p2 = new Point(w - 10, y);
                        e.Graphics.DrawLine(blackPen, p1, p2);
                        y += 10;
                        e.Graphics.DrawString(
                            string.Format("Tên Khách Hàng : {0}", tenKhachHang),
                            new Font("Courier New", 10, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(100, y)
                            );
                        e.Graphics.DrawString(
                            string.Format("Số Tiền Thanh Toán : {0}", thanhTien),
                            new Font("Courier New", 10, FontStyle.Bold),
                            Brushes.Black,
                            new PointF(w / 2, y)
                            );

                        y += 20;
                        p1 = new Point(10, y);
                        p2 = new Point(w - 10, y);
                        e.Graphics.DrawLine(blackPen, p1, p2);
                        #endregion
                        #region body

                        e.Graphics.DrawString("STT", new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(10, y));
                        e.Graphics.DrawString("Tên Vật Liệu", new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(50, y));
                        e.Graphics.DrawString("Số Lượng", new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w / 2 + 100, y));
                        e.Graphics.DrawString("Đơn Giá", new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w / 2, y));
                        e.Graphics.DrawString("Thành Tiền", new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w - 200, y));

                        int i = 1;
                        y += 20;
                        foreach (DataGridViewRow row1 in dataGridView_GioHang.Rows)
                        {
                            if (row1.Cells.Count >= 7 && row1.Cells[0].Value != null)
                            {
                                string tenVatTu = row1.Cells[1].Value.ToString();
                                string giaBan = row1.Cells[2].Value.ToString();
                                string soLuong = row1.Cells[3].Value.ToString();
                                string thanhtiens = row1.Cells[4].Value.ToString();

                                e.Graphics.DrawString(string.Format("{0}", i++), new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(10, y));
                                e.Graphics.DrawString(string.Format("{0}", tenVatTu), new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(50, y));
                                e.Graphics.DrawString(string.Format("{0}", soLuong), new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w / 2 + 100, y));
                                e.Graphics.DrawString(string.Format("{0}", giaBan), new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w / 2, y));
                                e.Graphics.DrawString(string.Format("{0}", thanhtiens), new Font("Courier New", 10, FontStyle.Bold), Brushes.Black, new Point(w - 200, y));
                                y += 20;

                            }
                        }
                        y += 40;
                        p1 = new Point(10, y);
                        p2 = new Point(w - 10, y);
                        e.Graphics.DrawLine(blackPen, p1, p2);
                        #endregion
                    }
                }

            }
        }
    }
}
