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

namespace FabricationSample
{
  public partial class AddServiceTemplateConditionWindow : Window
  {
    #region ctor

    public AddServiceTemplateConditionWindow()
    {
      InitializeComponent();
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
    }

    private void textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (!char.IsDigit(e.Text, e.Text.Length - 1))
      {
        e.Handled = true;
      }
    }

    private void btnAddServiceTemplateCondition_Click(object sender, RoutedEventArgs e)
    {
      string description = txtDescription.Text;
      string lower = (bool)chkGreaterThanUnrestricted.IsChecked ? "-1" : txtGreaterThan.Text;
      string upper = (bool)chkLessThanUnrestricted.IsChecked ? "-1" : txtLessThan.Text;

      if (!string.IsNullOrWhiteSpace(description) &&
        !string.IsNullOrWhiteSpace(lower) &&
        !string.IsNullOrWhiteSpace(upper))
      {
        FabricationManager.DBEditor.addServiceTemplateCondition(description, lower, upper);
        this.Close();
      }
      else
        MessageBox.Show("Missing information, all fields are required.", "Service Template Condition", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    #endregion

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


  }
}
