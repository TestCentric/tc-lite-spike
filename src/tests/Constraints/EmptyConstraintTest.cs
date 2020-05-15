// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class EmptyConstraintTest : ConstraintTestBaseWithArgumentException
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EmptyConstraint();
            expectedDescription = "<empty>";
            stringRepresentation = "<empty>";
        }

        internal static object[] SuccessData = new object[] 
        {
            new System.Collections.Generic.List<int>(),
            string.Empty,
            new object[0],
            new SimpleObjectCollection()
        };

        internal static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( new object[] { 1, 2, 3 }, "< 1, 2, 3 >" )
        };

        internal static object[] InvalidData = new object[]
            {
                null,
                5
            };
    }

    [TestFixture]
    public class NullOrEmptyStringConstraintTest : ConstraintTestBaseWithArgumentException
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new NullOrEmptyStringConstraint();
            expectedDescription = "null or empty string";
            stringRepresentation = "<nullorempty>";
        }

        internal static object[] SuccessData = new object[] 
        {
            string.Empty,
            null
        };

        internal static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" )
        };

        internal static object[] InvalidData = new object[]
            {
                5
            };
    }
}