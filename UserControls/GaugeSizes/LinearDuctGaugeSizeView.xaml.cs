using Autodesk.Fabrication.DB;
using FabricationSample.FunctionExamples;
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
  /// Interaction logic for LinearDuctGaugeSizeView.xaml
  /// </summary>
  public partial class LinearDuctGaugeSizeView : UserControl
  {
    private RoundDuctGauge _gauge;
    private RoundGaugeSize _selectedGaugeSize;

    public LinearDuctGaugeSizeView(RoundDuctGauge gauge)
    {
      InitializeComponent();

      _gauge = gauge;
      LoadGaugeSizes();
    }

    public void LoadGaugeSizes()
    {
        dgLinearDuctGaugeSizes.ItemsSource = new ObservableCollection<RoundGaugeSize>(_gauge.RoundDuctSizes);
      dgLinearDuctGaugeSizes.IsReadOnly = !FabricationManager.CurrentMaterial.CanChange;
    }


    private void deleteGaugeSize_Click(object sender, RoutedEventArgs e)
    {
      if (_selectedGaugeSize == null)
        return;

      if (MessageBox.Show("Confirm to Delete Gauge Size", "Delete Gauge Size",
   MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
      {
        if (FabricationAPIExamples.DeleteRoundDuctGaugeSize(_gauge, _selectedGaugeSize))
          LoadGaugeSizes();
      }
    }

    private void dgGaugeSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count == 0)
      {
        _selectedGaugeSize = null;
        return;
      }

      _selectedGaugeSize = e.AddedItems[0] as RoundGaugeSize;
    }


  }
}
