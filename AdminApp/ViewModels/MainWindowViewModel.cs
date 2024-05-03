using AdminApp.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackingApp.Services.API;
using System.Windows;
using System.Windows.Media;

namespace AdminApp.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IClientActions client;

    public MainWindowViewModel(IClientActions client)
    {
        this.client = client;
    }

    [ObservableProperty]
    private Visibility isBusy = Visibility.Collapsed;

    [ObservableProperty]
    private bool frameCanGoBack = false;

    [ObservableProperty]
    private Visibility showLogin = Visibility.Visible;

    [ObservableProperty]
    private string fullName = "";

    [ObservableProperty]
    private string phoneNo = "";

    [ObservableProperty]
    private string role = "";

    [ObservableProperty]
    private Brush employeeTabBrush = "#60a5fa".Hex2Brush()!;

    [ObservableProperty]
    private Brush categoriesTabBrush = "#60a5fa".Hex2Brush()!;

    [ObservableProperty]
    private Brush productsTabBrush = "#60a5fa".Hex2Brush()!;


    [ObservableProperty]
    private string currentPath = "";


    public async Task<bool> Logout()
    {
        var result = await client.Put(APIURLs.Users_Logout + "/" + LocalDB.User!.Id);
        if(result.Status != Enums.EAPIStatus.Success)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
            return false;
        }
        else
        {
            return true;
        }
    }

}
