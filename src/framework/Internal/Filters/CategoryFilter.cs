// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// CategoryFilter is able to select or exclude tests
	/// based on their categories.
	/// </summary>
	/// 
	[Serializable]
	public class CategoryFilter : TestFilter
	{
        List<string> categories = new List<string>();

		/// <summary>
		/// Construct an empty CategoryFilter
		/// </summary>
		public CategoryFilter()
		{
		}

		/// <summary>
		/// Construct a CategoryFilter using a single category name
		/// </summary>
		/// <param name="name">A category name</param>
		public CategoryFilter( string name )
		{
			if ( name != null && name != string.Empty )
				categories.Add( name );
		}

		/// <summary>
		/// Construct a CategoryFilter using an array of category names
		/// </summary>
		/// <param name="names">An array of category names</param>
		public CategoryFilter( string[] names )
		{
			if ( names != null )
				categories.AddRange( names );
		}

		/// <summary>
		/// Add a category name to the filter
		/// </summary>
		/// <param name="name">A category name</param>
		public void AddCategory(string name) 
		{
			categories.Add( name );
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns></returns>
        public override bool Match(ITest test)
        {
            IList testCategories = test.Properties[PropertyNames.Category] as IList;

			if ( testCategories == null || testCategories.Count == 0)
				return false;

			foreach( string cat in this.categories )
				if ( testCategories.Contains( cat ) )
					return true;

			return false;
        }
		
		/// <summary>
		/// Return the string representation of a category filter
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for( int i = 0; i < categories.Count; i++ )
			{
				if ( i > 0 ) sb.Append( ',' );
				sb.Append( categories[i] );
			}
			return sb.ToString();
		}
	}
}
