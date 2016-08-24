﻿using System;
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
        private int labelYMarginInitial = 35;
        private int labelYMarginCurrent = 35;
        private int labelYMarginOffset = 40;
        private int labelXMarginCurrent = 20;

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

        public void populateNextWorkingDay()
        {
            List<string> rowStrings = new List<string>();
            foreach (var lvi in listViewTables.Items)
            {
                DataTable dt = welcomeSQLManager.getDatabaseTableInfo(databaseName, lvi.ToString());

                foreach (DataRow col in dt.Rows)
                {
                    string columnName = col[dt.Columns["ColumnName"]].ToString();
                    string dataType = col[dt.Columns["DataType"]].ToString();

                    if(dataType == "System.DateTime")
                    {
                        rowStrings.Add(welcomeSQLManager.getDatabaseTableInfoByDate(databaseName, lvi.ToString(), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), columnName));
                    }
                }
            }

            foreach (var rowStringComplete in rowStrings)
            {
                txtBlock_tomorrowJobs.Text += rowStringComplete + Environment.NewLine;
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
    }
}