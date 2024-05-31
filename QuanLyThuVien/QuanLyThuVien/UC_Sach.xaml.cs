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
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            InitTenDauSach();
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

        private void InitTenDauSach()
        {
            string query = "SELECT TenDauSach FROM DAUSACH"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string maDS = reader["TenDauSach"].ToString();
                cbTenDauSach.Items.Add(maDS);
            }
            sqlConnection.Close();
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT MaSach AS 'Mã sách', TenDauSach AS 'Tên đầu sách', " +
                "TenTheLoai AS 'Thể loại', NamXuatBan AS 'Năm xuất bản', " +
                "NhaXuatBan AS 'Nhà xuất bản', SoLuongTon AS 'Số lượng tồn' " +
                "FROM SACH JOIN DAUSACH ON SACH.MaDauSach = DAUSACH.MaDauSach " +
                "JOIN THELOAI ON THELOAI.MaTheLoai = DAUSACH.MaTheLoai";
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
                tblMaSach.Text = row_selected.Row["Mã sách"].ToString();
                // txbTenSach.Text = row_selected.Row["Tên sách"].ToString();
                cbTenDauSach.Text = row_selected.Row["Tên đầu sách"].ToString();
                txbNamXuatBan.Text = row_selected.Row["Năm xuất bản"].ToString();
                txbNhaXuatBan.Text = row_selected.Row["Nhà xuất bản"].ToString();
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
            tblMaSach.Text = IncreasePrimaryKey();
            txbTenSach.Text = "";
            cbTenDauSach.SelectedIndex = -1;
            txbNamXuatBan.Text = "";
            txbNhaXuatBan.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'KhoangCachNamXuatBan'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                int khoangCachNamXuatBan = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                query = "SELECT * FROM SACH WHERE MaSach = @MaSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                object tonTaiMaSach = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM SACH WHERE TenSach = @TenSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
                object tonTaiTenSach = sqlCommand.ExecuteScalar();

                if (tblMaSach.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (tonTaiMaSach != null)
                    MessageBox.Show("Đã tồn tại sách");
                else if (txbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else if (tonTaiTenSach != null)
                    MessageBox.Show("Tên sách không được trùng");
                else if (cbTenDauSach.SelectedIndex == -1)
                    MessageBox.Show("Tên đầu sách không được để trống");
                else if (txbNhaXuatBan.Text == "")
                    MessageBox.Show("Nhà xuất bản không được để trống");
                else if (Int32.TryParse(txbNamXuatBan.Text, out int namXuatBan))
                {
                    if (DateTime.Now.Year - namXuatBan <= khoangCachNamXuatBan && DateTime.Now.Year - namXuatBan >= 0)
                    {

                        query = "SELECT MaDauSach FROM DAUSACH WHERE TenDauSach = @TenDauSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@TenDauSach", cbTenDauSach.Text);
                        string maDauSach = sqlCommand.ExecuteScalar().ToString();

                        query = "INSERT INTO SACH (MaSach, TenSach, MaDauSach, NamXuatBan, NhaXuatBan) VALUES (@MaSach, @TenSach, @MaDauSach, @NamXuatBan, @NhaXuatBan)";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                        sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
                        sqlCommand.Parameters.AddWithValue("@MaDauSach", maDauSach);
                        sqlCommand.Parameters.AddWithValue("@NamXuatBan", txbNamXuatBan.Text);
                        sqlCommand.Parameters.AddWithValue("@NhaXuatBan", txbNhaXuatBan.Text);
                        sqlCommand.ExecuteScalar();
                        MessageBox.Show("Thêm thành công");
                    }
                    else
                    {
                        MessageBox.Show("Khoảng cách năm xuất bản là " + khoangCachNamXuatBan.ToString());
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

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'KhoangCachNamXuatBan'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                int khoangCachNamXuatBan = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                query = "SELECT * FROM SACH WHERE MaSach = @MaSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                object tonTaiMaSach = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM SACH WHERE TenSach = @TenSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
                sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                object tonTaiTenSach = sqlCommand.ExecuteScalar();

                if (tblMaSach.Text == "")
                    MessageBox.Show("Vui lòng chọn sách để cập nhật thông tin");
                else if (tonTaiMaSach == null)
                    MessageBox.Show("Không tồn tại sách");
                else if (tonTaiTenSach != null)
                    MessageBox.Show("Không được trùng tên sách");
                else if (txbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else if (txbNhaXuatBan.Text == "")
                    MessageBox.Show("Nhà xuất bản không được để trống");
                else if (Int32.TryParse(txbNamXuatBan.Text, out int namXuatBan))
                {
                    if (DateTime.Now.Year - namXuatBan <= khoangCachNamXuatBan && DateTime.Now.Year - namXuatBan >= 0)
                    {

                        query = "SELECT MaDauSach FROM DAUSACH WHERE TenDauSach = @TenDauSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@TenDauSach", cbTenDauSach.Text);
                        string maDauSach = sqlCommand.ExecuteScalar().ToString();

                        query = "UPDATE SACH  SET TenSach = @TenSach, MaDauSach = @MaDauSach, NamXuatBan = @NamXuatBan, NhaXuatBan = @NhaXuatBan WHERE MaSach = @MaSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                        sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
                        sqlCommand.Parameters.AddWithValue("@MaDauSach", maDauSach);
                        sqlCommand.Parameters.AddWithValue("@NamXuatBan", txbNamXuatBan.Text);
                        sqlCommand.Parameters.AddWithValue("@NhaXuatBan", txbNhaXuatBan.Text);
                        sqlCommand.ExecuteScalar();
                        MessageBox.Show("Cập nhật thành công");
                    }
                    else
                    {
                        MessageBox.Show("Khoảng cách năm xuất bản là " + khoangCachNamXuatBan.ToString());
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

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM SACH WHERE MaSach = @MaSach";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                    if (sqlCommand.ExecuteScalar() == null)
                        MessageBox.Show("Không tồn tại sách");
                    else
                    {
                        query = "DELETE FROM SACH WHERE MaSach = @MaSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", tblMaSach.Text);
                        sqlCommand.ExecuteScalar();

                        tblMaSach.Text = "";
                        txbTenSach.Text = "";
                        cbTenDauSach.SelectedIndex = -1;
                        txbNamXuatBan.Text = "";
                        txbNhaXuatBan.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    MessageBox.Show("Dữ liệu sách đang được sử dụng, không thể xóa");
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachSach();

                   
                }
            }            
        }

        private void cbTenDauSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTenDauSach.SelectedIndex != -1)
            {
                sqlConnection.Open();
                string query = "SELECT MaDauSach FROM DAUSACH WHERE TenDauSach = @TenDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenDauSach", cbTenDauSach.SelectedValue as string);
                object maDauSach = sqlCommand.ExecuteScalar();
                
                if (maDauSach != null)
                {

                    query = "SELECT TenTheLoai FROM DAUSACH JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE MaDauSach = @maDauSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@maDauSach", maDauSach.ToString());
                    object tenTheLoai = sqlCommand.ExecuteScalar();
                    if (tenTheLoai != null)
                        tblTheLoai.Text = tenTheLoai.ToString();
                }
                sqlConnection.Close();
            }    
        }
    }
}
