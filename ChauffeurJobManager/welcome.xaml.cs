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
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
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

        public void updateDataGrids()
        {
            if (listViewTables.SelectedItem != null)
            {
                DataTable dataSet = welcomeSQLManager.getDataTable(databaseName, listViewTables.SelectedItem.ToString());
                dataSet.Columns.RemoveAt(0);
                selectedTableDataGrid.ItemsSource = dataSet.DefaultView;

                if (showNextWorkingDay)
                {
                    DataTable dt = welcomeSQLManager.getTableStructureInfo(databaseName, listViewTables.SelectedItem.ToString());
                    foreach(DataRow col in dt.Rows)
                    {
                        string columnName = col[dt.Columns["ColumnName"]].ToString();
                        string dataType = col[dt.Columns["DataType"]].ToString();

                        if (dataType == "System.DateTime")
                        {
                            nWD.populateDataGridByDate(databaseName, listViewTables.SelectedItem.ToString(), columnName);
                        }
                    }
                }
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            updateDataGrids();
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
                MessageBoxResult messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Please select a table to delete from the list", "", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            MessageBoxResult messageBoxResultYESNO = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you would like to delete table " + listViewTables.SelectedItem.ToString() + "?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResultYESNO == MessageBoxResult.No)
                return;

            welcomeSQLManager.sendQueryToDatabase("DROP TABLE IF EXISTS " + databaseName + "." + listViewTables.SelectedItem.ToString());
            welcomeSQLManager.closeConnection();
            selectedTableDataGrid.ItemsSource = null;
            updateTableList();
        }

        private void btn_openTable_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTables.SelectedItem == null)
            {
                MessageBoxResult messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Please select a table to open from the list", "", MessageBoxButton.OK, MessageBoxImage.Question);
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
                updateDataGrids();
        }

        private void btn_exportToCSV_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTables.SelectedItem == null)
            {
                MessageBoxResult messageBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show("Please select a table to export a CSV from", "", MessageBoxButton.OK, MessageBoxImage.Question);
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
                updateDataGrids();
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