// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.AssertionTests
{
	[TestFixture]
	public class NotSameFixture : AssertionTestBase
	{
		private readonly string s1 = "S1";
		private readonly string s2 = "S2";

		[Test]
		public void NotSame()
		{
			Assert.AreNotSame(s1, s2);
		}

		[Test]
		public void NotSameFails()
		{
			ThrowsAssertionException(() => Assert.AreNotSame( s1, s1 ),
				"  Expected: not same as \"S1\"" + Environment.NewLine +
				"  But was:  \"S1\"" + Environment.NewLine);
		}
	}
}
