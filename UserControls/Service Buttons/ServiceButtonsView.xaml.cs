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
using Autodesk.Fabrication.DB;
using FabricationSample.Data;
using FabricationSample.Manager;
using FabricationSample.FunctionExamples;

namespace FabricationSample.UserControls
{
    public enum ServiceButtonsViewType
    {
        Services,
        ServiceTemplates
    }

    /// <summary>
    /// Interaction logic for ServiceButtons.xaml
    /// </summary>
    public partial class ServiceButtonsView : UserControl
    {

        #region Private Members

        private ServiceButtonsViewType _viewType;

        #endregion

        #region Public Members

        public ServiceTab CurrentServiceTab { get; set; }
        public ServiceButton CurrentServiceButton { get; set; }

        #endregion

        #region ctor

        public ServiceButtonsView(ServiceButtonsViewType viewType)
        {
            InitializeComponent();

            _viewType = viewType;

            LoadServiceTabs(-1);
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        public void LoadServiceTabs(int id)
        {
            ServiceTemplate serviceTemplate = _viewType == ServiceButtonsViewType.Services ? FabricationManager.CurrentService.ServiceTemplate : FabricationManager.CurrentServiceTemplate;
            if (serviceTemplate == null)
                return;

            var tabs = new ObservableCollection<FabServiceTab>();
            int index = 0;
            foreach (ServiceTab t in serviceTemplate.ServiceTabs)
            {
                tabs.Add(new FabServiceTab(t));

                if (id >= 0 && id == t.Id)
                {
                    index = tabs.Count - 1;
                }
            }

            tbServiceTab.ItemsSource = tabs;
            tbServiceTab.SelectedIndex = index;
        }

        public void LoadServiceButtons()
        {
            var tab = tbServiceTab.SelectedItem as FabServiceTab;
            if (tab == null)
                return;

            LoadServiceTabs(tab.Tab.Id);
        }

        private void lvSelectServiceButton_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                FabServiceButton button = e.AddedItems[0] as FabServiceButton;
                if (button != null)
                {
                    CurrentServiceButton = button.Button;

                    if (FabricationManager.DBEditor != null)
                        FabricationManager.DBEditor.ServiceButtonSelected(button);
                }
            }
        }

