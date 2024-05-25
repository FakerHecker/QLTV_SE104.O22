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
using System.Windows.Shapes;
using System.Configuration;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_QuanLyChiTietPhieuNhapSach.xaml
    /// </summary>
    public partial class W_QuanLyChiTietPhieuNhapSach : Window
    {
        SqlConnection sqlConnection;
        public W_QuanLyChiTietPhieuNhapSach()
        {
            InitializeComponent();

            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            HienThiChiTietPhieuNhapSach();
        }

        private void HienThiChiTietPhieuNhapSach()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM CT_PHIEUNHAPSACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvChiTietPhieuNhap.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvChiTietPhieuNhap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txbMaCTPhieuNhap.Text = row_selected.Row["MaCTPhieuNhapSach"].ToString();
                txbMaPhieuNhapSach.Text = row_selected.Row["MaPhieuNhapSach"].ToString();
                txbMaSach.Text = row_selected.Row["MaSach"].ToString();
                txbSoLuong.Text = row_selected.Row["SoLuong"].ToString();
                txbDonGia.Text = row_selected.Row["DonGia"].ToString();
            }
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            txbMaCTPhieuNhap.Text = "";
            txbMaPhieuNhapSach.Text = "";
            txbMaSach.Text = "";
            txbSoLuong.Text = "";
            txbDonGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO CT_PHIEUNHAPSACH (MaCTPhieuNhapSach, MaPhieuNhapSach, MaSach, SoLuong, DonGia) VALUES (@MaCTPhieuNhapSach, @MaPhieuNhapSach, @MaSach, @SoLuong, @DonGia)";
                sqlConnection.Open();

                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", txbMaCTPhieuNhap.Text);
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", txbMaPhieuNhapSach.Text);
                sqlCommand.Parameters.AddWithValue("@MaSach", txbMaSach.Text);
                sqlCommand.Parameters.AddWithValue("@SoLuong", txbSoLuong.Text);
                sqlCommand.Parameters.AddWithValue("@DonGia", txbDonGia.Text);

                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiChiTietPhieuNhapSach();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE CT_PHIEUNHAPSACH SET SoLuong = @SoLuong, DonGia = @DonGia WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@SoLuong", txbSoLuong.Text);
                sqlCommand.Parameters.AddWithValue("@DonGia", txbDonGia.Text);
                sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", txbMaCTPhieuNhap.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiChiTietPhieuNhapSach();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM CT_PHIEUNHAPSACH WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", txbMaCTPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiChiTietPhieuNhapSach();
                }
            }                    
        }
    }
}
