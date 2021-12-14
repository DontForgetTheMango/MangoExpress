using System;
using System.Collections.Generic;
using MangoExpressStandard.DTO;
using MangoExpressStandard.Factory;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandard.BaseTest.ExampleTests
{
    [TestFixture]
    public class StandardTestExample : StandardBaseTest
    {
        [Test]
        public void FirstTest()
        {
            var ctr = new CaptureTestResults(Driver, () =>
            {
                var testPageA = PomFactory.GetTestPageA(Driver);
                testPageA.ClickTestButton();
                // test goes here
            });
            ctr.Invoke();
        }

        // list of scenarios to be iterated through
        private static List<TestDetails> testDetailsList
            = DeserializeTestDetails.GetTestDetails<TestDetails>($@"{AssemblyPath}\path\to\json\tests");

        [Test, TestCaseSource(nameof(testDetailsList))]
        public void SecondTest(TestDetails testDetails)
        {
            var ctr = new CaptureTestResults(Driver, testDetails, () =>
            {
                // test goes here
            });
            ctr.Invoke();
        }
    }
}
