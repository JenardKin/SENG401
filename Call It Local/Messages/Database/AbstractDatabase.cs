using MySql.Data.MySqlClient;

using System;
using System.Threading;

namespace Messages.Database
{
    /// <summary>
    /// This class is used as a base class for the creation and deletion of a database.
    /// To use this class you will need to implement the databaseName and tables properties.
    /// It is recommended that the inhereting class be a singleton.
    /// </summary>
    public abstract partial class AbstractDatabase
    {
        /// <summary>
        /// Creates the connection object, creates a mutex, and attempts to create the database if it does not exist already
        /// </summary>
        protected AbstractDatabase()
        {
            mutex = new Mutex(false);
            connection = new MySqlConnection("SERVER=localhost;DATABASE=mysql;UID=" + UID + ";AUTO ENLIST=false;PASSWORD=" + Password);
            createDB();
        }

        /// <summary>
        /// Creates the database, if it does not already exist
        /// </summary>
        public void createDB()
        {
            string commandString;
            MySqlCommand command;
            commandString = "CREATE DATABASE " + databaseName + ";";

            if(connection == null)
            {
                connection = new MySqlConnection("SERVER=localhost;DATABASE=mysql;UID=" + UID + ";AUTO ENLIST=false;PASSWORD=" + Password);
            }
            
            if (openConnection() == true)
            {
                //First try to create the actual database
                try
                {
                    command = new MySqlCommand(commandString, connection);
                    command.ExecuteNonQuery();
                    Debug.consoleMsg("Successfully created database " + databaseName);
                }
                catch (MySqlException e)
                {
                    if (e.Number == 1007)//Database already exists, no need to continure further
                    {
                        Debug.consoleMsg("Database already exists.");
                        closeConnection();
                        connection = new MySqlConnection("SERVER=localhost;DATABASE=" + databaseName + ";UID=" + UID + ";AUTO ENLIST=false;PASSWORD=" + Password);
                        return;
                    }
                    Debug.consoleMsg("Unable to create database"
                        + databaseName + " Error: " + e.Number + e.Message);
                    closeConnection();
                    return;
                }

                //Then try to create each of the tables in the database
                foreach (Table table in tables)
                {
                    try
                    {
                        commandString = table.getCreateCommand();
                        command = new MySqlCommand(commandString, connection);
                        command.ExecuteNonQuery();
                        Debug.consoleMsg("Successfully created the table "
                            + table.databaseName + "." + table.tableName);

                    }
                    catch (MySqlException e)
                    {
                        Debug.consoleMsg("Unable to create table "
                            + table.databaseName + "." + table.tableName
                            + " Error: " + e.Number + e.Message);
                    }
                }

                closeConnection();
                connection = new MySqlConnection("SERVER=localhost;DATABASE=" + databaseName + ";UID=" + UID + ";AUTO ENLIST=false;PASSWORD=" + Password);
            }
        }

        /// <summary>
        /// Deletes the database if it exists
        /// </summary>
        public void deleteDatabase()
        {
            if (openConnection() == true)
            {
                string commandString;
                MySqlCommand command;
                foreach (Table table in tables)
                {
                    try
                    {
                        commandString = table.getDropCommand();
                        command = new MySqlCommand(commandString, connection);
                        command.ExecuteNonQuery();
                        Debug.consoleMsg("Successfully deleted table "
                            + table.databaseName + "." + table.tableName);
                    }
                    catch (MySqlException e)
                    {
                        Debug.consoleMsg("Unable to delete table "
                            + table.databaseName + "." + table.tableName
                            + " Error: " + e.Number + e.Message);
                    }
                }

                commandString = "DROP DATABASE " + databaseName + ";";
                command = new MySqlCommand(commandString, connection);
                try
                {
                    command.ExecuteNonQuery();
                    Debug.consoleMsg("Successfully deleted database " + databaseName);
                }
                catch (MySqlException e)
                {
                    Debug.consoleMsg("Unable to delete database " + databaseName
                        + " Error: " + e.Number + e.Message);
                }
                finally
                {
                    closeConnection();
                }
            }
        }

        /// <summary>
        /// Attempts to open a connection to the database
        /// This function must be called before using the connection object
        /// Any function that calls this function should also call closeConnection() when it it finished with the connection object.
        /// </summary>
        /// <returns>true if the connection was successful, false otherwise</returns>
        protected bool openConnection()
        {
            try
            {
                mutex.WaitOne();
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                mutex.ReleaseMutex();
                switch (e.Number)
                {
                    case 0:
                        Debug.consoleMsg("Cannot connect to database.");
                        break;
                    case 1045:
                        Debug.consoleMsg("Invalid username or password for database.");
                        break;
                    default:
                        Debug.consoleMsg("Cannot connect to database. Error code <" + e.Number + ">");
                        break;
                }
                return false;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Equals("The connection is already open."))
                {
                    return true;
                }
                mutex.ReleaseMutex();
                return false;
            }
            catch (Exception e)
            {
                mutex.ReleaseMutex();
                return false;
            }
        }

        /// <summary>
        /// Attempts to close the connection with the database
        /// This function MUST be called when you are finished with the connection object
        /// If you do not, the mutex will prevent any further use of the database.
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        protected bool closeConnection()
        {
            try
            {
                connection.Close();
                mutex.ReleaseMutex();
                return true;
            }
            catch (MySqlException e)
            {
                mutex.ReleaseMutex();
                Debug.consoleMsg("Could not close connection to database. Error message: " + e.Number + e.Message);
                return false;
            }
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables.
    /// </summary>
    public abstract partial class AbstractDatabase
    {
        /// <summary>
        /// This object is used for making all queries to the database.
        /// It should be opened and closed
        /// for each query made using the functions provided above.
        /// </summary>
        protected MySqlConnection connection;

        /// <summary>
        /// This is the username used to login to the database by the connection
        /// </summary>
        private const string UID = "root";

        /// <summary>
        /// This is the password used to login to the database by the connection
        /// </summary>
        private const string Password = "root";//"abc123";

        /// <summary>
        /// This is the name of the database. This property must be defined by the inheriting class
        /// </summary>
        public abstract String databaseName { get; }

        /// <summary>
        /// This represents the tables in the database. The inheriting class must define and populate
        /// this property so that this class may properly create or delete the database
        /// </summary>
        protected abstract Table[] tables { get; }

        /// <summary>
        /// The mutex object used to ensure that only one process may access the database at any given time
        /// </summary>
        private Mutex mutex;
    }
}
