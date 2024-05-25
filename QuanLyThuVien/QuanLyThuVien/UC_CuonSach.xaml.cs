using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for UC_CuonSach.xaml
    /// </summary>
    public partial class UC_CuonSach : UserControl
    {
        SqlConnection sqlConnection;
        private string maCuonSach = "CS000";
        public UC_CuonSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            InitTenSach();
            InitMaCuonSach();
            HienThiDanhSachCuonSach();
        }

        private void InitTenSach()
        {
            cbTenSach.Items.Clear();
            string query = "SELECT TenSach FROM SACH"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["TenSach"].ToString();
                cbTenSach.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitMaCuonSach()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaCuonSach FROM CUONSACH ORDER BY MaCuonSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maCuonSach = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private void HienThiDanhSachCuonSach()
        {
            sqlConnection.Open();
            string query = "SELECT MaCuonSach AS 'Mã cuốn sách', TenSach AS 'Tên sách', TinhTrang AS 'Tình trạng' FROM CUONSACH JOIN SACH ON CUONSACH.MaSach = SACH.MaSach";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCuonSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvCuonSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaCuonSach.Text = row_selected.Row["Mã cuốn sách"].ToString();
                cbTenSach.Text = row_selected.Row["Tên sách"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maCuonSach.Substring(2)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maCuonSach = $"CS{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maCuonSach;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaCuonSach.Text = IncreasePrimaryKey();
            cbTenSach.Text = "";
        }


        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM CUONSACH WHERE MaCuonSach = @MaCuonSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                if (tblMaCuonSach.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (sqlCommand.ExecuteScalar() != null)
                    MessageBox.Show("Đã tồn tại cuốn sách");
                else if (cbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else
                {
                    MessageBox.Show("Thêm thành công");

                    query = "SELECT MaSach FROM SACH WHERE TenSach = @TenSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenSach", cbTenSach.Text);
                    string maSach = sqlCommand.ExecuteScalar().ToString();

                    query = "INSERT INTO CUONSACH (MaCuonSach, MaSach, TinhTrang) VALUES (@MaCuonSach, @MaSach, 0)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
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
                HienThiDanhSachCuonSach();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();

                string query = "SELECT * FROM CUONSACH WHERE MaCuonSach = @MaCuonSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tồn tại cuốn sách");
                else if (cbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else
                {
                    MessageBox.Show("Cập nhật thành công");

                    query = "SELECT MaSach FROM SACH WHERE TenSach = @TenSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenSach", cbTenSach.Text);
                    string maSach = sqlCommand.ExecuteScalar().ToString();

                    query = "UPDATE CUONSACH SET MaSach = @MaSach  WHERE MaCuonSach = @MaCuonSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
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
                HienThiDanhSachCuonSach();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    DataRowView selectedRow = dgvCuonSach.SelectedItem as DataRowView;
                    if (selectedRow != null)
                    {
                        if (selectedRow.Row["Tình trạng"].ToString() == "True")
                        {
                            MessageBox.Show("Cuốn sách đang được mượn");
                        }
                        else
                        {
                            sqlConnection.Open();
                            string query = "SELECT * FROM CUONSACH WHERE MaCuonSach = @MaCuonSach";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                            if (sqlCommand.ExecuteScalar() == null)
                                MessageBox.Show("Không tồn tại loại độc giả");
                            else
                            {
                                query = "DELETE FROM CUONSACH WHERE MaCuonSach = @MaCuonSach";
                                sqlCommand = new SqlCommand(query, sqlConnection);
                                sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                                sqlCommand.ExecuteScalar();

                                tblMaCuonSach.Text = "";
                                cbTenSach.Text = "";
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.ToString());
                    MessageBox.Show("Cuốn sách đang được sử dụng, không thể xóa");
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachCuonSach();
                }
            }    
        }
    }
}
