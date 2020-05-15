// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Marks a test to use a Sequential join of any argument 
    /// data provided. Arguments will be combined into test cases,
    /// taking the next value of each argument until all are used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=false)]
    public class SequentialAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SequentialAttribute() : base(PropertyNames.JoinType, "Sequential") { }
    }
}
