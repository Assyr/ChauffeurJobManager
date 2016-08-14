using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for postcodeLookup.xaml
    /// </summary>
    public partial class postcodeLookup : Window
    {
        public postcodeLookup()
        {
            InitializeComponent();
        }

        public void findFullAddress()
        {
            string apiKey = "iddqd";
            string postcode = txtBox_Postcode.Text;
            string baseUrl = "https://api.ideal-postcodes.co.uk/v1/postcodes/";

            WebRequest wrGetURL;

            wrGetURL = WebRequest.Create(baseUrl + postcode + "?api_key=" + apiKey);

            WebProxy proxy = new WebProxy("myproxy", 80);
            proxy.BypassProxyOnLocal = true;

            wrGetURL.Proxy = WebProxy.GetDefaultProxy();

            Stream objStream = wrGetURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";

            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
                Console.WriteLine(sLine);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            findFullAddress();
        }
    }
}
