using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Registry;

namespace ConnectAndSell.DataAccessStandard.Server.Common.Polly.Abstractions
{
    /// <summary>
    /// Polly doesn't support async void methods. So you can't pass an Action.
    /// http://www.thepollyproject.org/2017/06/09/polly-and-synchronous-versus-asynchronous-policies/
    /// Returns a Task in order to avoid execution of void methods asynchronously, which causes unexpected out-of-sequence execution of policy hooks and continuing policy actions, and a risk of unobserved exceptions.
    /// https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
    /// https://github.com/App-vNext/Polly/issues/107#issuecomment-218835218
    /// </summary>
    public interface IPolicyAsyncExecutor
    {
        /// <summary>
        /// PolicyRegistry Interface.  This holds all the policies
        /// </summary>
        PolicyRegistry PolicyRegistry { get; set; }

        /// <summary>
        /// ExecuteAsync Interface
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <typeparam name="T">Generic Class</typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);

        /// <summary>
        /// ExecuteAsync Interface with context
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="context">Polly Context </param>
        /// <typeparam name="T">Generic Class</typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<Context, Task<T>> action, Context context);

        /// <summary>
        /// ExecuteAsync Interface 
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<Task> action);

        /// <summary>
        /// ExecuteAsync Interface with context
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="context">Polly Context</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<Context, Task> action, Context context);

        /// <summary>
        /// ExecuteAsync Interface with cancellation Token
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<CancellationToken, Task> action,CancellationToken cancellationToken);

        /// <summary>
        /// ExecuteAsync Interface with cancellation Token
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Generic Class</typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken);

        /// <summary>
        /// ExecuteAsync Interface with context and  cancellation Token
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="context">Polly Context </param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <typeparam name="T">Generic Class</typeparam>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(Func<Context,CancellationToken, Task<T>> action, Context context, CancellationToken cancellationToken);

        /// <summary>
        ///  ExecuteAsync Interface with context and  cancellation Token
        /// </summary>
        /// <param name="action">Action to be execute</param>
        /// <param name="context">Polly Context </param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        Task ExecuteAsync(Func<Context, CancellationToken,Task> action, Context context, CancellationToken cancellationToken);
    }
}
