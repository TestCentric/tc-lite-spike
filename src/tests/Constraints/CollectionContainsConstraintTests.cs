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
    public class CollectionContainsConstraintTests
    {
        [Test]
        public void CanTestContentsOfArray()
        {
            object item = "xyz";
            object[] c = new object[] { 123, item, "abc" };
            Assert.That(c, new CollectionContainsConstraint(item));
        }

#if !SILVERLIGHT
        [Test]
        public void CanTestContentsOfArrayList()
        {
            object item = "xyz";
            ArrayList list = new ArrayList(new object[] { 123, item, "abc" });
            Assert.That(list, new CollectionContainsConstraint(item));
        }

        [Test]
        public void CanTestContentsOfSortedList()
        {
            object item = "xyz";
            SortedList list = new SortedList();
            list.Add("a", 123);
            list.Add("b", item);
            list.Add("c", "abc");
            Assert.That(list.Values, new CollectionContainsConstraint(item));
            Assert.That(list.Keys, new CollectionContainsConstraint("b"));
        }
#endif

        [Test]
        public void CanTestContentsOfCollectionNotImplementingIList()
        {
            SimpleObjectCollection ints = new SimpleObjectCollection(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Assert.That(ints, new CollectionContainsConstraint(9));
        }

        [Test]
        public void IgnoreCaseIsHonored()
        {
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("WORLD").IgnoreCase);
        }
        [Test]
        public void UsesProvidedIComparer()
        {
            MyComparer comparer = new MyComparer();
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("World").Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer : IComparer
        {
            public bool Called;

            public int Compare(object x, object y)
            {
                Called = true;
                return 0;
            }
        }

        [Test]
        public void UsesProvidedEqualityComparer()
        {
            MyEqualityComparer comparer = new MyEqualityComparer();
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("World").Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyEqualityComparer : IEqualityComparer
        {
            public bool Called;

            bool IEqualityComparer.Equals(object x, object y)
            {
                Called = true;
                return x == y;
            }

            int IEqualityComparer.GetHashCode(object x)
            {
                return x.GetHashCode();
            }
        }

        [Test]
        public void UsesProvidedEqualityComparerOfT()
        {
            MyEqualityComparerOfT<string> comparer = new MyEqualityComparerOfT<string>();
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("World").Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyEqualityComparerOfT<T> : IEqualityComparer<T>
        {
            public bool Called;

            bool IEqualityComparer<T>.Equals(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y) == 0;
            }

            int IEqualityComparer<T>.GetHashCode(T x)
            {
                return x.GetHashCode();
            }
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<string> comparer = new MyComparer<string>();
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("World").Using(comparer));
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
            MyComparison<string> comparer = new MyComparison<string>();
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("World").Using(new Comparison<string>(comparer.Compare)));
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

        [Test]
        public void ContainsWithRecursiveStructure()
        {
            SelfRecursiveEnumerable item = new SelfRecursiveEnumerable();
            SelfRecursiveEnumerable[] container = new SelfRecursiveEnumerable[] { new SelfRecursiveEnumerable(), item };

            Assert.That(container, new CollectionContainsConstraint(item));
        }

        class SelfRecursiveEnumerable : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return this;
            }
        }


#if !NETCF_2_0
        [Test]
        public void UsesProvidedLambdaExpression()
        {
            Assert.That(new string[] { "Hello", "World" },
                new CollectionContainsConstraint("WORLD").Using<string>((x, y) => StringUtil.Compare(x, y, true)));
        }
#endif
    }
}