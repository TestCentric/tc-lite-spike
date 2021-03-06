// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class LessThanConstraintTests : ComparisonConstraintTest
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = comparisonConstraint = new LessThanConstraint(5);
            expectedDescription = "less than 5";
            stringRepresentation = "<lessthan 5>";
        }

        internal object[] SuccessData = new object[] { 4, 4.999 };

        internal object[] FailureData = new object[] { new object[] { 6, "6" }, new object[] { 5, "5" } };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThan(expected));
        }

        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThan(expected));
        }
    }
}