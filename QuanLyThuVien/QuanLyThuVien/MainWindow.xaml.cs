using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void btQuanLyDocGia_Click(object sender, RoutedEventArgs e)
        {
            btnDocGia.Visibility = Visibility.Visible;
            btnLoaiDocGia.Visibility = Visibility.Visible;
            btnPhieuThuTienPhat.Visibility = Visibility.Visible;
        }


        private void btQuanLySach_Click(object sender, RoutedEventArgs e)
        {
            btnQuanLyChiTietPhieuNhap.Visibility = Visibility.Visible;
        }

        private void btnQuanLyChiTietPhieuNhap_Click(object sender, RoutedEventArgs e)
        {
            W_QuanLyChiTietPhieuNhapSach ql = new W_QuanLyChiTietPhieuNhapSach ();
            ql.Show();
        }

        private void btnDocGia_Click(object sender, RoutedEventArgs e)
        {
            W_QuanLyDocGia ql = new W_QuanLyDocGia ();
            ql.Show();
        }

        private void btnLoaiDocGia_Click(object sender, RoutedEventArgs e)
        {
            W_QuanLyLoaiDocGia ql = new W_QuanLyLoaiDocGia ();
            ql.Show();
        }

        private void btnPhieuThuTienPhat_Click(object sender, RoutedEventArgs e)
        {
            W_QuanLyPhieuThuTienPhat ql = new W_QuanLyPhieuThuTienPhat ();
            ql.Show();
        }
    }
}
