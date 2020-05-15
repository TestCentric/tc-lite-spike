// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class RangeConstraintTest : ConstraintTestBaseWithArgumentException
    {
        RangeConstraint<int> rangeConstraint;

        [SetUp]
        public void SetUp()
        {
            theConstraint = rangeConstraint = new RangeConstraint<int>(5, 42);
            expectedDescription = "in range (5,42)";
            stringRepresentation = "<range 5 42>";
        }

        internal object[] SuccessData = new object[] { 5, 23, 42 };

        internal object[] FailureData = new object[] { new object[] { 4, "4" }, new object[] { 43, "43" } };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer<T> : IComparer<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [Test]
        public void UsesProvidedComparisonOfT()
        {
            MyComparison<int> comparer = new MyComparison<int>();
            Assert.That(rangeConstraint.Using(new Comparison<int>(comparer.Compare)).Matches(19));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

#if !NETCF
        [Test]
        public void UsesProvidedLambda()
        {
            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            Assert.That(rangeConstraint.Using(comparer).Matches(19));
        }
#endif
    }
}