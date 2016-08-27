using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for nextWorkingDay.xaml
    /// </summary>
    public partial class nextWorkingDay : Window
    {
        MySQLManager nextWorkingDaySQLManager = new MySQLManager();

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public nextWorkingDay()
        {
            InitializeComponent();
        }
        //Remove close button from window (user must use the show/hide button in the welcome screen.
        private void nextWorkingDayWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        public void populateDataGridByDate(string databaseName, string tableName, string columnName)
        {
            DataTable dataSet = nextWorkingDaySQLManager.getDataTableFilteredByDate(databaseName, tableName, DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), columnName);
            dataSet.Columns.RemoveAt(0);
            selectedTableDataGrid.ItemsSource = dataSet.DefaultView;
        }
    }
}
