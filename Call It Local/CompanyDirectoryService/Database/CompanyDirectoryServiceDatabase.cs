using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.DataTypes.Database.CompanyDirectory;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyDirectoryService.Database
{
    //The following follows the same design as EchoService/Database/EchoServiceDatabase.cs
    //Code and comments recycled from that cs file.

    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>
    public partial class CompanyDirectoryServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private CompanyDirectoryServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static CompanyDirectoryServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new CompanyDirectoryServiceDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Saves company info into the database
        /// </summary>
        /// <param name="account">Information about the company</param>
        public void saveCompanyInfo(AccountCreated account)
        {
            if(openConnection() == true)
            {
                string query = @"INSERT INTO businessinfo(username, address, phonenumber, email)" +
                    @"VALUES('" + account.username + @"', '" + account.address + @"', '" + account.phonenumber +
                    @"', '" + account.email + @"');";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
        }
        
        /// <summary>
        /// Searches database for a company with a specific name
        /// </summary>
        /// <param name="companyName">Name of the company to search for</param>
        public CompanySearchResponse searchCompanyInfo(string companyName)
        {
            bool result = false;
            string message = "";
            CompanyList companyList = new CompanyList();
            if (openConnection() == true)
            {
                string query = @"SELECT b.username FROM businessinfo as b WHERE b.username LIKE '%" + companyName + @"%' COLLATE utf8_general_ci;";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                List<string> companyNames = new List<string>();
                if (reader.Read())
                {
                    result = true;
                    companyNames.Add(reader.GetString("username"));
                    while (reader.Read())
                    {
                        companyNames.Add(reader.GetString("username"));
                    }
                    companyList.companyNames = companyNames.ToArray();
                }
                else
                {
                    message = "No companies containing '" + companyName + "' found";
                }
                reader.Close();
                closeConnection();
            }
            else
            {
                message = "Unable to connect to database";
                Debug.consoleMsg("Unable to connect to database");
            }
            return new CompanySearchResponse(result, message, companyList);
        }

        /// <summary>
        /// Gets the info of a single company
        /// </summary>
        /// <param name="companyName">Name of the company</param>
        public GetCompanyInfoResponse getCompanyInfo(string companyName)
        {
            bool result = false;
            string message = "";
            CompanyInstance companyInst = new CompanyInstance(companyName);
            if (openConnection() == true)
            {
                string query = @"SELECT * FROM businessinfo as b WHERE b.username='" + companyName + @"' COLLATE utf8_general_ci;";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    result = true;
                    companyInst.companyName = reader.GetString("username");
                    companyInst.email = reader.GetString("email");
                    companyInst.locations = new string[] { reader.GetString("address") };
                    companyInst.phoneNumber = reader.GetString("phonenumber");
                }
                else
                {
                    message = "No company named '" + companyName + "' found";
                }
                closeConnection();
            }
            else
            {
                message = "Unable to connect to database";
                Debug.consoleMsg("Unable to connect to database");
            }
            return new GetCompanyInfoResponse(result, message, companyInst);
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class CompanyDirectoryServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "companydirectoryservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static CompanyDirectoryServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
                (
                    dbname,
                    "businessinfo",
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
                    }
                )
        };
    }
}
