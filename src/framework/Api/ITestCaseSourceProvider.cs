// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NYI
using System;

namespace TCLite.Framework.Api
{
    /// <summary>
    /// The ITestCaseSourceProvider interface is implemented by Types that 
    /// are able to provide a test case source for use by a test method.
    /// </summary>
    public interface IDynamicTestCaseSource
    {
        /// <summary>
        /// Returns a test case source. May be called on a provider
        /// implementing the source internally or able to create
        /// a source instance on it's own.
        /// </summary>
        /// <returns></returns>
        ITestCaseSource GetTestCaseSource();
        
        /// <summary>
        /// Returns a test case source based on an instance of a 
        /// source object.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        ITestCaseSource GetTestCaseSource(object instance);
    }
}
#endif
