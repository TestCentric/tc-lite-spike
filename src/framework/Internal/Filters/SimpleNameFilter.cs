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
	/// SimpleName filter selects tests based on their name
	/// </summary>
    [Serializable]
    internal class SimpleNameFilter : ValueMatchFilter
    {
        private List<string> names = new List<string>();

        /// <summary>
        /// Construct a SimpleNameFilter for a single name
        /// </summary>
        /// <param name="nameToAdd">The name the filter will recognize.</param>
        public SimpleNameFilter(string nameToAdd) : base(nameToAdd) { }

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
            return Match(test.FullName);
		}

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "cat"; }
        }
	}
}
