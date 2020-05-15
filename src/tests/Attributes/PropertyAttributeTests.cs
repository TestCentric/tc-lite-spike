// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;
using TCLite.TestData.PropertyAttributeTests;

namespace TCLite.Framework.Attributes
{
	[TestFixture]
	public class PropertyAttributeTests
	{
		TestSuite fixture;

		[SetUp]
		public void CreateFixture()
		{
			fixture = TestBuilder.MakeFixture( typeof( FixtureWithProperties ) );
		}

		[Test]
		public void PropertyWithStringValue()
		{
			Test test1 = (Test)fixture.Tests[0];
			Assert.That( test1.Properties["user"].Contains("Charlie"));
		}

		[Test]
		public void PropertiesWithNumericValues()
		{
			Test test2 = (Test)fixture.Tests[1];
			Assert.AreEqual( 10.0, test2.Properties.Get("X") );
			Assert.AreEqual( 17.0, test2.Properties.Get("Y") );
		}

		[Test]
		public void PropertyWorksOnFixtures()
		{
			Assert.AreEqual( "SomeClass", fixture.Properties.Get("ClassUnderTest") );
		}

		[Test]
		public void CanDeriveFromPropertyAttribute()
		{
			Test test3 = (Test)fixture.Tests[2];
			Assert.AreEqual( 5, test3.Properties.Get("Priority") );
		}
	}
}
