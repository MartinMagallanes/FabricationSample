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
  public partial class AddServiceTemplateWindow : Window
  {
    #region ctor

    public AddServiceTemplateWindow()
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

    private void btnAddServiceTemplate_Click(object sender, RoutedEventArgs e)
    {
      string serviceTemplateName = txtNewServiceTemplateName.Text;

      if (!string.IsNullOrWhiteSpace(serviceTemplateName))
      {
        FabricationManager.DBEditor.addServiceTemplate(serviceTemplateName);
        this.Close();
      }
      else
        MessageBox.Show("Template name cannot be empty", "Service Template", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    #endregion

    
  }
}
