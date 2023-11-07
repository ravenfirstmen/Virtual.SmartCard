using System;

namespace Virtual.SmartCard
{
    public abstract class SmartCardResourceAware : IDisposable
    {
        protected abstract void DisposeResources();

        #region IDisposable

        private readonly object _me = new object();

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_me) // thread safety
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        DisposeResources();
                    }
                    _disposed = true;
                }
            }
        }

        ~SmartCardResourceAware()
        {
            Dispose(false);
        }

        #endregion
    }
}
