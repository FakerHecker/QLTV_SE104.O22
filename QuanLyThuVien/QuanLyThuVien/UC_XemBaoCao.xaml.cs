using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Interaction logic for UC_XemBaoCao.xaml
    /// </summary>
    public partial class UC_XemBaoCao : UserControl
    {
        SqlConnection sqlConnection;
        public UC_XemBaoCao()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True"; sqlConnection = new SqlConnection(connectionString);
            sqlConnection = new SqlConnection(connectionString);

           

        }

        private void HienThiBaoCaoTheoTheLoai()
        {
            sqlConnection.Open();
            string query = "SELECT MaBaoCaoMuonSach AS 'Mã báo cáo mượn sách', Thang AS 'Tháng', Nam AS 'Năm', TongSoLuotMuon AS 'Tổng số lượt mượn', NgayLapBaoCao AS 'Ngày lập báo cáo' FROM BAOCAOMUONSACH";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void HienThiBaoCaoSachTraTre()
        {
            sqlConnection.Open();

            string query = "SELECT MaBaoCaoTraTre AS 'Mã báo cáo trả trễ', NgayMuon AS 'Ngày mượn', MaCuonSach AS 'Mã cuốn sách', SoNgayTraTre AS 'Số ngày trả trễ', Thang AS 'Tháng', Nam AS 'Năm', NgayLapBaoCao AS 'Ngày lập báo cáo' FROM BAOCAOTRATRE";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;

            sqlConnection.Close();

            lbTongSoLuotMuon.Visibility = Visibility.Hidden;
            tblTongSoLuotMuon.Visibility = Visibility.Hidden;

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
                int currentNumber = int.Parse(maBC.Substring(4));
                currentNumber++;
                maBC = $"BCMS{currentNumber:D3}";
            }
            
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
                int currentNumber = int.Parse(maBC.Substring(4));
                currentNumber++;
                maBC = $"BCTT{currentNumber:D3}";
            }
         

            return maBC;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (cbLoaiBaoCao.SelectedIndex == 0)
            {
                DataRowView selectedItem = dataGridView.SelectedItem as DataRowView;
                if (selectedItem != null)
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM CT_BCMUONSACH WHERE MaBaoCaoMuonSach = @MaBaoCaoMuonSach";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaBaoCaoMuonSach", selectedItem.Row["Mã báo cáo mượn sách"].ToString());
                    sqlCommand.ExecuteScalar();

                    query = "DELETE FROM BAOCAOMUONSACH WHERE MaBaoCaoMuonSach = @MaBaoCaoMuonSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaBaoCaoMuonSach", selectedItem.Row["Mã báo cáo mượn sách"].ToString());
                    sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                }
                HienThiBaoCaoTheoTheLoai();
            }
            else if (cbLoaiBaoCao.SelectedIndex == 1)
            {
                DataRowView selectedItem = dataGridView.SelectedItem as DataRowView;
                if (selectedItem != null)
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM BAOCAOTRATRE WHERE MaBaoCaoTraTre = @MaBaoCaoTraTre";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaBaoCaoTraTre", selectedItem.Row["Mã báo cáo trả trễ"].ToString());
                    sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                }
                HienThiBaoCaoSachTraTre();
            }
        }
    }
}