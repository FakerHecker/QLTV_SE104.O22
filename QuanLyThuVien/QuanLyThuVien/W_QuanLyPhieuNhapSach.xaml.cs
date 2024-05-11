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
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_QuanLyPhieuNhapSach.xaml
    /// </summary>
    public partial class W_QuanLyPhieuNhapSach : Window
    {
        SqlConnection sqlConnection;
        public W_QuanLyPhieuNhapSach()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachPhieuNhapSach();
        }

        private void HienThiDanhSachPhieuNhapSach()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM PHIEUNHAPSACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPhieuNhapSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            txbMaPhieuNhapSach.Text = "";
            dtpNgayNhap.SelectedDate = DateTime.Now;
            txbTongTien.Text = "0";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO PHIEUNHAPSACH (MaPhieuNhapSach, NgayNhap, TongTien) VALUES (@MaPhieuNhapSach, @NgayNhap, @TongTien)";
                sqlConnection.Open();
                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", txbMaPhieuNhapSach.Text);
                sqlCommand.Parameters.AddWithValue("@NgayNhap", dtpNgayNhap.Text);
                sqlCommand.Parameters.AddWithValue("@TongTien", txbTongTien.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachPhieuNhapSach();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE PHIEUNHAPSACH SET NgayNhap = @NgayNhap, TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@NgayNhap", dtpNgayNhap.Text);
                sqlCommand.Parameters.AddWithValue("@TongTien", txbTongTien.Text);
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", txbMaPhieuNhapSach.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachPhieuNhapSach();
            }
        }

        private void dgvPhieuNhapSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txbMaPhieuNhapSach.Text = row_selected.Row["MaPhieuNhapSach"].ToString();
                dtpNgayNhap.SelectedDate = Convert.ToDateTime(row_selected.Row["NgayNhap"].ToString());
                txbTongTien.Text = row_selected.Row["TongTien"].ToString();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", txbMaPhieuNhapSach.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachPhieuNhapSach();
            }
        }
    }
}
