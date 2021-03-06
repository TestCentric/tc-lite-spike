﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Tests
{
	[TestFixture]
	public class RangeTests
	{
		private static readonly string NL = Environment.NewLine;

		[Test]
		public void InRangeSucceeds()
		{
			Assert.That( 7, Is.InRange(5, 10) );
			Assert.That(0.23, Is.InRange(-1.0, 1.0));
			Assert.That(DateTime.Parse("12-December-2008"),
				Is.InRange(DateTime.Parse("1-October-2008"), DateTime.Parse("31-December-2008")));
		}

		[Test]
		public void InRangeFails()
		{
			string expectedMessage = $"  Expected: in range (5,10){NL}  But was:  12{NL}";

			Assert.That(
				new TestDelegate( FailingInRangeMethod ),
				Throws.TypeOf(typeof(AssertionException)).With.Message.EqualTo(expectedMessage));
		}

		private void FailingInRangeMethod()
		{
			Assert.That(12, Is.InRange(5, 10));
		}

		[Test]
		public void NotInRangeSucceeds()
		{
			Assert.That(12, Is.Not.InRange(5, 10));
			Assert.That(2.57, Is.Not.InRange(-1.0, 1.0));
		}

		[Test]
		public void NotInRangeFails()
		{
			string expectedMessage = $"  Expected: not in range (5,10){NL}  But was:  7{NL}";

			Assert.That(
				new TestDelegate(FailingNotInRangeMethod),
				Throws.TypeOf(typeof(AssertionException)).With.Message.EqualTo(expectedMessage));
		}

		private void FailingNotInRangeMethod()
		{
			Assert.That(7, Is.Not.InRange(5, 10));
		}
	}
}
