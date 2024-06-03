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
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            InitTieuChuanTraCuu();
            HienThiDanhSachSach();
        }

        private void InitTieuChuanTraCuu()
        {
            cbKey.Items.Add("Mã cuốn sách");
            cbKey.Items.Add("Tên sách");
            cbKey.Items.Add("Thể loại");
            cbKey.Items.Add("Tác giả");
            cbKey.Items.Add("Tình Trạng");
            cbKey.SelectedIndex = 0;
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvCuonSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }


        private void btnTimSach_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            if (cbKey.SelectedIndex == 0)
            {
                
                string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE MaCuonSach = @MaCuonSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCuonSach", cbValue.Text);
                
                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;


            }
            else if (cbKey.SelectedIndex == 1) 
            {
                string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenDauSach = @TenSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenSach", cbValue.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            else if (cbKey.SelectedIndex == 2)
            {
                string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenTheLoai = @TenTheLoai";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTheLoai", cbValue.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            else if (cbKey.SelectedIndex == 3)
            {
                string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TenTacGia = @TenTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", cbValue.Text);

                SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCuonSach.ItemsSource = dt.DefaultView;
            }
            else if (cbKey.SelectedIndex == 4)
            {
                string query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', DAUSACH.TenDauSach AS 'Tên sách', THELOAI.TenTheLoai AS 'Thể loại', TACGIA.TenTacGia AS 'Tác giả', TinhTrang AS 'Tình trạng' " +
                                "FROM  SACH JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach " +
                                "JOIN DAUSACH ON Sach.MaDauSach = DauSach.MaDauSach " +
                                "JOIN CT_TACGIA ON DAUSACH.MaDauSach = CT_TACGIA.MaDauSach " +
                                "JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia " +
                                "JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai WHERE TinhTrang = @TinhTrang";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                if(cbValue.Text == "Đã mượn")
                {
                    sqlCommand.Parameters.AddWithValue("@TinhTrang", true);
                }    
                else if(cbValue.Text == "Chưa mượn")
                {
                    sqlCommand.Parameters.AddWithValue("@TinhTrang", false);
                }    
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
            tblSoSachTimThay.Text = (dgvCuonSach.Items.Count).ToString();

        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            cbValue.Text = "";
            tblSoSachTimThay.Text = "";
            tblTongSoSach.Text = "";
            HienThiDanhSachSach();
        }

        private void cbKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbKey.SelectedIndex == 0)
            {
                cbValue.Items.Clear();
                string query = "SELECT MaCuonSach FROM CUONSACH"; // Thay thế YourTableName bằng tên bảng của bạn

                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string maDS = reader["MaCuonSach"].ToString();
                    cbValue.Items.Add(maDS);
                }
                sqlConnection.Close();
            }
            else if (cbKey.SelectedIndex == 1)
            {
                cbValue.Items.Clear();
                string query = "SELECT TenDauSach FROM DAUSACH"; // Thay thế YourTableName bằng tên bảng của bạn

                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string maDS = reader["TenDauSach"].ToString();
                    cbValue.Items.Add(maDS);
                }
                sqlConnection.Close();
            }
            else if (cbKey.SelectedIndex == 2)
            {
                cbValue.Items.Clear();
                string query = "SELECT * FROM THELOAI"; // Thay thế YourTableName bằng tên bảng của bạn

                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string maDS = reader["TenTheLoai"].ToString();
                    cbValue.Items.Add(maDS);
                }
                sqlConnection.Close();
            }
            else if (cbKey.SelectedIndex == 3)
            {
                cbValue.Items.Clear();
                string query = "SELECT * FROM TACGIA"; // Thay thế YourTableName bằng tên bảng của bạn

                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string maDS = reader["TenTacGia"].ToString();
                    cbValue.Items.Add(maDS);
                }
                sqlConnection.Close();
            }
            else if(cbKey.SelectedIndex == 4)
            {
                cbValue.Items.Clear();
                cbValue.Items.Add("Đã mượn");
                cbValue.Items.Add("Chưa mượn");
            }
        }
    }
}
