using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private int tableColumnCount;

        private List<Label> labelList = new List<Label>();
        private List<Control> controlTest = new List<Control>();

        MySQLManager jobManagerSQLManager = new MySQLManager();

        public string tableDatabaseName;
        public object tableName;
        
        public jobManager()
        {
            InitializeComponent();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }

        public void populateJobManagerWindow()
        {
            string tableName = txtBlock_tableName.Text;

            Grid grid = jobManagerGrid;
            DataTable columnInfo = jobManagerSQLManager.getTableStructureInfo(tableDatabaseName, tableName);
            foreach (DataRow col in columnInfo.Rows)
            {
                //Get how many columns are in our table and store it later for update implementation
                tableColumnCount++;

                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                string dataType = col[columnInfo.Columns["DataType"]].ToString();

                Label lblColumnName = new Label();

                if (labelYMarginCurrent >= 195)
                {
                    lblColumnName.Margin = new Thickness(labelXMarginCurrent + 325, labelYMarginInitial += labelYMarginOffset, 0, 0);
                    findAndPlotColumnDataType(dataType, labelXMarginCurrent + 30, labelYMarginInitial, jobManagerGrid, columnName);
                }
                else
                {
                    lblColumnName.Margin = new Thickness(labelXMarginCurrent, labelYMarginCurrent += labelYMarginOffset, 0, 0);
                    findAndPlotColumnDataType(dataType, labelXMarginCurrent - 600, labelYMarginCurrent, jobManagerGrid, columnName);
                }

                lblColumnName.VerticalAlignment = VerticalAlignment.Top;

                lblColumnName.Content = columnName + ":";
                lblColumnName.RenderTransformOrigin = new Point(1.882, 0.635);

                grid.Children.Add(lblColumnName);
            }
        }

        private void findAndPlotColumnDataType(string columnDataType, int controlXMarginCurrent, int controlYMarginInitial, Grid gridFunc, string columnName)
        {

            switch (columnDataType)
            {
                case "System.String":
                    //Implement logic for handling string
                    Console.WriteLine("System.String detected");
                    TextBox tb = new TextBox();
                    Button btn_postcodeLookup = new Button();
                    tb.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    tb.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    btn_postcodeLookup.Click += (s, e) => { postcodeLookup inst = new postcodeLookup(); inst.ShowDialog(); tb.Text = inst.addressToReturn; };
                    btn_postcodeLookup.Content = "...";
                    tb.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    btn_postcodeLookup.Margin = new Thickness(controlXMarginCurrent + 160, controlYMarginInitial, 0, 0);
                    tb.VerticalAlignment = VerticalAlignment.Top;
                    btn_postcodeLookup.VerticalAlignment = VerticalAlignment.Top;
                    tb.Width = 120;
                    tb.Height = 30;
                    btn_postcodeLookup.Width = 30;
                    btn_postcodeLookup.Height = 20;
                    tb.TextAlignment = TextAlignment.Left;
                    tb.TextWrapping = TextWrapping.Wrap;
                    Panel.SetZIndex(tb, 4);
                    Panel.SetZIndex(btn_postcodeLookup, 4);
                    gridFunc.Children.Add(tb);
                    gridFunc.Children.Add(btn_postcodeLookup);
                    controlTest.Add(tb);
                    //controlTest.Add(btn_postcodeLookup); -- causes crash. perhaps add to its own list
                    break;
                case "System.Int32":
                    Console.WriteLine("System.Int32 detected");
                    //Implement logic for handling int32
                    IntegerUpDown iud = new IntegerUpDown();

                    if (columnName == "jobID")
                        iud.IsEnabled = false;

                    iud.Margin = new Thickness(controlXMarginCurrent, controlYMarginInitial, 0, 0);
                    iud.VerticalAlignment = VerticalAlignment.Top;
                    iud.Width = 120;
                    iud.Height = 20;
                    Panel.SetZIndex(iud, 4);
                    gridFunc.Children.Add(iud);
                    controlTest.Add(iud);
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
                    controlTest.Add(dud);
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
                    controlTest.Add(dp);
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
                    controlTest.Add(tp);
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

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (checkIfControlsAreEmpty())
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("ONE OR MORE INPUT FIELDS ARE EMPTY");
                return;
            }

            string tableName = txtBlock_tableName.Text;
            string sqlColumnName = "insert into " + tableDatabaseName + "." + tableName + "(";
            string sqlColumnData = "values('";

            DataTable columnInfo = jobManagerSQLManager.getTableStructureInfo(tableDatabaseName, tableName);

            int ControlListindex = 1;

            foreach (DataRow col in columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                string dataType = col[columnInfo.Columns["DataType"]].ToString();

                if (columnName == "jobID")
                    continue;

                sqlColumnName += columnName + ",";

                //Grab it all and build a string to use latter to INSERT into table in one go
                switch (dataType)
                {
                    case "System.String":
                        TextBox tb = controlTest[ControlListindex] as TextBox;
                        Console.WriteLine(tb.Text);
                        sqlColumnData += tb.Text + "','";
                        ControlListindex++;
                        break;
                    case "System.Int32":

                        IntegerUpDown iud = controlTest[ControlListindex] as IntegerUpDown;
                        Console.WriteLine(iud.Text);
                        sqlColumnData += iud.Text + "','";
                        ControlListindex++;
                        break;
                    case "System.Single":
                        DecimalUpDown dud = controlTest[ControlListindex] as DecimalUpDown;
                        Console.WriteLine(dud.Text);
                        sqlColumnData += dud.Text.Remove(0, 1) + "','";
                        ControlListindex++;
                        break;
                    case "System.DateTime":
                        DatePicker dtp = controlTest[ControlListindex] as DatePicker;
                        DateTime? date = dtp.SelectedDate;
                        string value = date.Value.ToString("yyyy-MM-dd");
                        Console.WriteLine(value);
                        sqlColumnData += value + "','";
                        ControlListindex++;
                        break;
                    case "System.TimeSpan":
                        TimePicker tp = controlTest[ControlListindex] as TimePicker;
                        Console.WriteLine(tp.Text);
                        sqlColumnData += tp.Text + "','";
                        ControlListindex++;
                        break;
                }
            }

            sqlColumnName += ") ";
            sqlColumnData += ");";

            sqlColumnName = sqlColumnName.Remove(sqlColumnName.Length - 3, 1);
            sqlColumnData = sqlColumnData.Remove(sqlColumnData.Length - 4, 2);

            Console.WriteLine(sqlColumnName + sqlColumnData);

            jobManagerSQLManager.openConnection(tableDatabaseName);
            jobManagerSQLManager.sendQueryToDatabase(sqlColumnName + sqlColumnData);
            jobManagerSQLManager.closeConnection();

            populateDataGrid();

            clearAllControlFields();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (checkIfControlsAreEmpty())
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("ONE OR MORE INPUT FIELDS ARE EMPTY");
                return;
            }

            //Update record in table implementation
            string updateQuery = "update " + tableDatabaseName + "." + txtBlock_tableName.Text + " set ";

            int ControlListindex = 0;
            int jobIDValue = 0;

            DataTable columnInfo = jobManagerSQLManager.getTableStructureInfo(tableDatabaseName, txtBlock_tableName.Text);

            foreach (DataRow col in columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                string dataType = col[columnInfo.Columns["DataType"]].ToString();

                Console.WriteLine(columnName);
                Console.WriteLine(dataType);
                //Grab it all and build a string to use latter to INSERT into table in one go
                switch (dataType)
                {
                    case "System.String":
                            TextBox tb = controlTest[ControlListindex] as TextBox;
                            Console.WriteLine(tb.Text);
                            updateQuery += "'," + columnName + "='" + tb.Text;
                            ControlListindex++;
                            break;
                    case "System.Int32":
                        if (columnName == "jobID")
                        {
                            IntegerUpDown iud = controlTest[ControlListindex] as IntegerUpDown;
                            Console.WriteLine(iud.Text);
                            jobIDValue = (int)iud.Value;
                            updateQuery += columnName + "='" + iud.Text;
                            ControlListindex++;
                            break;
                        }
                        else
                        {
                            IntegerUpDown iud = controlTest[ControlListindex] as IntegerUpDown;
                            Console.WriteLine(iud.Text);
                            updateQuery += "'," + columnName + "='" + iud.Text;
                            ControlListindex++;
                            break;
                        }
                    case "System.Single":
                        DecimalUpDown dud = controlTest[ControlListindex] as DecimalUpDown;
                        Console.WriteLine(dud.Text);
                        updateQuery += "'," + columnName + "='" + dud.Text.Remove(0,1);
                        ControlListindex++;
                        break;
                    case "System.DateTime":
                        DatePicker dtp = controlTest[ControlListindex] as DatePicker;
                        DateTime? date = dtp.SelectedDate;
                        string value = date.Value.ToString("yyyy-MM-dd");
                        Console.WriteLine(value);
                        updateQuery += "'," + columnName + "='" + value;
                        ControlListindex++;
                        break;
                    case "System.TimeSpan":
                        TimePicker tp = controlTest[ControlListindex] as TimePicker;
                        Console.WriteLine(tp.Text);
                        updateQuery += "'," + columnName + "='" + tp.Text;
                        ControlListindex++;
                        break;
                }
            }

            updateQuery += "' where jobID='" + jobIDValue + "';";
            Console.WriteLine(updateQuery);

            jobManagerSQLManager.openConnection(tableDatabaseName);
            jobManagerSQLManager.sendQueryToDatabase(updateQuery);
            jobManagerSQLManager.closeConnection();

            populateDataGrid();

            clearAllControlFields();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if(checkIfControlsAreEmpty())
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("ONE OR MORE INPUT FIELDS ARE EMPTY");
                return;
            }

            //Delete record in table implementation
            string deleteQuery = "delete from " + tableDatabaseName + "." + txtBlock_tableName.Text + " where jobID='";

            int ControlListindex = 0;
            int jobIDValue = 0;

            DataTable columnInfo = jobManagerSQLManager.getTableStructureInfo(tableDatabaseName, txtBlock_tableName.Text);

            foreach (DataRow col in columnInfo.Rows)
            {
                string columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                string dataType = col[columnInfo.Columns["DataType"]].ToString();

                Console.WriteLine(columnName);
                Console.WriteLine(dataType);
                //Grab it all and build a delete string to delete from jobID
                switch (dataType)
                {
                    case "System.Int32":
                        if (columnName == "jobID")
                        {
                            IntegerUpDown iud = controlTest[ControlListindex] as IntegerUpDown;
                            Console.WriteLine(iud.Text);
                            jobIDValue = (int)iud.Value;
                            ControlListindex++;
                            break;
                        }
                        else
                            break;
                }
            }

            deleteQuery += jobIDValue + "';";

            jobManagerSQLManager.openConnection(tableDatabaseName);
            jobManagerSQLManager.sendQueryToDatabase(deleteQuery);
            jobManagerSQLManager.closeConnection();

            populateDataGrid();

            clearAllControlFields();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView drv = (DataRowView)SQLTableDataGrid.SelectedItem;

            for (int x = 0; x < tableColumnCount; x++)
            {
                Console.WriteLine(drv[x].ToString());

                if (controlTest[x] is TextBox)
                {
                    TextBox tb = controlTest[x] as TextBox;
                    tb.Text = drv[x].ToString();
                }
                else if (controlTest[x] is IntegerUpDown)
                {
                    IntegerUpDown iud = controlTest[x] as IntegerUpDown;
                    iud.Value = (int)drv[x];
                }
                else if (controlTest[x] is DecimalUpDown)
                {
                    DecimalUpDown dud = controlTest[x] as DecimalUpDown;
                    dud.Text = drv[x].ToString();
                }
                else if (controlTest[x] is DatePicker)
                {
                    DatePicker dp = controlTest[x] as DatePicker;
                    dp.Text = drv[x].ToString();
                }
                else if (controlTest[x] is TimePicker)
                {
                    TimePicker tp = controlTest[x] as TimePicker;
                    TimeSpan ts = TimeSpan.Parse(drv[x].ToString());
                    tp.Text = ts.ToString(@"hh\:mm");
                }
            }
        }

        private void btnClearFields_Click(object sender, RoutedEventArgs e)
        {
            clearAllControlFields();
        }

        private void clearAllControlFields()
        {

            for (int x = 0; x < controlTest.Count; x++)
            {
                if (controlTest[x] is TextBox)
                {
                    TextBox tb = controlTest[x] as TextBox;
                    tb.Clear();
                }
                else if (controlTest[x] is IntegerUpDown)
                {
                    IntegerUpDown iud = controlTest[x] as IntegerUpDown;
                    iud.Value = null;
                }
                else if (controlTest[x] is DecimalUpDown)
                {
                    DecimalUpDown dud = controlTest[x] as DecimalUpDown;
                    dud.Value = null;
                }
                else if (controlTest[x] is DatePicker)
                {
                    DatePicker dp = controlTest[x] as DatePicker;
                    dp.Text = null;
                }
                else if (controlTest[x] is TimePicker)
                {
                    TimePicker tp = controlTest[x] as TimePicker;
                    tp.Value = null;
                }
            }

        }

        private bool checkIfControlsAreEmpty()
        {
            for (int x = 1; x < controlTest.Count; x++)
            {
                if (controlTest[x] is TextBox)
                {
                    TextBox tb = controlTest[x] as TextBox;
                    if (tb.Text == string.Empty)
                    {
                        Console.WriteLine("Textbox empty");
                        return true;
                    }
                }
                else if (controlTest[x] is IntegerUpDown)
                {
                    IntegerUpDown iud = controlTest[x] as IntegerUpDown;
                    if (iud.Text == string.Empty)
                    {
                        Console.WriteLine("IntegerUpDown empty");
                        return true;
                    }
                }
                else if (controlTest[x] is DecimalUpDown)
                {
                    DecimalUpDown dud = controlTest[x] as DecimalUpDown;
                    if (dud.Text == string.Empty)
                    {
                        Console.WriteLine("DecimalUpDown empty");
                        return true;
                    }
                }
                else if (controlTest[x] is DatePicker)
                {
                    DatePicker dp = controlTest[x] as DatePicker;
                    if (dp.Text == string.Empty)
                    {
                        Console.WriteLine("DatePicker empty");
                        return true;
                    }
                }
                else if (controlTest[x] is TimePicker)
                {
                    TimePicker tp = controlTest[x] as TimePicker;
                    if (tp.Text == string.Empty)
                    {
                        Console.WriteLine("TimePicker empty");
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
