// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Threading;
using System.Globalization;
using TCLite.Framework;

namespace TCLite.Framework.Assertions
{
	[TestFixture]
	public class ConditionAssertTests //: MessageChecker
	{
		[Test]
		public void IsTrue()
		{
			Assert.True(true);
		}

		[Test]
		public void IsFalse()
		{
			Assert.False(false);
		}

		[Test]
		public void IsNull()
		{
			Assert.Null(null);
		}

		[Test]
		public void IsNotNull()
		{
			String s1 = "S1";
			Assert.NotNull(s1);
		}

#if NYI
		[Test]
		public void IsNaN()
		{
			Assert.IsNaN(double.NaN);
		}

		[Test]
		public void IsEmpty()
		{
			Assert.IsEmpty( "", "Failed on empty String" );
			Assert.IsEmpty( new int[0], "Failed on empty Array" );
			Assert.IsEmpty( new ArrayList(), "Failed on empty ArrayList" );
			Assert.IsEmpty( new Hashtable(), "Failed on empty Hashtable" );
			Assert.IsEmpty( (IEnumerable)new int[0], "Failed on empty IEnumerable" );
		}

		[Test]
		public void IsNotEmpty()
		{
			int[] array = new int[] { 1, 2, 3 };
			ArrayList list = new ArrayList( array );
			Hashtable hash = new Hashtable();
			hash.Add( "array", array );

			Assert.IsNotEmpty( "Hi!", "Failed on String" );
			Assert.IsNotEmpty( array, "Failed on Array" );
			Assert.IsNotEmpty( list, "Failed on ArrayList" );
			Assert.IsNotEmpty( hash, "Failed on Hashtable" );
			Assert.IsNotEmpty( (IEnumerable)array, "Failed on IEnumerable" );
		}

#endif
	}
}
