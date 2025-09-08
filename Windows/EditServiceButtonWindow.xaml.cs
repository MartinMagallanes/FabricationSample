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
  public partial class EditServiceButtonWindow : Window
  {
    private string _newName;
    private string _newButtonCode;
    public string NewName { get { return _newName; } }
    public string NewButtonCode { get { return _newButtonCode; } }

    #region ctor

    public EditServiceButtonWindow(string name, string buttonCode)
    {
      InitializeComponent();

      txtNewName.Text = name;
      txtNewButtonCode.Text = buttonCode;
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
      string newName = txtNewName.Text;
      string newButtonCode = txtNewButtonCode.Text;
      int errors = 0;
      if (!string.IsNullOrWhiteSpace(newName))
        _newName = newName;
      else
      {
        errors++;
        MessageBox.Show("Name cannot be empty", "Edit Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }

      _newButtonCode = newButtonCode;

      if (errors == 0)
        this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    #endregion


  }
}
