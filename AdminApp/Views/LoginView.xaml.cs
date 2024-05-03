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
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView : UserControl
{
    private LoginViewModel viewmodel;

    public LoginView()
    {
        InitializeComponent();
        DataContext = viewmodel = App.serviceProvider.GetService<LoginViewModel>()!;
    }

    private void borderLogin_Click(object sender, RoutedEventArgs e)
    {
        var password = txtPassword.Password;
        viewmodel.LoginAction(password);
    }
}
