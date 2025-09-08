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
using System.Text.RegularExpressions;

namespace FabricationSample
{
  public partial class EditValueWindow : Window
  {
    private double _value;
    
    public double Value { get { return _value; } }

    #region ctor

    public EditValueWindow(double value)
    {
      InitializeComponent();
      _value = value;
      txtValue.Text = value.ToString();
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

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      _value = Convert.ToDouble(txtValue.Text);
      
      this.Close();
    }

    private void txtValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = !IsTextAllowed(e.Text);
    }

    private static bool IsTextAllowed(string text)
    {
      Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
      return !regex.IsMatch(text);
    }

    #endregion


  }
}
