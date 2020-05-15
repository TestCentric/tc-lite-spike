// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.AttributeInheritanceData
{
	// Sample Test from a post by Scott Bellware

	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
	class ConcernAttribute : TestFixtureAttribute
	{
		private Type typeOfConcern;

		public ConcernAttribute( Type typeOfConcern )
		{
			this.typeOfConcern = typeOfConcern;
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	class SpecAttribute : TestAttribute
	{
	}

	/// <summary>
	/// Summary description for AttributeInheritance.
	/// </summary>
	[Concern(typeof(ClassUnderTest))]
	public class When_collecting_test_fixtures
	{
		[Spec]
		public void should_include_classes_with_an_attribute_derived_from_TestFixtureAttribute()
		{
		}
	}

    class ClassUnderTest { }
}
