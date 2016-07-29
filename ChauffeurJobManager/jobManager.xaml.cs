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

        MySQLManager jobManagerSQLManager = new MySQLManager();

        public string tableDatabaseName;


        public jobManager()
        {
            InitializeComponent();
        }



        public void populateJobManagerWindow()
        {
            string tableName = txtBlock_tableName.Text;
            //First read in our xml and populate with the correct columns
            DataTable columnInfo = jobManagerSQLManager.getDatabaseTableInfo(tableDatabaseName, tableName);

            foreach (DataRow col in  columnInfo.Rows)
            {
                String columnName = col[columnInfo.Columns["ColumnName"]].ToString();
                Console.WriteLine(columnName);
                String dataType = col[columnInfo.Columns["DataType"]].ToString();
                Console.WriteLine(dataType);

            }

        }
    }
}
