using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class FunctionAttribute : Attribute
    {
        private string name;
        private bool isComposable;

        /// <summary>Gets or sets the name of the function.</summary>
        /// <returns>The name of the function or stored procedure.</returns>
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

        /// <summary>Gets or sets whether a method is mapped to a function or to a stored procedure.</summary>
        /// <returns>
        /// <see langword="true" /> if a function; <see langword="false" /> if a stored procedure.</returns>
        public bool IsComposable
        {
            get
            {
                return this.isComposable;
            }
            set
            {
                this.isComposable = value;
            }
        }
    }
}
