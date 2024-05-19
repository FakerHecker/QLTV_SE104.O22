using System;
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

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_Sach.xaml
    /// </summary>
    public partial class UC_Sach : UserControl
    {
        SqlConnection sqlConnection;
        private string maSach = "S000";
        public UC_Sach()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            InitMaSach();
            HienThiDanhSachSach();
        }

        private void InitMaSach()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaSach FROM SACH ORDER BY MaSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maSach = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private void InitMaDauSach()
        {
            string query = "SELECT MaDauSach FROM DAUSACH"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string maDS = reader["MaDauSach"].ToString();
                cbMaDauSach.Items.Add(maDS);
            }
            sqlConnection.Close();
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM SACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaSach.Text = row_selected.Row["MaSach"].ToString();
                txbTenSach.Text = row_selected.Row["TenSach"].ToString();
                cbMaDauSach.Text = row_selected.Row["MaDauSach"].ToString();

                //tblTenDauSach.Text = row_selected.Row["TenDauSach"].ToString();

                txbNamXuatBan.Text = row_selected.Row["NamXuatBan"].ToString();
                txbNhaXuatBan.Text = row_selected.Row["NhaXuatBan"].ToString();
            }
        }
        private string IncreasePrimaryKey()
        {
            int currentNumber = int.Parse(maSach.Substring(1)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            maSach = $"S{currentNumber:D3}"; // Format lại chuỗi số phiếu
            return maSach;
        }
        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            InitMaDauSach();
            tblMaSach.Text = IncreasePrimaryKey();
            txbTenSach.Text = "";
            cbMaDauSach.SelectedIndex = -1;
            txbNamXuatBan.Text = "";
            txbNhaXuatBan.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.TryParse(txbNamXuatBan.Text, out int namXuatBan))
                {
                    if (DateTime.Now.Year - namXuatBan <= 8 && DateTime.Now.Year - namXuatBan >= 0)
                    {
                        string query = "INSERT INTO SACH (MaSach, TenSach, MaDauSach, NamXuatBan, NhaXuatBan) VALUES (@MaSach, @TenSach, @MaDauSach, @NamXuatBan, @NhaXuatBan)";
                        sqlConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                        sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
                        sqlCommand.Parameters.AddWithValue("@MaDauSach", cbMaDauSach.Text);
                        sqlCommand.Parameters.AddWithValue("@NamXuatBan", txbNamXuatBan.Text);
                        sqlCommand.Parameters.AddWithValue("@NhaXuatBan", txbNhaXuatBan.Text);
                        sqlCommand.ExecuteScalar();
                        MessageBox.Show("Thêm thành công");
                    }    
                    else
                    {
                        MessageBox.Show("Khoảng cách năm xuất bản là 8");
                    }

                }
                else
                {
                    MessageBox.Show("Năm xuất bản không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {

                sqlConnection.Close();
                HienThiDanhSachSach();
            }
        }
    }
}
