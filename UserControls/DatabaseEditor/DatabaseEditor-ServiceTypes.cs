using Autodesk.Fabrication;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Results;
using FabricationSample.Data;
using FabricationSample.FunctionExamples;
using FabricationSample.Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        ObservableCollection<ServiceTypeGridItem> _lstServiceTypes;

        #endregion

        #region Service Types

        private void dgServiceTypes_Loaded(object sender, RoutedEventArgs e)
        {
            LoadServiceTypes();
        }

        private void btnUpdateServiceTypes_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            if ((_lstServiceTypes != null) && _lstServiceTypes.Count > 0)
            {
                ObservableCollection<ServiceTypeGridItem> modTypes = new ObservableCollection<ServiceTypeGridItem>(_lstServiceTypes.Where(x => x.Id >= 100).ToList());

                foreach (ServiceTypeGridItem item in modTypes)
                {
                    if (item.Entry == null)
                        FabricationAPIExamples.AddServiceTypeToDB(item.Id, item.Name);
                    else if (item.Id != item.Entry.Id)
                        CastServiceType(item.Entry).UpdateId(item.Id);
                    else if (item.Name != item.Entry.Name)
                        CastServiceType(item.Entry).UpdateName(item.Name);
                }

                LoadServiceTypes();

            }

        }

        private void btnDeleteServiceTypes_Click(object sender, RoutedEventArgs e)
        {
            ServiceTypeGridItem item = dgServiceTypes.SelectedItem as ServiceTypeGridItem;

            if (item != null)
            {
                FabricationAPIExamples.DeleteServiceTypeFromDB(CastServiceType(item.Entry));
                LoadServiceTypes();
            }
        }

        private void LoadServiceTypes()
        {
            var serviceTypes = new List<ServiceTypeGridItem>();

            foreach (ServiceType serv in Database.ServiceTypes)
            {
                serviceTypes.Add(new ServiceTypeGridItem()
                {
                    Name = serv.Name,
                    Id = serv.Id,
                    Entry = serv,
                    CanModify = serv.GetType() == typeof(UserDefinedServiceType) ? true : false
                });
            }

            _lstServiceTypes = new ObservableCollection<ServiceTypeGridItem>(serviceTypes.OrderBy(x => x.Id));
            dgServiceTypes.ItemsSource = _lstServiceTypes;
        }

        private UserDefinedServiceType CastServiceType(ServiceType serviceType)
        {
            UserDefinedServiceType userDefinedType = null;

            if (serviceType.GetType() == typeof(UserDefinedServiceType))
                userDefinedType = serviceType as UserDefinedServiceType;

            return userDefinedType;
        }

        #endregion
    }
}


