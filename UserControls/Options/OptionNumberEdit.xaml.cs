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

namespace FabricationSample.UserControls.Options
{
  /// <summary>
  /// Interaction logic for OptionNumberEdit.xaml
  /// </summary>
  public partial class OptionNumberEdit : UserControl
  {
    Item _itm;
    ItemOptionBase _opt;

    public OptionNumberEdit(Item itm, ItemOptionBase opt)
    {
      _itm = itm;
      _opt = opt;
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      Type optType = _opt.GetType();

      chkLock.IsChecked = (bool)_opt.IsLocked;

      if (optType==typeof(ItemMinMaxNumericOption))
      {
        ItemMinMaxNumericOption nOpt = _opt as ItemMinMaxNumericOption;
        lblRange.Text += string.Format("{0:0.0}", nOpt.Lowest) + " and " + string.Format("{0:0.0}", nOpt.Highest);
        txtValue.Text = string.Format("{0:0.0}", _opt.Value.ToString());
      }
      else if (optType == typeof(ItemMinMaxIntegerOption))
      {
        ItemMinMaxIntegerOption nOpt = _opt as ItemMinMaxIntegerOption;
        lblRange.Text += string.Format("{0:0}", nOpt.Lowest) + " and " + string.Format("{0:0}", nOpt.Highest);
        txtValue.Text = string.Format("{0:0}", _opt.Value.ToString());
      }
      
    }

    private void btnUpdatePrices_Click(object sender, RoutedEventArgs e)
    {
      Type optType = _opt.GetType();

      if (optType == typeof(ItemMinMaxNumericOption))
      {
        double newValue = 0;
        if (double.TryParse(txtValue.Text.Trim(), out newValue))
        {
          ItemMinMaxNumericOption nOpt = _opt as ItemMinMaxNumericOption;
          if (newValue>=nOpt.Lowest && newValue<=nOpt.Highest)
          {
            nOpt.ChangeValue(newValue);
          }
          else
          {
            System.Windows.MessageBox.Show("New value out of range", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
          }
          
        }
        else
        {
          System.Windows.MessageBox.Show("Error parsing new value", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
      }
      else if (optType == typeof(ItemMinMaxIntegerOption))
      {
        int newValue = 0;
        if (int.TryParse(txtValue.Text.Trim(), out newValue))
        {
          ItemMinMaxIntegerOption nOpt = _opt as ItemMinMaxIntegerOption;
          if (newValue >= nOpt.Lowest && newValue <= nOpt.Highest)
          {
            nOpt.ChangeValue(newValue);
          }
          else
          {
            System.Windows.MessageBox.Show("New value out of range", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
          }
        }
        else
        {
          System.Windows.MessageBox.Show("Error parsing new value", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
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
