// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Marks a test to use a pairwise join of any argument 
    /// data provided. Arguments will be combined in such a
    /// way that all possible pairs of arguments are used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=false)]
    public class PairwiseAttribute : PropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public PairwiseAttribute() : base(PropertyNames.JoinType, "Pairwise") { }
    }
}
