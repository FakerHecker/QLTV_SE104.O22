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
    /// Interaction logic for UC_PhieuThuTienPhat.xaml
    /// </summary>
   
    public partial class UC_PhieuThuTienPhat : UserControl
    {
        SqlConnection sqlConnection;
        private string maPhieuThu = "PTTP000";
        public UC_PhieuThuTienPhat()
        {
            InitializeComponent();

            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            InitMaPhieuThu();
            InitMaDocGia();
            HienThiDanhSachPhieuThuTienPhat();
        }

        private void InitMaDocGia()
        {
            string query = "SELECT * FROM DOCGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["MaDocGia"].ToString();
                cbMaDocGia.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitMaPhieuThu()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaPhieuThuTien FROM PHIEUTHUTIENPHAT ORDER BY MaPhieuThuTien DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maPhieuThu = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private void HienThiDanhSachPhieuThuTienPhat()
        {
            sqlConnection.Open();
            string query = "SELECT MaPhieuThuTien AS 'Mã phiếu thu tiền', MaDocGia AS 'Mã độc giả', NgayThu AS 'Ngày thu', SoTienThu AS 'Số tiền thu', ConLai AS 'Còn lại' FROM PHIEUTHUTIENPHAT";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPhieuThuTienPhat.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvPhieuThuTienPhat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaPhieuThu.Text = row_selected.Row["Mã phiếu thu tiền"].ToString();
                cbMaDocGia.Text = row_selected.Row["Mã độc giả"].ToString();
                txbSoTienThu.Text = row_selected.Row["Số tiền thu"].ToString();
                tblTongNo.Text = "";
                tblConLai.Text = row_selected.Row["Còn lại"].ToString();
                dpNgayThu.Text = row_selected.Row["Ngày thu"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maPhieuThu.Substring(4)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            maPhieuThu = $"PTTP{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maPhieuThu;
        }
        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaPhieuThu.Text = IncreasePrimaryKey();
            cbMaDocGia.SelectedIndex = -1;
            tblHoTen.Text = "";
            tblTongNo.Text = "";
            txbSoTienThu.Text = "";
            tblConLai.Text = "";
            dpNgayThu.Text = DateTime.Now.ToString();
        }


        private void cbMaDocGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMaDocGia.SelectedIndex != -1)
            {
                sqlConnection.Open();
                string maDocGia = cbMaDocGia.SelectedValue as string;
                string query = "SELECT HoVaTen FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                tblHoTen.Text = sqlCommand.ExecuteScalar().ToString();

                query = "SELECT TongNo FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                tblTongNo.Text = sqlCommand.ExecuteScalar().ToString();
                sqlConnection.Close();

                txbSoTienThu.Text = "";
                tblConLai.Text = "";
            }    
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUTHUTIENPHAT WHERE MaPhieuThuTien = @MaPhieuThuTien";
                SqlCommand sqlCommand = new SqlCommand( query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);

                if (tblMaPhieuThu.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (cbMaDocGia.Text == "")
                    MessageBox.Show("Mã độc giả không được để trống");
                else if (txbSoTienThu.Text == "")
                    MessageBox.Show("Số tiền thu không được để trống");
                else if (double.Parse(tblConLai.Text) < 0)
                    MessageBox.Show("Số tiền thu không được lớn hơn tổng nợ");
                else if (double.Parse(txbSoTienThu.Text) == 0)
                    MessageBox.Show("Số tiền thu phải lớn hơn 0");
                else if (sqlCommand.ExecuteScalar() != null)
                {
                    MessageBox.Show("Đã tồn tại phiếu thu tiền phạt");
                }
                else
                {

                    query = "INSERT INTO PHIEUTHUTIENPHAT (MaPhieuThuTien, MaDocGia, SoTienThu, ConLai, NgayThu) VALUES (@MaPhieuThuTien, @MaDocGia, @SoTienThu, @ConLai, @NgayThu)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@SoTienThu", txbSoTienThu.Text);
                    sqlCommand.Parameters.AddWithValue("@ConLai", tblConLai.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayThu", dpNgayThu.Text);
                    sqlCommand.ExecuteScalar();
                    MessageBox.Show("Thêm thành công");

                    query = "UPDATE DOCGIA SET TongNo = @TongNo WHERE MaDocGia = @MaDocGia";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@TongNo", tblConLai.Text);
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
                HienThiDanhSachPhieuThuTienPhat();
            }
        }

       

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUTHUTIENPHAT WHERE MaPhieuThuTien = @MaPhieuThuTien";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
                if (sqlCommand.ExecuteScalar() == null)
                {
                    MessageBox.Show("Không tồn tại phiếu thu tiền phạt");
                }
                {
                    query = "DELETE FROM PHIEUTHUTIENPHAT WHERE MaPhieuThuTien = @MaPhieuThuTien";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
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
                HienThiDanhSachPhieuThuTienPhat();
                tblMaPhieuThu.Text = "";
                cbMaDocGia.SelectedIndex = -1;
                tblHoTen.Text = "";
                tblTongNo.Text = "";
                txbSoTienThu.Text = "";
                tblConLai.Text = "";
                dpNgayThu.Text = "";
            }
        }

        private void txbSoTienThu_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txbSoTienThu.Text, out double inputValue))
            {
                // Người dùng đã nhập số
                double tongNo = 0;
                if (tblTongNo.Text.ToString() != "")
                {
                    tongNo = double.Parse(tblTongNo.Text.ToString());
                    double result = tongNo - (inputValue); // Thay thế hàm tính toán thực tế
                    tblConLai.Text = result.ToString();
                }    
                    
            }
            else if (txbSoTienThu.Text == "")
            {
                tblConLai.Text = "";
            }
            else
            {
                // Người dùng đã nhập chữ
                MessageBox.Show("Phải nhập số"); // Thay thế bằng thông báo hoặc xử lý khác
                txbSoTienThu.Text = "";
            }
        }

        private void btnInPhieuThu_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(a, "My Control Print");
            }  
        }
    }
}
