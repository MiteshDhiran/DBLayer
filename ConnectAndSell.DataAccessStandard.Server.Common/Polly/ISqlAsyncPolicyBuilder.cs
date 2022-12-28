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
using System.Threading.Tasks;
using ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions;


namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly
{
    public interface ISqlAsyncPolicyBuilder
    {
        ISqlAsyncPolicyBuilder WithDefaultPolicies();

        ISqlAsyncPolicyBuilder WithFallback<T>(Func<Task<T>> action);

        ISqlAsyncPolicyBuilder WithFallback(Func<Task> action);

        ISqlAsyncPolicyBuilder WithOverallTimeout(TimeSpan timeout);

        ISqlAsyncPolicyBuilder WithTimeoutPerRetry(TimeSpan timeout);

        ISqlAsyncPolicyBuilder WithOverallAndTimeoutPerRetry(TimeSpan overallTimeout, TimeSpan timeoutPerRetry);

        ISqlAsyncPolicyBuilder WithTransientErrors(int retryCount = 4);

        ISqlAsyncPolicyBuilder WithTransaction();

        ISqlAsyncPolicyBuilder WithCircuitBreaker(int exceptionsAllowedBeforeBreaking = 3);

        IPolicyAsyncExecutor Build();
    }

    public interface ISqlSyncPolicyBuilder
    {
        ISqlSyncPolicyBuilder WithDefaultPolicies();

        ISqlSyncPolicyBuilder WithFallback<T>(Func<T> action);

        ISqlSyncPolicyBuilder WithFallback(Action action);

        ISqlSyncPolicyBuilder WithOverallTimeout(TimeSpan timeout);

        ISqlSyncPolicyBuilder WithTimeoutPerRetry(TimeSpan timeout);

        ISqlSyncPolicyBuilder WithOverallAndTimeoutPerRetry(TimeSpan overallTimeout, TimeSpan timeoutPerRetry);

        ISqlSyncPolicyBuilder WithTransientErrors(int retryCount = 4);

        ISqlSyncPolicyBuilder WithTransaction();

        ISqlSyncPolicyBuilder WithCircuitBreaker(int exceptionsAllowedBeforeBreaking = 3);

        IPolicySyncExecutor Build();
    }
}
