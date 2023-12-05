using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_DotNet
{
    internal class DBConnect
    {
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.conStr);
        public SqlConnection conn
        {
            get { return cn; }
            set { cn = value; }
        }
        public SqlConnection GetConnection()
        {
            return cn;
        }
        public DataTable getDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, cn);
            da.Fill(dt);
            return dt;
        }
        public int updateDatabaseVatLieu(DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from VATLIEU", cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            int kq = da.Update(dt);
            return kq;
        }
        public bool Kt_TrungKhoa_VatLieu(string a)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            SqlCommand cmd;
            string sql_selected = "SELECT Count(*) FROM VATLIEU WHERE MAVL = @MaVL";
            cmd = new SqlCommand(sql_selected, cn);
            cmd.Parameters.AddWithValue("@MaVL", a);

            object result = cmd.ExecuteScalar();

            if (conn.State == ConnectionState.Open)
                conn.Close();

            if (result != null && result != DBNull.Value)
            {
                int kq = Convert.ToInt32(result);
                if (kq == 1)
                    return false;
            }
            return true;
        }
        public List<string> loadListBoxSell(string sql)
        {
            List<string> lst = new List<string>();
            SqlCommand cmd;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            cmd = new SqlCommand(sql, cn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                string tenvl = rd["TENVL"].ToString();
                lst.Add(tenvl);
            }
            if (conn.State == ConnectionState.Open)
                conn.Close();
            return lst;
        }
        public void OpenConnection()
        {
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }
        }
        public void CloseConnection()
        {
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
        }
        public int updateDatabase(DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Users", cn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            int kq = da.Update(dt);
            return kq;
        }
        public bool Kt_TrungKhoa(string a)
        {
            SqlCommand cmd;
            string sql_selected = "select Count(*) from Users Where userName = '" + a + "'";
            if (cn.State == ConnectionState.Closed)
                cn.Open();
            cmd = new SqlCommand(sql_selected, cn);
            int kq = (int)cmd.ExecuteScalar();
            if (cn.State == ConnectionState.Open)
                cn.Close();
            if (kq == 1)
                return false;
            else
                return true;
        }
    }
}
