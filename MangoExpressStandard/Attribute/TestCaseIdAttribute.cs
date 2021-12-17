using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace MangoExpressStandard.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class TestCaseIdAttribute : TestCaseAspect
    {
        public override void ProcessInvocation(IInvocation invocation)
        {
            var tca = typeof(TestCaseIdAttribute);

            var attrs = invocation.MethodInvocationTarget.CustomAttributes.Where(x => x.AttributeType == tca).ToList();
            attrs.AddRange(invocation.TargetType.CustomAttributes.Where(x => x.AttributeType == tca).ToList());

            var testCases = new List<string>();
            foreach(var attr in attrs)
            {
                testCases.Add(attr.ConstructorArguments[0].Value.ToString());
            }

            var logger = Logger.MangoLogger.GetLogger();
            logger.StartTestCase(string.Join(",", testCases));
            invocation.Proceed();
        }

        public string TestCaseName { get; }

        public int TestCaseId { get; }

        public TestCaseIdAttribute(string testCaseName = "", int testCaseId = 0)
        {
            TestCaseName = testCaseName;
            TestCaseId = testCaseId;
        }
    }
}
