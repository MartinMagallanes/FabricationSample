using Autodesk.Fabrication;
using Autodesk.Fabrication.Content;
using Autodesk.Fabrication.DB;
using Autodesk.Fabrication.Events;
using Autodesk.Fabrication.Results;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace FabricationSample.FunctionExamples
{
    class FabricationAPIExamples
    {
        #region AddCustomItemData
        /// <summary>
        /// Add a new Custom Data Entry to the Fabrication Database with a String CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddCustomStringDataToDB(int customDataId, string description, string defaultValue)
        {

            DBOperationResult result = Database.AddCustomItemData<string>(customDataId, CustomDataType.String,
                                                                          CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        /// <summary>
        /// Add a new Custom Data Entry to the Fabrication Database with a Double CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddCustomNumericDataToDB(int customDataId, string description, double defaultValue)
        {

            DBOperationResult result = Database.AddCustomItemData<double>(customDataId, CustomDataType.Double,
                                                                          CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        /// <summary>
        /// Add a new Custom Data Entry to the Fabrication Database with a Integer CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddCustomIntegerDataToDB(int customDataId, string description, int defaultValue)
        {

            DBOperationResult result = Database.AddCustomItemData<int>(customDataId, CustomDataType.Integer,
                                                                          CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        #endregion

        #region DeleteCustomItemData
        /// <summary>
        /// Delete a Custom Data Entry from the Fabrication Database.
        /// </summary>
        /// <param name="data">The Custom Data entry to delete.</param>
        public static void DeleteCustomDataFromDB(CustomData data)
        {

            DBOperationResult result = Database.DeleteCustomItemData(data);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Custom Data deleted and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Custom Data deleted but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region AddCustomJobData
        /// <summary>
        /// Add a new Job Custom Data Entry to the Fabrication Database with a String CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddJobCustomStringDataToDB(int customDataId, string description, string defaultValue)
        {
            DBOperationResult result = Database.AddCustomJobData<string>(customDataId, CustomDataType.String,
                                                                          CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Job Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Add a new Job Custom Data Entry to the Fabrication Database with a Double CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddJobCustomNumericDataToDB(int customDataId, string description, double defaultValue)
        {
            DBOperationResult result = Database.AddCustomJobData<double>(customDataId, CustomDataType.Double, CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Job Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Add a new Job Custom Data Entry to the Fabrication Database with a Integer CustomDataType.
        /// </summary>
        /// <param name="customDataId">The id of the Custom Data Entry.</param>
        /// <param name="description">The description of the Custom Data Entry.</param>
        /// <param name="defaultValue">The default value of the Custom Data Entry.</param>
        public static void AddJobCustomIntegerDataToDB(int customDataId, string description, int defaultValue)
        {
            DBOperationResult result = Database.AddCustomJobData<int>(customDataId, CustomDataType.Integer, CustomDataAddMode.Always, description, defaultValue);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Custom Data added and saved to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Custom Data added but failed to save to Fabrication Database: " +
                      description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Job Custom Data could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region DeleteCustomJobData
        /// <summary>
        /// Delete a Custom Job Data Entry from the Fabrication Database.
        /// </summary>
        /// <param name="data">The Custom Job Data entry to delete.</param>
        public static void DeleteJobCustomDataFromDB(CustomData data)
        {
            DBOperationResult result = Database.DeleteCustomJobData(data);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveCustomData method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveCustomData().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Custom Data deleted and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Custom Data deleted but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region UpdateCustomData
        /// <summary>
        /// Updates the custom data on the JobInfo from the job custom data in the Fabrication Database.
        /// </summary>
        /// <param name="reset">If true, any changes currently set on the JobInfo custom data are disguarded. If false the charges are merged.</param>
        public static void UpdateCustomData(bool reset)
        {
            Autodesk.Fabrication.JobInfo.UpdateCustomData(reset);
        }
        #endregion

        #region AddJobStatusToDB
        /// <summary>
        /// Adds a Job Status entry to the Fabrication Database.
        /// </summary>
        /// <param name="description">The Description of the Job Status entry to add.</param>
        public static JobStatus AddJobStatusToDB(string description)
        {
            JobStatus rv = null;
            DBOperationResult result = Database.AddJobStatus(description);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveJobStatuses method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveJobStatuses().Status == ResultStatus.Succeeded)
                {
                    rv = Database.JobStatuses.Last();
                    MessageBox.Show("Job Status added and saved to Fabrication Database: " +
                         description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Job Status added but failed to save to Fabrication Database: " +
                         description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Job Status could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

            return rv;
        }
        #endregion

        #region MoveJobStatus
        /// <summary>
        /// Delete a Job Status entry from the Fabrication Database.
        /// </summary>
        /// <param name="jobStatus">The Job Status entry to remove from the Fabrication Database.</param>
        public static void MoveJobStatusInDB(JobStatus jobStatus, int by)
        {
            DBOperationResult result = Database.MoveJobStatus(jobStatus, by);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveJobStatuses method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveJobStatuses().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Status moved and changes saved to the Fabrication Database: " +
                                    Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Status move but failed to save changes to the Fabrication Database: " +
                                    Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region DeleteJobStatus
        /// <summary>
        /// Delete a Job Status entry from the Fabrication Database.
        /// </summary>
        /// <param name="jobStatus">The Job Status entry to remove from the Fabrication Database.</param>
        public static void DeleteJobStatusFromDB(JobStatus jobStatus)
        {
            DBOperationResult result = Database.DeleteJobStatus(jobStatus);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveJobStatuses method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveJobStatuses().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Job Status deleted and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Job Status deleted but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region AddItemStatus
        /// <summary>
        /// Adds a Item Status field to the Fabrication Database.
        /// </summary>
        /// <param name="description">The Description of the Item Status field to add.</param>
        /// <param name="layerTag">The Layer Tag of the Item Status field to add.</param>
        /// <param name="output">The Output status of the Item Status field to add.</param>
        /// <param name="color">The Layer Color of the Item Status field to add. Uses AutoCAD Color Index, pass 0 for "ByBlock" or 256 for "ByLayer".</param>
        public static void AddItemStatusToDB(string description, string layerTag, bool output, int color)
        {
            DBOperationResult result = Database.AddItemStatus(description, layerTag, output, color);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveItemStatuses method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveItemStatuses().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Item Status added and saved to Fabrication Database: " +
                                    description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Item Status added but failed to save to Fabrication Database: " +
                                    description + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Item Status could not be added: " + description + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region DeleteItemStatus
        /// <summary>
        /// Delete a Item Status entry from the Fabrication Database.
        /// </summary>
        /// <param name="itemStatus">The Item Status entry to remove from the Fabrication Database.</param>
        public static void DeleteItemStatusFromDB(ItemStatus itemStatus)
        {

            DBOperationResult result = Database.DeleteItemStatus(itemStatus);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveItemStatuses method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveItemStatuses().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Item Status deleted and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Item Status deleted but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region AddServiceType
        /// <summary>
        /// Adds a Service Type entry to the Fabrication Database.
        /// </summary>
        /// <param name="id">The id of the Service Type entry to add.</param>
        /// <param name="name">The name of the Service Type entry to add.</param>
        public static void AddServiceTypeToDB(int id, string name)
        {

            DBOperationResult result = Database.AddServiceType(id, name);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveServiceTypes method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveServiceTypes().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Service Type added and saved to Fabrication Database: " +
                      name + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Service Type added but failed to save to Fabrication Database: " +
                      name + Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Service Type could not be added: " + name + Environment.NewLine + result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region DeleteServiceType
        /// <summary>
        /// Delete a Service Type entry from the Fabrication Database.
        /// </summary>
        /// <param name="serviceType">The Service Type entry to remove from the Fabrication Database.</param>
        public static void DeleteServiceTypeFromDB(UserDefinedServiceType serviceType)
        {

            DBOperationResult result = Database.DeleteServiceType(serviceType);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveServiceTypes method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveServiceTypes().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Service Type deleted and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Service Type deleted but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region UpdateServiceTypeName
        /// <summary>
        /// Updates a Service Type entry's name in the Fabrication Database (Only applies to UserDefinedServiceType Objects).
        /// </summary>
        /// <param name="serviceType">The Service Type entry to update in Fabrication Database.</param>
        /// <param name="name">The updated name to assign to the Service Type entry.</param>
        public static void UpdateServiceTypeName(UserDefinedServiceType serviceType, string name)
        {

            DBOperationResult result = serviceType.UpdateName(name);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveServiceTypes method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveServiceTypes().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Service Type name updated and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Service Type name updated but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region UpdateServiceTypeId
        /// <summary>
        /// Updates a Service Type entry's Id in the Fabrication Database (Only applies to UserDefinedServiceType Objects).
        /// </summary>
        /// <param name="serviceType">The Service Type entry to update in Fabrication Database.</param>
        /// <param name="id">The updated id to assign to the Service Type entry.</param>
        public static void UpdateServiceTypeId(UserDefinedServiceType serviceType, int id)
        {

            DBOperationResult result = serviceType.UpdateId(id);

            //The DBOperationResult will contain a message with information on the success or failure of the operation
            //Save the changes made to the Fabrication Database using the SaveServiceTypes method
            if (result.Status == ResultStatus.Succeeded)
            {
                if (Database.SaveServiceTypes().Status == ResultStatus.Succeeded)
                    MessageBox.Show("Service Type Id updated and changes saved to the Fabrication Database: " +
                      Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Service Type Id updated but failed to save changes to the Fabrication Database: " +
                       Environment.NewLine + result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region ChangeItemConnector
        /// <summary>
        /// Updates an Item's connector at the specified index.
        /// </summary>
        /// <param name="item">The Item to update the connector on.</param>
        /// <param name="updateConnector">The new connector to apply to the item.</param>
        /// <param name="connectorIndex">The connector index to apply to the new connector to.</param>
        public static void ChangeItemConnector(Item item, ConnectorInfo updateConnector, int connectorIndex)
        {
            ItemOperationResult result = item.ChangeConnector(updateConnector, connectorIndex);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
            {
                //Lock new connector to the item to prevent the item's re-applying connector from the specification
                item.Connectors[connectorIndex].IsLocked = true;
                MessageBox.Show("Item Connector #" + connectorIndex.ToString() + " Updated" + Environment.NewLine +
                                        result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
                MessageBox.Show("Item Connector #" + connectorIndex.ToString() + " update Failed" + Environment.NewLine +
                                        result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region GetItemConnectorsByHangerType
        /// <summary>
        /// Gets all connectors on an Item by the Connection Type.
        /// </summary>
        /// <param name="item">The Item to filter the Connectors on.</param>
        /// <param name="connType">The Connection Type to filter by.</param>
        public static ObservableCollection<Connector> GetItemConnectorsByHangerType(Item item)
        {
            ObservableCollection<Connector> lstConnectors = new ObservableCollection<Connector>();

            for (int i = 0; i < item.Connectors.Count; i++)
            {
                //Add filtered connectors to list if type is hanger
                if (item.GetConnectorConnectionType(i) == ConnectionType.Hanger)
                    lstConnectors.Add(item.Connectors[i]);
            }

            return lstConnectors;
        }

        #endregion

        #region ChangeItemMaterial
        /// <summary>
        /// Updates an Item's material.
        /// </summary>
        /// <param name="item">The item to update the material on.</param>
        /// <param name="material">The new material to apply to the item.</param>
        /// <param name="gauge">The material gauge to apply to the updated material.</param>
        /// <param name="checkValidSpecification">Check if the specification is valid for the new material.</param>
        public static void ChangeItemMaterial(Item item, Material material, Gauge gauge, bool checkValidSpecification)
        {
            ItemOperationResult result = item.ChangeMaterial(material, gauge, checkValidSpecification);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
            {
                //Lock new material to the item to prevent any specification override.
                item.IsMaterialLocked = true;
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region ChangeItemInsulation
        /// <summary>
        /// Updates an Item's material.
        /// </summary>
        /// <param name="item">The item to update the insulation on.</param>
        /// <param name="status">The insulation status to apply to the item.</param>
        /// <param name="insulation">The new insulation to apply to the item.</param>
        /// <param name="gauge">The material gauge to apply to the updated material.</param>
        public static void ChangeItemInsulation(Item item, InsulationStatus status, Material insulation, Gauge gauge)
        {
            ItemOperationResult result = item.ChangeInsulation(status, insulation, gauge);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
            {
                //Lock new insulation to the item to prevent any insulation specification override.
                item.Insulation.IsLocked = true;
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region ChangeItemSpecification
        /// <summary>
        /// Updates an Item's specification.
        /// </summary>
        /// <param name="item">The item to update the specification on.</param>
        /// <param name="specification">The specification to apply to the item.</param>
        /// <param name="checkValidMaterial">Check if the material is valid for the new specification.</param>
        public static void ChangeItemSpecification(Item item, Specification specification, bool checkValidMaterial)
        {
            ItemOperationResult result = item.ChangeSpecification(specification, checkValidMaterial);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
            {
                //Lock specification to item to prevent default being applied.
                item.IsSpecificationLocked = true;
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region ChangeItemInsulationSpecification
        /// <summary>
        /// Updates an Item's Insulation Specification.
        /// </summary>
        /// <param name="item">The item to update the insulation specification on.</param>
        /// <param name="insulationSpecification">The insulation specification to apply to the item.</param>
        public static void ChangeItemInsulationSpecification(Item item, InsulationSpecification insulationSpecification)
        {
            ItemOperationResult result = item.ChangeInsulationSpecification(insulationSpecification);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
            {
                //Lock specification to item to prevent default or carry over being applied.
                item.IsInsulationSpecificationLocked = true;
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region ChangeItemCutType
        /// <summary>
        /// Updates an Item's Cut Type.
        /// </summary>
        /// <param name="item">The item to update the insulation specification on.</param>
        /// <param name="cutType">The ItemCutType to apply to the item.</param>
        public static void ChangeItemCutType(Item item, ItemCutType cutType)
        {
            ItemOperationResult result = item.ChangeCutType(cutType);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        #endregion

        #region AddCustomItemDataToItem
        /// <summary>
        /// Adds a Custom Data entry to an Item.
        /// </summary>
        /// <param name="item">The Item to apply the Custom Data entry to.</param>
        /// <param name="data">The Custom Data entry from the Fabrication Database to apply to the Item.</param>
        public static void AddCustomDataToItem(Item item, CustomData data)
        {
            ItemOperationResult result = item.AddCustomData(data);

            //The ItemOperationResult will contain a message with information on the success or failure of the operation
            if (result.Status == ResultStatus.Succeeded)
                MessageBox.Show(result.Message, "Operation Succeeded", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        #endregion


        #region GetCustomJobDataOnJob
        /// <summary>
        /// Gets the JobInfo CustomJobData entry as a string.
        /// </summary>
        /// <param name="data">The CustomJobData entry from the JobInfo.</param>

        public static string GetCustomJobData(CustomJobData data)
        {
            StringBuilder rv = new StringBuilder();

            switch (data.Data.Type)
            {
                case CustomDataType.Double:
                    {
                        CustomJobDataDoubleValue dVal = data as CustomJobDataDoubleValue;
                        rv.Append(dVal.Value);
                    }
                    break;
                case CustomDataType.Integer:
                    {
                        CustomJobDataIntegerValue iVal = data as CustomJobDataIntegerValue;
                        rv.Append(iVal.Value);
                    }
                    break;
                case CustomDataType.String:
                    {
                        CustomJobDataStringValue sVal = data as CustomJobDataStringValue;
                        rv.Append(sVal.Value);
                    }
                    break;
                default:
                    break;
            }
            return rv.ToString();
        }
        #endregion

        #region SetCustomJobDataOnJob
        /// <summary>
        /// Sets the JobInfo CustomJobData entry.
        /// </summary>
        /// <param name="data">The CustomJobData entry from the JobInfo.</param>
        /// <param name="value">The value to set it too.</param>
        public static void SetCustomJobData(CustomJobData data, string value)
        {
            bool updated = false;

            switch (data.Data.Type)
            {
                case CustomDataType.Double:
                    {
                        CustomJobDataDoubleValue dVal = data as CustomJobDataDoubleValue;
                        double d = 0.0;
                        if (double.TryParse(value, out d))
                        {
                            dVal.Value = d;
                            updated = true;
                        }
                    }
                    break;
                case CustomDataType.Integer:
                    {
                        CustomJobDataIntegerValue iVal = data as CustomJobDataIntegerValue;
                        int i = 0;
                        if (int.TryParse(value, out i))
                        {
                            iVal.Value = i;
                            updated = true;
                        }
                    }
                    break;
                case CustomDataType.String:
                    {
                        CustomJobDataStringValue sVal = data as CustomJobDataStringValue;
                        sVal.Value = value;
                        updated = true;
                    }
                    break;
                default:
                    break;
            }

            if (updated)
                MessageBox.Show("Custom Job Data Updated: " + data.Data.Description, "Custom Job Data Update", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Custom Job Data Update Failed: " + data.Data.Description, "Custom Job Data Update", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region UpdateCustomItemDataOnItem
        /// <summary>
        /// Updates a Custom Item Data entry's value on an Item.
        /// </summary>
        /// <param name="item">The Item to apply the Custom Data entry to.</param>
        /// <param name="data">The Custom Data entry from the Fabrication Database to apply to the Item.</param>
        /// <param name="value">The updated value to apply to the .</param>
        public static void UpdateCustomItemData(Item item, CustomItemData data, string value)
        {
            bool updated = false;

            switch (data.Data.Type)
            {
                case CustomDataType.Double:
                    {
                        CustomDataDoubleValue dVal = data as CustomDataDoubleValue;
                        double d = 0.0;
                        if (double.TryParse(value, out d))
                        {
                            dVal.Value = d;
                            updated = true;
                        }
                    }
                    break;
                case CustomDataType.Integer:
                    {
                        CustomDataIntegerValue iVal = data as CustomDataIntegerValue;
                        int i = 0;
                        if (int.TryParse(value, out i))
                        {
                            iVal.Value = i;
                            updated = true;
                        }
                    }
                    break;
                case CustomDataType.String:
                    {
                        CustomDataStringValue sVal = data as CustomDataStringValue;
                        sVal.Value = value;
                        updated = true;
                    }
                    break;
                default:
                    break;
            }

            if (updated)
                MessageBox.Show("Custom Item Data Updated: " + data.Data.Description, "Custom Item Data Update", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Custom Item Data Update Failed: " + data.Data.Description, "Custom Item Data Update", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region CheckValidSpecificationForItem
        /// <summary>
        /// Checks that a specification is valid to assign to a Fabrication Item.
        /// </summary>
        /// <param name="item">The Fabrication Item to check.</param>
        /// <param name="specification">The specification to check is valid.</param>
        public static bool CheckSpecificationIsValidForItem(Item item, Specification specification)
        {
            bool isValid = specification.IsValidFor(item);
            if (isValid)
                MessageBox.Show("Specification: " + specification.Name + " is valid for item", "Valid Specification", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Specification: " + specification.Name + " is not valid for item", "Invalid Specification", MessageBoxButton.OK, MessageBoxImage.Error);

            return isValid;
        }

        #endregion

        #region GetServiceButtonImage
        /// <summary>
        /// Get an Image From a Service Button's image filename.
        /// </summary>
        /// <param name="button">The Service Button to get its image filename from.</param>
        /// <returns>Image</returns>
        public static Image GetServiceButtonImageFilename(ServiceButton button)
        {
            var filename = button.GetButtonImageFilename();

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Image.FromStream(fs);
            }
        }

        #endregion

        #region LINQItemQueryExamples
        public static void QueryJobItems()
        {
            //Find all Job Items that have Insulation Set
            List<Item> lstInsulatedItems = Job.Items.Where(x => x.Insulation.IsOn).ToList();

            //Find all Job Items that a Service of "General Supply"
            List<Item> lstGenSupplyItems = Job.Items.Where(x => x.Service.Name == "General Supply").ToList();

            //Find all Job Items that are Straights e.g. CID 35,40,866,2041 
            List<Item> lstStraightItems = Job.Items.Where(x => x.IsStraight).ToList();

            //Find all Job Items that have a specification of "DW144-LV"
            List<Item> lstSpecItems = Job.Items.Where(x => x.Specification.Name == "DW144-LV").ToList();

            //Find all Job Items that have a section of "1st Floor"
            List<Item> lstSectionItems = Job.Items.Where(x => x.Section.Description == "1st Floor").ToList();

        }
        #endregion

        #region LINQItemModifyItemServiceExamples
        public static void ModifyJobItemService()
        {
            //Find all Job Items that have a Service of "General Supply" and change to "General Extract"

            //Find all Job Items that a Service of "General Supply"
            List<Item> lstGenSupplyItems = Job.Items.Where(x => (x.Service != null) && x.Service.Name == "General Supply").ToList();

            //Get the General Extract Service from the Fabrication Database
            Service genExtract = Database.Services.FirstOrDefault(x => x.Name == "General Extract");

            if (genExtract != null)
            {
                lstGenSupplyItems.ForEach(x => x.Service = genExtract);
                //Update the UI View to reflect changes
                Autodesk.Fabrication.UI.UIApplication.UpdateView(lstGenSupplyItems);
            }
        }
        #endregion

        #region LINQItemModifyItemSectionExamples
        public static void ModifyJobItemSection()
        {
            //Find all Job Items that have a section of "Ground Floor" and change to "First Floor"

            //Find all Job Items that have a section of "Ground Floor"
            List<Item> lstSectionItems = Job.Items.Where(x => (x.Section != null) && x.Section.Description == "Ground Floor").ToList();

            //Get the "1st Floor" Section from the Fabrication Database
            Section firstFloor = Database.Sections.FirstOrDefault(x => x.Description == "1st Floor");

            if (firstFloor != null)
                lstSectionItems.ForEach(x => x.Section = firstFloor);
        }
        #endregion

        #region LINQItemModifyItemDimensionExamples
        public static void ModifyJobItemDimensions()
        {
            //Find all Job Items that have a CID number 866 and change length value

            //Find all Job Items that have a section of "Ground Floor"
            List<Item> lstStraight = Job.Items.Where(x => x.CID == 866).ToList();

            foreach (Item itm in lstStraight)
            {
                //Find Length Dimension
                ItemDimensionBase baseDim = itm.Dimensions.FirstOrDefault(x => x.Name == "Length");
                if (baseDim != null)
                {
                    ItemComboDimension comboDim = baseDim as ItemComboDimension;
                    if (comboDim != null)
                    {
                        //Get the "Value" entry of the ComboDimension
                        //ComboDimensions will only contain one ItemComboDimensionValueEntry
                        ItemComboDimensionValueEntry valDimEntry = comboDim.Options.FirstOrDefault(x => x.GetType() == typeof(ItemComboDimensionValueEntry)) as ItemComboDimensionValueEntry;
                        if (valDimEntry != null)
                        {
                            //Set the value for the dimension and set to selected
                            valDimEntry.Value = 1000;
                            valDimEntry.IsSelected = true;
                        }
                    }
                }
            }
            //Update the UI View to reflect changes
            Autodesk.Fabrication.UI.UIApplication.UpdateView(lstStraight);
        }
        #endregion

        #region LINQItemModifyItemOptionsExamples
        public static void ModifyJobItemOptions()
        {
            //Find all Job Items that have a CID number 866 and Override option to 

            //Find all Job Items that have a section of "Ground Floor"
            List<Item> lstStraight = Job.Items.Where(x => x.CID == 866).ToList();

            foreach (Item itm in lstStraight)
            {
                //Find Override Option
                ItemOptionBase baseOpt = itm.Options.FirstOrDefault(x => x.Name == "Override");
                if (baseOpt != null)
                {
                    ItemSelectOption selectOpt = baseOpt as ItemSelectOption;
                    if (selectOpt != null)
                    {
                        //Get the ItemOptionEntry and set to Selected
                        ItemOptionEntry optEntry = selectOpt.Options.FirstOrDefault(x => x.Name == "2xL") as ItemOptionEntry;

                        if (optEntry != null)
                            optEntry.IsSelected = true;
                    }
                }
            }
        }
        #endregion

        #region LINQDatabaseReadExamples

        public static string QueryDatabaseServices()
        {
            StringBuilder builder = new StringBuilder();

            //Add current Fabrication Configuration Name
            builder.AppendLine("Services contained in: " + Autodesk.Fabrication.ApplicationServices.Application.CurrentConfiguration);

            //Query all services in current Fabrication Database
            Database.Services.ToList().ForEach(x => builder.AppendLine(x.Name));

            return builder.ToString();
        }

        #endregion

        #region CreateItem

        /// <summary>
        /// Saves an Autodesk Fabrication Item with the option to create a copy
        /// </summary>
        /// <param name="CidNumber">The CID number of the pattern to use for the Item creation.</param>
        /// <param name="certified">Set the certified property on the item.</param>
        /// <param name="fixRelative">Set the fix relative property on the item.</param>
        /// <param name="catalogue">Set the catalogue property on the item.</param>
        /// <returns>A new Fabrication Item, null if the item was not created.</returns>
        public static Item CreateItem(int CidNumber, bool certified, bool fixRelative, bool catalogue, bool boughtOut)
        {
            Item itm = null;
            itm = ContentManager.CreateItem(CidNumber);

            if (itm != null)
            {
                if (certified)
                    itm.IsCertified = true;

                if (fixRelative)
                    itm.IsFixRelative = true;

                if (catalogue)
                    itm.IsCatalogue = true;

                if (boughtOut)
                    itm.BoughtOut = true;
            }

            return itm;
        }
        #endregion

        #region LoadItem

        /// <summary>
        /// Loads an Item from disk and updates properties.
        /// </summary>
        /// <param name="path">The path to the item on disk, including the item name i.e. path/item.itm</param>
        /// <param name="alias">The alias property to update on the loaded item</param>
        /// <param name="orderNumber">The order property to update on the loaded item</param>
        /// <param name="notes">The notes property to update on the loaded item</param>
        /// <returns>True if the item was loaded and updated, false if not.</returns>
        public static bool LoadItemAndUpdateProperties(string path, string alias, string orderNumber, string notes)
        {
            bool updated = false;
            Item itm = null;
            itm = ContentManager.LoadItem(path);

            if (itm != null)
            {
                itm.Alias = alias;
                itm.Order = orderNumber;
                itm.Notes = notes;

                if (ContentManager.SaveItem(itm).Status == ResultStatus.Succeeded)
                    updated = true;
            }

            return updated;
        }
        #endregion

        #region SaveItem

        /// <summary>
        /// Saves an Autodesk Fabrication Item with the option to create a copy
        /// </summary>
        /// <param name="itm">The Item to save.</param>
        /// <param name="createCopy">Option to create a copy of the item.</param>
        /// <param name="overWriteExisting">Overwrite exisitng item if exists</param>
        /// <returns>Saved, true or false.</returns>
        public static bool SaveItem(Item itm, bool createCopy, bool overWriteExisting)
        {
            bool saved = false;

            if (!createCopy)
            {
                if (ContentManager.SaveItem(itm).Status == ResultStatus.Succeeded)
                    saved = true;
            }
            else
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog()
                {
                    InitialDirectory = System.IO.Path.GetFullPath(Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath),
                    OverwritePrompt = true,
                    AddExtension = true,
                    DefaultExt = "itm",
                    Filter = "Autodesk Fabrication ITM files (*.itm)|*.itm",
                    CheckPathExists = true,
                    Title = "Save Item File",
                    FileName = "Copy of " + itm.Name,
                    RestoreDirectory = true
                };

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName = sfd.FileName;
                    if (ContentManager.SaveItemAs(itm, System.IO.Path.GetDirectoryName(fileName),
                                                System.IO.Path.GetFileNameWithoutExtension(fileName), overWriteExisting).Status == ResultStatus.Succeeded)
                        saved = true;
                }
            }

            return saved;
        }
        #endregion

        #region CreateProductItem

        /// <summary>
        /// Creates a Product List Round Duct Bend
        /// </summary>
        /// <returns>Created, true or false</returns>
        public static bool CreateProductDuctBendItem()
        {
            bool created = false;

            //Create Round Duct Bend, CID 523
            Item itm = null;
            itm = ContentManager.CreateItem(523);

            if (itm != null)
            {
                //Create the data template to apply to the Product List
                ItemProductListDataTemplate template = new ItemProductListDataTemplate();
                //Define the dimensions, options and attributes for the template
                //Default values can be applied for dimensions, options and attributes

                //Set the template to use the alias attribute and apply a default value
                template.SetAlias("SegBend");
                //Set the template to use the order attribute and apply a default value
                template.SetOrderNumber("ADSK-FAB-ORDER-xxx");
                //Set the template to use the bought out attribute and apply a default value
                template.SetBoughtOut(true);
                //Set the template to use the databaseId attribute and apply a default value
                template.SetDatabaseId("ADSK-FAB-ID-xxx");

                //Set the dimensions to apply to the template

                //Find the "Diameter" dimension from the item
                ItemDimensionBase dimDiameter = itm.Dimensions.FirstOrDefault(x => x.Name == "Diameter");
                double defaultDiameter = 50.0;

                //Add the "Diameter" Dimension Definition to the template
                if (dimDiameter != null)
                    template.AddDimensionDefinition(new ItemProductListDimensionDefinition(dimDiameter, true), defaultDiameter);

                //Find the "Top Extension" dimension from the item
                ItemDimensionBase dimTopExtn = itm.Dimensions.FirstOrDefault(x => x.Name == "Top Extension");

                //Add the "Top Extension" Dimension Definition to the template
                if (dimTopExtn != null)
                    template.AddDimensionDefinition(new ItemProductListDimensionDefinition(dimTopExtn, true), defaultDiameter / 2);

                //Find the "Bottom Extension" dimension from the item
                ItemDimensionBase dimBottomExtn = itm.Dimensions.FirstOrDefault(x => x.Name == "Bottom Extension");

                //Add the "Bottom Extension" Dimension Definition to the template
                if (dimBottomExtn != null)
                    template.AddDimensionDefinition(new ItemProductListDimensionDefinition(dimBottomExtn, true), defaultDiameter / 2);

                //Find the "Number Of Segments" option from the item
                ItemOptionBase optNoOfSegments = itm.Options.FirstOrDefault(x => x.Name == "Number Of Segments");

                //Add the "Number Of Segments" Option Definition to the template
                if (optNoOfSegments != null)
                    template.AddOptionDefinition(new ItemProductListOptionDefinition(optNoOfSegments, true), 4);

                //Create a new ItemProductList Object and apply the template
                //An ItemProductList Object must have a template set to be applied to an Item
                ItemProductList prodList = new ItemProductList();
                //Add the template
                prodList.AddDataTemplate(template);

                //Add rows to the ItemProductList
                //The data added to the row must conform to the template, for example if the weight attribute is passed as parameter of "AddRow" method
                //it will not be applied as it has not been defined on the template
                List<ItemProductListDimensionEntry> dimEntries = new List<ItemProductListDimensionEntry>();

                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Diameter").CreateDimensionEntry(50.0));
                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Top Extension").CreateDimensionEntry(25.0));
                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Bottom Extension").CreateDimensionEntry(25.0));

                List<ItemProductListOptionEntry> optEntries = new List<ItemProductListOptionEntry>();

                optEntries.Add(template.OptionsDefinitions.FirstOrDefault(x => x.Name == "Number Of Segments").CreateOptionEntry(4));

                //Add new row
                prodList.AddRow("50", "SegBend", null, null, string.Empty, "ADSK-FAB-ORDER-50", "ADSK-FAB-ID-50", true, null, null,
                                          new List<ItemProductListDimensionEntry>(dimEntries), new List<ItemProductListOptionEntry>(optEntries));

                //Clear and repopulate
                dimEntries.Clear();
                optEntries.Clear();

                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Diameter").CreateDimensionEntry(100.0));
                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Top Extension").CreateDimensionEntry(50.0));
                dimEntries.Add(template.DimensionsDefinitions.FirstOrDefault(x => x.Name == "Bottom Extension").CreateDimensionEntry(50.0));

                optEntries.Add(template.OptionsDefinitions.FirstOrDefault(x => x.Name == "Number Of Segments").CreateOptionEntry(5));

                //Add new row
                prodList.AddRow("100", "SegBend", null, null, string.Empty, "ADSK-FAB-ORDER-100", "ADSK-FAB-ID-100", true, null, null,
                                          new List<ItemProductListDimensionEntry>(dimEntries), new List<ItemProductListOptionEntry>(optEntries));

                //Lock dimensions i.e. Angle, Inner Radius
                ItemDimension dimAngle = itm.Dimensions.FirstOrDefault(x => x.Name == "Angle") as ItemDimension;
                if (dimAngle != null)
                {
                    dimAngle.Value = 90.0;
                    dimAngle.IsLocked = true;
                }

                ItemComboDimension dimInnerRadius = itm.Dimensions.FirstOrDefault(x => x.Name == "Inner Radius") as ItemComboDimension;
                if (dimInnerRadius != null)
                {
                    ItemComboDimensionEntry dimEntryHalfRadius = dimInnerRadius.Options.FirstOrDefault(x => x.Name == "Half Diameter");
                    if (dimEntryHalfRadius != null)
                    {
                        dimEntryHalfRadius.IsSelected = true;
                        dimInnerRadius.IsLocked = true;
                    }
                }

                //Set the Product List revision entry
                prodList.Revision = "ADSK-FAB-REV-1";

                //Create the Product List Item
                if (ContentManager.CreateProductItem(itm, prodList).Status == ResultStatus.Succeeded)
                {
                    //Save to disk and assign image
                    string itmFolder = Path.Combine(Path.GetFullPath(Autodesk.Fabrication.ApplicationServices.Application.ItemContentPath), "HVAC");
                    string itmName = "Segment Bend";
                    string imgPath = Path.Combine(itmFolder, "bend.png");

                    if (File.Exists(imgPath))
                        itm.SetImage(imgPath);

                    if (ContentManager.SaveItemAs(itm, itmFolder, itmName, true).Status == ResultStatus.Succeeded)
                        created = true;
                }
            }

            return created;
        }
        #endregion

        #region SetItemImage

        /// <summary>
        /// Sets an image file to display with the item.
        /// </summary>
        /// <param name="itm">The item to apply the image to.</param>
        /// <returns>Image set, true or false.</returns>
        public static bool SetItemImage(Item itm)
        {
            bool imageSet = false;

            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog()
            {
                CheckPathExists = true,
                Filter = "PNG (*.png)|*.png",
                Title = "Select PNG File"
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = dlg.FileName;

                ItemOperationResult result = itm.SetImage(imagePath);

                if (result.Status == ResultStatus.Succeeded)
                {
                    imageSet = true;
                    MessageBox.Show(result.Message, "Operation Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show(result.Message, "Operation Failed", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            return imageSet;
        }

        #endregion

        #region LoadServiceItem

        /// <summary>
        /// Loads an item from the service.
        /// </summary>
        /// <param name="service">The Service to load the Item from.</param>
        /// <param name="button">The ServiceButton to load the Item from.</param>
        /// <param name="buttonItem">The ServiceButtonItem to load the Item from.</param>
        /// <param name="carrySpecs">To carry over specifications from a previous item, pass true. If there
        /// is no previous item, this will set up this item with the default specification.
        /// </param>
        /// <returns>True if the Item was loaded.</returns>
        public static Item LoadServiceItem(Service service, ServiceButton button, ServiceButtonItem buttonItem, bool carrySpecs)
        {
            Item item = null;
            ItemOperationResult result = service.LoadServiceItem(button, buttonItem, carrySpecs);
            if (result.Status == ResultStatus.Succeeded)
                item = result.ReturnObject as Item;

            return item;
        }

        #endregion

        #region AddItemToJob
        /// <summary>
        /// Adds a disk Item to the current Job.
        /// </summary>
        /// <param name="job">The Job which to load the Item into.</param>
        /// <param name="item">The Item to be added to the Job.</param>
        /// <returns>True if the Item was added to the Job.</returns>
        public static bool AddItemToJob(Item item)
        {
            bool added = false;
            ItemOperationResult result = Job.AddItem(item);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                added = true;
            }

            MessageBox.Show(result.Message, "Add Item to Job", MessageBoxButton.OK, messageImage);

            return added;
        }
        #endregion

        #region DeleteJobItem
        /// <summary>
        /// Delete an Item from the current Job
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True or False</returns>
        public static bool DeleteJobItem(Item item)
        {
            bool deleted = false;

            ItemOperationResult result = Job.DeleteItem(item);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Job Item", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region AddNewServiceEntry

        /// <summary>
        /// Adds a new ServiceEntry into the Service.
        /// </summary>
        /// <param name="service">The Service which to add the ServiceEntry to.</param>
        /// <param name="serviceType">The ServiceType of the ServiceEntry.</param>
        /// <returns>If successful the new ServiceEntry, otherwise null </returns>
        public static ServiceEntry AddNewServiceEntry(Service service,
                                              ServiceType serviceType)
        {
            ServiceEntry newEntry = null;
            DBOperationResult result = service.AddServiceEntry(serviceType);
            if (result.Status == ResultStatus.Succeeded)
                newEntry = result.ReturnObject as ServiceEntry;

            return newEntry;
        }

        #endregion

        #region DeleteServiceEntry

        /// <summary>
        /// Deletes a ServiceEntry from the Service.
        /// </summary>
        /// <param name="service">The Service from which to delete the ServiceEntry.</param>
        /// <param name="serviceEntry">The ServiceEntry to delete.</param>
        /// <returns>True if successful </returns>
        public static bool DeleteServiceEntry(Service service,
                                              ServiceEntry serviceEntry)
        {
            DBOperationResult result = service.DeleteServiceEntry(serviceEntry);
            if (result.Status == ResultStatus.Succeeded)
                return true;

            return false;
        }

        #endregion

        #region AddNewService

        /// <summary>
        /// Adds a new Service into the Database.
        /// </summary>
        /// <param name="name">The name of the new Service.</param>
        /// <param name="group">The group name of the new Service.</param>
        /// <param name="serviceTemplate">The ServiceTemplate to create the Service from.</param>
        /// <returns>If successful the new Service, otherwise null </returns>    
        public static Service AddNewService(string name, string group, ServiceTemplate serviceTemplate)
        {
            Service newService = null;
            DBOperationResult result = Database.AddService(name, group, serviceTemplate);
            if (result.Status == ResultStatus.Succeeded)
            {
                newService = result.ReturnObject as Service;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service", MessageBoxButton.OK, messageImage);

            return newService;
        }


        #endregion

        #region DeleteService

        /// <summary>
        /// Deletes a Service from the Database.
        /// </summary>
        /// <param name="service">The Service to delete.</param>
        /// <returns>True or False</returns>    
        public static bool DeleteService(Service service)
        {
            bool deleted = false;

            DBOperationResult result = Database.DeleteService(service);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                deleted = true;
                messageImage = MessageBoxImage.Information;
            }
            MessageBox.Show(result.Message, "Delete Service", MessageBoxButton.OK, messageImage);

            return deleted;
        }


        #endregion

        #region AddNewServiceTemplate

        /// <summary>
        /// Adds a new ServiceTemplate into the Database.
        /// </summary>
        /// <param name="name">The name of the new ServiceTemplate.</param>
        /// <returns>If successful the new ServiceTemplate, otherwise null </returns>
        public static ServiceTemplate AddNewServiceTemplate(string name)
        {
            ServiceTemplate newServiceTemplate = null;
            DBOperationResult result = Database.AddServiceTemplate(name);
            if (result.Status == ResultStatus.Succeeded)
            {
                newServiceTemplate = result.ReturnObject as ServiceTemplate;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service Template", MessageBoxButton.OK, messageImage);

            return newServiceTemplate;
        }

        #endregion

        #region DeleteServiceTemplate

        /// <summary>
        /// Deletes a ServiceTemplate from the Database.
        /// </summary>
        /// <param name="serviceTemplate">The ServiceTemplate to delete.</param>
        /// <returns>True if successful </returns>
        public static bool DeleteServiceTemplate(ServiceTemplate serviceTemplate)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteServiceTemplate(serviceTemplate);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }
            MessageBox.Show(result.Message, "Delete Service Template", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewServiceTab

        /// <summary>
        /// Adds a new ServiceTab on the ServiceTemplate.
        /// </summary>
        /// <param name="serviceTemplate">The ServiceTemplate on which to create the ServiceTab</param>
        /// <param name="name">The name of the new ServiceTab.</param>
        /// <returns>If successful the new ServiceTab, otherwise null </returns>
        public static ServiceTab AddNewServiceTab(ServiceTemplate serviceTemplate, string name)
        {
            ServiceTab newServiceTab = null;
            DBOperationResult result = serviceTemplate.AddServiceTab(name);
            if (result.Status == ResultStatus.Succeeded)
            {
                newServiceTab = result.ReturnObject as ServiceTab;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service Tab", MessageBoxButton.OK, messageImage);

            return newServiceTab;
        }


        #endregion

        #region DeleteServiceTab

        /// <summary>
        /// Deletes a ServiceTab from the ServiceTemplate.
        /// </summary>
        /// <param name="serviceTemplate">The ServiceTemplate from which to delete the ServiceTab.</param>
        /// <param name="serviceTab">The ServiceTab to delete.</param>
        /// <returns>True if successful </returns>
        public static bool DeleteServiceTab(ServiceTemplate serviceTemplate, ServiceTab serviceTab)
        {
            bool deleted = false;
            DBOperationResult result = serviceTemplate.DeleteServiceTab(serviceTab);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }
            MessageBox.Show(result.Message, "Delete Service Tab", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewServiceButton

        /// <summary>
        /// Adds a new ServiceButton on the ServiceTab.
        /// </summary>
        /// <param name="tab">The ServiceTab on which to create the ServiceButton</param>
        /// <param name="name">The name of the new ServiceTab.</param>
        /// <returns>If successful the new ServiceButton, otherwise null </returns>
        public static ServiceButton AddNewServiceButton(ServiceTab tab, string name)
        {
            ServiceButton newServiceButton = null;
            DBOperationResult result = tab.AddServiceButton(name);
            if (result.Status == ResultStatus.Succeeded)
            {
                newServiceButton = result.ReturnObject as ServiceButton;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service Button", MessageBoxButton.OK, messageImage);

            return newServiceButton;
        }


        #endregion

        #region DeleteServiceButton

        /// <summary>
        /// Deletes a ServiceButton from the ServiceTab.
        /// </summary>
        /// <param name="serviceTab">The ServiceTab from which to delete the ServiceButton.</param>
        /// <param name="serviceButton">The ServiceButton to delete.</param>
        /// <returns>True if successful </returns>
        public static bool DeleteServiceButton(ServiceTab serviceTab, ServiceButton serviceButton)
        {
            bool deleted = false;
            DBOperationResult result = serviceTab.DeleteServiceButton(serviceButton);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }
            MessageBox.Show(result.Message, "Delete Service Button", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewServiceButtonItem

        /// <summary>
        /// Adds a new ServiceButtonItem on the ServiceButton.
        /// </summary>
        /// <param name="button">The ServiceButton on which to create the ServiceButtonItem</param>
        /// <param name="path">The path of the itm file to add.</param>
        /// <param name="templateCondition">The ServiceTemplateCondition to use.</param>
        /// <returns>If successful the new ServiceButtonItem, otherwise null </returns>
        public static ServiceButtonItem AddNewServiceButtonItem(ServiceButton button, string path, ServiceTemplateCondition templateCondition)
        {
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (button == null)
            {
                MessageBox.Show("Select a button first", "Add New Service Button Item", MessageBoxButton.OK, messageImage);
                return null;
            }

            ServiceButtonItem newServiceButtonItem = null;
            DBOperationResult result = button.AddServiceButtonItem(path, templateCondition);
            if (result.Status == ResultStatus.Succeeded)
            {
                newServiceButtonItem = result.ReturnObject as ServiceButtonItem;
            }

            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service Button Item", MessageBoxButton.OK, messageImage);

            return newServiceButtonItem;
        }


        #endregion

        #region DeleteServiceButtonItem

        /// <summary>
        /// Deletes a ServiceButtonItem from the ServiceButton
        /// </summary>
        /// <param name="serviceButton">The ServiceButton from which to delete the ServiceButtonItem.</param>
        /// <param name="buttonItem">The ServiceButtonItem to delete.</param>
        /// <returns>True if successful </returns>
        public static bool DeleteServiceButtonItem(ServiceButton serviceButton, ServiceButtonItem buttonItem)
        {
            bool deleted = false;
            DBOperationResult result = serviceButton.DeleteServiceButtonItem(buttonItem);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }
            MessageBox.Show(result.Message, "Delete Service Button Item", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region SetConditionOverride

        /// <summary>
        /// Sets the LessThanEqualTo and GreaterThan properties on the ServiceButtonItem.
        /// </summary>
        /// <param name="buttonItem">The ServiceButtonItem on which to set the condition override values.</param>
        /// <param name="greaterThan">The value of the GreaterThan property to set on the ServiceButtonItem.</param>
        /// <param name="lessThanEqualTo">The value of the LessThanEqualTo property to set on the ServiceButtonItem.</param>
        /// <returns>True if successful</returns>
        public static bool SetConditionOverride(ServiceButtonItem buttonItem, double greaterThan, double lessThanEqualTo)
        {
            DBOperationResult result = buttonItem.SetConditionOverride(greaterThan, lessThanEqualTo);
            if (result.Status == ResultStatus.Succeeded)
                return true;

            MessageBoxImage messageImage = MessageBoxImage.Error;
            MessageBox.Show(result.Message, "Set Service Button Item Condition Override", MessageBoxButton.OK, messageImage);

            return false;
        }

        #endregion

        #region AddNewServiceTemplateCondition

        /// <summary>
        /// Adds a new ServiceTemplateCondition on the ServiceTemplate.
        /// </summary>
        /// <param name="serviceTemplate">The ServiceTemplate on which to create the ServiceTemplateCondition</param>
        /// <param name="description">The description of the ServiceTemplateCondition.</param>
        /// <param name="greaterThan">The GreaterThan property to set on the new ServiceTemplateCondition.</param>
        /// <param name="lessThanEqualTo">The LessThanEqualTo property to set on the new ServiceTemplateCondition</param>
        /// <returns>If successful the new ServiceTemplateCondition, otherwise null </returns>
        public static ServiceTemplateCondition AddNewServiceTemplateCondition(ServiceTemplate serviceTemplate, string description, double greaterThan, double lessThanEqualTo)
        {

            ServiceTemplateCondition newServiceTemplateCondition = null;
            DBOperationResult result = serviceTemplate.AddServiceTemplateCondition(description, greaterThan, lessThanEqualTo);
            if (result.Status == ResultStatus.Succeeded)
            {
                newServiceTemplateCondition = result.ReturnObject as ServiceTemplateCondition;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Service Template Condition", MessageBoxButton.OK, messageImage);

            return newServiceTemplateCondition;
        }


        #endregion

        #region DeleteServiceTemplateCondition

        /// <summary>
        /// Deletes the ServiceTemplateCondition from the ServiceTemplate.
        /// </summary>
        /// <param name="serviceTemplate">The ServiceTemplate on which to delete the ServiceTemplateCondition</param>
        /// <param name="condition">The ServiceTemplateCondition to delete.</param>
        /// <returns>True if successful</returns>
        public static bool DeleteServiceTemplateCondition(ServiceTemplate serviceTemplate, ServiceTemplateCondition condition)
        {
            bool deleted = false;
            DBOperationResult result = serviceTemplate.DeleteServiceTemplateCondition(condition);

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }
            MessageBox.Show(result.Message, "Delete Service Template Condition", MessageBoxButton.OK, messageImage);

            return deleted;
        }


        #endregion

        #region SetServiceTemplateConditionValues

        /// <summary>
        /// Sets the LessThanEqualTo and GreaterThan properties on the ServiceTemplateCondition.
        /// </summary>
        /// <param name="serviceTemplateCondition">The ServiceTemplate on which to set the condition values.</param>
        /// <param name="greaterThan">The value of the GreaterThan property to set on the ServiceTemplateCondition.</param>
        /// <param name="lessThanEqualTo">The value of the LessThanEqualTo property to set on the ServiceTemplateCondition.</param>
        /// <returns>True if successful</returns>
        public static bool SetServiceTemplateConditionValues(ServiceTemplateCondition serviceTemplateCondition, double greaterThan, double lessThanEqualTo)
        {
            DBOperationResult result = serviceTemplateCondition.SetConditionValues(greaterThan, lessThanEqualTo);
            if (result.Status == ResultStatus.Succeeded)
                return true;

            MessageBoxImage messageImage = MessageBoxImage.Error;
            MessageBox.Show(result.Message, "Set Service Template Condition Values", MessageBoxButton.OK, messageImage);

            return false;
        }


        #endregion

        #region AddNewMaterial

        /// <summary>
        /// Adds a new Material into the Fabrication Database.
        /// </summary>
        /// <param name="name">The name of the new Material.</param>
        /// <param name="group">The group of the new Material.</param>
        /// <param name="materialUsage">The MaterialUsage of the new Material.</param>
        /// <returns>If successful the new Material, otherwise null </returns>
        public static Material AddNewMaterial(string name, string group, MaterialUsage materialUsage)
        {
            Material newMaterial = null;
            DBOperationResult result = Database.AddMaterial(name, group, materialUsage);
            if (result.Status == ResultStatus.Succeeded)
            {
                newMaterial = result.ReturnObject as Material;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Material", MessageBoxButton.OK, messageImage);

            return newMaterial;
        }

        #endregion

        #region DeleteMaterial

        /// <summary>
        /// Deletes a Material from the Fabrication Database.
        /// </summary>
        /// <param name="material">The Material to delete.</param>
        /// <returns>If successful true, otherwise false </returns>
        public static bool DeleteMaterial(Material material)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteMaterial(material);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Material", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region AddNewDamper

        /// <summary>
        /// Adds a new Damper into the Fabrication Database.
        /// </summary>
        /// <param name="name">The name of the new Damper.</param>
        /// <returns>If successful the new Damper, otherwise null </returns>
        public static DamperInfo AddNewDamper(string name)
        {
            DamperInfo newDamper = null;
            DBOperationResult result = Database.AddDamper(name);
            if (result.Status == ResultStatus.Succeeded)
            {
                newDamper = result.ReturnObject as DamperInfo;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Damper", MessageBoxButton.OK, messageImage);

            return newDamper;
        }

        #endregion

        #region DeleteDamper

        /// <summary>
        /// Deletes a Damper from the Fabrication Database.
        /// </summary>
        /// <param name="damper">The Damper to delete.</param>
        /// <returns>If successful true, otherwise false </returns>
        public static bool DeleteDamper(DamperInfo damper)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteDamper(damper);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Damper", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region AddNewConnector

        /// <summary>
        /// Adds a new Connector into the Fabrication Database.
        /// </summary>
        /// <param name="name">The name of the new Connector.</param>
        /// <param name="group">The group of the new Connector.</param>
        /// <param name="shape">The shape of the new Connector.</param>
        /// <param name="domain">The domain of the new Connector.</param>
        /// <param name="sex">The sex of the new Connector.</param>
        /// <returns>If successful the new Connector, otherwise null </returns>
        public static ConnectorInfo AddNewConnector(string name, string group, ConnectorLibraryType shape, ConnectorDomainType domain, ConnectorSexType sex)
        {
            ConnectorInfo newConnector = null;
            DBOperationResult result = Database.AddConnector(name, group, shape, domain, sex);
            if (result.Status == ResultStatus.Succeeded)
            {
                newConnector = result.ReturnObject as ConnectorInfo;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Connector", MessageBoxButton.OK, messageImage);

            return newConnector;
        }

        #endregion

        #region DeleteConnector

        /// <summary>
        /// Deletes a Connector from the Fabrication Database.
        /// </summary>
        /// <param name="connector">The Connector to delete.</param>
        /// <returns>If successful true, otherwise false </returns>
        public static bool DeleteConnector(ConnectorInfo connector)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteConnector(connector);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Connector", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region AddNewStiffener

        /// <summary>
        /// Adds a new Stiffener into the Fabrication Database.
        /// </summary>
        /// <param name="name">The name of the new Stiffener.</param>
        /// <returns>If successful the new Stiffener, otherwise null </returns>
        public static StiffenerInfo AddNewStiffener(string name)
        {
            StiffenerInfo newStiffener = null;
            DBOperationResult result = Database.AddStiffener(name);
            if (result.Status == ResultStatus.Succeeded)
            {
                newStiffener = result.ReturnObject as StiffenerInfo;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Stiffener", MessageBoxButton.OK, messageImage);

            return newStiffener;
        }

        #endregion

        #region DeleteStiffener

        /// <summary>
        /// Deletes a Stiffener from the Fabrication Database.
        /// </summary>
        /// <param name="stiffener">The Stiffener to delete.</param>
        /// <returns>If successful true, otherwise false </returns>
        public static bool DeleteStiffener(StiffenerInfo stiffener)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteStiffener(stiffener);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Stiffener", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region AddNewGauge

        /// <summary>
        /// Adds a new Gauge to the Material.
        /// </summary>
        /// <param name="material">The Material on which to add the new Gauge.</param>
        /// <param name="materialType">The MaterialType of the new Gauge.</param>
        /// <returns>If successful the new Gauge, otherwise null.</returns>
        public static Gauge AddNewGauge(Material material, MaterialType materialType)
        {
            Gauge newGauge = null;
            DBOperationResult result = material.AddGauge(materialType);
            if (result.Status == ResultStatus.Succeeded)
            {
                newGauge = result.ReturnObject as Gauge;
            }

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;
            MessageBox.Show(result.Message, "Add New Gauge", MessageBoxButton.OK, messageImage);

            return newGauge;
        }

        #endregion

        #region DeleteGauge

        /// <summary>
        /// Deletes a Gauge from the Material
        /// </summary>
        /// <param name="material">The Material on which to delete the Gauge.</param>
        /// <param name="gauge">The Gauge to delete the Gauge.</param>
        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteGauge(Material material, Gauge gauge)
        {
            bool deleted = false;
            DBOperationResult result = material.DeleteGauge(gauge);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge", MessageBoxButton.OK, messageImage);

            return deleted;
        }
        #endregion

        #region DeleteFlatBedGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteFlatBedGaugeSize(MachineGauge gauge, RectangularGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeleteFlatBedGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddFlatBedGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="length">The length of the GaugeSize.</param>
        /// <param name="width">The width of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static RectangularGaugeSize AddFlatBedGaugeSize(MachineGauge gauge, double length, double width)
        {
            DBOperationResult result = gauge.AddFlatBedGaugeSize(length, width);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            RectangularGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as RectangularGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region DeleteRotaryGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteRotaryGaugeSize(MachineGauge gauge, RoundGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeleteRotaryGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddRotaryGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="diameter">The length of the GaugeSize.</param>
        /// <param name="length">The width of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static RoundGaugeSize AddRotaryGaugeSize(MachineGauge gauge, double diameter, double length)
        {
            DBOperationResult result = gauge.AddRotaryGaugeSize(diameter, length);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            RoundGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as RoundGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region DeleteShearGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteShearGaugeSize(MachineGauge gauge, RectangularGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeleteShearGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddShearGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="length">The length of the GaugeSize.</param>
        /// <param name="width">The width of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static RectangularGaugeSize AddShearGaugeSize(MachineGauge gauge, double length, double width)
        {
            DBOperationResult result = gauge.AddShearGaugeSize(length, width);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            RectangularGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as RectangularGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region DeleteRoundGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteRoundGaugeSize(MachineGauge gauge, RoundGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeleteRoundDuctGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddRoundGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="diameter">The length of the GaugeSize.</param>
        /// <param name="length">The width of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static RoundGaugeSize AddRoundGaugeSize(MachineGauge gauge, double diameter, double length)
        {
            DBOperationResult result = gauge.AddRoundDuctGaugeSize(diameter, length);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            RoundGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as RoundGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region DeleteElectricalContainmentGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeleteElectricalContainmentGaugeSize(ElectricalContainmentGauge gauge, ElectricalContainmentGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeleteElectricalContainmentGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddElectricalContainmentGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="width">The width of the GaugeSize.</param>
        /// <param name="depth">The depth of the GaugeSize.</param>
        /// <param name="length">The length of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static ElectricalContainmentGaugeSize AddElectricalContainmentGaugeSize(ElectricalContainmentGauge gauge, double width, double depth, double length)
        {
            DBOperationResult result = gauge.AddElectricalContainmentGaugeSize(width, depth, length);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            ElectricalContainmentGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as ElectricalContainmentGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region DeletePipeworkGaugeSize

        /// <summary>
        /// Deletes a GaugeSize from the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="gaugeSize">The GaugeSize to delete.</param>

        /// <returns>If successful true, otherwise false.</returns>
        public static bool DeletePipeworkGaugeSize(PipeworkGauge gauge, PipeworkGaugeSize gaugeSize)
        {
            bool deleted = false;
            DBOperationResult result = gauge.DeletePipeworkGaugeSize(gaugeSize);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Gauge Size", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddPipeworkGaugeSize

        /// <summary>
        /// Adds a GaugeSize to the Gauge
        /// </summary>
        /// <param name="gauge">The Gauge on which to delete the GaugeSize.</param>
        /// <param name="diameter">The length of the GaugeSize.</param>
        /// <param name="length">The width of the GaugeSize.</param>
        /// <returns>If successful the new GaugeSize, otherwise null.</returns>
        public static PipeworkGaugeSize AddPipeworkGaugeSize(PipeworkGauge gauge, double diameter, double length)
        {
            DBOperationResult result = gauge.AddPipeworkGaugeSize(diameter, length);
            MessageBoxImage messageImage = MessageBoxImage.Error;
            PipeworkGaugeSize gaugeSize = null;
            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                gaugeSize = result.ReturnObject as PipeworkGaugeSize;
            }

            MessageBox.Show(result.Message, "Add Gauge Size", MessageBoxButton.OK, messageImage);

            return gaugeSize;
        }

        #endregion

        #region AddNewPriceList

        /// <summary>
        /// Adds a new PriceList to the Fabrication Database
        /// </summary>
        /// <param name="supplier">The supplier group to add the price list to.</param>
        /// <param name="pricelistName">The name of the new price list to add.</param>
        /// <param name="tableType">The TableType of the new price list to add</param>
        /// <returns>The newly added PriceList</returns>
        public static PriceListBase AddPriceList(SupplierGroup supplier, string pricelistName, TableType tableType)
        {
            PriceListBase priceList = null;

            DBOperationResult result = supplier.AddPriceList(pricelistName, tableType);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                priceList = result.ReturnObject as PriceListBase;
            }

            MessageBox.Show(result.Message, "Add PriceList", MessageBoxButton.OK, messageImage);

            return priceList;
        }

        #endregion

        #region DeletePriceList

        /// <summary>
        /// Adds a new PriceList to the Fabrication Database
        /// </summary>
        /// <param name="supplier">The supplier group to add the price list to.</param>
        /// <param name="priceList">The PriceList to delete.</param>
        /// <returns>True if successful, otherwise.</returns>
        public static bool DeletePriceList(SupplierGroup supplier, PriceListBase priceList)
        {
            bool deleted = false;
            DBOperationResult result = supplier.DeletePriceList(priceList);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete PriceList", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewPriceListEntry

        /// <summary>
        /// Adds a new entry to the PriceList.
        /// </summary>
        /// <param name="priceList">The PriceList to add the entry to.</param>
        /// <param name="databaseId">The database Id to add.</param>
        /// <returns>The newly added ProductEntry</returns>
        public static ProductEntry AddPriceListEntry(PriceList priceList, string databaseId)
        {
            ProductEntry entry = null;
            DBOperationResult result = priceList.AddEntry(databaseId);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                entry = result.ReturnObject as ProductEntry;
            }

            MessageBox.Show(result.Message, "Add PriceList Entry", MessageBoxButton.OK, messageImage);

            return entry;
        }

        #endregion

        #region DeletePriceListEntry

        /// <summary>
        /// Deletes an entry from a PriceList.
        /// </summary>
        /// <param name="fabTable">The PriceList to delete the entry from.</param>
        /// <param name="prodEntry">The ProductEntry to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeletePriceListEntry(PriceList priceList, ProductEntry prodEntry)
        {
            bool deleted = false;
            DBOperationResult result = priceList.DeleteEntry(prodEntry);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete PriceList Entry", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewFabricationTimesTableEntry

        /// <summary>
        /// Adds a new entry to the FabricationTimesTable.
        /// </summary>
        /// <param name="fabTable">The FabricationTimesTable to add the entry to.</param>
        /// <param name="databaseId">The database Id to add.</param>
        /// <returns>The newly added ProductEntry</returns>
        public static ProductEntry AddNewFabricationTimesTableEntry(FabricationTimesTable fabTable, string databaseId)
        {
            ProductEntry entry = null;
            DBOperationResult result = fabTable.AddEntry(databaseId);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                entry = result.ReturnObject as ProductEntry;
            }

            MessageBox.Show(result.Message, "Add FabricationTimesTable Entry", MessageBoxButton.OK, messageImage);

            return entry;
        }

        #endregion

        #region DeleteFabricationTimesTableEntry

        /// <summary>
        /// Deletes an entry from a FabricationTimesTable.
        /// </summary>
        /// <param name="fabTable">The FabricationTimesTable to delete the entry from.</param>
        /// <param name="prodEntry">The ProductEntry to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteFabricationTimesTableEntry(FabricationTimesTable fabTable, ProductEntry prodEntry)
        {
            bool deleted = false;
            DBOperationResult result = fabTable.DeleteEntry(prodEntry);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete FabricationTimesTable Entry", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewFabricationTimesTable

        /// <summary>
        /// Adds a new Fabrication Times Table to the Fabrication Database
        /// </summary>
        /// <param name="name">The name of the Fabrication Times Table to add.</param>
        /// <param name="group">The group name to add the Fabrication Times Table to.</param>
        /// <param name="tableType">The TableType to add.</param>
        /// <returns>The newly added FabricationTimesTable</returns>
        public static FabricationTimesTableBase AddNewFabricationTimesTable(string name, string group, TableType tableType)
        {
            FabricationTimesTableBase fabricationTimesTable = null;

            DBOperationResult result = Database.AddFabricationTimesTable(name, group, tableType);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                fabricationTimesTable = result.ReturnObject as FabricationTimesTableBase;
            }

            MessageBox.Show(result.Message, "Add Fabrication Times Table", MessageBoxButton.OK, messageImage);

            return fabricationTimesTable;
        }

        #endregion

        #region DeleteFabricationTimesTable

        /// <summary>
        /// Deletes a Fabrication Times Table from the Fabrication Database
        /// </summary>
        /// <param name="table">The name of the Fabrication Times Table to add.</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool DeleteFabricationTimesTable(FabricationTimesTableBase table)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteFabricationTimesTable(table);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Fabrication Times Table", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewInstallationTimesTableEntry

        /// <summary>
        /// Adds a new entry to the InstallationTimesTable.
        /// </summary>
        /// <param name="table">The InstallationTimesTable to add the entry to.</param>
        /// <param name="databaseId">The database Id to add.</param>
        /// <returns>The newly added ProductEntry</returns>
        public static ProductEntry AddNewInstallationTimesTableEntry(InstallationTimesTable table, string databaseId)
        {
            ProductEntry entry = null;
            DBOperationResult result = table.AddEntry(databaseId);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                entry = result.ReturnObject as ProductEntry;
            }

            MessageBox.Show(result.Message, "Add InstallationTimesTable Entry", MessageBoxButton.OK, messageImage);

            return entry;
        }

        #endregion

        #region DeleteInstallationTimesTableEntry

        /// <summary>
        /// Deletes an entry from an InstallationTimesTable.
        /// </summary>
        /// <param name="installTable">The InstallationTimesTable to delete the entry from.</param>
        /// <param name="prodEntry">The ProductEntry to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteInstallationTimesTableEntry(InstallationTimesTable installTable, ProductEntry prodEntry)
        {
            bool deleted = false;
            DBOperationResult result = installTable.DeleteEntry(prodEntry);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete InstallationTimesTable Entry", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewInstallationTimesTable

        /// <summary>
        /// Adds a new Installation Times Table to the Fabrication Database
        /// </summary>
        /// <param name="name">The name of the Installation Times Table to add.</param>
        /// <param name="group">The group name to add the Installation Times Table to.</param>
        /// <returns>The newly added InstallationTimesTable</returns>
        public static InstallationTimesTableBase AddNewInstallationTimesTable(string name, string group, TableType tableType)
        {
            InstallationTimesTableBase installationTimesTable = null;

            DBOperationResult result = Database.AddInstallationTimesTable(name, group, tableType);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                installationTimesTable = result.ReturnObject as InstallationTimesTableBase;
            }

            MessageBox.Show(result.Message, "Add Installation Times Table", MessageBoxButton.OK, messageImage);

            return installationTimesTable;
        }

        #endregion

        #region DeleteInstallationTimesTable

        /// <summary>
        /// Deletes an Installation Times Table from the Fabrication Database
        /// </summary>
        /// <param name="table">The name of the Installation Times Table to add.</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool DeleteInstallationTimesTable(InstallationTimesTableBase table)
        {
            bool deleted = false;
            DBOperationResult result = Database.DeleteInstallationTimesTable(table);
            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Installation Times Table", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddBreakPointColumn

        /// <summary>
        /// Adds a new column to the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to add the column to.</param>
        /// <param name="value">The value of the HorizontalBreakPoint value to add.</param>
        /// <param name="sort">Pass true to sort the value numerically.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool AddBreakPointColumn(BreakPointTable table, double value, bool sort)
        {
            DBOperationResult result = table.AddColumn(value, sort);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region MoveBreakPointColumn

        /// <summary>
        /// Moves a column in the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to move the column in.</param>
        /// <param name="index">The index of the column to move.</param>
        /// <param name="moveBy">The number of positions to move the column by. A negative value will move the column left,
        /// whilst a positive value will move the column to the right.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool MoveBreakPointColumn(BreakPointTable table, int index, int moveBy)
        {
            DBOperationResult result = table.MoveColumn(index, moveBy);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region AddBreakPointRow

        /// <summary>
        /// Adds a new new to the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to add the row to.</param>
        /// <param name="value">The value of the VerticalBreakPoint value to add.</param>
        /// <param name="sort">Pass true to sort the value numerically.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool AddBreakPointRow(BreakPointTable table, double value, bool sort)
        {
            DBOperationResult result = table.AddRow(value, sort);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region MoveBreakPointRow

        /// <summary>
        /// Moves a row in the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to move the row in.</param>
        /// <param name="index">The index of the row to move.</param>
        /// <param name="moveBy">The number of positions to move the row by. A negative value will move the row up,
        /// whilst a positive value will move the row down.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool MoveBreakPointRow(BreakPointTable table, int index, int moveBy)
        {
            DBOperationResult result = table.MoveRow(index, moveBy);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region DeleteBreakPointColumn

        /// <summary>
        /// Deletes a column in the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to delete the column in.</param>
        /// <param name="index">The index of the column to delete.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool DeleteBreakPointColumn(BreakPointTable table, int index)
        {
            DBOperationResult result = table.DeleteColumn(index);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region DeleteBreakPointRow

        /// <summary>
        /// Deletes a row in the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable to delete the row in.</param>
        /// <param name="index">The index of the row to delete.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool DeleteBreakPointRow(BreakPointTable table, int index)
        {
            DBOperationResult result = table.DeleteRow(index);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region GetBreakPointValue

        /// <summary>
        /// Gets a value from a BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <param name="rowIndex">The index of the row.</param>
        /// <returns>The specified value from the BreakPointTable</returns>
        public static double GetBreakPointValue(BreakPointTable table, int columnIndex, int rowIndex)
        {
            DBOperationResult result = table.GetValue(columnIndex, rowIndex);
            double value = 0;
            if (result.Status == ResultStatus.Succeeded)
                value = (double)result.ReturnObject;

            return value;
        }

        #endregion

        #region SetBreakPointValue

        /// <summary>
        /// Sets a value in the BreakPointTable.
        /// </summary>
        /// <param name="table">The BreakPointTable.</param>
        /// <param name="columnIndex">The index of the column.</param>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="value">The new value to set.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool SetBreakPointValue(BreakPointTable table, int columnIndex, int rowIndex, double value)
        {
            DBOperationResult result = table.SetValue(columnIndex, rowIndex, value);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region SetHorizontalBreakPointValue

        /// <summary>
        /// Sets a value in the BreakPointTable's HorizontalBreakPoints collection.
        /// </summary>
        /// <param name="table">The BreakPointTable.</param>
        /// <param name="index">The index of the HorizontalBreakPoint to change.</param>
        /// <param name="value">The new value to set.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool SetHorizontalBreakPointValue(BreakPointTable table, int index, double value)
        {
            DBOperationResult result = table.SetHorizontalBreakPointValue(index, value);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region SetVerticalBreakPointValue

        /// <summary>
        /// Sets a value in the BreakPointTable's VerticalBreakPoints collection.
        /// </summary>
        /// <param name="table">The BreakPointTable.</param>
        /// <param name="index">The index of the VerticalBreakPoint to change.</param>
        /// <param name="value">The new value to set.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool SetVerticalBreakPointValue(BreakPointTable table, int index, double value)
        {
            DBOperationResult result = table.SetVerticalBreakPointValue(index, value);
            bool ok = false;
            if (result.Status == ResultStatus.Succeeded)
                ok = true;

            return ok;
        }

        #endregion

        #region AddDocumentLink

        /// <summary>
        /// Adds an ItemDocumentLink to a Fabrication Item.
        /// </summary>
        /// <param name="item">The Fabrication Item to add the link to.</param>
        /// <param name="linkName">The name of the link to add.</param>
        /// <param name="linkTarget">The target location for the document i.e. url or local file</param>
        /// <param name="linkParams">Additional parameters to add to the document link e.g. page number.</param>
        /// <returns>ItemOperationResult</returns>
        public static ItemDocumentLink AddItemDocumentLink(Item item, string linkName, string linkTarget, string linkParams)
        {
            ItemDocumentLink documentLink = null;

            ItemOperationResult result = item.AddDocumentLink(linkName, linkTarget, linkParams);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                documentLink = result.ReturnObject as ItemDocumentLink;
            }

            MessageBox.Show(result.Message, "Add Item Document Link", MessageBoxButton.OK, messageImage);

            return documentLink;
        }

        #endregion

        #region RemoveDocumentLink

        /// <summary>
        /// Removes an ItemDocumentLink from a Fabrication Item
        /// </summary>
        /// <param name="item">The Fabrication Item to remove the ItemDocumentLink from.</param>
        /// <param name="link">The ItemDocumentLink to remove.</param>
        public static bool RemoveItemDocumentLink(Item item, ItemDocumentLink link)
        {
            bool removed = false;
            ItemOperationResult result = item.RemoveDocumentLink(link);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                removed = true;
            }

            MessageBox.Show(result.Message, "Remove Item Document Link", MessageBoxButton.OK, messageImage);

            return removed;
        }

        #endregion

        #region AddItemFolder

        /// <summary>
        /// Adds a new item folder to the currently loaded Fabrication Configuration.
        /// </summary>
        /// <param name="name">The name of the folder to add.</param>
        /// <param name="parent">The parent folder to add the new folder to, passing null will create a root folder.</param>
        /// <returns>True or false</returns>
        public static bool AddItemFolder(string name, ItemFolder parent)
        {
            bool added = false;
            DBOperationResult result = Autodesk.Fabrication.Content.ItemFolders.AddItemFolder(name, parent);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                added = true;
            }

            MessageBox.Show(result.Message, "Add Item Folder", MessageBoxButton.OK, messageImage);

            return added;
        }

        #endregion

        #region RemoveItemFolder

        /// <summary>
        /// Removes an item folder from the currently loaded Fabrication Configuration.
        /// </summary>
        /// <param name="folder">The item folder to remove.</param>
        /// <returns>True or false</returns>
        public static bool RemoveItemFolder(ItemFolder folder)
        {
            bool removed = false;
            DBOperationResult result = Autodesk.Fabrication.Content.ItemFolders.RemoveItemFolder(folder);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                removed = true;
            }

            MessageBox.Show(result.Message, "Remove Item Folder", MessageBoxButton.OK, messageImage);

            return removed;
        }

        #endregion

        #region UpdateSKey

        /// <summary>
        /// Updates the Item's SKey value.
        /// </summary>
        /// <param name="item">The Item to update.</param>
        /// <param name="skey">The SKey value to apply.</param>
        /// <returns>True or False</returns>
        public static bool UpdateSKey(Item item, string skey)
        {
            bool updated = false;

            ItemOperationResult result = item.UpdateSKey(skey);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                updated = true;
            }

            MessageBox.Show(result.Message, "Update SKey", MessageBoxButton.OK, messageImage);

            return updated;
        }

        #endregion

        #region AddNewSupplierGroup

        /// <summary>
        /// Adds a new Supplier Group to the Fabrication Database.
        /// </summary>
        /// <param name="name">The name of the Supplier Group to add.</param>
        /// <returns>The new SupplierGroup</returns>
        public static SupplierGroup AddNewSupplierGroup(string name)
        {
            SupplierGroup supplierGroup = null;

            DBOperationResult result = Database.AddSupplierGroup(name);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                supplierGroup = result.ReturnObject as SupplierGroup;
            }

            MessageBox.Show(result.Message, "Add Supplier Group", MessageBoxButton.OK, messageImage);

            return supplierGroup;
        }

        #endregion

        #region DeleteSupplierGroup

        /// <summary>
        /// Deletes a SupplierGroup from the Fabrication Database.
        /// </summary>
        /// <param name="supplierGroup">The SupplierGroup to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteSupplierGroup(SupplierGroup supplierGroup)
        {
            bool deleted = false;

            DBOperationResult result = Database.DeleteSupplierGroup(supplierGroup);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Supplier Group", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewDiscount

        /// <summary>
        /// Adds a new Discount to the Supplier Group Discounts
        /// </summary>
        /// <param name="sgd">The Supplier Group Discounts to add the discount to.</param>
        /// <param name="code">The discount code.</param>
        /// <param name="value">The discount code.</param>
        /// <param name="description">The discount code.</param>
        /// <returns>The new SupplierGroup</returns>
        public static Discount AddNewDiscount(SupplierGroupDiscounts sgd, string code, double value, string description)
        {
            Discount discount = null;

            DBOperationResult result = sgd.AddDiscount(code, value, description);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                discount = result.ReturnObject as Discount;
            }

            MessageBox.Show(result.Message, "Add Discount", MessageBoxButton.OK, messageImage);

            return discount;
        }

        #endregion

        #region DeleteDiscount

        /// <summary>
        /// Deletes a Discount from the Supplier Group Discounts
        /// </summary>
        /// <param name="sgd">The SupplierGroupDiscounts frpm which to delete the Discount.</param>
        /// <param name="discount">The Discount to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteDiscount(SupplierGroupDiscounts sgd, Discount discount)
        {
            bool deleted = false;

            DBOperationResult result = sgd.DeleteDiscount(discount);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Discount", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region AddNewSection

        /// <summary>
        /// Adds a new Section to the Fabrication Database.
        /// </summary>
        /// <param name="description">The description of the Section to add.</param>
        /// <param name="group">The group of the Section to add.</param>
        /// <returns>The new Section</returns>
        public static Section AddNewSection(string description, string group)
        {
            Section section = null;

            DBOperationResult result = Database.AddSection(description, group);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                section = result.ReturnObject as Section;
            }

            MessageBox.Show(result.Message, "Add Section", MessageBoxButton.OK, messageImage);

            return section;
        }

        #endregion

        #region DeleteSection

        /// <summary>
        /// Deletes a Section from the Fabrication Database.
        /// </summary>
        /// <param name="section">The Section to delete.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public static bool DeleteSection(Section section)
        {
            bool deleted = false;

            DBOperationResult result = Database.DeleteSection(section);

            MessageBoxImage messageImage = MessageBoxImage.Error;

            if (result.Status == ResultStatus.Succeeded)
            {
                messageImage = MessageBoxImage.Information;
                deleted = true;
            }

            MessageBox.Show(result.Message, "Delete Section", MessageBoxButton.OK, messageImage);

            return deleted;
        }

        #endregion

        #region SaveJobAs

        /// <summary>
        /// Save a Fabrication Job to a new file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SaveJobAs()
        {
            bool saved = false;
            MessageBoxImage messageImage = MessageBoxImage.Error;

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog()
            {
                OverwritePrompt = true,
                AddExtension = true,
                DefaultExt = "ESJ",
                Filter = "ESJ|*.ESJ|MAJ|*.MAJ",
                CheckPathExists = true,
                Title = "Save Fabrication Job",
                FileName = "Copy of " + Path.GetFileNameWithoutExtension(Job.Info.FileName),
                RestoreDirectory = true
            };

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = sfd.FileName;
                DBOperationResult result = Job.SaveAs(fileName);

                if (result.Status == ResultStatus.Succeeded)
                {
                    messageImage = MessageBoxImage.Information;
                    saved = true;
                }

                MessageBox.Show(result.Message, "Add Supplier Group", MessageBoxButton.OK, messageImage);
            }


            return saved;
        }

        #endregion

        #region AddNewAncillary 

        /// <summary>
        /// Adds a new Ancillary to the Database
        /// </summary>
        /// <param name="description">The description of the ancillary.</param>
        /// <param name="group">The group of the ancillary.</param>
        /// <param name="ancillaryType">The ancillary typey.</param>
        /// <returns>The new Ancillary</returns>
        public static AncillaryBase AddNewAncillary(string description, string group, AncillaryTypeEnum ancillaryType)
        {
            AncillaryBase ancillary = null;
            var result = Database.AddAncillary(description, group, ancillaryType);


            if (result.Status == ResultStatus.Succeeded)
            {
                ancillary = result.ReturnObject as AncillaryBase;
            }

            return ancillary;
        }
        #endregion

        #region DeleteAncillary 

        /// <summary>
        /// Deletes an Ancillary from the Database
        /// </summary>
        /// <param name="ancillary">The Ancillary to delete.</param>
        /// <returns>The new Ancillary</returns>
        public static bool DeleteAncillary(AncillaryBase ancillary)
        {
            bool deleted = false;
            var result = Database.DeleteAncillary(ancillary);

            if (result.Status == ResultStatus.Succeeded)
            {
                deleted = true;
            }

            return deleted;
        }
        #endregion

        #region SaveAncillaries

        /// <summary>
        /// Saves the Ancillary Database.
        /// </summary>
        /// <returns>True if successful, false otherwise. </returns>
        public static bool SaveAncillaries()
        {
            var result = Database.SaveAncillaries();

            MessageBoxImage messageImage = MessageBoxImage.Error;
            if (result.Status == ResultStatus.Succeeded)
                messageImage = MessageBoxImage.Information;

            MessageBox.Show(result.Message, "Save Ancillaries", MessageBoxButton.OK, messageImage);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region MoveServiceButton

        /// <summary>
        /// Changes the order of a ServiceButton on it's parent ServiceTab.
        /// </summary>
        /// <param name="tab">The ServiceTab on which the ServiceButton exists.</param>
        /// <param name="button">The ServiceButton to move.</param>
        /// <param name="moveBy">Amount to move the ServiceButton by, +/-</param>
        /// <returns>True if successful</returns>
        public static bool MoveServiceButton(ServiceTab tab, ServiceButton button, int moveBy)
        {
            var result = tab.MoveServiceButton(button, moveBy);
            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region ConnectorBreakPointTable_AddRow

        /// <summary>
        /// Adds a row to the ConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The ConnectorBreakPointTable </param>
        /// <returns>The value in the table</returns>
        public static bool ConnectorBreakPointTable_AddRow(ConnectorBreakPointTable table)
        {
            var result = table.AddRow();
            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region ConnectorBreakPointTable_DeleteRow

        /// <summary>
        /// Deletes a row in the ConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The ConnectorBreakPointTable </param>
        /// <param name="table">The index of the row to delete </param>
        /// <returns>The value in the table</returns>
        public static bool ConnectorBreakPointTable_DeleteRow(ConnectorBreakPointTable table, int index)
        {
            var result = table.DeleteRow(0);
            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region ConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a ConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The ConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index. </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double ConnectorBreakPointTable_GetValue(ConnectorBreakPointTable table, int columnIndex, int rowIndex)
        {
            var result = table.GetValue(columnIndex, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region ConnectorBreakPointTable_SetValue

        /// <summary>
        /// Gets a value from a ConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The ConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index. </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool ConnectorBreakPointTable_SetValue(ConnectorBreakPointTable table, int columnIndex, int rowIndex, double value)
        {
            var result = table.SetValue(columnIndex, rowIndex, value);
            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RoundConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a RoundConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RoundConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RoundConnectorBreakPointTable_GetValue(RoundConnectorBreakPointTable table, RoundConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RoundConnectorBreakPointTable_SetValue

        /// <summary>
        /// Sets a value from a RoundConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RoundConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RoundConnectorBreakPointTable_SetValue(RoundConnectorBreakPointTable table, RoundConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RoundConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The RoundConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static RoundConnectorBreakPointTable.BreakPointValueType RoundConnectorBreakPointTable_GetColumnType(RoundConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region RoundHexEndConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a RoundHexEndConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RoundHexEndConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RoundHexEndConnectorBreakPointTable_GetValue(RoundHexEndConnectorBreakPointTable table, RoundHexEndConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RoundHexEndConnectorBreakPointTable_SetValue

        /// <summary>
        /// Sets a value from a RoundHexEndConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RoundHexEndConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RoundHexEndConnectorBreakPointTable_SetValue(RoundHexEndConnectorBreakPointTable table, RoundHexEndConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RoundHexEndConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The RoundHexEndConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static RoundHexEndConnectorBreakPointTable.BreakPointValueType RoundHexEndConnectorBreakPointTable_GetColumnType(RoundHexEndConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region RectangularConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a RectangularConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RectangularConnectorBreakPointTable_GetValue(RectangularConnectorBreakPointTable table, RectangularConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RectangularConnectorBreakPointTable_SetValue

        /// <summary>
        /// Sets a value from a RectangularConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RectangularConnectorBreakPointTable_SetValue(RectangularConnectorBreakPointTable table, RectangularConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RectangularConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The RectangularConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static RectangularConnectorBreakPointTable.BreakPointValueType RectangularConnectorBreakPointTable_GetColumnType(RectangularConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region RectangularStepDownConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a RectangularStepDownConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularStepDownConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RectangularStepDownConnectorBreakPointTable_GetValue(RectangularStepDownConnectorBreakPointTable table, RectangularStepDownConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RectangularStepDownConnectorBreakPointTable_SetValue

        /// <summary>
        /// Sets a value from a RectangularStepDownConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularStepDownConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RectangularStepDownConnectorBreakPointTable_SetValue(RectangularStepDownConnectorBreakPointTable table, RectangularStepDownConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RectangularStepDownConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The RectangularStepDownConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static RectangularStepDownConnectorBreakPointTable.BreakPointValueType RectangularStepDownConnectorBreakPointTable_GetColumnType(RectangularStepDownConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_GetValueByType

        /// <summary>
        /// Gets a value from a RectangularCouplingPlateConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RectangularCouplingPlateConnectorBreakPointTable_GetValue(RectangularCouplingPlateConnectorBreakPointTable table, RectangularCouplingPlateConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_GetValueByColumn

        /// <summary>
        /// Gets a value from a RectangularCouplingPlateConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RectangularCouplingPlateConnectorBreakPointTable_GetValue(RectangularCouplingPlateConnectorBreakPointTable table, int columnIndex, int rowIndex)
        {
            var result = table.GetValue(columnIndex, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_SetValueByType

        /// <summary>
        /// Sets a value from a RectangularCouplingPlateConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RectangularCouplingPlateConnectorBreakPointTable_SetValue(RectangularCouplingPlateConnectorBreakPointTable table, RectangularCouplingPlateConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_SetValueByColumn

        /// <summary>
        /// Sets a value from a RectangularCouplingPlateConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RectangularCouplingPlateConnectorBreakPointTable_SetValue(RectangularCouplingPlateConnectorBreakPointTable table, int columnIndex, int rowIndex, double value)
        {
            var result = table.SetValue(columnIndex, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_SetSidesValue

        /// <summary>
        /// Sets a value from a RectangularCouplingPlateConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The sides type </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RectangularCouplingPlateConnectorBreakPointTable_SetValue(RectangularCouplingPlateConnectorBreakPointTable table, int rowIndex, RectangularCouplingPlateConnectorBreakPointTable.SidesType value)
        {
            var result = table.SetValue(rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region RectangularCouplingPlateConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The RectangularCouplingPlateConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static RectangularCouplingPlateConnectorBreakPointTable.BreakPointValueType RectangularCouplingPlateConnectorBreakPointTable_GetColumnType(RectangularCouplingPlateConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region OvalConnectorBreakPointTable_GetValue

        /// <summary>
        /// Gets a value from a OvalConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The OvalConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <returns>The value in the table</returns>
        public static double RectangularConnectorBreakPointTable_GetValue(OvalConnectorBreakPointTable table, OvalConnectorBreakPointTable.BreakPointValueType type, int rowIndex)
        {
            var result = table.GetValue(type, rowIndex);

            return result.Status == ResultStatus.Succeeded
              ? (double)result.ReturnObject
              : 0;
        }

        #endregion

        #region OvalConnectorBreakPointTable_SetValue

        /// <summary>
        /// Sets a value from a OvalConnectorBreakPointTable.
        /// </summary>
        /// <param name="table">The OvalConnectorBreakPointTable </param>
        /// <param name="type">The break point value type </param>
        /// <param name="rowIndex">The row index </param>
        /// <param name="value">The row index </param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool OvalConnectorBreakPointTable_SetValue(OvalConnectorBreakPointTable table, OvalConnectorBreakPointTable.BreakPointValueType type, int rowIndex, double value)
        {
            var result = table.SetValue(type, rowIndex, value);

            return result.Status == ResultStatus.Succeeded;
        }

        #endregion

        #region OvalConnectorBreakPointTable_GetColumnType

        /// <summary>
        /// Gets the break point value type for the specified column.
        /// </summary>
        /// <param name="table">The OvalConnectorBreakPointTable </param>
        /// <param name="columnIndex">The column index </param>
        /// <returns>The column's break point value type</returns>
        public static OvalConnectorBreakPointTable.BreakPointValueType OvalConnectorBreakPointTable_GetColumnType(OvalConnectorBreakPointTable table, int columnIndex)
        {
            return table.GetColumnType(columnIndex);
        }

        #endregion

        #region ConnectorStepDown_SetAsLinerThickness

        /// <summary>
        /// Sets the step down value to be derived from the liner thickness.
        /// </summary>
        /// <param name="connector">The RectangularConnectorInfo </param>
        public static void ConnectorStepDown_SetAsLinerThickness(RectangularConnectorInfo connector)
        {
            connector.SetStepDownAsLinerThickness();
        }

        #endregion

        #region ConnectorStepDown_SetAsValue

        /// <summary>
        /// Sets the step down value to a specific value.
        /// </summary>
        /// <param name="connector">The RectangularConnectorInfo </param>
        /// <param name="value">The value to set </param>
        public static void ConnectorStepDown_SetAsLinerThickness(RectangularConnectorInfo connector, double value)
        {
            connector.SetStepDownAsValue(value);
        }

        #endregion

        private void ChangeTopExtension()
        {
            //Find first square bend in job
            Item itm = Job.Items.FirstOrDefault(x => x.SourceName == "Square Bend");

            if (itm != null)
            {
                ItemDimension dim = itm.Dimensions.FirstOrDefault(x => x.Name == "Top Extension") as ItemDimension;
                if (dim != null)
                {
                    dim.Value = 12;
                    itm.Update();
                }
            }
        }

        private void ChangeConnector()
        {
            //Find first square bend in job
            Item itm = Job.Items.FirstOrDefault(x => x.SourceName == "Square Bend");

            if (itm != null)
            {
                //Find new connector to apply to item from Fabrication database
                ConnectorInfo newConnector = Database.Connectors.FirstOrDefault(x => x.Name == "Ductmate 35");

                if (newConnector != null)
                {
                    //Set the new connector to each connector on th item
                    for (int i = 0; i < itm.Connectors.Count; i++)
                    {
                        if (itm.ChangeConnector(newConnector, i).Status == ResultStatus.Succeeded)
                            itm.Connectors[i].IsLocked = true;
                    }
                    itm.Update();
                }
            }
        }

        #region SetQuantity

        /// <summary>
        /// Adjusts the quantity of an item.
        /// </summary>
        /// <param name="itm">The item to change quantity of.</param>
        /// <param name="chanegBy">The amount to adjust the quantity by +/-.</param>
        /// <returns>True or false</returns>
        public void ChangeItemQty(Item itm, int changeBy)
        {
            // read the current quantity, alter it by 'changeBy' and attempt to set the new quantity
            ItemOperationResult res = itm.SetQuantity(itm.Quantity - changeBy);
            if (res.Status == ResultStatus.Failed)
            {
                // the change failed - display why to the user
                // note: the usual reason for failure reducing quantity is that the item has nested parts
                MessageBox.Show(res.Message, "Setting Item Qty Failed");
            }
        }

        #endregion
    }


    public class FabricationAPIEventSubscriber
    {
        #region ItemUpdateEvents

        public void SubscribeToItemUpdateEvents(Item item)
        {
            //Subscribe to Item Updating/Updated Events
            item.ItemUpdating += itm_ItemUpdating;
            item.ItemUpdated += itm_ItemUpdated;
        }

        public void UnSubscribeToItemUpdateEvents(Item item)
        {
            //UnSubscribe to Item Updating/Updated Events
            item.ItemUpdating -= itm_ItemUpdating;
            item.ItemUpdated -= itm_ItemUpdated;
        }

        void itm_ItemUpdating(object sender, Autodesk.Fabrication.Events.FabricationAPIItemUpdatingEventArgs e)
        {
            Item item = sender as Item;

            if (MessageBox.Show("Confirm to run Item Update", "Item Updating", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                e.Cancel = true;
            else
                MessageBox.Show("Item Properties before Update" + Environment.NewLine + "Item Weight: " + item.Weight.ToString("0.##") +
                  Environment.NewLine + "Item Area: " + item.Area.ToString("0.##"), "Item Updating", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        void itm_ItemUpdated(object sender, Autodesk.Fabrication.Events.FabricationAPIItemUpdatedEventArgs e)
        {
            Item item = sender as Item;

            if (e.Status == FabricationAPIEventStatus.Succeeded)
                MessageBox.Show("Item Properties after Update" + Environment.NewLine + "Item Weight: " + item.Weight.ToString("0.##") +
                Environment.NewLine + "Item Area: " + item.Area.ToString("0.##"), "Item Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Item Update failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region JobEvents

        public void SubscribeToJobEvents()
        {
            //Subscribe to Job Adding/Added Events
            Job.ItemAddingToJob += Job_ItemAddingToJob;
            Job.ItemAddedToJob += Job_ItemAddedToJob;
        }

        public void UnSubscribeToJobEvents()
        {
            //UnSubscribe to Job Adding/Added Events
            Job.ItemAddingToJob -= Job_ItemAddingToJob;
            Job.ItemAddedToJob -= Job_ItemAddedToJob;
        }

        private void Job_ItemAddingToJob(object sender, FabricationAPIItemAddingToJobEventArgs e)
        {
            if (MessageBox.Show($"Confirm to add item to Job: {e.Item.SourceDescription}", "Adding Item to Job Event Handler", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                e.Cancel = true;
        }

        private void Job_ItemAddedToJob(object sender, FabricationAPIItemAddedToJobEventArgs e)
        {
            if (e.Status == FabricationAPIEventStatus.Succeeded)
                MessageBox.Show($"Item added to job : {e.Item.SourceDescription}", "Added Item to Job Event Handler", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Item could not be added to the job", "Added Item to Job Event Handler", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}