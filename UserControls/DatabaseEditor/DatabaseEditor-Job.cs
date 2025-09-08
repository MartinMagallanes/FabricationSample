using Autodesk.Fabrication;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Autodesk.Fabrication.PrintObjects;
using FabricationSample.UserControls.ItemEditor;

namespace FabricationSample.UserControls.DatabaseEditor
{
    /// <summary>
    /// Interaction logic for DatabaseEditor.xaml
    /// </summary>
    public partial class DatabaseEditor : UserControl
    {
        #region Private Members

        int _itemUpdateCount = 0;
        ItemStatus _updateStatus;
        Autodesk.Fabrication.DB.Section _updateSection;
        ObservableCollection<CustomDataGridItem> _lstJobInfoCustomData;
        ObservableCollection<JobStatusGridItem> _lstJobInfoStatuses;

        #endregion

        #region Job Items

        private void dgJobItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    try
                    {
                        FabricationManager.CurrentItem = Job.Items[grid.SelectedIndex];
                        FabricationManager.ParentWindow.LoadItemEditorControl();
                    }
                    catch (Exception)
                    {
                        System.Windows.MessageBox.Show("Error Loading Item Properties", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private void dgJobItems_Loaded(object sender, RoutedEventArgs e)
        {
            dgJobItems.DataContext = new ObservableCollection<Item>(Job.Items);
            cmbItemStatus.ItemsSource = new ObservableCollection<ItemStatus>(Database.ItemStatuses);
            cmbItemStatus.DisplayMemberPath = "Name";
            cmbItemStatus.SelectedValuePath = "Id";

            cmbItemSection.ItemsSource = new ObservableCollection<Section>(Database.Sections);
            cmbItemSection.DisplayMemberPath = "Description";
            cmbItemSection.SelectedValuePath = "Page";

            cmbService.ItemsSource = new ObservableCollection<Service>(Database.Services);
            cmbService.DisplayMemberPath = "Name";
            cmbService.SelectedValuePath = "Id";
        }

        private void cmbItemStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && cmbItemStatus.ItemsSource != null)
                _updateStatus = (ItemStatus)e.AddedItems[0];
        }

