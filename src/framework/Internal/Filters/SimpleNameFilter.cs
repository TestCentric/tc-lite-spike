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
    public class SimpleNameFilter : TestFilter
    {
        private List<string> names = new List<string>();

        /// <summary>
		/// Construct an empty SimpleNameFilter
		/// </summary>
        public SimpleNameFilter() { }

        /// <summary>
        /// Construct a SimpleNameFilter for a single name
        /// </summary>
        /// <param name="nameToAdd">The name the filter will recognize.</param>
        public SimpleNameFilter(string nameToAdd)
        {
            Add(nameToAdd);
        }

        /// <summary>
        /// Construct a SimpleNameFilter for an array of names
        /// </summary>
        /// <param name="namesToAdd">The names the filter will recognize.</param>
        public SimpleNameFilter(string[] namesToAdd)
        {
            foreach (string name in namesToAdd)
                Add(name);
        }

        /// <summary>
		/// Add a name to a SimpleNameFilter
		/// </summary>
        /// <param name="name">The name to be added.</param>
        public void Add(string name)
		{
            Guard.ArgumentNotNullOrEmpty(name, "name");

            names.Add(name);
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
			foreach( string name in names )
				if ( test.FullName == name )
					return true;

			return false;
		}
	}
}
