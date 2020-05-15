// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Marks a test to use a combinatorial join of any argument 
    /// data provided. Since this is the default, the attribute is
    /// not needed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=false)]
    public class CombinatorialAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CombinatorialAttribute() : base(PropertyNames.JoinType, "Combinatorial") { }
    }
}
