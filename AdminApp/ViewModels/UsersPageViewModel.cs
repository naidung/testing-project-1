
using AdminApp.Dtos;
using AdminApp.Extensions;
using AdminApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AdminApp.ViewModels;

public partial class UsersPageViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly IClientActions client;

    public UsersPageViewModel(MainWindow mainWindow, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.client = client;
        AddUserCommand = new RelayCommand(addUserAction);
    }


    public ICommand AddUserCommand { get; }

    private void addUserAction()
    {
        MainWindow.Instance.BodyFrame.Content = App.serviceProvider.GetService<UserDetailPage>();
        MainWindow.Instance.viewmodel.CurrentPath = "Thêm nhân viên";
    }

    [ObservableProperty]
    private ObservableCollection<UserDto> models = new ObservableCollection<UserDto>();

    [ObservableProperty]
    private bool isBusy;

    public async Task GetUsers()
    {
        var result = await client.Get(APIURLs.Users_AllUsers);
        if(result.Status != Enums.EAPIStatus.Success)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
            return;
        }
        var data = result.Data.DeserializeObject<List<UserDto>>()!;
        data.ForEach(e => e.Stt = data.IndexOf(e) + 1);
        Models = new ObservableCollection<UserDto>(data);
    }

    public async Task DeleteItem(long id)
    {
        if (!IsBusy)
        {
            IsBusy = true;
            var result = await client.Delete(APIURLs.Users_DeleteUser + "/" + id);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                IsBusy = false;
                return;
            }

            Models.Remove(Models.Where(e => e.Id == id).First());
            var list = new List<UserDto>(Models);
            list.ForEach(e => e.Stt = list.IndexOf(e) + 1);
            Models = new ObservableCollection<UserDto>(list);

            IsBusy = false;
        }
    }

    

}
