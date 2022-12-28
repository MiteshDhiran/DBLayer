using System;
using System.Collections.Generic;
using System.Text;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class RepositoryBaseArg
    {
        public TraceInfo Trace { get; }
        public ORMContext ORMContext { get; }
        public Action<Action> SQLRetryAction { get; }
        public SecureConnectionString SecuredConnectionString { get; }

        public Func<object> PrecompiledModelFunc { get; }
        public MDRXTransactionToken TransactionToken { get; }

        public RepositoryBaseArg(TraceInfo trace
        ,ORMContext ormContext
        ,Action<Action> sqlRetryAction
        ,SecureConnectionString securedConnectionString
        ,Func<object> precompiledModelFunc
        ): this
        (
             trace: trace,
                ormContext: ormContext,
                sqlRetryAction: sqlRetryAction,
                securedConnectionString: securedConnectionString ?? throw new ArgumentNullException($"{nameof(securedConnectionString)}"),
                transactionToken: null,
                precompiledModelFunc: precompiledModelFunc
        )
        {
            //Trace = trace ?? throw new ArgumentNullException($"{nameof(trace)}");
            //ORMContext = ormContext ?? throw new ArgumentNullException($"{nameof(ormContext)}");
            //SQLRetryAction = sqlRetryAction ?? throw new ArgumentNullException($"{nameof(sqlRetryAction)}");
            //SecuredConnectionString = securedConnectionString ?? throw new ArgumentNullException($"{nameof(securedConnectionString)}");
            //TransactionToken = null;
            //PrecompiledModelFunc = precompiledModelFunc;
        }

        public RepositoryBaseArg(TraceInfo trace
        ,ORMContext ormContext
        ,Action<Action> sqlRetryAction,
        MDRXTransactionToken transactionToken
        ,Func<object> precompiledModelFunc): this
                    (
                    trace:trace,
                    ormContext:ormContext,
                    sqlRetryAction:sqlRetryAction,
                    securedConnectionString:null,
                    transactionToken:transactionToken ?? throw new ArgumentNullException($"{nameof(transactionToken)}"),
                    precompiledModelFunc:precompiledModelFunc
                    )
        {
            //Trace = trace ?? throw new ArgumentNullException($"{nameof(trace)}");
            //ORMContext = ormContext ?? throw new ArgumentNullException($"{nameof(ormContext)}");
            //SQLRetryAction = sqlRetryAction ?? throw new ArgumentNullException($"{nameof(sqlRetryAction)}");
            //SecuredConnectionString = null;
            //TransactionToken = transactionToken ?? throw new ArgumentNullException($"{nameof(transactionToken)}");
            //PrecompiledModelFunc = precompiledModelFunc;
        }

        private RepositoryBaseArg(TraceInfo trace
        ,ORMContext ormContext
        ,Action<Action> sqlRetryAction
        ,SecureConnectionString securedConnectionString
        ,MDRXTransactionToken transactionToken
        ,Func<object> precompiledModelFunc
        )
        {
            Trace = trace ?? throw new ArgumentNullException($"{nameof(trace)}");
            ORMContext = ormContext ?? throw new ArgumentNullException($"{nameof(ormContext)}");
            SQLRetryAction = sqlRetryAction ?? throw new ArgumentNullException($"{nameof(sqlRetryAction)}");
            SecuredConnectionString = securedConnectionString;
            TransactionToken = transactionToken ;
            PrecompiledModelFunc = precompiledModelFunc;
        }
    }
}
