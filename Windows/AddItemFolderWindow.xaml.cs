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
using Autodesk.Fabrication.Content;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;

namespace FabricationSample
{
    public partial class AddItemFolderWindow : Window
    {

        private ItemFolder _itmFolder;
        private TreeViewItem _trvItm;

        #region ctor

        public AddItemFolderWindow(ItemFolder selectedFolder, TreeViewItem trvItm)
        {
            _itmFolder = selectedFolder;
            _trvItm = trvItm;
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

        private void btnAddItemFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderName = txtNewItemFolderName.Text;
            ItemFolder parent = (bool)chkRootFolder.IsChecked ? null : _itmFolder;

            if (FabricationAPIExamples.AddItemFolder(folderName, parent))
            {
                var folders = new List<ItemFolder>();

                if (parent == null)
                {
                    folders = Autodesk.Fabrication.Content.ItemFolders.Folders.OrderBy(x => x.Name).ToList();
                    try
                    {
                        FabricationManager.ItemFoldersView.trvItemFolders.Items.Clear();
                    }
                    catch (Exception ex)
                    {
                        string mess = ex.Message;
                    }
                }
                else
                {
                    folders = _itmFolder.SubFolders.OrderBy(x => x.Name).ToList();
                    _trvItm.Items.Clear();
                }

                FabricationManager.ItemFoldersView.PopulateItemFolders(folders, parent == null ? null : _trvItm);
            }

            this.Close();
        }

        #endregion


    }
}
