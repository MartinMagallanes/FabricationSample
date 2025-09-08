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

using FabricationSample.Data;
using FabricationSample.Manager;

namespace FabricationSample.UserControls.ItemEditor.ProductList
{
  /// <summary>
  /// Interaction logic for AddProductDataEntry.xaml
  /// </summary>
  public partial class AddProductDataEntry : UserControl
  {
    private bool _requiresNumericOnlyInput;

    public AddProductDataEntry()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      switch (FabricationManager.CurrentDataField.Field)
      {
        case ProductListDataFieldType.Alias:
          txtFieldLabel.Text = "Add Alias";
          break;
        case ProductListDataFieldType.Area:
          txtFieldLabel.Text = "Add Area";
          _requiresNumericOnlyInput = true;
          break;
        case ProductListDataFieldType.CADBlockName:
          txtFieldLabel.Text = "Add CAD Block Name";
          break;
        case ProductListDataFieldType.OrderNumber:
          txtFieldLabel.Text = "Add Order Number";
          break;
        case ProductListDataFieldType.Weight:
          txtFieldLabel.Text = "Add Weight";
          _requiresNumericOnlyInput = true;
          break;
        case ProductListDataFieldType.DatabaseId:
          txtFieldLabel.Text = "Add DatabaseId";
          break;
        case ProductListDataFieldType.BoughtOut:
          txtFieldLabel.Text = "Add Bought Out";
          break;
        case ProductListDataFieldType.Flow:
          txtFieldLabel.Text = "Add Flow";
          _requiresNumericOnlyInput = true;
          break;
        default:
          break;
      }
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
        double numericVal = 0;

        if (_requiresNumericOnlyInput)
        {
          if (!double.TryParse(txtAddField.Text.Trim(), out numericVal))
          {
            MessageBox.Show("Enter Value", "Missing Numeric Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            txtAddField.Focus();
            return;
          }
        }

        switch (FabricationManager.CurrentDataField.Field)
        {
          case ProductListDataFieldType.Alias:
            FabricationManager.CurrentDataField.Template.SetAlias(txtAddField.Text);
            break;
          case ProductListDataFieldType.Area:
            FabricationManager.CurrentDataField.Template.SetArea(numericVal);
            break;
          case ProductListDataFieldType.CADBlockName:
            FabricationManager.CurrentDataField.Template.SetCadBlockName(txtAddField.Text);
            break;
          case ProductListDataFieldType.OrderNumber:
            FabricationManager.CurrentDataField.Template.SetOrderNumber(txtAddField.Text);
            break;
          case ProductListDataFieldType.Weight:
            FabricationManager.CurrentDataField.Template.SetWeight(numericVal);
            break;
          case ProductListDataFieldType.DatabaseId:
            FabricationManager.CurrentDataField.Template.SetDatabaseId(txtAddField.Text);
            break;
          case ProductListDataFieldType.BoughtOut:
            break;
          case ProductListDataFieldType.Flow:
            FabricationManager.CurrentDataField.Template.SetFlow(numericVal, numericVal);
            break;
          default:
            break;
        }

        FabricationManager.ItemEditor.FinshEditingProductDataField();
      }
    }

    private void btnCancelAddField_Click(object sender, RoutedEventArgs e)
    {
      FabricationManager.ItemEditor.FinshEditingProductDataField();
    }

    private void txtAddField_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      if (_requiresNumericOnlyInput)
      {
        TextBox txt = sender as TextBox;
        double val = 0;
        bool handled = true;
        if (((e.Text == "." || e.Text == ",") && txt.Text.Length >= 1) && (!txt.Text.Contains(".") || !txt.Text.Contains(".")))
          handled = false;
        else if (double.TryParse(e.Text, out val))
          handled = false;
        e.Handled = handled;
      }
      else
        e.Handled = false;
    }
  }
}
