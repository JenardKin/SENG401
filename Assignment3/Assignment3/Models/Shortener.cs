using System;
namespace LinkShortener.Models
{
    public static class Shortener
    {
        public static readonly string characters = "ABCEDFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static readonly int charCount = characters.Length;

        public static string GetShortEncoding(int i)
        {
            if (i == 0)
                return characters[0].ToString();

            string s = "";

            while (i > 0)
            {
                s = characters[i % charCount] + s;
                i = i / charCount;
            }
            return s;
        }
        public static int GetLongDecoding(string s)
        {
            int i = 0;
            foreach (char c in s)
            {
                i = (i * charCount) + characters.IndexOf(c);
            }
            return i;
        }
    }
}
