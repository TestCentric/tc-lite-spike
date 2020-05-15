// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.CultureAttributeData
{
	[TestFixture, Culture( "en,fr,de" )]
	public class FixtureWithCultureAttribute
	{
		[Test, Culture("en,de")]
		public void EnglishAndGermanTest() { }

		[Test, Culture("fr")]
		public void FrenchTest() { }

		[Test, Culture("fr-CA")]
		public void FrenchCanadaTest() { }
	}

#if !NETCF
    [TestFixture, SetCulture("xx-XX")]
    public class FixtureWithInvalidSetCultureAttribute
    {
        [Test]
        public void SomeTest() { }
    }

    [TestFixture]
    public class FixtureWithInvalidSetCultureAttributeOnTest
    {
        [Test, SetCulture("xx-XX")]
        public void InvalidCultureSet() { }
    }
#endif
}