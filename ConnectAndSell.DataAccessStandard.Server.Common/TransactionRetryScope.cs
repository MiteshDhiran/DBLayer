/*
using System;
using System.Transactions;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
   public class TransactionRetryScope : IDisposable
  {
    private readonly RetryPolicyBase retryPolicy;
    private readonly TransactionRetryScope.TransactionScopeInitializer transactionScopeInit;
    private readonly Action unitOfWork;
    private TransactionScope transactionScope;
    private RetryCallback retryCallback;

    public TransactionRetryScope(Action unitOfWork)
      : this(TransactionScopeOption.Required, unitOfWork)
    {
    }

    public TransactionRetryScope(RetryPolicyBase retryPolicy, Action unitOfWork)
      : this(TransactionScopeOption.Required, retryPolicy, unitOfWork)
    {
    }

    public TransactionRetryScope(TransactionScopeOption scopeOption, Action unitOfWork)
      : this(scopeOption, RetryPolicyBase.NoRetry, unitOfWork)
    {
    }

    public TransactionRetryScope(
      TransactionScopeOption scopeOption,
      TimeSpan scopeTimeout,
      Action unitOfWork)
      : this(scopeOption, scopeTimeout, RetryPolicyBase.NoRetry, unitOfWork)
    {
    }

    public TransactionRetryScope(
      TransactionScopeOption scopeOption,
      TimeSpan scopeTimeout,
      RetryPolicyBase retryPolicy,
      Action unitOfWork)
    {
      this.transactionScopeInit = (TransactionRetryScope.TransactionScopeInitializer) (() => new TransactionScope(scopeOption, new TransactionOptions()
      {
        IsolationLevel = IsolationLevel.ReadCommitted,
        Timeout = scopeTimeout
      }));
      this.retryPolicy = Transaction.Current == (Transaction) null ? retryPolicy : RetryPolicyBase.NoRetry;
      this.transactionScope = this.transactionScopeInit();
      this.unitOfWork = unitOfWork;
      this.InitializeRetryPolicy();
    }

    public TransactionRetryScope(
      TransactionScopeOption scopeOption,
      TransactionOptions transactionOptions,
      Action unitOfWork)
      : this(scopeOption, transactionOptions, RetryPolicyBase.NoRetry, unitOfWork)
    {
    }

    public TransactionRetryScope(
      TransactionScopeOption scopeOption,
      TransactionOptions txOptions,
      RetryPolicyBase retryPolicy,
      Action unitOfWork)
    {
      this.transactionScopeInit = (TransactionRetryScope.TransactionScopeInitializer) (() => new TransactionScope(scopeOption, txOptions));
      this.retryPolicy = Transaction.Current == (Transaction) null ? retryPolicy : RetryPolicyBase.NoRetry;
      this.transactionScope = this.transactionScopeInit();
      this.unitOfWork = unitOfWork;
      this.InitializeRetryPolicy();
    }

    public TransactionRetryScope(
      TransactionScopeOption scopeOption,
      RetryPolicyBase retryPolicy,
      Action unitOfWork)
      : this(scopeOption, TimeSpan.Zero, retryPolicy, unitOfWork)
    {
    }

    public TransactionRetryScope(Transaction tx, Action unitOfWork)
      : this(tx, RetryPolicyBase.NoRetry, unitOfWork)
    {
    }

    public TransactionRetryScope(
      Transaction tx,
      RetryPolicyBase retryPolicy,
      Action unitOfWork)
    {
      this.transactionScopeInit = (TransactionRetryScope.TransactionScopeInitializer) (() => new TransactionScope(tx));
      this.retryPolicy = Transaction.Current == (Transaction) null ? retryPolicy : RetryPolicyBase.NoRetry;
      this.transactionScope = this.transactionScopeInit();
      this.unitOfWork = unitOfWork;
      this.InitializeRetryPolicy();
    }

    public TransactionRetryScope(Transaction tx, TimeSpan scopeTimeout, Action unitOfWork)
      : this(tx, scopeTimeout, RetryPolicyBase.NoRetry, unitOfWork)
    {
    }

    public TransactionRetryScope(
      Transaction tx,
      TimeSpan scopeTimeout,
      RetryPolicyBase retryPolicy,
      Action unitOfWork)
    {
      this.transactionScopeInit = (TransactionRetryScope.TransactionScopeInitializer) (() => new TransactionScope(tx, scopeTimeout));
      this.retryPolicy = Transaction.Current == (Transaction) null ? retryPolicy : RetryPolicyBase.NoRetry;
      this.transactionScope = this.transactionScopeInit();
      this.unitOfWork = unitOfWork;
      this.InitializeRetryPolicy();
    }

    public RetryPolicyBase RetryPolicy
    {
      get
      {
        return this.retryPolicy;
      }
    }

    public void InvokeUnitOfWork()
    {
      this.retryPolicy.ExecuteAction(this.unitOfWork, this.retryCallback);
    }

    public void Complete()
    {
      if (this.transactionScope == null)
        return;
      this.transactionScope.Complete();
    }

    void IDisposable.Dispose()
    {
      if (this.transactionScope == null)
        return;
      this.transactionScope.Dispose();
    }

    private void InitializeRetryPolicy()
    {
      this.retryCallback = (RetryCallback) ((currentRetryCount, lastException, delay) =>
      {
        try
        {
          if (this.retryPolicy.RetryCallback != null)
            this.retryPolicy.RetryCallback(currentRetryCount, lastException, delay);
          if (this.transactionScope != null)
            this.transactionScope.Dispose();
        }
        catch (Exception ex)
        {
          throw new RetryLimitExceededException(ex);
        }
        this.transactionScope = this.transactionScopeInit();
      });
    }

    private void InvokeUnitOfWork(Action action)
    {
      bool flag = false;
      try
      {
        this.retryPolicy.ExecuteAction(action);
        flag = true;
        this.Complete();
      }
      finally
      {
        if (!flag)
        {
          this.transactionScope.Dispose();
          this.transactionScope = (TransactionScope) null;
        }
      }
    }

    private delegate TransactionScope TransactionScopeInitializer();
  }
}
*/