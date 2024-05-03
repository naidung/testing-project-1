using AdminApp.Dtos;
using AdminApp.Extensions;
using AdminApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PackingApp.Services.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminApp.ViewModels;

public partial class ProductDetailViewModel : ObservableObject
{
    private readonly MainWindow mainWindow;
    private readonly MainWindowViewModel mainWindowViewModel;
    private readonly IClientActions client;

    public ProductDetailViewModel(MainWindow mainWindow, MainWindowViewModel mainWindowViewModel, IClientActions client)
    {
        this.mainWindow = mainWindow;
        this.mainWindowViewModel = mainWindowViewModel;
        this.client = client;
        ChangingCommand = new RelayCommand(ChangingAction);
    }

    private int? ProductId { get; set; }

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? shortDescription;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private double? price;

    [ObservableProperty]
    private string tags = "";

    [ObservableProperty]
    private ObservableCollection<CategoriesSelectViewModel> categoriesSelect = new();

    [ObservableProperty]
    private string? buttonText = "Thêm";

    [ObservableProperty]
    private bool buttonEnable = true;

    public async void PageInitialize(int? Id = null)
    {
        await GetCategories();
        if (Id != null)
        {
            ProductId = Id;
            ButtonText = "Lưu";
        }
        await GetProductInfo();
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
        CategoriesSelect = new ObservableCollection<CategoriesSelectViewModel>();
        data.ForEach(e => CategoriesSelect.Add(new CategoriesSelectViewModel(e, false)));
    }

    public async Task GetProductInfo()
    {
        if (ProductId != null)
        {
            var result = await client.Get(APIURLs.Products_ProductDetail + "/" + ProductId);
            if (result.Status != Enums.EAPIStatus.Success)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(result.Msg));
                return;
            }

            ProductDto product = result.Data.DeserializeObject<ProductDto>()!;
            
            Name = product.Name;
            ShortDescription = product.ShortDescription;
            Description = product.Description;
            Price = product.Price;
            Tags = string.Join(" | ", product.Tags);

            foreach (var item in CategoriesSelect)
            {
                if(product.CategoryIds.Contains(item.category.Id))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }
    }


    public ICommand ChangingCommand { get; }

    private async void ChangingAction()
    {
        if (ButtonEnable)
        {
            ButtonEnable = false;
            if (string.IsNullOrEmpty(Name))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm");
                    ButtonEnable = true;
                });
                return;
            }

            List<string> prodTags = Tags.Split('|').Where(e => !string.IsNullOrEmpty(e)).Select(e => e.Trim()).ToList();

            //add new product
            if (ProductId == null)
            {
                var result = await client.Post(APIURLs.Products_AddProduct, new ProductDto
                {
                    Name = Name,
                    Price = Price,
                    ShortDescription = ShortDescription,
                    Description = Description,
                    CategoryIds = CategoriesSelect.Where(e=> e.IsChecked).Select(e=> e.category.Id).ToList(), 
                    Tags = prodTags
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
            //update product
            else
            {
                var result = await client.Put(APIURLs.Products_UpdateProduct, new ProductDto
                {
                    Id = (int)ProductId,
                    Name = Name,
                    Price = Price,
                    ShortDescription = ShortDescription,
                    Description = Description,
                    CategoryIds = CategoriesSelect.Where(e => e.IsChecked).Select(e => e.category.Id).ToList(),
                    Tags = prodTags
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
                MessageBox.Show($"Đã {ButtonText} thành công sản phẩm");
                mainWindow.BodyFrame.Content = App.serviceProvider.GetService<ProductsPage>();
                mainWindowViewModel.CurrentPath = "Danh sách sản phẩm";
            });
        }

        ButtonEnable = true;
    }

}
