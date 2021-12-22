using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using MangoExpressStandard.Util;

namespace MangoExpressStandard.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class TestCaseRetryAttribute : IterceptorAspect
    {
        [DebuggerStepThrough]
        public override void ProcessInvocation(IInvocation invocation)
        {
            bool didThrow = false;
            var i = 0;

            do
            {
                didThrow = CatchNUnitAssertions.DoesCodeAssert(() =>
                {
                    invocation.Proceed();
                });

                i++;
            } while (didThrow && i < RetryCount);

            if (didThrow)
                invocation.Proceed();

            AspectLogger.Debug($"TestCaseRetryAttribute retried {i} times.");
        }

        public TestCaseRetryAttribute(int retryCount = 1)
        {
            RetryCount = retryCount;
        }

        public int RetryCount { get; }
    }
}
