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
using System.Windows.Input;

namespace FabricationSample.UserControls.DatabaseEditor
{
    /// <summary>
    /// Interaction logic for DatabaseEditor.xaml
    /// </summary>
    public partial class DatabaseEditor : UserControl
    {
        #region Service Templates

        private void tbiServiceTemplates_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServiceTemplates();

            FabricationManager.ItemFoldersView = new ItemFoldersView();
        }

        private void tbiServiceTemplates_UnLoaded(object sender, RoutedEventArgs e)
        {
            cmbSelectServiceTemplate.ItemsSource = null;
            dgTemplateConditions.ItemsSource = null;
        }


        private void LoadServiceTemplates()
        {
            ListCollectionView lcv = new ListCollectionView(new ObservableCollection<ServiceTemplate>(Database.ServiceTemplates.ToList()));
            lcv.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            cmbSelectServiceTemplate.ItemsSource = null;
            cmbSelectServiceTemplate.ItemsSource = lcv;
            cmbSelectServiceTemplate.Items.Refresh();
        }

        private void cmbSelectServiceTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems != null) && e.AddedItems.Count > 0)
            {
                FabricationManager.CurrentServiceTemplate = e.AddedItems[0] as ServiceTemplate;
                if (FabricationManager.CurrentServiceTemplate != null)
                {
                    cmbSelectServiceTemplate.Text = FabricationManager.CurrentServiceTemplate.Name;
                    cmbSelectServiceTemplate.UpdateLayout();
                    ButtonsTabControl_Templates.Content = new ServiceButtonsView(ServiceButtonsViewType.ServiceTemplates);
                    LoadServiceTemplateConditions();
                }
            }
        }

        private void addServiceTemplate_Click(object sender, RoutedEventArgs e)
        {
            AddServiceTemplateWindow win = new AddServiceTemplateWindow();
            win.ShowDialog();
        }

        private void deleteServiceTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentServiceTemplate == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Template: " + FabricationManager.CurrentServiceTemplate.Name, "Delete Service Template",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteServiceTemplate(FabricationManager.CurrentServiceTemplate))
                {
                    cmbSelectServiceTemplate.SelectedItem = null;
                    FabricationManager.CurrentServiceTemplate = null;
                    FabricationManager.CurrentService = null;

                    var view = ButtonsTabControl_Templates.Content as ServiceButtonsView;
                    if (view != null)
                    {
                        view.CurrentServiceTab = null;
                        view.CurrentServiceButton = null;
                    }

                    LoadServiceTemplates();

                    view = ButtonsTabControl_Services.Content as ServiceButtonsView;
                    if (view != null)
                    {
                        view.CurrentServiceTab = null;
                        view.CurrentServiceButton = null;
                        LoadServices(null);
                    }
                }
            }
        }

        private void editServiceTemplateName_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentServiceTemplate == null)
                return;

            EditNameWindow win = new EditNameWindow(null, FabricationManager.CurrentServiceTemplate.Name, null);
            win.ShowDialog();
            if (win.Completed)
            {
                FabricationManager.CurrentServiceTemplate.Name = win.NewName;
                LoadServiceTemplates();
                cmbSelectServiceTemplate.SelectedItem = FabricationManager.CurrentServiceTemplate;
            }
        }

        public void addServiceTemplate(string serviceTemplateName)
        {
            ServiceTemplate newServiceTemplate = FabricationAPIExamples.AddNewServiceTemplate(serviceTemplateName);
            if (newServiceTemplate == null)
                return;

            // reload the service templates and switch to the new one
            LoadServiceTemplates();
            //Refresh services also
            LoadServices(null);
            ListCollectionView lcv = cmbSelectServiceTemplate.ItemsSource as ListCollectionView;
            if (lcv == null)
                return;

            foreach (ServiceTemplate st in lcv)
            {
                if (newServiceTemplate.Id == st.Id)
                {
                    cmbSelectServiceTemplate.SelectedItem = newServiceTemplate;
                    break;
                }
            }

            var view = ButtonsTabControl_Templates.Content as ServiceButtonsView;
            if (view != null)
            {
                view.CurrentServiceTab = null;
                view.CurrentServiceButton = null;
            }

        }

        private void addServiceTab_Click(object sender, RoutedEventArgs e)
        {
            AddServiceTabWindow win = new AddServiceTabWindow();
            win.ShowDialog();
        }

        public void addServiceTab(string tabName)
        {
            if (FabricationManager.CurrentServiceTemplate == null)
                return;

            ServiceTab newServiceTab = FabricationAPIExamples.AddNewServiceTab(FabricationManager.CurrentServiceTemplate, tabName);

            if (newServiceTab == null)
                return;

            var serviceButtonsViewer = ButtonsTabControl_Templates.Content as ServiceButtonsView;
            if (serviceButtonsViewer != null)
                serviceButtonsViewer.LoadServiceTabs(newServiceTab.Id);

            var view = ButtonsTabControl_Services.Content as ServiceButtonsView;
            if (view == null)
                return;

            Service service = FabricationManager.CurrentService;
            if (service == null || service.ServiceTemplate == null)
                return;
            if (service.ServiceTemplate.Id != FabricationManager.CurrentServiceTemplate.Id)
                return;

            view.LoadServiceTabs(-1);
        }

        private void addServiceButton_Click(object sender, RoutedEventArgs e)
        {
            AddServiceButtonWindow win = new AddServiceButtonWindow();
            win.ShowDialog();
        }

        public void addServiceButton(string buttonName)
        {
            var view = ButtonsTabControl_Templates.Content as ServiceButtonsView;
            if (view == null || view.CurrentServiceTab == null)
                return;

            ServiceButton newButton = FabricationAPIExamples.AddNewServiceButton(view.CurrentServiceTab, buttonName);

            if (newButton == null)
                return;

            var buttonsViewer = ButtonsTabControl_Templates.Content as ServiceButtonsView;
            if (buttonsViewer != null)
                buttonsViewer.LoadServiceButtons();

            view = ButtonsTabControl_Services.Content as ServiceButtonsView;
            if (view == null)
                return;

            Service service = FabricationManager.CurrentService;
            if (service == null || service.ServiceTemplate == null)
                return;
            if (service.ServiceTemplate.Id != FabricationManager.CurrentServiceTemplate.Id)
                return;

            view.LoadServiceTabs(-1);
        }

        private void dgTemplateConditions_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadServiceTemplateConditions();
        }

        private void LoadServiceTemplateConditions()
        {
            if (FabricationManager.CurrentServiceTemplate == null)
                return;

            var conditions = new ObservableCollection<FabServiceTemplateCondition>();

            FabricationManager.CurrentServiceTemplate.Conditions.ToList().ForEach((x) =>
            {
                conditions.Add(new FabServiceTemplateCondition(x));
            });

            dgTemplateConditions.ItemsSource = conditions;
        }

        private void dgTemplateConditions_rowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var item = e.Row.Item as FabServiceTemplateCondition;
            if (item == null)
                return;

            if (!item.Description.Equals(item.Condition.Description))
                item.Condition.Description = item.Description;

            double lessThan = 0;
            if (item.LessThanOrEqual.Equals("Unrestricted"))
                lessThan = -1;
            else
                lessThan = Convert.ToDouble(item.LessThanOrEqual);

            double greaterThan = 0;
            if (item.GreaterThan.Equals("Unrestricted"))
                greaterThan = -1;
            else
                greaterThan = Convert.ToDouble(item.GreaterThan);

            if (lessThan != item.Condition.LessThanEqualTo || greaterThan != item.Condition.GreaterThan)
            {
                bool modified = FabricationAPIExamples.SetServiceTemplateConditionValues(item.Condition, greaterThan, lessThan);
                if (!modified)
                {
                    // reset the values
                    string lessThanString, greaterThanString;
                    if (item.Condition.LessThanEqualTo == -1)
                        lessThanString = "Unrestricted";
                    else
                        lessThanString = item.Condition.LessThanEqualTo.ToString();

                    if (item.Condition.GreaterThan == -1)
                        greaterThanString = "Unrestricted";
                    else
                        greaterThanString = item.Condition.GreaterThan.ToString();

                    item.LessThanOrEqual = lessThanString;
                    item.GreaterThan = greaterThanString;
                }
            }
        }

        private void addServiceTemplateCondition_Click(object sender, RoutedEventArgs e)
        {
            AddServiceTemplateConditionWindow win = new AddServiceTemplateConditionWindow();
            win.ShowDialog();
        }

        public void addServiceTemplateCondition(string description, string greaterThan, string lessThan)
        {
            if (FabricationManager.CurrentServiceTemplate == null)
                return;

            double greaterThanValue = Convert.ToDouble(greaterThan);
            double lessThanValue = Convert.ToDouble(lessThan);

            var condition = FabricationAPIExamples.AddNewServiceTemplateCondition(FabricationManager.CurrentServiceTemplate, description, greaterThanValue, lessThanValue);
            if (condition != null)
            {
                LoadServiceTemplateConditions();
            }
        }

        private void deleteTemplateCondition_Click(object sender, RoutedEventArgs e)
        {
            var condition = dgTemplateConditions.SelectedItem as FabServiceTemplateCondition;
            if (condition == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Template Condition: " + condition.Description, "Delete Service Template Condition",
          MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteServiceTemplateCondition(FabricationManager.CurrentServiceTemplate, condition.Condition))
                    LoadServiceTemplateConditions();
            }
        }

        private void btnSaveServiceTemplates_Click(object sender, RoutedEventArgs e)
        {
            DBOperationResult result = Database.SaveServices();
            MessageBoxImage image = MessageBoxImage.Information;

            if (result.Status == ResultStatus.Failed)
                image = MessageBoxImage.Exclamation;

            MessageBox.Show(result.Message, "Service Templates",
              MessageBoxButton.OK, image);

        }

        private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        #endregion
    }
}


