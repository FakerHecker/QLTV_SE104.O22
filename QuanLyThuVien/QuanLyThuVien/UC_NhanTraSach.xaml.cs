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
    /// Interaction logic for UC_NhanTraSach.xaml
    /// </summary>
    public partial class UC_NhanTraSach : UserControl
    {
        SqlConnection sqlConnection;
        public UC_NhanTraSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachPhieuMuon();
        }

        private void HienThiDanhSachPhieuMuon()
        {
            sqlConnection.Open();
            string query = "SELECT MaPhieuMuonTraSach AS 'Mã phiếu mượn trả', MaDocGia AS 'Mã độc giả', NgayMuon AS 'Ngày mượn', MaCuonSach AS 'Mã cuốn sách', NgayPhaiTra AS 'Ngày phải trả', NgayTra AS 'Ngày trả', TienPhatKyNay AS 'Tiền phạt kỳ này', SoTienTra AS 'Số tiền trả', ConLai AS 'Còn lại' FROM  PHIEUMUONTRASACH";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvPhieuMuon.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dpNgayTra_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime firstDateTime = dpNgayMuon.SelectedDate ?? DateTime.Now;
            DateTime secondDateTime =  dpNgayTra.SelectedDate ?? DateTime.Now;

            TimeSpan difference = secondDateTime.Subtract(firstDateTime);

            sqlConnection.Open();
            string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoNgayMuonToiDa'";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int soNgayMuonToiDa = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoTienPhatMoiNgay'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            int soTienPhatTrenNgay = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
            sqlConnection.Close();

            int khoangCachNgayMuonTra = Int32.Parse(difference.Days.ToString());
            if (khoangCachNgayMuonTra < 0)
            {
                MessageBox.Show("Ngày trả không được nhỏ hơn ngày mượn");
                dpNgayTra.Text = DateTime.Now.ToString();
            }
            else
            {
                int tienPhatKynay = 0;
                if (khoangCachNgayMuonTra > soNgayMuonToiDa)
                    tienPhatKynay = (khoangCachNgayMuonTra - soNgayMuonToiDa) * soTienPhatTrenNgay;
                tblTienPhatKyNay.Text = tienPhatKynay.ToString();
            } 
                
          
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Xử lý sự kiện khi người dùng nhấn Enter
                //MessageBox.Show("Enter đã được nhấn!");
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Thêm giá trị vào TextBox (ví dụ: txbTenCot.Text = tenCotValue;)
                        tblHoVaTen.Text = reader["MaDocGia"].ToString();
                        dpNgayMuon.Text = reader["NgayMuon"].ToString();
                        dpNgayPhaiTra.Text = reader["NgayPhaiTra"].ToString();
                        dpNgayTra.Text = DateTime.Now.ToString();
                    }
                }

                sqlConnection.Close();

            }
        }


        private void btnInPhieuTra_Click(object sender, RoutedEventArgs e)
        {
            if (txbMaPhieuMuon.Text == "")
                MessageBox.Show("Mã phiếu mượn trả không được để trống");
            else
            {
                try
                {

                    sqlConnection.Open();
                    string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                    object IsPhieuMuonExisted = sqlCommand.ExecuteScalar();

                    query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach AND NgayTra IS NOT NULL";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                    object IsReturned = sqlCommand.ExecuteScalar();


                    if (IsPhieuMuonExisted == null)
                        MessageBox.Show("Không tồn tại phiếu mượn");
                    else if (IsReturned != null)
                        MessageBox.Show("Phiếu mượn đã được trả");
                    else if (!float.TryParse(txbSoTienTra.Text, out float soTienTra) || soTienTra < 0)
                        MessageBox.Show("Số tiền trả không hợp lệ");
                    else if (double.TryParse(tblConlai.Text, out double conLai) && conLai < 0)
                        MessageBox.Show("Số tiền trả không được lớn hơn số tiền nợ");
                    else
                    {
                        //cập nhật phiếu mượn trả sách
                        query = "UPDATE PHIEUMUONTRASACH SET NgayTra = @NgayTra, TienPhatKyNay = @TienPhatKyNay, SoTienTra = @SoTienTra, ConLai = @ConLai WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                        sqlCommand.Parameters.AddWithValue("@NgayTra", dpNgayTra.Text);
                        sqlCommand.Parameters.AddWithValue("@TienPhatKyNay", tblTienPhatKyNay.Text);
                        sqlCommand.Parameters.AddWithValue("@SoTienTra", txbSoTienTra.Text);
                        sqlCommand.Parameters.AddWithValue("@ConLai", tblConlai.Text);
                        sqlCommand.ExecuteScalar();
                        MessageBox.Show("Trả sách thành công");

                        //tính lại tổng nợ cho độc giả
                        query = "SELECT TongNo FROM DOCGIA WHERE MaDocGia = @MaDocGia";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaDocGia", tblHoVaTen.Text);
                        string tongNo = sqlCommand.ExecuteScalar().ToString();

                        double newTongNo = double.Parse(tongNo) + double.Parse(tblConlai.Text);
                        query = "UPDATE DOCGIA SET TongNo = @TongNo WHERE MaDocGia = @MaDocGia";
                        sqlCommand = new SqlCommand( query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaDocGia", tblHoVaTen.Text);
                        sqlCommand.Parameters.AddWithValue("@TongNo", newTongNo.ToString());
                        sqlCommand.ExecuteScalar();

                        //cập nhật tình trạng cuốn sách thành chưa mượn
                        query = "SELECT MaCuonSach FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                        string maCuonSach = sqlCommand.ExecuteScalar().ToString();

                        query = "UPDATE CUONSACH SET TinhTrang = 0 WHERE MaCuonSach = @MaCuonSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
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
                    HienThiDanhSachPhieuMuon();
                }
            }    
           
        }

        private void txbMaPhieuMuon_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbMaPhieuMuon.Text.Length == 7)
            {
                sqlConnection.Open();
                string query = "SELECT * FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", txbMaPhieuMuon.Text);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Thêm giá trị vào TextBox (ví dụ: txbTenCot.Text = tenCotValue;)
                        tblHoVaTen.Text = reader["MaDocGia"].ToString();
                        tblMaSach.Text = reader["MaCuonSach"].ToString();
                        dpNgayMuon.Text = reader["NgayMuon"].ToString();
                        dpNgayPhaiTra.Text = reader["NgayPhaiTra"].ToString();
                        dpNgayTra.Text = DateTime.Now.ToString();
                    }
                }

                sqlConnection.Close();
            }    
        }

        private void txbSoTienTra_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txbSoTienTra.Text, out float soTienTra) && float.TryParse(tblTienPhatKyNay.Text, out float tienPhatKyNay))
            {
                float conLai = tienPhatKyNay- soTienTra;
                tblConlai.Text = conLai.ToString();
            }       
        }

        private void dgvPhieuMuon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                if (row_selected.Row["Ngày trả"].ToString() == "")
                {
                    txbMaPhieuMuon.Text = row_selected.Row["Mã phiếu mượn trả"].ToString();
                    tblHoVaTen.Text = row_selected.Row["Mã độc giả"].ToString();
                    tblMaSach.Text = row_selected.Row["Mã cuốn sách"].ToString();
                    dpNgayMuon.Text = row_selected.Row["Ngày mượn"].ToString();
                    dpNgayPhaiTra.Text = ((DateTime)row_selected.Row["Ngày phải trả"]).ToString("MM/dd/yyyy");
                    dpNgayTra.Text = DateTime.Now.ToString();
                    txbSoTienTra.Text = "";
                    tblConlai.Text = "";
                }    
               else
                {
                    txbMaPhieuMuon.Text = row_selected.Row["Mã phiếu mượn trả"].ToString();
                    tblHoVaTen.Text = row_selected.Row["Mã độc giả"].ToString();
                    tblMaSach.Text = row_selected.Row["Mã cuốn sách"].ToString();
                    dpNgayMuon.Text = row_selected.Row["Ngày mượn"].ToString();
                    dpNgayPhaiTra.Text = ((DateTime)row_selected.Row["Ngày phải trả"]).ToString("MM/dd/yyyy");
                    dpNgayTra.Text = row_selected.Row["Ngày trả"].ToString();
                    txbSoTienTra.Text = row_selected.Row["Số tiền trả"].ToString();
                    tblConlai.Text = row_selected.Row["Còn lại"].ToString();
                }    
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DataRowView selectedItem = dgvPhieuMuon.SelectedItem as DataRowView;
                if (selectedItem != null)
                {
                    string maPhieu = selectedItem.Row["Mã phiếu mượn trả"].ToString();
                    //MessageBox.Show(maPhieu);

                    string ngayTra = selectedItem.Row["Ngày trả"].ToString();
                    if (ngayTra == "")
                    {
                        MessageBox.Show("Không thể xóa vì chưa trả sách");
                    }
                    else
                    {
                        try
                        {
                            sqlConnection.Open();
                            string query = "DELETE FROM PHIEUMUONTRASACH WHERE MaPhieuMuonTraSach = @MaPhieuMuonTraSach";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaPhieuMuonTraSach", maPhieu);
                            sqlCommand.ExecuteScalar();
                            sqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            HienThiDanhSachPhieuMuon();
                        }
                    }
                }
            }            
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(gr, "My Control Print");
            }
        }
    }
}
