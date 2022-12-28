using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class TraceInfo
    {
        public string UserName { get; private set; }
        public DateTimeOffset CurrentTime { get; private set; }
        public Guid TraceID { get; private set; }
        public int ThreadID { get; }

        public TraceInfo(string userName, DateTimeOffset currentTime)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException($"Argument {nameof(userName)} is null or empty");
            }
            UserName = userName;
            CurrentTime = currentTime;
            TraceID = Guid.NewGuid();
            ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
    }
}
