using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Input;
using System.IO;
using Microsoft.Office.Interop;

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

        private MySQLManager welcomeSQLManager = new MySQLManager();

        private nextWorkingDay nWD = new nextWorkingDay();
        private bool showNextWorkingDay = false;

        public welcome()
        {
            InitializeComponent();
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Welcome page loaded!");
            this.Closed += new EventHandler(welcomeWindowClosing);
        }

        void welcomeWindowClosing(object sender, EventArgs e)
        {
            nWD.Close();
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

        public void updateDataGrid()
        {
            if (listViewTables.SelectedItem != null)
            {
                DataTable dataSet = new DataTable();
                Console.WriteLine(databaseName);
                welcomeSQLManager.openConnection(databaseName);
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter("select * from " + databaseName + "." + listViewTables.SelectedItem.ToString() + ";", welcomeSQLManager.sqlConnect);
                dataAdapter.Fill(dataSet);
                welcomeSQLManager.closeConnection();
                dataSet.Columns.RemoveAt(0);
                selectedTableDataGrid.ItemsSource = dataSet.DefaultView;
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            updateDataGrid();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            
            login loginWindow = new login();
            loginWindow.Top = this.Top;
            loginWindow.Left = this.Left;
            loginWindow.Show();
            Close();
        }

        private void btn_CreateTable_Click(object sender, RoutedEventArgs e)
        {
            createTableManager tableManager = new createTableManager(this);
            tableManager.databaseName = databaseName;
            tableManager.Top = this.Top;
            tableManager.Left = this.Left;
            tableManager.ShowDialog();
        }

        private void btn_DeleteTable_Click(object sender, RoutedEventArgs e)
        {
            welcomeSQLManager.openConnection(databaseName);
            if (listViewTables.SelectedItem == null)
            {
                MessageBox.Show("Please select a table to delete from the list", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            welcomeSQLManager.sendQueryToDatabase("DROP TABLE IF EXISTS " + databaseName + "." + listViewTables.SelectedItem.ToString());
            welcomeSQLManager.closeConnection();
            selectedTableDataGrid.ItemsSource = null;
            updateTableList();
        }

        private void btn_openTable_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTables.SelectedItem == null)
            {
                MessageBox.Show("Please select a table to open from the list", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                jobManager jobManagerWindow = new jobManager();

                //Populate jobManagerWindow with what we need.
                jobManagerWindow.txtBlock_tableName.Text = listViewTables.SelectedItem.ToString();
                jobManagerWindow.tableDatabaseName = databaseName;
                jobManagerWindow.tableName = listViewTables.SelectedItem;
                jobManagerWindow.populateJobManagerWindow();
                jobManagerWindow.populateDataGrid();
                jobManagerWindow.ShowDialog();
                updateDataGrid();
        }

        private void btn_exportToCSV_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTables.SelectedItem == null)
            {
                MessageBox.Show("Please select a table to export a CSV from", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                exportToCSV CSVExport = new exportToCSV();
                CSVExport.dg = selectedTableDataGrid;
                CSVExport.lbl_Header.Content = "Please select the excel template you" + Environment.NewLine + "would like to export " + listViewTables.SelectedItem.ToString() + " table to.";
                CSVExport.ShowDialog();
        }

        private void btn_toggleNextWorkingDay_Click(object sender, RoutedEventArgs e)
        {
            if(!showNextWorkingDay)
            {
                btn_toggleNextWorkingDay.Content = "Hide Next " + Environment.NewLine + "Working Day";
                showNextWorkingDay = true;
                nWD.Show();
            }
            else
            {
                btn_toggleNextWorkingDay.Content = "Show Next " + Environment.NewLine + "Working Day";
                showNextWorkingDay = false;
                nWD.Hide();
            }
        }
    }
}