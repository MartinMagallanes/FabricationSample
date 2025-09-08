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
  public partial class EditNameWindow : Window
  {
    private string _newName;
    private string _newGroup;
    private bool _completed;
    public string NewName { get { return _newName; } }
    public string NewGroup { get { return _newGroup; } }
    public bool Completed { get { return _completed; } }

    #region ctor

    public EditNameWindow(string title, string name, string group)
    {
      InitializeComponent();

      if (!String.IsNullOrWhiteSpace(title))
        WindowTitle.Content = title;

      txtNewName.Text = name;
      txtNewGroup.Text = group;

      int minusHeight = 0;

      if (group == null)
      {
        stpGroup.Visibility = Visibility.Collapsed;
        minusHeight += 45;
      }

      Height -= minusHeight;
      border_1.Height -= minusHeight;

      _completed = false;
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
      string newGroup = txtNewGroup.Text;
      int errors = 0;
      if (!string.IsNullOrWhiteSpace(newName))
        _newName = newName;
      else
      {
        errors++;
        MessageBox.Show("Name cannot be empty", "Edit Name", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }

      _newGroup = newGroup;

      if (errors == 0)
      {
        _completed = true;
        this.Close();
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    #endregion


  }
}
