using AdminApp.Dtos;
using AdminApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
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
/// Interaction logic for UsersPage.xaml
/// </summary>
public partial class UsersPage : Page
{
    private readonly UsersPageViewModel viewmodel;
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;

    public UsersPage(UsersPageViewModel viewmodel, MainWindow mainWindow, MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = this.viewmodel = viewmodel;
        _ = viewmodel.GetUsers();
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
    }

    private void DeleteItemClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            UserDto context = ((sender as Button)!.DataContext as UserDto)!;
            if (context == null) return;
            var confirm = MessageBox.Show("Bạn muốn xóa nhân viên này chứ?", "Xác nhận", MessageBoxButton.YesNoCancel);
            if(confirm == MessageBoxResult.Yes)
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
            UserDto context = ((sender as Button)!.DataContext as UserDto)!;
            if (context == null) return;
            var userDetailPage = App.serviceProvider.GetService<UserDetailPage>();
            mainWindow.BodyFrame.Content = userDetailPage;
            mainWindowViewModel.CurrentPath = "Thông tin nhân viên";
            userDetailPage!.GetUserInfo(context.Id);
        }
        catch { }
    }
}
