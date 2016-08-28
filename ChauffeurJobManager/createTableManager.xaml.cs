﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for createTableManager.xaml
    /// </summary>
    public partial class createTableManager : Window
    { 
        private List<ComboBox> comboBoxList = new List<ComboBox>();
        private List<TextBox> textBoxList = new List<TextBox>();
        private List<Label> labelList = new List<Label>();
        private int comboBoxCurrentValue = 85;
        private int comboBoxMarginOffset = 40;
        private int columnNumber = 1;

        public string databaseName;

        //store here
        welcome _temp;
                                //handler to our welcome window
       public createTableManager(welcome temp)
        {
            InitializeComponent();
            _temp = temp;
            createNewColumn();
            btn_addNewColumn.Background = Brushes.LightGreen;
            populateTemplateFileList();

        }

        private void button_Click_Close(object sender, RoutedEventArgs e)
        {
            //update our list view with our newly created table
            Close();
        }

        private void btn_addNewColumn_Click(object sender, RoutedEventArgs e)
        {
            createNewColumn();
        }

        private void createNewColumn()
        {
            Grid grid = tableManagerGrid;

            Label lblcolumnNumber = new Label();
            ComboBox dataTypeComboBox = new ComboBox();
            TextBox columnNameTextBox = new TextBox();


            dataTypeComboBox.Margin = new Thickness(35, comboBoxCurrentValue, 0, 0);
            columnNameTextBox.Margin = new Thickness(-260, comboBoxCurrentValue, 0, 0);
            lblcolumnNumber.Margin = new Thickness(30, comboBoxCurrentValue - 3, 0, 0);

            dataTypeComboBox.VerticalAlignment = VerticalAlignment.Top;
            columnNameTextBox.VerticalAlignment = VerticalAlignment.Top;
            lblcolumnNumber.VerticalAlignment = VerticalAlignment.Top;


            dataTypeComboBox.Width = 120;
            columnNameTextBox.Width = 120;
            columnNameTextBox.Height = 20;

            columnNameTextBox.TextWrapping = TextWrapping.Wrap;

            lblcolumnNumber.Content = "Column " + columnNumber + ":";

            lblcolumnNumber.RenderTransformOrigin = new Point(1.882, 0.635);

            dataTypeComboBox.Items.Add("CHAR");
            dataTypeComboBox.Items.Add("nvarchar(100)");
            dataTypeComboBox.Items.Add("INT");
            dataTypeComboBox.Items.Add("FLOAT");
            dataTypeComboBox.Items.Add("DATE");
            dataTypeComboBox.Items.Add("DATETIME");
            dataTypeComboBox.Items.Add("TIME");
            dataTypeComboBox.Items.Add("YEAR");
            dataTypeComboBox.SelectedIndex = 0;

            grid.Children.Add(lblcolumnNumber);
            grid.Children.Add(dataTypeComboBox);
            grid.Children.Add(columnNameTextBox);

            columnNumber++;
            comboBoxCurrentValue += comboBoxMarginOffset;
        }
        private void btn_SaveTableTemplate_Click(object sender, RoutedEventArgs e)
        {
            string XMLFileName = new TextRange(templateFileName.Document.ContentStart, templateFileName.Document.ContentEnd).Text;

            foreach (TextBox tb in findControlsInCurrentWindow<TextBox>(this))
            {
                //Make sure that the textBox is not empty
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("All columns must be given a valid name!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else //If any column table names are empty, spit out a message and return! - Fixes #1
                {
                    //add the data to the lists
                    Console.WriteLine(tb.Text);
                    textBoxList.Add(tb);
                }
            }

            if (string.IsNullOrWhiteSpace(XMLFileName))
            { 
                Xceed.Wpf.Toolkit.MessageBox.Show("You must enter a valid template name", "", MessageBoxButton.OK, MessageBoxImage.Information);
                templateFileName.Focus();
                return;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(XMLFileName + " template has been created.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                templateFileName.Document.Blocks.Clear();
            }


            foreach (ComboBox cb in findControlsInCurrentWindow<ComboBox>(this))
            {
                //add the data to the lists
                comboBoxList.Add(cb);
            }

            //Remove whitespace from richtextbox
            XMLFileName = XMLFileName.Trim();
            Console.WriteLine(XMLFileName);

            using (var e1 = textBoxList.GetEnumerator())
            using (var e2 = comboBoxList.GetEnumerator())
            {
                XmlWriter xmlWriter = XmlWriter.Create("Templates\\" + XMLFileName + ".xml");
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Column");

                while (e1.MoveNext() && e2.MoveNext())
                {
                    var item1 = e1.Current;
                    var item2 = e2.Current;
                    xmlWriter.WriteStartElement("columnData");
                    xmlWriter.WriteElementString("columnName", item1.Text);
                    xmlWriter.WriteElementString("columnDataType", item2.Text);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }

            populateTemplateFileList();
        }

        private static IEnumerable<T> findControlsInCurrentWindow<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in findControlsInCurrentWindow<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void populateTemplateFileList()
        {
            listView_TemplateList.Items.Clear();
            string templatesDirectory = System.AppDomain.CurrentDomain.BaseDirectory + "Templates";
            Console.WriteLine(templatesDirectory);
            DirectoryInfo dinfo = new DirectoryInfo(templatesDirectory);
            FileInfo[] info = dinfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo file in info)
            {
                Console.WriteLine(file.Name);
                listView_TemplateList.Items.Add(file.Name);
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        private void btn_CreateTableInDatabase_Click(object sender, RoutedEventArgs e)
        {
            if(listView_TemplateList.SelectedItem == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select a template to create a table from!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string templateFileName = listView_TemplateList.SelectedItem.ToString();
            string tableName = new TextRange(databaseTableName.Document.ContentStart, databaseTableName.Document.ContentEnd).Text;
            tableName = tableName.Trim();
            Console.WriteLine(tableName);

            XmlDocument file = new XmlDocument();
            file.Load("Templates/" + templateFileName);

            XmlElement element = file.DocumentElement;
            XmlNodeList columnNodes = element.SelectNodes("/Column/columnData");

            MySQLManager sql = new MySQLManager() ;

            Console.WriteLine("databaseName: " + databaseName); 
            sql.openConnection(databaseName);

            try
            {
                sql.sendQueryToDatabase("CREATE TABLE IF NOT EXISTS " + tableName + " (" + "`jobID` INT AUTO_INCREMENT," + "PRIMARY KEY(jobID));");
                foreach (XmlNode node in columnNodes)
                {
                    string columnName = node["columnName"].InnerText;
                    string columnDataType = node["columnDataType"].InnerText;
                    Console.WriteLine("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + columnDataType);
                    sql.sendQueryToDatabase("ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + columnDataType);
                }
                sql.closeConnection();
            }
            catch (MySqlException ex)
            {
                sql.sendQueryToDatabase("DROP TABLE IF EXISTS " + tableName);
                Xceed.Wpf.Toolkit.MessageBox.Show("Table couldn't be created in database - please check to make sure that your column names are not MySQL syntax. ERROR: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Question);
                //Can add more sophisticated checks later against a dictionary of MySQL syntax to let the user know specifically which column name is invalid.
            }

            databaseTableName.Document.Blocks.Clear();
            Console.WriteLine("Table has been created in " + databaseName);
            _temp.updateTableList(); //Update our listview table in our welcome window

            this.Close();
        }

        private void templateFileName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void databaseTableName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
