using AdminApp.Dtos;
using AdminApp.Enums;
using AdminApp.Extensions;
using AdminApp.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PackingApp.Services.API;
using System.Windows;

namespace AdminApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    public LoginViewModel(MainWindowViewModel mainWindowViewModel, IClientActions client, ERoleName roleName)
    {
        this.mainWindowViewModel = mainWindowViewModel;
        this.client = client;
        this.roleName = roleName;
    }

    [ObservableProperty]
    private string userName = "naidung";

    [ObservableProperty]
    private string password = "1306";

    [ObservableProperty]
    private bool buttonEnable = true;

    private readonly MainWindowViewModel mainWindowViewModel;
    private readonly IClientActions client;
    private readonly ERoleName roleName;

    public async void LoginAction(string password)
    {
        if (ButtonEnable)
        {
            ButtonEnable = false;

            if(string.IsNullOrEmpty(UserName) ||  string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin");
                ButtonEnable = true;
                return;
            }

            var userDto = new UserDto { UserName = UserName, Password = password };
            APIResult result = await client.Post(APIURLs.Users_Login, userDto);
            if(result.Status != Enums.EAPIStatus.Success)
            {
                MessageBox.Show(result.Msg);
                ButtonEnable = true;
                return;
            }

            LocalDB.User = result.Data.DeserializeObject<UserDto>();

            mainWindowViewModel.ShowLogin = Visibility.Collapsed;
            mainWindowViewModel.FullName = LocalDB.User!.FullName!;
            mainWindowViewModel.PhoneNo = LocalDB.User!.Phone!;
            mainWindowViewModel.Role = string.Join(", ", LocalDB.User!.Roles.Select(e=> roleName.GetRoleName((ERole)e)));

            ButtonEnable = true;
            UserName = "";
        }
    }

}
