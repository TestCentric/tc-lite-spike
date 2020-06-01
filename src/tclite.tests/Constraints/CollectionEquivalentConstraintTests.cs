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
    public class CollectionEquivalentConstraintTests
    {
        [Test]
        public void EqualCollectionsAreEquivalent()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "z");
            ICollection set2 = new SimpleObjectCollection("x", "y", "z");

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void WorksWithCollectionsOfArrays()
        {
            byte[] array1 = new byte[] { 0x20, 0x44, 0x56, 0x76, 0x1e, 0xff };
            byte[] array2 = new byte[] { 0x42, 0x52, 0x72, 0xef };
            byte[] array3 = new byte[] { 0x20, 0x44, 0x56, 0x76, 0x1e, 0xff };
            byte[] array4 = new byte[] { 0x42, 0x52, 0x72, 0xef };

            ICollection set1 = new SimpleObjectCollection(array1, array2);
            ICollection set2 = new SimpleObjectCollection(array3, array4);

            Constraint constraint = new CollectionEquivalentConstraint(set1);
            Assert.That(constraint.Matches(set2));

            set2 = new SimpleObjectCollection(array4, array3);
            Assert.That(constraint.Matches(set2));
        }

        [Test]
        public void EquivalentIgnoresOrder()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "z");
            ICollection set2 = new SimpleObjectCollection("z", "y", "x");

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentFailsWithDuplicateElementInActual()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "z");
            ICollection set2 = new SimpleObjectCollection("x", "y", "x");

            Assert.False(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentFailsWithDuplicateElementInExpected()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "x");
            ICollection set2 = new SimpleObjectCollection("x", "y", "z");

            Assert.False(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentHandlesNull()
        {
            ICollection set1 = new SimpleObjectCollection(null, "x", null, "z");
            ICollection set2 = new SimpleObjectCollection("z", null, "x", null);

            Assert.That(new CollectionEquivalentConstraint(set1).Matches(set2));
        }

        [Test]
        public void EquivalentHonorsIgnoreCase()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "z");
            ICollection set2 = new SimpleObjectCollection("z", "Y", "X");

            Assert.That(new CollectionEquivalentConstraint(set1).IgnoreCase.Matches(set2));
        }

#if !NETCF_2_0
        [Test]
        public void EquivalentHonorsUsing()
        {
            ICollection set1 = new SimpleObjectCollection("x", "y", "z");
            ICollection set2 = new SimpleObjectCollection("z", "Y", "X");

            Assert.That(new CollectionEquivalentConstraint(set1)
                .Using<string>((x, y) => StringUtil.Compare(x, y, true))
                .Matches(set2));
        }
#endif

#if NET_3_5 || NET_4_0
        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0,Silverlight")]
        public void WorksWithHashSets()
        {
            var hash1 = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var hash2 = new HashSet<string>(new string[] { "abracadabra", "presto", "hocuspocus" });

            Assert.That(new CollectionEquivalentConstraint(hash1).Matches(hash2));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0,Silverlight")]
        public void WorksWithHashSetAndArray()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocuspocus" };

            var constraint = new CollectionEquivalentConstraint(hash);
            Assert.That(constraint.Matches(array));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0,Silverlight")]
        public void WorksWithArrayAndHashSet()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocuspocus" };

            var constraint = new CollectionEquivalentConstraint(array);
            Assert.That(constraint.Matches(hash));
        }

        [Test, Platform("Net-3.5,Mono-3.5,Net-4.0,Mono-4.0,Silverlight")]
        public void FailureMessageWithHashSetAndArray()
        {
            var hash = new HashSet<string>(new string[] { "presto", "abracadabra", "hocuspocus" });
            var array = new string[] { "abracadabra", "presto", "hocusfocus" };

            var constraint = new CollectionEquivalentConstraint(hash);
            Assert.False(constraint.Matches(array));

            TextMessageWriter writer = new TextMessageWriter();
            constraint.WriteMessageTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(
                "  Expected: equivalent to < \"presto\", \"abracadabra\", \"hocuspocus\" >" + Environment.NewLine +
                "  But was:  < \"abracadabra\", \"presto\", \"hocusfocus\" >" + Environment.NewLine));
            Console.WriteLine(writer.ToString());
        }
#endif
    }
}