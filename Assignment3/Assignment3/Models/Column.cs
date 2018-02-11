using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Models.Database
{
    /// <summary>
    /// This class represents a column in a table in a MySql database
    /// </summary>
    public partial class Column
    {
        /// <summary>
        /// Constructor for a column object
        /// </summary>
        /// <param name="name">The name of the column</param>
        /// <param name="type">The data type the column contains, as well as its relevent restrictions</param>
        /// <param name="mods">Any specific modidfications to the data, i.e. UNIQUE, NOT NULL, etc</param>
        /// <param name="primaryKey">Indicates if this column is a primary key</param>
        public Column(string name, string type, string[] mods, bool primaryKey)
        {
            this.name = name.ToLower();
            this.type = type;
            this.primaryKey = primaryKey;
            this.mods = mods;
        }

        /// <summary>
        /// Returns the structure of the column in a way that can be used in a
        /// CREATE table statement
        /// </summary>
        /// <returns>The structure of the column as a string</returns>
        public string getCreateStructure()
        {
            string structure = name + " " + type;
            if (mods != null)
            {
                foreach (string mod in mods)
                {
                    structure += " " + mod;
                }
            }
            return structure;
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    public partial class Column
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public string name { get; }

        /// <summary>
        /// The data type of the column
        /// </summary>
        public string type { get; }

        /// <summary>
        /// The modfiers on the column, such as UNIQUE or NOT NULL
        /// May be null
        /// </summary>
        public string[] mods { get; }

        /// <summary>
        /// Represents whether or not this column is a primary key.
        /// </summary>
        public bool primaryKey { get; }
    }
}
