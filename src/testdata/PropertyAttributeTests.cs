// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.PropertyAttributeTests
{
	[TestFixture, Property("ClassUnderTest","SomeClass" )]
	public class FixtureWithProperties
	{
		[Test, Property("user","Charlie")]
		public void Test1() { }

		[Test, Property("X",10.0), Property("Y",17.0)]
		public void Test2() { }

		[Test, Priority(5)]
		public void Test3() { }
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class PriorityAttribute : PropertyAttribute
	{
		public PriorityAttribute( int level ) : base( level ) { }
	}
}
