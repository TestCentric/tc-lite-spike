// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
	/// <summary>
	/// ExplicitAttribute marks a test or test fixture so that it will
	/// only be run if explicitly executed from the gui or command line
	/// or if it is included by use of a filter. The test will not be
	/// run simply because an enclosing suite is run.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public class ExplicitAttribute : NUnitAttribute, IApplyToTest
	{
        private string reason;

        /// <summary>
		/// Default constructor
		/// </summary>
		public ExplicitAttribute()
		{
            this.reason = "";
        }

        /// <summary>
        /// Constructor with a reason
        /// </summary>
        /// <param name="reason">The reason test is marked explicit</param>
        public ExplicitAttribute(string reason)
        {
            this.reason = reason;
        }

        #region IApplyToTest members

        /// <summary>
        /// Modifies a test by marking it as explicit.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (test.RunState != RunState.NotRunnable)
            {
                test.RunState = RunState.Explicit;
                test.Properties.Set(PropertyNames.SkipReason, reason);
            }
        }

        #endregion
    }
}
