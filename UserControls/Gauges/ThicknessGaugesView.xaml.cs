using Autodesk.Fabrication.DB;
using FabricationSample.FunctionExamples;
using FabricationSample.Manager;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            List<Gauge> gauges = material.Gauges.ToList();
            if (gauges.Count == 0)
            {
                return;
            }
            gauges = gauges.OrderBy(g => g.Thickness).ToList();

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

            switch (FabricationManager.CurrentGauge.Type)
            {
                default:
                    break;

                case MaterialType.Ductwork:
                    FabricationManager.DBEditor.GaugesSizesView.Content = new RoundGaugeSizeView(FabricationManager.CurrentGauge as MachineGauge);
                    break;

                case MaterialType.Pipework:
                    FabricationManager.DBEditor.GaugesSizesView.Content = new PipeworkGaugeSizeView(FabricationManager.CurrentGauge as PipeworkGauge);
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
