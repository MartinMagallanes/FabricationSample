using Autodesk.Fabrication.DB;
using FabricationSample.Data;
using FabricationSample.FunctionExamples;
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

namespace FabricationSample
{
  /// <summary>
  /// Interaction logic for AddPriceListEntryWindow.xaml
  /// </summary>
  public partial class AddPriceListEntryWindow : Window
  {
    #region Private Members

    PriceList _priceList;
    ProductEntry _prodEntry;

    #endregion

    #region Public Properties

    public ProductEntry ProductEntry { get { return _prodEntry; } }

    #endregion

    #region ctor

    public AddPriceListEntryWindow(PriceList priceList)
    {
      InitializeComponent();

      _priceList = priceList;
      _prodEntry = null;
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
    }

    private void btnAddPriceListEntry_Click(object sender, RoutedEventArgs e)
    {
      string newEntryId = txtDatabaseId.Text;
      if (!string.IsNullOrWhiteSpace(newEntryId))
      {
        _prodEntry = FabricationAPIExamples.AddPriceListEntry(_priceList, newEntryId);
        this.Close();
      }
      else
      {
        txtDatabaseId.Focus();
        MessageBox.Show("Database Id can not be empty", "Add Product Entry", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }
    }

    #endregion
  }
}
