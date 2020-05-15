// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************
#if false
using System;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData.RepeatedTestFixture;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
    [TestFixture]
    public class RepeatedTestTests
	{
		private MethodInfo successMethod;
		private MethodInfo failOnFirstMethod;
		private MethodInfo failOnThirdMethod;

		[SetUp]
		public void SetUp()
		{
			Type testType = typeof(RepeatSuccessFixture);
			successMethod = testType.GetMethod ("RepeatSuccess");
			testType = typeof(RepeatFailOnFirstFixture);
			failOnFirstMethod = testType.GetMethod("RepeatFailOnFirst");
			testType = typeof(RepeatFailOnThirdFixture);
			failOnThirdMethod = testType.GetMethod("RepeatFailOnThird");
		}

		[Test]
		public void RepeatSuccess()
		{
			Assert.IsNotNull (successMethod);
			RepeatSuccessFixture fixture = new RepeatSuccessFixture();
            ITestResult result = TestBuilder.RunTestFixture(fixture);

            Assert.IsTrue(result.ResultState == ResultState.Success);
			Assert.AreEqual(1, fixture.FixtureSetupCount);
			Assert.AreEqual(1, fixture.FixtureTeardownCount);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void RepeatFailOnFirst()
		{
			Assert.IsNotNull (failOnFirstMethod);
			RepeatFailOnFirstFixture fixture = new RepeatFailOnFirstFixture();
            ITestResult result = TestBuilder.RunTestFixture(fixture);

            Assert.IsFalse(result.ResultState == ResultState.Success);
			Assert.AreEqual(1, fixture.SetupCount);
			Assert.AreEqual(1, fixture.TeardownCount);
			Assert.AreEqual(1, fixture.Count);
		}

		[Test]
		public void RepeatFailOnThird()
		{
			Assert.IsNotNull (failOnThirdMethod);
			RepeatFailOnThirdFixture fixture = new RepeatFailOnThirdFixture();
            ITestResult result = TestBuilder.RunTestFixture(fixture);

            Assert.IsFalse(result.ResultState == ResultState.Success);
			Assert.AreEqual(3, fixture.SetupCount);
			Assert.AreEqual(3, fixture.TeardownCount);
			Assert.AreEqual(3, fixture.Count);
		}

		[Test]
		public void IgnoreWorksWithRepeatedTest()
		{
			RepeatedTestWithIgnore fixture = new RepeatedTestWithIgnore();
            TestBuilder.RunTestFixture(fixture);

			Assert.AreEqual( 0, fixture.SetupCount );
			Assert.AreEqual( 0, fixture.TeardownCount );
			Assert.AreEqual( 0, fixture.Count );
		}

        [Test]
        public void CategoryWorksWithRepeatedTest()
        {
            TestSuite suite = TestBuilder.MakeFixture(typeof(RepeatedTestWithCategory));
            Test test = suite.Tests[0] as Test;
            System.Collections.IList categories = test.Properties["Category"];
            Assert.IsNotNull(categories);
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual("SAMPLE", categories[0]);
        }
	}
}
#endif