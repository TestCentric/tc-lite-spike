// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.DescriptionFixture
{
	[TestFixture(Description = "Fixture Description")]
	public class DescriptionFixture
	{
		[Test(Description = "Test Description")]
		public void Method()
		{}

		[Test]
		public void NoDescriptionMethod()
		{}

        [Test]
        [Description("Separate Description")]
        public void SeparateDescriptionMethod()
        { }

        [Test, Description("method description")]
        [TestCase(5, Description = "case description")]
        public void TestCaseWithDescription(int x)
        { }
	}
}
