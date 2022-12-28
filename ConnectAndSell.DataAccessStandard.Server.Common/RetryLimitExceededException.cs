using System;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    [Serializable]
    public sealed class RetryLimitExceededException : Exception
    {
        public RetryLimitExceededException()
            : this("Resources.ExceptionTextRetryLimitExceeded")
        {
        }

        public RetryLimitExceededException(string message)
            : base(message)
        {
        }

        public RetryLimitExceededException(Exception innerException)
            : base(innerException != null ? innerException.Message : "Resources.ExceptionTextRetryLimitExceeded", innerException)
        {
        }
    }
}
