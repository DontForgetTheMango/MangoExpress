using System;
using System.Collections.Generic;
using MangoExpressStandard.DTO;
using MangoExpressStandard.Util;
using NUnit.Framework;

namespace MangoExpressStandardUnitTests
{
    public class DeserializeTestDetailsTests
    {
        private static List<TestDetails> DetailsWithExecuteTrue
            = DeserializeTestDetails.GetTestDetails<TestDetails>($@"{AppSettings.TestDetailsDirectory}/ExecuteIsTrue.json");

        [Test, TestCaseSource(nameof(DetailsWithExecuteTrue))]
        public void TestDetailsWithExecuteTrue(TestDetails td)
        {
            //foreach (var td in DetailsWithExecuteTrue)
            Console.WriteLine(td.Name);
        }

        private static List<TestDetails> DetailsWithinFolder
            = DeserializeTestDetails.GetTestDetails<TestDetails>($@"{AppSettings.TestDetailsDirectory}/ExecuteFolder");

        [Test, TestCaseSource(nameof(DetailsWithinFolder))]
        public void TestDetailsWithinFolder(TestDetails td)
        {
            Console.WriteLine(DetailsWithinFolder.Count);

            //foreach (var td in DetailsWithinFolder)
            Console.WriteLine(td.Name);
        }

        private static List<TestDetails> DetailsWithExecuteFalse
            = DeserializeTestDetails.GetTestDetails<TestDetails>($@"{AppSettings.TestDetailsDirectory}/ExecuteIsFalse.json");

        [Test, TestCaseSource(nameof(DetailsWithExecuteFalse))]
        public void TestDetailsWithExecuteFalse(TestDetails td)
        {
            Assert.IsFalse(td.Name.Contains("False Test 1"));

            Console.WriteLine(DetailsWithExecuteFalse.Count);

            //foreach (var td in DetailsWithinFolder)
            Console.WriteLine(td.Name);
        }
    }
}
