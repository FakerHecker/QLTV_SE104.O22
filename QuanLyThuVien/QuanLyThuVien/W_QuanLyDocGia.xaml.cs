﻿using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_QuanLyDocGia.xaml
    /// </summary>
    public partial class W_QuanLyDocGia : Window
    {
        SqlConnection sqlConnection;
        public W_QuanLyDocGia()
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

        private void dataGridView_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
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

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            txtMaDocGia.Text = "";
            txtHoTen.Text = "";
            cbLoaiDG.SelectedIndex = 0;
            dtNgaySinh.SelectedDate = new DateTime(2000, 1, 1);
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            dtNgayLapThe.SelectedDate = DateTime.Now;
            DateTime date = DateTime.Now.AddDays(180);
            txtNgayHetHan.Text = date.ToString();
            txtTongNo.Text = "0";
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

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE DOCGIA SET HoVaTen = @HoVaTen, DiaChi = @DiaChi WHERE MaDocGia = @MaDocGia";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                sqlCommand.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
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
                age = (int)difference.TotalDays/365;

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
                txtNgayHetHan.Text = newDate.ToString("dd/MM/yyyy");
            }
        }

    }
}