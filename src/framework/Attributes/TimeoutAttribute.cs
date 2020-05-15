// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Commands;
using TCLite.Framework.Api;

namespace TCLite.Framework
{
    /// <summary>
    /// Used on a method, marks the test with a timeout value in milliseconds. 
    /// The test will be run in a separate thread and is cancelled if the timeout 
    /// is exceeded. Used on a class or assembly, sets the default timeout 
    /// for all contained test methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public class TimeoutAttribute : PropertyAttribute, IApplyToContext
    {
        private int _timeout;

        /// <summary>
        /// Construct a TimeoutAttribute given a time in milliseconds
        /// </summary>
        /// <param name="timeout">The timeout value in milliseconds</param>
        public TimeoutAttribute(int timeout)
            : base(timeout)
        {
            _timeout = timeout;
        }

        void IApplyToContext.ApplyToContext(TestExecutionContext context)
        {
            context.TestCaseTimeout = _timeout;
        }
    }
}

