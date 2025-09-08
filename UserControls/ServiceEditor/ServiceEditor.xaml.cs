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
using Autodesk.Fabrication;
using Autodesk.Fabrication.Results;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Units;
using Autodesk.Fabrication.LineWeights;

using FabricationSample.FunctionExamples;
using FabricationSample.Data;

using FabricationSample.Manager;

namespace FabricationSample.UserControls.ServiceEditor
{
    /// <summary>
    /// Interaction logic for ServiceEditor.xaml
    /// </summary>
    public partial class ServiceEditor : UserControl
    {
        #region Private Members

        ObservableCollection<ServiceType> _lstServiceTypes;

        #endregion

        #region ctor

        public ServiceEditor()
        {
            InitializeComponent();
        }

        #endregion


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtServiceGroup.Text += FabricationManager.CurrentService.Group;
            txtServiceName.Text += FabricationManager.CurrentService.Name;
        }


        private void dgServiceEntries_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServiceEntries();
        }

        private void LoadServiceTypes()
        {
            _lstServiceTypes = new ObservableCollection<ServiceType>(Database.ServiceTypes.OrderBy(x => x.Description).ToList());
        }

        private void LoadServiceEntries()
        {
            if (FabricationManager.CurrentService == null)
                return;

            if (_lstServiceTypes == null)
                LoadServiceTypes();

            ObservableCollection<ServiceEntryMapper> entries = new ObservableCollection<ServiceEntryMapper>();

            foreach (ServiceEntry entry in FabricationManager.CurrentService.ServiceEntries)
                entries.Add(new ServiceEntryMapper(entry));

            dgServiceEntries.ItemsSource = entries;
            cmbServiceType.ItemsSource = _lstServiceTypes;
            cmbLineWeights.ItemsSource = LineWeightValues();
            cmbLayerColor.ItemsSource = LayerColors();
        }

        private void newServiceEntry_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceEntries == null)
                return;

            var entries = dgServiceEntries.ItemsSource as ObservableCollection<ServiceEntryMapper>;
            if (entries == null)
                return;

            // locate the 1st service type that is not used
            ServiceType serviceType = null;
            bool found = false;
            foreach (ServiceType st in _lstServiceTypes)
            {
                found = false;
                foreach (ServiceEntry entry in FabricationManager.CurrentService.ServiceEntries)
                {
                    if (entry.ServiceType.Id == st.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    serviceType = st;
                    break;
                }
            }

            if (serviceType == null)
            {
                // could not add a new entry
                return;
            }

            ServiceEntry newEntry = FabricationAPIExamples.AddNewServiceEntry(FabricationManager.CurrentService, serviceType);
            entries.Add(new ServiceEntryMapper(newEntry));
        }

        private void deleteServiceEntry_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceEntries.SelectedItem == null)
                return;

            var entry = dgServiceEntries.SelectedItem as ServiceEntryMapper;
            if (entry == null)
                return;

            if (MessageBox.Show("Confirm to Delete Service Entry", "Delete Service Entry",
        MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (FabricationAPIExamples.DeleteServiceEntry(FabricationManager.CurrentService, entry.ServiceEntry))
                {
                    LoadServiceEntries();
                }
            }

        }

        private void ServiceTypeComboChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            var selectedType = e.AddedItems[0] as ServiceType;
            if (selectedType == null)
                return;

            var selectedRow = dgServiceEntries.SelectedItem as ServiceEntryMapper;
            if (selectedRow == null)
                return;

            selectedRow.ServiceTypeId = selectedType.Id;
            selectedRow.ServiceTypeDescription = selectedType.Description;
        }

        private void btnUpdateServiceEntries_Click(object sender, RoutedEventArgs e)
        {
            if (dgServiceEntries == null || FabricationManager.CurrentService == null)
                return;

            foreach (ServiceEntryMapper mapper in dgServiceEntries.ItemsSource)
            {
                ServiceEntry entry = mapper.ServiceEntry;
                if (mapper.ServiceTypeId != entry.ServiceType.Id)
                {
                    // locate the ServiceType by id
                    ServiceType st = _lstServiceTypes.ToList().Find(x => x.Id == mapper.ServiceTypeId);
                    if (st != null)
                        entry.ServiceType = st;
                }

                if (!mapper.LayerTag1.Equals(entry.LayerTag1))
                    entry.LayerTag1 = mapper.LayerTag1;

                if (!mapper.LayerTag2.Equals(entry.LayerTag2))
                    entry.LayerTag2 = mapper.LayerTag2;

                if (mapper.LayerColor != entry.LayerColor)
                    entry.LayerColor = mapper.LayerColor;

                if (!mapper.LevelBlock.Equals(entry.LevelBlock))
                    entry.LevelBlock = mapper.LevelBlock;

                if (!mapper.SizeBlock.Equals(entry.SizeBlock))
                    entry.SizeBlock = mapper.SizeBlock;

                if (mapper.LineWeight != entry.LineWeight.LineWeightValue)
                    entry.SetLineWeightValue(mapper.LineWeight);

                if (mapper.IncludesInsulation != entry.IncludesInsulation)
                    entry.IncludesInsulation = mapper.IncludesInsulation;
            }

            dgServiceEntries.UpdateLayout();

            if (Database.SaveServices().Status == ResultStatus.Succeeded)
                MessageBox.Show("Service Entries Saved", "Service Entries", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Service Entries could not be Saved", "Service Entries", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private Dictionary<LineWeight.LineWeightEnum, string> LineWeightValues()
        {
            var values = Enum.GetValues(typeof(LineWeight.LineWeightEnum)).Cast<LineWeight.LineWeightEnum>().ToList();
            var dictionary = new Dictionary<LineWeight.LineWeightEnum, string>();

            Autodesk.Fabrication.Units.MeasurementUnits units = Database.Units;

            foreach (LineWeight.LineWeightEnum lineweight in values)
            {
                string description = LineWeight.GetLineWeightDescription(units, lineweight);
                dictionary.Add(lineweight, description);
            }

            return dictionary;
        }

        private ObservableCollection<int> LayerColors()
        {
            var colors = new ObservableCollection<int>();
            for (int i = 0; i < 256; i++)
                colors.Add(i);

            return colors;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FabricationManager.CurrentService = null;
            FabricationManager.ParentWindow.LoadDBEditorControl();
        }
    }
}
