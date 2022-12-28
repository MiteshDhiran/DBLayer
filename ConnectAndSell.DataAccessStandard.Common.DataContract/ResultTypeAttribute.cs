using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ResultTypeAttribute : Attribute
    {
        private Type type;

        /// <summary>Initializes a new instance of the <see cref="T:System.Data.Linq.Mapping.ResultTypeAttribute" /> class.</summary>
        /// <param name="type">The type of the result returned by a function having various result types.</param>
        public ResultTypeAttribute(Type type)
        {
            this.type = type;
        }

        /// <summary>Gets the valid or expected type mapping for a function having various result types.</summary>
        /// <returns>The type of result (<see cref="T:System.Type" />).</returns>
        public Type Type
        {
            get
            {
                return this.type;
            }
        }
    }
}
