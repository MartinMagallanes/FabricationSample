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
   public partial class AddDiscountWindow : Window
   {
      #region Private Members

      private SupplierGroupDiscounts _sgd;

      #endregion

      #region Public Properties

      public Discount Discount { get; private set; } = null;

      #endregion

      #region ctor

      public AddDiscountWindow(SupplierGroupDiscounts sgd)
      {
         InitializeComponent();

         _sgd = sgd;
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

      private void btnAddDiscount_Click(object sender, RoutedEventArgs e)
      {
         // need to validate the values
         string errorMessage = string.Empty;
         double value = 0;
         double.TryParse(txtValue.Text, out value);

         Discount = FabricationAPIExamples.AddNewDiscount(_sgd, txtCode.Text, value, txtDesc.Text);
         if (Discount != null)
            Close();
      }

      #endregion

   }
}
