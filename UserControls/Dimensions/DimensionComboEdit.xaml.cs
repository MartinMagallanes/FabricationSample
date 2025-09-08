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

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using FabricationSample.UserControls.ItemEditor;

using FabricationSample.Manager;

namespace FabricationSample.UserControls.Dimensions
{
  /// <summary>
  /// Interaction logic for DimensionComboEdit.xaml
  /// </summary>
  public partial class DimensionComboEdit : UserControl
  {
    Item _itm;
    ItemComboDimension _dim;

    public DimensionComboEdit(Item itm, ItemComboDimension dim)
    {
      _itm = itm;
      _dim = dim;
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      ItemComboDimensionEntry dimEntry = _dim.Options.ToList().FirstOrDefault(x => x.IsSelected);

      txtValue.Text = "0.0";
      cmbDims.ItemsSource = new ObservableCollection<ItemComboDimensionEntry>(_dim.Options);
      cmbDims.DisplayMemberPath = "Name";

      chkLock.IsChecked = (bool)_dim.IsLocked;

      if (dimEntry.GetType() == typeof(ItemComboDimensionValueEntry))
      {
        txtValue.IsEnabled = true;
        ItemComboDimensionValueEntry valEntry = dimEntry as ItemComboDimensionValueEntry;
        txtValue.Text = valEntry.Value.ToString();
        cmbDims.SelectedItem = valEntry;
      }
      else
      {
        cmbDims.SelectedItem = dimEntry;
      }

    }

    private void cmbDims_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0)
      {
        ItemComboDimensionEntry optEntry = e.AddedItems[0] as ItemComboDimensionEntry;

        if (optEntry.GetType() == typeof(ItemComboDimensionValueEntry))
        {
          txtValue.IsEnabled = true;
        }
        else
        {
          txtValue.IsEnabled = false;
        }
      }
    }

    private void btnUpdateDimension_Click(object sender, RoutedEventArgs e)
    {
      ItemComboDimensionEntry dimEntry = (ItemComboDimensionEntry)cmbDims.SelectedItem;

      if (dimEntry.GetType() == typeof(ItemComboDimensionValueEntry))
      {
        ItemComboDimensionValueEntry valEntry = dimEntry as ItemComboDimensionValueEntry;
        valEntry.IsSelected = true;
        double newValue = 0;

        if (double.TryParse(txtValue.Text.Trim(), out newValue))
        {
          valEntry.Value = newValue;
        }
      }
      else
      {
        dimEntry.IsSelected = true;
      }
      string name =_dim.Name;
      _dim.IsLocked = chkLock.IsChecked.Value;

      FabricationManager.ItemEditor.ParseDimensions();

      if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
        Autodesk.Fabrication.UI.UIApplication.UpdateView(new List<Item>() { _itm });

      _dim = _itm.Dimensions.FirstOrDefault(x => x is ItemComboDimension && x.Name == name) as ItemComboDimension;
    }
  }
}
