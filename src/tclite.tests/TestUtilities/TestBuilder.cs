// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Threading;
using TCLite.Framework;
using TCLite.Framework.Api;
using TCLite.Framework.Builders;
using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Commands;
using TCLite.Framework.Extensibility;
using TCLite.Framework.Internal.WorkItems;

namespace TCLite.TestUtilities
{
    /// <summary>
    /// Utility Class used to build NUnit tests for use as test data
    /// </summary>
    public class TestBuilder
    {
        private static NUnitTestFixtureBuilder fixtureBuilder = new NUnitTestFixtureBuilder();
        private static NUnitTestCaseBuilder testBuilder = new NUnitTestCaseBuilder();

        public static TestSuite MakeFixture(Type type)
        {
            return (TestSuite)fixtureBuilder.BuildFrom(type);
        }

        public static TestSuite MakeFixture(object fixture)
        {
            return (TestSuite)fixtureBuilder.BuildFrom(fixture.GetType());
        }

        public static TestSuite MakeParameterizedMethodSuite(Type type, string methodName)
        {
            return (TestSuite)MakeTestCase(type, methodName);
        }

        public static Test MakeTestCase(Type type, string methodName)
        {
            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
                Assert.Fail("Unable to find method {0} in type {1}", methodName, type.FullName);
            return testBuilder.BuildFrom(method);
        }

        public static Test MakeTestCase(object fixture, string methodName)
        {
            return MakeTestCase(fixture.GetType(), methodName);
        }

        public static TestResult RunTestFixture(Type type)
        {
            TestSuite suite = MakeFixture(type);
            
            TestExecutionContext context = new TestExecutionContext();
            context.TestObject = null;

            CompositeWorkItem work = new CompositeWorkItem(suite, TestFilter.Empty);
            return ExecuteAndWaitForResult(work, context);
        }

        public static TestResult RunTestFixture(object fixture)
        {
            TestSuite suite = MakeFixture(fixture);
            
            TestExecutionContext context = new TestExecutionContext();
            context.TestObject = fixture;

            WorkItem work = suite.CreateWorkItem(TestFilter.Empty);
            return ExecuteAndWaitForResult(work, context);
        }

        public static ITestResult RunTestCase(Type type, string methodName)
        {
            Test test = MakeTestCase(type, methodName);

            object testObject = null;
            if (!IsStaticClass(type))
                testObject = Activator.CreateInstance(type);

            return RunTest(test, testObject);
        }

        public static ITestResult RunTestCase(object fixture, string methodName)
        {
            Test test = MakeTestCase(fixture, methodName);
            return RunTest(test, fixture);
        }

        public static WorkItem RunTestCaseAsync(object fixture, string methodName)
        {
            Test test = MakeTestCase(fixture, methodName);
            return RunTestAsync(test, fixture);
        }

        public static ITestResult RunTest(Test test)
        {
            return RunTest(test, null);
        }

        public static WorkItem RunTestAsync(Test test)
        {
            return RunTestAsync(test, (object)null);
        }

        public static WorkItem RunTestAsync(Test test, object testObject)
        {
            TestExecutionContext context = new TestExecutionContext();
            context.TestObject = testObject;

            WorkItem work = test.CreateWorkItem(TestFilter.Empty);
            work.Execute(context);

            return work;
        }

        public static ITestResult RunTest(Test test, object testObject)
        {
            TestExecutionContext context = new TestExecutionContext();
            context.TestObject = testObject;

            WorkItem work = test.CreateWorkItem(TestFilter.Empty);
            return ExecuteAndWaitForResult(work, context);
        }

        private static TestResult ExecuteAndWaitForResult(WorkItem work, TestExecutionContext context)
        {
            work.Execute(context);

            // TODO: Replace with an event
            while (work.State != WorkItemState.Complete)
                Thread.Sleep(1);

            return work.Result;
        }

        private static bool IsStaticClass(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        private TestBuilder() { }
    }
}
