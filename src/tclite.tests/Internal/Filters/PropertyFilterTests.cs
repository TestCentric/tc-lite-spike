﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace TCLite.Framework.Internal.Filters
{
    [TestFixture("Priority", "High", false)]
    [TestFixture("Author", "Charlie", true)]
    public class PropertyFilterTests : TestFilterTests
    {
        private readonly TestFilter _filter;

        public PropertyFilterTests(string name, string value, bool isRegex)
        {
            _filter = new PropertyFilter(name, value) { IsRegex = isRegex };
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
            Assert.False(_filter.Match(_yetAnotherFixture));
        }

        [Test]
        public void PassTest()
        {
            Assert.That(_filter.Pass(_topLevelSuite));
            Assert.That(_filter.Pass(_dummyFixture));
            Assert.That(_filter.Pass(_dummyFixture.Tests[0]));

            Assert.False(_filter.Pass(_anotherFixture));
            Assert.False(_filter.Pass(_yetAnotherFixture));
        }

        [Test]
        public void ExplicitMatchTest()
        {
            Assert.That(_filter.IsExplicitMatch(_topLevelSuite));
            Assert.That(_filter.IsExplicitMatch(_dummyFixture));
            Assert.False(_filter.IsExplicitMatch(_dummyFixture.Tests[0]));

            Assert.False(_filter.IsExplicitMatch(_anotherFixture));
            Assert.False(_filter.IsExplicitMatch(_yetAnotherFixture));
        }
    }
}
