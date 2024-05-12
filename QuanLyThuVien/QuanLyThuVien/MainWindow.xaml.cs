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
            btnSach.Visibility = Visibility.Visible;
            btnTacGia.Visibility= Visibility.Visible;
            btnTheLoai.Visibility= Visibility.Visible;
        }

        private void btnQuanLyChiTietPhieuNhap_Click(object sender, RoutedEventArgs e)
        {
            W_QuanLyChiTietPhieuNhapSach ql = new W_QuanLyChiTietPhieuNhapSach ();
            ql.Show();
        }

        private void btnDocGia_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new DocGia();
        }

        private void btnLoaiDocGia_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_LoaiDocGia();
        }

        private void btnPhieuThuTienPhat_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_PhieuThuTienPhat();
        }

        private void btnSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_Sach();
        }

        private void btnTheLoai_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_TheLoai();
        }

        private void btnTacGia_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_TacGia();
        }
    }
}
