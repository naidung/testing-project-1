using AdminApp.Enums;
using AdminApp.ViewModels;
using AdminApp.Views;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AdminApp;

public partial class App : Application
{
    public static IServiceProvider serviceProvider { get; set; } = null!;

    public App()
    {
        ServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddTransient<UsersPage>();
        services.AddTransient<UserDetailPage>();
        services.AddTransient<CategoriesPage>();
        services.AddTransient<CategoryDetailPage>();
        services.AddTransient<ProductsPage>();
        services.AddTransient<ProductDetailPage>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<UsersPageViewModel>();
        services.AddTransient<UserDetailViewModel>();
        services.AddTransient<CategoriesViewModel>();
        services.AddTransient<CategoryDetailViewModel>();
        services.AddTransient<ProductsViewModel>();
        services.AddTransient<ProductDetailViewModel>();

        services.AddTransient<IClientActions, ClientActions>();
        services.AddTransient<ERoleName>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow mainWindow = serviceProvider.GetService<MainWindow>()!;
        mainWindow.Show();
    }

}
