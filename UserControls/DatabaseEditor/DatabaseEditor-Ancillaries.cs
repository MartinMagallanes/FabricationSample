using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using FabricationSample.Data;
using FabricationSample.FunctionExamples;
using FabricationSample.Manager;
using FabricationSample.UserControls.Ancillaries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace FabricationSample.UserControls.DatabaseEditor
{
  /// <summary>
  /// Interaction logic for DatabaseEditor.xaml
  /// </summary>
  public partial class DatabaseEditor : UserControl
  {
    #region Ancillaries

    private void tbiAncillaries_Loaded(object sender, RoutedEventArgs e)
    {
      var ancillaryTypes = Enum.GetValues(typeof(AncillaryTypeEnum)).Cast<AncillaryTypeEnum>();
      ancillaryTypes = ancillaryTypes.OrderBy(x => x.ToString());

      cmbSelectAncillary.ItemsSource = ancillaryTypes;
      cmbSelectAncillary.SelectedIndex = 0;
    }
    private void editAncillary_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.CurrentAncillary == null)
        return;

      var win = new EditAncillaryWindow(FabricationManager.CurrentAncillary);
      win.ShowDialog();

      FabricationManager.AncillariesView.RefreshView();
    }

    private void addAncillary_Click(object sender, RoutedEventArgs e)
    {
      var selectedValue = (AncillaryTypeEnum)cmbSelectAncillary.SelectedValue;
      var win = new AddAncillaryWindow(selectedValue);
      win.ShowDialog();

      FabricationManager.AncillariesView.RefreshView();
    }

    private void deleteAncillary_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.CurrentAncillary == null)
        return;

      FabricationAPIExamples.DeleteAncillary(FabricationManager.CurrentAncillary);

      FabricationManager.AncillariesView.DeleteAncillary();

      FabricationManager.AncillariesView.RefreshView();
    }

    private void saveAncillaries_Click(object sender, RoutedEventArgs e)
    {
      FabricationAPIExamples.SaveAncillaries();
    }

    private void cmbSelectAncillary_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var combo = sender as ComboBox;
      var selectedValue = (AncillaryTypeEnum)combo.SelectedValue;
      var s = e.AddedItems[0];

      ccAncillaries.Content = new AncillariesView(selectedValue);
    }

    #endregion

  }
}


    