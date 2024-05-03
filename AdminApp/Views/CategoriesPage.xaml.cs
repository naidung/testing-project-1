using AdminApp.Dtos;
using AdminApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace AdminApp.Views;

/// <summary>
/// Interaction logic for CategoriesPage.xaml
/// </summary>
public partial class CategoriesPage : Page
{
    private readonly CategoriesViewModel viewmodel;
    private readonly CategoriesViewModel viewmodel1;
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;

    public CategoriesPage(CategoriesViewModel viewmodel, MainWindow mainWindow, MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = this.viewmodel = viewmodel;
        _ = viewmodel.GetCategories();
        viewmodel1 = viewmodel;
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
    }

    private void DeleteItemClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            CategoryDto context = ((sender as Button)!.DataContext as CategoryDto)!;
            if (context == null) return;
            var confirm = MessageBox.Show("Bạn muốn xóa nhóm này chứ?", "Xác nhận", MessageBoxButton.YesNoCancel);
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
            CategoryDto context = ((sender as Button)!.DataContext as CategoryDto)!;
            if (context == null) return;
            var categoryDetailPage = App.serviceProvider.GetService<CategoryDetailPage>();
            mainWindow.BodyFrame.Content = categoryDetailPage;
            mainWindowViewModel.CurrentPath = "Thay đổi thông tin nhóm sản phẩm";
            categoryDetailPage!.PageInitialize(context.Id);
        }
        catch { }
    }

}
