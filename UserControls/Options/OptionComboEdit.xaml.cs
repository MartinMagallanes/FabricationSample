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

using FabricationSample.Manager;

namespace FabricationSample.UserControls.Options
{
  /// <summary>
  /// Interaction logic for OptionComboEdit.xaml
  /// </summary>
  public partial class OptionComboEdit : UserControl
  {
    Item _itm;
    ItemComboOption _opt;

    public OptionComboEdit(Item itm, ItemComboOption opt)
    {
      _itm = itm;
      _opt = opt;
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      ItemOptionEntry optEntry = _opt.Options.ToList().FirstOrDefault(x => x.IsSelected);

      txtValue.Text = "0.0";
      cmbOption.ItemsSource = new ObservableCollection<ItemOptionEntry>(_opt.Options);
      cmbOption.DisplayMemberPath = "Name";

      chkLock.IsChecked = (bool)_opt.IsLocked;

      if (optEntry.GetType() == typeof(ItemOptionValueEntry))
      {
        txtValue.IsEnabled = true;
        ItemOptionValueEntry valEntry = optEntry as ItemOptionValueEntry;
        txtValue.Text = valEntry.Value.ToString();
        cmbOption.SelectedItem = valEntry;
      }
      else
      {
        cmbOption.SelectedItem = optEntry;
      }



    }

    private void cmbOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0)
      {
        ItemOptionEntry optEntry = e.AddedItems[0] as ItemOptionEntry;

        if (optEntry.GetType() == typeof(ItemOptionValueEntry))
        {
          txtValue.IsEnabled = true;
        }
        else
        {
          txtValue.IsEnabled = false;
        }
      }
    }

    private void btnUpdatePrices_Click(object sender, RoutedEventArgs e)
    {
      ItemOptionEntry optEntry = (ItemOptionEntry)cmbOption.SelectedItem;

      if (optEntry.GetType() == typeof(ItemOptionValueEntry))
      {
        ItemOptionValueEntry valEntry = optEntry as ItemOptionValueEntry;
        valEntry.IsSelected = true;
        double newValue = 0;

        if (double.TryParse(txtValue.Text.Trim(), out newValue))
        {
          valEntry.Value = newValue;
        }
      }
      else
      {
        optEntry.IsSelected = true;
      }

      _opt.IsLocked = chkLock.IsChecked.Value;

      _itm.Update();

      FabricationManager.ItemEditor.ParseDimensions();
      FabricationManager.ItemEditor.ParseOptions();

      if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
        Autodesk.Fabrication.UI.UIApplication.UpdateView(new List<Item>() { _itm });

    }
  }
}
