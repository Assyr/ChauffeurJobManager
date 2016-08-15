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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChauffeurJobManager
{
    public class Address
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
    }
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

            string json = get_web_content(baseUrl + postcode + "?api_key=" + apiKey);

            //remove useless stuff
            json = json.Remove(json.Length - 33);
            json = json.Remove(0, 10);

            Console.WriteLine(json);

            var list = JsonConvert.DeserializeObject<List<Address>>(json);

            foreach (Address a in list)
            {
                Console.WriteLine(a.line_1 + " , " + a.line_2);
            }
        }

        public string get_web_content(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string output = reader.ReadToEnd();
            response.Close();

            return output;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            findFullAddress();
        }
    }
}
