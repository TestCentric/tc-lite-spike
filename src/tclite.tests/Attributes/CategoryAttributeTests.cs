// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData.CategoryAttributeData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
	/// <summary>
	/// Summary description for CategoryAttributeTests.
	/// </summary>
	public class CategoryAttributeTests
	{
		TestSuite fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestBuilder.MakeFixture( typeof( FixtureWithCategories ) );
		}

		[Test]
		public void CategoryOnFixture()
		{
			Assert.That( fixture.Properties.Contains("Category", "DataBase"));
		}

		[Test]
		public void CategoryOnTestMethod()
		{
			Test test1 = (Test)fixture.Tests[0];
			Assert.That( test1.Properties.Contains("Category", "Long") );
		}

		[Test]
		public void CanDeriveFromCategoryAttribute()
		{
			Test test2 = (Test)fixture.Tests[1];
			Assert.That(test2.Properties["Category"], Contains.Item("Critical") );
		}
		
		[Test]
		public void DerivedCategoryMayBeInherited()
		{
			Assert.That(fixture.Properties.Contains("Category", "MyCategory"));
		}

        [Test]
        public void CanSpecifyOnMethodAndTestCase()
        {
            TestSuite test3 = (TestSuite)fixture.Tests[2];
            Assert.That(test3.Name, Is.EqualTo("Test3"));
            Assert.That(test3.Properties["Category"], Contains.Item("Top"));
            Test testCase = (Test)test3.Tests[0];
            Assert.That(testCase.Name, Is.EqualTo("Test3(5)"));
            Assert.That(testCase.Properties["Category"], Contains.Item("Bottom"));
        }

        [Test]
        public void CategoryNameMayContainSpeckalCharacters()
        {
            Test test4 = (Test)fixture.Tests[3];
            Assert.That(test4.RunState, Is.EqualTo(RunState.Runnable));
        }
	}
}
