// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Builders;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// TestFixtureBuilder contains static methods for building
	/// TestFixtures from types. It uses builtin SuiteBuilders
	/// and any installed extensions to do it.
	/// </summary>
	public class TestFixtureBuilder
	{
        private static Extensibility.ISuiteBuilder builder = new NUnitTestFixtureBuilder();

        /// <summary>
        /// Determines whether this instance [can build from] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can build from] the specified type; otherwise, <c>false</c>.
        /// </returns>
		public static bool CanBuildFrom( Type type )
		{
            return builder.CanBuildFrom(type);
		}

		/// <summary>
		/// Build a test fixture from a given type.
		/// </summary>
		/// <param name="type">The type to be used for the fixture</param>
		/// <returns>A TestSuite if the fixture can be built, null if not</returns>
		public static Test BuildFrom( Type type )
		{
            return builder.BuildFrom( type );
		}

		/// <summary>
		/// Build a fixture from an object. 
		/// </summary>
		/// <param name="fixture">The object to be used for the fixture</param>
		/// <returns>A TestSuite if fixture type can be built, null if not</returns>
		public static Test BuildFrom( object fixture )
		{
			Test suite = BuildFrom( fixture.GetType() );
			if( suite != null)
				suite.Fixture = fixture;
			return suite;
		}

		/// <summary>
		/// Private constructor to prevent instantiation
		/// </summary>
		private TestFixtureBuilder() { }
	}
}
