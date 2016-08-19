using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Newtonsoft.Json;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for postcodeLookup.xaml
    /// </summary>
    public partial class postcodeLookup : Window
    {

        public string addressToReturn = "test";

        //Struct that is used to hold information from the deserialize
        public struct Address
        {
            public string line_1 { get; set; }
            public string line_2 { get; set; }
            public string postcode { get; set; }
        }

        public postcodeLookup()
        {
            InitializeComponent();
        }

        public void findFullAddress(string pCode)
        {
            string apiKey = "iddqd"; //need to store my API key in database and retrieve.
            string baseUrl = "https://api.ideal-postcodes.co.uk/v1/postcodes/";
            string json = getAllTextFromURL(baseUrl + pCode + "?api_key=" + apiKey);

            //Remove useless stuff from json response.
            json = json.Remove(json.Length - 33);
            json = json.Remove(0, 10);

            List<Address> addressList = JsonConvert.DeserializeObject<List<Address>>(json);

            comboBox_addressList.Items.Clear();

            foreach (Address listLoopInst in addressList)
            {
                comboBox_addressList.Items.Add(listLoopInst.line_1 + ", " + listLoopInst.line_2 +  ", " + listLoopInst.postcode);
            }
            comboBox_addressList.SelectedIndex = 0;
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

        private void btn_getAddress_Click(object sender, RoutedEventArgs e)
        {
            string postCodeText = txtBox_Postcode.Text;
            postCodeText = postCodeText.Trim();
            postCodeText = postCodeText.Replace(" ", "");
            postCodeText = postCodeText.Insert(postCodeText.Length - 3, " ");
            txtBox_Postcode.Text = postCodeText;
            findFullAddress(postCodeText);
        }

        private void btn_Insert_Click(object sender, RoutedEventArgs e)
        {
            addressToReturn = comboBox_addressList.SelectedItem.ToString();
            Close();
        }
    }
}
