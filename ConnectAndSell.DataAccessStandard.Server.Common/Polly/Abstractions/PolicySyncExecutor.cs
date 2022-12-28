using System;
using System.Collections.Generic;
using System.Linq;
using Polly;
using Polly.Registry;

namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions
{
     /// <summary>
    /// 
    /// </summary>
    public class PolicySyncExecutor : IPolicySyncExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        public PolicyRegistry PolicyRegistry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policies"></param>
        public PolicySyncExecutor(IEnumerable<ISyncPolicy> policies)
        {
            if (policies == null) throw new ArgumentNullException(nameof(policies));
            var syncPolicies = policies.ToList();
            if (syncPolicies.Count == 0) throw new ArgumentException("You must supply at least one policy.", nameof(policies));

            PolicyRegistry = new PolicyRegistry
            {
                [nameof(PolicySyncExecutor)] = syncPolicies.Count == 1 ? syncPolicies.Single() : Policy.Wrap(syncPolicies.ToArray())
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Execute<T>(Func<T> action)
        {
            var policy = PolicyRegistry.Get<ISyncPolicy>(nameof(PolicySyncExecutor));
            return policy.Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action action)
        {
            var policy = PolicyRegistry.Get<ISyncPolicy>(nameof(PolicySyncExecutor));
            policy.Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Execute<T>(Func<Context, T> action, Context context)
        {
            var policy = PolicyRegistry.Get<ISyncPolicy>(nameof(PolicySyncExecutor));
            return policy.Execute(action, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        public void Execute(Action<Context> action, Context context)
        {
            var policy = PolicyRegistry.Get<ISyncPolicy>(nameof(PolicySyncExecutor));
            policy.Execute(action, context);
        }
    }
}
