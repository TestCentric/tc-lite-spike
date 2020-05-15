// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
	using System;

    /// <summary>
    /// Attribute used to mark a class that contains one-time SetUp 
    /// and/or TearDown methods that apply to all the tests in a
    /// namespace or an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=true)]
    public class SetUpAttribute : NUnitAttribute
    { }

    /// <summary>
    /// Attribute used to mark a class that contains one-time SetUp 
    /// and/or TearDown methods that apply to all the tests in a
    /// namespace or an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=true)]
    public class PreTestAttribute : NUnitAttribute
    { }

    /// <summary>
    /// Attribute used to mark a class that contains one-time SetUp 
    /// and/or TearDown methods that apply to all the tests in a
    /// namespace or an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited=true)]
    public class PostTestAttribute : NUnitAttribute
    { }
}
