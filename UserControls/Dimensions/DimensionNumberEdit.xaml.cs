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

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using FabricationSample.UserControls.ItemEditor;

using FabricationSample.Manager;

namespace FabricationSample.UserControls.Dimensions
{
  /// <summary>
  /// Interaction logic for OptionNumberEdit.xaml
  /// </summary>
  public partial class DimensionNumberEdit : UserControl
  {
    Item _itm;
    ItemDimension _dim;

    public DimensionNumberEdit(Item itm, ItemDimension dim)
    {
      _itm = itm;
      _dim = dim;
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      txtValue.Text = string.Format("{0:00}", _dim.Value.ToString());
      chkLock.IsChecked = (bool)_dim.IsLocked;
    }

    private void btnUpdateDimension_Click(object sender, RoutedEventArgs e)
    {

      double newValue = 0;
      if (double.TryParse(txtValue.Text.Trim(), out newValue))
      {
        _dim.Value = newValue;
      }
      else
      {
        System.Windows.MessageBox.Show("Incorrect value type", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }

      _dim.IsLocked = chkLock.IsChecked.Value;
      string name = _dim.Name;
      FabricationManager.ItemEditor.ParseDimensions();

      if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
        Autodesk.Fabrication.UI.UIApplication.UpdateView(new List<Item>() { _itm });

      // reassign the dim
      _dim = _itm.Dimensions.FirstOrDefault(x => x is ItemDimension && x.Name == name) as ItemDimension;
    }
  }
}
