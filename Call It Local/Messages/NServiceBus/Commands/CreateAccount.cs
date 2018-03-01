using NServiceBus;

using Messages.DataTypes;

using System;

namespace Messages.NServiceBus.Commands
{

    /// <summary>
    ///  This class represents the command to create an account as well as all of the information needed to do so.
    /// </summary>
    [Serializable]
    public partial class CreateAccount : ICommand
    {
        /// <summary>
        /// The username of the new account
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// The password for the new account
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// The address of the new user
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// The phone number of the new user
        /// </summary>
        public string phonenumber { get; set; }

        /// <summary>
        /// the email of the new user
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// The type of account
        /// </summary>
        public AccountType type { get; set; }
    }
}
