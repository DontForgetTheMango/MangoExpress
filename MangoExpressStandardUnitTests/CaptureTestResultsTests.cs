using System;
using MangoExpressStandard.BaseTest;
using MangoExpressStandard.DTO;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    public class CaptureTestResultsTests : StandardBaseTest
    {
        [Test]
        public void CtrTestIgnore()
        {
            TestDetails tdFailure = new TestDetails
            {
                Name = "I should Ignore",
                UponFailure = TestDetails.TestFailureEnum.Ignore
            };

            var ctr = new CaptureTestResults(Driver, tdFailure, () =>
            {
                Assert.Fail();
            });
            ctr.Invoke();
        }


        [Test]
        public void CtrTestWarn()
        {
            TestDetails tdFailure = new TestDetails
            {
                Name = "I should Warn",
                UponFailure = TestDetails.TestFailureEnum.Warn
            };

            var ctr = new CaptureTestResults(Driver, tdFailure, () =>
            {
                Assert.Fail();
            });
            ctr.Invoke();
        }


        [Test]
        public void CtrTestFail()
        {
            TestDetails tdFailure = new TestDetails
            {
                Name = "I should Fail",
                UponFailure = TestDetails.TestFailureEnum.Fail
            };

            var ctr = new CaptureTestResults(Driver, tdFailure, () =>
            {
                Assert.Fail();
            });
            ctr.Invoke();
        }
    }
}
