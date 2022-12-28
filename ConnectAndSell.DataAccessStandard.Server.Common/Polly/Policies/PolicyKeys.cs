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

namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly.Policies
{
    internal static class PolicyKeys
    {
        public const string SqlFallbackAsyncPolicy = "A.SqlFallbackAsyncPolicy";
        public const string SqlFallbackSyncPolicy = "A.SqlFallbackSyncPolicy";
        public const string SqlOverallTimeoutSyncPolicy = "B.SqlOverallTimeoutSyncPolicy";
        public const string SqlOverallTimeoutAsyncPolicy = "B.SqlOverallTimeoutAsyncPolicy";
        public const string SqlCommonTransientErrorsSyncPolicy = "C.SqlCommonTransientErrorsSyncPolicy";
        public const string SqlCommonTransientErrorsAsyncPolicy = "C.SqlCommonTransientErrorsAsyncPolicy";
        public const string SqlTransactionAsyncPolicy = "D.SqlTransactionAsyncPolicy";
        public const string SqlTransactionSyncPolicy = "D.SqlTransactionSyncPolicy";
        public const string SqlTimeoutPerRetrySyncPolicy = "E.SqlTimeoutPerRetrySyncPolicy";
        public const string SqlTimeoutPerRetryAsyncPolicy = "E.SqlTimeoutPerRetryAsyncPolicy";
        public const string SqlCircuitBreakerAsyncPolicy = "SqlCircuitBreakerAsyncPolicy";
        public const string SqlCircuitBreakerSyncPolicy = "SqlCircuitBreakerSyncPolicy";
    }
}
