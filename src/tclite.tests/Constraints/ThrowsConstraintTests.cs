// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class ThrowsConstraintTest_ExactType : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new ThrowsConstraint(
                new ExactTypeConstraint(typeof(ArgumentException)));
            expectedDescription = "<System.ArgumentException>";
            stringRepresentation = "<throws <typeof System.ArgumentException>>";
        }

        internal static object[] SuccessData = new object[]
        {
            new TestDelegate( TestDelegates.ThrowsArgumentException )
        };

        internal static object[] FailureData = new object[]
        {
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsNothing ), "no exception thrown" ),
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsSystemException ), "<System.Exception>" )
        };
    }

    [TestFixture]
    public class ThrowsConstraintTest_InstanceOfType : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new ThrowsConstraint(
                new InstanceOfTypeConstraint(typeof(TestDelegates.CustomException)));
            expectedDescription = "instance of <TCLite.TestUtilities.TestDelegates+CustomException>";
            stringRepresentation = "<throws <instanceof TCLite.TestUtilities.TestDelegates+CustomException>>";
        }

        internal static object[] SuccessData = new object[]
        {
            new TestDelegate( TestDelegates.ThrowsCustomException ),
            new TestDelegate( TestDelegates.ThrowsDerivedCustomException )
        };

        internal static object[] FailureData = new object[]
        {
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsArgumentException ), "<System.ArgumentException>" ),
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsNothing ), "no exception thrown" ),
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsSystemException ), "<System.Exception>" )
        };
    }

// TODO: Find a different example for use with NETCF - ArgumentException does not have a ParamName member
#if !NETCF && !SILVERLIGHT
    public class ThrowsConstraintTest_WithConstraint : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new ThrowsConstraint(
                new AndConstraint(
                    new ExactTypeConstraint(typeof(ArgumentException)),
                    new PropertyConstraint("ParamName", new EqualConstraint("myParam"))));
            expectedDescription = @"<System.ArgumentException> and property ParamName equal to ""myParam""";
            stringRepresentation = @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>";
        }

        internal static object[] SuccessData = new object[]
        {
            new TestDelegate( TestDelegates.ThrowsArgumentException )
        };

        internal static object[] FailureData = new object[]
        {
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsCustomException ), "<TCLite.TestUtilities.TestDelegates+CustomException>" ),
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsNothing ), "no exception thrown" ),
            new TestCaseData( new TestDelegate( TestDelegates.ThrowsSystemException ), "<System.Exception>" )
        };
    }
#endif
}
