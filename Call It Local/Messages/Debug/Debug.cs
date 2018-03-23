using System;

namespace Messages
{
    public static class Debug
    {

        /// <summary>
        /// Outputs the message to the console if the debug bool is true
        /// Otherwise this function will do nothing
        /// </summary>
        /// <param name="msg"></param>
        public static void consoleMsg(string msg)
        {
#if DEBUG
            Console.WriteLine(msg);
#else
            return;
#endif
        }
    }
}
