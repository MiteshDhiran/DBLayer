using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public interface IPoolManagerStatistic
    {
        /// <summary>
        /// 
        /// </summary>
        int CurrentPoolCapacity { get; }
        
        /// <summary>
        /// 
        /// </summary>
        int MaxPoolCapacity { get; }
        /// <summary>
        /// 
        /// </summary>
        long MissCount { get; }
        /// <summary>
        /// 
        /// </summary>
        long NonReturnableCount { get; }
        /// <summary>
        /// 
        /// </summary>
        string PoolManagerName { get; }
        /// <summary>
        /// 
        /// </summary>
        Type PoolObjectType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
