using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_BaoCaoThongKe.xaml
    /// </summary>
    public partial class UC_BaoCaoThongKe : UserControl
    {
        SqlConnection sqlConnection;
        public UC_BaoCaoThongKe() 
            
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security = True"; sqlConnection = new SqlConnection(connectionString);
            sqlConnection = new SqlConnection(connectionString);

            InitComboBoxThangItems();
            InitComboBoxNamItems();
            dpNgayLap.SelectedDate = DateTime.Now;
            
        }
        private void InitComboBoxThangItems()
        {
            for (int i = 1; i <= 12; i++)
                cbThang.Items.Add(i.ToString());
            cbThang.SelectedIndex = DateTime.Now.Month - 1;
        }
        private void InitComboBoxNamItems()
        {
            int nam = DateTime.Now.Year;
            for (int i = nam - 4; i <= nam; i++)
                cbNam.Items.Add(i.ToString());
            cbNam.SelectedIndex = 4;
            
        }

        

        private void HienThiBaoCaoTheoTheLoai()
        {
            sqlConnection.Open();
            string query =  " SELECT THELOAI.TenTheLoai AS 'Tên thể loại', COUNT(*) AS 'Số lượt mượn', 100.0 * COUNT(*) / SUM(COUNT(*)) OVER() AS 'Tỉ lệ (%)' " +
                            " FROM PHIEUMUONTRASACH JOIN CUONSACH ON PHIEUMUONTRASACH.MaCuonSach = CUONSACH.MaCuonSach JOIN SACH ON CUONSACH.MaSach = SACH.MaSach JOIN DAUSACH ON SACH.MaDauSach = DAUSACH.MaDauSach JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai " +
                            " WHERE MONTH(NgayMuon) = @month AND YEAR(NgayMuon) = @year" +
                            " GROUP BY THELOAI.TenTheLoai";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@month", cbThang.SelectedValue as string);
            sqlCommand.Parameters.AddWithValue("@year", cbNam.SelectedValue as string);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;

            query = " SELECT COUNT(THELOAI.TenTheLoai)" +
                            " FROM PHIEUMUONTRASACH JOIN CUONSACH ON PHIEUMUONTRASACH.MaCuonSach = CUONSACH.MaCuonSach JOIN SACH ON CUONSACH.MaSach = SACH.MaSach JOIN DAUSACH ON SACH.MaDauSach = DAUSACH.MaDauSach JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai " +
                            " WHERE MONTH(NgayMuon) = @month AND YEAR(NgayMuon) = @year";
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@month", cbThang.SelectedValue as string);
            sqlCommand.Parameters.AddWithValue("@year", cbNam.SelectedValue as string);
            tblTongSoLuotMuon.Text = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();

            lbTongSoLuotMuon.Visibility = Visibility.Visible;
            tblTongSoLuotMuon.Visibility = Visibility.Visible;

            dpNgayLap.Visibility = Visibility.Hidden;
            cbThang.Visibility = Visibility.Visible;
            cbNam.Visibility = Visibility.Visible;

            lbNgayLap.Visibility = Visibility.Hidden;
            lbThang.Visibility = Visibility.Visible;
            lbNam.Visibility = Visibility.Visible;
        }

        private void HienThiBaoCaoSachTraTre()
        {
            sqlConnection.Open();

            string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoNgayMuonToiDa'";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            int soNgayMuonToiDa = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
            
            query = "SELECT CUONSACH.MaCuonSach AS 'Mã cuốn sách', TenSach AS 'Tên cuốn sách', NgayMuon AS 'Ngày mượn', DATEDIFF(day, NgayMuon, @NgayPhaiTra) - @SoNgayMuonToiDa AS 'Số ngày trả trễ' " +
                "FROM PHIEUMUONTRASACH JOIN CUONSACH ON PHIEUMUONTRASACH.MaCuonSach = CUONSACH.MaCuonSach JOIN SACH ON CUONSACH.MaSach = Sach.MaSach " +
                "WHERE DATEDIFF(day, NgayMuon, @NgayPhaiTra) > @SoNgayMuonToiDa AND NgayTra IS NULL";
            sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@NgayPhaiTra", DateTime.Parse(dpNgayLap.Text));
            sqlCommand.Parameters.AddWithValue("@SoNgayMuonToiDa", soNgayMuonToiDa);

           

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

            lbTongSoLuotMuon.Visibility = Visibility.Hidden;
            tblTongSoLuotMuon.Visibility = Visibility.Hidden;

            dpNgayLap.Visibility = Visibility.Visible;
            cbThang.Visibility = Visibility.Hidden;
            cbNam.Visibility = Visibility.Hidden;

            lbNgayLap.Visibility = Visibility.Visible;
            lbThang.Visibility = Visibility.Hidden;
            lbNam.Visibility = Visibility.Hidden;
        }

        private void cbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                HienThiBaoCaoTheoTheLoai();
                InitMaBaoCaoMuonSach();
                
            }
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                HienThiBaoCaoSachTraTre();
                InitMaBaoCaoTraTre();
               
            }  
            
        }

        private void cbThang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                HienThiBaoCaoTheoTheLoai();
            }
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                HienThiBaoCaoSachTraTre();
            }
            
        }

        private void cbNam_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                HienThiBaoCaoTheoTheLoai();
            }
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                HienThiBaoCaoSachTraTre();
            }
            
        }

        private void btnInBaoCao_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(dataGridView, "My Control Print");
                printDlg.PrintVisual(gr, "My Control Print");
            }
        }
        
        private void InitMaBaoCaoMuonSach()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaBaoCaoMuonSach FROM BAOCAOMUONSACH ORDER BY MaBaoCaoMuonSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            object maBaoCao = sqlCommand.ExecuteScalar();
            sqlConnection.Close();
            string maBC = "BCMS001";
            if (maBaoCao != null)
            {
                int currentNumber = int.Parse(maBaoCao.ToString().Substring(4));
                currentNumber++;
                maBC = $"BCMS{currentNumber:D3}";
            }
            tblMaBaoCao.Text = maBC;
        }

        private string InitMaBaoCaoTraTre()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaBaoCaoTraTre FROM BAOCAOTRATRE ORDER BY MaBaoCaoTraTRe DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            object maBaoCao = sqlCommand.ExecuteScalar();
            sqlConnection.Close();
            string maBC = "BCTT001";
            if (maBaoCao != null)
            {
                int currentNumber = int.Parse(maBaoCao.ToString().Substring(4));
                currentNumber++;
                maBC = $"BCTT{currentNumber:D3}";
            }
            tblMaBaoCao.Text = maBC;

            return maBC;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                 
                try
                {
                    if (dataGridView.Items.Count > 0)
                    {

                        sqlConnection.Open();
                        string query = "SELECT * FROM BAOCAOMUONSACH WHERE Thang = @Thang AND Nam = @Nam";
                        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@Thang", cbThang.Text);
                        sqlCommand.Parameters.AddWithValue("@Nam", cbNam.Text);
                        object trungPhieu = sqlCommand.ExecuteScalar();
                        sqlConnection.Close();
                        
                        if (trungPhieu != null)
                        {
                            MessageBox.Show("Đã tồn tại báo báo mượn sách tháng " + cbThang.Text + " năm " + cbNam.Text);
                        }    
                        else
                        {
                            // lưu vào bảng BAOCAOMUONSACH
                            sqlConnection.Open();
                            query = "INSERT INTO BAOCAOMUONSACH (MaBaoCaoMuonSach, Thang, Nam, TongSoLuotMuon) VALUES (@MaBaoCaoMuonSach, @Thang, @Nam, @TongSoLuotMuon)";
                            sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaBaoCaoMuonSach", tblMaBaoCao.Text);
                            sqlCommand.Parameters.AddWithValue("@Thang", cbThang.Text);
                            sqlCommand.Parameters.AddWithValue("@Nam", cbNam.Text);
                            sqlCommand.Parameters.AddWithValue("@NgayLapBaoCao", dpNgayLap.Text);
                            sqlCommand.Parameters.AddWithValue("@TongSoLuotMuon", tblTongSoLuotMuon.Text);
                            sqlCommand.ExecuteScalar();
                            MessageBox.Show("Lưu thành công");

                            // lưu vào bảng chi tiết BCMS
                            foreach (var item in dataGridView.Items)
                            {
                                // Xử lý từng dòng dữ liệu tại đây
                                // Ví dụ: truy cập các giá trị của từng cột bằng cách sử dụng các thuộc tính hoặc phương thức của đối tượng item
                                string tenTheLoai = ((DataRowView)item).Row["Tên thể loại"].ToString();
                                query = "SELECT MaTheLoai FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                                sqlCommand = new SqlCommand(query, sqlConnection);
                                sqlCommand.Parameters.AddWithValue("@TenTheLoai", tenTheLoai);
                                object maTL = sqlCommand.ExecuteScalar();
                                string maTheLoai = "";
                                if (maTL != null)
                                {
                                    maTheLoai = maTL.ToString();
                                    query = "INSERT INTO CT_BCMUONSACH (MaBaoCaoMuonSach, MaTheLoai, SoLuotMuon, TiLe) VALUES (@MaBaoCaoMuonSach, @MaTheLoai, @SoLuotMuon, @TiLe)";
                                    sqlCommand = new SqlCommand(query, sqlConnection);
                                    sqlCommand.Parameters.AddWithValue("@MaBaoCaoMuonSach", tblMaBaoCao.Text);
                                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                                    sqlCommand.Parameters.AddWithValue("@SoLuotMuon", ((DataRowView)item).Row["Số lượt mượn"].ToString());
                                    sqlCommand.Parameters.AddWithValue("@TiLe", ((DataRowView)item).Row["Tỉ lệ (%)"]);
                                    sqlCommand.ExecuteScalar();
                                }
                                else
                                {
                                    MessageBox.Show("Không tồn tại thể loại");
                                }
                            }

                            sqlConnection.Close();
                        }    
                      
                    }
                    else
                        MessageBox.Show("Không có dữ liệu");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }   
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                try
                {
                    
                    if (dataGridView.Items.Count > 0)
                    {
                       
                        foreach (var item in dataGridView.Items)
                        {

                            string maBaoCao = InitMaBaoCaoTraTre();
                            sqlConnection.Open();                           
                            string maCuonSach = ((DataRowView)item).Row["Mã cuốn sách"].ToString();
                            string query = "INSERT INTO BAOCAOTRATRE (MaBaoCaoTraTre, MaCuonSach, NgayMuon, SoNgayTraTre, NgayLapBaoCao) " +
                                       " VALUES (@MaBaoCaoTraTre, @MaCuonSach, @NgayMuon, @SoNgayTraTre, @NgayLapBaoCao)";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlCommand.Parameters.AddWithValue("@MaBaoCaoTraTre", maBaoCao);
                            sqlCommand.Parameters.AddWithValue("@NgayLapBaoCao", DateTime.Parse(dpNgayLap.Text));
                            sqlCommand.Parameters.AddWithValue("@MaCuonSach", maCuonSach);
                            sqlCommand.Parameters.AddWithValue("@NgayMuon", ((DataRowView)item).Row["Ngày mượn"]);
                            sqlCommand.Parameters.AddWithValue("@SoNgayTraTre", ((DataRowView)item).Row["Số ngày trả trễ"]);
                            sqlCommand.ExecuteScalar();
                            sqlConnection.Close();
                            
                        }
                        MessageBox.Show("Lưu thành công");
                        
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu");
                    }    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }    
            else
            {
                MessageBox.Show("Vui lòng chọn loại báo cáo");
            }    
        }

        private void dpNgayLap_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                HienThiBaoCaoTheoTheLoai();
            }
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                HienThiBaoCaoSachTraTre();
            }
        }

        private void dataGridView_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Ngày mượn")
            {
                var column = e.Column as System.Windows.Controls.DataGridTextColumn;
                if (column != null)
                {
                    column.Binding.StringFormat = "dd/MM/yyyy";
                }
            }
        }
    }
}
