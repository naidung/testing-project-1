using AdminApp.Helpers;
using AdminApp.ViewModels;
using AdminApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public static MainWindow Instance { get; set; } = null!;

    public MainWindowViewModel viewmodel { get; set; } = null!;

    public MainWindow(MainWindowViewModel viewmodel)
    {
        InitializeComponent();
        Instance = this;
        this.DataContext = this.viewmodel = viewmodel;
    }

    private void borderEmployees_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewmodel.EmployeeTabBrush = "#93c5fd".Hex2Brush()!;
        viewmodel.CategoriesTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.ProductsTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.CurrentPath = "Danh sách nhân viên";
        BodyFrame.Content = App.serviceProvider.GetService<UsersPage>();
    }

    private void borderCategories_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewmodel.EmployeeTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.CategoriesTabBrush = "#93c5fd".Hex2Brush()!;
        viewmodel.ProductsTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.CurrentPath = "Danh sách nhóm sản phẩm";
        BodyFrame.Content = App.serviceProvider.GetService<CategoriesPage>();
    }

    private void borderProducts_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        viewmodel.EmployeeTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.CategoriesTabBrush = "#60a5fa".Hex2Brush()!;
        viewmodel.ProductsTabBrush = "#93c5fd".Hex2Brush()!;
        viewmodel.CurrentPath = "Danh sách sản phẩm";
        BodyFrame.Content = App.serviceProvider.GetService<ProductsPage>();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        //BodyFrame.GoBack();
    }

    private async void btnLogout_Click(object sender, RoutedEventArgs e)
    {
        var confirm = MessageBox.Show("Bạn muốn đăng xuất chứ", "Xác nhận", MessageBoxButton.YesNoCancel);
        if(confirm == MessageBoxResult.Yes)
        {
            var result = await viewmodel.Logout();
            if(result)
            {
                BodyFrame.Content = null;
                LocalDB.User = null;
                viewmodel.EmployeeTabBrush = "#60a5fa".Hex2Brush()!;
                viewmodel.CategoriesTabBrush = "#60a5fa".Hex2Brush()!;
                viewmodel.ProductsTabBrush = "#60a5fa".Hex2Brush()!;
                viewmodel.CurrentPath = "Trang chủ";
                viewmodel.ShowLogin = Visibility.Visible;
                viewmodel.FullName = "";
                viewmodel.PhoneNo = "";
                viewmodel.Role = "";
            }
        }
    }

}