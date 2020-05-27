// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Commands
{
    /// <summary>
    /// OneTimeTearDownCommand performs any teardown actions
    /// specified for a suite and calls Dispose on the user
    /// test object, if any.
    /// </summary>
    public class OneTimeTearDownCommand : TestCommand
    {
        private readonly TestSuite suite;
        private readonly Type fixtureType;

        /// <summary>
        /// Construct a OneTimeTearDownCommand
        /// </summary>
        /// <param name="suite">The test suite to which the command applies</param>
        public OneTimeTearDownCommand(TestSuite suite)
            : base(suite)
        {
            this.suite = suite;
            this.fixtureType = suite.FixtureType;
        }

        /// <summary>
        /// Overridden to run the teardown methods specified on the test.
        /// </summary>
        /// <param name="context">The TestExecutionContext to be used.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            TestResult suiteResult = context.CurrentResult;

            if (fixtureType != null)
            {
                MethodInfo[] teardownMethods =
                    Reflect.GetMethodsWithAttribute(fixtureType, typeof(TestFixtureTearDownAttribute), true);

                try
                {
                    int index = teardownMethods.Length;
                    while (--index >= 0)
                    {
                        MethodInfo fixtureTearDown = teardownMethods[index];
                        if (!fixtureTearDown.IsStatic && context.TestObject == null)
                            Console.WriteLine("TestObject should not be null!!!");
                        Reflect.InvokeMethod(fixtureTearDown, fixtureTearDown.IsStatic ? null : context.TestObject);
                    }

                    IDisposable disposable = context.TestObject as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
                catch (Exception ex)
                {
                    // Error in TestFixtureTearDown or Dispose causes the
                    // suite to be marked as a error, even if
                    // all the contained tests passed.
                    TCLiteException nex = ex as TCLiteException;
                    if (nex != null)
                        ex = nex.InnerException;

                    // TODO: Can we move this logic into TestResult itself?
                    string message = "TearDown : " + ExceptionHelper.BuildMessage(ex);
                    if (suiteResult.Message != null)
                        message = suiteResult.Message + Environment.NewLine + message;

                    string stackTrace = "--TearDown" + Environment.NewLine + ExceptionHelper.BuildStackTrace(ex);
                    if (suiteResult.StackTrace != null)
                        stackTrace = suiteResult.StackTrace + Environment.NewLine + stackTrace;

                    // TODO: What about ignore exceptions in teardown?
                    suiteResult.SetResult(ResultState.Error, message, stackTrace);
                }
            }

            return suiteResult;
        }
    }
}
