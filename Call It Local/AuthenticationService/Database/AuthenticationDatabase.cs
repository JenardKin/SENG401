using Messages.Database;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest;

using MySql.Data.MySqlClient;

using System;

namespace AuthenticationService.Database
{
    /// <summary>
    /// This class is used to manipulate and read the Authentication Service's database in a safe and consistent manner.
    /// It follows the singleton design pattern, as only one instance of this class should ever be in existance at any given time.
    /// </summary>
    public partial class AuthenticationDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private AuthenticationDatabase(){ }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static AuthenticationDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new AuthenticationDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Attempts to insert a new user account into the database
        /// </summary>
        /// <param name="accountInfo">Contains information about the </param>
        /// <returns>A message indicating the result of the attempt</returns>
        public ServiceBusResponse insertNewUserAccount(CreateAccount accountInfo)
        {
            bool result = false;
            string message = "";
            if(openConnection() == true)
            {
                string query = @"INSERT INTO user(username, password, address, phonenumber, email, type) " +
                    @"VALUES('" + accountInfo.username + @"', '" + accountInfo.password + 
                    @"', '" + accountInfo.address + @"', '" + accountInfo.phonenumber + 
                    @"', '" + accountInfo.email + @"', '" + accountInfo.type.ToString() + @"');";

                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    result = true;
                }
                catch(MySqlException e)
                {
                    Messages.Debug.consoleMsg("Unable to complete insert new user into database." +
                        " Error :" + e.Number + e.Message);
                    Messages.Debug.consoleMsg("The query was:" + query);
                    message = e.Message;
                }
                catch (Exception e)
                {
                    Messages.Debug.consoleMsg("Unable to Unable to complete insert new user into database." +
                        " Error:" + e.Message);
                    message = e.Message;
                }
                finally
                {
                    closeConnection();
                }
            }
            else
            {
                message = "Unable to connect to database";
            }

            return new ServiceBusResponse(result, message);
        }

        /// <summary>
        /// This function is used to check and see if the given username and password correspond
        /// to an existing user account.
        /// </summary>
        /// <param name="username">The username to check the database for</param>
        /// <param name="password">The password to check the database for</param>
        /// <returns>True if the info corresponds to an entry in the database, false otherwise</returns>
        public ServiceBusResponse isValidUserInfo(string username, string password)
        {
            string query = @"SELECT * FROM " + databaseName + @".user " +
                @"WHERE username='" + username + @"' " +
                @"AND password='" + password + @"';";
            
            bool result = false;
            string message = "";

            if (openConnection() == true)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = command.ExecuteReader();
                    result = dataReader.Read();
                    dataReader.Close();
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
                finally
                {
                    closeConnection();
                }
            }
            else
            {
                result = false;
                message = "Could not connect to database.";
            }
            return new ServiceBusResponse(result, message);
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables as well as the schema definition in the form of Table/Column objects
    /// </summary>
    public partial class AuthenticationDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "authenticationservicedb";
        public override String databaseName { get; } = dbname;

        /// <summary>
        /// The singleton instance of the database
        /// </summary>
        private static AuthenticationDatabase instance;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
                (
                    dbname,
                    "user",
                    new Column[]
                    {
                        new Column
                        (
                            "username", "VARCHAR(50)",
                            new string[]
                            {
                                "NOT NULL",
                                "UNIQUE"
                            }, true
                        ),
                        new Column
                        (
                            "password", "VARCHAR(50)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "address", "VARCHAR(50)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "phonenumber", "VARCHAR(10)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "email", "VARCHAR(100)",
                            new string[]
                            {
                                "NOT NULL",
                                "UNIQUE"
                            }, false
                        ),
                        new Column
                        (
                            "type", "VARCHAR(20)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        )
                    }
                )
        };
    }
}
