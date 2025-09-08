using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Dynamic;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;
using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;
using Autodesk.Fabrication.Units;
using Autodesk.Fabrication.LineWeights;

//Use data mapping classes for control bindings
namespace FabricationSample.Data
{
    public class ConnectorMapper : INotifyPropertyChanged
    {
        public string Index { get; set; }
        public string ConnName { get; set; }
        public ConnectorInfo Conn { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SeamMapper : INotifyPropertyChanged
    {
        public string Index { get; set; }
        public string SeamName { get; set; }
        public SeamInfo Seam { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DamperMapper : INotifyPropertyChanged
    {
        public string Index { get; set; }
        public string DamperName { get; set; }
        public DamperInfo Damper { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class CustomDataMapper : INotifyPropertyChanged
    {
        public bool OnItem { get; set; }
        public string Value { get; set; }
        public CustomData Entry { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class OptionMapper : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string OptionType { get; set; }
        public bool IsLocked { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DimensionMapper : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string DimensionType { get; set; }
        public bool IsLocked { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ConnectionPointMapper : INotifyPropertyChanged
    {
        public Item Item { get; set; }
        public ConnectionType ConnType { get; set; }
        public Point3D Location { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ServiceEntryMapper : INotifyPropertyChanged
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeDescription { get; set; }
        public string LayerTag1 { get; set; }
        public string LayerTag2 { get; set; }
        public int LayerColor { get; set; }
        public string LevelBlock { get; set; }
        public bool IncludesInsulation { get; set; }
        public string SizeBlock { get; set; }
        public LineWeight.LineWeightEnum LineWeight { get; set; }

        public ServiceEntry ServiceEntry { get; set; }

        public ServiceEntryMapper(ServiceEntry entry)
        {
            ServiceEntry = entry;

            ServiceTypeId = entry.ServiceType.Id;
            ServiceTypeDescription = entry.ServiceType.Description;
            LayerTag1 = entry.LayerTag1;
            LayerTag2 = entry.LayerTag2;
            LayerColor = entry.LayerColor;
            LevelBlock = entry.LevelBlock;
            IncludesInsulation = entry.IncludesInsulation;
            SizeBlock = entry.SizeBlock;
            LineWeight = entry.LineWeight.LineWeightValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum ProductEntryCostedBy
    {
        Length, Each
    }

    public class ProductEntryGridItem : INotifyPropertyChanged
    {
        private double _productEntryValue;
        private ProductEntryStatus _status;
        private DateTime? _date;
        private ProductEntryCostedBy _costedBy;
        private string _discountCode;

        public event PropertyChangedEventHandler PropertyChanged;

        public SupplierGroup SupplierGroup { get; set; }
        public ProductEntry Entry { get; set; }

        public ProductEntryStatus Status
        {
            get { return _status; }
            set
            {
                Entry.Status = value;
                _status = value;
            }
        }
        public double ProductEntryValue
        {
            get { return _productEntryValue; }
            set
            {
                Entry.Value = value;
                _productEntryValue = value;
            }
        }
        public DateTime? date
        {
            get { return _date; }
            set
            {
                if (value.HasValue)
                {
                    Entry.Date = value.Value;
                    _date = value;
                }
            }
        }

        public ProductEntryCostedBy CostedBy
        {
            get { return Entry.CostedByLength ? ProductEntryCostedBy.Length : ProductEntryCostedBy.Each; }
            set
            {
                bool costedBy = value == ProductEntryCostedBy.Length ? true : false;
                Entry.CostedByLength = costedBy;
                _costedBy = value;
            }
        }

        public String DiscountCode
        {
            get { return _discountCode; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Discount discount = SupplierGroup.Discounts.Discounts.FirstOrDefault(x => x.Code == value);
                    if (discount != null)
                    {
                        Entry.DiscountCode = discount.Code;
                        _discountCode = value;
                    }

                }

            }
        }

        public string SupplierId { get; set; }

        public ProductEntryGridItem()
        {

        }

        public ProductEntryGridItem(ProductEntry productEntry, SupplierGroup supplierGroup)
        {
            SupplierGroup = supplierGroup;
            Entry = productEntry;
            SupplierId = "";
            _productEntryValue = Entry.Value;
            _status = Entry.Status;
            _date = Entry.Date;
            _discountCode = Entry.DiscountCode == null ? string.Empty : Entry.DiscountCode;
        }
    }

    public class CustomDataGridItem : INotifyPropertyChanged
    {
        public String AddMode { get; set; }
        public int Id { get; set; }
        public String Description { get; set; }
        public String DataType { get; set; }
        public String Value { get; set; }
        public bool Exists { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CustomDataGridItem()
        {
            AddMode = "Always";
            Id = 0; // Automatically assigns the next available id if zero.
            DataType = "String";
        }
    }

    public class JobStatusGridItem : INotifyPropertyChanged
    {
      public String Description { get; set; }
      public bool Active { get; set; }
      public DateTime LastActivated { get; set; }
      public bool DoSave { get; set; }
      public JobStatusAction DoCopy { get; set; }
      public String CopyJobToFolder { get; set; }
      public bool DoExport { get; set; }
      public String ExportFile { get; set; }
      public bool DeActivateOnCompletion { get; set; }
      public bool Exists { get; set; }
      public JobStatus Status { get; set; }

      public event PropertyChangedEventHandler PropertyChanged;

      public JobStatusGridItem()
      {
         Description = "Untitled";
         Active = false;
         LastActivated = DateTime.Now;
         DoSave = false;
         DoCopy = JobStatusAction.Nothing;
         CopyJobToFolder = "";
         DoExport = false;
         ExportFile = "";
         DeActivateOnCompletion = false;
      }
   }

   public class ItemStatusGridItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String LayerTag { get; set; }
        public int Color { get; set; }
        public bool Output { get; set; }
        public bool Exists { get; set; }
        public ItemStatus Status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ServiceTypeGridItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ServiceType Entry { get; set; }
        public bool CanModify { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    #region Product List

    public class ProductListGridItemDimension : INotifyPropertyChanged
    {
        private ItemProductListDataRow _parentRow;
        private ItemProductListDimensionEntry _entry;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _entry.Definition.Name; }
        }

        public double DimensionValue
        {
            get { return _entry.Value; }
            set { _parentRow.SetDimensionValue(_entry.Definition, value); OnPropertyChanged("DimensionValue"); }
        }

        public ProductListGridItemDimension(ItemProductListDataRow parentRow, ItemProductListDimensionEntry entry)
        {
            _parentRow = parentRow;
            _entry = entry;
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class ProductListGridItemOption : INotifyPropertyChanged
    {
        private ItemProductListDataRow _parentRow;
        private ItemProductListOptionEntry _entry;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _entry.Definition.Name; }
        }

        public double OptionValue
        {
            get { return _entry.Value; }
            set { _parentRow.SetOptionValue(_entry.Definition, value); OnPropertyChanged("DimensionValue"); }
        }

        public ProductListGridItemOption(ItemProductListDataRow parentRow, ItemProductListOptionEntry entry)
        {
            _parentRow = parentRow;
            _entry = entry;
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class ProductListGridItem : INotifyPropertyChanged
    {
        private ItemProductListDataRow _row;
        private ObservableCollection<ProductListGridItemDimension> _dimensions;
        private ObservableCollection<ProductListGridItemOption> _options;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public ItemProductListDataRow Row
        {
            get { return _row; }
        }
        public string Name
        {
            get { return Row.Name; }
            set { Row.SetName(value); }
        }
        public double? Area
        {
            get { return Row.Area; }
            set { _row.SetArea(value.Value); OnPropertyChanged("Area"); }
        }
        public double? Weight
        {
            get { return Row.Weight; }
            set { _row.SetWeight(value.Value); OnPropertyChanged("Weight"); }
        }
        public string Alias
        {
            get { return Row.Alias; }
            set { _row.SetAlias(value); OnPropertyChanged("Alias"); }
        }
        public string OrderNumber
        {
            get { return Row.OrderNumber; }
            set { _row.SetOrderNumber(value); OnPropertyChanged("OrderNumber"); }
        }
        public string CADBlockName
        {
            get { return Row.CADBlockName; }
            set { _row.SetCadBlockName(value); OnPropertyChanged("CADBlockName"); }
        }
        public string DatabaseId
        {
            get { return Row.DatabaseId; }
            set { _row.SetDatabaseId(value); }
        }
        public bool? BoughtOut
        {
            get { return Row.BoughtOut; }
            set { _row.SetBoughtOut(value.Value); OnPropertyChanged("DatabaseId"); }
        }
        public double? MinimumFlow
        {
            get { return Row.MinimumFlow; }
            set { _row.SetMinimumFlow(value.Value); OnPropertyChanged("MinimumFlow"); }
        }
        public double? MaximumFlow
        {
            get { return Row.MaximumFlow; }
            set { _row.SetMaximumFlow(value.Value); OnPropertyChanged("MaximumFlow"); }
        }
        public ObservableCollection<ProductListGridItemDimension> Dimensions
        {
            get { return _dimensions; }
        }

        public ObservableCollection<ProductListGridItemOption> Options
        {
            get { return _options; }
        }

        public ProductListGridItem(ItemProductListDataRow row)
        {
            _row = row;

            _dimensions = new ObservableCollection<ProductListGridItemDimension>();
            _options = new ObservableCollection<ProductListGridItemOption>();

            if ((_row.Dimensions != null) && _row.Dimensions.Count > 0)
            {
                foreach (ItemProductListDimensionEntry entry in row.Dimensions)
                {
                    _dimensions.Add(new ProductListGridItemDimension(_row, entry));
                }
            }

            if ((_row.Options != null) && _row.Options.Count > 0)
            {
                foreach (ItemProductListOptionEntry entry in row.Options)
                {
                    _options.Add(new ProductListGridItemOption(_row, entry));

                }
            }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public enum ProductListDataFieldType
    {
        Alias, Area, CADBlockName, OrderNumber, Weight, DatabaseId, BoughtOut, Flow, Dimension
    }

    public class ProductListDataField : INotifyPropertyChanged
    {
        private bool _isUsed;
        private ItemProductListDataTemplate _template;
        private ProductListDataFieldType _field;
        private string _name;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public ItemProductListDataTemplate Template
        {
            get { return _template; }
        }

        public bool IsUsed
        {
            get
            {
                switch (_field)
                {
                    case ProductListDataFieldType.Alias:
                        _isUsed = _template.UseAlias;
                        break;
                    case ProductListDataFieldType.Area:
                        _isUsed = _template.UseArea;
                        break;
                    case ProductListDataFieldType.CADBlockName:
                        _isUsed = _template.UseCadBlockName;
                        break;
                    case ProductListDataFieldType.OrderNumber:
                        _isUsed = _template.UseOrderNumber;
                        break;
                    case ProductListDataFieldType.Weight:
                        _isUsed = _template.UseWeight;
                        break;
                    case ProductListDataFieldType.DatabaseId:
                        _isUsed = _template.UseDatabaseId;
                        break;
                    case ProductListDataFieldType.BoughtOut:
                        _isUsed = _template.UseBoughtOut;
                        break;
                    case ProductListDataFieldType.Flow:
                        _isUsed = _template.UseFlow;
                        break;
                    default:
                        break;
                }
                return _isUsed;
            }

            set
            {
                _isUsed = value;
                OnPropertyChanged("IsUsed");
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public ProductListDataFieldType Field
        {
            get { return _field; }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ProductListDataField(string name, ProductListDataFieldType field, ItemProductListDataTemplate template)
        {
            _name = name;
            _field = field;
            _template = template;
        }
    }

    public class ProductListDimensionField : INotifyPropertyChanged
    {
        private ItemProductListDataTemplate _template;
        private ItemDimensionBase _dimension;
        private bool _isUsed;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public ItemProductListDataTemplate Template
        {
            get { return _template; }
        }

        public bool IsUsed
        {
            get
            {
                _isUsed = _template.IsDimensionUsed(_dimension);
                return _isUsed;
            }

            set
            {
                _isUsed = value;
                OnPropertyChanged("IsUsed");
            }
        }

        public ItemDimensionBase Dimension
        {
            get
            {
                return _dimension;
            }

            set
            {
                _dimension = value;
                OnPropertyChanged("Dimension");
            }
        }

        public string Name
        {
            get { return _dimension.Name; }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ProductListDimensionField(ItemDimensionBase baseDimension, ItemProductListDataTemplate template)
        {
            _template = template;
            _dimension = baseDimension;
        }
    }

    public class ProductListOptionField : INotifyPropertyChanged
    {
        private ItemProductListDataTemplate _template;
        private ItemOptionBase _option;
        private bool _isUsed;

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public ItemProductListDataTemplate Template
        {
            get { return _template; }
        }

        public bool IsUsed
        {
            get
            {
                _isUsed = _template.IsOptionUsed(_option);
                return _isUsed;
            }

            set
            {
                _isUsed = value;
                OnPropertyChanged("IsUsed");
            }
        }

        public ItemOptionBase Option
        {
            get
            {
                return _option;
            }

            set
            {
                _option = value;
                OnPropertyChanged("Option");
            }
        }

        public string Name
        {
            get { return _option.Name; }
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ProductListOptionField(ItemOptionBase baseOption, ItemProductListDataTemplate template)
        {
            _template = template;
            _option = baseOption;
        }
    }


    #endregion

    #region MAPProd Data

    public class MapProdGridItem : ObservableCollection<MapProdDataField>
    {
        private ProductDefinition _definition;

        public MapProdGridItem(ProductDefinition definition)
        {
            _definition = definition;

            this.Add(new MapProdDataField("Description", _definition.Description));
            this.Add(new MapProdDataField("Finish", _definition.Finish));
            this.Add(new MapProdDataField("Group", _definition.Group == null ? string.Empty : _definition.Group.Name));
            this.Add(new MapProdDataField("Id", _definition.Id));
            this.Add(new MapProdDataField("InstallType", _definition.InstallType));
            this.Add(new MapProdDataField("Manufacturer", _definition.Manufacturer));
            this.Add(new MapProdDataField("Material", _definition.Material));
            this.Add(new MapProdDataField("Product", _definition.ProductName));
            this.Add(new MapProdDataField("Range", _definition.Range));
            this.Add(new MapProdDataField("Size", _definition.Size));
            this.Add(new MapProdDataField("Specification", _definition.Specification));

            _definition.SupplierIds.ToList().ForEach(x => this.Add(new MapProdDataField(x.ProductSupplier.Name, x.Id)));

        }

        // Declare the event 
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnCollectionChanged method to raise the event 
        protected void OnCollectionChanged(string name)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class MapProdDataField : INotifyPropertyChanged
    {
        private string _name;
        private string _value;

        public MapProdDataField(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    #endregion

    #region Services

    public class FabServiceTab : INotifyPropertyChanged
    {
        public ServiceTab Tab { get; set; }
        public string Name { get; set; }
        public ObservableCollection<FabServiceButton> Buttons { get; set; }

        public FabServiceTab(ServiceTab tab)
        {
            Tab = tab;
            Name = tab.Name;

            Buttons = new ObservableCollection<FabServiceButton>();
            foreach (ServiceButton b in tab.ServiceButtons)
                Buttons.Add(new FabServiceButton(b));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FabServiceButton : INotifyPropertyChanged
    {
        public ServiceButton Button { get; set; }

        public ImageSource Image
        {
            get
            {
                var img = Button.GetButtonImageFilename();
                if (File.Exists(img))
                    return new BitmapImage(new Uri(img));
                else
                    return null;
            }
        }

        public FabServiceButton(ServiceButton button)
        {
            Button = button;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FabServiceButtonItem : INotifyPropertyChanged
    {
        public double LessThan { get; set; }
        public double GreaterThan { get; set; }
        public ServiceTemplateCondition TemplateCondition { get; set; }
        public ServiceButtonItem ButtonItem { get; set; }

        public FabServiceButtonItem(ServiceButtonItem buttonItem)
        {
            ButtonItem = buttonItem;
            TemplateCondition = buttonItem.ServiceTemplateCondition;
            LessThan = ButtonItem.LessThanEqualTo;
            GreaterThan = ButtonItem.GreaterThan;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FabServiceTemplateCondition : INotifyPropertyChanged
    {
        private double _lessThanOrEqual;
        private double _greaterThan;

        public ServiceTemplateCondition Condition { get; set; }
        public string Description { get; set; }
        public string LessThanOrEqual
        {
            get
            {
                if (_lessThanOrEqual == -1)
                    return "Unrestricted";

                return _lessThanOrEqual.ToString();
            }
            set
            {
                if (value.ToLower().Equals("unrestricted"))
                {
                    _lessThanOrEqual = -1;
                    return;
                }

                double result = 0;
                bool isNumber = double.TryParse(value, out result);
                if (!isNumber)
                    return;

                _lessThanOrEqual = result;
                OnPropertyChanged("LessThanOrEqual");
            }
        }

        public string GreaterThan
        {
            get
            {
                if (_greaterThan == -1)
                    return "Unrestricted";

                return _greaterThan.ToString();
            }
            set
            {
                if (value.ToLower().Equals("unrestricted"))
                {
                    _greaterThan = -1;
                    return;
                }

                double result = 0;
                bool isNumber = double.TryParse(value, out result);
                if (!isNumber)
                    return;

                _greaterThan = result;
                OnPropertyChanged("LessThanOrEqual");
            }
        }

        // Declare the event 
        public event PropertyChangedEventHandler PropertyChanged;

        public FabServiceTemplateCondition(ServiceTemplateCondition condition)
        {
            Condition = condition;
            Description = condition.Description;
            _greaterThan = condition.GreaterThan;
            _lessThanOrEqual = condition.LessThanEqualTo;
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    #endregion

    #region BreakPoint Values

    public class BreakPointMappingValue : INotifyPropertyChanged
    {
        public double Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class BreakPointMappingValueRow : INotifyPropertyChanged
    {
        public ObservableCollection<BreakPointMappingValue> Values { get; set; }

        public BreakPointMappingValueRow()
        {
            Values = new ObservableCollection<BreakPointMappingValue>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }




  #endregion

    #region PriceListGrouping

    public class PriceListGrouping
    {
      public string Group { get; private set; }
      public PriceListBase PriceList { get; set; }

      public PriceListGrouping(string group, PriceListBase priceList)
      {
        if (string.IsNullOrWhiteSpace(group))
          Group = "None";
        else
          Group = group;

        PriceList = priceList;
      }
    }

    #endregion
}
