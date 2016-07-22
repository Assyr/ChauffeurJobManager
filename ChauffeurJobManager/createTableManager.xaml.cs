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

        private List<ComboBox> comboBoxList = new List<ComboBox>();
        private List<TextBox> textBoxList = new List<TextBox>();

        private int comboBoxCurrentValue = 80;
        private int comboBoxMarginOffset = 40;

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
            Console.WriteLine(comboBoxList[0].Text);
            Console.WriteLine(textBoxList[0].Text);

            //Here we will make our new controls - textbox and combobox and then add them to array
            //so we can loop through them later and get the values.
        }

        private void createNewColumn()
        {
            Grid grid = tableManagerGrid;

            ComboBox dataTypeComboBox = new ComboBox();
            TextBox columnNameTextBox = new TextBox();

            dataTypeComboBox.Margin = new Thickness(125, comboBoxCurrentValue, 0, 0);
            columnNameTextBox.Margin = new Thickness(-145, comboBoxCurrentValue, 0, 0);

            dataTypeComboBox.VerticalAlignment = VerticalAlignment.Top;
            columnNameTextBox.VerticalAlignment = VerticalAlignment.Top;

            dataTypeComboBox.Width = 120;
            columnNameTextBox.Width = 120;
            columnNameTextBox.Height = 20;

            columnNameTextBox.TextWrapping = TextWrapping.Wrap;

            dataTypeComboBox.Items.Add("CHAR");
            dataTypeComboBox.Items.Add("VARCHAR");
            dataTypeComboBox.Items.Add("INT");
            dataTypeComboBox.Items.Add("FLOAT");
            dataTypeComboBox.Items.Add("DATE");
            dataTypeComboBox.Items.Add("DATE+TIME");
            dataTypeComboBox.Items.Add("TIME");
            dataTypeComboBox.Items.Add("YEAR");
            dataTypeComboBox.SelectedIndex = 0;



            grid.Children.Add(dataTypeComboBox);
            grid.Children.Add(columnNameTextBox);

            comboBoxList.Add(dataTypeComboBox);
            textBoxList.Add(columnNameTextBox);

            comboBoxCurrentValue += comboBoxMarginOffset;
        }
    }
}
