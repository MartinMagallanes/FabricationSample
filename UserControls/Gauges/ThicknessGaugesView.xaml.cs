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

using FabricationSample.Manager;
using Autodesk.Fabrication.DB;
using FabricationSample.FunctionExamples;

namespace FabricationSample.UserControls
{
  /// <summary>
  /// Interaction logic for ThicknessGaugesView.xaml
  /// </summary>
  public partial class ThicknessGaugesView : UserControl
  {
    public ThicknessGaugesView()
    {
      InitializeComponent();

      LoadGauges();
    }

    private void LoadGauges()
    {
      Material material = FabricationManager.CurrentMaterial;
      if (material == null)
        return;

      FabricationManager.CurrentGauge = null;

      MaterialType materialType = FabricationManager.CurrentMaterial.Type;

      List<Gauge> gauges = material.Gauges.ToList();
      if (materialType == MaterialType.Ductwork)
        gauges = gauges.OrderBy(x => (x as MachineGauge).Thickness).ToList();
      else if (materialType == MaterialType.LinearDuct)
        gauges = gauges.OrderBy(x => (x as RoundDuctGauge).Thickness).ToList();
      else
        return;

      dgGauges.ItemsSource = new ObservableCollection<Gauge>(gauges);
    }

    public void deleteGauge_Click(object sender, RoutedEventArgs e)
    {
      Gauge gauge = dgGauges.SelectedItem as Gauge;
      if (gauge == null)
        return;

      Material material = FabricationManager.CurrentMaterial;
      if (material == null)
        return;

      if (MessageBox.Show("Confirm to Delete Gauge", "Delete Gauge",
          MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
      {
        if (FabricationAPIExamples.DeleteGauge(material, gauge))
        {
          LoadGauges();
        }
      }
    }

    private void dgGauges_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Material material = FabricationManager.CurrentMaterial;
      if (material == null)
        return;

      FabricationManager.CurrentGauge = e.AddedItems[0] as Gauge;

      switch (material.Type)
      {
        default:
          break;

        case MaterialType.Ductwork:
          FabricationManager.DBEditor.GaugesSizesView.Content = new MachineGaugeSizeView(FabricationManager.CurrentGauge as MachineGauge);
          break;

        case MaterialType.LinearDuct:
          FabricationManager.DBEditor.GaugesSizesView.Content = new LinearDuctGaugeSizeView(FabricationManager.CurrentGauge as RoundDuctGauge);
          break;
      }
    }

    private void dgGauges_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
      if (!(e.Column is DataGridTextColumn))
        return;

      var column = e.Column as DataGridTextColumn;
      if (!(column.Header is string))
        return;

      string header = column.Header as string;
      bool cancel = !(FabricationManager.CurrentMaterial.CanChange); 
      if (cancel)
      {
        if (header.Equals("Cost Per") || header.Equals("Weight Per"))
          cancel = false;
      }

      e.Cancel = cancel;
    }
  }
}
