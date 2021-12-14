using System;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandard.BaseTest.ExampleTests
{
    [TestFixture]
    public class OrderedTestExample : OrderedBaseTest
    {
        public override void SetUp()
        {
            base.SetUp();

            // don't run tests if following test classes failed
            PrerequisiteTestFixtures.IgnoreIfFailed();
        }

        [Test, Order(1), Timeout(TimeoutConst.FiveMinutes)]
        public void _01_FirstTest()
        {
            var ctr = new CaptureTestResults(Driver, () =>
            {
                // test goes here
            });
            ctr.Invoke();
        }

        [Test, Order(2), Timeout(TimeoutConst.TenMinutes)]
        public void _02_SecondTest()
        {
            // if first test did not pass, this test gets ignored.
            PrerequisiteTests.IgnoreIfNotPassed("OrderedTestExample._01_FirstTest");

            var ctr = new CaptureTestResults(Driver, () =>
            {
                // test goes here
            });
            ctr.Invoke();
        }
    }
}
