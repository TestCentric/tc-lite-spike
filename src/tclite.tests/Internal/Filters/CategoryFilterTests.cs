﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Internal.Filters
{
    [TestFixture("Dummy", false)]
    [TestFixture("D.mmy", true)]
    public class CategoryFilterTests : TestFilterTests
    {
        private readonly TestFilter _filter;

        public CategoryFilterTests(string value, bool isRegex)
        {
            _filter = new CategoryFilter(value) { IsRegex = isRegex };
        }
        [Test]
        public void IsNotEmpty()
        {
            Assert.False(_filter.IsEmpty);
        }

        [Test]
        public void MatchTest()
        {
            Assert.That(_filter.Match(_dummyFixture));
            Assert.False(_filter.Match(_anotherFixture));
        }

        [Test]
        public void PassTest()
        {
            Assert.That(_filter.Pass(_topLevelSuite));
            Assert.That(_filter.Pass(_dummyFixture));
            Assert.That(_filter.Pass(_dummyFixture.Tests[0]));

            Assert.False(_filter.Pass(_anotherFixture));
        }

        [Test]
        public void ExplicitMatchTest()
        {
            Assert.That(_filter.IsExplicitMatch(_topLevelSuite));
            Assert.That(_filter.IsExplicitMatch(_dummyFixture));
            Assert.False(_filter.IsExplicitMatch(_dummyFixture.Tests[0]));

            Assert.False(_filter.IsExplicitMatch(_anotherFixture));
        }
    }
}
