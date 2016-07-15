using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        welcome welcomeWindow = new welcome();

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text.Length == 0) { //If username field is empty
                errormessage.Text = "Username cannot be empty.";
                textBoxUsername.Focus(); //Set focus to email textBox
            }
        }
    }
}
