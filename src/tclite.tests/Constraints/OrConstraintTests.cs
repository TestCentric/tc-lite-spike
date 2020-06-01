// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class OrConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new OrConstraint(new EqualConstraint(42), new EqualConstraint(99));
            expectedDescription = "42 or 99";
            stringRepresentation = "<or <equal 42> <equal 99>>";
        }

        internal object[] SuccessData = new object[] { 99, 42 };

        internal object[] FailureData = new object[] { new object[] { 37, "37" } };

		[Test]
        public void CanCombineTestsWithOrOperator()
        {
            Assert.That(99, new EqualConstraint(42) | new EqualConstraint(99) );
        }
    }
}