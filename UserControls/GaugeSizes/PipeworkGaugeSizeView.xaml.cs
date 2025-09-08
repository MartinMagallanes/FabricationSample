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
    /// Interaction logic for PipeworkGaugeSizeView.xaml
    /// </summary>
    public partial class PipeworkGaugeSizeView : UserControl
    {
        private PipeworkGauge _gauge;
        private PipeworkGaugeSize _selectedGaugeSize;

        public PipeworkGaugeSizeView(PipeworkGauge gauge)
        {
            InitializeComponent();

            _gauge = gauge;
            LoadGaugeSizes();
        }

        public void LoadGaugeSizes()
        {
            dgPipeworkGaugeSizes.ItemsSource = new ObservableCollection<PipeworkGaugeSize>(_gauge.PipeworkSizes);
            dgPipeworkGaugeSizes.IsReadOnly = !FabricationManager.CurrentMaterial.CanChange;
        }


        private void deleteGaugeSize_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGaugeSize == null)
                return;

            if (MessageBox.Show("Confirm to Delete Gauge Size", "Delete Gauge Size",
         MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeletePipeworkGaugeSize(_gauge, _selectedGaugeSize))
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

            _selectedGaugeSize = e.AddedItems[0] as PipeworkGaugeSize;
        }


    }
}
