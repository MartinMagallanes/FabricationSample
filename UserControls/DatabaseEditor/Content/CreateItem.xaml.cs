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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.Geometry;
using Autodesk.Fabrication.Content;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;
using FabricationSample.UserControls.DatabaseEditor;

namespace FabricationSample.UserControls
{
  /// <summary>
  /// Interaction logic for CreateItem.xaml
  /// </summary>
  public partial class CreateItem : UserControl
  {

    public CreateItem()
    {
      InitializeComponent();
    }

    private void txtCID_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      Regex regex = new Regex("[^0-9]+");
      e.Handled = regex.IsMatch(e.Text);
    }

    private void btnCreateItem_Click(object sender, RoutedEventArgs e)
    {
      int cidNo;
      string itemName = txtItemName.Text;
      string returnMessage = string.Empty;
      MessageBoxImage image = MessageBoxImage.Exclamation;

      if (txtCID.Text != string.Empty)
      {
        if (txtItemName.Text != string.Empty)
        {
          if (int.TryParse(txtCID.Text, out cidNo))
          {
            if (FabricationManager.ItemFoldersView.trvItemFolders.SelectedItem != null)
            {
              TreeViewItem trvItem = (TreeViewItem)FabricationManager.ItemFoldersView.trvItemFolders.SelectedItem;
              ItemFolder folder = trvItem.Tag as ItemFolder;
              Item item = FabricationAPIExamples.CreateItem(cidNo, (bool)chkSetCertified.IsChecked, (bool)chkSetFixRelative.IsChecked, (bool)chkSetCatalogue.IsChecked, (bool)chkSetBoughtOut.IsChecked);

              if (item != null)
              {
               
                if ((bool)chkSetImageFile.IsChecked)
                  FabricationAPIExamples.SetItemImage(item);

                ItemOperationResult result = ContentManager.SaveItemAs(item, folder.Directory, itemName, (bool)chkItemOverWriteExisting.IsChecked);

                if (result.Status == ResultStatus.Succeeded)
                {
                  image = MessageBoxImage.Information;
                  if (!(bool)chkItemOverWriteExisting.IsChecked)
                    FabricationManager.ItemFoldersView.AddNewTreeViewItem(trvItem, ItemFoldersView.TreeViewNodeType.file, System.IO.Path.Combine(folder.Directory, itemName + ".itm"));
                }

                returnMessage = result.Message;
              }
              else
                returnMessage = "Item could not be created, possible invalid CID";
            }

            else
              returnMessage = "No folder selected";
          }
          else
            returnMessage = "Unable to Parse CID Number";
        }
        else
          returnMessage = "Requires Item Name";
      }
      else
        returnMessage = "Requires CID Number";

      MessageBox.Show(returnMessage, "Create Item", MessageBoxButton.OK, image);

    }

  }
}

