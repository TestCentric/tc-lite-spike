// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Api
{
	/// <summary>
	/// Interface to be implemented by filters applied to tests.
	/// The filter applies when running the test, after it has been
	/// loaded, since this is the only time an ITest exists.
	/// </summary>
	public interface ITestFilter
	{
        /// <summary>
        /// Indicates whether this is the EmptyFilter
        /// </summary>
        bool IsEmpty { get; }

		/// <summary>
		/// Determine if a particular test passes the filter criteria. Pass
		/// may examine the parents and/or descendants of a test, depending
		/// on the semantics of the particular filter
		/// </summary>
		/// <param name="test">The test to which the filter is applied</param>
		/// <returns>True if the test passes the fFilter, otherwise false</returns>
		bool Pass( ITest test );
	}
}
