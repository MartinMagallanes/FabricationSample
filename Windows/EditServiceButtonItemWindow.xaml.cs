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
  public partial class EditServiceButtonItemWindow : Window
  {
    #region Private Members
    ServiceTemplate _selectedServiceTemplate;
    ServiceButtonItem _buttonItem;
    ServiceTemplateCondition _selectedTemplateCondition;
    #endregion


    #region ctor

    public EditServiceButtonItemWindow(ServiceTemplate serviceTemplate, ServiceButtonItem buttonItem)
    {
      InitializeComponent();

      _buttonItem = buttonItem;
      _selectedServiceTemplate = serviceTemplate;
    }

    #endregion

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
      FabricationManager.EditServiceButtonItemWindow = this;
      ItemsFolder_ServiceTemplates.Content = FabricationManager.ItemFoldersView;

      var conditions = _selectedServiceTemplate.Conditions.ToList();
      cmbTemplateConditions.ItemsSource = new ObservableCollection<ServiceTemplateCondition>(conditions);

      ServiceTemplateCondition selectedCondition = conditions.FirstOrDefault(x => x.Id == _buttonItem.ServiceTemplateCondition.Id);
      cmbTemplateConditions.SelectedItem = selectedCondition;

      string itemPath = _buttonItem.ItemPath;
      if (!String.IsNullOrWhiteSpace(itemPath))
      {
        string itemContentPath = Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath;
        bool contains = itemPath.Contains(itemContentPath);
        if (contains)
          itemPath = itemPath.Replace(itemContentPath, "");
      }

      txtSelectedItemPath.Text = itemPath;

      SetRangeControls();

      FabricationManager.ItemFoldersView.SelectItemPath(_buttonItem.ItemPath);
    }

    private void SetRangeControls()
    {
      if (_buttonItem.LessThanEqualTo == -1)
      {
        txtLessThan.IsEnabled = false;
        txtLessThan.Text = "";
        chkLessThanUnrestricted.IsChecked = true;
      }
      else
      {
        txtLessThan.IsEnabled = true;
        txtLessThan.Text = _buttonItem.LessThanEqualTo.ToString();
        chkLessThanUnrestricted.IsChecked = false;
      }

      if (_buttonItem.GreaterThan == -1)
      {
        txtGreaterThan.IsEnabled = false;
        txtGreaterThan.Text = "";
        chkGreaterThanUnrestricted.IsChecked = true;
      }
      else
      {
        txtGreaterThan.IsEnabled = true;
        txtGreaterThan.Text = _buttonItem.GreaterThan.ToString();
        chkGreaterThanUnrestricted.IsChecked = false;
      }
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

      // update the greater than / less than values
    }


    private void chkLessThanUnrestricted_Checked(object sender, RoutedEventArgs e)
    {
      txtLessThan.IsEnabled = false;
    }

    private void chkGreaterThanUnrestricted_Checked(object sender, RoutedEventArgs e)
    {
      txtGreaterThan.IsEnabled = false;
    }

    private void chkLessThanUnrestricted_Unchecked(object sender, RoutedEventArgs e)
    {
      txtLessThan.IsEnabled = true;
    }

    private void chkGreaterThanUnrestricted_Unchecked(object sender, RoutedEventArgs e)
    {
      txtGreaterThan.IsEnabled = true;
    }

    private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsDigit(e.Text, e.Text.Length - 1))
      {
        e.Handled = true;
      }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
      double currentLessThan = _buttonItem.LessThanEqualTo;
      double currentGreaterThan = _buttonItem.GreaterThan;

      double newLessThan = 0;
      double newGreaterThan = 0;

      if (chkLessThanUnrestricted.IsChecked == true)
        newLessThan = -1;
      else
      {
        if (!String.IsNullOrWhiteSpace(txtLessThan.Text))
          newLessThan = Convert.ToDouble(txtLessThan.Text);
        else
          newLessThan = currentLessThan;
      }

      if (chkGreaterThanUnrestricted.IsChecked == true)
        newGreaterThan = -1;
      else
      {
        if (!String.IsNullOrWhiteSpace(txtGreaterThan.Text))
          newGreaterThan = Convert.ToDouble(txtGreaterThan.Text);
        else
          newGreaterThan = currentGreaterThan;
      }

      bool ok = true;
      if (currentLessThan != newLessThan || currentGreaterThan != newGreaterThan)
      {
        ok = FabricationAPIExamples.SetConditionOverride(_buttonItem, newGreaterThan, newLessThan);
      }

      if (!ok)
        return;
      
      string fullPath = Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath + txtSelectedItemPath.Text;
      if (!fullPath.Equals(_buttonItem.ItemPath))
      {
        _buttonItem.ItemPath = fullPath;
               
        if (FabricationManager.CurrentService.ServiceTemplate != null && FabricationManager.CurrentService.ServiceTemplate.Id == _selectedServiceTemplate.Id)        
        {
          var servicesView = FabricationManager.DBEditor.ButtonsTabControl_Services as ServiceButtonsView;
          servicesView.LoadServiceButtons();
        }
        if (FabricationManager.CurrentServiceTemplate != null && FabricationManager.CurrentServiceTemplate.Id == _selectedServiceTemplate.Id)        
        {
          var templatesView = FabricationManager.DBEditor.ButtonsTabControl_Templates as ServiceButtonsView;
          templatesView.LoadServiceButtons();
        }
      }

      int newConditionId = -1;
      ServiceTemplateCondition condition = cmbTemplateConditions.SelectedItem as ServiceTemplateCondition;
      if (condition != null)
        newConditionId = condition.Id;

      if (newConditionId != -1 && newConditionId != _buttonItem.ServiceTemplateCondition.Id)
        _buttonItem.ServiceTemplateCondition = condition;

      this.Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      FabricationManager.EditServiceButtonItemWindow = null;
    }
  }
}
