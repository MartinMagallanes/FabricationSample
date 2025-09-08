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
    #region Installation Times

    InstallationTimesTableBase _it;
    ObservableCollection<ProductEntryGridItem> _lstInstallationTimes;
    private bool _installtionTimesAreLoading;

    private void tbiInstallationTimes_Loaded(object sender, RoutedEventArgs e)
    {
      LoadInstallationTimesTables();
      cmbInstallationTimesTable.SelectedIndex = -1;
      btnNewInstallTableRow.IsEnabled = false;
      btnNewInstallTableColumn.IsEnabled = false;
      btnNewInstallTableProdEntry.IsEnabled = false;
      btnDeleteInstallTable.IsEnabled = false;
      btnEditInstallTable.IsEnabled = false;
      cmbInstallTableSet.IsEnabled = false;
    }

    private void LoadInstallationTimesTables()
    {
      _installtionTimesAreLoading = true;
      ListCollectionView lcv = new ListCollectionView(new ObservableCollection<InstallationTimesTableBase>(Database.InstallationTimesTable.ToList()));
      lcv.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
      lcv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      lcv.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
      cmbInstallationTimesTable.ItemsSource = lcv;

      //Bind Supplier Ids
      cmbInstallTableSupplierId.ItemsSource = ProductDatabase.Suppliers;
      cmbInstallTableSupplierId.DisplayMemberPath = "Name";
      cmbInstallTableSupplierId.SelectedValuePath = "Id";
      _installtionTimesAreLoading = false;

    }

    private void cmbInstallTableSupplierId_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _selectedSupplier = (ProductSupplier)cmbInstallTableSupplierId.SelectedItem;
      if (_selectedSupplier != null)
        _bgwInstallationtimes.RunWorkerAsync();
    }

    private void cmbInstallTableSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      
      if (cmbInstallTableSet.SelectedItem != null)
      {
        var table = _it as InstallationTimesTableWithBreakpoints;
        table.CurrentSet = (int)cmbInstallTableSet.SelectedItem;
        ccInstallTimes.Content = new BreakPointView((table).Table);
      }
    }

    private void PopulateSets()
    {
      cmbInstallTableSet.Items.Clear();

      var table = _it as InstallationTimesTableWithBreakpoints;

      for (int i = 0; i < table.SetCount; i++)
      {
        cmbInstallTableSet.Items.Add(i + 1);
      }

      cmbInstallTableSet.SelectedItem = 1;
    }


    private void ExtractInstallationTableData()
    {
      if (_it != null)
      {
        if (_it.Type == TableType.ProductId)
        {
          prgInstallationTimes.Maximum = (_it as InstallationTimesTable).Products.Count > 0 ? (_it as InstallationTimesTable).Products.Count : 1;

          if (_lstInstallationTimes == null)
            _lstInstallationTimes = new ObservableCollection<ProductEntryGridItem>();

          else
            _lstInstallationTimes.Clear();

          //Populate List for grid binding
          foreach (ProductEntry prodEntry in (_it as InstallationTimesTable).Products)
          {
            ProductEntryGridItem gridItem = new ProductEntryGridItem { Entry = prodEntry, SupplierId = "", ProductEntryValue = prodEntry.Value, Status = prodEntry.Status };
            _lstInstallationTimes.Add(gridItem);
          }

          ccInstallTimes.Content = new ProductIdInstallationTimesView(_lstInstallationTimes);
          btnNewInstallTableRow.IsEnabled = false;
          btnNewInstallTableColumn.IsEnabled = false;
          btnNewInstallTableProdEntry.IsEnabled = true;
          cmbInstallTableSet.IsEnabled = false;
        }
        else
        {
          ccInstallTimes.Content = new BreakPointView((_it as InstallationTimesTableWithBreakpoints).Table);
          btnNewInstallTableRow.IsEnabled = true;
          btnNewInstallTableColumn.IsEnabled = true;
          btnNewInstallTableProdEntry.IsEnabled = false;
          cmbInstallTableSet.IsEnabled = true;
          PopulateSets();
        }
      }
    }

    private void cmbInstallationTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count > 0 && !_installtionTimesAreLoading)
      {
        btnDeleteInstallTable.IsEnabled = true;
        btnEditInstallTable.IsEnabled = true;
        _it = e.AddedItems[0] as InstallationTimesTableBase;
        ExtractInstallationTableData();
      }
    }

    private void btnDeleteInstallTable_Click(object sender, RoutedEventArgs e)
    {
      if (_it == null)
        return;

      if (MessageBox.Show("Confirm to Delete Installation Time Table", "Delete Table",
          MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
      {
        if (FabricationAPIExamples.DeleteInstallationTimesTable(_it))
        {
          _it = null;
          ccInstallTimes.Content = null;
          LoadInstallationTimesTables();
          cmbInstallationTimesTable.SelectedIndex = -1;
          btnNewInstallTableRow.IsEnabled = false;
          btnNewInstallTableColumn.IsEnabled = false;
          btnNewInstallTableProdEntry.IsEnabled = false;
          btnDeleteInstallTable.IsEnabled = false;
          btnEditInstallTable.IsEnabled = false;
        }
      }
    }

    private void btnNewInstallTable_Click(object sender, RoutedEventArgs e)
    {
      AddInstallTimesTableWindow win = new AddInstallTimesTableWindow(_it);
      win.ShowDialog();
      var newIt = win.InstallationTimesTable;

      if (newIt != null)
      {
        _it = newIt;
        LoadInstallationTimesTables();
        ExtractInstallationTableData();
        ListCollectionView lcv = cmbInstallationTimesTable.ItemsSource as ListCollectionView;
        for (int i = 0; i < lcv.Count; i++)
        {
          InstallationTimesTableBase it = lcv.GetItemAt(i) as InstallationTimesTableBase;
          if (it.Name == _it.Name && it.Group == _it.Group)
          {
            cmbInstallationTimesTable.SelectedIndex = i;
            break;
          }
        }
      }
    }

    private void btnEditInstallTable_Click(object sender, RoutedEventArgs e)
    {
      if (_it == null)
        return;

      EditNameWindow win = new EditNameWindow("Edit Table", _it.Name, _it.Group);
      win.ShowDialog();
      if (win.Completed)
      {
        cmbInstallationTimesTable.IsEditable = true;
        _it.Name = win.NewName;
        _it.Group = win.NewGroup;

        var lcv = (cmbInstallationTimesTable.ItemsSource as ListCollectionView);
        lcv.Refresh();
        cmbInstallationTimesTable.Items.Refresh();
        cmbInstallationTimesTable.UpdateLayout();
        cmbInstallationTimesTable.IsEditable = false;
      }
    }

    private void btnNewInstallTableColumn_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.BreakPointPriceListView == null)
        return;

      if (FabricationAPIExamples.AddBreakPointColumn((_it as InstallationTimesTableWithBreakpoints).Table, 0.0, false))
        FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
    }

    private void btnNewInstallTableRow_Click(object sender, RoutedEventArgs e)
    {
      if (FabricationManager.BreakPointPriceListView == null)
        return;

      if (FabricationAPIExamples.AddBreakPointRow((_it as InstallationTimesTableWithBreakpoints).Table, 0.0, false))
        FabricationManager.BreakPointPriceListView.LoadRowsAndColumns();
    }


    private void btnUpdateInstallationTimes_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        //Save Installation Times
        DBOperationResult result = Database.SaveInstallationTimes();
        MessageBox.Show(result.Message, result.Status == ResultStatus.Succeeded ? "Operation Complete" : "Operation Failed",
          MessageBoxButton.OK, result.Status == ResultStatus.Succeeded ? MessageBoxImage.Information : MessageBoxImage.Exclamation);

      }
      catch (Exception ex)
      {
        MessageBox.Show("Error\nException Type: " + ex.GetType().FullName + "\nException Message: " + ex.Message, "Addin Handled Error",
          MessageBoxButton.OK, MessageBoxImage.Exclamation);

      }
    }

    private void btnNewInstallationTableEntry_Click(object sender, RoutedEventArgs e)
    {
      if (_it != null)
      {
        AddInstallationTimesEntryWindow win = new AddInstallationTimesEntryWindow(_it as InstallationTimesTable);
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
          _lstInstallationTimes.Add(gridItem);
          FabricationManager.ProductIdInstallationTimesView.dgInstallationTimes.Items.Refresh();
        }
      }
      else
        MessageBox.Show("No Installation Table Selected", "Add Product Entry", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    #endregion
  }
}


