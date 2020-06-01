// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class AssignableFromConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new AssignableFromConstraint(typeof(D1));
            expectedDescription = string.Format("assignable from <{0}>", typeof(D1));
            stringRepresentation = string.Format("<assignablefrom {0}>", typeof(D1));
        }

        internal object[] SuccessData = new object[] { new D1(), new B() };

        internal object[] FailureData = new object[] { 
            new TestCaseData( new D2(), "<TCLite.Framework.Constraints.Tests.AssignableFromConstraintTests+D2>" ) };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}