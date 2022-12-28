using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false)]
    public sealed class ParameterAttribute : Attribute
    {
        private string name;
        private string dbType;

        /// <summary>Gets or sets the name of the parameter.</summary>
        /// <returns>The name as a string.</returns>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>Gets or sets the type of the parameter for a provider-specific database.</summary>
        /// <returns>The type as a string.</returns>
        public string DbType
        {
            get
            {
                return this.dbType;
            }
            set
            {
                this.dbType = value;
            }
        }
    }
}
