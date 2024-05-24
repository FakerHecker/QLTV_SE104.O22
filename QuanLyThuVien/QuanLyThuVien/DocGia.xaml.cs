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
        string maDocGia = "DG000";
        public DocGia()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True"; sqlConnection = new SqlConnection(connectionString);
            InitMaDocGia();
            InitLoaiDocGia();
            HienThiDanhSachDocGia();

        }

        private void InitMaDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaDocGia FROM DOCGIA ORDER BY MaDocGia DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maDocGia = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private void InitLoaiDocGia()
        {
            cbLoaiDG.Items.Clear();
            string query = "SELECT * FROM LOAIDOCGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["TenLoaiDocGia"].ToString();
                cbLoaiDG.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void HienThiDanhSachDocGia()
        {
            
            sqlConnection.Open();
            string query = "SELECT MaDocGia AS 'Mã độc giả', HoVaTen AS 'Họ và tên', TenLoaiDocGia AS 'Loại độc giả', NgaySinh AS 'Ngày sinh', DiaChi AS 'Địa chỉ', Email, NgayLapThe AS 'Ngày lập thẻ', NgayHetHan AS 'Ngày hết hạn', TongNo AS 'Tổng nợ'  FROM DOCGIA JOIN LOAIDOCGIA ON DOCGIA.MaLoaiDocGia = LOAIDOCGIA.MaLoaiDocGia";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
            
           
        }
        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maDocGia.Substring(2)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maDocGia = $"DG{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maDocGia;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            tblMaDocGia.Text = IncreasePrimaryKey();
            txtHoTen.Text = "";

            InitLoaiDocGia();
            cbLoaiDG.SelectedIndex = 0;

            dtNgaySinh.SelectedDate = new DateTime(2000, 1, 1);
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            dtNgayLapThe.SelectedDate = DateTime.Now;

            int thoiHanThe;
            string query = "SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'ThoiHanThe'";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            thoiHanThe = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
            sqlConnection.Close();
            DateTime selectedDate = dtNgayLapThe.SelectedDate.Value;
            DateTime newDate = selectedDate.AddMonths(thoiHanThe);
            tblNgayHetHan.Text = newDate.ToString("MM/dd/yyyy");

            txtTongNo.Text = "0";

        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tìm thấy độc giả");
                else
                {
                    query = "DELETE FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                    sqlCommand.ExecuteScalar();

                    tblMaDocGia.Text = "";
                    txtHoTen.Text = "";
                    cbLoaiDG.SelectedIndex = -1;
                    dtNgaySinh.Text = "";
                    txtDiaChi.Text = "";
                    txtEmail.Text = "";
                    dtNgayLapThe.Text = "";
                    tblNgayHetHan.Text = "";
                    txtTongNo.Text = "";
                } 
                    
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Không thể xóa, độc giả đang được sử dụng");
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
                int age = 0;
                if (dtNgaySinh.SelectedDate != null && dtNgayLapThe.SelectedDate != null)
                {
                    DateTime start = dtNgaySinh.SelectedDate.Value.Date;
                    DateTime finish = dtNgayLapThe.SelectedDate.Value.Date;
                    TimeSpan difference = finish - start;
                    age = (int)difference.TotalDays / 365;
                }    
                

                SqlCommand sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiThieu'", sqlConnection);
                int minAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiDa'", sqlConnection);
                int maxAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());


                sqlCommand = new SqlCommand("SELECT * FROM DOCGIA WHERE MaDocGia = @MaDocGia", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                object tonTaiDocGia = sqlCommand.ExecuteScalar();

                //kiểm tra trùng
                sqlCommand = new SqlCommand("SELECT * FROM DOCGIA WHERE HoVaTen = @TenDocGia AND NgaySinh = @NgaySinh AND DiaChi = @DiaChi AND Email = @Email", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenDocGia", txtHoTen.Text);
                sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                object trungDocGia = sqlCommand.ExecuteScalar();

                if (tblMaDocGia.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (txtHoTen.Text == "")
                    MessageBox.Show("Tên độc giả không được để trống");
                else if (txtDiaChi.Text == "")
                    MessageBox.Show("Địa chỉ không được bỏ trống");
                else if (txtEmail.Text == "")
                    MessageBox.Show("Email không được bỏ trống");
                else if (tonTaiDocGia != null || trungDocGia != null)
                    MessageBox.Show("Đã tồn tại độc giả");
                else if (age > minAge && age < maxAge)
                {

                    string maLoaiDG;
                    sqlCommand = new SqlCommand("SELECT MaLoaiDocGia FROM LOAIDOCGIA WHERE TenLoaiDocGia = @TenLoaiDocGia", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", cbLoaiDG.Text);
                    maLoaiDG = sqlCommand.ExecuteScalar().ToString();

                    MessageBox.Show("Thêm thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", maLoaiDG);
                    sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                    sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayLapThe", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayHetHan", tblNgayHetHan.Text);
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
                tblNgayHetHan.Text = newDate.ToString("MM/dd/yyyy");
            }

        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE DOCGIA SET HoVaTen = @HoVaTen, MaLoaiDocGia = @MaLoaiDocGia, NgaySinh = @NgaySinh, DiaChi = @DiaChi, Email = @Email, NgayLapThe = @NgayLapThe, NgayHetHan = @NgayHetHan, TongNo = @TongNo WHERE MaDocGia = @MaDocGia";

                sqlConnection.Open();
                int age = 0;
                if (dtNgaySinh.SelectedDate != null && dtNgayLapThe.SelectedDate != null)
                {
                    DateTime start = dtNgaySinh.SelectedDate.Value.Date;
                    DateTime finish = dtNgayLapThe.SelectedDate.Value.Date;
                    TimeSpan difference = finish - start;
                    age = (int)difference.TotalDays / 365;
                }

                SqlCommand sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiThieu'", sqlConnection);
                int minAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                sqlCommand = new SqlCommand("SELECT GIATRI FROM THAMSO WHERE TenThamSo = 'TuoiToiDa'", sqlConnection);
                int maxAge = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                sqlCommand = new SqlCommand("SELECT * FROM DOCGIA WHERE MaDocGia = @MaDocGia", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                object khongThayDocGIa = sqlCommand.ExecuteScalar(); ;

                sqlCommand = new SqlCommand("SELECT * FROM DOCGIA WHERE HoVaTen = @TenDocGia AND NgaySinh = @NgaySinh AND DiaChi = @DiaChi AND Email = @Email", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenDocGia", txtHoTen.Text);
                sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                object trungDocGia = sqlCommand.ExecuteScalar();


                if (txtHoTen.Text == "")
                    MessageBox.Show("Tên độc giả không được để trống");
                else if (txtDiaChi.Text == "")
                    MessageBox.Show("Địa chỉ không được bỏ trống");
                else if (txtEmail.Text == "")
                    MessageBox.Show("Email không được bỏ trống");
                else if (khongThayDocGIa == null)
                    MessageBox.Show("Không tìm thấy độc giả");
                else if (trungDocGia != null)
                    MessageBox.Show("Trùng thông tin độc giả");
                else if (age > minAge && age < maxAge)
                {

                    string maLoaiDG;
                    sqlCommand = new SqlCommand("SELECT MaLoaiDocGia FROM LOAIDOCGIA WHERE TenLoaiDocGia = @TenLoaiDocGia", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenLoaiDocGia", cbLoaiDG.Text);
                    maLoaiDG = sqlCommand.ExecuteScalar().ToString();

                    MessageBox.Show("Cập nhật thành công");
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", tblMaDocGia.Text);
                    sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                    sqlCommand.Parameters.AddWithValue("@MaLoaiDocGia", maLoaiDG);
                    sqlCommand.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Text);
                    sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                    sqlCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayLapThe", dtNgayLapThe.Text);
                    sqlCommand.Parameters.AddWithValue("@NgayHetHan", tblNgayHetHan.Text);
                    sqlCommand.Parameters.AddWithValue("@TongNo", txtTongNo.Text);
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
                tblMaDocGia.Text = row_selected.Row["Mã độc giả"].ToString();
                txtHoTen.Text = row_selected.Row["Họ và tên"].ToString();
                cbLoaiDG.Text = row_selected.Row["Loại độc giả"].ToString();
                dtNgaySinh.SelectedDate = Convert.ToDateTime(row_selected.Row["Ngày sinh"].ToString());
                txtDiaChi.Text = row_selected.Row["Địa chỉ"].ToString();
                txtEmail.Text = row_selected.Row["Email"].ToString();
                dtNgayLapThe.SelectedDate = Convert.ToDateTime(row_selected.Row["Ngày lập thẻ"].ToString());
                tblNgayHetHan.Text = ((DateTime)(row_selected.Row["Ngày hết hạn"])).ToString("MM/dd/yyyy");
                txtTongNo.Text = row_selected.Row["Tổng nợ"].ToString();
            }
        }
    }
}
