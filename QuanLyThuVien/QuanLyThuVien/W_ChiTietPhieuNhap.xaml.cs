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
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_ChiTietPhieuNhap.xaml
    /// </summary>
    public partial class W_ChiTietPhieuNhap : Window
    {
        SqlConnection sqlConnection;
        string maPNS;
        private string maCTPN = "CTPNS000";
        public W_ChiTietPhieuNhap(string ma)
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            tblMaPhieuNhap.Text = ma;
            maPNS = ma;
            InitMaPhieuNhap();
            HienThiDanhSachChiTietPhieuNhap();
        }
        
        private void HienThiDanhSachChiTietPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", maPNS);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvChiTietPhieuNhap.ItemsSource = dt.DefaultView;
            tblTongTien.Text = CalculateTongTien();
            sqlConnection.Close();
        }
        private void InitMaPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaCTPhieuNhapSach FROM CT_PHIEUNHAPSACH ORDER BY MaCTPhieuNhapSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maCTPN = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private string IncreasePrimaryKey()
        {
            
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maCTPN.Substring(5));
            currentNumber++;
            maCTPN = $"CTPNS{currentNumber:D3}";
            return maCTPN;
        }

        private void InitMaSach()
        {
            string query = "SELECT MaSach FROM SACH"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["MaSach"].ToString();
                cbMaSach.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaCTPhieuNhap.Text = IncreasePrimaryKey();
            txbSoLuong.Text = "";
            txbDonGia.Text = "";
            InitMaSach();
        }

        private string CalculateTongTien()
        {
            float TongTien = 0;
            string query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
            TongTien = float.Parse(sqlCommand.ExecuteScalar().ToString());
            return TongTien.ToString();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    string query = "INSERT INTO CT_PHIEUNHAPSACH (MaCTPhieuNhapSach, MaPhieuNhapSach, MaSach, SoLuong, DonGia) VALUES (@MaCTPhieuNhapSach, @MaPhieuNhapSach, @MaSach, @SoLuong, @DonGia)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", cbMaSach.Text);
                    sqlCommand.Parameters.AddWithValue("@SoLuong", soLuong.ToString());
                    sqlCommand.Parameters.AddWithValue("@DonGia", donGia.ToString());
                    sqlCommand.ExecuteScalar();

                    float TongTien = 0;
                    query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    TongTien = float.Parse(sqlCommand.ExecuteScalar().ToString());

                    query = "UPDATE PHIEUNHAPSACH SET TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TongTien", TongTien.ToString());
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();


                    
                }
                else
                {
                    MessageBox.Show("Số lượng và đơn giá không phù hợp");
                } 
                    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachChiTietPhieuNhap();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                {
                    MessageBox.Show("Thêm thành công");
                    string query = "UPDATE CT_PHIEUNHAPSACH SET MaCTPhieuNhapSach = @MaCTPhieuNhapSach, MaPhieuNhapSach = @MaPhieuNhapSach, MaSach = @MaSach, SoLuong = @SoLuong, DonGia = @DonGia";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", cbMaSach.Text);
                    sqlCommand.Parameters.AddWithValue("@SoLuong", soLuong.ToString());
                    sqlCommand.Parameters.AddWithValue("@DonGia", donGia.ToString());
                    sqlCommand.ExecuteScalar();

                    float TongTien = 0;
                    query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    TongTien = float.Parse(sqlCommand.ExecuteScalar().ToString());

                    query = "UPDATE PHIEUNHAPSACH SET TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TongTien", TongTien.ToString());
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();


                   

                }
                else
                {
                    MessageBox.Show("Số lượng và đơn giá không phù hợp");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachChiTietPhieuNhap();
            }
        }

        private void txbSoLuong_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                tblThanhTien.Text = (soLuong * donGia).ToString();
        }

        private void txbDonGia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                tblThanhTien.Text = (soLuong * donGia).ToString();
        }
    }
}
