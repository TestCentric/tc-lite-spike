// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData
{
    [TestFixture(1)]
    [TestFixture(2)]
    public class ParameterizedTestFixture
    {
        [Test]
        public void MethodWithoutParams()
        {
        }

        [TestCase(10,20)]
        public void MethodWithParams(int x, int y)
        {
        }
	}
    
	[TestFixture(Category = "XYZ")]
	public class TestFixtureWithSingleCategory
	{
	}
	
	[TestFixture(Category = "X,Y,Z")]
	public class TestFixtureWithMultipleCategories
	{
	}
}
