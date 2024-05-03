using AdminApp.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AdminApp.ViewModels;

public partial class CategoriesSelectViewModel : ObservableObject
{
    public CategoriesSelectViewModel(CategoryDto category)
    {
        this.category = category;
        Name = category.Name;
    }

    public CategoriesSelectViewModel(CategoryDto category, bool isChecked)
    {
        this.category = category;
        this.IsChecked = isChecked;
        Name = category.Name;
    }

    public CategoryDto category;

    [ObservableProperty]
    private bool isChecked = true;

    [ObservableProperty]
    private string? name;

}
