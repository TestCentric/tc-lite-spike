// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// NotFilter negates the operation of another filter
	/// </summary>
	[Serializable]
	public class NotFilter : TestFilter
	{
		ITestFilter baseFilter;
        bool topLevel = false;

		/// <summary>
		/// Construct a not filter on another filter
		/// </summary>
		/// <param name="baseFilter">The filter to be negated</param>
		public NotFilter( ITestFilter baseFilter)
		{
			this.baseFilter = baseFilter;
		}

        /// <summary>
        /// Indicates whether this is a top-level NotFilter,
        /// requiring special handling of Explicit
        /// </summary>
        public bool TopLevel
        {
            get { return topLevel; }
            set { topLevel = value; }
        }

		/// <summary>
		/// Gets the base filter
		/// </summary>
		public ITestFilter BaseFilter
		{
			get { return baseFilter; }
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
            if (topLevel && test.RunState == RunState.Explicit)
                return false;

			return !baseFilter.Pass( test );
		}

		/// <summary>
		/// Determine whether any descendant of the test matches the filter criteria.
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if at least one descendant matches the filter criteria</returns>
		protected override bool MatchDescendant(ITest test)
		{
			if (!test.HasChildren || test.Tests == null || topLevel && test.RunState == RunState.Explicit)
				return false;

			foreach (ITest child in test.Tests)
			{
				if (Match(child) || MatchDescendant(child))
					return true;
			}

			return false;
		}	
	}
}
