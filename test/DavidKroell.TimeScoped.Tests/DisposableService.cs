using System;

namespace DavidKroell.TimeScoped.Tests
{
    internal class DisposableService : IDisposable
    {
        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }
    }
}