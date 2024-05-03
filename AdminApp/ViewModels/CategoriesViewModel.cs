
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

public partial class CategoriesViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly IClientActions client;

    public CategoriesViewModel(MainWindow mainWindow, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.client = client;
        AddCategoryCommand = new RelayCommand(addCategoryAction);
    }


    public ICommand AddCategoryCommand { get; }

    private void addCategoryAction()
    {
        var categoryDetailPage = App.serviceProvider.GetService<CategoryDetailPage>();
        MainWindow.Instance.BodyFrame.Content = categoryDetailPage;
        categoryDetailPage!.PageInitialize(null);
        MainWindow.Instance.viewmodel.CurrentPath = "Thêm nhóm sản phẩm";
    }

    [ObservableProperty]
    private ObservableCollection<CategoryDto> models = new ObservableCollection<CategoryDto>();

    [ObservableProperty]
    private bool buttonAddEnable = true;

    [ObservableProperty]
    private bool isBusy;

    public async Task GetCategories()
    {
        var result = await client.Get(APIURLs.Categories_AllCategories);
        if(result.Status != Enums.EAPIStatus.Success)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
            return;
        }
        var data = result.Data.DeserializeObject<List<CategoryDto>>()!;
        data.ForEach(e => e.Stt = data.IndexOf(e) + 1);
        Models = new ObservableCollection<CategoryDto>(data);
    }

    public async Task DeleteItem(int id)
    {
        if (!IsBusy)
        {
            IsBusy = true;
            var result = await client.Delete(APIURLs.Categories_DeleteCategory + "/" + id);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                IsBusy = false;
                return;
            }

            Models.Remove(Models.Where(e => e.Id == id).First());

            Models.Remove(Models.Where(e => e.Id == id).First());
            var list = new List<CategoryDto>(Models);
            list.ForEach(e => e.Stt = list.IndexOf(e) + 1);
            Models = new ObservableCollection<CategoryDto>(list);

            IsBusy = false;
        }
    }

    

}
