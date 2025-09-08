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

using FabricationSample.UserControls.ItemEditor;

namespace FabricationSample.UserControls.Options
{
  /// <summary>
  /// Interaction logic for OptionSelectEdit.xaml
  /// </summary>
  public partial class OptionSelectEdit : UserControl
  {
    Item _itm;
    ItemSelectOption _opt;

    public OptionSelectEdit(Item itm, ItemSelectOption opt)
    {
      _itm = itm;
      _opt = opt;
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      ItemOptionEntry optEntry = _opt.Options.ToList().FirstOrDefault(x => x.IsSelected);
      cmbOption.ItemsSource = new ObservableCollection<ItemOptionEntry>(_opt.Options);
      cmbOption.DisplayMemberPath = "Name";
      cmbOption.SelectedItem = optEntry;

      chkLock.IsChecked = (bool)_opt.IsLocked;
    }

    private void cmbOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void btnUpdatePrices_Click(object sender, RoutedEventArgs e)
    {
      ItemOptionEntry optEntry = (ItemOptionEntry)cmbOption.SelectedItem;
      optEntry.IsSelected = true;

      _opt.IsLocked = chkLock.IsChecked.Value;
      _itm.Update();

      FabricationManager.ItemEditor.ParseDimensions();
      FabricationManager.ItemEditor.ParseOptions();

      if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
        Autodesk.Fabrication.UI.UIApplication.UpdateView(new List<Item>() { _itm });
    }
  }
}
