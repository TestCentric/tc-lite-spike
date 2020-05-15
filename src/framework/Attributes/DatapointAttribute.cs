// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
    /// <summary>
    /// Used to mark a field for use as a datapoint when executing a theory
    /// within the same fixture that requires an argument of the field's Type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DatapointAttribute : NUnitAttribute
    {
    }
}
