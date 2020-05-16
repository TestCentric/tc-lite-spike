// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    public abstract class ConstraintTestBaseNoData
    {
        protected Constraint theConstraint;
        protected string expectedDescription = "<NOT SET>";
        protected string stringRepresentation = "<NOT SET>";

        [Test]
        public void ProvidesProperDescription()
        {
            TextMessageWriter writer = new TextMessageWriter();
            theConstraint.WriteDescriptionTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(expectedDescription));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(theConstraint.ToString(), Is.EqualTo(stringRepresentation));
        }
    }

    public abstract class ConstraintTestBase : ConstraintTestBaseNoData
    {
        [Test, TestCaseSource("SuccessData")]
        public void SucceedsWithGoodValues(object value)
        {
            if (!theConstraint.Matches(value))
            {
                MessageWriter writer = new TextMessageWriter();
                theConstraint.WriteMessageTo(writer);
                Assert.Fail(writer.ToString());
            }
        }

        [Test, TestCaseSource("FailureData")]
        public void FailsWithBadValues(object badValue, string message)
        {
            string NL = Environment.NewLine;

            Assert.False(theConstraint.Matches(badValue));

            TextMessageWriter writer = new TextMessageWriter();
            theConstraint.WriteMessageTo(writer);
            Assert.That( writer.ToString(), Is.EqualTo(
                TextMessageWriter.Pfx_Expected + expectedDescription + NL +
                TextMessageWriter.Pfx_Actual + message + NL ));
        }
    }

    /// <summary>
    /// Base class for testing constraints that can throw an ArgumentException
    /// </summary>
    public abstract class ConstraintTestBaseWithArgumentException : ConstraintTestBase
    {
        [Test, TestCaseSource("InvalidData")]
        public void InvalidDataThrowsArgumentException(object value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                theConstraint.Matches(value);
            });
        }
    }

    /// <summary>
    /// Base class for tests that can throw multiple exceptions. Use
    /// TestCaseData class to specify the expected exception type.
    /// </summary>
    public abstract class ConstraintTestBaseWithExceptionTests : ConstraintTestBase
    {
        [Test, TestCaseSource("InvalidData")]
        public void InvalidDataThrowsException(object value)
        {
            theConstraint.Matches(value);
        }
    }
}