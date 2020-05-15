// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Tests
{
	/// <summary>
	/// Tests for MaxTime decoration.
	/// </summary>
    [TestFixture]
    public class MaxTimeTests
	{
		[Test,MaxTime(1000)]
		public void MaxTimeNotExceeded()
		{
		}

        // TODO: We need a way to simulate the clock reliably
        [Test]
        public void MaxTimeExceeded()
        {
            ITestResult suiteResult = TestBuilder.RunTestFixture(typeof(MaxTimeFixture));
            Assert.AreEqual(ResultState.Failure, suiteResult.ResultState);
            TestResult result = (TestResult)suiteResult.Children[0];
            Assert.That(result.Message, Contains.Substring("exceeds maximum of 1ms"));
        }

        [Test]
        public void FailureReportHasPriorityOverMaxTime()
		{
            ITestResult result = TestBuilder.RunTestFixture(typeof(MaxTimeFixtureWithFailure));
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            result = (TestResult)result.Children[0];
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            Assert.That(result.Message, Is.EqualTo("Intentional Failure"));
        }

        [Test]
        public void ErrorReportHasPriorityOverMaxTime()
        {
            ITestResult result = TestBuilder.RunTestFixture(typeof(MaxTimeFixtureWithError));
            Assert.AreEqual(ResultState.Failure, result.ResultState);
            result = (ITestResult)result.Children[0];
            Assert.AreEqual(ResultState.Error, result.ResultState);
            Assert.That(result.Message, Contains.Substring("Exception message"));
        }
    }
}
