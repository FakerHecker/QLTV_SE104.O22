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
            btnSach.Visibility = Visibility.Hidden;
            btnTacGia.Visibility = Visibility.Hidden;
            btnTheLoai.Visibility = Visibility.Hidden;
            btnPhieuMuonSach.Visibility = Visibility.Hidden;
            btnPhieuTraSach.Visibility = Visibility.Hidden;
            CC.Content = new DocGia();
        }


        private void btQuanLySach_Click(object sender, RoutedEventArgs e)
        {
            btnDocGia.Visibility = Visibility.Hidden;
            btnLoaiDocGia.Visibility = Visibility.Hidden;
            btnPhieuThuTienPhat.Visibility = Visibility.Hidden;

            btnSach.Visibility = Visibility.Visible;
            btnTacGia.Visibility = Visibility.Visible;
            btnTheLoai.Visibility = Visibility.Visible;
            btnPhieuNhapSach.Visibility = Visibility.Visible;
            btnDauSach.Visibility = Visibility.Visible;

            btnPhieuMuonSach.Visibility = Visibility.Hidden;
            btnPhieuTraSach.Visibility = Visibility.Hidden;
            CC.Content = new UC_Sach();
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

        private void btnQuanLyMuonTra_Click(object sender, RoutedEventArgs e)
        {
            btnDocGia.Visibility = Visibility.Hidden;
            btnLoaiDocGia.Visibility = Visibility.Hidden;
            btnPhieuThuTienPhat.Visibility = Visibility.Hidden;
            btnSach.Visibility = Visibility.Hidden;
            btnTacGia.Visibility = Visibility.Hidden;
            btnTheLoai.Visibility = Visibility.Hidden;
            btnPhieuMuonSach.Visibility = Visibility.Visible;
            btnPhieuTraSach.Visibility = Visibility.Visible;
            CC.Content = new UC_ChoMuonSach();
        }

        private void btnPhieuMuonSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_ChoMuonSach();
        }

        private void btnPhieuTraSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_NhanTraSach();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_TraCuuSach();
            btnDocGia.Visibility = Visibility.Hidden;
            btnLoaiDocGia.Visibility = Visibility.Hidden;
            btnPhieuThuTienPhat.Visibility = Visibility.Hidden;
            btnSach.Visibility = Visibility.Hidden;
            btnTacGia.Visibility = Visibility.Hidden;
            btnTheLoai.Visibility = Visibility.Hidden;
            btnPhieuMuonSach.Visibility = Visibility.Hidden;
            btnPhieuTraSach.Visibility = Visibility.Hidden;
        }

        private void btnLapBaoCao_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_BaoCaoThongKe();
            btnDocGia.Visibility = Visibility.Hidden;
            btnLoaiDocGia.Visibility = Visibility.Hidden;
            btnPhieuThuTienPhat.Visibility = Visibility.Hidden;
            btnSach.Visibility = Visibility.Hidden;
            btnTacGia.Visibility = Visibility.Hidden;
            btnTheLoai.Visibility = Visibility.Hidden;
            btnPhieuMuonSach.Visibility = Visibility.Hidden;
            btnPhieuTraSach.Visibility = Visibility.Hidden;
        }

        private void btnThayDoiQuyDinh_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_ThayDoiQuyDinh();
            btnDocGia.Visibility = Visibility.Hidden;
            btnLoaiDocGia.Visibility = Visibility.Hidden;
            btnPhieuThuTienPhat.Visibility = Visibility.Hidden;
            btnSach.Visibility = Visibility.Hidden;
            btnTacGia.Visibility = Visibility.Hidden;
            btnTheLoai.Visibility = Visibility.Hidden;
            btnPhieuMuonSach.Visibility = Visibility.Hidden;
            btnPhieuTraSach.Visibility = Visibility.Hidden;
        }

        private void btnPhieuNhapSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_PhieuNhapSach();
        }

        private void btnDauSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_DauSach();
        }
    }
}
