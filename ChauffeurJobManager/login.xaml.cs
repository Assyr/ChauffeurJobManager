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



namespace ChauffeurJobManager
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        MySQLManager SQLManager = new MySQLManager();
        welcome welcomeScreen;

        public login()
        {
            InitializeComponent();
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

                SQLManager.openConnection(SQLManager.loginDatabase);
                //Not sure! Must pass it as PasswordBox for better security?
                if(SQLManager.loginAuth(username, password)) //if auth was succesful
                {
                    //Connect to users company database and grab all tables
                    SQLManager.openConnection(SQLManager.userCompanyDatabase);
                    IList<string> listOfDatabaseTables = SQLManager.getDatabaseTables();
                    welcomeScreen = new welcome();

                    foreach (string table  in listOfDatabaseTables)
                    {
                        System.Windows.Controls.Button newBtn = new Button();
                        newBtn.Content = table;
                        welcomeScreen.tableButtons.Children.Add(newBtn);
                    }
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
