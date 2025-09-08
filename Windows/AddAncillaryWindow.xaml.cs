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
using FabricationSample.UserControls.Ancillaries;

namespace FabricationSample
{
  public partial class AddAncillaryWindow : Window
  {
    private AncillaryTypeEnum m_ancillaryType;

    #region ctor

    public AddAncillaryWindow(AncillaryTypeEnum ancillaryType)
    {
      InitializeComponent();

      m_ancillaryType = ancillaryType;
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
      var ancillary = FabricationAPIExamples.AddNewAncillary(Guid.NewGuid().ToString(), "Group", m_ancillaryType);
      if (ancillary == null)
        return;

      ccAncillaryDetails.Content = new AncillaryDetailsView(this, ancillary);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      if (FabricationManager.AncillariesView == null)
        return;

      var ancillary = (ccAncillaryDetails.Content as AncillaryDetailsView).Ancillary;
      if (ancillary == null)
        return;

      FabricationManager.AncillariesView.AddNewAncillary(ancillary);
    }

    #endregion


  }
}
