using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LinkShortener.Models.Database
{
    public partial class ReviewDatabase : AbstractDatabase
    {
        private ReviewDatabase() { }

        public static ReviewDatabase getInstance()
        {
            if(instance == null)
            {
                instance = new ReviewDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Gets a long URL based on the id of the short url
        /// </summary>
        /// <param name="id">The id of the short url</param>
        /// <throws type="ArgumentException">Throws an argument exception if the short url id does not refer to anything in the database</throws>
        /// <returns>The long url the given short url refers to</returns>
        public string getLongUrl(string id)
        {
            string query = @"SELECT * FROM " + dbname + ".shortenedLinks "
                + "WHERE id=" + id + ";";

            if(openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if(reader.Read() == true)
                {
                    string originalUrl = reader.GetString("original");
                    reader.Close();
                    closeConnection();
                    return originalUrl;
                }
                else
                {
                    //Throw an exception indicating no result was found
                    throw new ArgumentException("No url in the database matches that id.");
                }
            }
            else
            {
                throw new Exception("Could not connect to database.");
            }
        }
        /// <summary>
        /// Gets reviews based on company name
        /// </summary>
        /// <param name="companyName">The name of the company</param>
        /// <throws type="ArgumentException">Throws an argument exception if the short url id does not refer to anything in the database</throws>
        /// <returns>The reviews for the company</returns>
        public Review getCompanyReviews(string companyName)
        {
            string query = @"SELECT * FROM " + dbname + ".writtenReviews "
                + @"WHERE companyName='" + companyName + @"';";

            if (openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                Review[] reviews;
                if (reader.Read() == true)
                {
                    //SET REVIEW DATA HERE
                    //reviews.add... = reader.GetString("username");
                    while (reader.Read())
                    {
                        //...
                    }
                    reader.Close();
                    closeConnection();
                    return reviews;
                }
                else
                {
                    //Throw an exception indicating no result were found
                    throw new ArgumentException("No reviews in the database matches that company name.");
                }
            }
            else
            {
                throw new Exception("Could not connect to database.");
            }
        }
        /// <summary>
        /// Saves the company review to the database to be accessed later via the company name
        /// </summary>
        public bool saveCompanyReview(string companyName, string username, string review, int stars, long timestamp)
        {
            string query = @"INSERT INTO " + dbname + ".writtenReviews "
                + @"VALUES('" + companyName + @"', '" + username + @"', '" + review + @"', " + stars + ", " + timestamp + @");";

            if (openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                int rows = command.ExecuteNonQuery();
                if (rows == 1) return true;
                else return false;
            }
            else
            {
                throw new Exception("Could not connect to database");
            }
        }
    }

    public partial class ReviewDatabase : AbstractDatabase
    {
        private static ReviewDatabase instance = null;

        private const String dbname = "Reviews";
        public override String databaseName { get; } = dbname;

        protected override Table[] tables { get; } =
        {
            // This represents the database schema
            new Table
            (
                dbname,
                "writtenReviews",
                new Column[]
                {
                    new Column
                    (
                        "companyName", "VARCHAR(300)",
                        new string[]
                        {
                            "NOT NULL",
                            "UNIQUE",
                        }, true
                    ),
                    new Column
                    (
                        "username", "VARCHAR(300)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "review", "VARCHAR(1000)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "stars", "INT(32)",
                        new string[]
                        {
                            "NOT NULL"
                        }, false
                    ),
                    new Column
                    (
                        "timestamp", "INT(64)",
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