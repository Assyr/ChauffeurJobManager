using MySql.Data.MySqlClient;
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
using Xceed.Wpf.Toolkit;

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
        private List<TextBox> textBoxList = new List<TextBox>();
        private List<IntegerUpDown> integerUpDownList = new List<IntegerUpDown>();
        private List<DecimalUpDown> decimalUpDownList = new List<DecimalUpDown>();
        private List<DatePicker> datePickerList = new List<DatePicker>();
        private List<TimePicker> timePickerList = new List<TimePicker>();

        MySQLManager jobManagerSQLManager = new MySQLManager();

        public string tableDatabaseName;
        public object tableName;


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
                    textBoxList.Add(tb);
                    //Implement logic for handing string
                    break;
                case "System.Int32":
                    Console.WriteLine("System.Int32 detected");
                    //Implement logic for handling int32
                    IntegerUpDown iud = new IntegerUpDown();
                    iud.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    iud.VerticalAlignment = VerticalAlignment.Top;
                    iud.Width = 120;
                    iud.Height = 20;
                    Panel.SetZIndex(iud, 4);
                    gridFunc.Children.Add(iud);
                    integerUpDownList.Add(iud);
                    break;
                case "System.Single":
                    Console.WriteLine("System.Single detected");
                    //Implement logic for handling Single
                    DecimalUpDown dud = new DecimalUpDown();
                    dud.FormatString = "C2";
                    dud.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    dud.VerticalAlignment = VerticalAlignment.Top;
                    dud.Width = 120;
                    dud.Height = 20;
                    Panel.SetZIndex(dud, 4);
                    gridFunc.Children.Add(dud);
                    decimalUpDownList.Add(dud);
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
                    datePickerList.Add(dp);
                    break;
                case "System.TimeSpan":
                    Console.WriteLine("System.TimeSpan detected");
                    //Implement logic for handling TimeSpan
                    TimePicker tp = new TimePicker();
                    tp.Format = Xceed.Wpf.Toolkit.DateTimeFormat.ShortTime;
                    tp.ShowDropDownButton = false;
                    tp.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    tp.VerticalAlignment = VerticalAlignment.Top;
                    tp.Width = 120;
                    tp.Height = 20;
                    Panel.SetZIndex(tp, 4);
                    gridFunc.Children.Add(tp);
                    timePickerList.Add(tp);
                    break;
            }
        }

        public void populateDataGrid()
        {
            DataTable dataSet = new DataTable();
            Console.WriteLine(tableDatabaseName);
            jobManagerSQLManager.openConnection(tableDatabaseName);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter("select * from " + tableDatabaseName + "." + tableName.ToString() + ";", jobManagerSQLManager.sqlConnect);
            dataAdapter.Fill(dataSet);
            jobManagerSQLManager.closeConnection();
            SQLTableDataGrid.ItemsSource = dataSet.DefaultView;
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

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string tableName = txtBlock_tableName.Text;
            string sqlColumnName = "insert into " + tableDatabaseName + "." + tableName + "(";
            string sqlColumnData = "values('";

                //"insert into student.studentinfo(idStudentInfo,Name,Father_Name,Age,Semester) values('" + this.IdTextBox.Text + "','" + this.NameTextBox.Text + "','" + this.FnameTextBox.Text + "','" + this.AgeTextBox.Text + "','" + this.SemesterTextBox.Text + "');";

            DataTable columnInfo = jobManagerSQLManager.getDatabaseTableInfo(tableDatabaseName, tableName);

            int stringIndex = 0;
            int intIndex = 0;
            int singleIndex = 0;
            int dateTimeIndex = 0;
            int timeSpanIndex = 0;

            foreach (DataRow col in columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                string dataType = col[columnInfo.Columns["DataType"]].ToString();

                sqlColumnName += columnName + ",";

                //Grab it all and build a string to use latter to INSERT into table in one go
                switch (dataType)
                {
                    case "System.String":
                        //jobManagerSQLManager.sendQueryToDatabase("");
                        Console.WriteLine(textBoxList[stringIndex].Text);
                        sqlColumnData += textBoxList[stringIndex].Text + "','";
                        stringIndex++;
                        break;
                    case "System.Int32":
                        Console.WriteLine(integerUpDownList[intIndex].Text);
                        sqlColumnData += integerUpDownList[intIndex].Text + "','";
                        intIndex++;
                        break;
                    case "System.Single":
                        Console.WriteLine(decimalUpDownList[singleIndex].Text);
                        sqlColumnData += decimalUpDownList[singleIndex].Text.Remove(0,1) + "','";
                        singleIndex++;
                        break;
                    case "System.DateTime":
                        Console.WriteLine(datePickerList[dateTimeIndex].Text);
                        DateTime? date = datePickerList[dateTimeIndex].SelectedDate;
                        string value = date.Value.ToString("yyyy-MM-dd");
                        Console.WriteLine(value);
                        sqlColumnData += value + "','";
                        dateTimeIndex++;
                        break;
                    case "System.TimeSpan":
                        Console.WriteLine(timePickerList[timeSpanIndex].Text);
                        sqlColumnData += timePickerList[timeSpanIndex].Text + "','";
                        timeSpanIndex++;
                        break;
                }
            }

            sqlColumnName += ") ";
            sqlColumnData += ");";

            sqlColumnName = sqlColumnName.Remove(sqlColumnName.Length - 3 , 1);
            sqlColumnData = sqlColumnData.Remove(sqlColumnData.Length - 4, 2);

            Console.WriteLine(sqlColumnName + sqlColumnData);

            jobManagerSQLManager.openConnection(tableDatabaseName);
            jobManagerSQLManager.sendQueryToDatabase(sqlColumnName + sqlColumnData);
            jobManagerSQLManager.closeConnection();

            populateDataGrid();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Update record in table implementation
            Xceed.Wpf.Toolkit.MessageBox.Show("UPDATE");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //Delete record in table implementation
            Xceed.Wpf.Toolkit.MessageBox.Show("DELETE");
        }

        
    }
}
