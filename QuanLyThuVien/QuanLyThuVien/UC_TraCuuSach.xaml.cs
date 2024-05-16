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
using static MaterialDesignThemes.Wpf.Theme;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_TraCuuSach.xaml
    /// </summary>
    public partial class UC_TraCuuSach : UserControl
    {
        SqlConnection sqlConnection;
        public UC_TraCuuSach()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachSach();
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT CUONSACH.MaCuonSach, SACH.TenSach, THELOAI.TenTheLoai, TACGIA.TenTacGia FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCuonSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }


        private void btnTimSach_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            if (rbMaSach.IsChecked == true)
            {
                
                string query = "SELECT CUONSACH.MaCuonSach, SACH.TenSach, THELOAI.TenTheLoai, TACGIA.TenTacGia " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE MaCuonSach = @MaCuonSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCuonSach", txbMaCuonSach.Text);
                
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;


            }
            else if (rbTenSach.IsChecked == true) 
            {
                string query = "SELECT CUONSACH.MaCuonSach, SACH.TenSach, THELOAI.TenTheLoai, TACGIA.TenTacGia " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenSach = @TenSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            else if (rbTheLoai.IsChecked == true)
            {
                string query = "SELECT CUONSACH.MaCuonSach, SACH.TenSach, THELOAI.TenTheLoai, TACGIA.TenTacGia " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenTheLoai = @TenTheLoai";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTheLoai", txbTheLoai.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            else if (rbTacGia.IsChecked == true)
            {
                string query = "SELECT CUONSACH.MaCuonSach, SACH.TenSach, THELOAI.TenTheLoai, TACGIA.TenTacGia " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenTacGia = @TenTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTacGia.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            sqlConnection.Close();

            sqlConnection.Open();
            string query1 = "SELECT COUNT(*) FROM CUONSACH";
            SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection);
            tblTongSoSach.Text = sqlCommand1.ExecuteScalar().ToString();
            sqlConnection.Close();
            tblSoSachTimThay.Text = (dgvCuonSach.Items.Count - 1).ToString();

        }
    }
}
