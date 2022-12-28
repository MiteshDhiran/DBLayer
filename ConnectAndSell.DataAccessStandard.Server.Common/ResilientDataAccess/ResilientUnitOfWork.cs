/******************************************************************************************************
/ P r o p r i e t a r y N o t i c e                                                                   '
/ Unpublished © 2019 Allscripts Healthcare, LLC and/or its affiliates. All Rights Reserved.           '
/                                                                                                     '
/ P r o p r i e t a r y N o t i c e: This software has been provided pursuant to a License Agreement, '
/ with Allscripts Healthcare, LLC and/or its affiliates, containing restrictions on its use. This     '
/ software contains valuable trade secrets and proprietary information of Allscripts Healthcare, LLC  '
/ and/or its affiliates and is protected by trade secret and copyright law. This software may not be  '
/ copied or distributed in any form or medium, disclosed to any third parties, or used in any manner  '
/ not provided for in said License Agreement except with prior written authorization from Allscripts  '
/ Healthcare, LLC and/or its affiliates. Notice to U.S. Government Users: This software is            '
/ “Commercial Computer Software.”                                                                     '
/                                                                                                     '
/ Sunrise Clinical Manager (SCM) is a trademark of Allscripts Healthcare, LLC and/or its affiliates.  '
/ P r o p r i e t a r y N o t i c e                                                                   '
/******************************************************************************************************/

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MDRX.DataAccessStandard.Server.Common.Polly;
using MDRX.DataAccessStandard.Server.Common.Polly.Abstractions;

namespace MDRX.DataAccessStandard.Server.Common.ResilientDataAccess
{
    /// <summary>
    /// Encapsulates a unit of work with a retry policy that can be used to execute multiple DB statements inside a transaction.
    /// </summary>
    public class ResilientUnitOfWork
    {
        private readonly IPolicySyncExecutor _retryPolicy;
        private readonly IPolicyAsyncExecutor _retryPolicyAsync;

        public ResilientUnitOfWork(IPolicySyncExecutor retryPolicy = null, IPolicyAsyncExecutor asyncRetryPolicy = null)
        {
            _retryPolicy = retryPolicy ?? new SqlPolicyBuilder().UseSyncExecutor()
                               .WithOverallTimeout(TimeSpan.FromSeconds(30)).WithTransaction().WithCircuitBreaker()
                               .Build();
            _retryPolicyAsync = asyncRetryPolicy ?? new SqlPolicyBuilder().UseAsyncExecutor()
                                    .WithOverallTimeout(TimeSpan.FromSeconds(30)).WithTransaction().WithCircuitBreaker()
                                    .Build();
        }

        /// <summary>
        /// Executes the unit of work inside a transaction. If any transient SQL errors occur, the transaction is rolled back and the *entire*
        /// unit of work is retried according to the configured retry policy.
        /// This method uses a TransactionScope, so beware of things that may cause MSDTC escalation:
        /// https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/transaction-management-escalation#how-escalation-is-initiated
        /// Opening and closing the same connection within the unit of work should be fine, even multiple connections with the same exact
        /// connection string are fine, but two connections with different connection strings, or nested connections (i.e., two open connections at
        /// the same time) will cause MSDTC escalation.
        /// </summary>
        /// <typeparam name="T">The return type from the unit of work.</typeparam>
        /// <param name="unitOfWork">The unit of work to execute.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns></returns>
        public T Execute<T>(Func<T> unitOfWork, TransactionOptions? transactionOptions = null)
        {
            return _retryPolicy.Execute(() =>
            {
                using (var scope = CreateTransactionScope(transactionOptions))
                {
                    var result = unitOfWork();
                    scope.Complete(); // if an exception is thrown in unitOfWork, this will not execute and the transaction will be rolled back
                    // any SQL statements executed inside unitOfWork should be rolled back at that point
                    return result;
                }
            });
        }

