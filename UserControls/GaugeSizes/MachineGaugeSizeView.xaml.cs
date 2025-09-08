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
    /// Interaction logic for RoundGaugeSizeView.xaml
    /// </summary>
    public partial class RoundGaugeSizeView : UserControl
    {
        private MachineGauge _gauge;
        private GaugeSize _selectedGaugeSize;

        public RoundGaugeSizeView(MachineGauge gauge)
        {
            InitializeComponent();

            _gauge = gauge;
            LoadGaugeSizes();
        }

        public int GetTabIndex()
        {
            return tbGaugeSize.SelectedIndex;
        }

        public void LoadGaugeSizes()
        {
            dgFlatBedGaugeSizes.ItemsSource = new ObservableCollection<RectangularGaugeSize>(_gauge.FlatBedSizes);
            dgRotaryGaugeSizes.ItemsSource = new ObservableCollection<RoundGaugeSize>(_gauge.RotarySizes);
            dgShearGaugeSizes.ItemsSource = new ObservableCollection<RectangularGaugeSize>(_gauge.ShearSizes);

            dgFlatBedGaugeSizes.IsReadOnly = !FabricationManager.CurrentMaterial.CanChange;
            dgRotaryGaugeSizes.IsReadOnly = !FabricationManager.CurrentMaterial.CanChange;
            dgShearGaugeSizes.IsReadOnly = !FabricationManager.CurrentMaterial.CanChange;
        }

        private void radioGroup_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedGaugeSize == null)
                return;

            int index = tbGaugeSize.SelectedIndex;
            switch (index)
            {
                default:
                    break;

                case 0: // flat bed
                    _gauge.DefaultFlatBedSize = _selectedGaugeSize as RectangularGaugeSize;
                    break;

                case 2: // shear
                    _gauge.DefaultShearSize = _selectedGaugeSize as RectangularGaugeSize;
                    break;
            }
        }

        private void deleteFlatBedGaugeSize_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGaugeSize == null)
                return;

            if (MessageBox.Show("Confirm to Delete Gauge Size", "Delete Gauge Size",
         MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteFlatBedGaugeSize(_gauge, _selectedGaugeSize as RectangularGaugeSize))
                    LoadGaugeSizes();
            }
        }

        private void deleteRotaryGaugeSize_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGaugeSize == null)
                return;

            if (MessageBox.Show("Confirm to Delete Gauge Size", "Delete Gauge Size",
         MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteRotaryGaugeSize(_gauge, _selectedGaugeSize as RoundGaugeSize))
                    LoadGaugeSizes();
            }
        }

        private void deleteShearGaugeSize_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGaugeSize == null)
                return;

            if (MessageBox.Show("Confirm to Delete Gauge Size", "Delete Gauge Size",
         MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteShearGaugeSize(_gauge, _selectedGaugeSize as RectangularGaugeSize))
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

            _selectedGaugeSize = e.AddedItems[0] as GaugeSize;
        }


    }
}
