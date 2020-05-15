// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.TestCaseAttributeFixture
{
    [TestFixture]
    public class TestCaseAttributeFixture
    {
		[TestCase("12-Octobar-1942")]
		public void MethodHasInvalidDateFormat(DateTime dt)
		{}

        [TestCase(2,3,4,Description="My Description")]
        public void MethodHasDescriptionSpecified(int x, int y, int z)
        {}

		[TestCase(2,3,4,TestName="XYZ")]
		public void MethodHasTestNameSpecified(int x, int y, int z)
		{}
 
        [TestCase(2, 3, 4, Category = "XYZ")]
        public void MethodHasSingleCategory(int x, int y, int z)
        { }
 
        [TestCase(2, 3, 4, Category = "X,Y,Z")]
        public void MethodHasMultipleCategories(int x, int y, int z)
        { }
 
		[TestCase(2, 2000000, ExpectedResult=4)]
		public int MethodCausesConversionOverflow(short x, short y)
		{
			return x + y;
		}

        [TestCase(1)]
        [TestCase(2, Ignore = true)]
        [TestCase(3, IgnoreReason = "Don't Run Me!")]
        public void MethodWithIgnoredTestCases(int num)
        {
        }

        [TestCase(1)]
        [TestCase(2, Explicit = true)]
        [TestCase(3, Explicit = true, Reason = "Connection failing")]
        public void MethodWithExplicitTestCases(int num)
        {
        }
    }
}
