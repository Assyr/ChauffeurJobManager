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

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for postcodeLookup.xaml
    /// </summary>
    public partial class postcodeLookup : Window
    {
        //Struct that is used to hold information from the deserialize
        public struct Address
        {
            public string line_1 { get; set; }
            public string line_2 { get; set; }
            public string post_town { get; set; }
            public string postcode { get; set; }
        }

        public postcodeLookup()
        {
            InitializeComponent();
        }

        public void findFullAddress()
        {
            string apiKey = "iddqd"; //need to store my API key in database and retrieve.
            string postcode = txtBox_Postcode.Text;
            string baseUrl = "https://api.ideal-postcodes.co.uk/v1/postcodes/";
            string json = getAllTextFromURL(baseUrl + postcode + "?api_key=" + apiKey);

            //Remove useless stuff from json response.
            json = json.Remove(json.Length - 33);
            json = json.Remove(0, 10);

            List<Address> addressList = JsonConvert.DeserializeObject<List<Address>>(json);

            comboBox_addressList.Items.Clear();

            foreach (Address listLoopInst in addressList)
            {
                comboBox_addressList.Items.Add(listLoopInst.line_1 + ", " + listLoopInst.line_2 + ", " + listLoopInst.post_town + ", " + listLoopInst.postcode);
            }
        }

        //Grabs all text from URL page supplied and return
        public string getAllTextFromURL(string url)
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
