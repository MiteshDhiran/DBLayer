/*******************************************************************************************************
P r o p r i e t a r y N o t i c e                                                                      '
Unpublished © 2018-2019 Allscripts Healthcare, LLC and/or its affiliates. All Rights Reserved.              '
                                                                                                       '
P r o p r i e t a r y N o t i c e: This software has been provided pursuant to a License Agreement,    '
with Allscripts Healthcare, LLC and/or its affiliates, containing restrictions on its use. This        '
software contains valuable trade secrets and proprietary information of Allscripts Healthcare, LLC     '
and/or its affiliates and is protected by trade secret and copyright law. This software may not be     '
copied or distributed in any form or medium, disclosed to any third parties, or used in any manner     '
not provided for in said License Agreement except with prior written authorization from Allscripts     '
Healthcare, LLC and/or its affiliates. Notice to U.S. Government Users: This software is               '
“Commercial Computer Software.”                                                                        '
                                                                                                       '
Sunrise Clinical Manager (SCM) is a trademark of Allscripts Healthcare, LLC and/or its affiliates.     '
P r o p r i e t a r y N o t i c e                                                                      '
*******************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions;
using ConnectAndSell.DataAccessStandard.Server.Common.Polly.Policies;
using Polly;
using Polly.Registry;



namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly
{
    /// <summary>
    /// The order which is inserted the policies into the list matters.
    /// https://github.com/App-vNext/Polly/wiki/PolicyWrap#usage-recommendations
    /// Usage:
    ///To create the Policy
    ///var builder = new SqlPolicyBuilder();
    ///resilientAsyncStrategy = builder
    ///.UseAsyncExecutor()
    ///.WithFallback(async () => await DoFallbackAsync())
    ///.WithOverallTimeout(TimeSpan.FromMinutes(10))
    ///.WithTransientErrors(retryCount: 3)
    ///.WithCircuitBreaker(exceptionsAllowedBeforeBreaking: 2)
    ///.Build();
    ///To execute using the Policy
    ///resilientAsyncStrategy.Execute(() =>
    ///{
    ///string sql = "insert into customer (CustomerName,Address,City,state,zip) Values (@CustomerName,@Address,@city,@state,@zip)";
    ///SqlMapper.Execute(conn, sql, new { CustomerName = "TEST", Address = "XX", city = "YY", state = "NC", zip = "27560" }, commandType: CommandType.Text);
    ///})
    /// </summary>
    internal class SqlAsyncPolicyBuilder : ISqlAsyncPolicyBuilder
    {
        private List<IAsyncPolicy> _policies;
        private const int RetryCount = 5;
        private readonly bool _sharedPolicies;
        private const int ExceptionsAllowedBeforeBreaking = 3;
        private readonly TimeSpan _overallTimeout = GetTimeout();
        private static readonly PolicyRegistry _registry = new PolicyRegistry();

        public SqlAsyncPolicyBuilder(bool sharedPolicies = false)
        {
            _sharedPolicies = sharedPolicies;
            _policies = new List<IAsyncPolicy>();
        }

        public ISqlAsyncPolicyBuilder WithDefaultPolicies()
        {
            _policies.Add(AsyncPolicies.GetTimeOutPolicy(_overallTimeout, PolicyKeys.SqlOverallTimeoutAsyncPolicy));
            _policies.Add(AsyncPolicies.GetCommonTransientErrorsPolicies(RetryCount));
            _policies.AddRange(AsyncPolicies.GetCircuitBreakerPolicies(ExceptionsAllowedBeforeBreaking));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithTransientErrors(int retryCount)
        {
            _policies.Add(AsyncPolicies.GetCommonTransientErrorsPolicies(RetryCount));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithTransaction()
        {
            _policies.Add(AsyncPolicies.GetTransactionPolicy(RetryCount));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithCircuitBreaker(int exceptionsAllowedBeforeBreaking)
        {
            _policies.AddRange(AsyncPolicies.GetCircuitBreakerPolicies(ExceptionsAllowedBeforeBreaking));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithFallback<T>(Func<Task<T>> action)
        {
            _policies.Add(AsyncPolicies.GetFallbackPolicy(action));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithFallback(Func<Task> action)
        {
            _policies.Add(AsyncPolicies.GetFallbackPolicy(action));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithOverallTimeout(TimeSpan timeout)
        {
            _policies.Add(AsyncPolicies.GetTimeOutPolicy(timeout, PolicyKeys.SqlOverallTimeoutAsyncPolicy));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithTimeoutPerRetry(TimeSpan timeout)
        {
            _policies.Add(AsyncPolicies.GetTimeOutPolicy(timeout, PolicyKeys.SqlTimeoutPerRetryAsyncPolicy));
            return this;
        }

        public ISqlAsyncPolicyBuilder WithOverallAndTimeoutPerRetry(TimeSpan overallTimeout, TimeSpan timeoutPerRetry)
        {
            _policies.Add(AsyncPolicies.GetTimeOutPolicy(_overallTimeout, PolicyKeys.SqlOverallTimeoutAsyncPolicy));
            _policies.Add(AsyncPolicies.GetTimeOutPolicy(timeoutPerRetry, PolicyKeys.SqlTimeoutPerRetryAsyncPolicy));
            return this;
        }

        public IPolicyAsyncExecutor Build()
        {
            if (!_policies.Any())
                throw new InvalidOperationException("There are no policies to execute.");

            // to prevent consumers uses WithDefaultPolicies together with other methods.
            var duplicatedPolicies = _policies.GroupBy(x => x.PolicyKey)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (duplicatedPolicies.Any())
                throw new InvalidOperationException("There are duplicated policies. When you use WithDefaultPolicies method, you can't use either WithTransientErrors. WithCircuitBreaker or WithOverallTimeout methods at the same time, because those policies are already included.");

            // if there is timeout per retry but there's not retry policy.
            var retryPolicyNames = new[] { PolicyKeys.SqlCommonTransientErrorsAsyncPolicy, PolicyKeys.SqlTransactionAsyncPolicy };
            var retryPolicies = _policies.Where(x => retryPolicyNames.Contains(x.PolicyKey));
            var timeoutPerRetryPolicy = _policies.Where(x => x.PolicyKey == PolicyKeys.SqlTimeoutPerRetryAsyncPolicy);

            if (timeoutPerRetryPolicy.Any() && !retryPolicies.Any())
                throw new InvalidOperationException("You're trying to use Timeout per retries but you don't have Retry policies configured.");

            // The order of policies into the list is important (not mandatory) in order to get a consistent resilience strategy.
            // That's why I named the policies alphabetically.
            // https://github.com/App-vNext/Polly/wiki/PolicyWrap#usage-recommendations
            _policies = _policies.OrderBy(x => x.PolicyKey).ToList();

            if (_sharedPolicies)
            {
                for (var index = 0; index < _policies.Count; index++)
                {
                    var policy = _policies[index];
                    _registry.TryGet(policy.PolicyKey, out IAsyncPolicy sharedPolicy);

                    if (sharedPolicy == null)
                    {
                        _registry.Add(policy.PolicyKey, policy);
                    }
                    else
                    {
                        // replaces new policy to the one already exists into register
                        _policies[index] = sharedPolicy;
                    }
                }
            }

            return new PolicyAsyncExecutor(_policies);
        }

        /// <summary>
        /// Gets the timeout based on retries and exponential back-off
        /// </summary>
        private static TimeSpan GetTimeout()
        {
            var retry = 1;
            var delay = TimeSpan.Zero;
            while (retry <= RetryCount)
            {
                delay += TimeSpan.FromSeconds(Math.Pow(2, retry));
                retry++;
            }

            // plus an arbitrary max time the operation could take
            return delay + TimeSpan.FromSeconds(10);
        }
    }
}
