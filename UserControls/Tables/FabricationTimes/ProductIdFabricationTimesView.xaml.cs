using Autodesk.Fabrication.DB;
using FabricationSample.Data;
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
    /// Interaction logic for ProductIdFabricationTimesView.xaml
    /// </summary>
    public partial class ProductIdFabricationTimesView : UserControl
    {
        private ObservableCollection<ProductEntryGridItem> _items;
        private FabricationTimesTable _ft;
        public ProductIdFabricationTimesView(FabricationTimesTable ft, ObservableCollection<ProductEntryGridItem> items)
        {
            _ft = ft;
            _items = items;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgFabTimes.ItemsSource = _items;

            cmbFabTableProdEntryCostedBy.ItemsSource = Enum.GetNames(typeof(ProductEntryCostedBy));

            FabricationManager.ProductIdFabricationTimesView = this;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void deleteEntry_Click(object sender, RoutedEventArgs e)
        {
            var item = dgFabTimes.SelectedItem as ProductEntryGridItem;
            if (item == null)
                return;

            if (FabricationAPIExamples.DeleteFabricationTimesTableEntry(_ft, item.Entry))
            {
               _items.Remove(item);
            }
        }
    }
}
