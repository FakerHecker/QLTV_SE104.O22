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
    /// Interaction logic for UC_ChoMuonSach.xaml
    /// </summary>
    public partial class UC_ChoMuonSach : UserControl
    {
        SqlConnection sqlConnection;
        private string maPhieuMuonTraSach = "PMTS000";
        public UC_ChoMuonSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;"; 
            sqlConnection = new SqlConnection(connectionString);
            InitMaPhieuMuon();
            InitMaDocGia();
            InitPhieuMuon();
            HienThiDanhSachSach();
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void InitMaPhieuMuon()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaPhieuMuonTraSach FROM PHIEUMUONTRASACH ORDER BY MaPhieuMuonTraSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maPhieuMuonTraSach = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }
        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maPhieuMuonTraSach.Substring(4)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maPhieuMuonTraSach = $"PMTS{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maPhieuMuonTraSach;
        }
        private void InitPhieuMuon()
        {
            tblMaPhieuMuonTra.Text = IncreasePrimaryKey();
            dpNgayMuon.Text = DateTime.Now.ToString();
        }

        private void dgvSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaCuonSach.Text = row_selected.Row["MaCuonSach"].ToString();
            }
        }

        private void InitMaDocGia()
        {
            string query = "SELECT MaDocGia FROM DOCGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string maDS = reader["MaDocGia"].ToString();
                cbMaDocGia.Items.Add(maDS);
            }
            sqlConnection.Close();
        }

        private void btnChoMuon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO PHIEUMUONTRASACH (MaPhieuMuonTraSach, MaDocGia, NgayMuon, MaCuonSach, NgayPhaiTra) VALUES (@MaPhieuMuonTraSach, @MaDocGia, @NgayMuon, @MaCuonSach, @NgayPhaiTra)";
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand("SELECT TinhTrang FROM CUONSACH WHERE MaCuonSach = @MaCuonSach", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                if(sqlCommand.ExecuteScalar().ToString() == "False")
                {
                    MessageBox.Show("Thêm thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", tblMaPhieuMuonTra.Text);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayMuon", dpNgayMuon.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayPhaiTra", (dpNgayMuon.SelectedDate ?? DateTime.Now).AddDays(4).ToString());
                    sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                    sqlCommand.ExecuteScalar();
                }    
                else
                {
                    MessageBox.Show("Sách đã được mượn");
                }    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void btnTimSach_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach WHERE TenSach = @TenSach";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

            query = "SELECT COUNT(*) FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach WHERE TenSach = @TenSach";
            sqlConnection.Open();
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
            tblSoSachTimThay.Text = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();

        }
    }
}
