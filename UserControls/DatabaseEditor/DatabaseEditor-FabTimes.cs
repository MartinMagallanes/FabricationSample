using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using FabricationSample.Data;
using FabricationSample.FunctionExamples;
using FabricationSample.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FabricationSample.UserControls.DatabaseEditor
{
  /// <summary>
  /// Interaction logic for DatabaseEditor.xaml
  /// </summary>
  public partial class DatabaseEditor : UserControl
  {
    #region Private Members

    FabricationTimesTableBase _ft;
    ObservableCollection<ProductEntryGridItem> _lstFabTimes;

    #endregion

    #region Fabrication Times

    private bool _fabricationTimesAreLoading;

    private void tbiFabTimes_Loaded(object sender, RoutedEventArgs e)
    {
      LoadFabricationTimesTables();
      cmbFabTimesTable.SelectedIndex = -1;
      btnNewFabTableRow.IsEnabled = false;
      btnNewFabTableColumn.IsEnabled = false;
      btnNewFabTableProdEntry.IsEnabled = false;
      btnDeleteFabTable.IsEnabled = false;
      btnEditFabTable.IsEnabled = false;
    }

    private void LoadFabricationTimesTables()
    {
      _fabricationTimesAreLoading = true;
      ListCollectionView lcv = new ListCollectionView(new ObservableCollection<FabricationTimesTableBase>(Database.FabricationTimesTable.ToList()));
      lcv.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
      lcv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      lcv.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
      cmbFabTimesTable.ItemsSource = lcv;

      //Bind Supplier Ids
      cmbFabTableSupplierId.ItemsSource = new ObservableCollection<ProductSupplier>(ProductDatabase.Suppliers);
      cmbFabTableSupplierId.DisplayMemberPath = "Name";
      cmbFabTableSupplierId.SelectedValuePath = "Id";
      _fabricationTimesAreLoading = false;
    }

    private void cmbFabTableSupplierId_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _selectedSupplier = (ProductSupplier)cmbFabTableSupplierId.SelectedItem;
      _bgwFabtimes.RunWorkerAsync();
    }

    private void ExtractFabricationTableData()
    {
      if (_ft != null)
      {
        if (_ft.Type == TableType.ProductId)
        {
          prgFabTimes.Maximum = (_ft as FabricationTimesTable).Products.Count > 0 ? (_ft as FabricationTimesTable).Products.Count : 1;
          if (_lstFabTimes == null)
            _lstFabTimes = new ObservableCollection<ProductEntryGridItem>();
          else
            _lstFabTimes.Clear();

          //Populate List for grid binding
          foreach (ProductEntry prodEntry in (_ft as FabricationTimesTable).Products)
          {
            ProductEntryGridItem gridItem = new ProductEntryGridItem { Entry = prodEntry, SupplierId = "", ProductEntryValue = prodEntry.Value, Status = prodEntry.Status };
            _lstFabTimes.Add(gridItem);
          }

          ccFabricationTimes.Content = new ProductIdFabricationTimesView(_ft as FabricationTimesTable, _lstFabTimes);
          btnNewFabTableRow.IsEnabled = false;
          btnNewFabTableColumn.IsEnabled = false;
          btnNewFabTableProdEntry.IsEnabled = true;
        }
        else
        {
          ccFabricationTimes.Content = new BreakPointView((_ft as FabricationTimesTableWithBreakpoints).Table);
          btnNewFabTableRow.IsEnabled = true;
          btnNewFabTableColumn.IsEnabled = true;
          btnNewFabTableProdEntry.IsEnabled = false;
        }
      }
    }

    private void cmbFabTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0 && !_fabricationTimesAreLoading)
      {
        btnDeleteFabTable.IsEnabled = true;
        btnEditFabTable.IsEnabled = true;
        _ft = e.AddedItems[0] as FabricationTimesTableBase;
        ExtractFabricationTableData();
      }
    }

    private void btnDeleteFabTable_Click(object sender, RoutedEventArgs e)
    {
      if (_ft == null)
        return;

      if (MessageBox.Show("Confirm to Delete Fabrication Time Table", "Delete Table",
          MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
      {
        if (FabricationAPIExamples.DeleteFabricationTimesTable(_ft))
        {
          _ft = null;
          ccFabricationTimes.Content = null;
          LoadFabricationTimesTables();
          cmbFabTimesTable.SelectedIndex = -1;
          btnNewFabTableRow.IsEnabled = false;
          btnNewFabTableColumn.IsEnabled = false;
          btnNewFabTableProdEntry.IsEnabled = false;
          btnDeleteFabTable.IsEnabled = false;
          btnEditFabTable.IsEnabled = false;
        }
      }
    }

    private void btnEditFabTable_Click(object sender, RoutedEventArgs e)
    {
      if (_ft == null)
        return;

      EditNameWindow win = new EditNameWindow("Edit Table", _ft.Name, _ft.Group);
      win.ShowDialog();
      if (win.Completed)
      {
        cmbFabTimesTable.IsEditable = true;
        _ft.Name = win.NewName;
        _ft.Group = win.NewGroup;

        var lcv = (cmbFabTimesTable.ItemsSource as ListCollectionView);
        lcv.Refresh();

        cmbFabTimesTable.Items.Refresh();
        cmbFabTimesTable.UpdateLayout();
        cmbFabTimesTable.IsEditable = false;
      }
    }

    private void btnNewFabTableProdEntry_Click(object sender, RoutedEventArgs e)
    {
      if (_ft != null)
      {
        AddFabricationTimesEntryWindow win = new AddFabricationTimesEntryWindow(_ft as FabricationTimesTable);
        win.ShowDialog();

        ProductEntry prodEntry = win.ProductEntry;
        if (prodEntry != null)
        {
          ProductEntryGridItem gridItem =
            new ProductEntryGridItem
            {
              Entry = prodEntry,
              SupplierId = "",
              ProductEntryValue = prodEntry.Value,
              Status = prodEntry.Status,
              date = prodEntry.Date
            };
          _lstFabTimes.Add(gridItem);
        }
      }
      else
        MessageBox.Show("No Fabrication Table Selected", "Add Product Entry", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    private void btnNewfabricationTable_Click(object sender, RoutedEventArgs e)
    {
      AddFabricationTimesTableWindow win = new AddFabricationTimesTableWindow(_ft);
      win.ShowDialog();
      var newFt = win.FabricationTimesTable;

      if (newFt != null)
      {
        _ft = newFt;
        LoadFabricationTimesTables();
        ExtractFabricationTableData();
        ListCollectionView lcv = cmbFabTimesTable.ItemsSource as ListCollectionView;
        for (int i = 0; i < lcv.Count; i++)
        {
          FabricationTimesTableBase ft = lcv.GetItemAt(i) as FabricationTimesTableBase;
          if (ft.Name == _ft.Name && ft.Group == _ft.Group)
          {
            cmbFabTimesTable.SelectedIndex = i;
            break;
          }
        }

      }
    }

    private void btnNewFabTableColumn_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.BreakPointPriceListView == null)
        return;

      if (FabricationAPIExamples.AddBreakPointColumn((_ft as FabricationTimesTableWithBreakpoints).Table, 0.0, false))
        FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
    }

    private void btnNewFabTableRow_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.BreakPointPriceListView == null)
        return;

      if (FabricationAPIExamples.AddBreakPointRow((_ft as FabricationTimesTableWithBreakpoints).Table, 0.0, false))
        FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
    }

    private void btnUpdateFabricationTimes_Click(object sender, RoutedEventArgs e)
    {

      try
      {
        //Save Fabrication Times
        DBOperationResult result = Database.SaveFabricationTimes();
        MessageBox.Show(result.Message, result.Status == ResultStatus.Succeeded ? "Operation Complete" : "Operation Failed",
          MessageBoxButton.OK, result.Status == ResultStatus.Succeeded ? MessageBoxImage.Information : MessageBoxImage.Exclamation);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error\nException Type: " + ex.GetType().FullName + "\nException Message: " + ex.Message, "Addin Handled Error",
          MessageBoxButton.OK, MessageBoxImage.Exclamation);

      }

    }
    #endregion
  }
}


    