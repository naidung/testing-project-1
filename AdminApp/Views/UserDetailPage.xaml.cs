﻿using AdminApp.ViewModels;
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
/// Interaction logic for UserDetailPage.xaml
/// </summary>
public partial class UserDetailPage : Page
{
    private readonly UserDetailViewModel viewmodel;

    public UserDetailPage(UserDetailViewModel viewmodel)
    {
        InitializeComponent();
        this.viewmodel = viewmodel;
        DataContext = this.viewmodel = viewmodel;
    }

    public void GetUserInfo(long id)
    {
        viewmodel.SetUserId(id);
        _ = viewmodel.GetUserInfo();
    }

}
