using System;
using System.Collections.Generic;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    [Serializable]
    public class PropertyMismatchException : ApplicationException
    {
        private List<string> propList;

        public List<string> MissingProperties
        {
            get
            {
                return this.propList;
            }
        }

        public PropertyMismatchException()
        {
        }

        public PropertyMismatchException(string message)
            : base(message)
        {
        }

        public PropertyMismatchException(string message, List<string> missingProperties)
            : base(message)
        {
            this.propList = missingProperties;
        }

        public PropertyMismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PropertyMismatchException(
            string message,
            Exception innerException,
            List<string> missingProperties)
            : base(message, innerException)
        {
            this.propList = missingProperties;
        }
    }
}
