using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FabricationSample.Manager;
using Path = System.IO.Path;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Autodesk.Fabrication.Content;

using FabricationSample.FunctionExamples;

namespace FabricationSample.UserControls
{
    /// <summary>
    /// Interaction logic for ItemListView.xaml
    /// </summary>
    public partial class ItemFoldersView : UserControl
    {
        public enum TreeViewNodeType
        {
            folder, file
        }

        public ItemFoldersView()
        {
            InitializeComponent();
            var folders =  new List<ItemFolder>(Autodesk.Fabrication.Content.ItemFolders.Folders.OrderBy(x => x.Name));
            PopulateItemFolders(folders, null);
        }

        private void trvItemFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FabricationManager.DBEditor == null)
                return;

            TreeViewItem trvItm = trvItemFolders.SelectedItem as TreeViewItem;

            if (trvItm.Tag.GetType() == typeof(ItemFolder))
                trvItm.ContextMenu = trvItemFolders.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
            else
                trvItm.ContextMenu = null;

            FabricationManager.DBEditor.ItemFolders_SelectedItemChange(trvItm);
        }

        public void PopulateItemFolders(List<ItemFolder> folders, TreeViewItem parentItem)
        {
            foreach (ItemFolder folder in folders)
            {
                TreeViewItem item = GetTreeViewItem(TreeViewNodeType.folder, folder.Name, folder);

                if (parentItem == null)
                    trvItemFolders.Items.Add(item);
                else
                    parentItem.Items.Add(item);

                //Add dummy sub item
                item.Items.Add("Loading...");
            }
        }

        private void PopulateItems(TreeViewItem item)
        {
            ItemFolder folder = item.Tag as ItemFolder;

            if (folder != null && Directory.Exists(folder.Directory))
            {
                foreach (string f in Directory.GetFiles(folder.Directory, "*.itm", SearchOption.TopDirectoryOnly))
                {
                    TreeViewItem subItem = GetTreeViewItem(TreeViewNodeType.file, Path.GetFileNameWithoutExtension(f), f);
                    item.Items.Add(subItem);
                }
            }

        }

        public void AddNewTreeViewItem(TreeViewItem parentItem, TreeViewNodeType nodeType, string path)
        {
            if (nodeType == TreeViewNodeType.file)
            {
                TreeViewItem subItem = GetTreeViewItem(TreeViewNodeType.file, Path.GetFileNameWithoutExtension(path), path);
                parentItem.Items.Add(subItem);
            }
            else if (nodeType == TreeViewNodeType.folder)
            {
                //TODO: Handle adding new folders
            }
        }

        private TreeViewItem GetTreeViewItem(TreeViewNodeType node, string text, object tag)
        {
            TreeViewItem item = new TreeViewItem();

            string imagePath = String.Empty;
            string contextMenu = string.Empty;

            switch (node)
            {
                case TreeViewNodeType.folder:
                    imagePath = "pack://application:,,,/FabricationSample;component/Resources/Folder-32.png";
                    break;
                case TreeViewNodeType.file:
                    imagePath = "pack://application:,,,/FabricationSample;component/Resources/part.png";
                    //contextMenu = "ItemContext";
                    break;
                default:
                    break;
            }

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            // create Image
            Image image = new Image();
            image.Width = 20;
            image.Height = 20;
            image.Stretch = Stretch.Fill;
            image.Source = new BitmapImage(new Uri(imagePath));

            // Label
            Label lbl = new Label();
            lbl.Content = text;


            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            item.Tag = tag;
            item.FontWeight = FontWeights.Normal;

            return item;
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;

            if ((item.HasItems) && item.Items[0] is string)
            {
                item.Items.Clear();
                ItemFolder folder = item.Tag as ItemFolder;
                if (folder != null)
                {
                    PopulateItemFolders(folder.SubFolders.ToList(), item);
                    PopulateItems(item);
                }

            }
        }

        private void mnuAddFolder_Click(object sender, RoutedEventArgs e)
        {
            if (trvItemFolders.SelectedItem != null)
            {
                TreeViewItem trvItm = trvItemFolders.SelectedItem as TreeViewItem;
                if ((trvItm != null) && trvItm.Tag.GetType() == typeof(ItemFolder))
                {
                    ItemFolder folder = trvItm.Tag as ItemFolder;
                    AddItemFolderWindow win = new AddItemFolderWindow(folder, trvItm);
                    win.ShowDialog();
                }
            }
        }

        private void mnuDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (trvItemFolders.SelectedItem != null)
            {
                TreeViewItem trvItm = trvItemFolders.SelectedItem as TreeViewItem;
                if ((trvItm != null) && trvItm.Tag.GetType() == typeof(ItemFolder))
                {
                    ItemFolder folder = trvItm.Tag as ItemFolder;
                    if (FabricationAPIExamples.RemoveItemFolder(folder))
                    {
                        ItemsControl parent = GetSelectedTreeViewItemParent(trvItm);

                        TreeViewItem parentItem = parent as TreeViewItem;
                        try
                        {
                            if (parentItem != null)
                                parentItem.Items.Remove(trvItm);
                            else
                                trvItemFolders.Items.Remove(trvItm);
                        }
                        catch (Exception)
                        {

                        }


                    }

                }
            }
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        private void trvItemFolders_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TreeViewItem trvItm = trvItemFolders.SelectedItem as TreeViewItem;

            if (trvItm != null)
            {
                if (trvItm.Tag.GetType() != typeof(ItemFolder))
                    e.Handled = true;
            }
        }

        public void SelectItemPath(string path)
        {
            foreach (TreeViewItem item in trvItemFolders.Items)
            {
                item.IsExpanded = false;

                string tagPath;
                if (item.Tag.GetType() == typeof(ItemFolder))
                {
                    var itemFolder = item.Tag as ItemFolder;
                    tagPath = itemFolder.Directory;
                }
                else
                    tagPath = item.Tag as string;

                if (path.Equals(tagPath))
                {
                    item.IsSelected = true;
                    item.BringIntoView();
                    return;
                }
                else if (path.Contains(tagPath))
                {
                    item.IsExpanded = true;
                    bool selected = SelectItem(item, path);
                    if (selected)
                        return;
                }
            }
        }

        private bool SelectItem(TreeViewItem treeViewItem, string path)
        {
            foreach (TreeViewItem item in treeViewItem.Items)
            {
                item.IsExpanded = false;

                string tagPath;
                if (item.Tag.GetType() == typeof(ItemFolder))
                {
                    var itemFolder = item.Tag as ItemFolder;
                    tagPath = itemFolder.Directory;
                }
                else
                    tagPath = item.Tag as string;

                if (path.Equals(tagPath))
                {
                    item.IsSelected = true;
                    item.BringIntoView();
                    return true;
                }
                else if (path.Contains(tagPath))
                {
                    item.IsExpanded = true;
                    bool selected = SelectItem(item, path);
                    if (selected)
                        return true;
                }
            }

            return false;
        }
    }
}
