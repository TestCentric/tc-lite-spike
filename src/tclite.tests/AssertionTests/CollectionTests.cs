// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.Collections;
using TCLite.Framework;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;

namespace TCLite.Framework.AssertionTests
{
    [TestFixture]
    class CollectionTests : AssertionTestBase
    {
        [Test]
        public void CanMatchTwoCollections()
        {
            ICollection expected = new SimpleObjectCollection(1, 2, 3);
            ICollection actual = new SimpleObjectCollection(1, 2, 3);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CanMatchAnArrayWithACollection()
        {
            ICollection collection = new SimpleObjectCollection(1, 2, 3);
            int[] array = new int[] { 1, 2, 3 };

            Assert.That(collection, Is.EqualTo(array));
            Assert.That(array, Is.EqualTo(collection));
        }

        [Test]
        public void FailureMatchingArrayAndCollection()
        {
            int[] expected = new int[] { 1, 2, 3 };
            ICollection actual = new SimpleObjectCollection(1, 5, 3);

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected is <System.Int32[3]>, actual is <TCLite.TestUtilities.SimpleObjectCollection> with 3 elements" + NL +
                "  Values differ at index [1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual   + "5" + NL);
        }
    }
}
