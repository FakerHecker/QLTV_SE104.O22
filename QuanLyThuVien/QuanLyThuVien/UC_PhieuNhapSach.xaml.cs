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
using System.Windows.Threading;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_PhieuNhapSach.xaml
    /// </summary>
    public partial class UC_PhieuNhapSach : UserControl
    {
        SqlConnection sqlConnection;
        private string maPhieuNhapSach = "PNS000";

        private DispatcherTimer timer;
        public UC_PhieuNhapSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            InitMaPhieuNhap();
            HienThiPhieuNhap();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Gọi hàm để làm mới dữ liệu của DataGrid
            //HienThiPhieuNhap();
        }

        private void HienThiPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT MaPhieuNhapSach AS 'Mã phiếu nhập sách', NgayNhap AS 'Ngày nhập', TongTien AS 'Tổng tiền' FROM PHIEUNHAPSACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPhieuNhap.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvPhieuNhap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaPhieuNhap.Text = row_selected.Row["Mã phiếu nhập sách"].ToString();
                dpNgayNhap.Text = row_selected.Row["Ngày nhập"].ToString();
                tblTongTien.Text = row_selected.Row["Tổng tiền"].ToString();
            }
        }

        private void InitMaPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaPhieuNhapSach FROM PHIEUNHAPSACH ORDER BY MaPhieuNhapSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maPhieuNhapSach = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maPhieuNhapSach.Substring(3)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maPhieuNhapSach = $"PNS{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maPhieuNhapSach;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            tblMaPhieuNhap.Text = IncreasePrimaryKey();
            dpNgayNhap.Text = DateTime.Now.ToString();
            tblTongTien.Text = "0";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);

                if (tblMaPhieuNhap.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (dpNgayNhap.Text == "")
                    MessageBox.Show("Ngày nhập không được bỏ trống");
                else if (sqlCommand.ExecuteScalar() != null)
                    MessageBox.Show("Đã tồn tại phiếu nhập");
                else
                {
                    MessageBox.Show("Thêm thành công");
                    query = "INSERT INTO PHIEUNHAPSACH (MaPhieuNhapSach, NgayNhap, TongTien) VALUES (@MaPhieuNhapSach, @NgayNhap, @TongTien)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayNhap", dpNgayNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@TongTien", tblTongTien.Text);
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
                HienThiPhieuNhap();
            }
        }

        private void btnXemChiTietPhieu_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "SELECT * FROM PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
            object phieuNhap = sqlCommand.ExecuteScalar();
            sqlConnection.Close();

            if (phieuNhap == null)
                MessageBox.Show("Không tồn tại phiếu nhập");
            else
            {
                W_ChiTietPhieuNhap ChiTietPhieu = new W_ChiTietPhieuNhap(tblMaPhieuNhap.Text);
                ChiTietPhieu.Show();
            }                  
            
                    
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);

                if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tồn tại phiếu nhập");
                else
                {
                    query = "DELETE FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();

                    query = "DELETE FROM PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
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
                HienThiPhieuNhap();
                tblMaPhieuNhap.Text = "";
                dpNgayNhap.Text = "";
                tblTongTien.Text = "";
            }
        }
    }
}
