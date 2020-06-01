// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class InstanceOfTypeConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new InstanceOfTypeConstraint(typeof(D1));
            expectedDescription = string.Format("instance of <{0}>", typeof(D1));
            stringRepresentation = string.Format("<instanceof {0}>", typeof(D1));
        }

        internal object[] SuccessData = new object[] { new D1(), new D2() };

        internal object[] FailureData = new object[] { 
            new TestCaseData( new B(), "<TCLite.Framework.Constraints.Tests.InstanceOfTypeConstraintTests+B>" ) 
        };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}