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
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
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
            string query = "SELECT MaLoaiDocGia AS 'Mã loại độc giả', TenLoaiDocGia AS 'Tên loại độc giả' FROM LOAIDOCGIA";
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
                tblMaLoaiDocGia.Text = row_selected.Row["Mã loại độc giả"].ToString();
                txbTenLoaiDocGia.Text = row_selected.Row["Tên loại độc giả"].ToString();
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
                sqlConnection.Open();
                string query = "SELECT * FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                object tonTaiLDG = sqlCommand.ExecuteScalar();

                query = "SELECT TenLoaiDocGia FROM LOAIDOCGIA WHERE TenLoaiDocGia = @TenLoaiDocGia";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                object trungTen = sqlCommand.ExecuteScalar();

                if (tblMaLoaiDocGia.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (tonTaiLDG != null || trungTen != null)
                    MessageBox.Show("Đã tồn tại tên loại độc giả");
                else if (txbTenLoaiDocGia.Text == "")
                    MessageBox.Show("Tên loại độc giả không được để trống");
                else
                {
                    MessageBox.Show("Thêm thành công");
                    query = "INSERT INTO LOAIDOCGIA (MaLoaiDocGia, TenLoaiDocGia) VALUES (@MaLoaiDocGia, @TenLoaiDocGia)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                    sqlCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Không thể lưu");
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
                sqlConnection.Open();

                string query = "SELECT * FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                object khongTonTai = sqlCommand.ExecuteScalar();

                query = "SELECT TenLoaiDocGia FROM LOAIDOCGIA WHERE TenLoaiDocGia = @TenLoaiDocGia";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                object trungTen = sqlCommand.ExecuteScalar();

                if (khongTonTai == null)
                    MessageBox.Show("Không tồn tại loại độc giả");
                else if (trungTen != null)
                    MessageBox.Show("Đã tồn tại tên loại độc giả");
                else if (txbTenLoaiDocGia.Text == "")
                    MessageBox.Show("Tên loại độc giả không được để trống");
                else
                {
                    MessageBox.Show("Cập nhật thành công");
                    query = "UPDATE LOAIDOCGIA SET TenLoaiDocGia = @TenLoaiDocGia  WHERE MaLoaiDocGia = @MaLoaiDocGia";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                    sqlCommand.ExecuteScalar();
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Không thể cập nhật");
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                    if (sqlCommand.ExecuteScalar() == null)
                        MessageBox.Show("Không tồn tại loại độc giả");
                    else
                    {
                        query = "DELETE FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                        sqlCommand.ExecuteScalar();

                        tblMaLoaiDocGia.Text = "";
                        txbTenLoaiDocGia.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    MessageBox.Show("Loại độc giả đang được sử dụng, không thể xóa");
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachLoaiDocGia();
                }
            }             
        }
    }
}
