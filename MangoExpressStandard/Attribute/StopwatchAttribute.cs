using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;

namespace MangoExpressStandard.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class StopwatchAttribute : IterceptorAspect
    {
        [DebuggerStepThrough]
        public override void ProcessInvocation(IInvocation invocation)
        {
            var className = invocation.TargetType.FullName.Split('.').Last();
            var methodName = invocation.Method.Name;

            // No one needs timing for the initializer method
            var toLogOrNotToLog = methodName != "Initialize";

            if (toLogOrNotToLog)
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    invocation.Proceed();
                    AspectLogger.Debug($"{className}.{methodName}:PASS:{stopwatch.Elapsed:mm':'ss':'fff'}");
                }
                catch (Exception ex)
                {
                    AspectLogger.Debug($"{className}.{methodName}:FAIL:{stopwatch.Elapsed:mm':'ss':'fff'}");
                    AspectLogger.Debug(ex.Message);
                }
                finally
                {
                    stopwatch.Stop();
                }
            }
            else
            {
                // method is not Initialize and does not have StopwatchAttribute assigned to it.
                invocation.Proceed();
            }
        }

        public int MaxMilliseconds { get; }

        public StopwatchAttribute(int maxMilliseconds)
        {
            MaxMilliseconds = maxMilliseconds;
        }
    }
}
