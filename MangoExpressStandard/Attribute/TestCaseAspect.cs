using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;

namespace MangoExpressStandard.Attribute
{
    public abstract class TestCaseAspect : System.Attribute, IInterceptor
    {
        [DebuggerStepThrough]
        public void Intercept(IInvocation invocation)
        {
            var thisType = GetType();

            if (!CanIntercept(invocation, thisType))
            {
                invocation.Proceed();
                return;
            }

            ProcessInvocation(invocation);
        }

        [DebuggerStepThrough]
        public bool CanIntercept(IInvocation invocation, Type type)
        {
            return invocation.TargetType.CustomAttributes.Any(x => x.AttributeType == type) ||
                invocation.MethodInvocationTarget.CustomAttributes.Any(x => x.AttributeType == type) ||
                invocation.TargetType.CustomAttributes.Any(x => x.AttributeType == type);
        }

        public abstract void ProcessInvocation(IInvocation invocation);
    }
}
