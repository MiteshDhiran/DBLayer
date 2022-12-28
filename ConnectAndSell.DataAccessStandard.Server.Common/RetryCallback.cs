using System;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public delegate void RetryCallback(int currentRetryCount,Exception lastException,TimeSpan delay);
}
