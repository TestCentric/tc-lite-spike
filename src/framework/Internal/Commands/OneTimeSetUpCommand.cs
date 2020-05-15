// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Framework.Internal.Commands
{
    /// <summary>
    /// OneTimeSetUpCommand runs any one-time setup methods for a suite,
    /// constructing the user test object if necessary.
    /// </summary>
    public class OneTimeSetUpCommand : TestCommand
    {
        private readonly TestSuite suite;
        private readonly Type fixtureType;
        private readonly object[] arguments;

        /// <summary>
        /// Constructs a OneTimeSetUpComand for a suite
        /// </summary>
        /// <param name="suite">The suite to which the command applies</param>
        public OneTimeSetUpCommand(TestSuite suite) : base(suite) 
        {
            this.suite = suite;
            this.fixtureType = suite.FixtureType;
            this.arguments = suite.arguments;
        }

        /// <summary>
        /// Overridden to run the one-time setup for a suite.
        /// </summary>
        /// <param name="context">The TestExecutionContext to be used.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            if (fixtureType != null)
            {
                if (context.TestObject == null && !IsStaticClass(fixtureType))
                    context.TestObject = Reflect.Construct(fixtureType, arguments);

                foreach (MethodInfo method in  Reflect.GetMethodsWithAttribute(fixtureType, typeof(TestFixtureSetUpAttribute), true))
                    Reflect.InvokeMethod(method, method.IsStatic ? null : context.TestObject);
            }

            return context.CurrentResult;
        }

        private static bool IsStaticClass(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }
    }
}
