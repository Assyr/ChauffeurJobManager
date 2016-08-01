using System;
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

                    lblColumnName.Margin = new Thickness(labelXMarginCurrent + 225, labelYMarginInitial += labelYMarginOffset, 0, 0);
                    findColumnDataType(dataType);

                }
                else
                {
                    lblColumnName.Margin = new Thickness(labelXMarginCurrent, labelYMarginCurrent += labelYMarginOffset, 0, 0);
                    findColumnDataType(dataType);
                }
                lblColumnName.VerticalAlignment = VerticalAlignment.Top;

                lblColumnName.Content = columnName + ":";
                lblColumnName.RenderTransformOrigin = new Point(1.882, 0.635);

                grid.Children.Add(lblColumnName);
            }
        }

        private object findColumnDataType(string columnDataType)
        {
            Label l = new Label();
            switch(columnDataType)
            {
                case "System.String":
                    Console.WriteLine("System.String detected");
                    //Implement logic for handing string
                    break;
                case "System.Int32":
                    Console.WriteLine("System.Int32 detected");
                    //Implement logic for handling int32
                    break;
                case "System.Single":
                    Console.WriteLine("System.Single detected");
                    //Implement logic for handling Single
                    break;
                case "System.DateTime":
                    Console.WriteLine("System.DateTime detected");
                    //Implement logic for handling DateTime
                    break;
                case "System.TimeSpan":
                    Console.WriteLine("System.TimeSpan detected");
                    //Implement logic for handling TimeSpan
                    break;

            }


            return l;
        }
    }
}
