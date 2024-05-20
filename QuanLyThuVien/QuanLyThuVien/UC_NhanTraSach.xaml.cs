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
    /// Interaction logic for UC_NhanTraSach.xaml
    /// </summary>
    public partial class UC_NhanTraSach : UserControl
    {
        SqlConnection sqlConnection;
        public UC_NhanTraSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachPhieuMuon();
        }

        private void HienThiDanhSachPhieuMuon()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM  PHIEUMUONTRASACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPhieuMuon.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void txbMaPhieuMuon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Xử lý sự kiện khi người dùng nhấn Enter
                //MessageBox.Show("Enter đã được nhấn!");
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Thêm giá trị vào TextBox (ví dụ: txbTenCot.Text = tenCotValue;)
                        tblHoVaTen.Text = reader["MaDocGia"].ToString();
                        dpNgayMuon.Text = reader["NgayMuon"].ToString();
                        dpNgayPhaiTra.Text = reader["NgayPhaiTra"].ToString();
                        dpNgayTra.Text = DateTime.Now.ToString();
                    }
                }

                sqlConnection.Close();

            }
        }

        private void dpNgayTra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime firstDateTime = dpNgayMuon.SelectedDate ?? DateTime.Now;
            DateTime secondDateTime =  dpNgayTra.SelectedDate ?? DateTime.Now;

            TimeSpan difference = secondDateTime.Subtract(firstDateTime);
            tblTienPhatKyNay.Text = difference.Days.ToString() + "000";
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Xử lý sự kiện khi người dùng nhấn Enter
                //MessageBox.Show("Enter đã được nhấn!");
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Thêm giá trị vào TextBox (ví dụ: txbTenCot.Text = tenCotValue;)
                        tblHoVaTen.Text = reader["MaDocGia"].ToString();
                        dpNgayMuon.Text = reader["NgayMuon"].ToString();
                        dpNgayPhaiTra.Text = reader["NgayPhaiTra"].ToString();
                        dpNgayTra.Text = DateTime.Now.ToString();
                    }
                }

                sqlConnection.Close();

            }
        }

        private void txbSoTienTra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (double.TryParse(txbSoTienTra.Text, out double soTienTra)) 
                {   
                    double conLai = (double.Parse(tblTienPhatKyNay.Text) - soTienTra);
                    if (conLai < 0)
                    {
                        MessageBox.Show("Số tiền trả không được vượt quá tiền phạt");
                    }
                    else
                    {
                        tblConlai.Text = conLai.ToString();
                    }    
                }
                else
                {
                    MessageBox.Show("Số tiền trả không hợp lệ");
                }

            }
        }

        private void btnInPhieuTra_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE PHIEUMUONTRASACH SET NgayTra = @NgayTra, TienPhatKyNay = @TienPhatKyNay, SoTienTra = @SoTienTra, ConLai = @ConLai WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                sqlCommand.Parameters.AddWithValue("@NgayTra", dpNgayPhaiTra.Text);
                sqlCommand.Parameters.AddWithValue("@TienPhatKyNay", tblTienPhatKyNay.Text);
                sqlCommand.Parameters.AddWithValue("@SoTienTra", txbSoTienTra.Text);
                sqlCommand.Parameters.AddWithValue("@ConLai", tblConlai.Text);
                sqlCommand.ExecuteScalar();
                MessageBox.Show("Thêm thành công");

                sqlCommand = new SqlCommand("SELECT TongNo FROM DOCGIA WHERE MaDocGia = @MaDocGia", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", tblHoVaTen.Text);
                string tongNo = sqlCommand.ExecuteScalar().ToString();

                double newTongNo = double.Parse(tongNo) + double.Parse(tblConlai.Text);
                sqlCommand = new SqlCommand("UPDATE DOCGIA SET TongNo = @TongNo WHERE MaDocGia = @MaDocGia", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", tblHoVaTen.Text);
                sqlCommand.Parameters.AddWithValue("@TongNo", newTongNo.ToString());
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {

                sqlConnection.Close();
                HienThiDanhSachPhieuMuon();
            }
        }
    }
}
