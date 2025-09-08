using System;
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
using System.Text.RegularExpressions;

using FabricationSample.Data;
using FabricationSample.Manager;

using Autodesk.Fabrication;

namespace FabricationSample.UserControls.ItemEditor.ProductList
{

  public partial class AddProductOptionEntry : UserControl
  {

    public AddProductOptionEntry()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      txtFieldLabel.Text = "Add " + FabricationManager.CurrentOptionField.Name;
    }

    private void btnAddField_Click(object sender, RoutedEventArgs e)
    {
      if (txtAddField.Text == string.Empty)
      {
        MessageBox.Show("Enter Value", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        txtAddField.Focus();
      }
      else
      {
        double val = 0;

        if (double.TryParse(txtAddField.Text.Trim(), out val))
        {
          FabricationManager.CurrentOptionField.Template.AddOptionDefinition(new ItemProductListOptionDefinition(FabricationManager.CurrentOptionField.Option, true), val);
          FabricationManager.ItemEditor.FinshEditingProductOptionField();
        }
        else
        {
          MessageBox.Show("Incorrect Data", "Data Formatting", MessageBoxButton.OK, MessageBoxImage.Exclamation);
          txtAddField.Focus();
        }
      }
    }

    private void btnCancelAddField_Click(object sender, RoutedEventArgs e)
    {
      FabricationManager.ItemEditor.FinshEditingProductOptionField();
    }

    private void txtAddField_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      TextBox txt = sender as TextBox;
      double val = 0;
      bool handled = true;
      if (((e.Text == "." || e.Text == ",") && txt.Text.Length >= 1) && (!txtAddField.Text.Contains(".") || !txtAddField.Text.Contains(".")))
        handled = false;
      else if (double.TryParse(e.Text, out val))
        handled = false;
      e.Handled = handled;
    }
  }
}
