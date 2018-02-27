using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Assignment4.Models.Database
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
        /// Gets reviews based on company name
        /// </summary>
        /// <param name="companyName">The name of the company</param>
        /// <throws type="ArgumentException">Throws an argument exception if the short url id does not refer to anything in the database</throws>
        /// <returns>The reviews for the company</returns>
        public List<Review> getCompanyReviews(string companyName)
        {
            string query = @"SELECT * FROM " + dbname + ".writtenReviews "
                + @"WHERE companyName='" + companyName + @"';";

            if (openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                List<Review> rList = new List<Review>();
                if (reader.Read() == true)
                {
                    //SET REVIEW DATA HERE
                    rList.Add(new Review
                    {
                        companyName = reader.GetString("companyName"),
                        username = reader.GetString("username"),
                        review = reader.GetString("review"),
                        stars = reader.GetInt32("stars"),
                        timestamp = reader.GetInt64("timestamp")
                    });
                    while (reader.Read())
                    {
                        rList.Add(new Review
                        {
                            companyName = reader.GetString("companyName"),
                            username = reader.GetString("username"),
                            review = reader.GetString("review"),
                            stars = reader.GetInt32("stars"),
                            timestamp = reader.GetInt64("timestamp")
                        });
                    }
                    reader.Close();
                    closeConnection();
                    return rList;
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
        public bool saveCompanyReview(string companyName, string username, string review, int? stars, long? timestamp)
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
                            "NOT NULL"
                        }, false
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