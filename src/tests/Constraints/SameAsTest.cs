// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class SameAsTest : ConstraintTestBase
    {
        private static readonly object obj1 = new object();
        private static readonly object obj2 = new object();

        [SetUp]
        public void SetUp()
        {
            theConstraint = new SameAsConstraint(obj1);
            expectedDescription = "same as <System.Object>";
            stringRepresentation = "<sameas System.Object>";
        }

        internal static object[] SuccessData = new object[] { obj1 };

        internal static object[] FailureData = new object[] { 
            new TestCaseData( obj2, "<System.Object>" ),
            new TestCaseData( 3, "3" ),
            new TestCaseData( "Hello", "\"Hello\"" ) };
    }
}