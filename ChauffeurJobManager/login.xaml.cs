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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public login()
        {
            InitializeComponent();
            AllocConsole();
            loginUsername.Focus();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (loginUsername.Text.Length == 0)
            { //If username field is empty
                errormessage.Text = "Username field cannot be empty.";
                loginUsername.Focus(); //Set focus to email textBox
            }
            else if (loginPassword.Password.Length == 0)
            {
                errormessage.Text = "Password field cannot be empty.";
                loginPassword.Focus();
            }
            else
            {

                string username = loginUsername.Text;
                string password = loginPassword.Password; //Get password as SecureString - gets deleted from memory when not in use
                MySQLManager SQLManager = new MySQLManager();
                SQLManager.openConnection(SQLManager.loginDatabase);
                //Not sure! Must pass it as PasswordBox for better security?
                if(SQLManager.loginAuth(username, password)) //if auth was succesful
                {
                    //Connect to users company database and grab all tables
                    SQLManager.openConnection(SQLManager.userCompanyDatabase);

                    welcome welcomeScreen = new welcome();
                    welcomeScreen.listOfDatabaseTables = SQLManager.getDatabaseTables();
                    SQLManager.closeConnection();
                    welcomeScreen.updateTableList();
                    welcomeScreen.databaseName = SQLManager.userCompanyDatabase;
                    //Switch to welcome screen
                    welcomeScreen.TextBlockName.Text = username;
                    welcomeScreen.Top = this.Top;
                    welcomeScreen.Left = this.Left;
                    welcomeScreen.Show();
                    Close();
                }
                else //If auth was unsuccesful
                {
                    errormessage.Text = DateTime.Now.ToString("h:mm:ss tt - ") + "Access denied - username and / or password is incorrect";
                }
            }
        }
    }
}
