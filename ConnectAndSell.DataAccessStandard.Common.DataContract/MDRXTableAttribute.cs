using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>Designates a class as an entity class that is associated with a database table.</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MDRXTableAttribute : Attribute
    {
        private string name;

        /// <summary>Gets or sets the name of the table or view.</summary>
        /// <returns>By default, the value is the same as the name of the class.</returns>
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
    }
}
