using NServiceBus;

using System;

namespace Messages.DataTypes.Database.CompanyDirectory
{
    /// <summary>
    /// This class represents a company whos information is entered in one or more databases
    /// </summary>
    [Serializable]
    public partial class CompanyInstance
    {
        public CompanyInstance(string companyName)
        {
            this.companyName = companyName;
        }

        /// <summary>
        /// Creates a CompanyInstance object using the 
        /// </summary>
        /// <param name="companyName">The name of the company</param>
        /// <param name="phoneNumber">The phone number of the company</param>
        /// <param name="email">The email of the company</param>
        /// <param name="locations">An array of locations the company resides</param>
        public CompanyInstance(string companyName, string phoneNumber, string email, string[] locations)
        {
            this.companyName = companyName;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.locations = locations;
        }
        
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    public partial class CompanyInstance
    {
        /// <summary>
        /// The name of the company, corresponds to the username of the companies account
        /// </summary>
        public String companyName { get; set; } = null;

        /// <summary>
        /// The phone number of the company
        /// </summary>
        public String phoneNumber { get; set; } = null;

        /// <summary>
        /// The email of the company
        /// </summary>
        public String email { get; set; } = null;

        /// <summary>
        /// A list containing the address' of the company
        /// </summary>
        public String[] locations { get; set; } = null;

        
    }
}
