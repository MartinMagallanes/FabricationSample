using System.Windows;
using System.Windows.Controls;

using Autodesk.Fabrication;
using Autodesk.Fabrication.Content;

using FabricationSample.FunctionExamples;


using FabricationSample.Manager;

namespace FabricationSample.UserControls
{
  /// <summary>
  /// Interaction logic for LoadItem.xaml
  /// </summary>
  public partial class LoadItem : UserControl
  {

    public LoadItem()
    {
      InitializeComponent();
    }

    private void btnLoadItem_Click(object sender, RoutedEventArgs e)
    {
      if (LoadItemFromTreeViewSelection())
          FabricationManager.ParentWindow.LoadItemEditorControl();
    }

    private bool LoadItemFromTreeViewSelection()
    {
      bool loaded = false;

      TreeViewItem trvItm = FabricationManager.ItemFoldersView.trvItemFolders.SelectedItem as TreeViewItem;

      if (trvItm != null)
      {
        Item itm = ContentManager.LoadItem(trvItm.Tag.ToString());
        if (itm != null)
        {
          FabricationManager.CurrentItem = itm;
          FabricationManager.CurrentLoadedItemPath = trvItm.Tag.ToString();
          loaded = true;
        }
      }

      return loaded;
    }

    private void btnAddItemToJob_Click(object sender, RoutedEventArgs e)
    {
      if (LoadItemFromTreeViewSelection())
        FabricationAPIExamples.AddItemToJob(FabricationManager.CurrentItem);
    }
  }
}

