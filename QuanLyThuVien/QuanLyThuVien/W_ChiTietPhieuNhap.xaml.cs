using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_ChiTietPhieuNhap.xaml
    /// </summary>
    public partial class W_ChiTietPhieuNhap : Window
    {
        SqlConnection sqlConnection;
        string maPNS;
        private string maCTPN = "CTPNS000";

        
        public W_ChiTietPhieuNhap(string ma)
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True";
            sqlConnection = new SqlConnection(connectionString);
            tblMaPhieuNhap.Text = ma;
            maPNS = ma;
            InitMaPhieuNhap();
            InitTenSach();
            HienThiDanhSachChiTietPhieuNhap(); 
        }

        

        private void HienThiDanhSachChiTietPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT MaCTPhieuNhapSach AS 'Mã chi tiết phiếu nhập sách', MaPhieuNhapSach AS 'Mã phiếu nhập sách', TenDauSach AS 'Tên sách', SoLuong AS 'Số lượng', DonGia AS 'Đơn giá' " +
                "FROM CT_PHIEUNHAPSACH JOIN SACH ON CT_PHIEUNHAPSACH.MaSach = SACH.MaSach JOIN DAUSACH ON DAUSACH.MaDauSach = SACH.MaDauSach WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", maPNS);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvChiTietPhieuNhap.ItemsSource = dt.DefaultView;
            tblTongTien.Text = CalculateTongTien();
            sqlConnection.Close();
        }
        private void InitMaPhieuNhap()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaCTPhieuNhapSach FROM CT_PHIEUNHAPSACH ORDER BY MaCTPhieuNhapSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maCTPN = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private string IncreasePrimaryKey()
        {
            
            // Tăng giá trị của biến số phiếu
            int currentNumber = int.Parse(maCTPN.Substring(5));
            currentNumber++;
            maCTPN = $"CTPNS{currentNumber:D3}";
            return maCTPN;
        }

        private void InitMaSach()
        {
            string query = "SELECT MaSach FROM SACH"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["MaSach"].ToString();
                cbTenSach.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaCTPhieuNhap.Text = IncreasePrimaryKey();
            txbSoLuong.Text = "";
            txbDonGia.Text = "";
           
        }

        private string CalculateTongTien()
        {
            string query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);           
            return sqlCommand.ExecuteScalar().ToString();
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM CT_PHIEUNHAPSACH WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);

                if (tblMaPhieuNhap.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (cbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else if (sqlCommand.ExecuteScalar() != null)
                    MessageBox.Show("Đã tồn tại chi tiết phiếu nhập sách");
                else if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                {
                    // lấy mã sách
                    query = "SELECT MaSach FROM SACH JOIN DAUSACH ON SACH.MaDauSach = DAUSACH.MaDauSach WHERE TenDauSach = @TenSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenSach", cbTenSach.Text);
                    string maSach = sqlCommand.ExecuteScalar().ToString();

                    MessageBox.Show("Thêm thành công");
                    query = "INSERT INTO CT_PHIEUNHAPSACH (MaCTPhieuNhapSach, MaPhieuNhapSach, MaSach, SoLuong, DonGia) VALUES (@MaCTPhieuNhapSach, @MaPhieuNhapSach, @MaSach, @SoLuong, @DonGia)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
                    sqlCommand.Parameters.AddWithValue("@SoLuong", soLuong.ToString());
                    sqlCommand.Parameters.AddWithValue("@DonGia", donGia.ToString());
                    sqlCommand.ExecuteScalar();

                    float TongTien = 0;
                    query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    TongTien = float.Parse(sqlCommand.ExecuteScalar().ToString());

                    query = "UPDATE PHIEUNHAPSACH SET TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TongTien", TongTien.ToString());
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();     
                    
                    for (int i = 0; i < soLuong; i++)
                    {
                        //tạo mã sách
                        query = "SELECT TOP 1 MaCuonSach FROM CUONSACH ORDER BY MaCuonSach DESC";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        object maCS = sqlCommand.ExecuteScalar();
                        string maCuonSach = "CS000";
                        if (maCS != null)
                            maCuonSach = maCS.ToString();

                        int currentNumber = int.Parse(maCuonSach.Substring(2));
                        currentNumber++;
                        maCuonSach = $"CS{currentNumber:D3}";

                        //nhập sách
                        query = "INSERT INTO CUONSACH (MaCuonSach, MaSach, TinhTrang) VALUES (@MaCuonSach, @MaSach, 0)";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
                        sqlCommand.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
                        sqlCommand.ExecuteScalar();

                    }    
                }
                else
                {
                    MessageBox.Show("Số lượng và đơn giá không phù hợp");
                } 
                    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachChiTietPhieuNhap();
            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM CT_PHIEUNHAPSACH WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);

                if (tblMaPhieuNhap.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (cbTenSach.Text == "")
                    MessageBox.Show("Tên sách không được để trống");
                else if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tồn tại chi tiết phiếu nhập sách");
                else if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                {
                    query = "SELECT MaSach FROM SACH WHERE TenSach = @TenSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenSach", cbTenSach.Text);
                    string tenSach = sqlCommand.ExecuteScalar().ToString();

                    MessageBox.Show("Cập nhật thành công");
                    query = "UPDATE CT_PHIEUNHAPSACH SET  MaPhieuNhapSach = @MaPhieuNhapSach, MaSach = @MaSach, SoLuong = @SoLuong, DonGia = @DonGia WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.Parameters.AddWithValue("@MaSach", tenSach);
                    sqlCommand.Parameters.AddWithValue("@SoLuong", soLuong.ToString());
                    sqlCommand.Parameters.AddWithValue("@DonGia", donGia.ToString());
                    sqlCommand.ExecuteScalar();

                    float TongTien = 0;
                    query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    TongTien = float.Parse(sqlCommand.ExecuteScalar().ToString());

                    query = "UPDATE PHIEUNHAPSACH SET TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TongTien", TongTien.ToString());
                    sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                    sqlCommand.ExecuteScalar();

                   

                }
                else
                {
                    MessageBox.Show("Số lượng và đơn giá không phù hợp");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachChiTietPhieuNhap();
            }
        }

        private void txbSoLuong_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                tblThanhTien.Text = (soLuong * donGia).ToString();
        }

        private void txbDonGia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txbSoLuong.Text, out float soLuong) && float.TryParse(txbDonGia.Text, out float donGia) && soLuong > 0 && donGia > 0)
                tblThanhTien.Text = (soLuong * donGia).ToString();
        }

        private void InitTenSach()
        {
            
            sqlConnection.Open();
            string query = "SELECT * FROM DAUSACH"; // Thay thế YourTableName bằng tên bảng của bạn
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["TenDauSach"].ToString();
                cbTenSach.Items.Add(companyName);
            }
            sqlConnection.Close();

        }

        private void dgvChiTietPhieuNhap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaCTPhieuNhap.Text = row_selected.Row["Mã chi tiết phiếu nhập sách"].ToString();
                tblMaPhieuNhap.Text = row_selected.Row["Mã phiếu nhập sách"].ToString();
                cbTenSach.Text = row_selected.Row["Tên sách"].ToString();
                txbSoLuong.Text = row_selected.Row["Số lượng"].ToString();
                txbDonGia.Text = row_selected.Row["Đơn giá"].ToString();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM CT_PHIEUNHAPSACH WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                    if (sqlCommand.ExecuteScalar() == null)
                        MessageBox.Show("Không tìm thấy chi tiết phiếu");
                    else if (dgvChiTietPhieuNhap.SelectedItems.Count <= 0)
                        MessageBox.Show("Không có thông tin chi tiết phiếu");
                    else
                    {

                        //xóa sách
                        query = "SELECT MaSach FROM SACH JOIN DAUSACH ON SACH.MaDauSach = DAUSACH.MaDauSach WHERE TenDauSach = @TenSach";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@TenSach", cbTenSach.Text);
                        string maSach = sqlCommand.ExecuteScalar().ToString();

                        query = "SELECT COUNT(*) FROM (SELECT SUB_CUONSACH.MaCuonSach FROM (SELECT MaCuonSach FROM CUONSACH WHERE TinhTrang = 0 AND MaSach = @MaSach EXCEPT SELECT MaCuonSach FROM BAOCAOTRATRE) AS SUB_CUONSACH" +
                            " EXCEPT (SELECT MaCuonSach FROM PHIEUMUONTRASACH) ) AS ABC";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
                        int soLuong = Int32.Parse(sqlCommand.ExecuteScalar().ToString());

                        DataRowView selectedItem = dgvChiTietPhieuNhap.SelectedItem as DataRowView;
                        int soLuongXoa = Int32.Parse(selectedItem.Row["Số lượng"].ToString());
                        if (soLuong < soLuongXoa)
                            MessageBox.Show("Không thể xóa, vì số lượng sách không đủ");
                        else
                        {
                            query = "SELECT TOP (@x) MaCuonSach FROM (SELECT SUB_CUONSACH.MaCuonSach FROM (SELECT MaCuonSach FROM CUONSACH WHERE TinhTrang = 0 AND MaSach = @MaSach EXCEPT SELECT MaCuonSach FROM BAOCAOTRATRE) AS SUB_CUONSACH EXCEPT (SELECT MaCuonSach FROM PHIEUMUONTRASACH) ) AS ABC";
                            sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaSach", maSach);
                            //MessageBox.Show(soLuongXoa.ToString());
                            sqlCommand.Parameters.AddWithValue("@x", soLuongXoa);
                            SqlDataReader reader = sqlCommand.ExecuteReader();

                            string[] maCuonSach = new string[soLuongXoa];

                            int i = 0;
                            while (reader.Read())
                            {
                                string companyName = reader["MaCuonSach"].ToString();

                                maCuonSach[i] = companyName;
                                i++;
                            }
                            sqlConnection.Close();

                            sqlConnection.Open();

                            for (int j = 0; j < soLuongXoa; j++)
                            {
                                MessageBox.Show(maCuonSach[j]);
                                query = "DELETE FROM CUONSACH WHERE MaCuonSach = @MaCuonSach";
                                sqlCommand = new SqlCommand(query, sqlConnection);
                                sqlCommand.Parameters.AddWithValue("@MaCuonSach", maCuonSach[j]);
                                sqlCommand.ExecuteScalar();

                            }


                            query = "DELETE FROM CT_PHIEUNHAPSACH WHERE MaCTPhieuNhapSach = @MaCTPhieuNhapSach";
                            sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaCTPhieuNhapSach", tblMaCTPhieuNhap.Text);
                            sqlCommand.ExecuteScalar();

                            //tính lại tổng tiền
                            float TongTien = 0;
                            query = "SELECT SUM(SoLuong * DonGia) FROM CT_PHIEUNHAPSACH WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                            sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                            float.TryParse(sqlCommand.ExecuteScalar().ToString(), out TongTien);

                            query = "UPDATE PHIEUNHAPSACH SET TongTien = @TongTien WHERE MaPhieuNhapSach = @MaPhieuNhapSach";
                            sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@TongTien", TongTien.ToString());
                            sqlCommand.Parameters.AddWithValue("@MaPhieuNhapSach", tblMaPhieuNhap.Text);
                            sqlCommand.ExecuteScalar();

                            tblTongTien.Text = TongTien.ToString();
                        }

                        tblMaCTPhieuNhap.Text = "";
                        cbTenSach.Text = "";
                        txbSoLuong.Text = "";
                        txbDonGia.Text = "";
                        tblThanhTien.Text = "";

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    HienThiDanhSachChiTietPhieuNhap();
                }
            }    
        }
    }
}
