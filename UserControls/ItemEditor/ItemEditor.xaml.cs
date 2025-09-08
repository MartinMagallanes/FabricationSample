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
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms.VisualStyles;
using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;
using Autodesk.Fabrication.Content;
using Autodesk.Fabrication.PrintObjects;
using FabricationSample.UserControls.Options;
using FabricationSample.UserControls.Dimensions;
using FabricationSample.UserControls.ItemEditor.ProductList;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;

namespace FabricationSample.UserControls.ItemEditor
{
   /// <summary>
   /// Interaction logic for ItemEditor.xaml
   /// </summary>
   public partial class ItemEditor : UserControl
   {
      public ObservableCollection<CustomDataMapper> DataMapper { get; set; }
      public ObservableCollection<ConnectorMapper> Conns { get; set; }
      public ObservableCollection<SeamMapper> Seams { get; set; }
      public ObservableCollection<DamperMapper> Dampers { get; set; }
      public ObservableCollection<OptionMapper> Options { get; set; }
      public ObservableCollection<DimensionMapper> Dimensions { get; set; }
      public ObservableCollection<ProductListGridItem> ProductListData { get; set; }
      public ObservableCollection<ProductListDataField> ProductListDataFields { get; set; }
      public ObservableCollection<ProductListDimensionField> ProductListDimensionFields { get; set; }
      public ObservableCollection<ProductListOptionField> ProductListOptionFields { get; set; }

      #region Private Members

      Material _selectedMaterial;
      Gauge _selectedGauge;
      Material _selectedInsMaterial;
      Gauge _selectedInsGauge;
      InsulationStatus _selectedInsulationStatus;
      AirturnInfo _selectedAirturn;
      SplitterInfo _selectedSplitter1;
      SplitterInfo _selectedSplitter2;
      StiffenerInfo _selectedStiffner;
      int _selectedStiffenerQty;
      Specification _selectedSpecification;
      Specification _selectedInsSpecification;
      ServiceType _selectedServiceType;

      #endregion

      public ItemEditor()
      {
         InitializeComponent();
      }

      //Load Item Details and bind controls
      private void UserControl_Loaded(object sender, RoutedEventArgs e)
      {
         bool isOwned = FabricationManager.CurrentItem.IsOwned;
         bool isTemplateDiskItem = (FabricationManager.CurrentItem.ItemType == ItemType.DiskItem && !FabricationManager.CurrentItem.IsCatalogue);

         if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
         {
            //Disable add item to job button
            btnAddItemToJob.IsEnabled = false;
            FabricationManager.CurrentLoadedItemPath = string.Empty;
         }
         else
            //Enable add item to job button
            btnAddItemToJob.IsEnabled = true;

         //Item Name
         txtItemName.Text += FabricationManager.CurrentItem.Name;
         //Item Owner
         txtItemOwner.Text += string.IsNullOrEmpty(FabricationManager.CurrentItem.OwnerInformation) ? " none" : FabricationManager.CurrentItem.OwnerInformation;
         //Item Type
         txtItemType.Text += FabricationManager.CurrentItem.ItemType == ItemType.JobItem ? "Job Item" : "Disk Item";
         //Name Button
         if (FabricationManager.CurrentItem.ItemType == ItemType.DiskItem)
            btnUpdateItems.Content = "Save Item to Disk";
         //Item Image
         if ((!string.IsNullOrEmpty(FabricationManager.CurrentItem.ImagePath)) && File.Exists(FabricationManager.CurrentItem.ImagePath))
            imgItem.Source = new ImageSourceConverter().ConvertFromString(FabricationManager.CurrentItem.ImagePath) as ImageSource;

         //Materials and Gauge
         cmbMaterial.ItemsSource = new ObservableCollection<Material>(Database.Materials);
         cmbMaterial.DisplayMemberPath = "Name";

         if (FabricationManager.CurrentItem.Material != null)
            cmbMaterial.SelectedIndex = Database.Materials.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.Material.Name);

         Gauge gauge = FabricationManager.CurrentItem.Gauge;
         if (gauge != null)
         {
            cmbGauge.SelectedIndex = FabricationManager.CurrentItem.Material.Gauges.ToList().FindIndex(x => x.Thickness == gauge.Thickness);
         }

         if (isOwned || isTemplateDiskItem)
         {
            cmbMaterial.IsEnabled = false;
            cmbGauge.IsEnabled = false;
            chkCheckValidSpecification.IsEnabled = false;
         }

         //Insulation
         cmbInsulationMaterial.ItemsSource = new ObservableCollection<Material>(); Database.Materials.Where(x => x.Insulation == true);
         cmbInsulationMaterial.DisplayMemberPath = "Name";

         cmbInsulationMaterial.Text = FabricationManager.CurrentItem.Insulation.Material != null ? FabricationManager.CurrentItem.Insulation.Material.Name : string.Empty;

         Gauge insulationGauge = FabricationManager.CurrentItem.Insulation.Gauge;
         if (insulationGauge != null)
         {
            cmbInsulationGauge.Text = insulationGauge.Thickness.ToString();
         }
         else
            cmbInsulationGauge.Text = string.Empty;

         cmbInsulationSetting.ItemsSource = new ObservableCollection<string>(System.Enum.GetNames(typeof(InsulationStatus)));
         cmbInsulationSetting.SelectedIndex = (int)FabricationManager.CurrentItem.Insulation.Status;

         //Airturns
         if (FabricationManager.CurrentItem.Airturns.Count == 0)
            cmbAirturns.IsEnabled = false;
         else
         {
            cmbAirturns.ItemsSource = new ObservableCollection<AirturnInfo>(Database.Airturns);
            cmbAirturns.DisplayMemberPath = "Name";
            cmbAirturns.Text = FabricationManager.CurrentItem.Airturns[0].Info != null ? FabricationManager.CurrentItem.Airturns[0].Info.Name : string.Empty;
         }

         //Splitters
         if (FabricationManager.CurrentItem.Splitters.Count == 0)
         {
            cmbSplitterOne.IsEnabled = false;
            cmbSplitterTwo.IsEnabled = false;
         }
         else
         {
            if (FabricationManager.CurrentItem.Splitters.Count == 1)
            {
               cmbSplitterTwo.IsEnabled = false;

               cmbSplitterOne.ItemsSource = new ObservableCollection<SplitterInfo>(Database.Splitters);
               cmbSplitterOne.DisplayMemberPath = "Name";

               if ((FabricationManager.CurrentItem.Splitters[0].Info != null) && (FabricationManager.CurrentItem.Splitters[0].Info.Name != null))
                  cmbSplitterOne.SelectedIndex = Database.Splitters.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.Splitters[0].Info.Name);

            }
            else
            {
               cmbSplitterOne.ItemsSource = new ObservableCollection<SplitterInfo>(Database.Splitters);
               cmbSplitterOne.DisplayMemberPath = "Name";

               if ((FabricationManager.CurrentItem.Splitters[0].Info != null) && (FabricationManager.CurrentItem.Splitters[0].Info.Name != null))
                  cmbSplitterOne.SelectedIndex = Database.Splitters.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.Splitters[0].Info.Name);

               cmbSplitterTwo.ItemsSource = new ObservableCollection<SplitterInfo>(Database.Splitters);
               cmbSplitterTwo.DisplayMemberPath = "Name";

