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
    /// Interaction logic for UC_ThayDoiQuyDinh.xaml
    /// </summary>
    public partial class UC_ThayDoiQuyDinh : UserControl
    {
        SqlConnection sqlConnection;
        public UC_ThayDoiQuyDinh()
        {
            InitializeComponent();
            string connectionString = @"Data Source=.\;Initial Catalog=QLTV;Integrated Security=True;";
            sqlConnection = new SqlConnection(connectionString);
            HienThiQuyDinh();
        }
        private void HienThiQuyDinh()
        {
            sqlConnection.Open();
            string query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'ThoiHanThe'";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            tblThoiHanThe.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'TuoiToiThieu'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblTuoiToiThieu.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'TuoiToiDa'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblTuoiToiDa.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'KhoangCachNamXuatBan'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblKhoangCachNamXuatBan.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoNgayMuonToiDa'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblSoNgayMuonToiDa.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoSachMuonToiDa'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblSoSachMuonToiDa.Text = sqlCommand.ExecuteScalar().ToString();

            query = "SELECT GiaTri FROM THAMSO WHERE TenThamSo = 'SoTienPhatMoiNgay'";
            sqlCommand = new SqlCommand(query, sqlConnection);
            tblSoTienPhat.Text = sqlCommand.ExecuteScalar().ToString();
            sqlConnection.Close();
        }

        private void btnThayDoi_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection.Open();
            
            if (cbDieuKien.SelectedIndex == 0)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'ThoiHanThe'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();

            }   
            else if (cbDieuKien.SelectedIndex == 1)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'TuoiToiThieu'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            else if (cbDieuKien.SelectedIndex == 2)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'TuoiToiDa'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            else if (cbDieuKien.SelectedIndex == 3)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'KhoangCachNamXuatBan'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            else if (cbDieuKien.SelectedIndex == 4)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'SoNgayMuonToiDa'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            else if (cbDieuKien.SelectedIndex == 5)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'SoSachMuonToiDa'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            else if (cbDieuKien.SelectedIndex == 6)
            {
                string query = "UPDATE THAMSO SET GiaTri = @GiaTri WHERE TenThamSo = 'SoTienPhatMoiNgay'";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@GiaTri", txbGiaTri.Text);
                sqlCommand.ExecuteScalar();
            }
            sqlConnection.Close();
            HienThiQuyDinh();

        }
    }
}
