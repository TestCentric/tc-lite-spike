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
    /// PlatformAttribute is used to mark a test fixture or an
    /// individual method as applying to a particular platform only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited=false)]
    public class PlatformAttribute : IncludeExcludeAttribute, IApplyToTest
    {
        private PlatformHelper platformHelper = new PlatformHelper();

        /// <summary>
        /// Constructor with no platforms specified, for use
        /// with named property syntax.
        /// </summary>
        public PlatformAttribute() { }

        /// <summary>
        /// Constructor taking one or more platforms
        /// </summary>
        /// <param name="platforms">Comma-deliminted list of platforms</param>
        public PlatformAttribute(string platforms) : base(platforms) { }

        #region IApplyToTest members

        /// <summary>
        /// Causes a test to be skipped if this PlatformAttribute is not satisfied.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (test.RunState != RunState.NotRunnable && !platformHelper.IsPlatformSupported(this))
            {
                test.RunState = RunState.Skipped;
                test.Properties.Add(PropertyNames.SkipReason, platformHelper.Reason);
            }
        }

        #endregion
    }
}
