using AdminApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdminApp.Views;

/// <summary>
/// Interaction logic for ProductDetailPage.xaml
/// </summary>
public partial class ProductDetailPage : Page
{
    ProductDetailViewModel viewmodel;

    public ProductDetailPage(ProductDetailViewModel viewmodel)
    {
        InitializeComponent();

        DataContext = this.viewmodel = viewmodel;
    }

    public void PageInitialize(int? id)
    {
        viewmodel.PageInitialize(id);
    }

}
