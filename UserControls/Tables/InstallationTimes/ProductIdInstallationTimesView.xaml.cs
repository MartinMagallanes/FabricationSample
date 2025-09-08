using FabricationSample.Data;
using FabricationSample.Manager;
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

namespace FabricationSample.UserControls
{
  /// <summary>
  /// Interaction logic for ProductIdInstallationTimesView.xaml
  /// </summary>
  public partial class ProductIdInstallationTimesView : UserControl
  {
    private ObservableCollection<ProductEntryGridItem> _items;

    public ProductIdInstallationTimesView(ObservableCollection<ProductEntryGridItem> items)
    {
      InitializeComponent();

      _items = items;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      dgInstallationTimes.ItemsSource = _items;

      cmbInstallationTableProdEntryCostedBy.ItemsSource = Enum.GetNames(typeof(ProductEntryCostedBy));

      FabricationManager.ProductIdInstallationTimesView = this;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {

    }
    private void deleteEntry_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
