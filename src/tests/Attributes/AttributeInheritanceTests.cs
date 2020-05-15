// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;
using TCLite.TestData.AttributeInheritanceData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Tests
{
	[TestFixture]
	public class AttributeInheritanceTests
	{
		[Test]
		public void InheritedFixtureAttributeIsRecognized()
		{
			Assert.That( TestBuilder.MakeFixture( typeof (When_collecting_test_fixtures) ) != null );
		}

		[Test]
		public void InheritedTestAttributeIsRecognized()
		{
			Test fixture = TestBuilder.MakeFixture( typeof( When_collecting_test_fixtures ) );
			Assert.AreEqual( 1, fixture.TestCaseCount );
		}
    }
}
