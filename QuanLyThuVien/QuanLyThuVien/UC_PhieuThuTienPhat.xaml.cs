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

            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            InitMaPhieuThu();
            HienThiDanhSachPhieuThuTienPhat();
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
            string query = "SELECT PHIEUTHUTIENPHAT.MaPhieuThuTien, PHIEUTHUTIENPHAT.MaDocGia, DOCGIA.HoVaTen, PHIEUTHUTIENPHAT.NgayThu, DOCGIA.TongNo, PHIEUTHUTIENPHAT.SoTienThu, PHIEUTHUTIENPHAT.ConLai FROM PHIEUTHUTIENPHAT JOIN DOCGIA ON PHIEUTHUTIENPHAT.MaDocGia = DOCGIA.MaDocGia";
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
                tblMaPhieuThu.Text = row_selected.Row["MaPhieuThuTien"].ToString();
                cbMaDocGia.Text = row_selected.Row["MaDocGia"].ToString();
                tblHoTen.Text = row_selected.Row["HoVaTen"].ToString();
                tblTongNo.Text = row_selected.Row["TongNo"].ToString();
                txbSoTienThu.Text = row_selected.Row["SoTienThu"].ToString();
                tblConLai.Text = row_selected.Row["ConLai"].ToString();
                dpNgayThu.Text = row_selected.Row["NgayThu"].ToString();
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

            string selectedMaDocGia = "";
            if ((cbMaDocGia.SelectedItem) != null)
                selectedMaDocGia = ((ComboBoxItem)cbMaDocGia.SelectedItem).Content.ToString();
            if (selectedMaDocGia != "")
            {
                try
                {
                    
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Open();   
                    string query = "SELECT HoVaTen FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", selectedMaDocGia);
                    tblHoTen.Text = sqlCommand.ExecuteScalar().ToString();

                    query = "SELECT TongNo FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", selectedMaDocGia);
                    tblTongNo.Text = sqlCommand.ExecuteScalar().ToString();

                    sqlConnection.Close();

                }
            }    
           
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO PHIEUTHUTIENPHAT (MaPhieuThuTien, MaDocGia, SoTienThu, ConLai, NgayThu) VALUES (@MaPhieuThuTien, @MaDocGia, @SoTienThu, @ConLai, @NgayThu)";
                sqlConnection.Open();
               
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@SoTienThu", txbSoTienThu.Text);
                sqlCommand.Parameters.AddWithValue("@ConLai", tblConLai.Text);
                sqlCommand.Parameters.AddWithValue("@NgayThu", dpNgayThu.Text);
                if (double.Parse(tblConLai.Text) < 0)
                    MessageBox.Show("Tiền thu không được lớn hơn tổng nợ");
                else
                {
                    sqlCommand.ExecuteScalar();

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
                MessageBox.Show("Thêm thành công");
                sqlConnection.Close();
                HienThiDanhSachPhieuThuTienPhat();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE PHIEUTHUTIENPHAT  SET MaDocGia = @MaDocGia, SoTienThu = @SoTienThu, NgayThu = @NgayThu WHERE MaPhieuThuTien = @MaPhieuThuTien";
                sqlConnection.Open();
                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@SoTienThu", txbSoTienThu.Text);
                sqlCommand.Parameters.AddWithValue("@NgayThu", dpNgayThu.Text);
                sqlCommand.ExecuteScalar();

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
                string query = "DELETE FROM PHIEUTHUTIENPHAT WHERE MaPhieuThuTien = @MaPhieuThuTien";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaPhieuThuTien", tblMaPhieuThu.Text);
                sqlCommand.ExecuteScalar();
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
                MessageBox.Show("Invalid input"); // Thay thế bằng thông báo hoặc xử lý khác
            }
        }
    }
}