        private void tbServiceTab_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                FabServiceTab tab = e.AddedItems[0] as FabServiceTab;
                if (tab != null)
                {
                    CurrentServiceTab = tab.Tab;
                }
            }
        }

        private void buttonContextOpening(object sender, ContextMenuEventArgs e)
        {
            var listView = sender as ListView;
            if (listView == null)
                return;

            FabServiceButton button = listView.SelectedItem as FabServiceButton;
            if (button == null)
                return;

            var image = e.OriginalSource as Image;
            if (image != null && image.ContextMenu != null)
            {
                bool enable = button.Button.ServiceButtonItems.Count > 0;
                foreach (var item in image.ContextMenu.Items)
                {
                    if (item.GetType() == typeof(MenuItem))
                    {
                        var menuItem = item as MenuItem;

                        // conditionally enable/disable these two menu items if there are no button items on the button
                        if (menuItem.Header.Equals("Edit Button Item"))
                            menuItem.IsEnabled = enable;
                        else if (menuItem.Header.Equals("Delete Button Item"))
                            menuItem.IsEnabled = enable;
                        else
                            menuItem.IsEnabled = true;
                    }
                }

            }
        }

        private void deleteServiceTab_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var tab = item.DataContext as FabServiceTab;
            if (tab == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Tab: " + tab.Name, "Delete Service Tab",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                ServiceTemplate serviceTemplate = _viewType == ServiceButtonsViewType.Services ? FabricationManager.CurrentService.ServiceTemplate : FabricationManager.CurrentServiceTemplate;

                if (FabricationAPIExamples.DeleteServiceTab(serviceTemplate, tab.Tab))
                {
                    int id = -1;
                    if (tab == tbServiceTab.SelectedItem)
                    {
                        CurrentServiceTab = null;
                        CurrentServiceButton = null;
                    }
                    else if (CurrentServiceTab != null)
                    {
                        id = CurrentServiceTab.Id;
                    }

                    UpdateServiceTabs(id);
                }
            }
        }

        private void editServiceTabName_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var tab = item.DataContext as FabServiceTab;
            if (tab == null)
                return;

            EditNameWindow win = new EditNameWindow(null, tab.Name, null);
            win.ShowDialog();
            if (win.Completed)
            {
                tab.Name = win.NewName;
                tab.Tab.Name = win.NewName;
                UpdateServiceTabs(tab.Tab.Id);
            }
        }

        private void deleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var button = item.DataContext as FabServiceButton;
            if (button == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Button: " + button.Button.Name, "Delete Service Button",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteServiceButton(CurrentServiceTab, button.Button))
                {
                    if (button.Button == CurrentServiceButton)
                    {
                        CurrentServiceButton = null;
                    }

                    UpdateServiceButtons();
                }
            }
        }

        private void editServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var button = item.DataContext as FabServiceButton;
            if (button == null)
                return;

            EditServiceButtonWindow win = new EditServiceButtonWindow(button.Button.Name, button.Button.ButtonCode);
            win.ShowDialog();
            if (!String.IsNullOrWhiteSpace(win.NewName))
            {
                button.Button.Name = win.NewName;
                button.Button.ButtonCode = win.NewButtonCode;
                UpdateServiceButtons();
            }
        }

        private void UpdateServiceTabs(int id)
        {
            LoadServiceTabs(id);

            Service service = FabricationManager.CurrentService;
            ServiceTemplate serviceTemplate = FabricationManager.CurrentServiceTemplate;

            int idFromService = -1, idFromTemplate = -1;
            if (service != null)
                idFromService = service.ServiceTemplate.Id;
            if (serviceTemplate != null)
                idFromTemplate = serviceTemplate.Id;

            bool sameId = (idFromService == idFromTemplate);
            if (!sameId)
                return;

            if (_viewType == ServiceButtonsViewType.Services)
            {
                // update service templates
                var view = FabricationManager.DBEditor.ButtonsTabControl_Templates.Content as ServiceButtonsView;
                if (view != null)
                {
                    view.LoadServiceTabs(id);
                }
            }
            else
            {
                // update services
                var view = FabricationManager.DBEditor.ButtonsTabControl_Services.Content as ServiceButtonsView;
                if (view != null)
                {
                    view.LoadServiceTabs(id);
                }
            }
        }

        private void UpdateServiceButtons()
        {
            LoadServiceButtons();

            Service service = FabricationManager.CurrentService;
            ServiceTemplate serviceTemplate = FabricationManager.CurrentServiceTemplate;

            int idFromService = -1, idFromTemplate = -1;
            if (service != null)
                idFromService = service.ServiceTemplate.Id;
            if (serviceTemplate != null)
                idFromTemplate = serviceTemplate.Id;

            bool sameId = idFromService == idFromTemplate;
            if (!sameId)
                return;

            if (_viewType == ServiceButtonsViewType.Services)
            {
                // update service templates
                var view = FabricationManager.DBEditor.ButtonsTabControl_Templates.Content as ServiceButtonsView;
                if (view != null)
                {
                    view.LoadServiceTabs(-1);
                }
            }
            else
            {
                // update services
                var view = FabricationManager.DBEditor.ButtonsTabControl_Services.Content as ServiceButtonsView;
                if (view != null)
                {
                    view.LoadServiceTabs(-1);
                }
            }
        }

        private void UpdateServiceButtonItems(FabServiceButton button)
        {
            if (FabricationManager.DBEditor == null)
                return;

            var conditionsCombo = FabricationManager.DBEditor.cmbSelectButtonItem;
            if (conditionsCombo == null)
                return;

            if (_viewType == ServiceButtonsViewType.Services)
            {
                // update the combo
                conditionsCombo.ItemsSource = new ObservableCollection<ServiceButtonItem>(button.Button.ServiceButtonItems);
                if (button.Button.ServiceButtonItems.Count == 0)
                    conditionsCombo.SelectedIndex = -1;
                else
                    conditionsCombo.SelectedIndex = 0;
            }
            else
            {
                conditionsCombo.ItemsSource = null;
                conditionsCombo.SelectedIndex = -1;
            }
        }

        private void addButtonItem_Click(object sender, RoutedEventArgs e)
        {
            ServiceTemplate serviceTemplate = null;
            if (_viewType == ServiceButtonsViewType.Services)
                serviceTemplate = FabricationManager.CurrentService.ServiceTemplate;
            else
                serviceTemplate = FabricationManager.CurrentServiceTemplate;

            AddServiceButtonItemWindow win = new AddServiceButtonItemWindow(serviceTemplate);
            win.ShowDialog();
        }

        private void editButtonItem_Click(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as MenuItem;
            if (item == null)
                return;

            var condition = item.DataContext as ServiceButtonItem;
            if (condition == null)
                return;

            ServiceTemplate serviceTemplate = null;
            if (_viewType == ServiceButtonsViewType.Services)
                serviceTemplate = FabricationManager.CurrentService.ServiceTemplate;
            else
                serviceTemplate = FabricationManager.CurrentServiceTemplate;

            EditServiceButtonItemWindow win = new EditServiceButtonItemWindow(serviceTemplate, condition);
            win.ShowDialog();
        }

        private void deleteButtonItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var button = item.DataContext as FabServiceButton;
            if (button == null)
                return;

            var item2 = e.OriginalSource as MenuItem;
            if (item2 == null)
                return;

            var buttonItem = item2.DataContext as ServiceButtonItem;
            if (buttonItem == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Button Item: " + buttonItem.ServiceTemplateCondition.Description, "Delete Service Button Item",
                MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteServiceButtonItem(button.Button, buttonItem))
                {
                    UpdateServiceButtonItems(button);
                }
            }
        }

        private void moveButtonUp_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var button = item.DataContext as FabServiceButton;
            if (button == null)
                return;

            var tab = this.tbServiceTab.SelectedValue as FabServiceTab;
            if (tab == null)
                return;

            FabricationAPIExamples.MoveServiceButton(tab.Tab, button.Button, -1);
            UpdateServiceButtons();
        }

        private void moveButtonDown_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null)
                return;

            var button = item.DataContext as FabServiceButton;
            if (button == null)
                return;

            var tab = this.tbServiceTab.SelectedValue as FabServiceTab;
            if (tab == null)
                return;

            FabricationAPIExamples.MoveServiceButton(tab.Tab, button.Button, 1);
            UpdateServiceButtons();
        }

    }
}

