// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// Combines multiple filters so that a test must pass one 
	/// of them in order to pass this filter.
	/// </summary>
	[Serializable]
	public class OrFilter : TestFilter
	{
		private List<ITestFilter> filters = new List<ITestFilter>();

		/// <summary>
		/// Constructs an empty OrFilter
		/// </summary>
		public OrFilter() { }

		/// <summary>
		/// Constructs an AndFilter from an array of filters
		/// </summary>
		/// <param name="filters"></param>
		public OrFilter( params ITestFilter[] filters )
		{
			this.filters.AddRange( filters );
		}

		/// <summary>
		/// Adds a filter to the list of filters
		/// </summary>
		/// <param name="filter">The filter to be added</param>
		public void Add( ITestFilter filter )
		{
			this.filters.Add( filter );
		}

		/// <summary>
		/// Return an array of the composing filters
		/// </summary>
		public ITestFilter[] Filters
		{
			get
			{
                return filters.ToArray();
			}
		}

		/// <summary>
		/// Checks whether the OrFilter is matched by a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if any of the component filters pass, otherwise false</returns>
		public override bool Pass( ITest test )
		{
			foreach( ITestFilter filter in filters )
				if ( filter.Pass( test ) )
					return true;

			return false;
		}

		/// <summary>
		/// Checks whether the OrFilter is matched by a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if any of the component filters match, otherwise false</returns>
		public override bool Match( ITest test )
		{
			foreach( TestFilter filter in filters )
				if ( filter.Match( test ) )
					return true;

			return false;
		}
	}
}
