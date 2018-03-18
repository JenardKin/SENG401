using System;

namespace Messages.DataTypes.Database.CompanyDirectory
{
    /// <summary>
    /// Contains a list of company names
    /// </summary>
    [Serializable]
    public partial class CompanyList
    {
        /// <summary>
        /// A list of the company names
        /// </summary>
        public string[] companyNames { get; set; }
    }
}
