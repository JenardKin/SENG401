using CompanyDirectoryService.Database;

using Messages.NServiceBus.Events;
using Messages.DataTypes;

using NServiceBus;
using NServiceBus.Logging;

using System.Threading.Tasks;

namespace CompanyService.Handlers
{
    /// <summary>
    /// This is the handler class for the account created event. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class AccountCreatedHandler : IHandleMessages<AccountCreated>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<AccountCreated>();

        /// <summary>
        /// Saves the account to the database if it is of type business
        /// This method will be called by the NServiceBus framework when an event of type "AccountCreated" is published.
        /// </summary>
        /// <param name="account">Information about the account</param>
        /// <param name="context"></param>
        /// <returns>Nothing</returns>
        public Task Handle(AccountCreated account, IMessageHandlerContext context)
        {
            if(account.type == AccountType.business)
            {
                CompanyDirectoryServiceDatabase.getInstance().saveCompanyInfo(account);
            }
            return Task.CompletedTask;
        }
    }
}
