using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Prechart.Service.Core.TestBase
{
    public abstract class WithSubject<TTestSubject> : IDisposable
        where TTestSubject : class
    {
        public TTestSubject Subject { get; set; }

        protected WithSubject() => Init();

        private void Init()
        {
            Setup();
            When();
        }

        public TSubstituteType An<TSubstituteType>()
            where TSubstituteType : class => Substitute.For<TSubstituteType>();

        protected async Task<TAwaitType> WithTimeout<TAwaitType>(Task<TAwaitType> task, int timeout = 1000)
        {
            if (await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false) == task)
            {
#pragma warning disable VSTHRD103
                return task.Result;
#pragma warning restore VSTHRD103
            }
            else
            {
                throw new TimeoutException();
            }
        }

        private void ExecuteDelegates(params Type[] types)
        {
            var properties = new List<PropertyInfo>();
            var type = GetType();
            while (type != null)
            {
                properties.AddRange(type
                    .GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(p => types.Contains(p.PropertyType))
                    .Reverse());
                type = type.BaseType;
            }

            properties.Reverse();
            foreach (var p in properties)
            {
                if (p.GetValue(this) is System.Delegate func && func.DynamicInvoke() is Task result)
                {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                    result.GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                }
            }
        }

        private void Setup() => ExecuteDelegates(typeof(Given), typeof(And));

        private void When() => ExecuteDelegates(typeof(When));

        private void Teardown() => ExecuteDelegates(typeof(Teardown));

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Teardown();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
