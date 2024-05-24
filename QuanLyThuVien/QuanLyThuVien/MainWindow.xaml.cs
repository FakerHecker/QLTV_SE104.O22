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
        private void ChangeSize(Button button)
        {
            if (button != null)
            {
                button.Width += 20; // Increase the width by 10
                button.Height += 20; // Increase the height by 10
            }
        }
        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            // Lấy đối tượng TextBlock từ ListViewItem
            ListViewItem item = (ListViewItem)sender;
            TextBlock textBlock = FindVisualChild<TextBlock>(item);

            // Thay đổi kích thước của ListViewItem
            item.Height = 130; // Đặt kích thước lớn hơn khi con trỏ chuột chạm vào

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
            item.Height = 100; // Đặt lại kích thước khi con trỏ chuột rời khỏi

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
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.Background = new SolidColorBrush(Colors.SkyBlue);
            }
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemPerson":
                    ChangeButtonBackground(btnDocGia);
                    SP1.Visibility = Visibility.Visible;

                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Hidden;
                    CC.Content = new DocGia();
                    break;
                case "ItemBook":
                    ChangeButtonBackground(btnCuonSach);
                    SP2.Visibility = Visibility.Visible;
                    SP1.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Hidden;
                    CC.Content = new UC_CuonSach();
                    break;
                case "ItemBorrow":
                    ChangeButtonBackground(btnPhieuMuonSach);
                    SP3.Visibility = Visibility.Visible;
                    SP2.Visibility = Visibility.Hidden;
                    SP1.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Hidden;
                    CC.Content = new UC_ChoMuonSach();
                    break;
                case "ItemSearch":
                    
                    CC.Content = new UC_TraCuuSach();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Hidden;
                    break;
                case "ItemFix":
                    CC.Content = new UC_ThayDoiQuyDinh();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Hidden;
                    break;
                case "ItemReport":
                    ChangeButtonBackground(btnTaoBaoCao);
                    CC.Content = new UC_BaoCaoThongKe();
                    SP1.Visibility = Visibility.Hidden;
                    SP2.Visibility = Visibility.Hidden;
                    SP3.Visibility = Visibility.Hidden;
                    SP4.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
        private Button lastClickedButton = null;

        private void ChangeButtonBackground(Button clickedButton)
        {
            // Định nghĩa màu nền khi nút được nhấn
            var clickedBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#441A90"));

            // Lấy ra StackPanel chứa button được nhấn
            var parentStackPanel = FindParent<StackPanel>(clickedButton);

            // Đặt lại màu nền cho tất cả các button trong StackPanel trừ button được nhấn
            if (parentStackPanel != null)
            {
                foreach (var child in parentStackPanel.Children)
                {
                    if (child is Button button)
                    {
                        if (button == clickedButton)
                        {
                            button.Background = clickedBackground;
                        }
                        else
                        {
                            button.Background = Brushes.MediumPurple; // Đặt màu nền cho button không được nhấn
                        }
                    }
                }
            }
        }

        // Hàm hỗ trợ để tìm kiếm StackPanel cha của một control
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                ChangeButtonBackground(clickedButton);

                // Handle the button content change logic
                if (clickedButton == btnDocGia)
                {
                    CC.Content = new DocGia();
                }
                else if (clickedButton == btnLoaiDocGia)
                {
                    CC.Content = new UC_LoaiDocGia();
                }
                else if (clickedButton == btnPhieuThuTienPhat)
                {
                    CC.Content = new UC_PhieuThuTienPhat();
                }
                else if (clickedButton == btnCuonSach)
                {
                    CC.Content = new UC_CuonSach();
                }
                else if (clickedButton == btnSach)
                {
                    CC.Content = new UC_Sach();
                }
                else if (clickedButton == btnTheLoai)
                {
                    CC.Content = new UC_TheLoai();
                }
                else if (clickedButton == btnTacGia)
                {
                    CC.Content = new UC_TacGia();
                }
                else if (clickedButton == btnPhieuNhapSach)
                {
                    CC.Content = new UC_PhieuNhapSach();
                }
                else if (clickedButton == btnDauSach)
                {
                    CC.Content = new UC_DauSach();
                }
                else if (clickedButton == btnPhieuMuonSach)
                {
                    CC.Content = new UC_ChoMuonSach();
                }
                else if (clickedButton == btnPhieuTraSach)
                {
                    CC.Content = new UC_NhanTraSach();
                }
                else if (clickedButton == btnTaoBaoCao)
                {
                    CC.Content = new UC_BaoCaoThongKe();
                }
                else if (clickedButton == btnXemBaoCao)
                {
                    CC.Content = new UC_XemBaoCao();
                }
            }
        }
    }
}
