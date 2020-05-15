// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class NullConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new NullConstraint();
            stringRepresentation = "<null>";
            expectedDescription = "null";
        }

        internal object[] SuccessData = new object[] { null };

        internal object[] FailureData = new object[] { new object[] { "hello", "\"hello\"" } };
    }

    [TestFixture]
    public class TrueConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new TrueConstraint();
            stringRepresentation = "<true>";
            expectedDescription = "True";
        }

        internal object[] SuccessData = new object[] { true, 2 + 2 == 4 };

        internal object[] FailureData = new object[] { 
            new object[] { null, "null" }, 
            new object[] { "hello", "\"hello\"" },
            new object[] { false, "False" },
            new object[] { 2 + 2 == 5, "False" } };
    }

    [TestFixture]
    public class FalseConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new FalseConstraint();
            stringRepresentation = "<false>";
            expectedDescription = "False";
        }

        internal object[] SuccessData = new object[] { false, 2 + 2 == 5 };

        internal object[] FailureData = new object[] { 
            new object[] { null, "null" },
            new object[] { "hello", "\"hello\"" },
            new object[] { true, "True" },
            new object[] { 2 + 2 == 4, "True" } };
    }

    [TestFixture]
    public class NaNConstraintTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new NaNConstraint();
            stringRepresentation = "<nan>";
            expectedDescription = "NaN";
        }

        internal object[] SuccessData = new object[] { double.NaN, float.NaN };

        internal object[] FailureData = new object[] { 
            new object[] { null, "null" },
            new object[] { "hello", "\"hello\"" },
            new object[] { 42, "42" },
            new object[] { double.PositiveInfinity, "Infinity" },
            new object[] { double.NegativeInfinity, "-Infinity" },
            new object[] { float.PositiveInfinity, "Infinity" },
            new object[] { float.NegativeInfinity, "-Infinity" } };
    }
}
