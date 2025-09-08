using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
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

namespace FabricationSample.UserControls.DatabaseEditor
{
    /// <summary>
    /// Interaction logic for DatabaseEditor.xaml
    /// </summary>
    public partial class DatabaseEditor : UserControl
    {
        #region Private Members

        SupplierGroup _sg;
        PriceListBase _pl;
        ObservableCollection<ProductEntryGridItem> _lstPrices;

        #endregion

        #region Pricelists

        private void dgPrices_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void tbiPrices_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSupplierGroup.ItemsSource = new ObservableCollection<SupplierGroup>(Database.SupplierGroups.OrderBy(x => x.Name));
            cmbSupplierId.ItemsSource = new ObservableCollection<ProductSupplier>(ProductDatabase.Suppliers);
            cmbSupplierId.DisplayMemberPath = "Name";
            cmbSupplierId.SelectedValuePath = "Id";
            LoadSupplierGroups();
        }

        private void LoadSupplierGroups()
        {
            _sg = null;
            cmbPriceList.Items.Clear();

            cmbSupplierGroup.SelectedIndex = -1;
            btnAddPriceList.IsEnabled = false;
            btnAddProductEntry.IsEnabled = false;
            btnAddBreakPointColumn.IsEnabled = false;
            btnAddBreakPointRow.IsEnabled = false;
            btnDeletePriceList.IsEnabled = false;
            btnDeleteSupplierGroup.IsEnabled = false;
            btnEditSupplierGroup.IsEnabled = false;
            btnEditPriceList.IsEnabled = false;

            ccPriceListTable.Content = null;
        }

        private void cmbSupplierGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ccPriceListTable.Content = null;

            cmbPriceList.Items.Clear();

