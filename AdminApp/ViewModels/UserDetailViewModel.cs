using AdminApp.Dtos;
using AdminApp.Enums;
using AdminApp.Extensions;
using AdminApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminApp.ViewModels;

public partial class UserDetailViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;
    private readonly IClientActions client;

    public UserDetailViewModel(MainWindow mainWindow, MainWindowViewModel mainWindowViewModel, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
        this.client = client;
        ChangingCommand = new RelayCommand(()=>
        {
            _ = ChangingAction();
        });
    }

    private long? UserId { get; set; }

    [ObservableProperty]
    private Visibility addMode = Visibility.Visible;

    [ObservableProperty]
    private string? fullName;

    [ObservableProperty]
    private string? phone;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private bool isAdmin;

    [ObservableProperty]
    private bool isUpdater;

    [ObservableProperty]
    private string userName = "";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string? buttonText = "Thêm";

    [ObservableProperty]
    private bool buttonEnable = true;

    public void SetUserId(long Id)
    {
        UserId = Id;
        ButtonText = "Lưu";
        AddMode = Visibility.Collapsed;
    }

    public async Task GetUserInfo()
    {
        if (UserId != null)
        {
            var result = await client.Get(APIURLs.Users_UserDetail + "/" + UserId);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                return;
            }

            UserDto user = result.Data.DeserializeObject<UserDto>()!;
            FullName = user.FullName;
            Phone = user.Phone;
            Email = user.Email;
            IsAdmin = user.Roles.Any(e => e == (int)ERole.Admin);
            IsUpdater = user.Roles.Any(e => e == (int)ERole.Updater);
        }
    }


    public ICommand ChangingCommand { get; }

    private async Task ChangingAction()
    {
        if (ButtonEnable)
        {
            ButtonEnable = false;
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin");
                    ButtonEnable = true;
                });
                return;
            }

            if(UserId == null)
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Vui lòng nhập đủ thông tin");
                        ButtonEnable = true;
                    });
                    return;
                }
            }

            List<int> roles = new List<int>();
            if (IsAdmin) roles.Add((int)ERole.Admin);
            if (IsUpdater) roles.Add((int)ERole.Updater);

            if (UserId == null)
            {
                var result = await client.Post(APIURLs.Users_AddUser, new UserDto
                {
                    FullName = FullName,
                    Email = Email,
                    Phone = Phone,
                    UserName = UserName,
                    Password = Password,
                    Roles = roles
                });
                if (result.Status != Enums.EAPIStatus.Success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(result.Msg);
                        ButtonEnable = true;
                    });
                    return;
                }
            }
            else
            {
                var result = await client.Put(APIURLs.Users_UpdateUser, new UserDto
                {
                    Id = (long)UserId,
                    FullName = FullName,
                    Email = Email,
                    Phone = Phone,
                    Roles = roles
                });
                if (result.Status != Enums.EAPIStatus.Success)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(result.Msg);
                        ButtonEnable = true;
                    });
                    return;
                }
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Đã {ButtonText} thành công nhân viên");
                mainWindow.BodyFrame.Content = App.serviceProvider.GetService<UsersPage>();
                mainWindowViewModel.CurrentPath = "Danh sách nhân viên";
            });
        }

        ButtonEnable = true;
    }

}
