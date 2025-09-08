using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using FabricationSample.FunctionExamples;
using FabricationSample.Manager;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FabricationSample.UserControls.DatabaseEditor
{
    /// <summary>
    /// Interaction logic for DatabaseEditor.xaml
    /// </summary>
    public partial class DatabaseEditor : UserControl
    {
        #region Private Members


        #endregion

        #region Materials

        private void tbiMaterials_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMaterials(null, null);
        }

        private void LoadMaterials(string name, string group)
        {
            // setup materials
            ListCollectionView materials = new ListCollectionView(new ObservableCollection<Material>(Database.Materials));
            materials.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            materials.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
            materials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            cmbSelectMaterial.ItemsSource = materials;

            if (!String.IsNullOrWhiteSpace(name))
            {
                foreach (Material m in materials)
                {
                    if (m.Name.Equals(name))
                    {
                        if (String.IsNullOrWhiteSpace(m.Group) && String.IsNullOrWhiteSpace(group))
                        {
                            cmbSelectMaterial.SelectedItem = m;
                            break;
                        }
                        else if (!String.IsNullOrWhiteSpace(m.Group) && m.Group.Equals(group))
                        {
                            cmbSelectMaterial.SelectedItem = m;
                            break;
                        }
                    }
                }
            }
        }

        private void cmbSelectMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems != null) && e.AddedItems.Count > 0)
            {
                Material material = e.AddedItems[0] as Material;
                FabricationManager.CurrentMaterial = material;
                if (material != null)
                {
                    bool enable = material.CanChange;
                    btnAddGaugeSize.IsEnabled = enable;
                    btnAddGauge.IsEnabled = enable;

                    LoadGauges();
                }
            }
        }

        private void LoadGauges()
        {
            Material material = FabricationManager.CurrentMaterial;
            if (material != null && material.Gauges.Count > 0)
            {
                if (material.Gauges.Any(g => g is MachineGauge))
                {
                    GaugesView.Content = new ThicknessGaugesView();
                }
                else
                {
                    GaugesView.Content = new SpecGaugesView();
                }

                GaugesSizesView.Content = null;
                FabricationManager.CurrentGauge = null;
            }
        }
        private void editMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentMaterial == null)
                return;

            EditMaterialWindow win = new EditMaterialWindow(FabricationManager.CurrentMaterial);
            win.ShowDialog();

            if (!String.IsNullOrWhiteSpace(win.NewName))
            {
                FabricationManager.CurrentMaterial.Name = win.NewName;
                FabricationManager.CurrentMaterial.Group = win.NewGroup;
                //FabricationManager.CurrentMaterial.Usage = win.NewMaterialUsage; //TODO: consider making a cloner to change usage
                LoadMaterials(win.NewName, win.NewGroup);
            }
        }

        private void addMaterial_Click(object sender, RoutedEventArgs e)
        {
            AddMaterialWindow win = new AddMaterialWindow();
            win.ShowDialog();
        }

        private void deleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentMaterial != null)
            {
                if (MessageBox.Show("Confirm to Delete Material", "Delete Material",
                  MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (FabricationAPIExamples.DeleteMaterial(FabricationManager.CurrentMaterial))
                    {
                        FabricationManager.CurrentMaterial = null;
                        LoadMaterials(null, null);
                    }
                }
            }
        }

        public void addMaterial(string name, string group, MaterialUsage materialUsage)
        {
            Material newMaterial = FabricationAPIExamples.AddNewMaterial(name, group, materialUsage);
            if (newMaterial != null)
            {
                FabricationManager.CurrentMaterial = newMaterial;
                LoadMaterials(name, group);
            }
        }

        private void btnAddGauge_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentMaterial == null)
                return;

            Gauge gauge = FabricationAPIExamples.AddNewGauge(FabricationManager.CurrentMaterial, FabricationManager.CurrentGauge.Type);
            if (gauge == null)
                return;

            LoadGauges();
        }

        private void btnAddGaugeSize_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationManager.CurrentGauge == null)
                return;

            Material material = FabricationManager.CurrentMaterial;
            if (material == null)
                return;

            switch (FabricationManager.CurrentGauge.Type)
            {
                default:
                    break;

                case MaterialType.Ductwork:
                    var view = GaugesSizesView.Content as RoundGaugeSizeView;
                    int index = view.GetTabIndex();

                    switch (index)
                    {
                        default:
                            break;

                        case 0: // flat bed
                            FabricationAPIExamples.AddFlatBedGaugeSize(FabricationManager.CurrentGauge as MachineGauge, 0, 0);
                            break;

                        case 1: // rotary
                            FabricationAPIExamples.AddRotaryGaugeSize(FabricationManager.CurrentGauge as MachineGauge, 0, 0);
                            break;

                        case 2: // shear
                            FabricationAPIExamples.AddShearGaugeSize(FabricationManager.CurrentGauge as MachineGauge, 0, 0);
                            break;
                    }

                    GaugesSizesView.Content = new RoundGaugeSizeView(FabricationManager.CurrentGauge as MachineGauge);

                    break;

                case MaterialType.ElectricalContainment:
                    FabricationAPIExamples.AddElectricalContainmentGaugeSize(FabricationManager.CurrentGauge as ElectricalContainmentGauge, 0, 0, 0);
                    GaugesSizesView.Content = new ElectricalContainmentGaugeSizeView(FabricationManager.CurrentGauge as ElectricalContainmentGauge);
                    break;

                case MaterialType.Pipework:
                    FabricationAPIExamples.AddPipeworkGaugeSize(FabricationManager.CurrentGauge as PipeworkGauge, 0, 0);
                    GaugesSizesView.Content = new PipeworkGaugeSizeView(FabricationManager.CurrentGauge as PipeworkGauge);
                    break;
            }
        }

        private void btnSaveMaterials_Click(object sender, RoutedEventArgs e)
        {
            DBOperationResult result = Database.SaveMaterials();
            if (result.Status == ResultStatus.Succeeded)
            {
                MessageBox.Show(result.Message, "Save Materials",
                   MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(result.Message, "Save Materials",
                   MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion


    }
}


