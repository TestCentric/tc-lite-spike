// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Internal.Filters
{
    public class MultipleFilterTests : TestFilterTests
    {
        [Test]
        public void TestNestedAndFilters()
        {
            var filter = new AndFilter(
                new CategoryFilter("Dummy"),
                new PropertyFilter("Priority", "High"));

            Assert.That(filter.Match(_dummyFixture));
            Assert.That(filter.IsExplicitMatch(_dummyFixture));

            Assert.False(filter.Match(_anotherFixture));
            Assert.False(filter.IsExplicitMatch(_anotherFixture));

            Assert.False(filter.Match(_yetAnotherFixture));
            Assert.False(filter.IsExplicitMatch(_yetAnotherFixture));

            Assert.False(filter.Match(_explicitFixture));
            Assert.False(filter.IsExplicitMatch(_explicitFixture));
        }

        [Test]
        public void TestNestedNotCategoryFilters()
        {
            var filter = new NotFilter(new CategoryFilter("NotDummy"));

            Assert.That(filter.Match(_dummyFixture));
            Assert.False(filter.IsExplicitMatch(_dummyFixture));

            Assert.That(filter.Match(_anotherFixture));
            Assert.False(filter.IsExplicitMatch(_anotherFixture));

            Assert.That(filter.Match(_yetAnotherFixture));
            Assert.False(filter.IsExplicitMatch(_yetAnotherFixture));

            Assert.That(filter.Match(_explicitFixture));
            Assert.False(filter.IsExplicitMatch(_explicitFixture));
        }

        [Test]
        public void TestNestedAndOrFilters()
        {
            var filter = new AndFilter(
                new NotFilter(new CategoryFilter("NotDummy")),
                new OrFilter(
                    new PropertyFilter("Priority", "High"),
                    new PropertyFilter("Priority", "Low")));

            Assert.That(filter.Match(_dummyFixture));
            Assert.False(filter.IsExplicitMatch(_dummyFixture));

            Assert.That(filter.Match(_anotherFixture));
            Assert.False(filter.IsExplicitMatch(_anotherFixture));

            Assert.False(filter.Match(_yetAnotherFixture));
            Assert.False(filter.IsExplicitMatch(_yetAnotherFixture));

            Assert.False(filter.Match(_explicitFixture));
            Assert.False(filter.IsExplicitMatch(_explicitFixture));
        }

        [Test]
        public void TestNestedOrNotFilters()
        {
            var filter = new OrFilter(
                new CategoryFilter("Dummy"),
                new NotFilter(new CategoryFilter("Dummy")));

            Assert.That(filter.Match(_dummyFixture));
            Assert.That(filter.IsExplicitMatch(_dummyFixture));

            Assert.That(filter.Match(_anotherFixture));
            Assert.False(filter.IsExplicitMatch(_anotherFixture));

            Assert.That(filter.Match(_yetAnotherFixture));
            Assert.False(filter.IsExplicitMatch(_yetAnotherFixture));

            Assert.That(filter.Match(_explicitFixture));
            Assert.False(filter.IsExplicitMatch(_explicitFixture));
        }

        [Test]
        public void TestLotsOfNestedOrFilters()
        {
            var filter = new NotFilter(
                new NotFilter(
                    new NotFilter(
                        new NotFilter(new CategoryFilter("Dummy")))));

            Assert.That(filter.Match(_dummyFixture));
            Assert.False(filter.IsExplicitMatch(_dummyFixture));

            Assert.False(filter.Match(_anotherFixture));
            Assert.False(filter.IsExplicitMatch(_anotherFixture));

            Assert.False(filter.Match(_yetAnotherFixture));
            Assert.False(filter.IsExplicitMatch(_yetAnotherFixture));

            Assert.False(filter.Match(_explicitFixture));
            Assert.False(filter.IsExplicitMatch(_explicitFixture));
        }
    }
}
