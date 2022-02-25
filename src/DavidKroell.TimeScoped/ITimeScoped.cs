namespace DavidKroell.TimeScoped
{
    public interface ITimeScoped<out TService> where TService : class
    {
        TService Instance { get; }
    }
}