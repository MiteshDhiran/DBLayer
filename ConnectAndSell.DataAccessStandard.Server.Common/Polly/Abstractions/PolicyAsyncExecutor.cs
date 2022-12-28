using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Registry;

namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions
{
    public class PolicyAsyncExecutor : IPolicyAsyncExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        public PolicyRegistry PolicyRegistry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policies"></param>
        public PolicyAsyncExecutor(IEnumerable<IAsyncPolicy> policies)
        {
            if (policies == null) throw new ArgumentNullException(nameof(policies));
            var asyncPolicies = policies.ToList();
            if (asyncPolicies.Count == 0) throw new ArgumentException("You must supply at least one policy.", nameof(policies));

            PolicyRegistry = new PolicyRegistry
            {
                [nameof(PolicyAsyncExecutor)] = asyncPolicies.Count == 1 ? asyncPolicies.Single() : Policy.WrapAsync(asyncPolicies.ToArray())
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            return await policy.ExecuteAsync(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<Task> action)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            await policy.ExecuteAsync(action);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<Context, Task<T>> action, Context context)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            return await policy.ExecuteAsync(action, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<Context, Task> action, Context context)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            await policy.ExecuteAsync(action, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            await policy.ExecuteAsync(action, cancellationToken );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            return await policy.ExecuteAsync(action, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<Context,CancellationToken, Task<T>> action, Context context, CancellationToken cancellationToken)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            return await policy.ExecuteAsync(action, context,cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<Context, CancellationToken, Task> action, Context context, CancellationToken cancellationToken)
        {
            var policy = PolicyRegistry.Get<IAsyncPolicy>(nameof(PolicyAsyncExecutor));
            await policy.ExecuteAsync(action, context, cancellationToken);
        }
    }
}
