using AdminApp.Dtos;
using AdminApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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

namespace AdminApp.Views;

/// <summary>
/// Interaction logic for ProductsPage.xaml
/// </summary>
public partial class ProductsPage : Page
{
    private readonly ProductsViewModel viewmodel;
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;

    public ProductsPage(ProductsViewModel viewmodel, MainWindow mainWindow, MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = this.viewmodel = viewmodel;
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
    }

    private void DeleteItemClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            ProductDto context = ((sender as Button)!.DataContext as ProductDto)!;
            if (context == null) return;
            var confirm = MessageBox.Show("Bạn muốn xóa sản phẩm này chứ?", "Xác nhận", MessageBoxButton.YesNoCancel);
            if (confirm == MessageBoxResult.Yes)
            {
                _ = viewmodel.DeleteItem(context.Id);
            }
        }
        catch { }
    }

    private void ViewDetailItemClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            ProductDto context = ((sender as Button)!.DataContext as ProductDto)!;
            if (context == null) return;
            var productDetailPage = App.serviceProvider.GetService<ProductDetailPage>();
            mainWindow.BodyFrame.Content = productDetailPage;
            mainWindowViewModel.CurrentPath = "Thông tin sản phẩm";
            productDetailPage!.PageInitialize(context.Id);
        }
        catch { }
    }

}
