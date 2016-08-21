using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for exportToCSV.xaml
    /// </summary>
    public partial class exportToCSV : Window
    {
        public DataGrid dg = new DataGrid();

        public exportToCSV()
        {
            InitializeComponent();
            populateExcelFileList();
        }

        private void populateExcelFileList()
        {
            listView_ExcelList.Items.Clear();
            string templatesDirectory = AppDomain.CurrentDomain.BaseDirectory + "Excel";
            Console.WriteLine(templatesDirectory);
            DirectoryInfo dinfo = new DirectoryInfo(templatesDirectory);
            FileInfo[] info = dinfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo file in info)
            {
                Console.WriteLine(file.Name);
                listView_ExcelList.Items.Add(file.Name);
            }
        }

        private void btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.ExcludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();

            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object missingValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            Console.WriteLine(listView_ExcelList.SelectedItem.ToString());
            xlWorkBook = xlexcel.Workbooks.Open(System.Environment.CurrentDirectory + "\\Excel\\" + listView_ExcelList.SelectedItem.ToString());
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);         //index to paste at..
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[8, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
    }
}
