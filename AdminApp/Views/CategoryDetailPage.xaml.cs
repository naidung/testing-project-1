using AdminApp.Dtos;
using AdminApp.ViewModels;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdminApp.Views;

/// <summary>
/// Interaction logic for CategoryDetailPage.xaml
/// </summary>
public partial class CategoryDetailPage : Page
{
    private readonly CategoryDetailViewModel viewmodel;

    public CategoryDetailPage(CategoryDetailViewModel viewmodel)
    {
        InitializeComponent();
        DataContext = this.viewmodel = viewmodel;
    }

    public void PageInitialize(int? id)
    {
        viewmodel.SetCategoryId(id);
        _ = viewmodel.PageInitialize();
    }

    private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var selectedIndex = cmbCategories.SelectedIndex;
            CategoryDto selectedCategory = viewmodel.Categories[selectedIndex];
            viewmodel.ParentId = selectedCategory.Id;
        }
        catch { }
    }

    private void btnDeleteParent_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        viewmodel.SelectedIndex = -1;
        viewmodel.ParentId = null;
    }
}
