using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for UC_DauSach.xaml
    /// </summary>
    public partial class UC_DauSach : UserControl
    {
        SqlConnection sqlConnection;
        string maDauSach = "DS000";
        public UC_DauSach()
        {
            InitializeComponent();
            string connectionString = @"Data Source=DESKTOP-AV6EQV4\SQLEXPRESS;Initial Catalog=QLTV_DB;User ID=sa;Password=123456;Pooling=False;Encrypt=True;TrustServerCertificate=True";
            sqlConnection = new SqlConnection(connectionString);
            InitTenTheLoai();
            InitTenTacGia();
            InitMaDauSach();
            HienThiDanhSachDauSach();
        }
        

        private void HienThiDanhSachDauSach()
        {
            sqlConnection.Open();
            string query = "SELECT MaDauSach AS 'Mã đầu sách', TenDauSach AS 'Tên đầu sách', TenTheLoai AS 'Tên thể loại' FROM DAUSACH JOIN THELOAI ON DAUSACH.MaTheLoai = THELOAI.MaTheLoai";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvDanhSachDauSach.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dgvDanhSachDauSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tblMaDauSach.Text = row_selected.Row["Mã đầu sách"].ToString();
                txbTenDauSach.Text = row_selected.Row["Tên đầu sách"].ToString();
                cbTenTheLoai.Text = row_selected.Row["Tên thể loại"].ToString();

                lbTacGia.Items.Clear();

                sqlConnection.Open();
                SqlCommand command = new SqlCommand("SELECT TenTacGia FROM CT_TACGIA JOIN TACGIA ON CT_TACGIA.MaTacGia = TACGIA.MaTacGia WHERE MaDauSach = @MaDauSach", sqlConnection);
                command.Parameters.AddWithValue("@MaDauSach", row_selected.Row["Mã đầu sách"].ToString());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string value = reader["TenTacGia"].ToString();
                    // Thêm giá trị vào danh sách hiển thị trong ListBox
                    lbTacGia.Items.Add(value);
                }
                sqlConnection.Close();
            }

            
        }

        private void InitTenTheLoai()
        {
            string query = "SELECT TenTheLoai FROM THELOAI"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["TenTheLoai"].ToString();
                cbTenTheLoai.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitTenTacGia()
        {
            string query = "SELECT TenTacGia FROM TACGIA"; // Thay thế YourTableName bằng tên bảng của bạn

            sqlConnection.Open();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string companyName = reader["TenTacGia"].ToString();
                cbTacGia.Items.Add(companyName);
            }
            sqlConnection.Close();
        }

        private void InitMaDauSach()
        {
            sqlConnection.Open();
            string query = "SELECT TOP 1 MaDauSach FROM DAUSACH ORDER BY MaDauSach DESC";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            if (sqlCommand.ExecuteScalar() != null)
                maDauSach = sqlCommand.ExecuteScalar().ToString();
            //MessageBox.Show(maPhieuThu);
            sqlConnection.Close();
        }

        private string IncreasePrimaryKey()
        {
            int currentNumber = int.Parse(maDauSach.Substring(2));
            currentNumber++;
            maDauSach = $"DS{currentNumber:D3}";
            return maDauSach;
        }

        private void btnThemMoi_Click(object sender, RoutedEventArgs e)
        {
            tblMaDauSach.Text = IncreasePrimaryKey();
            txbTenDauSach.Text = "";
            cbTenTheLoai.SelectedIndex = -1;
            cbTacGia.SelectedIndex = -1;
            lbTacGia.Items.Clear();
        }

        private void btnThemTacGia_Click(object sender, RoutedEventArgs e)
        {
           if (cbTacGia.SelectedIndex != -1)
            {
                string tacGia = cbTacGia.Text;
                bool check = false;
                for (int i = 0; i < lbTacGia.Items.Count; i++)
                {
                    var item = lbTacGia.Items[i].ToString();
                    if (tacGia == item)
                        check = true;
                }
                if (check == false)
                    lbTacGia.Items.Add(tacGia);
            }    
        }

        private void btnXoaTacGia_Click(object sender, RoutedEventArgs e)
        {
            if (lbTacGia.SelectedItem != null)
            {
                lbTacGia.Items.Remove(lbTacGia.SelectedItem);
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                object tonTaiMaDauSach = sqlCommand.ExecuteScalar();

                query = "SELECT * FROM DAUSACH WHERE TenDauSach = @TenDauSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                object tonTaiTenDauSach = sqlCommand.ExecuteScalar();

                if (tonTaiMaDauSach != null)
                    MessageBox.Show("Đã tồn tại mã đầu sách");
                else if (tonTaiTenDauSach != null)
                    MessageBox.Show("Đã tồn tại tên đầu sách");
                else if (tblMaDauSach.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (txbTenDauSach.Text == "")
                    MessageBox.Show("Tên đầu sách không được để trống");
                else if (cbTenTheLoai.Text == "")
                    MessageBox.Show("Tên thể loại không được để trống");
                else if (lbTacGia.Items.Count == 0)
                    MessageBox.Show("Tác giả của sách không được để trống");
                else
                {
                    query = "SELECT MaTheLoai FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenTheLoai", cbTenTheLoai.Text);
                    string maTheLoai = sqlCommand.ExecuteScalar().ToString();

                    query = "INSERT INTO DAUSACH (MaDauSach, TenDauSach, MaTheLoai) VALUES (@MaDauSach, @TenDauSach, @MaTheLoai)";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    sqlCommand.ExecuteScalar();

                    for (int i = 0; i < lbTacGia.Items.Count; i++)
                    {
                        var tenTacGia = lbTacGia.Items[i];

                        query = "SELECT MaTacGia FROM TACGIA WHERE TenTacGia = @TenTacGia";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@TenTacGia", tenTacGia.ToString());
                        string maTacGia = sqlCommand.ExecuteScalar().ToString();

                        query = "INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                        sqlCommand.Parameters.AddWithValue("@MaTacGia", maTacGia);
                        sqlCommand.ExecuteScalar();
                    }

                    MessageBox.Show("Thêm thành công");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();

            }
        }

        private void btnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                object tonTaiMaDauSach = sqlCommand.ExecuteScalar();

                query = "SELECT COUNT(*) FROM DAUSACH WHERE TenDauSach = @TenDauSach AND MaDauSach <> @MaDauSach";
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                int tonTaiTenDauSach = Int32.Parse(sqlCommand.ExecuteScalar().ToString());


                if (tonTaiMaDauSach == null)
                    MessageBox.Show("Không tồn tại đầu sách");
                else if (tonTaiTenDauSach == 1)
                    MessageBox.Show("Đã tồn tại tên đầu sách");
                else if (tblMaDauSach.Text == "")
                    MessageBox.Show("Vui lòng chọn 'Thêm mới' để nhập thông tin");
                else if (txbTenDauSach.Text == "")
                    MessageBox.Show("Tên đầu sách không được để trống");
                else if (cbTenTheLoai.Text == "")
                    MessageBox.Show("Tên thể loại không được để trống");
                else if (lbTacGia.Items.Count == 0)
                    MessageBox.Show("Tác giả của sách không được để trống");
                else
                {

                    query = "SELECT MaTheLoai FROM THELOAI WHERE TenTheLoai = @TenTheLoai";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@TenTheLoai", cbTenTheLoai.Text);
                    string maTheLoai = sqlCommand.ExecuteScalar().ToString();

                    query = "UPDATE DAUSACH SET TenDauSach = @TenDauSach, MaTheLoai = @MaTheLoai WHERE MaDauSach = @MaDauSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@TenDauSach", txbTenDauSach.Text);
                    sqlCommand.Parameters.AddWithValue("@MaTheLoai", maTheLoai);
                    sqlCommand.ExecuteScalar();

                    query = "DELETE FROM CT_TACGIA WHERE MaDauSach = @MaDauSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.ExecuteScalar();

                    for (int i = 0; i < lbTacGia.Items.Count; i++)
                    {
                        var tenTacGia = lbTacGia.Items[i];

                        query = "SELECT MaTacGia FROM TACGIA WHERE TenTacGia = @TenTacGia";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@TenTacGia", tenTacGia.ToString());
                        string maTacGia = sqlCommand.ExecuteScalar().ToString();

                        query = "INSERT INTO CT_TACGIA (MaDauSach, MaTacGia) VALUES (@MaDauSach, @MaTacGia)";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                        sqlCommand.Parameters.AddWithValue("@MaTacGia", maTacGia);
                        sqlCommand.ExecuteScalar();
                    }

                    MessageBox.Show("Cập nhật thành công");
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                if (sqlCommand.ExecuteScalar() == null)
                    MessageBox.Show("Không tìm thấy mã đầu sách");
                else
                {
                    query = "DELETE FROM CT_TACGIA WHERE MaDauSach = @MaDauSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.ExecuteScalar();

                    query = "DELETE FROM DAUSACH WHERE MaDauSach = @MaDauSach";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@MaDauSach", tblMaDauSach.Text);
                    sqlCommand.ExecuteScalar();
                }

                tblMaDauSach.Text = "";
                txbTenDauSach.Text = "";
                cbTenTheLoai.SelectedIndex = -1;
                cbTacGia.SelectedIndex = -1;
                lbTacGia.Items.Clear();
                                   
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("Đầu sách đang được sử dụng");
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDauSach();
            }
        }
    }
}
