
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

public partial class ProductsViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly IClientActions client;

    public ProductsViewModel(MainWindow mainWindow, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.client = client;
        AddProductCommand = new RelayCommand(addProductAction);
        ApplySearchCommand = new RelayCommand(ApplySearchAction);
        _ = GetProducts();
        _ = GetCategories();
    }


    public ICommand AddProductCommand { get; }

    private void addProductAction()
    {
        var detailPage = App.serviceProvider.GetService<ProductDetailPage>();
        MainWindow.Instance.BodyFrame.Content = detailPage;
        MainWindow.Instance.viewmodel.CurrentPath = "Thêm sản phẩm";
        detailPage!.PageInitialize(null);
    }

    [ObservableProperty]
    private ObservableCollection<ProductDto> models = new ObservableCollection<ProductDto>();

    [ObservableProperty]
    private ObservableCollection<CategoriesSelectViewModel> categoryModels = new ObservableCollection<CategoriesSelectViewModel>();

    [ObservableProperty]
    private bool isButtonEnable = true;

    [ObservableProperty]
    private bool isBusy;

    public async Task GetProducts(string? url = null)
    {
        var result = await client.Get(url??APIURLs.Products_AllProducts);
        if(result.Status != Enums.EAPIStatus.Success)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
            return;
        }
        var data = result.Data.DeserializeObject<ProductQueryDto>()!;
        data.Products.ForEach(e => e.Stt = data.Products.IndexOf(e) + 1);
        Models = new ObservableCollection<ProductDto>(data.Products);
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
        CategoryModels = new ObservableCollection<CategoriesSelectViewModel>();
        data.ForEach(e => CategoryModels.Add(new CategoriesSelectViewModel(e)));
    }

    public async Task DeleteItem(long id)
    {
        if (!IsBusy)
        {
            IsBusy = true;
            var result = await client.Delete(APIURLs.Products_DeleteProduct + "/" + id);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                IsBusy = false;
                return;
            }

            Models.Remove(Models.Where(e => e.Id == id).First());

            IsBusy = false;
        }
    }

    public ICommand ApplySearchCommand { get; }

    private async void ApplySearchAction()
    {
        if (IsButtonEnable)
        {
            IsButtonEnable = false;
            List<int> cateIds = CategoryModels.Where(e=> e.IsChecked).Select(e=> e.category.Id).ToList();
            string url = APIURLs.Products_AllProducts;
            if (cateIds.Count > 0)
            {
                url += $"?cateIds={string.Join(",", cateIds)}";
            }
            else
            {
                url += $"?cateIds=,";
            }
            await GetProducts(url);
            IsButtonEnable = true;
        }
    }

}
