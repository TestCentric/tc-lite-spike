// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.TestUtilities;

namespace TCLite.Framework.Syntax
{
#if NYI
    public class UniqueTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<uniqueitems>";
            staticSyntax = Is.Unique;
            builderSyntax = Builder().Unique;
        }
    }
#endif

    public class CollectionOrderedTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered>";
            staticSyntax = Is.Ordered;
            builderSyntax = Builder().Ordered;
        }
    }

    public class CollectionOrderedTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<ordered descending>";
            staticSyntax = Is.Ordered.Descending;
            builderSyntax = Builder().Ordered.Descending;
        }
    }

    public class CollectionOrderedTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = new SimpleObjectComparer();
            parseTree = "<ordered TCLite.TestUtilities.SimpleObjectComparer>";
            staticSyntax = Is.Ordered.Using(comparer);
            builderSyntax = Builder().Ordered.Using(comparer);
        }
    }

    public class CollectionOrderedTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = new SimpleObjectComparer();
            parseTree = "<ordered descending TCLite.TestUtilities.SimpleObjectComparer>";
            staticSyntax = Is.Ordered.Using(comparer).Descending;
            builderSyntax = Builder().Ordered.Using(comparer).Descending;
        }
    }

    public class CollectionOrderedByTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<orderedby SomePropertyName>";
            staticSyntax = Is.Ordered.By("SomePropertyName");
            builderSyntax = Builder().Ordered.By("SomePropertyName");
        }
    }

    public class CollectionOrderedByTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<orderedby SomePropertyName descending>";
            staticSyntax = Is.Ordered.By("SomePropertyName").Descending;
            builderSyntax = Builder().Ordered.By("SomePropertyName").Descending;
        }
    }

    public class CollectionOrderedByTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<orderedby SomePropertyName TCLite.TestUtilities.SimpleObjectComparer>";
            staticSyntax = Is.Ordered.By("SomePropertyName").Using(new SimpleObjectComparer());
            builderSyntax = Builder().Ordered.By("SomePropertyName").Using(new SimpleObjectComparer());
        }
    }

    public class CollectionOrderedByTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<orderedby SomePropertyName descending TCLite.TestUtilities.SimpleObjectComparer>";
            staticSyntax = Is.Ordered.By("SomePropertyName").Using(new SimpleObjectComparer()).Descending;
            builderSyntax = Builder().Ordered.By("SomePropertyName").Using(new SimpleObjectComparer()).Descending;
        }
    }

    public class CollectionContainsTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains 42>";
            staticSyntax = Contains.Item(42);
            builderSyntax = Builder().Contains(42);
        }
    }

    public class CollectionContainsTest_String : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains \"abc\">";
            staticSyntax = Contains.Item("abc");
            builderSyntax = Builder().Contains("abc");
        }
    }

#if !SILVERLIGHT
    public class CollectionContainsTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains 42>";
            staticSyntax = Contains.Item(42).Using(Comparer.Default);
            builderSyntax = Builder().Contains(42).Using(Comparer.Default);
        }

        [Test]
        public void ComparerIsCalled()
        {
            TestComparer comparer = new TestComparer();
            Assert.That(new int[] { 1, 2, 3 },
                Contains.Item(2).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        public void ComparerIsCalledInExpression()
        {
            TestComparer comparer = new TestComparer();
            Assert.That(new int[] { 1, 2, 3 },
                Has.Length.EqualTo(3).And.Contains(2).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }
    }

    public class CollectionContainsTest_Comparer_String : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains \"abc\">";
            staticSyntax = Contains.Item("abc").Using(Comparer.Default);
            builderSyntax = Builder().Contains("abc").Using(Comparer.Default);
        }

        [Test]
        public void ComparerIsCalled()
        {
            TestComparer comparer = new TestComparer();
            Assert.That(new string[] { "Hello", "World" },
                Contains.Item("World").Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        public void ComparerIsCalledInExpression()
        {
            TestComparer comparer = new TestComparer();
            Assert.That(new string[] { "Hello", "World" },
                Has.Length.EqualTo(2).And.Contains("World").Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }
    }

    public class CollectionMemberTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains 42>";
            staticSyntax = Has.Member(42);
            builderSyntax = Builder().Contains(42);
        }
    }

    public class CollectionMemberTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<contains 42>";
            staticSyntax = Has.Member(42).Using(Comparer.Default);
            builderSyntax = Builder().Contains(42).Using(Comparer.Default);
        }
    }
#endif

#if NYI
    public class CollectionSubsetTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            int[] ints = new int[] { 1, 2, 3 };
            parseTree = "<subsetof System.Int32[]>";
            staticSyntax = Is.SubsetOf(ints);
            builderSyntax = Builder().SubsetOf(ints);
        }
    }

    public class CollectionEquivalentTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            int[] ints = new int[] { 1, 2, 3 };
            parseTree = "<equivalent System.Int32[]>";
            staticSyntax = Is.EquivalentTo(ints);
            builderSyntax = Builder().EquivalentTo(ints);
        }
    }
#endif
}
