using AdminApp.Dtos;
using AdminApp.Extensions;
using AdminApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AdminApp.ViewModels;

public partial class CategoryDetailViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;
    private readonly IClientActions client;

    public CategoryDetailViewModel(MainWindow mainWindow, MainWindowViewModel mainWindowViewModel, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
        this.client = client;
        ChangingCommand = new RelayCommand(() =>
        {
            _ = ChangingAction();
        });
    }

    private long? CategoryId { get; set; }

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private int? parentId;

    [ObservableProperty]
    private ObservableCollection<CategoryDto> categories = new();

    private int selectedIndex = -1;
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            if (value >= 0)
            {
                if (Categories.Count > value)
                {
                    ParentId = Categories[value].Id;
                }
            }
            SetProperty<int>(ref selectedIndex, value);
        }
    }

    [ObservableProperty]
    private string? buttonText = "Thêm";

    [ObservableProperty]
    private bool buttonEnable = true;

    public void SetCategoryId(int? Id)
    {
        CategoryId = Id;
        if (Id != null)
            ButtonText = "Lưu";
    }


    public async Task PageInitialize()
    {
        await GetCategories();
        if (CategoryId != null)
        {
            await GetCategoryInfo();
            var selectedItem = Categories.Where(e => e.Id == ParentId).FirstOrDefault();
            SelectedIndex = selectedItem != null ? Categories.IndexOf(selectedItem) : -1;
        }
    }

    public async Task GetCategories()
    {
        var result = await client.Get(APIURLs.Categories_AllCategories);
        if (result.Status != Enums.EAPIStatus.Success)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
            return;
        }
        var data = result.Data.DeserializeObject<List<CategoryDto>>()!;
        data.ForEach(e => e.Stt = data.IndexOf(e) + 1);
        Categories = new ObservableCollection<CategoryDto>(data);
    }

    public async Task GetCategoryInfo()
    {
        if (CategoryId != null)
        {
            var result = await client.Get(APIURLs.Categories_CategoryDetail + "/" + CategoryId);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                return;
            }

            CategoryDto category = result.Data.DeserializeObject<CategoryDto>()!;
            Name = category.Name;
            ParentId = category.ParentId;
        }
    }


    public ICommand ChangingCommand { get; }

    private async Task ChangingAction()
    {
        if (ButtonEnable)
        {
            ButtonEnable = false;
            if (string.IsNullOrEmpty(Name))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin");
                    ButtonEnable = true;
                });
                return;
            }

            //add
            if (CategoryId == null)
            {
                var result = await client.Post(APIURLs.Categories_AddCategory, new CategoryDto
                {
                    Name = Name,
                    ParentId = ParentId
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
                var result = await client.Put(APIURLs.Categories_UpdateCategory, new CategoryDto
                {
                    Id = (int)CategoryId,
                    Name = Name,
                    ParentId = ParentId
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
                MessageBox.Show($"Đã {ButtonText} thành công nhóm sản phẩm");
                mainWindow.BodyFrame.Content = App.serviceProvider.GetService<CategoriesPage>();
                mainWindowViewModel.CurrentPath = "Danh sách nhóm sản phẩm";
            });
        }

        ButtonEnable = true;
    }

}
