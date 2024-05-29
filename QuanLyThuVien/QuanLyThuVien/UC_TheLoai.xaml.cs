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
    /// Interaction logic for UC_TheLoai.xaml
    /// </summary>
    public partial class UC_TheLoai : UserControl
    {
        SqlConnection sqlConnection;
        private string maTheLoai= "TL000";
        public UC_TheLoai()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            InitMaTheLoai();
            HienThiDanhSachTheLoaiSach();
        }
        private void InitMaTheLoai()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaTheLoai FROM THELOAI ORDER BY MaTheLoai DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maTheLoai = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }
        private void HienThiDanhSachTheLoaiSach()
        {
            sqlConnection.Open();
            string query = "SELECT MaTheLoai AS 'Mã thể loại', TenTheLoai AS 'Tên thể loại' FROM THELOAI";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvLoaiSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvLoaiSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaTheLoai.Text = row_selected.Row["Mã thể loại"].ToString();
                txbTenTheLoai.Text = row_selected.Row["Tên thể loại"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maTheLoai.Substring(2)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maTheLoai = $"TL{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maTheLoai;
        }
        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaTheLoai.Text = IncreasePrimaryKey();
            txbTenTheLoai.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM THELOAI WHERE MaTheLoai = @MaTheLoai";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                object tonTai = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTheLoai", txbTenTheLoai.Text);
                object trungTen = sqlCommand.ExecuteScalar();

                if (tblMaTheLoai.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (txbTenTheLoai.Text == "")
                    MessageBox.Show("Tên thể loại không được bỏ trống");
                else if (tonTai != null )
                    MessageBox.Show("Trùng mã thể loại");
                else if (trungTen != null)
                    MessageBox.Show("Đã tồn tại tên thể loại");
                else
                {
                    query = "INSERT INTO THELOAI (MaTheLoai, TenTheLoai) VALUES (@MaTheLoai, @TenTheLoai)";                   
                    MessageBox.Show("Thêm thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                    sqlCommand.Parameters.AddWithValue("@TenTheLoai", txbTenTheLoai.Text);
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
                HienThiDanhSachTheLoaiSach();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                sqlConnection.Open();
                string query = "SELECT * FROM THELOAI WHERE MaTheLoai = @MaTheLoai";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                object khongTonTai = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTheLoai", txbTenTheLoai.Text);
                sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                object trungTen = sqlCommand.ExecuteScalar();

                if (khongTonTai == null)
                    MessageBox.Show("Không tồn tại mã thể loại");
                else if (trungTen != null)
                    MessageBox.Show("Đã tồn tại tên thể loại");
                else if (txbTenTheLoai.Text == "")
                    MessageBox.Show("Tên thể loại không được bỏ trống");
                else
                {
                    query = "UPDATE THELOAI SET TenTheLoai = @TenTheLoai  WHERE MaTheLoai = @MaTheLoai";
                    MessageBox.Show("Cập nhật thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                    sqlCommand.Parameters.AddWithValue("@TenTheLoai", txbTenTheLoai.Text);
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
                HienThiDanhSachTheLoaiSach();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM THELOAI WHERE MaTheLoai = @MaTheLoai";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);

                    if (sqlCommand.ExecuteScalar() == null)
                        MessageBox.Show("Không tồn tại thể loại");
                    else
                    {
                        query = "DELETE FROM THELOAI WHERE MaTheLoai = @MaTheLoai";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaTheLoai", tblMaTheLoai.Text);
                        sqlCommand.ExecuteScalar();
                        tblMaTheLoai.Text = "";
                        txbTenTheLoai.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    MessageBox.Show("Dữ liệu thể loại đang được sử dụng, không thể xóa");
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachTheLoaiSach();
                }
            }    
        }
    }
}
