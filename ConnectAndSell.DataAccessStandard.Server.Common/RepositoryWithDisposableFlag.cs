using System;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class RepositoryWithDisposableFlag : IDisposable
    {
        public void Dispose()
        {
            if (IsDisposable)
            {
                RepositoryProvider?.Dispose();
            }
        }
        public RepositoryWithDisposableFlag(IRepositoryProvider repositoryProvider, bool isDisposable)
        {
            RepositoryProvider = repositoryProvider ?? throw new ArgumentNullException($"{nameof(repositoryProvider)} is null");
            IsDisposable = isDisposable;
        }

        public IRepositoryProvider RepositoryProvider { get; set; }
        public bool IsDisposable { get; }

        public void DisposeExplicitly()
        {
            RepositoryProvider?.Dispose();
        }
        
    }
}
