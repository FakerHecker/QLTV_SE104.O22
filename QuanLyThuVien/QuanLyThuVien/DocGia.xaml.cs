using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for DocGia.xaml
    /// </summary>
    public partial class DocGia : UserControl
    {
        SqlConnection sqlConnection;
        public DocGia()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
           
            HienThiDanhSachDocGia();
        }

        private void HienThiDanhSachDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM DOCGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }
        private string IncreasePrimaryKey()
        {
            string temp;
            string query = "SELECT COUNT(MaDocGia) FROM DOCGIA";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() == null)
            {
                temp = sqlCommand.ExecuteScalar().ToString();
                temp = "DG001";

            }
            else
            {
                temp = (Int32.Parse(sqlCommand.ExecuteScalar().ToString()) + 1 ).ToString();
                if (temp.Length == 1)
                    temp = "DG00" + temp;
                else if (temp.Length == 2)
                    temp = "DG0" + temp;
                else
                    temp = "DG" + temp;
            }
            sqlConnection.Close();
            return temp;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            txtMaDocGia.Text = IncreasePrimaryKey();
            txtHoTen.Text = "";
            cbLoaiDG.SelectedIndex = 0;
            dtNgaySinh.SelectedDate = new DateTime(2000, 1, 1);
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            dtNgayLapThe.SelectedDate = DateTime.Now;
            DateTime date = DateTime.Now.AddDays(180);
            txtNgayHetHan.Text = date.ToString();
            txtTongNo.Text = "0";
            IncreasePrimaryKey();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaDocGia", txtMaDocGia.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDocGia();
            }

        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO DOCGIA (MaDocGia, HoVaTen, MaLoaiDocGia, NgaySinh, DiaChi, Email, NgayLapThe, NgayHetHan, TongNo) VALUES (@MaDocGia, @HoVaTen, @MaLoaiDocGia, @NgaySinh, @DiaChi, @Email, @NgayLapThe, @NgayHetHan, @TongNo)";
                sqlConnection.Open();
                int age;
                DateTime start = dtNgaySinh.SelectedDate.Value.Date;
                DateTime finish = dtNgayLapThe.SelectedDate.Value.Date;
                TimeSpan difference = finish - start;
                age = (int)difference.TotalDays / 365;

                SqlCommand sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiThieu'", sqlConnection);
                int minAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiDa'", sqlConnection);
                int maxAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                if (age > minAge && age < maxAge)
                {
                    MessageBox.Show("Thêm thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", txtMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", cbLoaiDG.Text);
                    sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                    sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayLapThe", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayHetHan", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@TongNo", 0);
                    sqlCommand.ExecuteScalar();
                }

                else
                    MessageBox.Show("Tuổi không hợp lệ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDocGia();
            }
        }

        private void dtNgayLapThe_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtNgayLapThe.SelectedDate.HasValue)
            {
                int thoiHanThe;
                string query = "SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'ThoiHanThe'";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                thoiHanThe = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
                sqlConnection.Close();
                DateTime selectedDate = dtNgayLapThe.SelectedDate.Value;
                DateTime newDate = selectedDate.AddMonths(thoiHanThe);
                txtNgayHetHan.Text = newDate.ToString("MM/dd/yyyy");
            }
            
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE DOCGIA SET HoVaTen = @HoVaTen, MaLoaiDocGia = @MaLoaiDocGia, NgaySinh = @NgaySinh, DiaChi = @DiaChi, Email = @Email, NgayLapThe = @NgayLapThe, NgayHetHan = @NgayHetHan, TongNo = @TongNo WHERE MaDocGia = @MaDocGia";
                
                sqlConnection.Open();
                int age;
                DateTime start = dtNgaySinh.SelectedDate.Value.Date;
                DateTime finish = dtNgayLapThe.SelectedDate.Value.Date;
                TimeSpan difference = finish - start;
                age = (int)difference.TotalDays / 365;

                SqlCommand sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiThieu'", sqlConnection);
                int minAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiDa'", sqlConnection);
                int maxAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                if (age > minAge && age < maxAge)
                {
                    MessageBox.Show("Cập nhật thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", txtMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", cbLoaiDG.Text);
                    sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                    sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayLapThe", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayHetHan", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@TongNo", 0);
                    sqlCommand.ExecuteScalar();
                }

                else
                    MessageBox.Show("Tuổi không hợp lệ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDocGia();
            }
        }

        private void dataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txtMaDocGia.Text = row_selected.Row["MaDocGia"].ToString();
                txtHoTen.Text = row_selected.Row["HoVaTen"].ToString();
                cbLoaiDG.Text = row_selected.Row["MaLoaiDocGia"].ToString();
                dtNgaySinh.SelectedDate = Convert.ToDateTime(row_selected.Row["NgaySinh"].ToString());
                txtDiaChi.Text = row_selected.Row["DiaChi"].ToString();
                txtEmail.Text = row_selected.Row["Email"].ToString();
                dtNgayLapThe.SelectedDate = Convert.ToDateTime(row_selected.Row["NgayLapThe"].ToString());
                txtNgayHetHan.Text = row_selected.Row["NgayHetHan"].ToString();
                txtTongNo.Text = row_selected.Row["TongNo"].ToString();
            }
        }


    }
}
