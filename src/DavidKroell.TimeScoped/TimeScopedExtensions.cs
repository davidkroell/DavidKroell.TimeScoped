using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DavidKroell.TimeScoped
{
    public static class TimeScopedExtensions
    {
        public static IServiceCollection AddTimeScoped<TService>(this IServiceCollection sc, TimeSpan validTimeSpan)
            where TService : class
        {
            sc.TryAddTransient<TService>();

            sc.AddSingleton<ITimeScoped<TService>, TimeScopedProvider<TService>>(sp =>
                new TimeScopedProvider<TService>(sp, validTimeSpan));

            return sc;
        }

        public static IServiceCollection AddTimeScoped<TService, TImplementation>(this IServiceCollection sc,
            TimeSpan validTimeSpan) where TService : class where TImplementation : class, TService
        {
            sc.TryAddTransient<TService, TImplementation>();

            sc.AddSingleton<ITimeScoped<TService>, TimeScopedProvider<TService>>(sp =>
                new TimeScopedProvider<TService>(sp, validTimeSpan));

            return sc;
        }
    }
}