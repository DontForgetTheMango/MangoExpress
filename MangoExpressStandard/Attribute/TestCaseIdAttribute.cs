using System;
namespace MangoExpressStandard.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class TestCaseIdAttribute : System.Attribute
    {


        public string TestCaseName { get; }

        public int TestCaseId { get; }

        public TestCaseIdAttribute(string testCaseName = "", int testCaseId = 0)
        {
            TestCaseName = testCaseName;
            TestCaseId = testCaseId;
        }
    }
}
