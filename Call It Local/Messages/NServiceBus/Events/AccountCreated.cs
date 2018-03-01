using NServiceBus;

using Messages.NServiceBus.Commands;
using Messages.DataTypes;

namespace Messages.NServiceBus.Events
{

    /// <summary>
    /// Represents the event published when a new account is created
    /// </summary>
    public class AccountCreated : IEvent
    {
        public AccountCreated(CreateAccount newAcct)
        {
            username = newAcct.username;
            password = newAcct.password;
            address = newAcct.address;
            phonenumber = newAcct.phonenumber;
            email = newAcct.email;
            type = newAcct.type;
        }

        /// <summary>
        /// New username
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Password for account
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
        /// The email of the new user
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// The type of acount for the new user
        /// </summary>
        public AccountType type { get; set; }
    }
}
