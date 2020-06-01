// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.TestData.DescriptionFixture;
using TCLite.TestUtilities;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
{
	// TODO: Review to see if we need these tests

	[TestFixture]
	public class DescriptionTests
	{
		static readonly Type FixtureType = typeof( DescriptionFixture );

		[Test]
		public void ReflectionTest()
		{
			Test testCase = TestBuilder.MakeTestCase( FixtureType, "Method" );
			Assert.AreEqual( RunState.Runnable, testCase.RunState );
		}

        [Test]
        public void Description()
        {
            Test testCase = TestBuilder.MakeTestCase(FixtureType, "Method");
            Assert.AreEqual("Test Description", testCase.Properties.Get(PropertyNames.Description));
        }

        [Test]
		public void NoDescription()
		{
			Test testCase = TestBuilder.MakeTestCase( FixtureType, "NoDescriptionMethod" );
			Assert.Null(testCase.Properties.Get(PropertyNames.Description));
		}

		[Test]
		public void FixtureDescription()
		{
			TestSuite suite = new TestSuite("suite");
			suite.Add( TestBuilder.MakeFixture( typeof( DescriptionFixture ) ) );

			TestSuite mockFixtureSuite = (TestSuite)suite.Tests[0];

			Assert.AreEqual("Fixture Description", mockFixtureSuite.Properties.Get(PropertyNames.Description));
		}

        [Test]
        public void SeparateDescriptionAttribute()
        {
            Test testCase = TestBuilder.MakeTestCase(FixtureType, "SeparateDescriptionMethod");
            Assert.AreEqual("Separate Description", testCase.Properties.Get(PropertyNames.Description));
        }

        [Test]
        public void DescriptionOnTestCase()
        {
            TestSuite parameterizedMethodSuite = TestBuilder.MakeParameterizedMethodSuite(FixtureType, "TestCaseWithDescription");
            Assert.AreEqual("method description", parameterizedMethodSuite.Properties.Get(PropertyNames.Description));
            Test testCase = (Test)parameterizedMethodSuite.Tests[0];
            Assert.AreEqual("case description", testCase.Properties.Get(PropertyNames.Description));
        }
    }
}
