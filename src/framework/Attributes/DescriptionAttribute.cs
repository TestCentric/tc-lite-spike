// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
	/// <summary>
	/// Attribute used to provide descriptive text about a 
	/// test case or fixture.
	/// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public sealed class DescriptionAttribute : PropertyAttribute
    {
        /// <summary>
        /// Construct a description Attribute
        /// </summary>
        /// <param name="description">The text of the description</param>
        public DescriptionAttribute(string description) : base(PropertyNames.Description, description) { }
    }

}
