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

  public partial class AddProductDataFlowEntry : UserControl
  {


    public AddProductDataFlowEntry()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void btnAddField_Click(object sender, RoutedEventArgs e)
    {
      if (txtAddFlowMin.Text == string.Empty)
      {
        MessageBox.Show("Enter Value", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        txtAddFlowMin.Focus();
      }
      else if (txtAddFlowMax.Text == string.Empty)
      {
        MessageBox.Show("Enter Value", "Missing Data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        txtAddFlowMax.Focus();
      }
      else
      {
        double flowMin = 0;
        double flowMax = 0;

        if (double.TryParse(txtAddFlowMin.Text.Trim(), out flowMin) && double.TryParse(txtAddFlowMax.Text.Trim(), out flowMax))
        {
          FabricationManager.CurrentDataField.Template.SetFlow(flowMin, flowMax);
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
      TextBox txt = sender as TextBox;
      double val = 0;
      bool handled = true;
      if (((e.Text == "." || e.Text == ",") && txt.Text.Length >= 1) && (!txt.Text.Contains(".") || !txt.Text.Contains(".")))
        handled = false;
      else if (double.TryParse(e.Text, out val))
        handled = false;
      e.Handled = handled;

    }
  }
}
