using System;
using System.IO;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Text.RegularExpressions;

using Autodesk.Fabrication;
using Autodesk.Fabrication.Content;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;
using Autodesk.Fabrication.Units;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;

namespace FabricationSample.UserControls.DatabaseEditor
{
   /// <summary>
   /// Interaction logic for DatabaseEditor.xaml
   /// </summary>
   public partial class DatabaseEditor : UserControl
   {
      #region Private Members

      BackgroundWorker _bgwPrices;
      BackgroundWorker _bgwFabtimes;
      BackgroundWorker _bgwInstallationtimes;
      ProductSupplier _selectedSupplier;

      ObservableCollection<CustomDataGridItem> _lstJobCustomData;
      ObservableCollection<CustomDataGridItem> _lstCustomData;
      ObservableCollection<ItemStatusGridItem> _lstItemStatuses;
      ObservableCollection<JobStatusGridItem> _lstJobStatuses;

      #endregion

      #region ctor

      public DatabaseEditor()
      {
         InitializeComponent();
         _bgwPrices = new BackgroundWorker();
         _bgwPrices.WorkerReportsProgress = true;
         _bgwPrices.DoWork += _bgwPrices_DoWork;
         _bgwPrices.ProgressChanged += _bgwPrices_ProgressChanged;
         _bgwPrices.RunWorkerCompleted += _bgwPrices_RunWorkerCompleted;

         _bgwFabtimes = new BackgroundWorker();
         _bgwFabtimes.WorkerReportsProgress = true;
         _bgwFabtimes.DoWork += _bgwFabtimes_DoWork;
         _bgwFabtimes.ProgressChanged += _bgwFabtimes_ProgressChanged;
         _bgwFabtimes.RunWorkerCompleted += _bgwFabTimes_RunWorkerCompleted;

         _bgwInstallationtimes = new BackgroundWorker();
         _bgwInstallationtimes.WorkerReportsProgress = true;
         _bgwInstallationtimes.DoWork += _bgwInstallationtimes_DoWork;
         _bgwInstallationtimes.ProgressChanged += _bgwInstallationtimes_ProgressChanged;
         _bgwInstallationtimes.RunWorkerCompleted += _bgwInstallationtimes_RunWorkerCompleted;
      }

      #endregion

      private void UserControl_Loaded(object sender, RoutedEventArgs e)
      {

      }

      #region BackGround Workers

      void _bgwInstallationtimes_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = sender as BackgroundWorker;
         int i = 0;

         foreach (ProductEntryGridItem prodEntry in FabricationManager.ProductIdInstallationTimesView.dgInstallationTimes.ItemsSource)
         {
            string supplierProdId = ProductDatabase.LookUpSupplierId(prodEntry.Entry.DatabaseId, _selectedSupplier.Id);
            prodEntry.SupplierId = supplierProdId;
            worker.ReportProgress(i);
            i++;
         }
      }