        private void cmbItemSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && cmbItemSection.ItemsSource != null)
                _updateSection = (Autodesk.Fabrication.DB.Section)e.AddedItems[0];
        }

        private void cmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && cmbService.ItemsSource != null)
                _updateService = (Autodesk.Fabrication.DB.Service)e.AddedItems[0];
        }

        private void btnUpdateItems_Click(object sender, RoutedEventArgs e)
        {
            //Reset Update Count
            _itemUpdateCount = 0;

            ObservableCollection<Item> lstUpdatedItems = new ObservableCollection<Item>();
            //Update Items with Requires Update Flag set
            foreach (Item itm in Job.Items)
            {
                SetUpdateItemProperties(itm);
                _itemUpdateCount++;
                lstUpdatedItems.Add(itm);
            }

            //Refresh Data Source
            dgJobItems.DataContext = new ObservableCollection<Item>(Job.Items);

            //Update UI View
            Autodesk.Fabrication.UI.UIApplication.UpdateView(lstUpdatedItems.ToList());

            MessageBox.Show(string.Format("{0} Item(s) Updated", _itemUpdateCount), "Items Updated", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void btnSaveJob_Click(object sender, RoutedEventArgs e)
        {
            bool saved = Job.Save();
            if (saved)
                MessageBox.Show("Job Saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Job could not be saved", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }

        private void btnSaveJobAs_Click(object sender, RoutedEventArgs e)
        {
            FabricationAPIExamples.SaveJobAs();
        }

        void SetUpdateItemProperties(Item itm)
        {
            //Update Status
            if (_updateStatus != null)
                itm.Status = _updateStatus;

            //Update Section
            if (_updateSection != null)
                itm.Section = _updateSection;

            //Update Service
            if (_updateService != null)
                itm.Service = _updateService;
        }

        private void btnLocateItemByHandle_Click(object sender, RoutedEventArgs e)
        {
            string acadHandle = txtACADHandle.Text;
            if (!string.IsNullOrEmpty(acadHandle))
            {
                Item item = FabricationAPIACADExamples.GetItemFromACADHandle(acadHandle);
                if (item != null)
                {
                    //Advised to use foreach when parsing Job.Items in CADmep Environment
                    //Accessing by Index results in the entire job being parsed each time and slow down
                    int indx = 0;

                    foreach (Item searchItm in Job.Items)
                    {
                        if (searchItm.UniqueId == item.UniqueId)
                        {
                            dgJobItems.SelectedIndex = indx;
                            break;
                        }
                        indx++;
                    }
                    //Try to avoid
                    //for (int i = 0; i < Job.Items.Count; i++)
                    //{
                    //  if (Job.Items[i].UniqueId == item.UniqueId)
                    //  {
                    //    dgJobItems.SelectedIndex = i;
                    //    break;
                    //  }
                    //}
                }
            }
        }

        private void btnGetHandleFromFabItem_Click(object sender, RoutedEventArgs e)
        {

            if ((dgJobItems.SelectedItems != null) && dgJobItems.SelectedItems.Count == 1)
            {
                try
                {
                    Item item = Job.Items[dgJobItems.SelectedIndex];

                    if (item != null)
                    {
                        string handle = FabricationAPIACADExamples.GetACADHandleForItem(item);
                        if (!string.IsNullOrEmpty(handle))
                            txtGetHandleFromFabItem.Text = handle;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error Locating ACAD Object Handle" + Environment.NewLine + ex.Message.ToString(),
                                                                            "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        private void btnRemoveJobItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgJobItems.SelectedItem != null)
            {
                Item removeItem = dgJobItems.SelectedItem as Item;
                if (removeItem != null)
                {
                    if (FabricationAPIExamples.DeleteJobItem(removeItem))
                        dgJobItems.DataContext = new ObservableCollection<Item>(Job.Items);
                }
            }
        }

        #endregion

        #region Job Information

        private void dgJobInfoStatuses_Loaded(object sender, RoutedEventArgs e)
        {
            LoadJobInfoStatuses();
        }

        private void LoadJobInfoStatuses()
        {
            _lstJobInfoStatuses = new ObservableCollection<JobStatusGridItem>();

            //Create Data Source to Bind to Grid
            foreach (JobStatus entry in Job.Info.Statuses)
            {
              _lstJobInfoStatuses.Add(new JobStatusGridItem()
              {
                Active = entry.Active,
                Description = entry.Description,
                LastActivated = entry.LastActivated,
               });
            }

            //Bind to Grid
            dgJobInfoStatuses.ItemsSource = _lstJobInfoStatuses;
        }

        private void dgJobInfoCustomData_Loaded(object sender, RoutedEventArgs e)
        {
            LoadJobInfoCustomData();
        }

        private void LoadJobInfoCustomData()
        {
            _lstJobInfoCustomData = new ObservableCollection<CustomDataGridItem>();

            //Create Data Source to Bind to Grid
            if(Job.Info.Custom.Count>0)
            {
              foreach (CustomJobData entry in Job.Info.Custom)
              {
                _lstJobInfoCustomData.Add(new CustomDataGridItem()
                {
                    Id = entry.Data.Id,
                    Description = entry.Data.Description,
                    Value = FabricationAPIExamples.GetCustomJobData(entry),
                });
              }
            }

            //Bind to Grid
            dgJobInfoCustomData.ItemsSource = _lstJobInfoCustomData;
        }

        private void tbiJobInformation_Loaded(object sender, RoutedEventArgs e)
        {
            txtJobName.Text = Job.Info.FileName;
            txtReference.Text = Job.Info.Reference;
            txtJobField1.Text = Job.Info.DescriptionField1;
            txtJobField2.Text = Job.Info.DescriptionField2;
            txtJobDate.Text = Job.Info.CreationDate.ToString();
            txtJobCustomer.Text = Job.Info.Customer.Name;
            txtJobCustomerAddress.Text = Job.Info.Customer.Address;
            if (Job.Info.RequiredDate != null)
            {
                dtpRequiredDate.DisplayDate = Job.Info.RequiredDate;
                dtpRequiredDate.Text = Job.Info.RequiredDate.ToString();
            }
        }

        private void btnUpdateJobInfo_Click(object sender, RoutedEventArgs e)
        {
            Job.Info.Reference = txtReference.Text;
            Job.Info.DescriptionField1 = txtJobField1.Text;
            Job.Info.DescriptionField2 = txtJobField2.Text;
            Job.Info.RequiredDate = (DateTime)dtpRequiredDate.SelectedDate;
            foreach(CustomDataGridItem cdgi in _lstJobInfoCustomData)
            {
                foreach (CustomJobData entry in Job.Info.Custom)
                {
                    if(entry.Data.Id == cdgi.Id)
                    {
                        if(FabricationAPIExamples.GetCustomJobData(entry) != cdgi.Value)
                            FabricationAPIExamples.SetCustomJobData(entry, cdgi.Value);
                        break;
                    }
                }
            }
            Job.Save();
        }

      private void CmbJobPO_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
        var newValue = e.AddedItems[0] as JobPrintObjectDisplay;
        if (newValue == null)
          return;

        txtJobPO.Text = PrintObject.GetValue(newValue.Value);
    }

      private void CmbJobPO_OnLoaded(object sender, RoutedEventArgs e)
      {
        var jobPrintObjects = Enum.GetValues(typeof(JobPrintObjectEnum)).Cast<JobPrintObjectEnum>().OrderBy(x => x.ToString());
        var jobPrintObjectDisplays = new List<JobPrintObjectDisplay>();
        jobPrintObjects.ToList().ForEach(x =>
        {
          var valid = PrintObject.IsValid(x);
          if (valid)
            jobPrintObjectDisplays.Add(new JobPrintObjectDisplay(x));
        });

        cmbJobPO.ItemsSource = jobPrintObjectDisplays;
        cmbJobPO.DisplayMemberPath = "DisplayValue";
      }

    #endregion
  }

  public class JobPrintObjectDisplay
  {
    public JobPrintObjectEnum Value { get; private set; }

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

    public JobPrintObjectDisplay(JobPrintObjectEnum value)
    {
      Value = value;
    }
  }
}
