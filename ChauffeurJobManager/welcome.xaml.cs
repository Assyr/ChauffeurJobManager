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

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for welcome.xaml
    /// </summary>
    /// 
    public partial class welcome : Window
    {
       // private login loginWindow;
        private MySQLManager SQLManager = new MySQLManager();
        public string databaseName;
        public IList<string> listOfDatabaseTables = new List<string>();

       
        

        public welcome()
        {
            InitializeComponent();
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Welcome page loaded!");
           
        }

        public void updateTableList()
        {
            foreach(string tableName in listOfDatabaseTables)
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
                SQLManager.openConnection(databaseName);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("select * from " + databaseName + "." + item.ToString() + ";", SQLManager.sqlConnect);
                dataAdapter.Fill(dataSet);
                SQLManager.closeConnection();
                selectedTableDataGrid.ItemsSource = dataSet.DefaultView;

            }
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
            tableManager.Top = this.Top;
            tableManager.Left = this.Left;
            tableManager.Show();
            Close();
        }
    }
}
