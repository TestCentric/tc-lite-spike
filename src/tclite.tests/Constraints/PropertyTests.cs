// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints.Tests
{
    public class PropertyExistsTest : ConstraintTestBaseWithExceptionTests
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new PropertyExistsConstraint("Length");
            expectedDescription = "property Length";
            stringRepresentation = "<propertyexists Length>";
        }

        internal static object[] SuccessData = new object[] { new int[0], "hello", typeof(Array) };

        internal static object[] FailureData = new object[] { 
            new TestCaseData( 42, "<System.Int32>" ),
            new TestCaseData( new SimpleObjectCollection(), "<TCLite.TestUtilities.SimpleObjectCollection>" ),
            new TestCaseData( typeof(Int32), "<System.Int32>" ) };
    }

    public class PropertyTest : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new PropertyConstraint("Length", new EqualConstraint(5));
            expectedDescription = "property Length equal to 5";
            stringRepresentation = "<property Length <equal 5>>";
        }

        internal static object[] SuccessData = new object[] { new int[5], "hello" };

        internal static object[] FailureData = new object[] { 
            new TestCaseData( new int[3], "3" ),
            new TestCaseData( "goodbye", "7" ) };

        [Test]
        public void PropertyEqualToValueWithTolerance()
        {
            Constraint c = new EqualConstraint(105m).Within(0.1m);
            TextMessageWriter w = new TextMessageWriter();
            c.WriteDescriptionTo(w);
            Assert.That(w.ToString(), Is.EqualTo("105m +/- 0.1m"));

            c = new PropertyConstraint("D", new EqualConstraint(105m).Within(0.1m));
            w = new TextMessageWriter();
            c.WriteDescriptionTo(w);
            Assert.That(w.ToString(), Is.EqualTo("property D equal to 105m +/- 0.1m"));
        }
    }
}
