using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for welcome.xaml
    /// </summary>
    /// 
    public partial class welcome : Window
    {
        //private login loginWindow;
        public string databaseName;
        public IList<string> listOfDatabaseTables = new List<string>();

        MySQLManager welcomeSQLManager = new MySQLManager();

        public welcome()
        {
            InitializeComponent();
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Welcome page loaded!");
        }

        public void updateTableList()
        {
            listViewTables.Items.Clear();
            welcomeSQLManager.openConnection(databaseName);
            listOfDatabaseTables = welcomeSQLManager.getDatabaseTables();
            welcomeSQLManager.closeConnection();

            foreach (string tableName in listOfDatabaseTables)
            {
                listViewTables.Items.Add(tableName);
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            object item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                DataTable dataSet = new DataTable();
                Console.WriteLine(databaseName);
                welcomeSQLManager.openConnection(databaseName);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("select * from " + databaseName + "." + item.ToString() + ";", welcomeSQLManager.sqlConnect);
                dataAdapter.Fill(dataSet);
                welcomeSQLManager.closeConnection();
                selectedTableDataGrid.ItemsSource = dataSet.DefaultView;

            }
            updateTableList();
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            
            login loginWindow = new login();
            loginWindow.Top = this.Top;
            loginWindow.Left = this.Left;
            loginWindow.Show();
            Close();
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            createTableManager tableManager = new createTableManager();
            tableManager.databaseName = databaseName;
            tableManager.Top = this.Top;
            tableManager.Left = this.Left;
            tableManager.ShowDialog();
        }
    }
}
