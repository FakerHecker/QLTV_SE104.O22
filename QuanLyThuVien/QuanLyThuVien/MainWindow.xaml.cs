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
        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            // Lấy đối tượng TextBlock từ ListViewItem
            ListViewItem item = (ListViewItem)sender;
            TextBlock textBlock = FindVisualChild<TextBlock>(item);

            // Thay đổi kích thước của ListViewItem
            item.Height = 70; // Đặt kích thước lớn hơn khi con trỏ chuột chạm vào

            // Đặt chữ in đậm
            if (textBlock != null)
            {
                textBlock.FontWeight = FontWeights.Bold;
            }
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            // Lấy đối tượng TextBlock từ ListViewItem
            ListViewItem item = (ListViewItem)sender;
            TextBlock textBlock = FindVisualChild<TextBlock>(item);

            // Đặt lại kích thước và font chữ khi con trỏ chuột rời khỏi ListViewItem
            item.Height = 60; // Đặt lại kích thước khi con trỏ chuột rời khỏi

            // Đặt chữ trở lại bình thường
            if (textBlock != null)
            {
                textBlock.FontWeight = FontWeights.Normal;
            }
        }

        // Hàm hỗ trợ để tìm đối tượng TextBlock trong ListViewItem
        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemPerson":
                    SP1.Visibility = Visibility.Visible;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    CC.Content = new DocGia();
                    break;
                case "ItemBook":
                    SP2.Visibility = Visibility.Visible;
                    SP1.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    CC.Content = new UC_Sach();
                    break;
                case "ItemBorrow":
                    SP3.Visibility = Visibility.Visible;
                    SP2.Visibility = Visibility.Hidden;
                    SP1.Visibility = Visibility.Hidden;
                    CC.Content = new UC_ChoMuonSach();
                    break;
                case "ItemSearch":
                    CC.Content = new UC_TraCuuSach();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    break;
                case "ItemFix":
                    CC.Content = new UC_ThayDoiQuyDinh();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    break;
                case "ItemReport":
                    CC.Content = new UC_BaoCaoThongKe();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
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

        private void btnPhieuMuonSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_ChoMuonSach();
        }

        private void btnPhieuTraSach_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new UC_NhanTraSach();
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

    }

}
