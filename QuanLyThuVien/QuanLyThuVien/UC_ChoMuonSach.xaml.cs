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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_ChoMuonSach.xaml
    /// </summary>
    public partial class UC_ChoMuonSach : UserControl
    {
        SqlConnection sqlConnection;
        private string maPhieuMuonTraSach = "PMTS000";
        public UC_ChoMuonSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            InitMaPhieuMuon();
            InitMaDocGia();
            InitPhieuMuon();
            HienThiDanhSachSach();
        }

        private void HienThiDanhSachSach()
        {
            sqlConnection.Open();
            string query = "SELECT MaCuonSach AS 'Mã cuốn sách', TenSach AS 'Tên sách', TinhTrang AS 'Tình trạng' FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void InitMaPhieuMuon()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaPhieuMuonTraSach FROM PHIEUMUONTRASACH ORDER BY MaPhieuMuonTraSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maPhieuMuonTraSach = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }
        private string IncreasePrimaryKey()
        {
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maPhieuMuonTraSach.Substring(4)); // Lấy phần số từ chuỗi hiện tại
            currentNumber++; // Tăng số phiếu
            //MessageBox.Show(currentNumber.ToString());
            maPhieuMuonTraSach = $"PMTS{currentNumber:D3}"; // Format lại chuỗi số phiếu

            // Hiển thị số phiếu lên giao diện (ví dụ: textBlockSoPhieu.Text = currentPhieuThu;)
            return maPhieuMuonTraSach;
        }
        private void InitPhieuMuon()
        {
            tblMaPhieuMuonTra.Text = IncreasePrimaryKey();
            dpNgayMuon.Text = DateTime.Now.ToString();
        }

        private void dgvSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaCuonSach.Text = row_selected.Row["Mã cuốn sách"].ToString();
                tblTenCuonSach.Text = row_selected.Row["Tên sách"].ToString();
            }
        }

        private void InitMaDocGia()
        {
            string query = "SELECT MaDocGia FROM DOCGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string maDS = reader["MaDocGia"].ToString();
                cbMaDocGia.Items.Add(maDS);
            }
            sqlConnection.Close();
        }

        private void btnChoMuon_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", tblMaPhieuMuonTra.Text);
            object phieuMuonTra = sqlCommand.ExecuteScalar();
            sqlConnection.Close();
            if (phieuMuonTra != null)
                MessageBox.Show("Đã tồn tại phiếu mượn trả, nhấn 'Thêm mới' để tạo phiếu khác");
            else if (cbMaDocGia.Text == "")
                MessageBox.Show("Mã độc giả không được để trống");
            else if (tblMaCuonSach.Text == "")
                MessageBox.Show("Mã cuốn sách không được để trống");
            else
            {
                try
                {
                    sqlConnection.Open();
                    
                    string maDocGia = cbMaDocGia.SelectedValue as string;

                    //Tính thẻ còn hạn hay không
                    query = "SELECT NgayHetHan FROM DOCGIA WHERE MaDocGia = @MaDocGia AND NgayHetHan < GETDATE()";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                    object hanThe = sqlCommand.ExecuteScalar();

                    //Tính sách hết hạn
                    query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaDocGia = @MaDocGia AND NgayPhaiTra < GETDATE() AND NgayTra IS NULL";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                    object sachHetHan = sqlCommand.ExecuteScalar();

                    //Tính số sách đang mượn
                    query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoNgayMuonToiDa'";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    string soNgayMuonToiDa = sqlCommand.ExecuteScalar().ToString();

                    query = "SELECT COUNT(*) FROM PHIEUMUONTRASACH WHERE MaDocGia = @MaDocGia AND GETDATE() - NgayMuon <= @SoNgayMuonToiDa AND NgayTra IS NULL";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                    sqlCommand.Parameters.AddWithValue("@SoNgayMuonToiDa", Int32.Parse(soNgayMuonToiDa));
                    string soSachDangMuon = sqlCommand.ExecuteScalar().ToString();

                    query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoSachMuonToiDa'";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    string soSachMuonToiDa = sqlCommand.ExecuteScalar().ToString();

                    //Kiểm tra sách có mượn hay không
                    sqlCommand = new SqlCommand("SELECT TinhTrang FROM CUONSACH WHERE MaCuonSach = @MaCuonSach", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                    string isBorrowed = sqlCommand.ExecuteScalar().ToString();




                    if (hanThe != null)
                        MessageBox.Show("Thẻ hết hạn");
                    else if (sachHetHan != null)
                        MessageBox.Show("Có sách hết hạn");
                    else if (Int32.Parse(soSachDangMuon) == Int32.Parse(soSachMuonToiDa))
                        MessageBox.Show("Đã đạt số sách mượn tối đa");
                    else if (isBorrowed == "True")
                        MessageBox.Show("Sách đã được mượn");
                    else
                    {
                        MessageBox.Show("Cho mượn thành công");
                        query = "INSERT INTO PHIEUMUONTRASACH (MaPhieuMuonTraSach, MaDocGia, NgayMuon, MaCuonSach, NgayPhaiTra) VALUES (@MaPhieuMuonTraSach, @MaDocGia, @NgayMuon, @MaCuonSach, @NgayPhaiTra)";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", tblMaPhieuMuonTra.Text);
                        sqlCommand.Parameters.AddWithValue("@MaDocGia", cbMaDocGia.Text);
                        sqlCommand.Parameters.AddWithValue("@NgayMuon", dpNgayMuon.Text);
                        sqlCommand.Parameters.AddWithValue("@NgayPhaiTra", (dpNgayMuon.SelectedDate ?? DateTime.Now).AddDays(Int32.Parse(soNgayMuonToiDa)).ToString());
                        sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                        sqlCommand.ExecuteScalar();

                       
                        query = "UPDATE CUONSACH SET TinhTrang = 1 WHERE MaCuonSach = @MaCuonSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaCuonSach", tblMaCuonSach.Text);
                        sqlCommand.ExecuteScalar();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachSach();
                }
            }
           
        }

        private void btnTimSach_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT MaCuonSach AS 'Mã cuốn sách', TenSach AS 'Tên sách', TinhTrang AS 'Tình trạng' FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach WHERE TenSach = @TenSach";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

            query = "SELECT COUNT(*) FROM  SACH INNER JOIN CUONSACH ON SACH.MaSach = CUONSACH.MaSach WHERE TenSach = @TenSach";
            sqlConnection.Open();
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@TenSach", txbTenSach.Text);
            tblSoSachTimThay.Text = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();

        }

        private void cbMaDocGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMaDocGia.SelectedIndex != -1)
            {
                sqlConnection.Open();
                string maDocGia = cbMaDocGia.SelectedValue as string;
                string query = "SELECT HoVaTen FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDocGia", maDocGia);
                tblTenDocGia.Text = sqlCommand.ExecuteScalar().ToString();              
                sqlConnection.Close();
                
            }    
            
            
        }

        private void btnInPhieu_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(gr, "My Control Print");
            }
        }

        private void dpNgayMuon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpNgayMuon.SelectedDate != null)
            {
                sqlConnection.Open();
                string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoNgayMuonToiDa'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                string soNgayMuonToiDa = sqlCommand.ExecuteScalar().ToString();
                sqlConnection.Close();
                tblNgayPhaiTra.Text = (dpNgayMuon.SelectedDate ?? DateTime.Now).AddDays(Int32.Parse(soNgayMuonToiDa)).ToString("MM/dd/yyyy");
            }
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            InitPhieuMuon();
        }
    }
}
