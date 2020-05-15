// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;

namespace TCLite.Framework.Internal
{
    [TestFixture]
    public class PropertyBagTests
    {
        PropertyBag bag;

        [SetUp]
        public void SetUp()
        {
            bag = new PropertyBag();
            bag.Add("Answer", 42);
            bag.Add("Tag", "bug");
            bag.Add("Tag", "easy");
        }

        [Test]
        public void CountReflectsNumberOfPairs()
        {
            Assert.That(bag.Count, Is.EqualTo(3));
        }

        [Test]
        public void IndexGetsListOfValues()
        {
            Assert.That(bag["Answer"].Count, Is.EqualTo(1));
            Assert.That(bag["Answer"], Contains.Item(42));

            Assert.That(bag["Tag"].Count, Is.EqualTo(2));
            Assert.That(bag["Tag"], Contains.Item("bug"));
            Assert.That(bag["Tag"], Contains.Item("easy"));
        }

        [Test]
        public void IndexGetsEmptyListIfNameIsNotPresent()
        {
            Assert.That(bag["Level"].Count, Is.EqualTo(0));
        }

        [Test]
        public void IndexSetsListOfValues()
        {
            bag["Zip"] = new string[] {"junk", "more junk"};
            Assert.That(bag["Zip"].Count, Is.EqualTo(2));
            Assert.That(bag["Zip"], Contains.Item("junk"));
            Assert.That(bag["Zip"], Contains.Item("more junk"));
        }

        [Test]
        public void CanClearTheBag()
        {
            bag.Clear();
            Assert.That(bag.Keys.Count, Is.EqualTo(0));
            Assert.That(bag.Count, Is.EqualTo(0));
        }

        [Test]
        public void AllKeysAreListed()
        {
            Assert.That(bag.Keys.Count, Is.EqualTo(2));
            Assert.That(bag.Keys, Has.Member("Answer"));
            Assert.That(bag.Keys, Has.Member("Tag"));
        }

        [Test]
        public void ContainsKey()
        {
            Assert.That(bag.ContainsKey("Answer"));
            Assert.That(bag.ContainsKey("Tag"));
            Assert.False(bag.ContainsKey("Target"));
        }

        [Test]
        public void ContainsKeyAndValue()
        {
            Assert.That(bag.Contains("Answer", 42));
        }

        [Test]
        public void ContainsPropertyEntry()
        {
            Assert.That(bag.Contains(new PropertyEntry("Answer", 42)));
        }

        [Test]
        public void CanRemoveKey()
        {
            bag.Remove("Tag");
            Assert.That(bag.Keys.Count, Is.EqualTo(1));
            Assert.That(bag.Count, Is.EqualTo(1));
            Assert.That(bag.Keys, Has.No.Member("Tag"));
        }

        [Test]
        public void CanRemoveMissingKeyWithoutError()
        {
            bag.Remove("Zip");
        }

        [Test]
        public void CanRemoveNameAndValue()
        {
            bag.Remove("Tag", "easy");
            Assert.That(bag["Tag"].Contains("bug"));
            Assert.False(bag["Tag"].Contains("easy"));
            Assert.That(bag.Count, Is.EqualTo(2));
        }

        [Test]
        public void CanRemoveNameAndMissingValueWithoutError()
        {
            bag.Remove("Tag", "wishlist");
        }

        [Test]
        public void CanRemovePropertyEntry()
        {
            bag.Remove(new PropertyEntry("Tag", "easy"));
            Assert.That(bag["Tag"].Contains("bug"));
            Assert.False(bag["Tag"].Contains("easy"));
            Assert.That(bag.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetReturnsSingleValue()
        {
            Assert.That(bag.Get("Answer"), Is.EqualTo(42));
            Assert.That(bag.Get("Tag"), Is.EqualTo("bug"));
        }

        [Test]
        public void SetAddsNewSingleValue()
        {
            bag.Set("Zip", "ZAP");
            Assert.That(bag["Zip"].Count, Is.EqualTo(1));
            Assert.That(bag["Zip"], Has.Member("ZAP"));
            Assert.That(bag.Get("Zip"), Is.EqualTo("ZAP"));
        }

        [Test]
        public void SetReplacesOldValues()
        {
            bag.Set("Tag", "ZAPPED");
            Assert.That(bag["Tag"].Count, Is.EqualTo(1));
            Assert.That(bag.Get("Tag"), Is.EqualTo("ZAPPED"));
        }

        [Test]
        public void EnumeratorReturnsAllEntries()
        {
            int count = 0;
            // NOTE: Ignore unsuppressed warning about entry in .NET 1.1 build
            foreach (PropertyEntry entry in bag)
                ++count;
            Assert.That(count, Is.EqualTo(3));

        }
    }
}
