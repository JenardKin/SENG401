using System;
using System.Collections.Generic;

namespace LinkShortener.Models.Database
{

    /// <summary>
    /// This class represents a table in a MySQL Database.
    /// </summary>
    public partial class Table
    {
        /// <summary>
        /// Creates a data structure to hold information about your table
        /// </summary>
        /// <param name="name">The name of the table</param>
        /// <param name="structure">The MySQL information to create the table. Should be able to append this string
        /// to a CREATE TABLE command.</param>
        public Table(string databaseName, string tableName, Column[] columns)
        {
            this.databaseName = databaseName;
            this.tableName = tableName;
            this.columns = columns;
        }

        /// <summary>
        /// Generates a MySQL command that when executed will create this table.
        /// </summary>
        /// <returns>The MySQL statement to create the table</returns>
        public string getCreateCommand()
        {
            string query = "CREATE TABLE " + databaseName + "." + tableName + "(";
            List<string> primaryKeys = new List<string>();

            foreach (Column column in columns)
            {
                query += column.getCreateStructure() + ",";
                if (column.primaryKey == true)
                {
                    primaryKeys.Add(column.name);
                }
            }
            if (primaryKeys.Count == 0)
            {
                query = query.Substring(0, query.Length - 1);//Removes the extra comma, since no primary keys are specified
            }
            else
            {
                query += "PRIMARY KEY(";
                int i;
                for (i = 0; i != primaryKeys.Count - 1; i++)
                {
                    query += primaryKeys[i] + ", ";
                }
                query += primaryKeys[i] + ")";
            }
            query += ");";
            return query;
        }

        /// <summary>
        /// Generates a MySQL command that when executed will delete this table.
        /// </summary>
        /// <returns>The MySQL statement to create the table</returns>
        public string getDropCommand()
        {
            return "DROP TABLE " + databaseName + "." + tableName + ";";
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables, as well as relevent
    /// getters / setters
    /// </summary>
    public partial class Table
    {
        /// <summary>
        /// The name of the database this table is used in
        /// </summary>
        public String databaseName { get; }

        /// <summary>
        /// The name of the table
        /// </summary>
        public string tableName { get; }

        /// <summary>
        /// Represents the structure of the database
        /// </summary>
        private readonly Column[] columns;
        /// <summary>
        /// Returns all the column objects associated with 
        /// </summary>
        /// <returns>An array of columns contained in the table</returns>
        public Column[] getColumns()
        {
            return columns;
        }
        /// <summary>
        /// Gets a specific column in the table with the column name as the deliminator
        /// </summary>
        /// <param name="name">The name of the column</param>
        /// <returns>The column object associated with the given name if it exists, or null if it does not</returns>
        public Column getColumn(string name)
        {
            foreach (Column column in columns)
            {
                if (column.name.Equals(name.ToLower()))
                {
                    return column;
                }
            }
            return null;
        }
    }
}
