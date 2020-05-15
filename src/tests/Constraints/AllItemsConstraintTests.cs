// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;
using RangeConstraint = TCLite.Framework.Constraints.RangeConstraint<int>;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class AllItemsConstraintTests
    {
        private static readonly string NL = Environment.NewLine;

        [Test]
        public void AllItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.OSVersion };
            Assert.That(c, new AllItemsConstraint(Is.Not.Null));
        }

        [Test]
        public void AllItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(c, new AllItemsConstraint(new NotConstraint(new EqualConstraint(null))));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "all items not null" + NL +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + NL));
        }

        [Test]
        public void AllItemsAreInRange()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100)));
        }

        [Test]
        public void AllItemsAreInRange_UsingIComparer()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(new SimpleObjectComparer())));
        }

        [Test]
        public void AllItemsAreInRange_UsingIComparerOfT()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(new SimpleObjectComparer())));
        }

        [Test]
        public void AllItemsAreInRange_UsingComparisonOfT()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100).Using(new SimpleObjectComparer())));
        }

        [Test]
        public void AllItemsAreInRangeFailureMessage()
        {
            int[] c = new int[] { 12, 27, 19, 32, 107, 99, 26 };
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(c, new AllItemsConstraint(new RangeConstraint(10, 100)));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "all items in range (10,100)" + NL +
                TextMessageWriter.Pfx_Actual + "< 12, 27, 19, 32, 107, 99, 26 >" + NL));
        }

        [Test]
        public void AllItemsAreInstancesOfType()
        {
            object[] c = new object[] { 'a', 'b', 'c' };
            Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
        }

        [Test]
        public void AllItemsAreInstancesOfTypeFailureMessage()
        {
            object[] c = new object[] { 'a', "b", 'c' };
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(c, new AllItemsConstraint(new InstanceOfTypeConstraint(typeof(char))));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "all items instance of <System.Char>" + NL +
                TextMessageWriter.Pfx_Actual + "< 'a', \"b\", 'c' >" + NL));
        }
    }
}