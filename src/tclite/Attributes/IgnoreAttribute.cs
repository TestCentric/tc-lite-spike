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
	/// Attribute used to mark a test that is to be ignored.
	/// Ignored tests result in a warning message when the
	/// tests are run.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method|AttributeTargets.Class|AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public class IgnoreAttribute : TCLiteAttribute, IApplyToTest
	{
		private string reason;

		/// <summary>
		/// Constructs the attribute without giving a reason 
		/// for ignoring the test.
		/// </summary>
		public IgnoreAttribute()
		{
			this.reason = "";
		}

		/// <summary>
		/// Constructs the attribute giving a reason for ignoring the test
		/// </summary>
		/// <param name="reason">The reason for ignoring the test</param>
		public IgnoreAttribute(string reason)
		{
			this.reason = reason;
        }

        #region IApplyToTest members

        /// <summary>
        /// Modifies a test by marking it as Ignored.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (test.RunState != RunState.NotRunnable)
            {
                test.RunState = RunState.Ignored;
                test.Properties.Set(PropertyNames.SkipReason, reason);
            }
        }

        #endregion
    }
}
