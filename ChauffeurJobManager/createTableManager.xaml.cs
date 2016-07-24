﻿using System;
using System.Collections.Generic;
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

        private int comboBoxCurrentValue = 80;
        private int comboBoxMarginOffset = 40;
        private int columnNumber = 1;


        public createTableManager()
        {
            InitializeComponent();
            createNewColumn();
            btn_addNewColumn.Background = Brushes.LightGreen;

        }

        private void button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_addNewColumn_Click(object sender, RoutedEventArgs e)
        {
            createNewColumn();

            //Here we will make our new controls - textbox and combobox and then add them to array
            //so we can loop through them later and get the values.
        }

        private void createNewColumn()
        {
            Grid grid = tableManagerGrid;

            Label lblcolumnNumber = new Label();
            ComboBox dataTypeComboBox = new ComboBox();
            TextBox columnNameTextBox = new TextBox();


            dataTypeComboBox.Margin = new Thickness(125, comboBoxCurrentValue, 0, 0);
            columnNameTextBox.Margin = new Thickness(-145, comboBoxCurrentValue, 0, 0);
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
            dataTypeComboBox.Items.Add("VARCHAR");
            dataTypeComboBox.Items.Add("INT");
            dataTypeComboBox.Items.Add("FLOAT");
            dataTypeComboBox.Items.Add("DATE");
            dataTypeComboBox.Items.Add("DATE+TIME");
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
            string XMLFileName;

            if(string.IsNullOrWhiteSpace(templateFileName.Text))
            {
                MessageBox.Show("You must enter a valid template name", "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
                templateFileName.Focus();
                return;
            }
            else
            {
                XMLFileName = templateFileName.Text;
                MessageBox.Show(templateFileName.Text + " template has been created.");
                templateFileName.Clear();
            }


            foreach (TextBox tb in findControlsInCurrentWindow<TextBox>(this))
            {
                //Make sure that the textBox is not empty
                if (!string.IsNullOrWhiteSpace(tb.Text))
                {
                    //add the data to the lists
                    textBoxList.Add(tb);
                }
            }

            foreach (ComboBox cb in findControlsInCurrentWindow<ComboBox>(this))
            {

                //add the data to the lists
                comboBoxList.Add(cb);
            }

            using (var e1 = textBoxList.GetEnumerator())
            using (var e2 = comboBoxList.GetEnumerator())
            {

                XmlWriter xmlWriter = XmlWriter.Create(XMLFileName + ".xml");
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
        }

        public static IEnumerable<T> findControlsInCurrentWindow<T>(DependencyObject depObj) where T : DependencyObject
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

    }
}
