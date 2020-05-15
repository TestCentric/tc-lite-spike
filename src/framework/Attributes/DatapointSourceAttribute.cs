// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
    /// <summary>
    /// Used to mark a field, property or method providing a set of datapoints to 
    /// be used in executing any theories within the same fixture that require an 
    /// argument of the Type provided. The data source may provide an array of
    /// the required Type or an IEnumerable&lt;T&gt;.
    /// Synonymous with DatapointsAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DatapointSourceAttribute : NUnitAttribute
    {
    }
}