        /// <summary>
        /// Executes the unit of work inside a transaction. If any transient SQL errors occur, the transaction is rolled back and the *entire*
        /// unit of work is retried according to the configured retry policy.
        /// This method uses a TransactionScope, so beware of things that may cause MSDTC escalation:
        /// https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/transaction-management-escalation#how-escalation-is-initiated
        /// Opening and closing the same connection within the unit of work should be fine, even multiple connections with the same exact
        /// connection string are fine, but two connections with different connection strings, or nested connections (i.e., two open connections at
        /// the same time) will cause MSDTC escalation.
        /// </summary>
        /// <param name="unitOfWork">The unit of work to execute.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <returns></returns>
        public void Execute(Action unitOfWork, TransactionOptions? transactionOptions = null)
        {
            _retryPolicy.Execute(() =>
            {
                using (var scope = CreateTransactionScope(transactionOptions))
                {
                    unitOfWork();
                    scope.Complete(); // if an exception is thrown in unitOfWork, this will not execute and the transaction will be rolled back
                    // any SQL statements executed inside unitOfWork should be rolled back at that point
                }
            });
        }

        /// <summary>
        /// Executes the unit of work inside a transaction. If any transient SQL errors occur, the transaction is rolled back and the *entire*
        /// unit of work is retried according to the configured retry policy.
        /// The cancellation token is used by the retry policy and passed into the unit of work as well, so cancellation is supported by both the
        /// retry logic (i.e., don't do a retry if cancellation has been requested) and by individual statements inside the unit of work.
        /// This method uses a TransactionScope, so beware of things that may cause MSDTC escalation:
        /// https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/transaction-management-escalation#how-escalation-is-initiated
        /// Opening and closing the same connection within the unit of work should be fine, even multiple connections with the same exact
        /// connection string are fine, but two connections with different connection strings, or nested connections (i.e., two open connections at
        /// the same time) will cause MSDTC escalation.
        /// </summary>
        /// <typeparam name="T">The return type from the unit of work.</typeparam>
        /// <param name="unitOfWorkAsync">The unit of work to execute.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <param name="cancellationToken">The cancellation token to be passed into the unit of work.</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> unitOfWorkAsync,
            TransactionOptions? transactionOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _retryPolicyAsync.ExecuteAsync(async token =>
            {
                using (var scope = CreateTransactionScope(transactionOptions))
                {
                    var result = await unitOfWorkAsync(token);
                    scope.Complete(); // if an exception is thrown in unitOfWorkAsync, this will not execute and the transaction will be rolled back
                    // any SQL statements executed inside unitOfWorkAsync should be rolled back at that point
                    return result;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Executes the unit of work inside a transaction. If any transient SQL errors occur, the transaction is rolled back and the *entire*
        /// unit of work is retried according to the configured retry policy.
        /// The cancellation token is used by the retry policy and passed into the unit of work as well, so cancellation is supported by both the
        /// retry logic (i.e., don't do a retry if cancellation has been requested) and by individual statements inside the unit of work.
        /// This method uses a TransactionScope, so beware of things that may cause MSDTC escalation:
        /// https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/transaction-management-escalation#how-escalation-is-initiated
        /// Opening and closing the same connection within the unit of work should be fine, even multiple connections with the same exact
        /// connection string are fine, but two connections with different connection strings, or nested connections (i.e., two open connections at
        /// the same time) will cause MSDTC escalation.
        /// </summary>
        /// <typeparam name="T">The return type from the unit of work.</typeparam>
        /// <param name="unitOfWorkAsync">The unit of work to execute.</param>
        /// <param name="transactionOptions">The transaction options.</param>
        /// <param name="cancellationToken">The cancellation token to be passed into the unit of work.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<CancellationToken, Task> unitOfWorkAsync,
            TransactionOptions? transactionOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _retryPolicyAsync.ExecuteAsync(async token =>
            {
                using (var scope = CreateTransactionScope(transactionOptions))
                {
                    await unitOfWorkAsync(token);
                    scope.Complete(); // if an exception is thrown in unitOfWorkAsync, this will not execute and the transaction will be rolled back
                    // any SQL statements executed inside unitOfWorkAsync should be rolled back at that point
                }
            }, cancellationToken);
        }

        private TransactionScope CreateTransactionScope(TransactionOptions? transactionOptions)
        {
            var options = transactionOptions ?? new TransactionOptions
                              { IsolationLevel = IsolationLevel.ReadCommitted };
            return new TransactionScope(TransactionScopeOption.Required,
                options, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
