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
            foreach (TextBox tb in findControlsInCurrentWindow<TextBox>(this))
            {
                foreach (ComboBox cb in findControlsInCurrentWindow<ComboBox>(this))
                {
                    //Make sure that the textBox is not empty
                    if (!string.IsNullOrWhiteSpace(tb.Text))
                    {
                        textBoxList.Add(tb);
                        comboBoxList.Add(cb);
                    }
                }
            }
            /*foreach (TextBox item in textBoxList)
            {
                foreach (ComboBox comboItem in comboBoxList)
                {


                    Console.WriteLine("Outputting all list items - textBox Value: " + item.Text + " - comboBox Value: " + comboItem.Text);
                }
            }*/

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
