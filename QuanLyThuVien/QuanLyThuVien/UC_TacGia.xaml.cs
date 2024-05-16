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
    /// Interaction logic for UC_TacGia.xaml
    /// </summary>
    public partial class UC_TacGia : UserControl
    {
        SqlConnection sqlConnection;
        private string maTacGia = "TG000";
        public UC_TacGia()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            InitMaTacGia();
            HienThiDanhSachTacGia();
        }

        private void InitMaTacGia()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaTacGia FROM TACGIA ORDER BY MaTacGia DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maTacGia = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private void HienThiDanhSachTacGia()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM TACGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvTacGia.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvTacGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaTacGia.Text = row_selected.Row["MaTacGia"].ToString();
                txbTenTacGia.Text = row_selected.Row["TenTacGia"].ToString();
            }
        }

        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maTacGia.Substring(2)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maTacGia = $"TG{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maTacGia;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaTacGia.Text = IncreasePrimaryKey();
            txbTenTacGia.Text = "";
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO TACGIA (MaTacGia, TenTacGia) VALUES (@MaTacGia, @TenTacGia)";
                sqlConnection.Open();

                MessageBox.Show("Thêm thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachTacGia();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE TACGIA SET TenTacGia = @TenTacGia  WHERE MaTacGia = @MaTacGia";
                sqlConnection.Open();

                MessageBox.Show("Cập nhật thành công");
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                sqlCommand.Parameters.AddWithValue("@TenTacGia", txbTenTacGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachTacGia();
            }
        }
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM TACGIA WHERE MaTacGia = @MaTacGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaTacGia", tblMaTacGia.Text);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachTacGia();
            }

        }

    }
}
