using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FabricationSample.UserControls.Ancillaries
{
  /// <summary>
  /// Interaction logic for AncillaryDetailsView.xaml
  /// </summary>
  /// 

  public partial class AncillaryDetailsView : UserControl
  {
    public AncillaryBase Ancillary;
    private Window m_parent;
    private bool m_productListTablesOnly = true;

    public AncillaryDetailsView(Window parent, AncillaryBase ancillary)
    {
      InitializeComponent();
      m_parent = parent;
      Ancillary = ancillary;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      // set the data context
      m_productListTablesOnly = true;
      var diameterHeight = 0.0;
      if (Ancillary.AncillaryType == AncillaryTypeEnum.AirturnTrack)
      {
        DataContext = Ancillary as AirturnTrackAncillary;
        rowITimeByLength.Height = new GridLength(0);
        rowITimeByQty.Height = new GridLength(0);
        rowITimesTable.Height = new GridLength(0);
        rowITimeType.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.AirturnVane)
      {
        DataContext = Ancillary as AirturnVaneAncillary;
        rowITimeByLength.Height = new GridLength(0);
        rowITimeByQty.Height = new GridLength(0);
        rowITimesTable.Height = new GridLength(0);
        rowITimeType.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.AncillaryMaterial)
      {
        DataContext = Ancillary as AncillaryMaterialAncillary;
        m_productListTablesOnly = false;
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Clip)
      {
        DataContext = Ancillary as ClipAncillary;
        rowCostByLength.Height = new GridLength(0);
        rowFTimeByLength.Height = new GridLength(0);
        rowITimeByLength.Height = new GridLength(0);
        rowWeightByLength.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Corner)
      {
        DataContext = Ancillary as CornerAncillary;
        rowCostByLength.Height = new GridLength(0);
        rowFTimeByLength.Height = new GridLength(0);
        rowITimeByLength.Height = new GridLength(0);
        rowWeightByLength.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Fixing)
      {
        DataContext = Ancillary as FixingAncillary;
        rowCostByLength.Height = new GridLength(0);
        rowFTimeByLength.Height = new GridLength(0);
        rowITimeByLength.Height = new GridLength(0);
        rowWeightByLength.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Gasket)
      {
        DataContext = Ancillary as GasketAncillary;
        m_productListTablesOnly = false;
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Isolator)
      {
        DataContext = Ancillary as IsolatorAncillary;
        rowCostByLength.Height = new GridLength(0);
        rowFTimeByLength.Height = new GridLength(0);
        rowITimeByLength.Height = new GridLength(0);
        rowWeightByLength.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.Sealant)
      {
        DataContext = Ancillary as SealantAncillary;
        rowCostByQty.Height = new GridLength(0);
        rowFTimeByQty.Height = new GridLength(0);
        rowITimeByQty.Height = new GridLength(0);
        rowWeightByQty.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.SeamMaterial)
      {
        DataContext = Ancillary as SeamMaterialAncillary;
        rowCostByQty.Height = new GridLength(0);
        rowFTimeByQty.Height = new GridLength(0);
        rowITimeByQty.Height = new GridLength(0);
        rowWeightByQty.Height = new GridLength(0);
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.SupportRod)
      {
        DataContext = Ancillary as SupportRodAncillary;
        m_productListTablesOnly = false;
        diameterHeight = rowDiameter.Height.Value;
      }
      else if (Ancillary.AncillaryType == AncillaryTypeEnum.TieRod)
      {
        DataContext = Ancillary as TieRodAncillary;
        m_productListTablesOnly = false;
        diameterHeight = rowDiameter.Height.Value;
      }

      rowDiameter.Height = new GridLength(diameterHeight);

      var costTypes = new List<AncillaryCostTypeEnum>() { AncillaryCostTypeEnum.PriceList, AncillaryCostTypeEnum.Value };
      var fabTypes = new List<AncillaryFabricationTimeTypeEnum>() { AncillaryFabricationTimeTypeEnum.TimesTable, AncillaryFabricationTimeTypeEnum.Value };
      var instTypes = new List<AncillaryInstallationTimeTypeEnum>() { AncillaryInstallationTimeTypeEnum.TimesTable, AncillaryInstallationTimeTypeEnum.Value };

      cmbCostType.ItemsSource = costTypes;
      cmbFTimeType.ItemsSource = fabTypes;
      cmbITimeType.ItemsSource = instTypes;
    }

    private void cmbCostType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (Ancillary.CanChange == false)
        return;

      var value = (AncillaryCostTypeEnum)e.AddedItems[0];

      Ancillary.CostType = value;

      if (value == AncillaryCostTypeEnum.Value)
      {
        txtBoxCostByQty.IsEnabled = true;
        txtBoxCostByLength.IsEnabled = true;
        cmbPriceList.IsEnabled = false;
      }
      else
      {
        txtBoxCostByQty.IsEnabled = false;
        txtBoxCostByLength.IsEnabled = false;
        cmbPriceList.IsEnabled = true;
      }
    }

    private void cmbFTimeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (Ancillary.CanChange == false)
        return;

      var value = (AncillaryFabricationTimeTypeEnum)e.AddedItems[0];

      Ancillary.FabricationTimeType = value;

      if (value == AncillaryFabricationTimeTypeEnum.Value)
      {
        txtboxFTimeByLength.IsEnabled = true;
        txtboxFTimeByQty.IsEnabled = true;
        cmbFTime.IsEnabled = false;
      }
      else
      {
        txtboxFTimeByLength.IsEnabled = false;
        txtboxFTimeByQty.IsEnabled = false;
        cmbFTime.IsEnabled = true;
      }
    }

    private void cmbITimeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (Ancillary.CanChange == false)
        return;

      var value = (AncillaryInstallationTimeTypeEnum)e.AddedItems[0];

      Ancillary.InstallationTimeType = value;

      if (value == AncillaryInstallationTimeTypeEnum.Value)
      {
        txtboxITimeByLength.IsEnabled = true;
        txtboxITimeByQty.IsEnabled = true;
        cmbITime.IsEnabled = false;
      }
      else
      {
        txtboxITimeByLength.IsEnabled = false;
        txtboxITimeByQty.IsEnabled = false;
        cmbITime.IsEnabled = true;
      }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      m_parent.Close();
    }

    private void cmbPriceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (cmbPriceList.IsEnabled == false)
        return;

      if (Ancillary.CanChange == false)
        return;

      var priceList = e.AddedItems[0] as PriceListBase;

      Ancillary.PriceList = priceList;
    }

    private void cmbFTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (cmbFTime.IsEnabled == false)
        return;

      if (Ancillary.CanChange == false)
        return;

      var fabTime = e.AddedItems[0] as FabricationTimesTableBase;

      Ancillary.FabricationTimesTable = fabTime;
    }

    private void cmbITime_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      if (cmbITime.IsEnabled == false)
        return;

      if (Ancillary.CanChange == false)
        return;

      var installTime = e.AddedItems[0] as InstallationTimesTableBase;

      Ancillary.InstallationTimesTable = installTime;
    }

    private void cmbPriceList_Loaded(object sender, RoutedEventArgs e)
    {
      var priceList = Ancillary.PriceList;

      if (Ancillary.CostType != AncillaryCostTypeEnum.PriceList)
        cmbPriceList.IsEnabled = false;

      // do price lists
      var priceLists = new List<PriceListBase>();
      foreach (var s in Database.SupplierGroups)
      {
        foreach (var p in s.PriceLists)
        {
          if (m_productListTablesOnly)
          {
            if (p is PriceList)
              priceLists.Add(p);
          }
          else
            priceLists.Add(p);
        }
      }

      priceLists = priceLists.OrderBy(x => x.Name).ToList();

      var obs = new ObservableCollection<PriceListBase>(priceLists);

      cmbPriceList.ItemsSource = obs;
      cmbPriceList.SelectedItem = priceList;
    }

    private void cmbFTime_Loaded(object sender, RoutedEventArgs e)
    {
      var fabTime = Ancillary.FabricationTimesTable;
    
      if (Ancillary.FabricationTimeType != AncillaryFabricationTimeTypeEnum.TimesTable)
        cmbFTime.IsEnabled = false;

      // fabrication times
      ListCollectionView ftimes = null;
      if (m_productListTablesOnly == false)
        ftimes = new ListCollectionView(Database.FabricationTimesTable);
      else
        ftimes = new ListCollectionView(Database.FabricationTimesTable.Where(x => x is FabricationTimesTable).ToList());

      ftimes.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
      ftimes.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      ftimes.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

      cmbFTime.ItemsSource = ftimes;
      cmbFTime.SelectedItem = fabTime;
    }

    private void cmbITime_Loaded(object sender, RoutedEventArgs e)
    {
      cmbITime.SelectedItem = Ancillary.InstallationTimesTable;

      if (Ancillary.InstallationTimeType != AncillaryInstallationTimeTypeEnum.TimesTable)
        cmbITime.IsEnabled = false;

      // installation times
      ListCollectionView itimes = null;
      if (m_productListTablesOnly == false)
        itimes = new ListCollectionView(Database.InstallationTimesTable);
      else
        itimes = new ListCollectionView(Database.InstallationTimesTable.Where(x => x is InstallationTimesTable).ToList());

      itimes.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
      itimes.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
      itimes.GroupDescriptions.Add(new PropertyGroupDescription("Group"));

      cmbITime.ItemsSource = itimes;
    }
  }
}
