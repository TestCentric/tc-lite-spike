// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class AndConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new AndConstraint(new GreaterThanConstraint(40), new LessThanConstraint(50));
            expectedDescription = "greater than 40 and less than 50";
            stringRepresentation = "<and <greaterthan 40> <lessthan 50>>";
        }

		internal object[] SuccessData = new object[] { 42 };

        internal object[] FailureData = new object[] { new object[] { 37, "37" }, new object[] { 53, "53" } };

		[Test]
        public void CanCombineTestsWithAndOperator()
        {
            Assert.That(42, new GreaterThanConstraint(40) & new LessThanConstraint(50));
        }
    }
}