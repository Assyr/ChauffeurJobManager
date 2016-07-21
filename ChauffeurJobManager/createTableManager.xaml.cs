using System;
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

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for createTableManager.xaml
    /// </summary>
    public partial class createTableManager : Window
    {
        public createTableManager()
        {
            InitializeComponent();
            btn_addNewColumn.Background = Brushes.LightGreen;
            dataTypeComboBox.Items.Add("CHAR");
            dataTypeComboBox.Items.Add("VARCHAR");
            dataTypeComboBox.Items.Add("INT");
            dataTypeComboBox.Items.Add("FLOAT");
            dataTypeComboBox.Items.Add("DATE");
            dataTypeComboBox.Items.Add("DATE+TIME");
            dataTypeComboBox.Items.Add("TIME");
            dataTypeComboBox.Items.Add("YEAR");

            //store object positions
            Point comboBoxLocationOnForm = dataTypeComboBox.TranslatePoint(new Point(0, 0), this);
            Point labelPosition = lbl_columnNumber.TranslatePoint(new Point(0, 0), this);

            Console.WriteLine(comboBoxLocationOnForm);
            Console.WriteLine(labelPosition);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_addNewColumn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