      void _bgwInstallationtimes_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         this.Dispatcher.Invoke(delegate
         {
            prgInstallationTimes.Value = e.ProgressPercentage;
         });
      }

      void _bgwInstallationtimes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         prgInstallationTimes.Value = 0;
      }

      void _bgwFabtimes_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = sender as BackgroundWorker;
         int i = 0;

         foreach (ProductEntryGridItem prodEntry in FabricationManager.ProductIdFabricationTimesView.dgFabTimes.ItemsSource)
         {
            string supplierProdId = ProductDatabase.LookUpSupplierId(prodEntry.Entry.DatabaseId, _selectedSupplier.Id);
            prodEntry.SupplierId = supplierProdId;
            worker.ReportProgress(i);
            i++;
         }
      }

      void _bgwFabtimes_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         this.Dispatcher.Invoke(delegate
         {
            prgFabTimes.Value = e.ProgressPercentage;
         });
      }

      void _bgwFabTimes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         prgFabTimes.Value = 0;
      }

      void _bgwPrices_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         prgPriceList.Value = 0;
      }

      void _bgwPrices_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         this.Dispatcher.Invoke(delegate
         {
            prgPriceList.Value = e.ProgressPercentage;
         });
      }

      void _bgwPrices_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = sender as BackgroundWorker;
         int i = 0;

         foreach (ProductEntryGridItem prodEntry in FabricationManager.ProductIdPriceListView.dgPrices.ItemsSource)
         {
            string supplierProdId = ProductDatabase.LookUpSupplierId(prodEntry.Entry.DatabaseId, _selectedSupplier.Id);
            prodEntry.SupplierId = supplierProdId;
            worker.ReportProgress(i);
            i++;
         }
      }

      #endregion

      #region Job Custom Data
      private void dgJobCustomData_Loaded(object sender, RoutedEventArgs e)
      {
         LoadJobCustomData();
      }

      private void LoadJobCustomData()
      {
         _lstJobCustomData = new ObservableCollection<CustomDataGridItem>();

         //Create Data Source to Bind to Grid
         foreach (CustomData entry in Database.CustomJobData)
         {
            _lstJobCustomData.Add(new CustomDataGridItem()
            {
               AddMode = entry.AddMode.ToString(),
               Id = entry.Id,
               Description = entry.Description,
               DataType = entry.Type.ToString(),
               Value = parseDefaultValue(entry.DefaultValue),
               Exists = true
            });
         }

         //Bind to Grid
         dgJobCustomData.ItemsSource = _lstJobCustomData;
         cmbJobCustomDataType.ItemsSource = Enum.GetNames(typeof(CustomDataType));
      }

      private void btnUpdateJobCustomData_Click(object sender, RoutedEventArgs e)
      {
         var lstNewCustomData = new ObservableCollection<CustomDataGridItem>(_lstJobCustomData.Where(x => x.Exists == false));
         var lstUpdateCustomData = new ObservableCollection<CustomDataGridItem>(_lstJobCustomData.Where(x => x.Exists == true));

         StringBuilder builder = new StringBuilder();

         //Add New Custom Data to Fabrication Database
         if ((lstNewCustomData != null) && lstNewCustomData.Count > 0)
         {
            foreach (CustomDataGridItem entry in lstNewCustomData)
            {
               int id = entry.Id;
               CustomDataType dataType = (CustomDataType)Enum.Parse(typeof(CustomDataType), entry.DataType, true);
               CustomDataAddMode addMode = (CustomDataAddMode)Enum.Parse(typeof(CustomDataAddMode), entry.AddMode, true);
               string customDataDescription = entry.Description;
               string customDataValue = entry.Value;

               switch (dataType)
               {
                  case CustomDataType.Double:
                     {
                        double val = 0;
                        if (double.TryParse(customDataValue, out val))
                           FabricationAPIExamples.AddJobCustomNumericDataToDB(id, customDataDescription, val);
                        break;
                     }
                  case CustomDataType.Integer:
                     {
                        int val = 0;
                        if (int.TryParse(entry.Value, out val))
                           FabricationAPIExamples.AddJobCustomIntegerDataToDB(id, customDataDescription, val);
                        break;
                     }
                  case CustomDataType.String:
                     FabricationAPIExamples.AddJobCustomStringDataToDB(id, customDataDescription, entry.Value);
                     break;
                  default:
                     break;
               }
            }
         }

         //Update any Existing entries if required
         if ((lstUpdateCustomData != null) && lstUpdateCustomData.Count > 0)
         {
            foreach (CustomDataGridItem entry in lstUpdateCustomData)
            {
               CustomData cData = Database.CustomJobData.FirstOrDefault(x => x.Id == entry.Id);

               if (cData != null)
               {
                  if (entry.Description != cData.Description)
                  {
                     builder.AppendLine("Updating Job Custom Data Description: " + cData.Description + " to " + entry.Description);
                     cData.Description = entry.Description;
                  }

                  if (entry.Value != parseDefaultValue(cData.DefaultValue))
                  {
                     builder.AppendLine("Updating Job Custom Data Default Value: " + parseDefaultValue(cData.DefaultValue) + " to " + entry.Value);
                     updateDefaultValue(cData, entry.Value);
                  }
               }
            }
            Database.SaveCustomData();

            string msg = builder.ToString();
            if (!string.IsNullOrEmpty(msg))
               MessageBox.Show(msg, "Job Custom Data", MessageBoxButton.OK, MessageBoxImage.Information);
         }

         //Reload any changes to grid
         LoadJobCustomData();

         // Update any changes to the custom data on the Job.
         FabricationAPIExamples.UpdateCustomData(false);
      }

      private void btnDeleteJobCustomData_Click(object sender, RoutedEventArgs e)
      {
         CustomDataGridItem item = dgJobCustomData.SelectedItem as CustomDataGridItem;

         if (item != null)
         {
            CustomData data = Database.CustomJobData.FirstOrDefault(x => x.Id == item.Id && x.Description == item.Description);
            FabricationAPIExamples.DeleteJobCustomDataFromDB(data);
            LoadJobCustomData();

            // Update any changes to the custom data on the Job.
            FabricationAPIExamples.UpdateCustomData(false);
         }
      }

      #endregion

      #region Custom Data

      private void dgCustomData_Loaded(object sender, RoutedEventArgs e)
      {
         LoadCustomData();
      }

      private void LoadCustomData()
      {
         _lstCustomData = new ObservableCollection<CustomDataGridItem>();

         //Create Data Source to Bind to Grid
         foreach (CustomData entry in Database.CustomItemData)
         {
            _lstCustomData.Add(new CustomDataGridItem()
            {
               AddMode = entry.AddMode.ToString(),
               Id = entry.Id,
               Description = entry.Description,
               DataType = entry.Type.ToString(),
               Value = parseDefaultValue(entry.DefaultValue),
               Exists = true
            });
         }

         //Bind to Grid
         dgCustomData.ItemsSource = _lstCustomData;
         cmbCustomDataAddMode.ItemsSource = Enum.GetNames(typeof(CustomDataAddMode));
         cmbCustomDataType.ItemsSource = Enum.GetNames(typeof(CustomDataType));

      }

      private void btnUpdateCustomData_Click(object sender, RoutedEventArgs e)
      {
         var lstNewCustomData = new ObservableCollection<CustomDataGridItem>(_lstCustomData.Where(x => x.Exists == false));
         var lstUpdateCustomData = new ObservableCollection<CustomDataGridItem>(_lstCustomData.Where(x => x.Exists == true));

         StringBuilder builder = new StringBuilder();

         //Add New Custom Data to Fabrication Database
         if ((lstNewCustomData != null) && lstNewCustomData.Count > 0)
         {
            foreach (CustomDataGridItem entry in lstNewCustomData)
            {
               int id = entry.Id;
               CustomDataType dataType = (CustomDataType)Enum.Parse(typeof(CustomDataType), entry.DataType, true);
               CustomDataAddMode addMode = (CustomDataAddMode)Enum.Parse(typeof(CustomDataAddMode), entry.AddMode, true);
               string customDataDescription = entry.Description;
               string customDataValue = entry.Value;

               switch (dataType)
               {
                  case CustomDataType.Double:
                     {
                        double val = 0;
                        if (double.TryParse(customDataValue, out val))
                           FabricationAPIExamples.AddCustomNumericDataToDB(id, customDataDescription, val);
                        break;
                     }
                  case CustomDataType.Integer:
                     {
                        int val = 0;
                        if (int.TryParse(entry.Value, out val))
                           FabricationAPIExamples.AddCustomIntegerDataToDB(id, customDataDescription, val);
                        break;
                     }
                  case CustomDataType.String:
                     FabricationAPIExamples.AddCustomStringDataToDB(id, customDataDescription, entry.Value);
                     break;
                  default:
                     break;
               }
            }
         }

         //Update any Existing entries if required
         if ((lstUpdateCustomData != null) && lstUpdateCustomData.Count > 0)
         {
            foreach (CustomDataGridItem entry in lstUpdateCustomData)
            {
               CustomData cData = Database.CustomItemData.FirstOrDefault(x => x.Id == entry.Id);

               if (cData != null)
               {
                  if (entry.Description != cData.Description)
                  {
                     builder.AppendLine("Updating Custom Data Description: " + cData.Description + " to " + entry.Description);
                     cData.Description = entry.Description;
                  }

                  if (entry.Value != parseDefaultValue(cData.DefaultValue))
                  {
                     builder.AppendLine("Updating Custom Data Default Value: " + parseDefaultValue(cData.DefaultValue) + " to " + entry.Value);
                     updateDefaultValue(cData, entry.Value);
                  }
               }
            }
            Database.SaveCustomData();

            string msg = builder.ToString();
            if (!string.IsNullOrEmpty(msg))
               MessageBox.Show(msg, "Custom Data", MessageBoxButton.OK, MessageBoxImage.Information);
         }

         //Reload any changes to grid
         LoadCustomData();

      }

      private void btnDeleteCustomData_Click(object sender, RoutedEventArgs e)
      {
         CustomDataGridItem item = dgCustomData.SelectedItem as CustomDataGridItem;

         if (item != null)
         {
            CustomData data = Database.CustomItemData.FirstOrDefault(x => x.Id == item.Id && x.Description == item.Description);

            FabricationAPIExamples.DeleteCustomDataFromDB(data);
            LoadCustomData();
         }
      }

      private string parseDefaultValue(CustomDataDefaultValue defaultValue)
      {
         string value = string.Empty;

         if (defaultValue.GetType() == typeof(CustomDataDefaultStringValue))
         {
            CustomDataDefaultStringValue sVal = defaultValue as CustomDataDefaultStringValue;
            value = sVal.Value;
         }
         else if (defaultValue.GetType() == typeof(CustomDataDefaultIntegerValue))
         {
            CustomDataDefaultIntegerValue iVal = defaultValue as CustomDataDefaultIntegerValue;
            value = iVal.Value.ToString();
         }
         else if (defaultValue.GetType() == typeof(CustomDataDefaultNumericValue))
         {
            CustomDataDefaultNumericValue dVal = defaultValue as CustomDataDefaultNumericValue;
            value = dVal.Value.ToString();
         }

         return value;
      }

      private void updateDefaultValue(CustomData cData, string newValue)
      {
         if (cData.DefaultValue.GetType() == typeof(CustomDataDefaultStringValue))
         {
            CustomDataDefaultStringValue sVal = cData.DefaultValue as CustomDataDefaultStringValue;
            sVal.Value = newValue;
         }
         else if (cData.DefaultValue.GetType() == typeof(CustomDataDefaultIntegerValue))
         {
            CustomDataDefaultIntegerValue iVal = cData.DefaultValue as CustomDataDefaultIntegerValue;
            int val = 0;
            int.TryParse(newValue, out val);
            iVal.Value = val;
         }
         else if (cData.DefaultValue.GetType() == typeof(CustomDataDefaultNumericValue))
         {
            CustomDataDefaultNumericValue dVal = cData.DefaultValue as CustomDataDefaultNumericValue;
            double val = 0;
            double.TryParse(newValue, out val);
            dVal.Value = val;
         }
      }

      #endregion

      #region Job Statuses
      private void dgJobStatuses_Loaded(object sender, RoutedEventArgs e)
      {
         LoadJobStatuses();
      }

      private void btnUpdateJobStatuses_Click(object sender, RoutedEventArgs e)
      {
         StringBuilder builder = new StringBuilder();

         var lstAddStatus = _lstJobStatuses.Where(x => !x.Exists).ToList();
         var lstModStatus = _lstJobStatuses.Where(x => x.Exists && (x.Description != x.Status.Description ||
                                                                    x.Active != x.Status.Active ||
                                                                    x.LastActivated != x.Status.LastActivated ||
                                                                    x.DoSave != x.Status.DoSave ||
                                                                    x.DoCopy != x.Status.DoCopy || 
                                                                    x.CopyJobToFolder != x.Status.CopyJobToFolder ||
                                                                    x.DoExport != x.Status.DoExport ||
                                                                    x.ExportFile != x.Status.ExportFile ||
                                                                    x.DeActivateOnCompletion != x.Status.DeActivateOnCompletion)).ToList();
         if (lstAddStatus.Count > 0)
         {
            foreach (JobStatusGridItem status in lstAddStatus)
            {
               JobStatus added = FabricationAPIExamples.AddJobStatusToDB(status.Description);
               if(added != null)
               {
                  added.Active = status.Active;
                  added.LastActivated = status.LastActivated;
                  added.DoSave = status.DoSave;
                  added.DoCopy = status.DoCopy;
                  added.CopyJobToFolder = status.CopyJobToFolder;
                  added.DoExport = status.DoExport;
                  added.ExportFile = status.ExportFile;
                  added.DeActivateOnCompletion = status.DeActivateOnCompletion;
               }
            }
         }
         if (lstModStatus.Count > 0)
         {
            foreach (JobStatusGridItem status in lstModStatus)
            {
               status.Status.Description = status.Description;
               status.Status.Active = status.Active;
               status.Status.LastActivated = status.LastActivated;
               status.Status.DoSave = status.DoSave;
               status.Status.DoCopy = status.DoCopy;
               status.Status.CopyJobToFolder = status.CopyJobToFolder;
               status.Status.DoExport = status.DoExport;
               status.Status.ExportFile = status.ExportFile;
               status.Status.DeActivateOnCompletion = status.DeActivateOnCompletion;
            }
         }
         if (lstModStatus.Count > 0 || lstAddStatus.Count > 0)
         {
            Database.SaveJobStatuses();
         }
         LoadJobStatuses();
      }

      private void btnDeleteJobStatuses_Click(object sender, RoutedEventArgs e)
      {
         JobStatusGridItem item = dgJobStatuses.SelectedItem as JobStatusGridItem;

         if (item != null)
         {
            FabricationAPIExamples.DeleteJobStatusFromDB(item.Status);
            Database.SaveJobStatuses();
            LoadJobStatuses();
         }
      }

      private void LoadJobStatuses()
      {
         _lstJobStatuses = new ObservableCollection<JobStatusGridItem>();

         foreach (JobStatus status in Database.JobStatuses)
         {
            _lstJobStatuses.Add(new JobStatusGridItem()
            {
               Description = status.Description,
               Active = status.Active,
               LastActivated = status.LastActivated,
               DoSave = status.DoSave,
               DoCopy = status.DoCopy,
               CopyJobToFolder = status.CopyJobToFolder,
               DoExport = status.DoExport,
               DeActivateOnCompletion = status.DeActivateOnCompletion,
               Exists = true,
               Status = status
            });
         }
         dgJobStatuses.ItemsSource = _lstJobStatuses;
         cmbJobStatusAction.ItemsSource = Enum.GetNames(typeof(JobStatusAction));
      }
      #endregion

      #region Item Statuses

      private void dgItemStatuses_Loaded(object sender, RoutedEventArgs e)
      {
         LoadItemStatuses();
      }

      private void btnUpdateItemStatuses_Click(object sender, RoutedEventArgs e)
      {
         StringBuilder builder = new StringBuilder();

         var lstAddStatus = _lstItemStatuses.Where(x => !x.Exists).ToList();
         var lstModStatus = _lstItemStatuses.Where(x => x.Exists && (x.Id != x.Status.Id ||
                                                     x.Name != x.Status.Name ||
                                                     x.Color != x.Status.Color ||
                                                     x.LayerTag != x.Status.LayerTag ||
                                                     x.Output != x.Status.Output)).ToList();

         if (lstAddStatus.Count > 0)
         {
            foreach (ItemStatusGridItem status in lstAddStatus)
            {
               FabricationAPIExamples.AddItemStatusToDB(status.Name, status.LayerTag, status.Output, status.Color);
            }
         }

         if (lstModStatus.Count > 0)
         {
            foreach (ItemStatusGridItem status in lstModStatus)
            {
               status.Status.Id = status.Id;
               status.Status.Name = status.Name;
               status.Status.LayerTag = status.LayerTag;
               status.Status.Output = status.Output;
               status.Status.Color = status.Color;
            }
         }

         if (lstModStatus.Count > 0 || lstAddStatus.Count > 0)
         {
            Database.SaveItemStatuses();
         }

         LoadItemStatuses();
      }

      private void btnDeleteItemStatuses_Click(object sender, RoutedEventArgs e)
      {
         ItemStatusGridItem item = dgItemStatuses.SelectedItem as ItemStatusGridItem;

         if (item != null)
         {
            FabricationAPIExamples.DeleteItemStatusFromDB(item.Status);
            Database.SaveItemStatuses();
            LoadItemStatuses();
         }

      }

      private void LoadItemStatuses()
      {
         _lstItemStatuses = new ObservableCollection<ItemStatusGridItem>();

         foreach (ItemStatus status in Database.ItemStatuses)
         {
            _lstItemStatuses.Add(new ItemStatusGridItem()
            {
               Id = status.Id,
               Name = status.Name,
               Color = status.Color,
               LayerTag = status.LayerTag,
               Output = status.Output,
               Exists = true,
               Status = status
            });
         }

         dgItemStatuses.ItemsSource = _lstItemStatuses;
      }

      #endregion

      #region Point Location

      private void tbiPointLocate_Loaded(object sender, RoutedEventArgs e)
      {
         var lstConnTypes = new ObservableCollection<string>(Enum.GetNames(typeof(ConnectionType)));
         lstConnTypes.Insert(0, "All");
         cmbFilterPoints.ItemsSource = lstConnTypes;
         cmbFilterPoints.SelectedIndex = 0;
      }

      private void btnPointLocate_Click(object sender, RoutedEventArgs e)
      {
         var lstConnPoints = new ObservableCollection<ConnectionPointMapper>();
         ConnectionType filterType;
         Enum.TryParse(cmbFilterPoints.Text, out filterType);

         foreach (Item item in Job.Items)
         {
            for (int i = 0; i < item.Connectors.Count; i++)
            {
               ConnectionType connType = item.GetConnectorConnectionType(i);

               if ((connType == filterType) || cmbFilterPoints.Text == "All")
               {
                  lstConnPoints.Add(new ConnectionPointMapper()
                  {
                     ConnType = connType,
                     Location = item.GetConnectorEndPoint(i),
                     Item = item
                  });
               }
            }
         }

         if (lstConnPoints.Count > 0)
            dgPointLocate.ItemsSource = lstConnPoints;
      }

      #endregion

      #region Application

      private void tbiApplication_Loaded(object sender, RoutedEventArgs e)
      {
         txtAppConfigName.Text = Autodesk.Fabrication.ApplicationServices.Application.CurrentConfiguration;
         txtAppExeName.Text = Autodesk.Fabrication.ApplicationServices.Application.ExecutableName;
         txtAppExePath.Text = Autodesk.Fabrication.ApplicationServices.Application.ExecutablePath;
         txtAppWorkingDirectory.Text = Autodesk.Fabrication.ApplicationServices.Application.WorkingDirectory;
         txtAppVersionName.Text = Autodesk.Fabrication.ApplicationServices.Application.VersionName;
         txtAppVersionNumber.Text = Autodesk.Fabrication.ApplicationServices.Application.VersionNumber;
         txtAppProductVersion.Text = Autodesk.Fabrication.ApplicationServices.Application.ProductVersion;
         txtAppProfileName.Text = Autodesk.Fabrication.ApplicationServices.Application.CurrentProfile;
         txtAppItemPath.Text = Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath;
         txtAppDatabase.Text = Autodesk.Fabrication.ApplicationServices.Application.DatabasePath;
         txtAppDatabaseUnits.Text = Database.Units.ToString();
      }

      #endregion

      #region Content Creation

      private bool _itemsHaveLoaded = false;

      private void tbiCreateItem_Loaded(object sender, RoutedEventArgs e)
      {
         //Load Top Level Item Folders once on load
         if (!_itemsHaveLoaded)
         {
            FabricationManager.ItemFoldersView = new ItemFoldersView();
            ItemsFolder_ManageContent.Content = FabricationManager.ItemFoldersView;

            _itemsHaveLoaded = true;
         }
      }

      public void ItemFolders_SelectedItemChange(TreeViewItem trvItm)
      {
         //Check if selection is a file or folder
         //Folder selections have a tag of type Autodesk.Fabrication.Content.IemFolder
         if (trvItm != null)
         {
            string path = string.Empty;
            bool isFile = false;

            if (trvItm.Tag.GetType() == typeof(string))
            {
               path = trvItm.Tag.ToString();
               isFile = path.EndsWith(".itm", StringComparison.InvariantCultureIgnoreCase);
            }

            if (ccItemCreateContent != null)
            {
               Type controlType = ccItemCreateContent.Content == null ? null : ccItemCreateContent.Content.GetType();
               if ((!isFile) && (controlType != typeof(CreateItem) || controlType == null))
                  ccItemCreateContent.Content = new CreateItem();
               else if ((isFile) && controlType != typeof(LoadItem) || controlType == null)
                  ccItemCreateContent.Content = new LoadItem();
            }

            if (isFile)
            {
               FabricationManager.CurrentLoadedItemPath = path;

               if (FabricationManager.EditServiceButtonItemWindow != null)
               {
                  string itemPath = path;
                  if (!String.IsNullOrWhiteSpace(itemPath))
                  {
                     string itemContentPath = Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath;
                     bool contains = itemPath.Contains(itemContentPath);
                     if (contains)
                        itemPath = itemPath.Replace(itemContentPath, "");
                  }

                  FabricationManager.EditServiceButtonItemWindow.txtSelectedItemPath.Text = itemPath;
               }
            }
         }

      }

      #endregion

      #region Supplier Groups + Discounts

      private void cmbSupplierDiscounts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         FabricationManager.CurrentSupplierGroup = e.AddedItems[0] as SupplierGroup;

         // setup the discounts table
         var discounts = new ObservableCollection<Discount>(FabricationManager.CurrentSupplierGroup.Discounts.Discounts.OrderBy(x => x.Code));
         dgSupplierDiscounts.ItemsSource = discounts;

         btnAddDiscount.IsEnabled = true;
         btnDeleteDiscount.IsEnabled = true;
      }

      private void tbiSupplierDiscounts_Loaded(object sender, RoutedEventArgs e)
      {
         var supllierGroups = new ObservableCollection<SupplierGroup>(Database.SupplierGroups.OrderBy(x => x.Name));

         cmbSupplierGroups.ItemsSource = supllierGroups;
         if (FabricationManager.CurrentSupplierGroup != null)
            cmbSupplierGroup.SelectedItem = FabricationManager.CurrentSupplierGroup;
      }

      private void AddDiscount_Click(object sender, RoutedEventArgs e)
      {
         var win = new AddDiscountWindow(FabricationManager.CurrentSupplierGroup.Discounts);
         win.ShowDialog();

         if (win.Discount != null)
         {
            var discounts = dgSupplierDiscounts.ItemsSource as ObservableCollection<Discount>;
            discounts.Add(win.Discount);
         }

      }

      private void DeleteDiscount_Click(object sender, RoutedEventArgs e)
      {
         var discount = dgSupplierDiscounts.SelectedItem as Discount;
         var supplierGroup = FabricationManager.CurrentSupplierGroup;
         if (discount == null || supplierGroup == null)
            return;

         if (FabricationAPIExamples.DeleteDiscount(supplierGroup.Discounts, discount))
         {
            var discounts = dgSupplierDiscounts.ItemsSource as ObservableCollection<Discount>;
            discounts.Remove(discount);
         }
      }

    #endregion



    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
      {
         if (_lstMapProdItems != null)
         {
            _lstMapProdItems.Clear();
            _lstMapProdItems = null;
         }

         GC.Collect();
      }

      public T GetVisualParent<T>(object childObject) where T : Visual
      {
         DependencyObject child = childObject as DependencyObject;
         while ((child != null) && !(child is T))
         {
            child = VisualTreeHelper.GetParent(child);
         }
         return child as T;
      }


  }
}
