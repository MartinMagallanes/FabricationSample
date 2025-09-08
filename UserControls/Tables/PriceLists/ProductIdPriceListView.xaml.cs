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
using Autodesk.Fabrication.DB;
using FabricationSample.Data;
using FabricationSample.Manager;
using FabricationSample.FunctionExamples;

namespace FabricationSample.UserControls
{

    /// <summary>
    /// Interaction logic for ProductIdPriceListView.xaml
    /// </summary>
    public partial class ProductIdPriceListView : UserControl
    {

        #region Private Members

        ObservableCollection<ProductEntryGridItem> _data;
        PriceList _pl;

        #endregion

        #region Public Members

        #endregion

        #region ctor

        public ProductIdPriceListView(PriceList pl, ObservableCollection<ProductEntryGridItem> data)
        {
            _data = data;
            _pl = pl;
            InitializeComponent();
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbProdEntryStatus.ItemsSource = Enum.GetNames(typeof(ProductEntryStatus));
            cmbProdEntryCostedBy.ItemsSource = Enum.GetNames(typeof(ProductEntryCostedBy));

            dgPrices.ItemsSource = _data;

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void deleteEntry_Click(object sender, RoutedEventArgs e)
        {
            var item = dgPrices.SelectedItem as ProductEntryGridItem;
            if (item == null)
                return;

            if (FabricationAPIExamples.DeletePriceListEntry(_pl, item.Entry))
            {
               _data.Remove(item);
            }
        }
    }
}

