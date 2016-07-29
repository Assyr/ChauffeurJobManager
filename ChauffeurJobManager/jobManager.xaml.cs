﻿using System;
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
    /// Interaction logic for jobManager.xaml
    /// </summary>
    public partial class jobManager : Window
    {

        private List<Label> labelList = new List<Label>();

        MySQLManager jobManagerSQLManager = new MySQLManager();

        public string tableDatabaseName;


        public jobManager()
        {
            InitializeComponent();
        }



        public void populateJobManagerWindow()
        {

            Grid grid = jobManagerGrid;

            string tableName = txtBlock_tableName.Text;
            //First read in our xml and populate with the correct columns
            DataTable columnInfo = jobManagerSQLManager.getDatabaseTableInfo(tableDatabaseName, tableName);

            foreach (DataRow col in  columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                Console.WriteLine(columnName);
                string dataType = col[columnInfo.Columns["DataType"]].ToString();
                Console.WriteLine(dataType);


                Label lblColumnName = new Label();
                lblColumnName.Margin = new Thickness(30, 80, 0, 0);
                lblColumnName.VerticalAlignment = VerticalAlignment.Top;

                lblColumnName.Content = columnName + ":";
                lblColumnName.RenderTransformOrigin = new Point(1.882, 0.635);

                grid.Children.Add(lblColumnName);

            }
        }
    }
}
