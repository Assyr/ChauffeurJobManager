using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for nextWorkingDay.xaml
    /// </summary>
    public partial class nextWorkingDay : Window
    {
        MySQLManager nextWorkingDaySQLManager = new MySQLManager();
        public nextWorkingDay()
        {
            InitializeComponent();
        }
        public void populateDataGridByDate(string databaseName, string tableName, string columnName)
        {
            DataTable dataSet = nextWorkingDaySQLManager.getDataTableFilteredByDate(databaseName, tableName, DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), columnName);
            dataSet.Columns.RemoveAt(0);
            selectedTableDataGrid.ItemsSource = dataSet.DefaultView;
        }
    }
}
