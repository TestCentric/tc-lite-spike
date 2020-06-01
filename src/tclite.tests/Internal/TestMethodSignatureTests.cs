// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Api;
using TCLite.TestData.TestMethodSignatureFixture;
using TCLite.TestUtilities;

namespace TCLite.Framework.Internal
{
	[TestFixture]
	public class TestMethodSignatureTests
	{
        private static Type fixtureType = typeof(TestMethodSignatureFixture);

        [Test]
		public void InstanceTestMethodIsRunnable()
		{
            TestAssert.IsRunnable(fixtureType, "InstanceTestMethod", ResultState.Success);
		}

		[Test]
		public void StaticTestMethodIsRunnable()
		{
            TestAssert.IsRunnable(fixtureType, "StaticTestMethod", ResultState.Success);
		}

		[Test]
		public void TestMethodWithoutParametersWithArgumentsProvidedIsNotRunnable()
		{
			TestAssert.FirstChildIsNotRunnable(fixtureType, "TestMethodWithoutParametersWithArgumentsProvided");
		}

        [Test]
        public void TestMethodWithArgumentsNotProvidedIsNotRunnable()
        {
            TestAssert.IsNotRunnable(fixtureType, "TestMethodWithArgumentsNotProvided");
        }

        [Test]
        public void TestMethodWithArgumentsProvidedIsRunnable()
        {
            TestAssert.IsRunnable(fixtureType, "TestMethodWithArgumentsProvided", ResultState.Success);
        }

        [Test]
        public void TestMethodWithWrongNumberOfArgumentsProvidedIsNotRunnable()
        {
            TestAssert.FirstChildIsNotRunnable(fixtureType, "TestMethodWithWrongNumberOfArgumentsProvided");
        }

        [Test]
        public void TestMethodWithWrongArgumentTypesProvidedGivesError()
        {
            TestAssert.IsRunnable(fixtureType, "TestMethodWithWrongArgumentTypesProvided", ResultState.Error);
        }

        [Test]
        public void StaticTestMethodWithArgumentsNotProvidedIsNotRunnable()
        {
            TestAssert.IsNotRunnable(fixtureType, "StaticTestMethodWithArgumentsNotProvided");
        }

        [Test]
        public void StaticTestMethodWithArgumentsProvidedIsRunnable()
        {
            TestAssert.IsRunnable(fixtureType, "StaticTestMethodWithArgumentsProvided", ResultState.Success);
        }

        [Test]
        public void StaticTestMethodWithWrongNumberOfArgumentsProvidedIsNotRunnable()
        {
            TestAssert.FirstChildIsNotRunnable(fixtureType, "StaticTestMethodWithWrongNumberOfArgumentsProvided");
        }

        [Test]
        public void StaticTestMethodWithWrongArgumentTypesProvidedGivesError()
        {
            TestAssert.IsRunnable(fixtureType, "StaticTestMethodWithWrongArgumentTypesProvided", ResultState.Error);
        }

        [Test]
        public void TestMethodWithConvertibleArgumentsIsRunnable()
        {
            TestAssert.IsRunnable(fixtureType, "TestMethodWithConvertibleArguments", ResultState.Success);
        }

        [Test]
        public void TestMethodWithNonConvertibleArgumentsGivesError()
        {
            TestAssert.IsRunnable(fixtureType, "TestMethodWithNonConvertibleArguments", ResultState.Error);
        }

        [Test]
		public void ProtectedTestMethodIsNotRunnable()
		{
			TestAssert.IsNotRunnable( fixtureType, "ProtectedTestMethod" );
		}

		[Test]
		public void PrivateTestMethodIsNotRunnable()
		{
			TestAssert.IsNotRunnable( fixtureType, "PrivateTestMethod" );
		}

		[Test]
		public void TestMethodWithReturnTypeIsNotRunnable()
		{
			TestAssert.IsNotRunnable( fixtureType, "TestMethodWithReturnType" );
		}

		[Test]
		public void TestMethodWithMultipleTestCasesExecutesMultipleTimes()
		{
            ITestResult result = TestBuilder.RunTestCase(fixtureType, "TestMethodWithMultipleTestCases");

			Assert.That( result.ResultState, Is.EqualTo(ResultState.Success) );
            ResultSummary summary = new ResultSummary(result);
            Assert.That(summary.TestsRun, Is.EqualTo(3));
		}

        [Test]
        public void TestMethodWithMultipleTestCasesUsesCorrectNames()
        {
            string name = "TestMethodWithMultipleTestCases";
            string fullName = typeof (TestMethodSignatureFixture).FullName + "." + name;

            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(fixtureType, name);
            Assert.That(suite.TestCaseCount, Is.EqualTo(3));

            string[] names = new string[suite.Tests.Count];
            string[] fullNames = new string[suite.Tests.Count];

            int index = 0;
            foreach (Test test in suite.Tests)
            {
                names[index] = test.Name;
                fullNames[index] = test.FullName;
                index++;
            }

            Assert.That(names, Has.Member(name + "(12,3,4)"));
            Assert.That(names, Has.Member(name + "(12,2,6)"));
            Assert.That(names, Has.Member(name + "(12,4,3)"));

            Assert.That(fullNames, Has.Member(fullName + "(12,3,4)"));
            Assert.That(fullNames, Has.Member(fullName + "(12,2,6)"));
            Assert.That(fullNames, Has.Member(fullName + "(12,4,3)"));
        }

        [Test]
        public void RunningTestsThroughFixtureGivesCorrectResults()
        {
            ITestResult result = TestBuilder.RunTestFixture(fixtureType);
            ResultSummary summary = new ResultSummary(result);

            Assert.That(
                summary.ResultCount,
                Is.EqualTo(TestMethodSignatureFixture.Tests));
            Assert.That(
                summary.TestsRun,
                Is.EqualTo(TestMethodSignatureFixture.Runnable));
            //Assert.That(
            //    summary.NotRunnable,
            //    Is.EqualTo(TestMethodSignatureFixture.NotRunnable));
            //Assert.That(
            //    summary.Errors,
            //    Is.EqualTo(TestMethodSignatureFixture.Errors));
            Assert.That(
                summary.Failures,
                Is.EqualTo(TestMethodSignatureFixture.Failures + TestMethodSignatureFixture.Errors));
            Assert.That(
                summary.TestsNotRun,
                Is.EqualTo(TestMethodSignatureFixture.NotRunnable));
        }
    }
}