            if (e.AddedItems.Count > 0)
            {
                _sg = e.AddedItems[0] as SupplierGroup;

                if (_sg != null)
                {
                    foreach (PriceListBase pl in _sg.PriceLists)
                    {
                        cmbPriceList.Items.Add(pl.Name);
                    }

                    btnAddPriceList.IsEnabled = true;
                    btnDeleteSupplierGroup.IsEnabled = true;
                    btnEditSupplierGroup.IsEnabled = true;
                }
            }
        }

        private void cmbPriceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                btnEditPriceList.IsEnabled = true;
                btnDeletePriceList.IsEnabled = true;

                string selection = e.AddedItems[0].ToString();

                _pl = _sg.PriceLists.FirstOrDefault(x => x.Name == selection) as PriceListBase;

                if (_pl != null)
                {
                    //Handle Product Id Tables
                    if (_pl.Type == TableType.ProductId)
                    {
                        //Cast to Product Based Price List
                        PriceList pl = _pl as PriceList;

                        if (_lstPrices == null)
                            _lstPrices = new ObservableCollection<ProductEntryGridItem>();
                        else
                            _lstPrices.Clear();

                        foreach (ProductEntry prodEntry in pl.Products)
                        {
                            ProductEntryGridItem gridItem = new ProductEntryGridItem(prodEntry, _sg);

                            _lstPrices.Add(gridItem);
                        }

                        ProductIdPriceListView plIdView = new ProductIdPriceListView(pl, _lstPrices);
                        FabricationManager.ProductIdPriceListView = plIdView;
                        FabricationManager.BreakPointPriceListView = null;
                        ccPriceListTable.Content = FabricationManager.ProductIdPriceListView;

                        btnAddProductEntry.IsEnabled = true;
                        btnAddBreakPointColumn.IsEnabled = false;
                        btnAddBreakPointRow.IsEnabled = false;
                    }
                    //Handle BreakPoint Tables
                    else
                    {
                        BreakPointView plBpView = new BreakPointView((_pl as PriceListWithBreakPoints).DefaultTable);
                        FabricationManager.ProductIdPriceListView = null;
                        ccPriceListTable.Content = plBpView;

                        btnAddProductEntry.IsEnabled = false;
                        btnAddBreakPointColumn.IsEnabled = true;
                        btnAddBreakPointRow.IsEnabled = true;
                    }

                }
            }
        }

        private void cmbSupplierId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_pl.Type == TableType.ProductId)
            {
                if (e.AddedItems.Count > 0 && FabricationManager.ProductIdPriceListView.dgPrices.ItemsSource != null && !_bgwPrices.IsBusy)
                {
                    _selectedSupplier = (ProductSupplier)e.AddedItems[0];
                    prgPriceList.Maximum = (_pl as PriceList).Products.Count;

                    _bgwPrices.RunWorkerAsync();
                }
            }
        }

        private void btnUpdatePrices_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //Save Prices
                DBOperationResult result = Database.SaveProductCosts();
                MessageBox.Show(result.Message, result.Status == ResultStatus.Succeeded ? "Operation Complete" : "Operation Failed",
                  MessageBoxButton.OK, result.Status == ResultStatus.Succeeded ? MessageBoxImage.Information : MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error\nException Type: " + ex.GetType().FullName + "\nException Message: " + ex.Message, "Addin Handled Error",
                  MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        private void btnAddProductEntry_Click(object sender, RoutedEventArgs e)
        {
            if (_pl != null && _pl.Type == TableType.ProductId)
            {
                AddPriceListEntryWindow win = new AddPriceListEntryWindow(_pl as PriceList);
                win.ShowDialog();

                ProductEntry prodEntry = win.ProductEntry;
                if (prodEntry != null)
                {
                    ProductEntryGridItem gridItem = new ProductEntryGridItem { Entry = prodEntry, SupplierId = "", ProductEntryValue = prodEntry.Value, Status = prodEntry.Status, date = prodEntry.Date };
                    _lstPrices.Add(gridItem);
                }
            }
            else
                MessageBox.Show("No Price List Selected", "Add Product Entry", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void btnAddPriceList_Click(object sender, RoutedEventArgs e)
        {
            AddPriceListWindow win = new AddPriceListWindow(_sg);
            win.ShowDialog();

            PriceListBase priceList = win.PriceList;
            if (priceList != null)
            {
                _pl = priceList;
                cmbPriceList.Items.Add(_pl.Name);

                cmbPriceList.SelectedIndex = cmbPriceList.Items.Count - 1;

                //Handle Product Id Tables
                if (_pl.Type == TableType.ProductId)
                {
                    //Cast to Product Based Price List
                    PriceList pl = _pl as PriceList;

                    if (_lstPrices == null)
                        _lstPrices = new ObservableCollection<ProductEntryGridItem>();
                    else
                        _lstPrices.Clear();

                    foreach (ProductEntry prodEntry in pl.Products)
                    {
                        ProductEntryGridItem gridItem = new ProductEntryGridItem(prodEntry, _sg);
                        _lstPrices.Add(gridItem);
                    }

                    ProductIdPriceListView plIdView = new ProductIdPriceListView(pl, _lstPrices);
                    FabricationManager.ProductIdPriceListView = plIdView;
                    FabricationManager.BreakPointPriceListView = null;
                    ccPriceListTable.Content = FabricationManager.ProductIdPriceListView;
                }
                //Handle BreakPoint Tables
                else
                {
                    BreakPointView plBpView = new BreakPointView((_pl as PriceListWithBreakPoints).DefaultTable);
                    FabricationManager.ProductIdPriceListView = null;
                    ccPriceListTable.Content = plBpView;
                }

            }

        }

        private void btnAddSupplierGroup_Click(object sender, RoutedEventArgs e)
        {
            EditNameWindow win = new EditNameWindow("Add Supplier Group", "", null);
            win.ShowDialog();
            string name = win.NewName;
            if (String.IsNullOrWhiteSpace(name))
                return;

            if (FabricationAPIExamples.AddNewSupplierGroup(name) != null)
            {
                Database.SaveProductCosts();
                cmbSupplierGroup.ItemsSource = new ObservableCollection<SupplierGroup>(Database.SupplierGroups);
                _sg = Database.SupplierGroups.FirstOrDefault(x => x.Name.Equals(name));
                cmbSupplierGroup.SelectedItem = _sg;
            }
        }

        private void btnAddBreakPointColumn_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.BreakPointPriceListView == null)
                return;

            if (FabricationAPIExamples.AddBreakPointColumn((_pl as PriceListWithBreakPoints).DefaultTable, 0.0, false))
                FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
        }

        private void btnAddBreakPointRow_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationAPIExamples.AddBreakPointRow((_pl as PriceListWithBreakPoints).DefaultTable, 0.0, false))
                FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
        }

        private void btnDeletePriceList_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm to Delete Price List", "Delete Price List",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeletePriceList(_sg, _pl))
                {
                    _pl = null;
                    ccPriceListTable.Content = null;
                    cmbPriceList.Items.Clear();
                    foreach (PriceListBase pl in _sg.PriceLists)
                    {
                        cmbPriceList.Items.Add(pl.Name);
                    }
                }
            }
        }

        private void btnDeleteSupplierGroup_Click(object sender, RoutedEventArgs e)
        {
            if (_sg == null)
                return;

            if (FabricationAPIExamples.DeleteSupplierGroup(_sg))
            {
                var collection = cmbSupplierGroup.ItemsSource as ObservableCollection<SupplierGroup>;
                collection.Remove(_sg);

                LoadSupplierGroups();
            }
        }

        private void btnEditSupplierGroup_Click(object sender, RoutedEventArgs e)
        {
            if (_sg == null)
                return;

            EditNameWindow win = new EditNameWindow("Edit Supplier Group", _sg.Name, null);
            win.ShowDialog();
            if (win.Completed)
            {
                _sg.Name = win.NewName;
                cmbSupplierGroup.IsEditable = true;
                cmbSupplierGroup.Text = win.NewName;
                cmbSupplierGroup.Items.Refresh();
                cmbSupplierGroup.UpdateLayout();
                cmbSupplierGroup.IsEditable = false;
            }
        }

        private void btnEditPriceList_Click(object sender, RoutedEventArgs e)
        {
            if (_pl == null)
                return;

            EditNameWindow win = new EditNameWindow("Edit Price List", _pl.Name, null);
            win.ShowDialog();
            if (win.Completed)
            {
                _pl.Name = win.NewName;
                cmbPriceList.Items.Clear();
                int index = 0, count = 0;
                foreach (PriceListBase pl in _sg.PriceLists)
                {
                    cmbPriceList.Items.Add(pl.Name);
                    if (pl.Name.Equals(win.NewName))
                        index = count;

                    count++;
                }

                cmbPriceList.SelectedIndex = index;
            }
        }

        #endregion
    }
}
