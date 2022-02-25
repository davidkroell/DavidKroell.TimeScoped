using System;

namespace DavidKroell.TimeScoped
{
    internal class TimeScopedProvider<TService> : ITimeScoped<TService> where TService : class
    {
        private readonly IServiceProvider _provider;
        private readonly TimeSpan _validTimeSpan;
        private DateTime? _validUntil;
        private TService? _instance;
        public TService Instance => GetInstance();

        public TimeScopedProvider(IServiceProvider provider, TimeSpan validTimeSpan)
        {
            _provider = provider;
            _validTimeSpan = validTimeSpan;
        }

        private TService GetInstance()
        {
            if (_validUntil == null || _validUntil < DateTime.Now)
            {
                lock (this)
                {
                    CleanupOldInstance();

                    _instance = (TService) _provider.GetService(typeof(TService))!;

                    _validUntil = DateTime.Now.Add(_validTimeSpan);
                }
            }

            return _instance!;
        }

        private void CleanupOldInstance()
        {
            if (_instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _instance = null;
        }
    }
}