// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class NUnitComparerTests
    {
        private Tolerance tolerance;
        private NUnitComparer comparer;

        [SetUp]
        public void SetUp()
        {
            tolerance = Tolerance.Empty;
            comparer = new NUnitComparer();
        }

        [TestCase(4, 4)]
        [TestCase(4.0d, 4.0d)]
        [TestCase(4.0f, 4.0f)]
        [TestCase(4, 4.0d)]
        [TestCase(4, 4.0f)]
        [TestCase(4.0d, 4)]
        [TestCase(4.0d, 4.0f)]
        [TestCase(4.0f, 4)]
        [TestCase(4.0f, 4.0d)]
        [TestCase(null, null)]
        public void EqualItems(object x, object y)
        {
            Assert.That(comparer.Compare(x, y) == 0);
        }

        [TestCase(4, 2)]
        [TestCase(4.0d, 2.0d)]
        [TestCase(4.0f, 2.0f)]
        [TestCase(4, 2.0d)]
        [TestCase(4, 2.0f)]
        [TestCase(4.0d, 2)]
        [TestCase(4.0d, 2.0f)]
        [TestCase(4.0f, 2)]
        [TestCase(4.0f, 2.0d)]
        [TestCase(4, null)]
        public void UnequalItems(object greater, object lesser)
        {
            Assert.That(comparer.Compare(greater, lesser) > 0);
            Assert.That(comparer.Compare(lesser, greater) < 0);
        }
    }
}
