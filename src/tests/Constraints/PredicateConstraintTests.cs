// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCF
using System;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class PredicateConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new PredicateConstraint<int>((x) => x < 5 );
            expectedDescription = @"value matching lambda expression";
            stringRepresentation = "<predicate>";
        }

        internal object[] SuccessData = new object[] 
        {
            0,
            -5
        };

        internal object[] FailureData = new object[]
        {
            new TestCaseData(123, "123")
        };

        [Test]
        public void CanUseConstraintExpressionSyntax()
        {
            Assert.That(123, Is.TypeOf<int>().And.Matches<int>((int x) => x > 100));
        }
    }
}
#endif
