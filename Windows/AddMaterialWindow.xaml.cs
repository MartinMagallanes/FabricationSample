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
  public partial class AddMaterialWindow : Window
  {
    #region ctor

    public AddMaterialWindow()
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
      cmbMaterialTypes.ItemsSource = MaterialTypes();
    }

    private void btnAddMaterial_Click(object sender, RoutedEventArgs e)
    {
      string name = txtNewMaterialName.Text;
      string group = txtNewMaterialGroup.Text;

      if (string.IsNullOrWhiteSpace(name))
      {
        MessageBox.Show("Material name cannot be empty", "Material", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return;
      }
      else if (cmbMaterialTypes.SelectedItem == null)
      {
        MessageBox.Show("A material type needs to be selected", "Material", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return;
      }

      KeyValuePair<MaterialType, string> item = (KeyValuePair<MaterialType, string>)cmbMaterialTypes.SelectedItem; 

      FabricationManager.DBEditor.addMaterial(name, group, item.Key);
      this.Close();
    }

    #endregion

    private ObservableCollection<KeyValuePair<MaterialType, string>> MaterialTypes()
    {
      var values = Enum.GetValues(typeof(MaterialType)).Cast<MaterialType>().ToList();
      var dictionary = new Dictionary<MaterialType, string>();

      foreach (MaterialType matType in values)
      {
        string description = matType.ToString();
        dictionary.Add(matType, description);
      }

      var ordered = dictionary.OrderBy(x => x.Value);
      return new ObservableCollection<KeyValuePair<MaterialType, string>>(ordered);
    }

  }
}
