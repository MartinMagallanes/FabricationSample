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

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;
using FabricationSample.UserControls;

namespace FabricationSample
{
  public partial class AddServiceButtonItemWindow : Window
  {
    ServiceTemplateCondition _selectedTemplateCondition;
    ServiceTemplate _selectedTemplate;

    #region ctor

    public AddServiceButtonItemWindow(ServiceTemplate serviceTemplate)
    {
      InitializeComponent();

      _selectedTemplate = serviceTemplate;
    }

    #endregion

    #region Window Methods

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ButtonState == MouseButtonState.Pressed)
      {
        base.OnMouseLeftButtonDown(e);
        DragMove();
      }
    }

    private void CloseImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      ItemsFolder_ServiceTemplates.Content = FabricationManager.ItemFoldersView;
      cmbTemplateConditions.ItemsSource = new ObservableCollection<ServiceTemplateCondition>(FabricationManager.CurrentServiceTemplate.Conditions);
    }

    private void addServiceButtonItem_Click(object sender, RoutedEventArgs e)
    {
      string path = FabricationManager.CurrentLoadedItemPath;

      var view = FabricationManager.DBEditor.ButtonsTabControl_Templates.Content as ServiceButtonsView;

      addServiceButtonItem(view.CurrentServiceButton, path, _selectedTemplateCondition);
      this.Close();
    }

    private void cmbTemplateConditions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _selectedTemplateCondition = null;

      if (e.AddedItems == null || e.AddedItems.Count == 0)
        return;

      ServiceTemplateCondition condition = e.AddedItems[0] as ServiceTemplateCondition;
      if (condition == null)
        return;

      _selectedTemplateCondition = condition;
    }

    #endregion

    private void addServiceButtonItem(ServiceButton button, string path, ServiceTemplateCondition condition)
    {
      if (FabricationManager.DBEditor == null)
        return;

      FabricationAPIExamples.AddNewServiceButtonItem(button, path, condition);

      int idFromService = -1, idFromTemplate = -1;
      if (FabricationManager.CurrentService != null)
        idFromService = FabricationManager.CurrentService.ServiceTemplate.Id;
      if (FabricationManager.CurrentServiceTemplate != null)
        idFromTemplate = FabricationManager.CurrentServiceTemplate.Id;

      ServiceButtonsView view = null;
      if (_selectedTemplate.Id == idFromService)
      {
        // services window needs updating
        if (FabricationManager.DBEditor.ButtonsTabControl_Services != null)
        {
          view = FabricationManager.DBEditor.ButtonsTabControl_Services.Content as ServiceButtonsView;
          if (view != null)
            view.LoadServiceTabs(-1);
        }
      }

      if (_selectedTemplate.Id == idFromTemplate)
      {
        // templates window needs updating
        if (FabricationManager.DBEditor.ButtonsTabControl_Templates != null)
        {
          view = FabricationManager.DBEditor.ButtonsTabControl_Templates.Content as ServiceButtonsView;
          if (view != null)
            view.LoadServiceButtons();
        }
      }
    }
    
  }
}
