// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.TestUtilities
{
    public class TestAssert
    {
        #region IsRunnable

        /// <summary>
        /// Verify that a test is runnable
        /// </summary>
        public static void IsRunnable(Test test)
        {
            Assert.AreEqual(RunState.Runnable, test.RunState);
        }

        /// <summary>
        /// Verify that the first child test is runnable
        /// </summary>
        public static void FirstChildIsRunnable(Test test)
        {
            IsRunnable((Test)test.Tests[0]);
        }

        /// <summary>
        /// Verify that a Type can be used to create a
        /// runnable fixture
        /// </summary>
        public static void IsRunnable(Type type)
        {
            TestSuite suite = TestBuilder.MakeFixture(type);
            Assert.NotNull(suite, "Unable to construct fixture");
            Assert.AreEqual(RunState.Runnable, suite.RunState);
        }

        /// <summary>
        /// Verify that a Type is runnable, then run it and
        /// verify the result.
        /// </summary>
        public static void IsRunnable(Type type, ResultState resultState)
        {
            TestSuite suite = TestBuilder.MakeFixture(type);
            Assert.NotNull(suite, "Unable to construct fixture");
            Assert.AreEqual(RunState.Runnable, suite.RunState);
            ITestResult result = TestBuilder.RunTest(suite);
            Assert.AreEqual(resultState, result.ResultState);
        }

        /// <summary>
        /// Verify that a named test method is runnable
        /// </summary>
        public static void IsRunnable(Type type, string name)
        {
            Test test = TestBuilder.MakeTestCase(type, name);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        /// <summary>
        /// Verify that the first child (usually a test case)
        /// of a named test method is runnable
        /// </summary>
        public static void FirstChildIsRunnable(Type type, string name)
        {
            Test suite = TestBuilder.MakeTestCase(type, name);
            TestAssert.FirstChildIsRunnable(suite);
        }

        /// <summary>
        /// Verify that a named test method is runnable, then
        /// run it and verify the result.
        /// </summary>
        public static void IsRunnable(Type type, string name, ResultState resultState)
        {
            Test test = TestBuilder.MakeTestCase(type, name);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
            object testObject = Activator.CreateInstance(type);
            ITestResult result = TestBuilder.RunTest(test, testObject);
            if (result.HasChildren) // In case it's a parameterized method
                result = (ITestResult)result.Children[0];
            Assert.That(result.ResultState, Is.EqualTo(resultState));
        }

        #endregion

        #region IsNotRunnable
        public static void IsNotRunnable(Test test)
        {
            Assert.AreEqual(RunState.NotRunnable, test.RunState);
            ITestResult result = TestBuilder.RunTest(test, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
        }

        public static void IsNotRunnable(Type type)
        {
            TestSuite fixture = TestBuilder.MakeFixture(type);
            Assert.NotNull(fixture, "Unable to construct fixture");
            IsNotRunnable(fixture);
        }

        public static void IsNotRunnable(Type type, string name)
        {
            IsNotRunnable(TestBuilder.MakeTestCase(type, name));
        }

        public static void FirstChildIsNotRunnable(Test suite)
        {
            IsNotRunnable((Test)suite.Tests[0]);
        }

        public static void FirstChildIsNotRunnable(Type type, string name)
        {
            FirstChildIsNotRunnable(TestBuilder.MakeParameterizedMethodSuite(type, name));
        }
        #endregion

        private TestAssert() { }
    }
}
