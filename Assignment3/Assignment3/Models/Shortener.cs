namespace LinkShortener.Models
{
    public static class Shortener
    {
        // Reference for this encoding/decoding link-shortener algorithm is from:
        //https://stackoverflow.com/questions/742013/how-to-code-a-url-shortener

        //The characters used to encode the url
        public static readonly string characters = "ABCEDFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //The length of characters (36)
        public static readonly int charCount = characters.Length;

        //Method to encode an integer to a string
        //Takes an int (correlating to the PK of the DB table)
            //Hence the int MUST be a POSITIVE integer
        public static string GetShortEncoding(int i)
        {
            //Empty initialize the return
            string s = "";

            //Loop while i > 0 (characters left to be added to the encoding string)
            while (i > 0)
            {
                //Check if the modulo of i and length is 0, if so use the last character in our encoding characters
                //Otherwise, take modulo of i and length then subtract 1
                //This differs from the algorithm we referenced as we wanted to ensure the first entry to the DB
                    //i.e. PK == 1
                //Leads to an encoding of the string "A"
                if (i % charCount == 0)
                    s = characters[charCount - 1] + s;
                else
                    s = characters[i % charCount - 1] + s;
                //Divide i by length of characters (C# does floor division)
                i = i / charCount;
            }
            //Return the encoding
            return s;
        }
        //Method to decode a string and return its respective PK in our DB
        public static int GetLongDecoding(string s)
        {
            //Begin i at 0
            int i = 0;
            //Loop through all characters in the string (left-right)
            foreach (char c in s)
            {
                //This converts our base 32 string to an int (starting at 1) iteratively
                i = (i * charCount) + characters.IndexOf(c) + 1;
            }
            //Return the decoded int
            return i;
        }
    }
}
