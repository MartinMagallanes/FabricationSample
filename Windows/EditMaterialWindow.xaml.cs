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
  public partial class EditMaterialWindow : Window
  {
    private Material _material;
    private string _newName;
    private string _newGroup;
    private MaterialUsage _newType;
    public string NewName { get { return _newName; } }
    public string NewGroup { get { return _newGroup; } }
    public MaterialUsage NewMaterialUsage { get { return _newType; } }

    #region ctor

    public EditMaterialWindow(Material material)
    {
      InitializeComponent();

      _material = material;

      txtNewName.Text = material.Name;
      txtNewGroup.Text = material.Group;

      if (!material.CanChange)
      {
        txtNewName.IsReadOnly = true;
        txtNewGroup.IsReadOnly = true;
      }

      var usages = GetMaterialUsages();

      cmbMaterialUsage.ItemsSource = usages;

      foreach (MaterialUsage mu in usages)
      {
        if (mu == material.Usage)
        {
          cmbMaterialUsage.SelectedItem = mu;
          break;
        }
      }
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

      _newType = (MaterialUsage)cmbMaterialUsage.SelectedItem;

      if (errors == 0)
        this.Close();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    #endregion

    private ObservableCollection<MaterialUsage> GetMaterialUsages()
    {
      return new ObservableCollection<MaterialUsage>(Enum.GetValues(typeof(MaterialUsage)).Cast<MaterialUsage>());
    }
  }
}
