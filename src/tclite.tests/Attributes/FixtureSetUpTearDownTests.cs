// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
#if !NETCF
using System.Security.Principal;
#endif
using System.Threading;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Builders;
using TCLite.TestData.FixtureSetUpTearDownData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
	[TestFixture]
	public class FixtureSetupTearDownTest
	{
		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual(1, fixture.setUpCount, "SetUp");
			Assert.AreEqual(1, fixture.tearDownCount, "TearDown");
		}

        [Test]
        public void MakeSureSetUpAndTearDownAreCalledOnExplicitFixture()
        {
            ExplicitSetUpAndTearDownFixture fixture = new ExplicitSetUpAndTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

            Assert.AreEqual(1, fixture.setUpCount, "SetUp");
            Assert.AreEqual(1, fixture.tearDownCount, "TearDown");
        }

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown fixture = new InheritSetUpAndTearDown();
            TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

        [Test]
        public static void StaticSetUpAndTearDownAreCalled()
        {
            StaticSetUpAndTearDownFixture.setUpCount = 0;
            StaticSetUpAndTearDownFixture.tearDownCount = 0;
            TestBuilder.RunTestFixture(typeof(StaticSetUpAndTearDownFixture));

            Assert.AreEqual(1, StaticSetUpAndTearDownFixture.setUpCount);
            Assert.AreEqual(1, StaticSetUpAndTearDownFixture.tearDownCount);
        }

        [Test]
        public static void StaticClassSetUpAndTearDownAreCalled()
        {
            StaticClassSetUpAndTearDownFixture.setUpCount = 0;
            StaticClassSetUpAndTearDownFixture.tearDownCount = 0;

            TestBuilder.RunTestFixture(typeof(StaticClassSetUpAndTearDownFixture));

            Assert.AreEqual(1, StaticClassSetUpAndTearDownFixture.setUpCount);
            Assert.AreEqual(1, StaticClassSetUpAndTearDownFixture.tearDownCount);
        }

        [Test]
		public void OverriddenSetUpAndTearDownAreNotCalled()
		{
            DefineInheritSetUpAndTearDown fixture = new DefineInheritSetUpAndTearDown();
            TestBuilder.RunTestFixture(fixture);

            Assert.AreEqual(0, fixture.setUpCount);
            Assert.AreEqual(0, fixture.tearDownCount);
            Assert.AreEqual(1, fixture.derivedSetUpCount);
            Assert.AreEqual(1, fixture.derivedTearDownCount);
        }

        [Test]
        public void BaseSetUpCalledFirstAndTearDownCalledLast()
        {
            DerivedSetUpAndTearDownFixture fixture = new DerivedSetUpAndTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

            Assert.AreEqual(1, fixture.setUpCount);
            Assert.AreEqual(1, fixture.tearDownCount);
            Assert.AreEqual(1, fixture.derivedSetUpCount);
            Assert.AreEqual(1, fixture.derivedTearDownCount);
            Assert.That(fixture.baseSetUpCalledFirst, "Base SetUp called first");
            Assert.That(fixture.baseTearDownCalledLast, "Base TearDown called last");
        }

        [Test]
        public void StaticBaseSetUpCalledFirstAndTearDownCalledLast()
        {
            StaticSetUpAndTearDownFixture.setUpCount = 0;
            StaticSetUpAndTearDownFixture.tearDownCount = 0;
            DerivedStaticSetUpAndTearDownFixture.derivedSetUpCount = 0;
            DerivedStaticSetUpAndTearDownFixture.derivedTearDownCount = 0;

            DerivedStaticSetUpAndTearDownFixture fixture = new DerivedStaticSetUpAndTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

            Assert.AreEqual(1, DerivedStaticSetUpAndTearDownFixture.setUpCount);
            Assert.AreEqual(1, DerivedStaticSetUpAndTearDownFixture.tearDownCount);
            Assert.AreEqual(1, DerivedStaticSetUpAndTearDownFixture.derivedSetUpCount);
            Assert.AreEqual(1, DerivedStaticSetUpAndTearDownFixture.derivedTearDownCount);
            Assert.That(DerivedStaticSetUpAndTearDownFixture.baseSetUpCalledFirst, "Base SetUp called first");
            Assert.That(DerivedStaticSetUpAndTearDownFixture.baseTearDownCalledLast, "Base TearDown called last");
        }

        [Test]
		public void HandleErrorInFixtureSetup() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInSetUp = true;
            ITestResult result = TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

			Assert.AreEqual(ResultState.Error, result.ResultState);
			Assert.AreEqual("System.Exception : This was thrown from fixture setup", result.Message, "TestSuite Message");
			Assert.NotNull(result.StackTrace, "TestSuite StackTrace should not be null");

            Assert.AreEqual(1, result.Children.Count, "Result should have one child");
            Assert.AreEqual(1, result.FailCount, "Failure count");
		}

		[Test]
		public void RerunFixtureAfterSetUpFixed() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInSetUp = true;
            ITestResult result = TestBuilder.RunTestFixture(fixture);

            Assert.AreEqual(ResultState.Error, result.ResultState);

			//fix the blow up in setup
			fixture.Reinitialize();
            result = TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

            Assert.AreEqual(ResultState.Success, result.ResultState);
		}

		[Test]
		public void HandleIgnoreInFixtureSetup() 
		{
			IgnoreInFixtureSetUp fixture = new IgnoreInFixtureSetUp();
            ITestResult result = TestBuilder.RunTestFixture(fixture);

			// should have one suite and one fixture
            Assert.AreEqual(ResultState.Ignored, result.ResultState, "Suite should be ignored");
			Assert.AreEqual("TestFixtureSetUp called Ignore", result.Message);
			Assert.NotNull(result.StackTrace, "StackTrace should not be null");

            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(1, result.SkipCount);
		}

		[Test]
		public void HandleErrorInFixtureTearDown() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInTearDown = true;
            ITestResult result = TestBuilder.RunTestFixture(fixture);
            Assert.AreEqual(1, result.Children.Count);
            Assert.AreEqual(ResultState.Error, result.ResultState);

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );

			Assert.AreEqual("TearDown : System.Exception : This was thrown from fixture teardown", result.Message);
			Assert.NotNull(result.StackTrace, "StackTrace should not be null");
		}

		[Test]
		public void HandleExceptionInFixtureConstructor()
		{
			ITestResult result = TestBuilder.RunTestFixture( typeof( ExceptionInConstructor ) );

			Assert.AreEqual(ResultState.Error, result.ResultState);
			Assert.AreEqual("System.Exception : This was thrown in constructor", result.Message, "TestSuite Message");
			Assert.NotNull(result.StackTrace, "TestSuite StackTrace should not be null");

            Assert.AreEqual(1, result.Children.Count, "Result should have one child");
            Assert.AreEqual(1, result.FailCount, "Failure count");
		}

		[Test]
		public void RerunFixtureAfterTearDownFixed() 
		{
			MisbehavingFixture fixture = new MisbehavingFixture();
			fixture.blowUpInTearDown = true;
            ITestResult result = TestBuilder.RunTestFixture(fixture);
            Assert.AreEqual(1, result.Children.Count);

			fixture.Reinitialize();
            result = TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual( 1, fixture.setUpCount, "setUpCount" );
			Assert.AreEqual( 1, fixture.tearDownCount, "tearDownCOunt" );
		}

		[Test]
		public void HandleSetUpAndTearDownWithTestInName()
		{
			SetUpAndTearDownWithTestInName fixture = new SetUpAndTearDownWithTestInName();
            TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual(1, fixture.setUpCount);
			Assert.AreEqual(1, fixture.tearDownCount);
		}

        //[Test]
        //public void RunningSingleMethodCallsSetUpAndTearDown()
        //{
        //    SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
        //    TestSuite suite = TestBuilder.MakeFixture(fixture.GetType());
        //    suite.Fixture = fixture;
        //    Test test = (Test)suite.Tests[0];

        //    suite.Run(TestListener.NULL, new NameFilter(test.TestName));

        //    Assert.AreEqual(1, fixture.setUpCount);
        //    Assert.AreEqual(1, fixture.tearDownCount);
        //}

		[Test]
		public void IgnoredFixtureShouldNotCallFixtureSetUpOrTearDown()
		{
			IgnoredFixture fixture = new IgnoredFixture();
			TestSuite suite = new TestSuite("IgnoredFixtureSuite");
			TestSuite fixtureSuite = TestBuilder.MakeFixture( fixture.GetType() );
			Test test = (Test)fixtureSuite.Tests[0];
			suite.Add( fixtureSuite );

            TestBuilder.RunTest(fixtureSuite, fixture);
			Assert.False( fixture.setupCalled, "TestFixtureSetUp called running fixture" );
			Assert.False( fixture.teardownCalled, "TestFixtureTearDown called running fixture" );

            TestBuilder.RunTest(suite, fixture);
			Assert.False( fixture.setupCalled, "TestFixtureSetUp called running enclosing suite" );
			Assert.False( fixture.teardownCalled, "TestFixtureTearDown called running enclosing suite" );

            TestBuilder.RunTest(test, fixture);
			Assert.False( fixture.setupCalled, "TestFixtureSetUp called running a test case" );
			Assert.False( fixture.teardownCalled, "TestFixtureTearDown called running a test case" );
		}

		[Test]
		public void FixtureWithNoTestsShouldCallFixtureSetUpOrTearDown()
		{
			FixtureWithNoTests fixture = new FixtureWithNoTests();

            TestBuilder.RunTestFixture(fixture);

            Assert.That( fixture.setupCalled, Is.True, "SetUp should be called for a fixture with no tests" );
            Assert.That( fixture.teardownCalled, Is.True, "TearDown should be called for a fixture with no tests" );
		}

        [Test]
        public void DisposeCalledWhenFixtureImplementsIDisposable()
        {
            DisposableFixture fixture = new DisposableFixture();
            TestBuilder.RunTestFixture(fixture);
            Assert.True(fixture.disposeCalled);
        }
	}

#if !SILVERLIGHT && !NETCF
    [TestFixture]
    class ChangesMadeInFixtureSetUp
    {
        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [Test]
        public void TestThatChangesPersistUsingSameThread()
        {
            Assert.AreEqual("foo", Thread.CurrentPrincipal.Identity.Name);
            Assert.AreEqual("en-GB", Thread.CurrentThread.CurrentCulture.Name);
            Assert.AreEqual("en-GB", Thread.CurrentThread.CurrentUICulture.Name);
        }
    }
#endif
}
