using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DavidKroell.TimeScoped.Tests
{
    public class TimeScopedExtensionTests
    {
        [Test]
        public void AccessWithinValidTimespan_ReturnsSameInstance()
        {
            var sc = new ServiceCollection();

            sc.AddTimeScoped<DummyService>(TimeSpan.FromSeconds(3));

            var provider = sc.BuildServiceProvider();

            var timeScopedService = provider.GetRequiredService<ITimeScoped<DummyService>>();

            var instance1 = timeScopedService.Instance;

            Assert.AreSame(instance1, timeScopedService.Instance);
        }

        [Test]
        public async Task ExceedsValidTimespan_ReturnsDifferentInstance()
        {
            var sc = new ServiceCollection();

            sc.AddTimeScoped<DummyService>(TimeSpan.FromSeconds(3));

            var provider = sc.BuildServiceProvider();

            var timeScopedService = provider.GetRequiredService<ITimeScoped<DummyService>>();

            var instance1 = timeScopedService.Instance;

            await Task.Delay(5000);

            var instance2 = timeScopedService.Instance;

            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void RegistrationWithInterface_Works()
        {
            var sc = new ServiceCollection();

            sc.AddTimeScoped<IDummyService, DummyService>(TimeSpan.FromSeconds(3));

            var provider = sc.BuildServiceProvider();

            var timeScopedService = provider.GetRequiredService<ITimeScoped<IDummyService>>();

            var instance = timeScopedService.Instance;

            Assert.IsNotNull(instance);
            Assert.IsAssignableFrom<DummyService>(instance);
        }


        [Test]
        public async Task NewInstanceCreation_DisposesOldOne()
        {
            var sc = new ServiceCollection();

            sc.AddTimeScoped<DisposableService>(TimeSpan.FromSeconds(3));

            var provider = sc.BuildServiceProvider();

            var timeScopedService = provider.GetRequiredService<ITimeScoped<DisposableService>>();

            var instance = timeScopedService.Instance;

            Assert.IsFalse(instance.IsDisposed);

            await Task.Delay(5000);

            Assert.IsFalse(timeScopedService.Instance.IsDisposed);
            Assert.IsTrue(instance.IsDisposed);
        }
    }
}