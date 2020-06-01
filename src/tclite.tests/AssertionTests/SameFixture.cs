// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Text;
using TCLite.Framework;

namespace TCLite.Framework.AssertionTests
{
	[TestFixture]
	public class SameFixture : AssertionTestBase
	{
		[Test]
		public void Same()
		{
			string s1 = "S1";
			Assert.AreSame(s1, s1);
		}

		[Test]
		public void SameFails()
		{
			Exception ex1 = new Exception( "one" );
			Exception ex2 = new Exception( "two" );

			ThrowsAssertionException(() => Assert.AreSame(ex1, ex2),
				"  Expected: same as <System.Exception: one>" + NL +
				"  But was:  <System.Exception: two>" + NL);
		}

		[Test]
		public void SameValueTypes()
		{
			int index = 2;
			ThrowsAssertionException(() => Assert.AreSame(index, index),
				"  Expected: same as 2" + NL +
				"  But was:  2" + NL);
		}
	}
}
