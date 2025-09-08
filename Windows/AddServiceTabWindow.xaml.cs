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
  public partial class AddServiceTabWindow : Window
  {
    #region ctor

    public AddServiceTabWindow()
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

    private void btnAddServiceTab_Click(object sender, RoutedEventArgs e)
    {
      string serviceTab = txtNewServiceTabName.Text;

      if (!string.IsNullOrWhiteSpace(serviceTab))
      {
        FabricationManager.DBEditor.addServiceTab(serviceTab);
        this.Close();
      }
      else
        MessageBox.Show("Tab name cannot be empty", "Service Tab", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    #endregion

    
  }
}
