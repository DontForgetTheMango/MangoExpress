using System;
using MangoExpressStandard.BaseTest;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    [TestFixture]
    public class OrderedTests : OrderedBaseTest
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
                Assert.Fail();
            });
            ctr.Invoke();
        }

        /// <summary>
        /// Will not run dueto PrerequisiteTests
        /// </summary>
        [Test, Order(2), Timeout(TimeoutConst.TenMinutes)]
        public void _02_SecondTest()
        {
            // if first test did not pass, this test gets ignored.
            PrerequisiteTests.IgnoreIfNotPassed("MangoExpressStandardUnitTests.OrderedTests._01_FirstTest");

            var ctr = new CaptureTestResults(Driver, () =>
            {
                // test goes here
            });
            ctr.Invoke();
        }

        /// <summary>
        /// Will not run because previous test failed
        /// </summary>
        [Test, Order(3), Timeout(TimeoutConst.TenMinutes)]
        public void _03_ThirdTest()
        {
            var ctr = new CaptureTestResults(Driver, () =>
            {
                // test goes here
            });
            ctr.Invoke();
        }
    }
}
