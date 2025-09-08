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

        Service _updateService;
        ServiceButtonItem _selectedButtonItem;
        bool _specsInitialised;

        #endregion

        #region Services

        private void tbiServices_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSpecs();

            LoadServices(null);
        }

        public void LoadSpecs()
        {
            _specsInitialised = false;

            // setup specs
            ListCollectionView specs = new ListCollectionView(new ObservableCollection<Specification>(Database.Specifications));
            specs.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            specs.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
            specs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            cmbServiceSpecification.ItemsSource = specs;

            _specsInitialised = true;
        }

        public void LoadServices(Service service)
        {
            // setup service templates
            cmbUseServiceTemplate.ItemsSource = new ObservableCollection<ServiceTemplate>(Database.ServiceTemplates);

            // setup services
            ListCollectionView services = new ListCollectionView(new ObservableCollection<Service>(Database.Services));
            services.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            services.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
            services.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            cmbSelectService.ItemsSource = services;

            if (service != null)
            {
                foreach (Service s in services)
                {
                    if (service.Id == s.Id)
                    {
                        cmbSelectService.SelectedItem = s;
                        break;
                    }
                }
            }
        }

        private void serviceProperties_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && FabricationManager.CurrentService != null)
            {
                try
                {
                    FabricationManager.ParentWindow.LoadServiceEditorControl();
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Error Loading Service Properties", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void cmbUseServiceTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                ServiceTemplate serviceTemplate = e.AddedItems[0] as ServiceTemplate;
                FabricationManager.CurrentService.ServiceTemplate = serviceTemplate;

                ButtonsTabControl_Services.Content = new ServiceButtonsView(ServiceButtonsViewType.Services);

                cmbSelectButtonItem.ItemsSource = null;
                cmbSelectButtonItem.SelectedIndex = -1;
                btnAddItem.IsEnabled = false;
                _selectedButtonItem = null;

                var view = ButtonsTabControl_Services.Content as ServiceButtonsView;
                if (view != null)
                {
                    view.CurrentServiceButton = null;
                }
            }
        }

        private void chkServiceSpecification_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chkServiceSpecification.IsChecked)
            {
                FabricationManager.CurrentService.Specification = null;
                cmbServiceSpecification.IsEnabled = false;
            }
            else
            {
                cmbServiceSpecification.IsEnabled = true;
                if (cmbServiceSpecification.SelectedItem != null)
                    FabricationManager.CurrentService.Specification = cmbServiceSpecification.SelectedItem as Specification;
            }

        }

        private void cmbServiceSpecification_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_specsInitialised)
                return;

            if (e.AddedItems == null || e.AddedItems.Count == 0)
                return;

            if (FabricationManager.CurrentService == null)
                return;

            Specification spec = e.AddedItems[0] as Specification;
            if (spec == null)
                return;

            FabricationManager.CurrentService.Specification = spec;
        }
        private void cmbSelectService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems != null) && e.AddedItems.Count > 0)
            {
                FabricationManager.CurrentService = e.AddedItems[0] as Service;
                if (FabricationManager.CurrentService != null)
                {
                    FabricationManager.CurrentServiceTemplate = FabricationManager.CurrentService.ServiceTemplate;

                    // set the service template
                    if (cmbUseServiceTemplate.ItemsSource != null)
                    {
                        foreach (ServiceTemplate t in cmbUseServiceTemplate.ItemsSource)
                        {
                            if (t.Id == FabricationManager.CurrentServiceTemplate.Id)
                            {
                                cmbUseServiceTemplate.SelectedItem = t;
                                break;
                            }
                        }
                    }

                    if (cmbServiceSpecification.ItemsSource != null)
                    {
                        ListCollectionView specs = cmbServiceSpecification.ItemsSource as ListCollectionView;
                        Specification thisSpec = FabricationManager.CurrentService.Specification;
                        if (thisSpec != null)
                        {
                            cmbServiceSpecification.IsEnabled = true;
                            chkServiceSpecification.IsChecked = false;
                            string specGroup = thisSpec.Group;
                            string specName = thisSpec.Name;
                            bool sameGroup = false;
                            bool sameName = false;
                            foreach (Specification spec in specs)
                            {
                                sameGroup = false;
                                sameName = false;
                                if (String.IsNullOrWhiteSpace(specGroup) && String.IsNullOrWhiteSpace(spec.Group))
                                    sameGroup = true;
                                else if (FabricationManager.CurrentService.Specification.Group.Equals(spec.Group))
                                    sameGroup = true;

                                if (sameGroup)
                                {
                                    if (String.IsNullOrWhiteSpace(specName) && String.IsNullOrWhiteSpace(spec.Name))
                                        sameName = true;

                                    if (specName.Equals(spec.Name))
                                        sameName = true;
                                }

                                if (sameName)
                                {
                                    cmbServiceSpecification.SelectedItem = spec;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            cmbServiceSpecification.IsEnabled = false;
                            chkServiceSpecification.IsChecked = true;
                        }

                    }


                    ButtonsTabControl_Services.Content = new ServiceButtonsView(ServiceButtonsViewType.Services);

                    cmbSelectButtonItem.ItemsSource = null;
                    cmbSelectButtonItem.SelectedIndex = -1;
                    btnAddItem.IsEnabled = false;
                    _selectedButtonItem = null;

                    var view = ButtonsTabControl_Services.Content as ServiceButtonsView;
                    if (view != null)
                    {
                        view.CurrentServiceButton = null;
                    }
                }
            }
        }

        public void ServiceButtonSelected(FabServiceButton button)
        {
            if (cmbSelectButtonItem != null)
            {
                cmbSelectButtonItem.ItemsSource = new ObservableCollection<ServiceButtonItem>(button.Button.ServiceButtonItems);
                cmbSelectButtonItem.SelectedIndex = 0;
            }
        }

        private void cmbSelectButtonItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                ServiceButtonItem buttonItem = e.AddedItems[0] as ServiceButtonItem;
                if (buttonItem != null)
                {
                    string itemPath = buttonItem.ItemPath;
                    if (!String.IsNullOrWhiteSpace(itemPath))
                    {
                        string itemContentPath = Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath;
                        bool contains = itemPath.Contains(itemContentPath);
                        if (contains)
                            itemPath = itemPath.Replace(itemContentPath, "");
                    }

                    labItemPath.Content = itemPath;
                    labLessThan.Content = buttonItem.LessThanEqualTo.ToString();
                    labGreaterThan.Content = buttonItem.GreaterThan.ToString();

                    btnAddItem.IsEnabled = true;
                    _selectedButtonItem = buttonItem;
                }
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var view = ButtonsTabControl_Services.Content as ServiceButtonsView;
            if (view == null)
                return;

            Item item = FabricationAPIExamples.LoadServiceItem(FabricationManager.CurrentService, view.CurrentServiceButton, _selectedButtonItem, true);
            if (item != null)
                FabricationAPIExamples.AddItemToJob(item);
        }

        public void addService(string name, string group, ServiceTemplate serviceTemplate)
        {
            if (serviceTemplate == null)
                return;

            Service newService = FabricationAPIExamples.AddNewService(name, group, serviceTemplate);
            if (newService == null)
                return;

            // reload the services and switch to the new one
            LoadServices(newService);
        }

        private void EditServiceName_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentService == null)
                return;

            EditNameWindow win = new EditNameWindow("Edit Service", FabricationManager.CurrentService.Name, FabricationManager.CurrentService.Group);
            win.ShowDialog();
            if (win.Completed)
            {
                FabricationManager.CurrentService.Name = win.NewName;
                FabricationManager.CurrentService.Group = win.NewGroup;

                LoadServices(FabricationManager.CurrentService);
            }
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWindow win = new AddServiceWindow();
            win.ShowDialog();
        }

        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentService != null)
            {
                if (MessageBox.Show("Confirm to Delete Service", "Delete Service",
                  MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (FabricationAPIExamples.DeleteService(FabricationManager.CurrentService))
                    {
                        FabricationManager.CurrentService = null;
                        LoadServices(null);
                    }
                }
            }
        }

        private void SaveServices_Click(object sender, RoutedEventArgs e)
        {
            DBOperationResult result = Database.SaveServices();
            MessageBoxImage image = MessageBoxImage.Information;

            if (result.Status != ResultStatus.Succeeded)
                image = MessageBoxImage.Exclamation;

            MessageBox.Show(result.Message, "Save Services", MessageBoxButton.OK, image);

        }

        #endregion
    }
}


