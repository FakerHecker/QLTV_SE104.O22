using MaterialDesignThemes.Wpf;
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
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            dpThoiGian.Text = DateTime.Now.ToString();
            
        }

        private void HienThiBaoCaoTheoTheLoai()
        {
            sqlConnection.Open();
            string query = "SELECT MaTheLoai, COUNT(*) AS SoLuotMuon, 100.0 * COUNT(*) / SUM(COUNT(*)) OVER () AS TiLe FROM CT_BCMUONSACH JOIN BAOCAOMUONSACH ON CT_BCMUONSACH.MaBaoCaoMuonSach = BAOCAOMUONSACH.MaBaoCaoMuonSach GROUP BY MaTheLoai;";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            
            query = "SELECT COUNT(MaTheLoai) FROM CT_BCMUONSACH JOIN BAOCAOMUONSACH ON CT_BCMUONSACH.MaBaoCaoMuonSach = BAOCAOMUONSACH.MaBaoCaoMuonSach";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            tblTongSoLuotMuon.Text = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private void HienThiBaoCaoSachTraTre()
        {
            sqlConnection.Open();
            string query = "SELECT MaCuonSach, NgayMuon, DATEDIFF(day, NgayTra, NgayMuon) - 4 AS SoNgayTraTre FROM phieumuontrasach WHERE MONTH(NgayMuon) = 5 AND YEAR(NgayMuon) = 2024 AND DATEDIFF(day, NgayTra, NgayMuon) > 4;";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();

            
        }

        private void cbLoaiBaoCao_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}
