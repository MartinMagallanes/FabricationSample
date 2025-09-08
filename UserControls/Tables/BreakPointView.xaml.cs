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
using Autodesk.Fabrication.DB;
using FabricationSample.Data;
using FabricationSample.Manager;
using FabricationSample.FunctionExamples;
using System.Data;
using System.Windows.Controls.Primitives;

namespace FabricationSample.UserControls
{

    /// <summary>
    /// Interaction logic for BreakPointView.xaml
    /// </summary>
    public partial class BreakPointView : UserControl
    {

        #region Private Members
        private BreakPointTable _bpTable;
        private ObservableCollection<BreakPointMappingValueRow> _tableValues;
        #endregion

        #region ctor

        public BreakPointView(BreakPointTable bpTable)
        {
            _bpTable = bpTable;
            InitializeComponent();
            dgPrices.DataContext = _bpTable;
            FabricationManager.BreakPointPriceListView = this;
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bpTable != null)
            {
                LoadRowsAndColumns();
            }

        }

        public void LoadRowsAndColumns()
        {
            _tableValues = new ObservableCollection<BreakPointMappingValueRow>();

            foreach (BreakPointTableRow row in _bpTable.Rows)
            {
                BreakPointMappingValueRow mappingRow = new BreakPointMappingValueRow();

                foreach (BreakPointTableValue val in row.Values)
                {
                    mappingRow.Values.Add(new BreakPointMappingValue() { Value = val.Value });
                }
                _tableValues.Add(mappingRow);
            }

            dgPrices.DataContext = _bpTable;
            dgPrices.ItemsSource = _tableValues;
            dgPrices.Columns.Clear();
            for (int i = 0; i < _bpTable.HorizontalBreakPoints.Count; i++)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                TextBlock textBlock = new TextBlock { Text = _bpTable.HorizontalBreakPoints[i].ToString() };
                textColumn.Header = textBlock;
                Binding binding = new Binding("Values[" + i + "].Value");
                binding.Mode = BindingMode.TwoWay;
                textColumn.Binding = binding;
                dgPrices.Columns.Add(textColumn);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void dgPrices_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.GetIndex() < _bpTable.VerticalBreakPoints.Count)
            {
                e.Row.Header = _bpTable.VerticalBreakPoints[e.Row.GetIndex()].ToString();
            }

        }

        private void dgPrices_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox tb = e.EditingElement as TextBox;

            if ((tb != null) && tb.Text != "")
            {
                int col = e.Column.DisplayIndex;
                int row = e.Row.GetIndex();
                double value = Convert.ToDouble(tb.Text);

                if (value != FabricationAPIExamples.GetBreakPointValue(_bpTable, col, row))
                    FabricationAPIExamples.SetBreakPointValue(_bpTable, col, row, value);
            }
        }

        private void addRow_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationAPIExamples.AddBreakPointRow(_bpTable, 0.0, false))
                LoadRowsAndColumns();
        }

        private void addColumn_Click(object sender, RoutedEventArgs e)
        {
            if (FabricationAPIExamples.AddBreakPointColumn(_bpTable, 0.0, false))
                LoadRowsAndColumns();
        }

        private void insertColumn_Click(object sender, RoutedEventArgs e)
        {
            int index = GetColumnIndex(sender as MenuItem);
            if (FabricationAPIExamples.AddBreakPointColumn(_bpTable, 0.0, false))
            {
                int lastIndex = _bpTable.HorizontalBreakPoints.Count - 1;
                int moveBy = index - lastIndex;
                if (moveBy != 0)
                    FabricationAPIExamples.MoveBreakPointColumn(_bpTable, lastIndex, moveBy);

                LoadRowsAndColumns();
            }
        }

        private void insertRow_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPrices.SelectedIndex;
            if (FabricationAPIExamples.AddBreakPointRow(_bpTable, 0.0, false))
            {
                int lastIndex = _bpTable.VerticalBreakPoints.Count - 1;
                int moveBy = index - lastIndex;
                if (moveBy != 0)
                    FabricationAPIExamples.MoveBreakPointRow(_bpTable, lastIndex, moveBy);

                LoadRowsAndColumns();
            }
        }

        private void editRowValue_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPrices.SelectedIndex;

            double oldValue = _bpTable.VerticalBreakPoints[index];

            EditValueWindow win = new EditValueWindow(oldValue);
            win.ShowDialog();

            double newValue = win.Value;
            if (oldValue != newValue)
            {
                if (FabricationAPIExamples.SetVerticalBreakPointValue(_bpTable, index, newValue))
                    LoadRowsAndColumns();
            }
        }

        private void editColumnValue_Click(object sender, RoutedEventArgs e)
        {
            int index = GetColumnIndex(sender as MenuItem);
            if (index == -1)
                return;

            double oldValue = _bpTable.HorizontalBreakPoints[index];

            EditValueWindow win = new EditValueWindow(oldValue);
            win.ShowDialog();

            double newValue = win.Value;
            if (oldValue != newValue)
            {
                if (FabricationAPIExamples.SetHorizontalBreakPointValue(_bpTable, index, newValue))
                    LoadRowsAndColumns();
            }
        }

        private void shiftColumnLeft_Click(object sender, RoutedEventArgs e)
        {
            int index = GetColumnIndex(sender as MenuItem);
            if (index == 0)
                return;

            if (FabricationAPIExamples.MoveBreakPointColumn(_bpTable, index, -1))
                LoadRowsAndColumns();
        }

        private void shiftColumnRight_Click(object sender, RoutedEventArgs e)
        {
            int index = GetColumnIndex(sender as MenuItem);
            if (index == (_bpTable.HorizontalBreakPoints.Count - 1))
                return;

            if (FabricationAPIExamples.MoveBreakPointColumn(_bpTable, index, 1))
                LoadRowsAndColumns();
        }

        private void shiftRowUp_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPrices.SelectedIndex;
            if (index == 0)
                return;

            if (FabricationAPIExamples.MoveBreakPointRow(_bpTable, index, -1))
                LoadRowsAndColumns();
        }

        private void shiftRowDown_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPrices.SelectedIndex;
            if (index == (_bpTable.VerticalBreakPoints.Count - 1))
                return;

            if (FabricationAPIExamples.MoveBreakPointRow(_bpTable, index, 1))
                LoadRowsAndColumns();
        }

        private void deleteColumn_Click(object sender, RoutedEventArgs e)
        {
            int index = GetColumnIndex(sender as MenuItem);
            if (index < 0)
                return;

            _bpTable.DeleteColumn(index);
            LoadRowsAndColumns();
        }

        private void deleteRow_Click(object sender, RoutedEventArgs e)
        {
            int index = dgPrices.SelectedIndex;
            _bpTable.DeleteRow(index);
            LoadRowsAndColumns();
        }

        private int GetColumnIndex(MenuItem menuItem)
        {
            if (menuItem == null)
                return -1;

            var menu = menuItem.Parent as ContextMenu;
            if (menu == null || menu.PlacementTarget == null)
                return -1;

            var header = menu.PlacementTarget as DataGridColumnHeader;
            if (header == null)
                return -1;

            return header.DisplayIndex;
        }

    }


}

