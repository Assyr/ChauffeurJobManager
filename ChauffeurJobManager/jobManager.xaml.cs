using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        private int labelYMarginInitial = 35;
        private int labelYMarginCurrent = 35;
        private int labelYMarginOffset = 40;

        private int labelXMarginCurrent = 20;


        private List<Label> labelList = new List<Label>();

        MySQLManager jobManagerSQLManager = new MySQLManager();

        public string tableDatabaseName;


        public jobManager()
        {
            InitializeComponent();
        }

        public void populateJobManagerWindow()
        {
            string tableName = txtBlock_tableName.Text;

            Grid grid = jobManagerGrid;
            DataTable columnInfo = jobManagerSQLManager.getDatabaseTableInfo(tableDatabaseName, tableName);

            

            foreach (DataRow col in  columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                Console.WriteLine(columnName);
                string dataType = col[columnInfo.Columns["DataType"]].ToString();
                Console.WriteLine(dataType);


                Label lblColumnName = new Label();

                if (labelYMarginCurrent >= 195)
                {

                    lblColumnName.Margin = new Thickness(labelXMarginCurrent + 325, labelYMarginInitial += labelYMarginOffset, 0, 0);
                    findAndPlotColumnDataType(dataType, labelXMarginCurrent + 30, labelYMarginInitial, jobManagerGrid);

                }
                else
                {
                    lblColumnName.Margin = new Thickness(labelXMarginCurrent, labelYMarginCurrent += labelYMarginOffset, 0, 0);
                    findAndPlotColumnDataType(dataType, labelXMarginCurrent - 600, labelYMarginCurrent, jobManagerGrid);
                }
                lblColumnName.VerticalAlignment = VerticalAlignment.Top;

                lblColumnName.Content = columnName + ":";
                lblColumnName.RenderTransformOrigin = new Point(1.882, 0.635);

                grid.Children.Add(lblColumnName);
            }
        }

        private void findAndPlotColumnDataType(string columnDataType, int controlXMarginCurrent, int controlYMarginInitial, Grid gridFunc)
        {

            switch (columnDataType)
            {
                case "System.String":
                    Console.WriteLine("System.String detected");
                    TextBox tb = new TextBox();
                    tb.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    tb.VerticalAlignment = VerticalAlignment.Top;
                    tb.Width = 120;
                    tb.Height = 20;
                    tb.TextWrapping = TextWrapping.Wrap;
                    Panel.SetZIndex(tb, 4);
                    gridFunc.Children.Add(tb);
                    //Implement logic for handing string
                    break;
                case "System.Int32":
                    Console.WriteLine("System.Int32 detected");
                    //Implement logic for handling int32
                    break;
                case "System.Single":
                    Console.WriteLine("System.Single detected");
                    //Implement logic for handling Single
                    TextBox tbFloat = new TextBox();
                    tbFloat.PreviewTextInput += tbFloat_PreviewTextInput;
                    tbFloat.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    tbFloat.VerticalAlignment = VerticalAlignment.Top;
                    tbFloat.Width = 120;
                    tbFloat.Height = 20;
                    tbFloat.TextWrapping = TextWrapping.Wrap;
                    Panel.SetZIndex(tbFloat, 4);
                    gridFunc.Children.Add(tbFloat);
                    break;
                case "System.DateTime":
                    Console.WriteLine("System.DateTime detected");
                    DatePicker dp = new DatePicker();
                    dp.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    dp.VerticalAlignment = VerticalAlignment.Top;
                    dp.Width = 120;
                    dp.Height = 20;
                    Panel.SetZIndex(dp, 4);
                    gridFunc.Children.Add(dp);
                    break;
                case "System.TimeSpan":
                    Console.WriteLine("System.TimeSpan detected");
                    //Implement logic for handling TimeSpan
                    Xceed.Wpf.Toolkit.TimePicker tp = new Xceed.Wpf.Toolkit.TimePicker();
                    tp.Format = Xceed.Wpf.Toolkit.DateTimeFormat.ShortTime;
                    tp.ShowDropDownButton = false;
                    tp.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    tp.VerticalAlignment = VerticalAlignment.Top;
                    tp.Width = 120;
                    tp.Height = 20;
                    Panel.SetZIndex(tp, 4);
                    gridFunc.Children.Add(tp);
                    break;
            }
        }

        private void tbFloat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        public void findFullAddress()
        {
            string sURL;
            sURL = "https://api.getaddress.io/v2/uk/N19JY/IMS/?api-key="; //need to store API key server side and grab it.. can't be leaving it here. - API key has been removed for GitHub

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            WebProxy myProxy = new WebProxy("myproxy", 80);
            myProxy.BypassProxyOnLocal = true;

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
