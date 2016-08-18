using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Xml;

namespace ChauffeurJobManager
{
    class MySQLManager
    {
        public MySqlConnection sqlConnect;

        private int userCompanyID;
        public string userCompanyDatabase;

        public string loginDatabase = "chauffeurjobmanager";
        private string server = "127.0.0.1";
        private string uid = "root";
        private string password = "test";


        public bool openConnection(string _databaseName)
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            _databaseName + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";


            sqlConnect = new MySqlConnection(connectionString);
            try
            {
                sqlConnect.Open();
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
                sqlConnect.Close();
                Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Connection to server closed!");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool loginAuth(string authUsername, string authPassword)
        {
                MySqlCommand selectLoginCredentials = new MySqlCommand("select * from " + loginDatabase + "._users where username = '" + authUsername + "' and password = '" + authPassword + "' ; ", sqlConnect);
                MySqlDataReader myReader = selectLoginCredentials.ExecuteReader();

                int x = 0;
                while (myReader.Read())
                {
                    x++;
                }

                if (x == 1)
                {
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Login authentication succesful, welcome " + authUsername);
                    userCompanyDatabase += myReader.GetString("customer_database_name");
                    closeConnection();
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "userCompanyID = " + userCompanyID + " - Username = " + authUsername + " - Password = " + authPassword + " - Database = " + userCompanyDatabase);
                    return true;
                }
                else if (x > 1)
                {
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Access denied - check for duplicate login accounts");
                    closeConnection();
                    return false;
                }
                else
                {
                    Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt - ") + "Access denied - username and / or password is incorrect");
                    closeConnection();
                    return false;
                }
        }

        public IList<String> getDatabaseTables()
        {
            List<string> tables = new List<string>();
            DataTable dt = sqlConnect.GetSchema("Tables");
            foreach(DataRow row in dt.Rows)
            {
                string tableName = (string)row[2];
                tables.Add(tableName);
            }
            return tables;
        }

        public void sendQueryToDatabase(string query)
        {
            MySqlCommand sendQueryToDatabase = new MySqlCommand(query, sqlConnect);
            sendQueryToDatabase.ExecuteNonQuery();
        }

        public DataTable getDatabaseTableInfo(string databaseName, string tableToGetInfoFrom)
        {
            DataTable schema = null;

            Console.WriteLine("getDatabaseTableInfo connecting to database: " + databaseName);
            Console.WriteLine("getDatabaseTableInfo using table: " + tableToGetInfoFrom);
            openConnection(databaseName);
            MySqlCommand schemaCommand = new MySqlCommand("SELECT * FROM " + tableToGetInfoFrom, sqlConnect);

            MySqlDataReader reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly);
            schema = reader.GetSchemaTable();

            closeConnection();
            Console.WriteLine("Returning 'schema'");
            return schema;

        }
    }
}