using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Assignment4.Models.Database
{
    //Reuse of most code from assignment 3 - LinkDatabase.
    //Source code available on the D2L course website
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
        public List<Review> getCompanyReviews(string companyName)
        {
            //Query for selecting all rows with the same company name
            string query = @"SELECT * FROM writtenReviews "
                + @"WHERE companyName='" + companyName + @"';";

            if (openConnection() == true)
            {
                //Execute the sql query
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                //Generate a list of reviews from the sql query return
                List<Review> rList = new List<Review>();
                //Check that atleast one result was returned
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
                    //While loop the rest of the data (if any)
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
                    //Close the reader and return the list
                    reader.Close();
                    closeConnection();
                }
                else
                {
                    //Throw an exception indicating no result were found
                    throw new ArgumentException("No reviews in the database matches that company name.");
                }
                return rList;
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
            if (openConnection() == true)
            {
                //Query to insert a review into the db
                string query = @"INSERT INTO writtenReviews(companyName, username, review, stars, timestamp) "
                    + @"VALUES('" + companyName + @"', '" + username + @"', '" + review + @"', " + (int)stars + ", " + (long)timestamp + @");";
                //Execute the command, and if a row was affected (successful query) then return true, else return false
                MySqlCommand command = new MySqlCommand(query, connection);
                int rows = command.ExecuteNonQuery();
                closeConnection();
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
            // No column represents a primary key
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