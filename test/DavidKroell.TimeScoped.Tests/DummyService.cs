namespace DavidKroell.TimeScoped.Tests
{
    internal class DummyService : IDummyService
    {
        private int _counter;
        
        public int Inc()
        {
            _counter++;
            return _counter;
        }
    }
}