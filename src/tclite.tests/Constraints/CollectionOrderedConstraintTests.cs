// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class CollectionOrderedConstraintTests
    {
        private static readonly string NL = Environment.NewLine;
        
        [Test]
        public void IsOrdered()
        {
            ICollection collection = new SimpleObjectCollection("x", "y", "z");
            Assert.That(collection, Is.Ordered);
        }

        [Test]
        public void IsOrderedDescending()
        {
            ICollection collection = new SimpleObjectCollection("z", "y", "x");
            Assert.That(collection, Is.Ordered.Descending);
        }

        [Test]
        public void IsOrdered_Fails()
        {
            ICollection collection = new SimpleObjectCollection("x", "z", "y");

            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(collection, Is.Ordered);
            });

            Assert.That(ex.Message, Is.EqualTo(
                "  Expected: collection ordered" + NL +
                "  But was:  < \"x\", \"z\", \"y\" >" + NL));
        }

        [Test]
        public void IsOrdered_Allows_adjacent_equal_values()
        {
            ICollection collection = new SimpleObjectCollection("x", "x", "z");
            Assert.That(collection, Is.Ordered);
        }

        public void IsOrdered_Handles_null()
        {
            ICollection collection = new SimpleObjectCollection("x", null, "z");
            Assert.Throws<ArgumentNullException>(() =>
            {
                Assert.That(collection, Is.Ordered);
            });
        }

        [Test]
        public void IsOrdered_TypesMustBeComparable()
        {
            ICollection collection = new SimpleObjectCollection(1, "x");
            Assert.Throws<ArgumentException>(() =>
            {
                Assert.That(collection, Is.Ordered);
            });
        }

        [Test]
        public void IsOrdered_AtLeastOneArgMustImplementIComparable()
        {
            ICollection collection = new SimpleObjectCollection(new object(), new object());
            Assert.Throws<ArgumentException>(() =>
            {
                Assert.That(collection, Is.Ordered);
            });
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison()
        {
            ICollection collection = new SimpleObjectCollection(new object(), new object());

            AlwaysEqualComparer comparer = new AlwaysEqualComparer();
            Assert.That(collection, Is.Ordered.Using(comparer));
            Assert.That(comparer.Called, "TestComparer was not called");
        }

        [Test]
        public void IsOrdered_Handles_custom_comparison2()
        {
            ICollection collection = new SimpleObjectCollection(2, 1);

            TestComparer comparer = new TestComparer();
            Assert.That(collection, Is.Ordered.Using(comparer));
            Assert.That(comparer.Called, "TestComparer was not called");
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            ICollection al = new SimpleObjectCollection(1, 2);

            MyComparer<int> comparer = new MyComparer<int>();
            Assert.That(al, Is.Ordered.Using(comparer));
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
            ICollection al = new SimpleObjectCollection(1, 2);

            MyComparison<int> comparer = new MyComparison<int>();
            Assert.That(al, Is.Ordered.Using(new Comparison<int>(comparer.Compare)));
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

#if !NETCF_2_0
        [Test]
        public void UsesProvidedLambda()
        {
            ICollection al = new SimpleObjectCollection(1, 2);

            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            Assert.That(al, Is.Ordered.Using(comparer));
        }
#endif

        [Test]
        public void IsOrderedBy()
        {
            ICollection collection = new SimpleObjectCollection(
                new OrderedByTestClass(1),
                new OrderedByTestClass(2));

            Assert.That(collection, Is.Ordered.By("Value"));
        }

        [Test]
        public void IsOrderedBy_Comparer()
        {
            ICollection collection = new SimpleObjectCollection(
                new OrderedByTestClass(1),
                new OrderedByTestClass(2));

            Assert.That(collection, Is.Ordered.By("Value").Using(new SimpleObjectComparer()));
        }

        [Test]
        public void IsOrderedBy_Handles_heterogeneous_classes_as_long_as_the_property_is_of_same_type()
        {
            ICollection al = new SimpleObjectCollection(
                new OrderedByTestClass(1),
                new OrderedByTestClass2(2));

            Assert.That(al, Is.Ordered.By("Value"));
        }

        public class OrderedByTestClass
        {
            private int myValue;

            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass(int value)
            {
                Value = value;
            }
        }

        public class OrderedByTestClass2
        {
            private int myValue;
            public int Value
            {
                get { return myValue; }
                set { myValue = value; }
            }

            public OrderedByTestClass2(int value)
            {
                Value = value;
            }
        }
    }
}