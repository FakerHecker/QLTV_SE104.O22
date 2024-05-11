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
    /// Interaction logic for W_QuanLyLoaiDocGia.xaml
    /// </summary>
    public partial class W_QuanLyLoaiDocGia : Window
    {
        SqlConnection sqlConnection;
        public W_QuanLyLoaiDocGia()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachLoaiDocGia();
        }

        private void HienThiDanhSachLoaiDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM LOAIDOCGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvLoaiDocGia.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvLoaiDocGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txbMaLoaiDocGia.Text = row_selected.Row["MaLoaiDocGia"].ToString();
                txbTenLoaiDocGia.Text = row_selected.Row["TenLoaiDocGia"].ToString();
            }
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            txbMaLoaiDocGia.Text = "";
            txbTenLoaiDocGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO LOAIDOCGIA (MaLoaiDocGia, TenLoaiDocGia) VALUES (@MaLoaiDocGia, @TenLoaiDocGia)";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", txbMaLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();
                MessageBox.Show("Thêm thành công");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE LOAIDOCGIA SET TenLoaiDocGia = @TenLoaiDocGia WHERE MaLoaiDocGia = @MaLoaiDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", txbMaLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM LOAIDOCGIA WHERE MaLoaiDocGia = @MaLoaiDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", txbMaLoaiDocGia.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachLoaiDocGia();
            }
        }
    }
}
