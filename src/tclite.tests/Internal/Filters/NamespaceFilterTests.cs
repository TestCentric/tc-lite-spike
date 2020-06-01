// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Internal.Filters
{
    [TestFixture("TCLite.Framework.Internal.Filters", false, true)]
    [TestFixture("TCLite.Framework.*", true, true)]
    [TestFixture("TCLite.Framework", false, false)]
    public class NamespaceFilterTests : TestFilterTests
    {
        private readonly TestFilter _filter;
        private readonly bool _shouldMatch;

        public NamespaceFilterTests(string value, bool isRegex, bool shouldMatch)
        {
            _filter = new NamespaceFilter(value) { IsRegex = isRegex };
            _shouldMatch = shouldMatch;
        }

        [Test]
        public void IsNotEmpty()
        {
            Assert.False(_filter.IsEmpty);
        }

        [Test]
        public void MatchTest()
        {
            Assert.That(_filter.Match(_dummyFixture.Tests[0]), Is.EqualTo(_shouldMatch));
        }

        [Test]
        public void PassTest()
        {
            Assert.That(_filter.Pass(_nestingFixture), Is.EqualTo(_shouldMatch));
            Assert.That(_filter.Pass(_nestedFixture), Is.EqualTo(_shouldMatch));
            Assert.That(_filter.Pass(_emptyNestedFixture), Is.EqualTo(_shouldMatch));

            Assert.That(_filter.Pass(_topLevelSuite), Is.EqualTo(_shouldMatch));
            Assert.That(_filter.Pass(_dummyFixture), Is.EqualTo(_shouldMatch));
            Assert.That(_filter.Pass(_dummyFixture.Tests[0]), Is.EqualTo(_shouldMatch));
        }

    }
}
