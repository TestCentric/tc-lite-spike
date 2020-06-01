// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.TestUtilities;

namespace TCLite.Framework.Internal.Filters
{
    // Filter XML formats
    //
    // Empty Filter:
    //    <filter/>
    //
    // Id Filter:
    //    <id>1</id>
    //    <id>1,2,3</id>
    // 
    // TestName filter
    //    <test>xxxxxxx.xxx</test>
    //
    // Name filter
    //    <name>xxxxx</name>
    //
    // Namespace filter
    //    <namespace>xxxxx</namespace>
    //
    // Category filter 
    //    <cat>cat1</cat>
    //    <cat>cat1,cat2,cat3</cat>
    //
    // Property filter
    //    <prop name="xxxx">value</prop>
    //
    // And Filter
    //    <and><filter>...</filter><filter>...</filter></and>
    //    <filter><filter>...</filter><filter>...</filter></filter>
    //
    // Or Filter
    //    <or><filter>...</filter><filter>...</filter></or>

    public abstract class TestFilterTests
    {
        public const string DUMMY_CLASS = "TCLite.Framework.Internal.Filters.TestFilterTests+DummyFixture";

        protected readonly TestSuite _dummyFixture = TestBuilder.MakeFixture(typeof(DummyFixture));
        protected readonly TestSuite _anotherFixture = TestBuilder.MakeFixture(typeof(AnotherFixture));
        protected readonly TestSuite _yetAnotherFixture = TestBuilder.MakeFixture(typeof(YetAnotherFixture));
        protected readonly TestSuite _fixtureWithMultipleTests = TestBuilder.MakeFixture (typeof (FixtureWithMultipleTests));
        protected readonly TestSuite _nestingFixture = TestBuilder.MakeFixture(typeof(NestingFixture));
        protected readonly TestSuite _nestedFixture = TestBuilder.MakeFixture(typeof(NestingFixture.NestedFixture));
        protected readonly TestSuite _emptyNestedFixture = TestBuilder.MakeFixture(typeof(NestingFixture.EmptyNestedFixture));
        protected readonly TestSuite _topLevelSuite = new TestSuite("MySuite");
        protected readonly TestSuite _explicitFixture = TestBuilder.MakeFixture(typeof(ExplicitFixture));
        protected readonly TestSuite _specialFixture = TestBuilder.MakeFixture(typeof(SpecialCharactersFixture));

        [OneTimeSetUp]
        public void SetUpSuite()
        {
            _topLevelSuite.Add(_dummyFixture);
            _topLevelSuite.Add(_anotherFixture);
            _topLevelSuite.Add(_yetAnotherFixture);
            _topLevelSuite.Add(_fixtureWithMultipleTests);
            _topLevelSuite.Add(_nestingFixture);

            _nestingFixture.Add(_nestedFixture);
            _nestingFixture.Add(_emptyNestedFixture);
        }

        #region Fixtures Used by Tests

        [Category("Dummy"), Property("Priority", "High"), Property("Author", "Charlie Poole")]
        private class DummyFixture
        {
            [Test]
            public void Test() { }

        }


        [Category("Special,Character-Fixture+!")]
        private class SpecialCharactersFixture
        {
            [Test]
            public void Test() { }
        }

        [Category("Another"), Property("Priority", "Low"), Property("Author", "Fred Smith")]
        private class AnotherFixture
        {
            [Test]
            public void Test() { }
        }

        private class YetAnotherFixture
        { }

        private class FixtureWithMultipleTests
        {
            [Test]
            public void Test1 () {}

            [Test, Category ("Dummy")]
            public void Test2 () {}
        }

        private class NestingFixture
        {
            public class NestedFixture
            {
                [Test]
                public void Test() { }
            }

            internal class EmptyNestedFixture { }
        }

        [Explicit]
        private class ExplicitFixture
        { }
        #endregion
    }
}
