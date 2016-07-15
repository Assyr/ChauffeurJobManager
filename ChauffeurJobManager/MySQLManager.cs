using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace ChauffeurJobManager
{
    class MySQLManager
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;


        //Constructor
        public MySQLManager()
        {
            Initialize();
            AllocConsole();
        }

        public void Initialize()
        {
            server = "127.0.0.1";
            database = "lecdatabase";
            uid = "root";
            password = "test";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        public bool openConnection()
        {
            try
            {
                connection.Open();
                Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Connection to server opened!");
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Connection to server failed!");
                        break;
                    case 1045:
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Invalid username/password to MySQL");
                        break;
                }
                return false;
            }
        }

        public bool closeConnection()
        {
            try
            {
                connection.Close();
                Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Connection to server closed!");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
