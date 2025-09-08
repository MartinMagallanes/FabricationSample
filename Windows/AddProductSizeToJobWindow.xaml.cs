using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;

namespace FabricationSample
{
  public partial class AddProductSizeToJobWindow : Window
  {
    #region ctor

    public AddProductSizeToJobWindow()
    {
      InitializeComponent();
    }

    #endregion

    #region Window Methods

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ButtonState == MouseButtonState.Pressed)
      {
        base.OnMouseLeftButtonDown(e);
        DragMove();
      }
    }

    private void CloseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.CurrentItem.IsProductList)
      {
        cmbProductSize.ItemsSource = new ObservableCollection<ItemProductListDataRow>(FabricationManager.CurrentItem.ProductList.Rows);
        cmbProductSize.DisplayMemberPath = "Name";
        cmbProductSize.SelectedIndex = 0;
      }
    }

    private void btnAddProductSize_Click(object sender, RoutedEventArgs e)
    {
      FabricationManager.CurrentItem.LoadProductListEntry(cmbProductSize.SelectedIndex);
      this.Close();
    }

    #endregion


  }
}
