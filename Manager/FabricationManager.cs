using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using FabricationSample.UserControls;
using FabricationSample.UserControls.DatabaseEditor;
using FabricationSample.UserControls.ItemEditor;
using FabricationSample.UserControls.ServiceEditor;
using FabricationSample.Data;
using FabricationSample.UserControls.Ancillaries;

namespace FabricationSample.Manager
{
  public static class FabricationManager
  {
    public static Item CurrentItem { get; set; }
    public static Service CurrentService { get; set; }
    public static ServiceTemplate CurrentServiceTemplate { get; set; }
    public static Material CurrentMaterial { get; set; }
    public static Gauge CurrentGauge { get; set; }
    public static FabricationWindow ParentWindow { get; set; }
    public static DatabaseEditor DBEditor { get; set; }
    public static ItemEditor ItemEditor { get; set; }
    public static ServiceEditor ServiceEditor { get; set; }
    public static ProductListDataField CurrentDataField { get; set; }
    public static ProductListDimensionField CurrentDimensionField { get; set; }
    public static ProductListOptionField CurrentOptionField { get; set; }
    public static string CurrentLoadedItemPath { get; set; }
    public static ItemFoldersView ItemFoldersView { get; set; }
    public static EditServiceButtonItemWindow EditServiceButtonItemWindow { get; set; }
    public static ProductIdPriceListView ProductIdPriceListView { get; set; }
    public static BreakPointView BreakPointPriceListView { get; set; }
    public static ProductIdFabricationTimesView ProductIdFabricationTimesView { get; set; }
    public static ProductIdInstallationTimesView ProductIdInstallationTimesView { get; set; }
    public static SupplierGroup CurrentSupplierGroup { get; set; }

    public static AncillaryBase CurrentAncillary { get; set; }
    public static AncillariesView AncillariesView { get; set; }

  }
}
