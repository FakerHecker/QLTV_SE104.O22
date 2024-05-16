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
    /// Interaction logic for UC_LoaiDocGia.xaml
    /// </summary>
    public partial class UC_LoaiDocGia : UserControl
    {
        SqlConnection sqlConnection;
        private string maLoaiDocGia = "LDG000";
        public UC_LoaiDocGia()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            InitMaLoaiDocGia();
            HienThiDanhSachLoaiDocGia();
        }

        private void InitMaLoaiDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaLoaiDocGia FROM LoaiDOCGIA ORDER BY MaLoaiDocGia DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maLoaiDocGia = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private void HienThiDanhSachLoaiDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM LOAIDOCGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvLoaiDocGia.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvLoaiDocGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaLoaiDocGia.Text = row_selected.Row["MaLoaiDocGia"].ToString();
                txbTenLoaiDocGia.Text = row_selected.Row["TenLoaiDocGia"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maLoaiDocGia.Substring(3)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maLoaiDocGia = $"LDG{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maLoaiDocGia;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaLoaiDocGia.Text = IncreasePrimaryKey();
            txbTenLoaiDocGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO LOAIDOCGIA (MaLoaiDocGia, TenLoaiDocGia) VALUES (@MaLoaiDocGia, @TenLoaiDocGia)";
                sqlConnection.Open();
               
                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE LOAIDOCGIA SET TenLoaiDocGia = @TenLoaiDocGia  WHERE MaLoaiDocGia = @MaLoaiDocGia";
                sqlConnection.Open();

                MessageBox.Show("Cập nhật thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }

        }
    }
}
