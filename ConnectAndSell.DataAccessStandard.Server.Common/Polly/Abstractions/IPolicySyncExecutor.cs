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
using Polly;
using Polly.Registry;

namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPolicySyncExecutor
    {
        /// <summary>
        /// 
        /// </summary>
        PolicyRegistry PolicyRegistry { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Execute<T>(Func<T> action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Execute<T>(Func<Context, T> action, Context context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        void Execute(Action action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context"></param>
        void Execute(Action<Context> action, Context context);
    }
}
