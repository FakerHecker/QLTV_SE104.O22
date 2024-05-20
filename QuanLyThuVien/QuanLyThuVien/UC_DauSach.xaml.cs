using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_DauSach.xaml
    /// </summary>
    public partial class UC_DauSach : UserControl
    {
        SqlConnection sqlConnection;
        string maDauSach = "DS000";
        public UC_DauSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;"; 
            sqlConnection = new SqlConnection(connectionString);
            InitMaTheLoai();
            InitMaTacGia();
            InitMaDauSach();
            HienThiDanhSachDauSach();
        }
        

        private void HienThiDanhSachDauSach()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM DAUSACH";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvDanhSachDauSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvDanhSachDauSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaDauSach.Text = row_selected.Row["MaDauSach"].ToString();
                txbTenDauSach.Text = row_selected.Row["TenDauSach"].ToString();
                cbMaTheLoai.Text = row_selected.Row["MaTheLoai"].ToString();

                lbTacGia.Items.Clear();

                sqlConnection.Open();
                SqlCommand command = new SqlCommand("SELECT MaTacGia FROM CT_TACGIA WHERE MaDauSach = @MaDauSach", sqlConnection);
                command.Parameters.AddWithValue("@MaDauSach", row_selected.Row["MaDauSach"].ToString());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string value = reader["MaTacGia"].ToString();
                    // Thêm giá trị vào danh sách hiển thị trong ListBox
                    lbTacGia.Items.Add(value);
                }
                sqlConnection.Close();
            }

            
        }

        private void InitMaTheLoai()
        {
            string query = "SELECT MaTheLoai FROM THELOAI"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["MaTheLoai"].ToString();
                cbMaTheLoai.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitMaTacGia()
        {
            string query = "SELECT MaTacGia FROM TACGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["MaTacGia"].ToString();
                cbTacGia.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitMaDauSach()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaDauSach FROM DAUSACH ORDER BY MaDauSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maDauSach = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private string IncreasePrimaryKey()
        {
            int currentNumber = int.Parse(maDauSach.Substring(2));
            currentNumber++;
            maDauSach = $"DS{currentNumber:D3}";
            return maDauSach;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaDauSach.Text = IncreasePrimaryKey();
            txbTenDauSach.Text = "";
            cbMaTheLoai.SelectedIndex = -1;
            cbTacGia.SelectedIndex = -1;
            lbTacGia.Items.Clear();
        }

        private void btnThemTacGia_Click(object sender, RoutedEventArgs e)
        {
            string tacGia = cbTacGia.Text;
            

            bool check = false;
            for (int i = 0; i < lbTacGia.Items.Count; i++)
            {
                var item = lbTacGia.Items[i].ToString();
                if (tacGia == item)
                    check = true;
            }
            if (check == false)
                lbTacGia.Items.Add(tacGia);
        }

        private void btnXoaTacGia_Click(object sender, RoutedEventArgs e)
        {
            if (lbTacGia.SelectedItem != null)
            {
                lbTacGia.Items.Remove(lbTacGia.SelectedItem);
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                sqlConnection.Open();
                string query = "INSERT INTO DAUSACH (MaDauSach, TenDauSach, MaTheLoai) VALUES (@MaDauSach, @TenDauSach, @MaTheLoai)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                sqlCommand.Parameters.AddWithValue("@MaTheLoai", cbMaTheLoai.Text);
                sqlCommand.ExecuteScalar();

                for (int i = 0; i < lbTacGia.Items.Count; i++)
                {
                    var item = lbTacGia.Items[i];
                    query = "INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaTacGia", item.ToString());
                    sqlCommand.ExecuteScalar();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "UPDATE DAUSACH SET TenDauSach = @TenDauSach, MaTheLoai = @MaTheLoai WHERE MaDauSach = @MaDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                sqlCommand.Parameters.AddWithValue("@MaTheLoai", cbMaTheLoai.Text);
                sqlCommand.ExecuteScalar();

                query = "DELETE FROM CT_TACGIA WHERE MaDauSach = @MaDauSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                sqlCommand.ExecuteScalar();

                for (int i = 0; i < lbTacGia.Items.Count; i++)
                {
                    var item = lbTacGia.Items[i];
                    query = "INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaTacGia", item.ToString());
                    sqlCommand.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "DELETE FROM CT_TACGIA WHERE MaDauSach = @MaDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                sqlCommand.ExecuteScalar();

                query = "DELETE FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();
            }
        }
    }
}
