using Messages;
using Messages.Database;
using Messages.DataTypes;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest.Echo.Requests;

using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoService.Database
{
    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>
    public partial class EchoServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private EchoServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static EchoServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new EchoServiceDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Saves the foreward echo to the database
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public void saveAsIsEcho(AsIsEchoEvent echo)
        {
            if(openConnection() == true)
            {
                string query = @"INSERT INTO echoforward(timestamp, username, datain)" +
                    @"VALUES('" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString() +
                    @"', '" + echo.username + @"', '" + echo.data + @"');";

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
        /// Saves the reverse echo to the database
        /// </summary>
        /// <param name="echo">Information about the echo</param>
        public void saveReverseEcho(ReverseEchoRequest request)
        {
            if (openConnection() == true)
            {
                string query = @"INSERT INTO echoreverse(timestamp, username, datain)" +
                    @"VALUES('" + DateTimeOffset.Now.ToUnixTimeSeconds().ToString() +
                    @"', '" + request.username + @"', '" + request.data + @"');";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                closeConnection();
            }
            else
            {
                Debug.consoleMsg("Unable to connect to database");
            }
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class EchoServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "echoservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static EchoServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
            (
                dbname,
                "echoforward",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(64)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "username", "VARCHAR(50)",
                        new string[] {},
                        false
                    ),
                    new Column
                    (
                        "datain", "VARCHAR(" + SharedData.MAX_MESSAGE_LENGTH.ToString() + ")",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                }
            ),
            new Table
            (
                dbname,
                "echoreverse",
                new Column[]
                {
                    new Column
                    (
                        "id", "INT(64)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                            "AUTO_INCREMENT"
                        }, true
                    ),
                    new Column
                    (
                        "timestamp", "INT(32)",
                        new string[]
                        {
                            "NOT NULL",
                        }, false
                    ),
                    new Column
                    (
                        "username", "VARCHAR(50)",
                        new string[] 
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "datain", "VARCHAR(" + SharedData.MAX_MESSAGE_LENGTH.ToString() + ")",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                }
            )
        };
    }
}
