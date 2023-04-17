using FluentAssertions.Execution;
using NSubstitute.Core.Arguments;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prechart.Service.Core.TestBase
{
    public static class Verify
    {
        public static T That<T>(Action<T> action) => ArgumentMatcher.Enqueue<T>(new AssertionMatcher<T>(action));

        private class AssertionMatcher<T> : IArgumentMatcher<T>
        {
            private readonly Action<T> assertion;

            public AssertionMatcher(Action<T> assertion) => this.assertion = assertion;

            public bool IsSatisfiedBy(T argument)
            {
                List<string> failures = null;
                using (var scope = new AssertionScope())
                {
                    assertion(argument);

                    failures = scope.Discard().ToList();

                    if (failures.Count == 0)
                    {
                        return true;
                    }

                    failures.ForEach(x => Trace.WriteLine($"   NOMATCH: {x}"));
                }

                throw new Exception(string.Join(",", failures));
            }
        }
    }
}
