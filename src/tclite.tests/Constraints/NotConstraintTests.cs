// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Constraints;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class NotConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new NotConstraint( new EqualConstraint(null) );
            expectedDescription = "not null";
            stringRepresentation = "<not <equal null>>";
        }

        internal object[] SuccessData = new object[] { 42, "Hello" };

        internal object[] FailureData = new object[] { new object[] { null, "null" } };

        [Test]
        public void NotHonorsIgnoreCaseUsingConstructors()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That("abc", new NotConstraint(new EqualConstraint("ABC").IgnoreCase));
            });

            Assert.That(ex.Message, Contains.Substring("ignoring case"));
        }

        [Test]
        public void NotHonorsIgnoreCaseUsingPrefixNotation()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( "abc", Is.Not.EqualTo( "ABC" ).IgnoreCase );
            });

            Assert.That(ex.Message, Contains.Substring("ignoring case"));
        }

        [Test]
        public void NotHonorsTolerance()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( 4.99d, Is.Not.EqualTo( 5.0d ).Within( .05d ) );
            });

            Assert.That(ex.Message, Contains.Substring("+/-"));
        }

        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint(99));
        }
    }
}