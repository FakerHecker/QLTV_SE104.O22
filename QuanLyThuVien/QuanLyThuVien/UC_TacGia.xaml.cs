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
    /// Interaction logic for UC_TacGia.xaml
    /// </summary>
    public partial class UC_TacGia : UserControl
    {
        SqlConnection sqlConnection;
        private string maTacGia = "TG000";
        public UC_TacGia()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            InitMaTacGia();
            HienThiDanhSachTacGia();
        }

        private void InitMaTacGia()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaTacGia FROM TACGIA ORDER BY MaTacGia DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maTacGia = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private void HienThiDanhSachTacGia()
        {
            sqlConnection.Open();
            string query = "SELECT MaTacGia AS 'Mã tác giả', TenTacGia AS 'Tên tác giả' FROM TACGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvTacGia.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvTacGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaTacGia.Text = row_selected.Row["Mã tác giả"].ToString();
                txbTenTacGia.Text = row_selected.Row["Tên tác giả"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maTacGia.Substring(2)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maTacGia = $"TG{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maTacGia;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaTacGia.Text = IncreasePrimaryKey();
            txbTenTacGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM TACGIA WHERE MaTacGia = @MaTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                object tonTaiMaTacGia = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM TACGIA WHERE TenTacGia = @TenTacGia";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
                object tonTaiTenTacGia = sqlCommand.ExecuteScalar();


                if (tblMaTacGia.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (txbTenTacGia.Text == "")
                    MessageBox.Show("Tên tác giả không được bỏ trống");
                else if (tonTaiMaTacGia != null)
                    MessageBox.Show("Đã tồn tại mã tác giả");
                else if (tonTaiTenTacGia != null)
                    MessageBox.Show("Đã tồn tại tên tác giả");
                else
                {
                    query = "INSERT INTO TACGIA (MaTacGia, TenTacGia) VALUES (@MaTacGia, @TenTacGia)";
                    MessageBox.Show("Thêm thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                    sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
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
                HienThiDanhSachTacGia();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                sqlConnection.Open();
                string query = "SELECT * FROM TACGIA WHERE MaTacGia = @MaTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                object tonTaiMaTacGia = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM TACGIA WHERE TenTacGia = @TenTacGia";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
                object tonTaiTenTacGia = sqlCommand.ExecuteScalar();

                if (tonTaiMaTacGia == null)
                    MessageBox.Show("Không tồn tại tác giả");
                else if (tonTaiTenTacGia != null)
                    MessageBox.Show("Đã tồn tại tên tác giả");
                else if (txbTenTacGia.Text == "")
                    MessageBox.Show("Tên tác giả không được bỏ trống");
                else
                {
                    query = "UPDATE TACGIA SET TenTacGia = @TenTacGia  WHERE MaTacGia = @MaTacGia";
                    MessageBox.Show("Cập nhật thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                    sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
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
                HienThiDanhSachTacGia();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM TACGIA WHERE MaTacGia = @MaTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);

                if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tồn tại tác giả");
                else
                {
                    query = "DELETE FROM TACGIA WHERE MaTacGia = @MaTacGia";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                    sqlCommand.ExecuteScalar();
                }    
               

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Tác giả đang được sử dụng");
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachTacGia();
                tblMaTacGia.Text = "";
                txbTenTacGia.Text = "";
            }

        }

    }
}
