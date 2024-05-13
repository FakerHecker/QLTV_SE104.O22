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
    /// Interaction logic for UC_LoaiDocGia.xaml
    /// </summary>
    public partial class UC_LoaiDocGia : UserControl
    {
        SqlConnection sqlConnection;
        public UC_LoaiDocGia()
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
                tblMaLoaiDocGia.Text = row_selected.Row["MaLoaiDocGia"].ToString();
                txbTenLoaiDocGia.Text = row_selected.Row["TenLoaiDocGia"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            string temp;
            string query = "SELECT COUNT(MaLoaiDocGia) FROM LOAIDOCGIA";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() == null)
            {
                temp = sqlCommand.ExecuteScalar().ToString();
                temp = "LDG001";

            }
            else
            {
                temp = (Int32.Parse(sqlCommand.ExecuteScalar().ToString()) + 1).ToString();
                if (temp.Length == 1)
                    temp = "LDG00" + temp;
                else if (temp.Length == 2)
                    temp = "LDG0" + temp;
                else
                    temp = "LDG" + temp;
            }
            sqlConnection.Close();
            return temp;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaLoaiDocGia.Text = IncreasePrimaryKey();
            txbTenLoaiDocGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO LOAIDOCGIA (MaLoaiDocGia, TenLoaiDocGia) VALUES (@MaLoaiDocGia, @TenLoaiDocGia)";
                sqlConnection.Open();
               
                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
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

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE LOAIDOCGIA SET TenLoaiDocGia = @TenLoaiDocGia  WHERE MaLoaiDocGia = @MaLoaiDocGia";
                sqlConnection.Open();

                MessageBox.Show("Cập nhật thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", txbTenLoaiDocGia.Text);
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
                sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", tblMaLoaiDocGia.Text);
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
