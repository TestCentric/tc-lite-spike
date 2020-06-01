// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if false
// TODO: Rework this
// RepeatAttribute should either
//  1) Apply at load time to create the exact number of tests, or
//  2) Apply at run time, generating tests or results dynamically
//
// #1 is feasible but doesn't provide much benefit
// #2 requires infrastructure for dynamic test cases first
using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal.Commands;

namespace TCLite.Framework
{
	/// <summary>
	/// RepeatAttribute may be applied to test case in order
	/// to run it multiple times.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class RepeatAttribute : PropertyAttribute, ICommandDecorator
	{
        /// <summary>
        /// Construct a RepeatAttribute
        /// </summary>
        /// <param name="count">The number of times to run the test</param>
        public RepeatAttribute(int count) : base(count) { }

        //private int count;

        ///// <summary>
        ///// Construct a RepeatAttribute
        ///// </summary>
        ///// <param name="count">The number of times to run the test</param>
        //public RepeatAttribute(int count)
        //{
        //    this.count = count;
        //}

        ///// <summary>
        ///// Gets the number of times to run the test.
        ///// </summary>
        //public int Count
        //{
        //    get { return count; }
        //}

        #region ICommandDecorator Members

        CommandStage ICommandDecorator.Stage
        {
            get { return CommandStage.Repeat; }
        }

        int ICommandDecorator.Priority
        {
            get { return 0; }
        }

        TestCommand ICommandDecorator.Decorate(TestCommand command)
        {
            return new RepeatedTestCommand(command);
        }

        #endregion
    }
}
#endif