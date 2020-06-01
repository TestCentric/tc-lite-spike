// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class ExactTypeConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new ExactTypeConstraint(typeof(D1));
            expectedDescription = string.Format("<{0}>", typeof(D1));
            stringRepresentation = string.Format("<typeof {0}>", typeof(D1));
        }

        internal object[] SuccessData = new object[] { new D1() };

        internal object[] FailureData = new object[] { 
            new TestCaseData( new B(), "<TCLite.Framework.Constraints.Tests.ExactTypeConstraintTests+B>" ),
            new TestCaseData( new D2(), "<TCLite.Framework.Constraints.Tests.ExactTypeConstraintTests+D2>" )
        };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}