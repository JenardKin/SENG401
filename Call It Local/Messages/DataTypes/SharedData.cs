
namespace Messages.DataTypes
{
    public static class SharedData
    {
        /// <summary>
        /// The deliminator used to indicate the end of a message passed between client and bus server
        /// </summary>
        public const string msgEndDelim = "<EOF>";

        /// <summary>
        /// Indicates the maximum length of a message in the chat box
        /// </summary>
        public const int MAX_MESSAGE_LENGTH = 500;
    }
}
