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

using FabricationSample.Data;
using FabricationSample.Manager;

namespace FabricationSample.UserControls.ItemEditor.ProductList
{
  /// <summary>
  /// Interaction logic for AddProductDataEntry.xaml
  /// </summary>
  public partial class AddProductDataBoughtOutEntry : UserControl
  {

    public AddProductDataBoughtOutEntry()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void btnAddField_Click(object sender, RoutedEventArgs e)
    {
      ComboBoxItem itm = cmbBoughtOut.SelectedItem as ComboBoxItem;
      bool yesNo = itm.Content.ToString() == "Yes";
      FabricationManager.CurrentDataField.Template.SetBoughtOut(yesNo);
      FabricationManager.ItemEditor.FinshEditingProductDataField();
    }

    private void btnCancelAddField_Click(object sender, RoutedEventArgs e)
    {
      FabricationManager.ItemEditor.FinshEditingProductDataField();
    }

  }
}
