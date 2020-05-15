// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class EndsWithConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EndsWithConstraint("hello");
            expectedDescription = "String ending with \"hello\"";
            stringRepresentation = "<endswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "hello", "I said hello" };

        internal object[] FailureData = new object[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "hello there", "\"hello there\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }

    [TestFixture]
    public class EndsWithConstraintTestsIgnoringCase : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new EndsWithConstraint("hello").IgnoreCase;
            expectedDescription = "String ending with \"hello\", ignoring case";
            stringRepresentation = "<endswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "HELLO", "I said Hello" };

        internal object[] FailureData = new object[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( "hello there", "\"hello there\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }
}