               if ((FabricationManager.CurrentItem.Splitters[1].Info != null) && (FabricationManager.CurrentItem.Splitters[1].Info.Name != null))
                  cmbSplitterOne.SelectedIndex = Database.Splitters.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.Splitters[1].Info.Name);
            }
         }

         //Stiffeners
         if (FabricationManager.CurrentItem.Stiffeners.Count == 0)
         {
            cmbSiffener.IsEnabled = false;
            cmbSiffenerQty.IsEnabled = false;
         }
         else
         {
            cmbSiffener.ItemsSource = new ObservableCollection<StiffenerInfo>(Database.Stiffeners);
            cmbSiffenerQty.ItemsSource = new int[] { 0, 1, 2, 3 };
            cmbSiffener.DisplayMemberPath = "Name";
            cmbSiffener.Text = FabricationManager.CurrentItem.Stiffeners[0].Info != null ? FabricationManager.CurrentItem.Stiffeners[0].Info.Name : string.Empty;
            cmbSiffenerQty.Text = FabricationManager.CurrentItem.Stiffeners[0].Qty.ToString();
         }

         //Specifications
         cmbSpecification.ItemsSource = new ObservableCollection<Specification>(Database.Specifications);
         cmbSpecification.DisplayMemberPath = "Name";

         if (FabricationManager.CurrentItem.Specification != null)
            cmbSpecification.SelectedIndex = Database.Specifications.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.Specification.Name);

         if (isOwned || isTemplateDiskItem)
         {
            cmbSpecification.IsEnabled = false;
            chkCheckValidMaterial.IsEnabled = false;
         }

         //Insulation Specifications
         cmbInsSpecification.ItemsSource = new ObservableCollection<Specification>(Database.InsulationSpecifications);
         cmbInsSpecification.DisplayMemberPath = "Name";

         if (FabricationManager.CurrentItem.InsulationSpecification != null)
            cmbInsSpecification.SelectedIndex = Database.InsulationSpecifications.ToList().FindIndex(x => x.Name == FabricationManager.CurrentItem.InsulationSpecification.Name);

         //Spool Information
         txtSpoolName.Text = FabricationManager.CurrentItem.SpoolName;
         txtSpoolColor.Text = FabricationManager.CurrentItem.SpoolColor.ToString();

         //Item Alias
         txtItemAlias.Text = FabricationManager.CurrentItem.Alias;

         //Item Order
         txtItemOrder.Text = FabricationManager.CurrentItem.Order;

         //Item ETag
         txtItemETag.Text = FabricationManager.CurrentItem.EquipmentTag;

         //Item Zone
         txtItemZone.Text = FabricationManager.CurrentItem.Zone;

         //Item Notes
         txtItemNotes.Text = FabricationManager.CurrentItem.Notes;

         //Item Drawing Name
         txtItemDrawingName.Text = FabricationManager.CurrentItem.DrawingName;

         //Item Pallet
         txtItemPallet.Text = FabricationManager.CurrentItem.Pallet;

         //ACAD Handle
         txtACADObjectHandle.Text = Job.GetACADHandleFromItem(FabricationManager.CurrentItem);

         //Item Dimensions
         ParseDimensions();

         //Item Options
         ParseOptions();

         //Service Type
         cmbServiceType.ItemsSource = new ObservableCollection<ServiceType>(Database.ServiceTypes);
         cmbServiceType.DisplayMemberPath = "Name";

         if (FabricationManager.CurrentItem.ServiceType != null)
            cmbServiceType.SelectedIndex = Database.ServiceTypes.ToList().FindIndex(x => x.Id == FabricationManager.CurrentItem.ServiceType.Id);

         //Product List data
         BindProductListInfo();

         //Cut Type
         cmbCutType.ItemsSource = new ObservableCollection<ItemCutType>(FabricationManager.CurrentItem.ValidCutTypes);
         cmbCutType.SelectedItem = FabricationManager.CurrentItem.CutType;

         //Price List
         var lstPrices = new List<string>() { "None" };
         lstPrices.AddRange(Database.SupplierGroups.SelectMany(x => x.PriceLists).Select(x => x.Name));
         cmbPriceList.ItemsSource = new ObservableCollection<string>(lstPrices);

         if (FabricationManager.CurrentItem.PriceList == null)
            cmbPriceList.SelectedValue = "None";
         else
            cmbPriceList.SelectedValue = FabricationManager.CurrentItem.PriceList.Name;

         //Document Links
         dgDocumentLinks.ItemsSource = new ObservableCollection<ItemDocumentLink>(FabricationManager.CurrentItem.Links);

         //SKeys
         if (FabricationManager.CurrentItem.SupportsSKey)
            txtSKey.Text = FabricationManager.CurrentItem.SKey;
         else
            txtSKey.IsEnabled = false;

         //Visibility
         chkIsHiddenInViews.IsChecked = FabricationManager.CurrentItem.IsHiddenInViews;

         //Print objects
         var itemPrintObjects = Enum.GetValues(typeof(ItemPrintObjectEnum)).Cast<ItemPrintObjectEnum>().OrderBy(x => x.ToString());
         var itemPrintObjectDisplays = new List<ItemPrintObjectDisplay>();
         itemPrintObjects.ToList().ForEach(x =>
         {
             var valid = PrintObject.IsValid(x);
             if (valid)
               itemPrintObjectDisplays.Add(new ItemPrintObjectDisplay(x));
         });

         cmbItemPO.ItemsSource = itemPrintObjectDisplays;
         cmbItemPO.DisplayMemberPath = "DisplayValue";
         cmbItemPOCount.ItemsSource = m_printObjectNumbers;
         cmbItemPOCount.Visibility = Visibility.Collapsed;

         var partPrintObjects = Enum.GetValues(typeof(PartPrintObjectEnum)).Cast<PartPrintObjectEnum>().OrderBy(x => x.ToString());
         var partPrintObjectDisplays = new List<PartPrintObjectDisplay>();
         partPrintObjects.ToList().ForEach(x =>
         {
            var valid = PrintObject.IsValid(x);
            if (valid)
              partPrintObjectDisplays.Add(new PartPrintObjectDisplay(x));
         });

        cmbPartPO.ItemsSource = partPrintObjectDisplays;
        cmbPartPO.DisplayMemberPath = "DisplayValue";

        var ancPrintObjects = Enum.GetValues(typeof(AncillaryPrintObjectEnum)).Cast<AncillaryPrintObjectEnum>().OrderBy(x => x.ToString());
        var ancPrintObjectDisplays = new List<AncillaryPrintObjectDisplay>();
        ancPrintObjects.ToList().ForEach(x =>
        {
          var valid = PrintObject.IsValid(x);
          if (valid)
            ancPrintObjectDisplays.Add(new AncillaryPrintObjectDisplay(x));
        });

        cmbAncPO.ItemsSource = ancPrintObjectDisplays;
        cmbAncPO.DisplayMemberPath = "DisplayValue";
    }

      private void cmbMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbMaterial.ItemsSource != null)
         {
            _selectedMaterial = (Material)e.AddedItems[0];

            cmbGauge.ItemsSource = new ObservableCollection<Gauge>(_selectedMaterial.Gauges);
            cmbGauge.DisplayMemberPath = "Thickness";
         }
      }

      private void cmbGauge_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbGauge.ItemsSource != null)
            _selectedGauge = (Gauge)e.AddedItems[0];
      }

      private void cmbInsulationMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbInsulationMaterial.ItemsSource != null)
         {
            _selectedInsMaterial = (Material)e.AddedItems[0];
            cmbInsulationGauge.ItemsSource = new ObservableCollection<Gauge>(_selectedInsMaterial.Gauges);
            cmbInsulationGauge.DisplayMemberPath = "Thickness";
         }
      }

      private void cmbInsulationSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbInsulationSetting.ItemsSource != null)
         {
            _selectedInsulationStatus = (InsulationStatus)System.Enum.Parse(typeof(InsulationStatus), e.AddedItems[0].ToString());

            if (_selectedInsulationStatus == InsulationStatus.None)
            {
               cmbInsulationGauge.Text = string.Empty;
               _selectedInsGauge = null;
               cmbInsulationMaterial.Text = string.Empty;
               _selectedInsMaterial = null;
            }
         }
      }

      private void cmbInsulationGauge_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbInsulationGauge.ItemsSource != null)
            _selectedInsGauge = (Gauge)e.AddedItems[0];
      }

      private void cmbAirturns_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbAirturns.ItemsSource != null)
            _selectedAirturn = (AirturnInfo)e.AddedItems[0];
      }

      private void cmbSplitterOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbSplitterOne.ItemsSource != null)
            _selectedSplitter1 = (SplitterInfo)e.AddedItems[0];
      }

      private void cmbSplitterTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbSplitterTwo.ItemsSource != null)
            _selectedSplitter2 = (SplitterInfo)e.AddedItems[0];
      }

      private void cmbSiffener_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbSiffener.ItemsSource != null)
            _selectedStiffner = (StiffenerInfo)e.AddedItems[0];
      }

      private void cmbSiffenerQty_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbSiffenerQty.ItemsSource != null)
            _selectedStiffenerQty = (int)e.AddedItems[0];
      }

      private void cmbSpecification_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbSpecification.ItemsSource != null)
            _selectedSpecification = (Specification)e.AddedItems[0];
      }

      private void cmbInsSpecification_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbInsSpecification.ItemsSource != null)
            _selectedInsSpecification = (Specification)e.AddedItems[0];
      }

      private void dgConnectors_Loaded(object sender, RoutedEventArgs e)
      {
         Conns = new ObservableCollection<ConnectorMapper>();

         for (int i = 0; i < FabricationManager.CurrentItem.Connectors.Count; i++)
            Conns.Add(new ConnectorMapper { Index = "#" + i, Conn = FabricationManager.CurrentItem.Connectors[i].Info, ConnName = FabricationManager.CurrentItem.Connectors[i].Info.Name });

         dgConnectors.ItemsSource = Conns;
         Connector.ItemsSource = new ObservableCollection<ConnectorInfo>(Database.Connectors);
      }

      private void dgConnectors_MouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (sender != null)
         {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
               Point3D endDir = FabricationManager.CurrentItem.GetConnectorDirectionVector(grid.SelectedIndex);
               Point3D widthDir = FabricationManager.CurrentItem.GetConnectorWidthVector(grid.SelectedIndex);
               Point3D depthDir = FabricationManager.CurrentItem.GetConnectorDepthVector(grid.SelectedIndex);
               Point3D connEndPoint = FabricationManager.CurrentItem.GetConnectorEndPoint(grid.SelectedIndex);

               txtXEndDir.Text = Math.Round(endDir.X, 2).ToString();
               txtYEndDir.Text = Math.Round(endDir.Y, 2).ToString();
               txtZEndDir.Text = Math.Round(endDir.Z, 2).ToString();

               txtXWidthDir.Text = Math.Round(widthDir.X, 2).ToString();
               txtYWidthDir.Text = Math.Round(widthDir.Y, 2).ToString();
               txtZWidthDir.Text = Math.Round(widthDir.Z, 2).ToString();

               txtXDepthDir.Text = Math.Round(depthDir.X, 2).ToString();
               txtYDepthDir.Text = Math.Round(depthDir.Y, 2).ToString();
               txtZDepthDir.Text = Math.Round(depthDir.Z, 2).ToString();

               txtXConnPoint.Text = Math.Round(connEndPoint.X, 4).ToString();
               txtYConnPoint.Text = Math.Round(connEndPoint.Y, 4).ToString();
               txtZConnPoint.Text = Math.Round(connEndPoint.Z, 4).ToString();

               txtConnType.Text = Enum.GetName(typeof(ConnectionType), FabricationManager.CurrentItem.GetConnectorConnectionType(grid.SelectedIndex));
            }
         }

      }

      private void dgSeams_Loaded(object sender, RoutedEventArgs e)
      {
         Seams = new ObservableCollection<SeamMapper>();

         for (int i = 0; i < FabricationManager.CurrentItem.Seams.Count; i++)
            Seams.Add(new SeamMapper { Index = "#" + i, Seam = FabricationManager.CurrentItem.Seams[i].Info, SeamName = FabricationManager.CurrentItem.Seams[i].Info.Name });

         dgSeams.ItemsSource = Seams;
         Seam.ItemsSource = new ObservableCollection<SeamInfo>(Database.Seams);
      }

      private void dgDampers_Loaded(object sender, RoutedEventArgs e)
      {
         Dampers = new ObservableCollection<DamperMapper>();

         for (int i = 0; i < FabricationManager.CurrentItem.Dampers.Count; i++)
            Dampers.Add(new DamperMapper { Index = "#" + i, Damper = FabricationManager.CurrentItem.Dampers[i].Info, DamperName = FabricationManager.CurrentItem.Dampers[i].Info.Name });

         dgDampers.ItemsSource = Dampers;
         Damper.ItemsSource = new ObservableCollection<DamperInfo>(Database.Dampers);
      }

      private void cmbServiceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count > 0 && cmbServiceType.ItemsSource != null)
            _selectedServiceType = (ServiceType)e.AddedItems[0];
      }

      private void btnUpdateItems_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            if (FabricationManager.CurrentItem != null)
            {
               StringBuilder builder = new StringBuilder();

               //Update Item Specification if required
               if (UpdateSpecification())
                  FabricationAPIExamples.ChangeItemSpecification(FabricationManager.CurrentItem, _selectedSpecification, (bool)chkCheckValidMaterial.IsChecked);

               //Update Item Material and Gauge if required
               if (UpdateMaterial())
                  FabricationAPIExamples.ChangeItemMaterial(FabricationManager.CurrentItem, _selectedMaterial, _selectedGauge, (bool)chkCheckValidSpecification.IsChecked);

               //Update Item Insulation Material and Gauge if required
               if (UpdateInsMaterial())
                  FabricationAPIExamples.ChangeItemInsulation(FabricationManager.CurrentItem, _selectedInsulationStatus, _selectedInsMaterial, _selectedInsGauge);


               if (UpdateInsSpecification())
                  FabricationAPIExamples.ChangeItemInsulationSpecification(FabricationManager.CurrentItem, _selectedInsSpecification);

               if (UpdateCutType())
                  FabricationAPIExamples.ChangeItemCutType(FabricationManager.CurrentItem, (ItemCutType)cmbCutType.SelectedItem);

               //Update Item Connectors if required
               for (int i = 0; i < FabricationManager.CurrentItem.Connectors.Count; i++)
               {
                  if (Conns[i].Conn.Name != FabricationManager.CurrentItem.Connectors[i].Info.Name)
                     FabricationAPIExamples.ChangeItemConnector(FabricationManager.CurrentItem, Conns[i].Conn, i);
               }

               if (_selectedAirturn != null)
               {
                  FabricationManager.CurrentItem.Airturns[0].IsLocked = true;
                  FabricationManager.CurrentItem.Airturns[0].Info = _selectedAirturn;
                  builder.AppendLine("Airturn Updated");
               }

               if (_selectedSplitter1 != null)
               {
                  FabricationManager.CurrentItem.Splitters[0].IsLocked = true;
                  FabricationManager.CurrentItem.Splitters[0].Info = _selectedSplitter1;
                  builder.AppendLine("Splitter1 Updated");
               }

               if (_selectedSplitter2 != null)
               {
                  FabricationManager.CurrentItem.Splitters[1].IsLocked = true;
                  FabricationManager.CurrentItem.Splitters[1].Info = _selectedSplitter2;
                  builder.AppendLine("Splitter2 Updated");
               }

               if (UpdateSiffener())
               {
                  FabricationManager.CurrentItem.Stiffeners[0].IsLocked = true;
                  FabricationManager.CurrentItem.Stiffeners[0].Qty = _selectedStiffenerQty;
                  FabricationManager.CurrentItem.Stiffeners[0].Info = _selectedStiffner;
                  builder.AppendLine("Stiffener Updated");
               }

               //Seams
               for (int i = 0; i < FabricationManager.CurrentItem.Seams.Count; i++)
               {
                  if (Seams[i].Seam.Name != FabricationManager.CurrentItem.Seams[i].Info.Name)
                  {
                     FabricationManager.CurrentItem.Seams[i].Info = Seams[i].Seam;
                     FabricationManager.CurrentItem.Seams[i].IsLocked = true;
                     builder.AppendLine("Seam #" + (i + 1) + " Updated");
                  }
               }

               //Dampers
               for (int i = 0; i < FabricationManager.CurrentItem.Dampers.Count; i++)
               {
                  if (Dampers[i].Damper.Name != FabricationManager.CurrentItem.Dampers[i].Info.Name)
                  {
                     FabricationManager.CurrentItem.Dampers[i].Info = Dampers[i].Damper;
                     FabricationManager.CurrentItem.Dampers[i].IsLocked = true;
                     builder.AppendLine("Damper #" + (i + 1) + " Updated");
                  }
               }

               if (!string.IsNullOrEmpty(txtSpoolName.Text) && txtSpoolName.Text != FabricationManager.CurrentItem.SpoolName)
               {
                  FabricationManager.CurrentItem.SpoolName = txtSpoolName.Text;
                  builder.AppendLine("Spool Name Updated");
               }

               if (!string.IsNullOrEmpty(txtSpoolColor.Text) && FabricationManager.CurrentItem.SpoolColor != Convert.ToInt32(txtSpoolColor.Text.ToString()))
               {
                  FabricationManager.CurrentItem.SpoolColor = Convert.ToInt32(txtSpoolColor.Text.ToString());
                  builder.AppendLine("Spool Color Updated");
               }

               //Custom Data
               if ((DataMapper != null) && DataMapper.Count > 0)
               {
                  UpdateCustomData();
               }

               //Item Notes
               if (!string.IsNullOrEmpty(txtItemNotes.Text) && txtItemNotes.Text != FabricationManager.CurrentItem.Notes)
               {
                  FabricationManager.CurrentItem.Notes = txtItemNotes.Text;
                  builder.AppendLine("Item Notes Updated");
               }

               //Item Alias
               if (!string.IsNullOrEmpty(txtItemAlias.Text) && txtItemAlias.Text != FabricationManager.CurrentItem.Alias)
               {
                  FabricationManager.CurrentItem.Alias = txtItemAlias.Text;
                  builder.AppendLine("Item Alias Updated");
               }

               //Item Order
               if (!string.IsNullOrEmpty(txtItemOrder.Text) && txtItemOrder.Text != FabricationManager.CurrentItem.Order)
               {
                  FabricationManager.CurrentItem.Order = txtItemOrder.Text;
                  builder.AppendLine("Item Order Updated");
               }

               //Item Equipment Tag
               if (!string.IsNullOrEmpty(txtItemETag.Text) && txtItemETag.Text != FabricationManager.CurrentItem.EquipmentTag)
               {
                  FabricationManager.CurrentItem.EquipmentTag = txtItemETag.Text;
                  builder.AppendLine("Item ETag Updated");
               }

               //Item Zone
               if (!string.IsNullOrEmpty(txtItemZone.Text) && txtItemZone.Text != FabricationManager.CurrentItem.Zone)
               {
                  FabricationManager.CurrentItem.Zone = txtItemZone.Text;
                  builder.AppendLine("Item Zone Updated");
               }

               //Item Drawing Name
               if (!string.IsNullOrEmpty(txtItemDrawingName.Text) && txtItemDrawingName.Text != FabricationManager.CurrentItem.DrawingName)
               {
                  FabricationManager.CurrentItem.DrawingName = txtItemDrawingName.Text;
                  builder.AppendLine("Item Drawing Name Updated");
               }

               //Item Pallet
               if (!string.IsNullOrEmpty(txtItemPallet.Text) && txtItemPallet.Text != FabricationManager.CurrentItem.Pallet)
               {
                  FabricationManager.CurrentItem.Pallet = txtItemPallet.Text;
                  builder.AppendLine("Item Pallet Updated");
               }

               //Item SKey
               if ((txtSKey.IsEnabled) && (!string.IsNullOrEmpty(txtSKey.Text)) && txtSKey.Text != FabricationManager.CurrentItem.SKey)
               {
                  FabricationAPIExamples.UpdateSKey(FabricationManager.CurrentItem, txtSKey.Text);
               }

               //Item Costs
               UpdateItemCosts(ref builder);

               //Service Type
               if ((_selectedServiceType != null) && _selectedServiceType.Id != FabricationManager.CurrentItem.ServiceType.Id)
               {
                  FabricationManager.CurrentItem.ServiceType = _selectedServiceType;
                  builder.AppendLine("Item Service Type Updated");
               }

               //Visibility
               FabricationManager.CurrentItem.IsHiddenInViews = (bool)chkIsHiddenInViews.IsChecked;

               if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
               {
                  //Subscribe to Item Updating\Updated Events
                  if (chkEventSubscribe.IsChecked == true)
                  {
                     FabricationAPIEventSubscriber subscriber = new FabricationAPIEventSubscriber();
                     subscriber.SubscribeToItemUpdateEvents(FabricationManager.CurrentItem);
                     FabricationManager.CurrentItem.Update();
                     subscriber.UnSubscribeToItemUpdateEvents(FabricationManager.CurrentItem);
                  }
                  else
                     FabricationManager.CurrentItem.Update();
                  //Update View
                  Autodesk.Fabrication.UI.UIApplication.UpdateView(new List<Item>() { FabricationManager.CurrentItem });
                  System.Windows.MessageBox.Show("Item Properties Updated" + Environment.NewLine + builder.ToString(), "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
               }
               else if (FabricationManager.CurrentItem.ItemType == ItemType.DiskItem)
               {
                  bool saved = false;
                  MessageBoxResult result = MessageBox.Show("Click yes to save existing item or no to create copy?", "Save Item", MessageBoxButton.YesNo, MessageBoxImage.Question);

                  if (result == MessageBoxResult.Yes)
                     saved = FabricationAPIExamples.SaveItem(FabricationManager.CurrentItem, false, true);
                  else
                     saved = FabricationAPIExamples.SaveItem(FabricationManager.CurrentItem, true, true);

                  if (saved)
                     System.Windows.MessageBox.Show("Item Saved" + Environment.NewLine + builder.ToString(), "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                  else
                     System.Windows.MessageBox.Show("Item save failed", "Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);

               }
            }
         }
         catch (Exception)
         {
            System.Windows.MessageBox.Show("Item Properties Update Failed", "Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
         }
         finally
         {
            FabricationManager.ParentWindow.LoadDBEditorControl();
         }
      }

      private void txtSpoolColor_PreviewTextInput(object sender, TextCompositionEventArgs e)
      {
         Regex regex = new Regex("[^0-9]+");
         e.Handled = regex.IsMatch(e.Text);
      }

      private void btnBack_Click(object sender, RoutedEventArgs e)
      {
         FabricationManager.CurrentItem = null;
         FabricationManager.ParentWindow.LoadDBEditorControl();
      }

      private void btnChangeItemImage_Click(object sender, RoutedEventArgs e)
      {
         if (FabricationAPIExamples.SetItemImage(FabricationManager.CurrentItem))
         {
            //Item Image
            if ((!string.IsNullOrEmpty(FabricationManager.CurrentItem.ImagePath)) && File.Exists(FabricationManager.CurrentItem.ImagePath))
               imgItem.Source = new ImageSourceConverter().ConvertFromString(FabricationManager.CurrentItem.ImagePath) as ImageSource;
         }
      }

      private void btnAddItemToJob_Click(object sender, RoutedEventArgs e)
      {
         if (FabricationManager.CurrentItem.IsProductList)
         {
            AddProductSizeToJobWindow win = new AddProductSizeToJobWindow();
            win.ShowDialog();
         }

         if (chkJobEventSubscribe.IsChecked == true)
         {
            FabricationAPIEventSubscriber subscriber = new FabricationAPIEventSubscriber();
            subscriber.SubscribeToJobEvents();
            FabricationAPIExamples.AddItemToJob(FabricationManager.CurrentItem);
            subscriber.UnSubscribeToJobEvents();
         }
         else
            FabricationAPIExamples.AddItemToJob(FabricationManager.CurrentItem);
      }

      #region Custom Data

      private void dgCustomData_Loaded(object sender, RoutedEventArgs e)
      {
         DataMapper = new ObservableCollection<CustomDataMapper>();

         foreach (CustomData entry in Database.CustomItemData.ToList())
         {
            CustomDataMapper mapper = new CustomDataMapper();
            mapper.Entry = entry;

            if (FabricationManager.CurrentItem.CustomData.FirstOrDefault(x => x.Data.Id == entry.Id) != null)
            {
               CustomItemData itemData = FabricationManager.CurrentItem.CustomData.FirstOrDefault(x => x.Data.Id == entry.Id);
               mapper.OnItem = true;

               switch (entry.Type)
               {
                  case CustomDataType.Double:
                     CustomDataDoubleValue dVal = itemData as CustomDataDoubleValue;
                     mapper.Value = dVal.Value.ToString();
                     break;
                  case CustomDataType.Integer:
                     CustomDataIntegerValue iVal = itemData as CustomDataIntegerValue;
                     mapper.Value = iVal.Value.ToString();
                     break;
                  case CustomDataType.String:
                     CustomDataStringValue sVal = itemData as CustomDataStringValue;
                     mapper.Value = sVal.Value;
                     break;
                  default:
                     break;
               }
            }

            DataMapper.Add(mapper);
         }

         dgCustomData.ItemsSource = DataMapper;

      }

      private void UpdateCustomData()
      {
         foreach (CustomDataMapper mapper in DataMapper)
         {
            //Check for custom data to add where not already present on item
            if (mapper.OnItem && !FabricationManager.CurrentItem.CustomData.ToList().Exists(x => x.Data.Id == mapper.Entry.Id))
            {
               FabricationAPIExamples.AddCustomDataToItem(FabricationManager.CurrentItem, mapper.Entry);
            }

            //Update Custom Data values if required
            if (!string.IsNullOrEmpty(mapper.Value) && mapper.OnItem)
            {
               //Get new custom data from item
               CustomItemData itemData = FabricationManager.CurrentItem.CustomData.FirstOrDefault(x => x.Data.Id == mapper.Entry.Id);

               if (itemData != null)
                  FabricationAPIExamples.UpdateCustomItemData(FabricationManager.CurrentItem, itemData, mapper.Value);

            }
         }
      }


      #endregion

      #region Options

      public void ParseOptions()
      {
         Options = new ObservableCollection<OptionMapper>();

         foreach (ItemOptionBase option in FabricationManager.CurrentItem.Options)
         {
            Type optionType = option.GetType();

            OptionMapper oMapper = new OptionMapper()
            {
               Name = option.Name,
               IsLocked = option.IsLocked
            };

            if (optionType == typeof(ItemComboOption))
            {
               ItemComboOption cOpt = option as ItemComboOption;
               ItemOptionEntry optEntry = cOpt.Options.ToList().FirstOrDefault(x => x.IsSelected);

               if (optEntry.GetType() == typeof(ItemOptionValueEntry))
               {
                  ItemOptionValueEntry valEntry = optEntry as ItemOptionValueEntry;
                  oMapper.Value = valEntry.Value.ToString();
               }
               else
               {
                  oMapper.Value = optEntry.Name;
               }

               oMapper.OptionType = "ComboOption";
            }
            else if (optionType == typeof(ItemSelectOption))
            {
               ItemSelectOption sOpt = option as ItemSelectOption;
               oMapper.Value = sOpt.Options.ToList().Find(x => x.IsSelected).Name.ToString();
               oMapper.OptionType = "SelectOption";
            }
            else if (optionType == typeof(ItemMinMaxNumericOption))
            {
               oMapper.Value = option.Value.ToString();
               oMapper.OptionType = "MinMaxNumericOption";
            }
            else if (optionType == typeof(ItemMinMaxIntegerOption))
            {
               oMapper.Value = option.Value.ToString();
               oMapper.OptionType = "MinMaxIntegerOption";
            }
            else
            {
               oMapper.Value = option.Value.ToString();
               oMapper.OptionType = "NotImplementedOption";
            }

            Options.Add(oMapper);
         }

         if (Options != null)
         {
            dgOptions.ItemsSource = Options;
         }

      }

      private void dgOptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (sender != null)
         {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
               try
               {
                  OptionMapper oMapper = Options[grid.SelectedIndex];
                  if (oMapper.OptionType == "ComboOption")
                  {
                     ItemComboOption cOpt = FabricationManager.CurrentItem.Options[grid.SelectedIndex] as ItemComboOption;
                     ControlHostOpts.Content = new OptionComboEdit(FabricationManager.CurrentItem, cOpt);
                  }
                  else if (oMapper.OptionType == "SelectOption")
                  {
                     ItemSelectOption sOpt = FabricationManager.CurrentItem.Options[grid.SelectedIndex] as ItemSelectOption;
                     ControlHostOpts.Content = new OptionSelectEdit(FabricationManager.CurrentItem, sOpt);
                  }
                  else if (oMapper.OptionType == "MinMaxIntegerOption" || oMapper.OptionType == "MinMaxNumericOption")
                  {
                     ItemOptionBase sOpt = FabricationManager.CurrentItem.Options[grid.SelectedIndex] as ItemOptionBase;
                     ControlHostOpts.Content = new OptionNumberEdit(FabricationManager.CurrentItem, sOpt);
                  }
                  else
                  {
                     ControlHostOpts.Content = null;
                  }

               }
               catch (Exception)
               {
                  System.Windows.MessageBox.Show("Error Loading Item Option", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
               }

            }
         }
      }

      #endregion

      #region Dimensions

      public void ParseDimensions()
      {
         Dimensions = new ObservableCollection<DimensionMapper>();

         foreach (ItemDimensionBase dimension in FabricationManager.CurrentItem.Dimensions)
         {
            Type dimensionType = dimension.GetType();

            DimensionMapper dMapper = new DimensionMapper()
            {
               Name = dimension.Name,
               IsLocked = dimension.IsLocked
            };

            if (dimensionType == typeof(ItemDimension))
            {
               ItemDimension dim = dimension as ItemDimension;
               dMapper.Value = dim.Value.ToString();
               dMapper.DimensionType = "Standard";
            }
            else if (dimensionType == typeof(ItemComboDimension))
            {
               ItemComboDimension cDim = dimension as ItemComboDimension;
               ItemComboDimensionEntry dimEntry = cDim.Options.ToList().FirstOrDefault(x => x.IsSelected);

               if (dimEntry.GetType() == typeof(ItemComboDimensionValueEntry))
               {
                  ItemComboDimensionValueEntry valEntry = dimEntry as ItemComboDimensionValueEntry;
                  dMapper.Value = valEntry.Value.ToString();
               }
               else
               {
                  dMapper.Value = dimEntry.Name;
               }

               dMapper.DimensionType = "ComboDimension";
            }

            Dimensions.Add(dMapper);
         }

         if (Dimensions != null)
         {
            dgDimensions.ItemsSource = Dimensions;
         }

      }

      private void dgDimensions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
      {
         if (sender != null)
         {
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
            {
               try
               {
                  DimensionMapper dMapper = Dimensions[grid.SelectedIndex];
                  if (dMapper.DimensionType == "ComboDimension")
                  {
                     ItemComboDimension cDim = FabricationManager.CurrentItem.Dimensions[grid.SelectedIndex] as ItemComboDimension;
                     ControlHostDims.Content = new DimensionComboEdit(FabricationManager.CurrentItem, cDim);
                  }
                  else if (dMapper.DimensionType == "Standard")
                  {
                     ItemDimension sDim = FabricationManager.CurrentItem.Dimensions[grid.SelectedIndex] as ItemDimension;
                     ControlHostDims.Content = new DimensionNumberEdit(FabricationManager.CurrentItem, sDim);
                  }

                  else
                  {
                     ControlHostDims.Content = null;
                  }

               }
               catch (Exception)
               {
                  System.Windows.MessageBox.Show("Error Loading Item Dimension", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
               }

            }
         }
      }

      #endregion

      #region Costings

      private void txtItemCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
      {
         Regex regex = new Regex(@"[^0-9]+\.[^0-9]*");
         e.Handled = regex.IsMatch(e.Text);
      }

      private void tbiItemCosting_Loaded(object sender, RoutedEventArgs e)
      {
         txtItemCost.Text = FabricationManager.CurrentItem.Cost.ToString();
         txtItemFabCost.Text = FabricationManager.CurrentItem.FabricationRate.ToString();
         txtItemInstallationCost.Text = FabricationManager.CurrentItem.InstallationRate.ToString();
         txtItemExtraInstallationTime.Text = FabricationManager.CurrentItem.ExtraInstallationTime.ToString();
         txtItemExtraFabricationTime.Text = FabricationManager.CurrentItem.ExtraFabricationTime.ToString();

         cmbExtraInstallTimeUnits.ItemsSource = Enum.GetValues(typeof(Autodesk.Fabrication.Units.TimeUnits));
         cmbExtraInstallTimeUnits.SelectedItem = FabricationManager.CurrentItem.ExtraInstallationTimeUnits;

         cmbExtraFabricationTimeUnits.ItemsSource = Enum.GetValues(typeof(Autodesk.Fabrication.Units.TimeUnits));
         cmbExtraFabricationTimeUnits.SelectedItem = FabricationManager.CurrentItem.ExtraFabricationTimeUnits;
      }

      private void UpdateItemCosts(ref StringBuilder builder)
      {
         double newCost = 0;
         double newFabCost = 0;
         double newInstallCost = 0;
         double newExtraInstallationTime = 0;
         double newExtraFabricationTime = 0;

         if (double.TryParse(txtItemCost.Text.Trim(), out newCost))
         {
            if (newCost != FabricationManager.CurrentItem.Cost)
            {
               FabricationManager.CurrentItem.Cost = newCost;
               builder.AppendLine("Item Cost Updated");
            }
         }

         if (double.TryParse(txtItemFabCost.Text.Trim(), out newFabCost))
         {
            if (newFabCost != FabricationManager.CurrentItem.FabricationRate)
            {
               FabricationManager.CurrentItem.FabricationRate = newFabCost;
               builder.AppendLine("Item Fabrication Rate Updated");
            }
         }

         if (double.TryParse(txtItemInstallationCost.Text.Trim(), out newInstallCost))
         {
            if (newInstallCost != FabricationManager.CurrentItem.InstallationRate)
            {
               FabricationManager.CurrentItem.InstallationRate = newInstallCost;
               builder.AppendLine("Item Installation Rate Updated");
            }
         }

         if (double.TryParse(txtItemExtraInstallationTime.Text.Trim(), out newExtraInstallationTime))
         {
            if (newExtraInstallationTime != FabricationManager.CurrentItem.ExtraInstallationTime)
            {
               FabricationManager.CurrentItem.ExtraInstallationTime = newExtraInstallationTime;
               builder.AppendLine("Item Extra Installation Time Updated");
            }
         }

         if (double.TryParse(txtItemExtraFabricationTime.Text.Trim(), out newExtraFabricationTime))
         {
            if (newExtraFabricationTime != FabricationManager.CurrentItem.ExtraFabricationTime)
            {
               FabricationManager.CurrentItem.ExtraFabricationTime = newExtraFabricationTime;
               builder.AppendLine("Item Extra Fabrication Time Updated");
            }
         }

         if ((Autodesk.Fabrication.Units.TimeUnits)cmbExtraInstallTimeUnits.SelectedItem != FabricationManager.CurrentItem.ExtraInstallationTimeUnits)
         {
            FabricationManager.CurrentItem.ExtraInstallationTimeUnits = (Autodesk.Fabrication.Units.TimeUnits)cmbExtraInstallTimeUnits.SelectedItem;
            builder.AppendLine("Item Extra Installation Time Units Updated");
         }

         if ((Autodesk.Fabrication.Units.TimeUnits)cmbExtraFabricationTimeUnits.SelectedItem != FabricationManager.CurrentItem.ExtraFabricationTimeUnits)
         {
            FabricationManager.CurrentItem.ExtraFabricationTimeUnits = (Autodesk.Fabrication.Units.TimeUnits)cmbExtraFabricationTimeUnits.SelectedItem;
            builder.AppendLine("Item Extra Fabrication Time Units Updated");
         }

         string currentPriceListName = FabricationManager.CurrentItem.PriceList == null ? "None" : FabricationManager.CurrentItem.PriceList.Name;

         if (cmbPriceList.SelectedValue.ToString() != currentPriceListName)
         {
            if (cmbPriceList.SelectedValue.ToString() != "None")
            {
               foreach (PriceList pl in Database.SupplierGroups.SelectMany(x => x.PriceLists).ToList())
               {
                  if (pl.Name == cmbPriceList.SelectedValue.ToString())
                  {
                     FabricationManager.CurrentItem.PriceList = pl;
                     break;
                  }
               }
            }
            else
               FabricationManager.CurrentItem.PriceList = null;
            builder.AppendLine("Price List Updated");
         }


      }

      #endregion

      #region Helpers

      private bool UpdateMaterial()
      {
         bool update = false;
         if ((_selectedMaterial != null) && FabricationManager.CurrentItem.Material == null)
            update = true;
         else if ((_selectedMaterial != null) && _selectedMaterial.Name != FabricationManager.CurrentItem.Material.Name)
            update = true;
         else if (_selectedGauge != null)
         {
            if (_selectedGauge.Thickness != FabricationManager.CurrentItem.Gauge.Thickness)
               update = true;
         }

         return update;
      }

      private bool UpdateInsMaterial()
      {
         bool update = false;
         if ((_selectedInsMaterial != null) && FabricationManager.CurrentItem.Insulation.Material == null)
            update = true;
         else if ((_selectedInsMaterial != null) && _selectedInsMaterial.Name != FabricationManager.CurrentItem.Insulation.Material.Name)
            update = true;
         else if (_selectedInsMaterial != null && _selectedInsGauge != null)
         {
            if (_selectedGauge.Thickness != FabricationManager.CurrentItem.Insulation.Gauge.Thickness)
               update = true;
         }

         return update;
      }

      private bool UpdateSpecification()
      {
         if ((_selectedSpecification != null) && FabricationManager.CurrentItem.Specification == null)
            return true;
         else if ((_selectedSpecification != null) && _selectedSpecification.Name != FabricationManager.CurrentItem.Specification.Name)
            return true;
         else
            return false;
      }

      private bool UpdateInsSpecification()
      {
         bool update = false;
         if ((_selectedInsSpecification != null) && FabricationManager.CurrentItem.InsulationSpecification == null)
            update = true;
         else if ((_selectedInsSpecification != null) && _selectedInsSpecification.Name != FabricationManager.CurrentItem.InsulationSpecification.Name)
            update = true;

         return update;
      }

      private bool UpdateSiffener()
      {
         if ((_selectedStiffner != null) && FabricationManager.CurrentItem.Stiffeners[0].Info == null)
            return true;
         else if ((_selectedStiffner != null) && _selectedStiffner.Name != FabricationManager.CurrentItem.Stiffeners[0].Info.Name)
            return true;
         else
            return false;
      }

      private bool UpdateCutType()
      {
         if (FabricationManager.CurrentItem.CutType != (ItemCutType)cmbCutType.SelectedItem)
            return true;
         else
            return false;
      }

      #endregion

      #region Product List

      private bool maintainRowSelection;
      private int rowSelectedIndex;

      private void BindProductListInfo()
      {
         if (FabricationManager.CurrentItem.ProductList == null)
         {
            lstProdListDataFields.IsEnabled = false;
            lstProdListDimensionFields.IsEnabled = false;
            lstProdListOptionFields.IsEnabled = false;
            stkAddProductRow.Visibility = System.Windows.Visibility.Hidden;
            stkAddProductRevision.Visibility = System.Windows.Visibility.Hidden;
            stkCreateProductList.Visibility = System.Windows.Visibility.Visible;
            btnMoveDataDown.IsEnabled = false;
            btnMoveDataUp.IsEnabled = false;
         }
         else
         {
            lstProdListDataFields.IsEnabled = true;
            lstProdListDimensionFields.IsEnabled = true;
            lstProdListOptionFields.IsEnabled = true;
            stkAddProductRow.Visibility = System.Windows.Visibility.Visible;
            stkAddProductRevision.Visibility = System.Windows.Visibility.Visible;
            stkCreateProductList.Visibility = System.Windows.Visibility.Hidden;

            if (FabricationManager.CurrentItem.ProductList.Rows.Count > 1)
            {
               btnMoveDataDown.IsEnabled = true;
               btnMoveDataUp.IsEnabled = true;
            }

            BindProductListDataFields();
            BindProductListData();
            BindProductListDimensionFields();
            BindProductListOptionFields();

            if (maintainRowSelection)
            {
               dgProductList.SelectedIndex = rowSelectedIndex;
               maintainRowSelection = false;
            }
         }

      }

      private void BindProductListData()
      {

         ProductListData = ProductListData ?? new ObservableCollection<ProductListGridItem>();
         ProductListData.Clear();
         dgProductList.Columns.Clear();

         int index = 0;

         ItemProductListDataTemplate template = FabricationManager.CurrentItem.ProductList.Template;

         foreach (ItemProductListDataRow row in FabricationManager.CurrentItem.ProductList.Rows)
         {
            ProductListGridItem gridItem = new ProductListGridItem(row);

            if (gridItem.Name != null)
            {
               if (index == 0)
                  AddProductListColumn("Name", "Name");
            }

            if (template.UseAlias)
            {
               if (index == 0)
                  AddProductListColumn("Alias", "Alias");
            }

            if (template.UseArea)
            {
               if (index == 0)
                  AddProductListColumn("Area", "Area");
            }

            if (template.UseBoughtOut)
            {
               if (index == 0)
                  AddProductListColumn("BoughtOut", "BoughtOut");
            }

            if (template.UseCadBlockName)
            {
               if (index == 0)
                  AddProductListColumn("CADBlockName", "CADBlockName");
            }

            if (template.UseDatabaseId)
            {
               if (index == 0)
                  AddProductListColumn("DatabaseId", "DatabaseId");
            }

            if (index == 0)
               for (int i = 0; i < gridItem.Dimensions.Count; i++)
                  AddProductListColumn(gridItem.Dimensions[i].Name, "Dimensions[" + i + "].DimensionValue");

            if (index == 0)
               for (int i = 0; i < gridItem.Options.Count; i++)
                  AddProductListColumn(gridItem.Options[i].Name, "Options[" + i + "].OptionValue");

            if (template.UseFlow)
            {
               if (index == 0)
               {
                  AddProductListColumn("MaximumFlow", "MaximumFlow");
                  AddProductListColumn("MinimumFlow", "MinimumFlow");
               }
            }

            if (template.UseOrderNumber)
            {
               if (index == 0)
                  AddProductListColumn("OrderNumber", "OrderNumber");
            }

            if (template.UseWeight)
            {
               if (index == 0)
                  AddProductListColumn("Weight", "Weight");
            }

            ProductListData.Add(gridItem);

            index++;
         }

         if ((ProductListData != null) && ProductListData.Count > 0)
         {
            dgProductList.ItemsSource = ProductListData;
            dgProductList.Items.Refresh();
            txtAddProductRevision.Text = FabricationManager.CurrentItem.ProductList.Revision;
         }

         if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
            dgProductList.IsReadOnly = true;

      }

      private bool BindProductListDataFields()
      {
         bool hasData = false;

         if (FabricationManager.CurrentItem.ProductList.HasDataTemplate)
         {
            ProductListDataFields = ProductListDataFields ?? new ObservableCollection<ProductListDataField>();
            ProductListDataFields.Clear();
            ItemProductListDataTemplate template = FabricationManager.CurrentItem.ProductList.Template;
            ProductListDataFields.Add(new ProductListDataField("Alias", ProductListDataFieldType.Alias, template));
            ProductListDataFields.Add(new ProductListDataField("Area", ProductListDataFieldType.Area, template));
            ProductListDataFields.Add(new ProductListDataField("Bought Out", ProductListDataFieldType.BoughtOut, template));
            ProductListDataFields.Add(new ProductListDataField("CAD Block Name", ProductListDataFieldType.CADBlockName, template));
            ProductListDataFields.Add(new ProductListDataField("Database Id", ProductListDataFieldType.DatabaseId, template));
            ProductListDataFields.Add(new ProductListDataField("Flow", ProductListDataFieldType.Flow, template));
            ProductListDataFields.Add(new ProductListDataField("Order Number", ProductListDataFieldType.OrderNumber, template));
            ProductListDataFields.Add(new ProductListDataField("Weight", ProductListDataFieldType.Weight, template));
         }

         if ((ProductListDataFields != null) && ProductListDataFields.Count > 0)
         {
            hasData = true;
            lstProdListDataFields.ItemsSource = ProductListDataFields;
            lstProdListDataFields.Items.Refresh();
            if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
               lstProdListDataFields.IsEnabled = false;
         }

         return hasData;
      }

      private bool BindProductListDimensionFields()
      {
         bool hasData = false;

         if (FabricationManager.CurrentItem.ProductList.HasDataTemplate)
         {
            ProductListDimensionFields = ProductListDimensionFields ?? new ObservableCollection<ProductListDimensionField>();
            ProductListDimensionFields.Clear();
            ItemProductListDataTemplate template = FabricationManager.CurrentItem.ProductList.Template;

            foreach (ItemDimensionBase baseDim in FabricationManager.CurrentItem.Dimensions)
               ProductListDimensionFields.Add(new ProductListDimensionField(baseDim, template));
         }

         if ((ProductListDimensionFields != null) && ProductListDimensionFields.Count > 0)
         {
            hasData = true;

            lstProdListDimensionFields.ItemsSource = ProductListDimensionFields;
            lstProdListDimensionFields.Items.Refresh();
            if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
               lstProdListDimensionFields.IsEnabled = false;
         }

         return hasData;
      }

      private bool BindProductListOptionFields()
      {
         bool hasData = false;

         if (FabricationManager.CurrentItem.ProductList.HasDataTemplate)
         {
            ProductListOptionFields = ProductListOptionFields ?? new ObservableCollection<ProductListOptionField>();
            ProductListOptionFields.Clear();
            ItemProductListDataTemplate template = FabricationManager.CurrentItem.ProductList.Template;

            foreach (ItemOptionBase baseOpt in FabricationManager.CurrentItem.Options)
               ProductListOptionFields.Add(new ProductListOptionField(baseOpt, template));
         }

         if ((ProductListOptionFields != null) && ProductListOptionFields.Count > 0)
         {
            hasData = true;

            lstProdListOptionFields.ItemsSource = ProductListOptionFields;
            lstProdListOptionFields.Items.Refresh();
            if (FabricationManager.CurrentItem.ItemType == ItemType.JobItem)
               lstProdListOptionFields.IsEnabled = false;
         }

         return hasData;
      }

      private void AddProductListColumn(string headerName, string bindingName)
      {
         DataGridTextColumn textColumn = new DataGridTextColumn();
         textColumn.Header = headerName;
         Binding binding = new Binding(bindingName);
         binding.Mode = BindingMode.TwoWay;
         binding.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
         textColumn.Binding = binding;
         dgProductList.Columns.Add(textColumn);
      }

      private void chkProdListDataField_Checked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListDataField))
            {
               ProductListDataField dataField = chk.DataContext as ProductListDataField;
               lstProdListDataFields.SelectedItem = dataField;
               lstProdListDataFields.IsEnabled = false;
               FabricationManager.CurrentDataField = dataField;
               if (dataField.Field == ProductListDataFieldType.BoughtOut)
                  contentAddDataField.Content = new AddProductDataBoughtOutEntry();
               else if (dataField.Field == ProductListDataFieldType.Flow)
                  contentAddDataField.Content = new AddProductDataFlowEntry();
               else
                  contentAddDataField.Content = new AddProductDataEntry();
            }
         }
      }

      private void chkProdListDataField_Unchecked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListDataField))
            {
               ProductListDataField dataField = chk.DataContext as ProductListDataField;
               lstProdListDataFields.SelectedItem = dataField;
               FabricationManager.CurrentDataField = dataField;

               switch (FabricationManager.CurrentDataField.Field)
               {
                  case ProductListDataFieldType.Alias:
                     FabricationManager.CurrentDataField.Template.RemoveAlias();
                     break;
                  case ProductListDataFieldType.Area:
                     FabricationManager.CurrentDataField.Template.RemoveArea();
                     break;
                  case ProductListDataFieldType.CADBlockName:
                     FabricationManager.CurrentDataField.Template.RemoveCadBlockName();
                     break;
                  case ProductListDataFieldType.OrderNumber:
                     FabricationManager.CurrentDataField.Template.RemoveOrderNumber();
                     break;
                  case ProductListDataFieldType.Weight:
                     FabricationManager.CurrentDataField.Template.RemoveWeight();
                     break;
                  case ProductListDataFieldType.DatabaseId:
                     FabricationManager.CurrentDataField.Template.RemoveDatabaseId();
                     break;
                  case ProductListDataFieldType.BoughtOut:
                     FabricationManager.CurrentDataField.Template.RemoveBoughtOut();
                     break;
                  case ProductListDataFieldType.Flow:
                     FabricationManager.CurrentDataField.Template.RemoveFlow();
                     break;
                  default:
                     break;
               }
            }
            BindProductListInfo();
         }
      }

      private void chkProdListDimensionField_Checked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListDimensionField))
            {
               ProductListDimensionField dimField = chk.DataContext as ProductListDimensionField;
               lstProdListDimensionFields.SelectedItem = dimField;
               lstProdListDimensionFields.IsEnabled = false;
               FabricationManager.CurrentDimensionField = dimField;
               contentAddDimensionField.Content = new AddProductDimensionEntry();
            }
         }
      }

      private void chkProdListDimensionField_Unchecked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListDimensionField))
            {
               ProductListDimensionField dimField = chk.DataContext as ProductListDimensionField;
               lstProdListDimensionFields.SelectedItem = dimField;
               FabricationManager.CurrentDimensionField = dimField;
               dimField.Template.RemoveDimensionDefinition(new ItemProductListDimensionDefinition(dimField.Dimension, true));
               BindProductListInfo();
            }

         }
      }

      private void chkProdListOptionField_Checked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListOptionField))
            {
               ProductListOptionField optField = chk.DataContext as ProductListOptionField;
               lstProdListOptionFields.SelectedItem = optField;
               lstProdListOptionFields.IsEnabled = false;
               FabricationManager.CurrentOptionField = optField;
               contentAddOptionField.Content = new AddProductOptionEntry();
            }
         }
      }

      private void chkProdListOptionField_Unchecked(object sender, RoutedEventArgs e)
      {
         CheckBox chk = sender as CheckBox;

         if (chk != null)
         {
            if ((chk.DataContext != null) && chk.DataContext.GetType() == typeof(ProductListOptionField))
            {
               ProductListOptionField optField = chk.DataContext as ProductListOptionField;
               lstProdListOptionFields.SelectedItem = optField;
               FabricationManager.CurrentOptionField = optField;
               optField.Template.RemoveOptionDefinition(new ItemProductListOptionDefinition(optField.Option, true));
               BindProductListInfo();
            }

         }
      }

      public void FinshEditingProductDataField()
      {
         contentAddDataField.Content = null;
         lstProdListDataFields.IsEnabled = true;
         BindProductListInfo();

      }

      public void FinshEditingProductDimensionField()
      {
         contentAddDimensionField.Content = null;
         lstProdListDimensionFields.IsEnabled = true;
         BindProductListInfo();
      }

      public void FinshEditingProductOptionField()
      {
         contentAddOptionField.Content = null;
         lstProdListOptionFields.IsEnabled = true;
         BindProductListInfo();
      }

      private void dgProductList_PreviewKeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Delete)
         {
            if (MessageBox.Show("Confirm to Delete Row", "Delete Row", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
               var dataGrid = (DataGrid)sender;

               if (dataGrid.SelectedItem != null)
               {
                  int index = dataGrid.SelectedIndex;
                  FabricationManager.CurrentItem.ProductList.RemoveRow(index);
                  e.Handled = false;
               }
            }
            else
               e.Handled = true;

         }
      }

      private void btnAddRow_Click(object sender, RoutedEventArgs e)
      {
         if (!string.IsNullOrEmpty(txtAddRowName.Text))
         {
            if (FabricationManager.CurrentItem.ProductList.HasDataTemplate)
            {
               ItemOperationResult result = FabricationManager.CurrentItem.ProductList.AddDefaultRow(txtAddRowName.Text.Trim());

               if (result.Status == ResultStatus.Succeeded)
               {
                  BindProductListInfo();
                  MessageBox.Show(result.Message, "Row Added", MessageBoxButton.OK, MessageBoxImage.Information);
               }
               else
                  MessageBox.Show(result.Message, "Add Row Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
               MessageBox.Show("No data template associated with product list", "Add Row Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
         }
         else
         {
            MessageBox.Show("MIssing Data", "Add Row Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            txtAddRowName.Focus();
         }
      }

      private void btnCreateProductList_Click(object sender, RoutedEventArgs e)
      {
         bool created = false;

         if ((bool)chkCreateProductListFromFile.IsChecked)
            created = LoadProductListData();
         else
            created = CreateNewProductListItem();

         if (created)
            BindProductListInfo();
      }

      private bool LoadProductListData()
      {
         bool loaded = false;

         System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog()
         {
            CheckPathExists = true,
            Filter = "CSV File (*.csv)|*.csv",
            Title = "Select CSV File"
         };

         if (dlg.ShowDialog(System.Windows.Forms.Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle)) == System.Windows.Forms.DialogResult.OK)
         {
            string filename = dlg.FileName;
            string[] fileContents = File.ReadAllLines(filename);

            if (fileContents.Length > 0)
            {
               List<string> header = fileContents[0].Split(',').ToList();
               int nameIndex = header.FindIndex(x => x.ToLower().Trim() == "name");
               int weightIndex = header.FindIndex(x => x.ToLower().Trim() == "weight");
               int idIndex = header.FindIndex(x => x.ToLower().Trim() == "id");
               List<KeyValuePair<int, string>> dimPosns = new List<KeyValuePair<int, string>>();
               List<KeyValuePair<int, string>> optPosns = new List<KeyValuePair<int, string>>();

               for (int i = 0; i < header.Count; i++)
               {
                  if (header[i].Contains("DIM:"))
                  {
                     string dimName = header[i].Replace("DIM:", "");

                     if (!string.IsNullOrEmpty(dimName))
                        dimPosns.Add(new KeyValuePair<int, string>(i, dimName.Trim()));
                  }

                  if (header[i].Contains("OPT:"))
                  {
                     string optName = header[i].Replace("OPT:", "");

                     if (!string.IsNullOrEmpty(optName))
                        optPosns.Add(new KeyValuePair<int, string>(i, optName.Trim()));
                  }
               }

               ItemProductList prodList = new ItemProductList();

               for (int i = 1; i < fileContents.Length; i++)
               {
                  string[] line = fileContents[i].Split(',');
                  string name = line[nameIndex];
                  double weight = double.Parse(line[weightIndex]);
                  string id = line[idIndex];

                  //Add DataTemplate to product List
                  if (!prodList.HasDataTemplate)
                  {
                     var dimDefs = new List<ItemProductListDimensionDefinition>();
                     var optDefs = new List<ItemProductListOptionDefinition>();

                     foreach (KeyValuePair<int, string> kvp in dimPosns)
                     {
                        ItemDimensionBase dimFromItem = FabricationManager.CurrentItem.Dimensions.FirstOrDefault(x => x.Name == kvp.Value);
                        if (dimFromItem != null)
                           dimDefs.Add(new ItemProductListDimensionDefinition(dimFromItem, true));
                     }

                     foreach (KeyValuePair<int, string> kvp in optPosns)
                     {
                        ItemOptionBase optFromItem = FabricationManager.CurrentItem.Options.FirstOrDefault(x => x.Name == kvp.Value);
                        if (optFromItem != null)
                           optDefs.Add(new ItemProductListOptionDefinition(optFromItem, true));
                     }

                     ItemProductListDataTemplate template = new ItemProductListDataTemplate();

                     template.SetWeight(null);
                     template.SetDatabaseId(null);

                     foreach (ItemProductListDimensionDefinition def in dimDefs)
                     {
                        template.AddDimensionDefinition(def, 0);
                     }

                     foreach (ItemProductListOptionDefinition opt in optDefs)
                     {
                        template.AddOptionDefinition(opt, 0);
                     }

                     prodList.AddDataTemplate(template);
                  }

                  var dimEntries = new List<ItemProductListDimensionEntry>();
                  var optEntries = new List<ItemProductListOptionEntry>();

                  foreach (ItemProductListDimensionDefinition dimDef in prodList.Template.DimensionsDefinitions)
                  {
                     KeyValuePair<int, string> kvp = dimPosns.FirstOrDefault(x => x.Value == dimDef.Name);
                     dimEntries.Add(dimDef.CreateDimensionEntry(double.Parse(line[kvp.Key])));
                  }

                  foreach (ItemProductListOptionDefinition optDef in prodList.Template.OptionsDefinitions)
                  {
                     KeyValuePair<int, string> kvp = optPosns.FirstOrDefault(x => x.Value == optDef.Name);
                     optEntries.Add(optDef.CreateOptionEntry(double.Parse(line[kvp.Key])));
                  }

                  prodList.AddRow(name, null, null, weight, null, null, id, null, null, null, dimEntries, optEntries);
               }

               if (ContentManager.CreateProductItem(FabricationManager.CurrentItem, prodList).Status == ResultStatus.Succeeded)
                  loaded = true;
            }
         }

         return loaded;
      }

      private bool CreateNewProductListItem()
      {
         bool created = false;

         ItemProductList prodList = new ItemProductList();
         prodList.AddDataTemplate(new ItemProductListDataTemplate());

         if (ContentManager.CreateProductItem(FabricationManager.CurrentItem, prodList).Status == ResultStatus.Succeeded)
            created = true;

         return created;
      }

      private void btnUpdateRevision_Click(object sender, RoutedEventArgs e)
      {
         if (txtAddProductRevision.Text == string.Empty)
         {
            MessageBox.Show("Enter Value", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            txtAddProductRevision.Focus();
         }
         else
         {
            FabricationManager.CurrentItem.ProductList.Revision = txtAddProductRevision.Text;
            MessageBox.Show("Revision Updated", "Revision", MessageBoxButton.OK, MessageBoxImage.Information);
            BindProductListInfo();

         }
      }

      private void btnMoveDataUp_Click(object sender, RoutedEventArgs e)
      {
         if (dgProductList.SelectedItem != null)
         {
            int index = dgProductList.SelectedIndex;
            if (FabricationManager.CurrentItem.ProductList.MoveRowPosition(index, ItemProductListRowMoveDirection.Up).Status == ResultStatus.Succeeded)
            {
               maintainRowSelection = true;
               rowSelectedIndex = index + (int)ItemProductListRowMoveDirection.Up;
               BindProductListInfo();
            }
         }
      }

      private void btnMoveDataDown_Click(object sender, RoutedEventArgs e)
      {
         if (dgProductList.SelectedItem != null)
         {
            int index = dgProductList.SelectedIndex;
            if (FabricationManager.CurrentItem.ProductList.MoveRowPosition(index, ItemProductListRowMoveDirection.Down).Status == ResultStatus.Succeeded)
            {
               maintainRowSelection = true;
               rowSelectedIndex = index + (int)ItemProductListRowMoveDirection.Down;
               BindProductListInfo();
            }
         }
      }

      #endregion

      #region Document Links

      private void ButtonAddDocumentLink_Click(object sender, RoutedEventArgs e)
      {
         ItemDocumentLink link = FabricationAPIExamples.AddItemDocumentLink(FabricationManager.CurrentItem,
           txtAddDocumentLinkname.Text, txtAddDocumentLinkTarget.Text, txtAddDocumentLinkParams.Text);

         if (link != null)
            dgDocumentLinks.ItemsSource = new ObservableCollection<ItemDocumentLink>(FabricationManager.CurrentItem.Links);

      }

      private void ButtonRemoveDocumentLink_Click(object sender, RoutedEventArgs e)
      {
         if ((dgDocumentLinks.SelectedItem != null) &&
           FabricationAPIExamples.RemoveItemDocumentLink(FabricationManager.CurrentItem, dgDocumentLinks.SelectedItem as ItemDocumentLink))
            dgDocumentLinks.ItemsSource = new ObservableCollection<ItemDocumentLink>(FabricationManager.CurrentItem.Links);
      }

      private void Hyperlink_Click(object sender, RoutedEventArgs e)
      {
         try
         {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
         }
         catch (Exception)
         {
            MessageBox.Show("Error navigating to target.", "Document Link Target", MessageBoxButton.OK, MessageBoxImage.Exclamation);
         }

      }
      #endregion

      #region Parts

      private void dg_parts_OnLoaded(object sender, RoutedEventArgs e)
      {
         var item = FabricationManager.CurrentItem;
         if (item == null)
            return;

         dg_parts.ItemsSource = item.Parts;
      }

      #endregion

      #region Collars

      private void dg_collars_OnLoaded(object sender, RoutedEventArgs e)
      {
         var item = FabricationManager.CurrentItem;
         if (item == null)
            return;

         dg_Collars.ItemsSource = item.Collars;

      }

    #endregion

    private void dgAncillaries_Loaded(object sender, RoutedEventArgs e)
    {
      var item = FabricationManager.CurrentItem;
      if (item == null)
        return;

      var ancillaries = item.Ancillaries;

      dgAncillaries.ItemsSource = ancillaries;
    }

     private readonly ObservableCollection<int> m_printObjectNumbers = new ObservableCollection<int>();
     private void CmbItemPO_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
     {
       var newValue = e.AddedItems[0] as ItemPrintObjectDisplay;
       if (newValue == null)
         return;
       
       var count = PrintObject.Count(FabricationManager.CurrentItem, newValue.Value);
       if (count <= 1)
       {
         txtItemPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, newValue.Value, 0);
         ShowOrHideItemPrintObjectData(newValue);
         cmbItemPOCount.Visibility = Visibility.Collapsed;
       }
       else
       {
         var numberList = Enumerable.Range(0, count).ToList();

         m_printObjectNumbers.Clear();
         numberList.ForEach(x => m_printObjectNumbers.Add(x));

         cmbItemPOCount.Visibility = Visibility.Visible;         
         cmbItemPOCount.SelectedIndex = 0;  
         cmbItemPOCount.UpdateLayout();
       }
   
     }

       private void CmbItemPOCount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
       {
           var value = cmbItemPO.SelectedItem as ItemPrintObjectDisplay;
           if (value == null)
               return;

           var index = 0;
           if (cmbItemPOCount.Visibility == Visibility.Visible && e.AddedItems.Count > 0)
           {
               index = (int)e.AddedItems[0];
               if (index < 0)
                   index = 0;
           }

           txtItemPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, value.Value, index);
           ShowOrHideItemPrintObjectData(value);

       }

     private void ShowOrHideItemPrintObjectData(ItemPrintObjectDisplay value)
     {
       if (string.IsNullOrWhiteSpace(txtItemPO.Text))
       {
         txtItemPOUnits.Visibility = Visibility.Collapsed;
       }
       else
       {
         txtItemPOUnits.Visibility = Visibility.Visible;
         var units = PrintObject.GetUnits(value.Value);
         if (string.IsNullOrWhiteSpace(units))
         {
           txtItemPOUnits.Visibility = Visibility.Collapsed;
         }
         else
         {
           txtItemPOUnits.Visibility = Visibility.Visible;
           txtItemPOUnits.Text = units;
         }
       }
    }

     private void ShowOrHidePartPrintObjectData(PartPrintObjectDisplay value)
     {
       if (string.IsNullOrWhiteSpace(txtPartPO.Text))
       {
         txtPartPOUnits.Visibility = Visibility.Collapsed;
       }
       else
       {
         txtPartPOUnits.Visibility = Visibility.Visible;
         var units = PrintObject.GetUnits(value.Value);
         if (string.IsNullOrWhiteSpace(units))
         {
           txtPartPOUnits.Visibility = Visibility.Collapsed;
         }
         else
         {
           txtPartPOUnits.Visibility = Visibility.Visible;
           txtPartPOUnits.Text = units;
         }
       }
     }

     private void ShowOrHideAncillaryPrintObjectData(AncillaryPrintObjectDisplay value)
     {
       if (string.IsNullOrWhiteSpace(txtAncPO.Text))
       {
         txtAncPOUnits.Visibility = Visibility.Collapsed;
       }
       else
       {
         txtAncPOUnits.Visibility = Visibility.Visible;
         var units = PrintObject.GetUnits(value.Value);
         if (string.IsNullOrWhiteSpace(units))
         {
           txtAncPOUnits.Visibility = Visibility.Collapsed;
         }
         else
         {
           txtAncPOUnits.Visibility = Visibility.Visible;
           txtAncPOUnits.Text = units;
         }
       }
     }

    private void CmbPartPO_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
     {
       var selectedPart = dg_parts.SelectedItem as Part;
       if (selectedPart == null)
         return;

       var newValue = e.AddedItems[0] as PartPrintObjectDisplay;
       if (newValue == null)
         return;

       txtPartPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, selectedPart, newValue.Value); 
       ShowOrHidePartPrintObjectData(newValue);
     }

     private void CmbAncPO_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
     {
       var selectedAncillary = dgAncillaries.SelectedItem as AncillaryUsage;
       if (selectedAncillary == null)
         return;

       var newValue = e.AddedItems[0] as AncillaryPrintObjectDisplay;
       if (newValue == null)
         return;

       txtAncPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, selectedAncillary, newValue.Value);
       ShowOrHideAncillaryPrintObjectData(newValue);
     }


    private void Dg_parts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
       var part = e.AddedItems[0] as Part;
       if (part == null)
         return;

       var value = cmbPartPO.SelectedItem as PartPrintObjectDisplay;
       if (value == null)
         return;

       txtPartPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, part, value.Value);
       ShowOrHidePartPrintObjectData(value);
    }

    private void DgAncillaries_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var ancillary = e.AddedItems[0] as AncillaryUsage;
      if (ancillary == null)
        return;

      var value = cmbAncPO.SelectedItem as AncillaryPrintObjectDisplay;
      if (value == null)
        return;

      txtAncPO.Text = PrintObject.GetValue(FabricationManager.CurrentItem, ancillary, value.Value);
      ShowOrHideAncillaryPrintObjectData(value);
    }

   }


  public class ItemPrintObjectDisplay
  {
    public ItemPrintObjectEnum Value { get; private set; }

    public string DisplayValue
    {
      get
      {
        var str = "";
        foreach (var s in Regex.Split(Value.ToString(), @"(?=[A-Z])"))
          str += s + " ";

        if (string.IsNullOrWhiteSpace(str))
          str = "No value";

        return str;
      }
    }

    public ItemPrintObjectDisplay(ItemPrintObjectEnum value)
    {
      Value = value;
    }
  }

  public class PartPrintObjectDisplay
  {
    public PartPrintObjectEnum Value { get; private set; }

    public string DisplayValue
    {
      get
      {
        var str = "";
        foreach (var s in Regex.Split(Value.ToString(), @"(?=[A-Z])"))
          str += s + " ";

        if (string.IsNullOrWhiteSpace(str))
          str = "No value";

        return str;
      }
    }

    public PartPrintObjectDisplay(PartPrintObjectEnum value)
    {
      Value = value;
    }
  }

  public class AncillaryPrintObjectDisplay
  {
    public AncillaryPrintObjectEnum Value { get; private set; }

    public string DisplayValue
    {
      get
      {
        var str = "";
        foreach (var s in Regex.Split(Value.ToString(), @"(?=[A-Z])"))
          str += s + " ";

        if (string.IsNullOrWhiteSpace(str))
          str = "No value";

        return str;
      }
    }

    public AncillaryPrintObjectDisplay(AncillaryPrintObjectEnum value)
    {
      Value = value;
    }
  }
}
