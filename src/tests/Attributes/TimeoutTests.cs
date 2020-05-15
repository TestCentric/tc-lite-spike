// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Framework;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
    public class TimeoutTests
    {
        Thread parentThread;
        Thread setupThread;

        [TestFixtureSetUp]
        public void GetParentThreadInfo()
        {
			this.parentThread = Thread.CurrentThread;
        }

        [SetUp]
        public void GetSetUpThreadInfo()
        {
            this.setupThread = Thread.CurrentThread;
        }

        [Test, Timeout(50)]
        public void TestWithTimeoutRunsOnSeparateThread()
        {
            Assert.That(Thread.CurrentThread, Is.Not.EqualTo(parentThread));
        }

        [Test, Timeout(50)]
        public void TestWithTimeoutRunsSetUpAndTestOnSameThread()
        {
            Assert.That(Thread.CurrentThread, Is.EqualTo(setupThread));
        }

#if NYI
        [Test]
        [Platform(Exclude = "Mono", Reason = "Runner hangs at end when this is run")]
        [Platform(Exclude = "Net-1.1,Net-1.0", Reason = "Cancels the run when executed")]
        public void TestWithInfiniteLoopTimesOut()
        {
            TimeoutFixture fixture = new TimeoutFixture();
            TestSuite suite = TestBuilder.MakeFixture(fixture);
            Test test = TestFinder.Find("InfiniteLoopWith50msTimeout", suite, false);
            ITestResult result = TestBuilder.RunTest(test, fixture);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Contains.Substring("50ms"));
            Assert.That(fixture.TearDownWasRun, "TearDown was not run");
        }

        [Test]
        [Platform(Exclude = "Mono", Reason = "Runner hangs at end when this is run")]
        public void TimeoutCanBeSetOnTestFixture()
        {
            TestResult suiteResult = TestBuilder.RunTestFixture(typeof(ThreadingFixtureWithTimeout));
            Assert.That(suiteResult.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(suiteResult.Message, Is.EqualTo("One or more child tests had errors"));
            ITestResult result = TestFinder.Find("Test2WithInfiniteLoop", suiteResult, false);
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Failure));
            Assert.That(result.Message, Contains.Substring("50ms"));
        }
#endif
    }
}
