using System;
using System.Windows;
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
                MySQLManager loginSQLManager = new MySQLManager();
                loginSQLManager.openConnection(loginSQLManager.loginDatabase);
                //Not sure! Must pass it as PasswordBox for better security?
                if(loginSQLManager.loginAuth(username, password)) //if auth was succesful
                {
                    //Connect to users company database and grab all tables
                    loginSQLManager.openConnection(loginSQLManager.userCompanyDatabase);

                    welcome welcomeScreen = new welcome();
                    welcomeScreen.databaseName = loginSQLManager.userCompanyDatabase;
                    welcomeScreen.updateTableList();
                    loginSQLManager.closeConnection();
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
