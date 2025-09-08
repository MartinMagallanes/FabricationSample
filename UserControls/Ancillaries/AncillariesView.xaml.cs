using Autodesk.Fabrication.DB;
using FabricationSample.Data;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using FabricationSample.Manager;

namespace FabricationSample.UserControls.Ancillaries
{
  /// <summary>
  /// Interaction logic for AncillariesView.xaml
  /// </summary>
  public partial class AncillariesView : UserControl
  {
    public AncillaryTypeEnum AncillaryType { get; private set; }
    public AncillariesView(AncillaryTypeEnum ancillaryType)
    {
      InitializeComponent();

      AncillaryType = ancillaryType;
      FabricationManager.AncillariesView = this;
    }

    private void dgAncillaries_Loaded(object sender, RoutedEventArgs e)
    {
      // setup ancillaries
      dgColDiameter.Visibility = Visibility.Collapsed;
      if (AncillaryType == AncillaryTypeEnum.AirturnTrack)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<AirturnTrackAncillary>();
        var obsAncillaries = new ObservableCollection<AirturnTrackAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        iTimeQtyHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.AirturnVane)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<AirturnVaneAncillary>();
        var obsAncillaries = new ObservableCollection<AirturnVaneAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        iTimeQtyHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.AncillaryMaterial)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<AncillaryMaterialAncillary>();
        var obsAncillaries = new ObservableCollection<AncillaryMaterialAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
      }
      else if (AncillaryType == AncillaryTypeEnum.Gasket)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<GasketAncillary>();
        var obsAncillaries = new ObservableCollection<GasketAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
      }
      else if (AncillaryType == AncillaryTypeEnum.SupportRod)
      {
        dgColDiameter.Visibility = Visibility.Visible;
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<SupportRodAncillary>();
        var obsAncillaries = new ObservableCollection<SupportRodAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
      }
      else if (AncillaryType == AncillaryTypeEnum.TieRod)
      {
        dgColDiameter.Visibility = Visibility.Visible;
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<TieRodAncillary>();
        var obsAncillaries = new ObservableCollection<TieRodAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
      }
      else if (AncillaryType == AncillaryTypeEnum.Clip)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<ClipAncillary>();
        var obsAncillaries = new ObservableCollection<ClipAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costLengthHeader.Visibility = Visibility.Collapsed;
        weightLengthHeader.Visibility = Visibility.Collapsed;
        fTimeLengthHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.Corner)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<CornerAncillary>();
        var obsAncillaries = new ObservableCollection<CornerAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costLengthHeader.Visibility = Visibility.Collapsed;
        weightLengthHeader.Visibility = Visibility.Collapsed;
        fTimeLengthHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.Fixing)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<FixingAncillary>();
        var obsAncillaries = new ObservableCollection<FixingAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costLengthHeader.Visibility = Visibility.Collapsed;
        weightLengthHeader.Visibility = Visibility.Collapsed;
        fTimeLengthHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.Isolator)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<IsolatorAncillary>();
        var obsAncillaries = new ObservableCollection<IsolatorAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costLengthHeader.Visibility = Visibility.Collapsed;
        weightLengthHeader.Visibility = Visibility.Collapsed;
        fTimeLengthHeader.Visibility = Visibility.Collapsed;
        iTimeLengthHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.Sealant)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<SealantAncillary>();
        var obsAncillaries = new ObservableCollection<SealantAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costQtyHeader.Visibility = Visibility.Collapsed;
        weightQtyHeader.Visibility = Visibility.Collapsed;
        fTimeQtyHeader.Visibility = Visibility.Collapsed;
        iTimeQtyHeader.Visibility = Visibility.Collapsed;
      }
      else if (AncillaryType == AncillaryTypeEnum.SeamMaterial)
      {
        var ancillaries = Database.Ancillaries.Where(x => x.AncillaryType == AncillaryType).Cast<SeamMaterialAncillary>();
        var obsAncillaries = new ObservableCollection<SeamMaterialAncillary>(ancillaries.OrderBy(x => x.Group).ThenBy(x => x.Description));

        ListCollectionView ancilsView = new ListCollectionView(obsAncillaries);

        ancilsView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
        dgAncillaries.ItemsSource = ancilsView;
        costQtyHeader.Visibility = Visibility.Collapsed;
        weightQtyHeader.Visibility = Visibility.Collapsed;
        fTimeQtyHeader.Visibility = Visibility.Collapsed;
        iTimeQtyHeader.Visibility = Visibility.Collapsed;
      }
    }
    private void dgAncillaries_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e == null || e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      var ancillary = e.AddedItems[0] as AncillaryBase;
      if (ancillary != null)
        FabricationManager.CurrentAncillary = ancillary;
    }

    public void AddNewAncillary(AncillaryBase ancillary)
    {
      var ancillaries = dgAncillaries.ItemsSource as ListCollectionView;
      ancillaries.AddNewItem(ancillary);
    }

    public void DeleteAncillary()
    {
      var ancillary = dgAncillaries.SelectedItem as AncillaryBase;
      if (ancillary == null)
        return;

      var ancillaries = dgAncillaries.ItemsSource as ListCollectionView;
      ancillaries.Remove(ancillary);
    }

    public void RefreshView()
    {
      dgAncillaries_Loaded(null, null);
    }
  }
}
