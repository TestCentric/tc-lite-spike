// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class StartsWithConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new StartsWithConstraint("hello");
            expectedDescription = "String starting with \"hello\"";
            stringRepresentation = "<startswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "hello", "hello there" };

        internal object[] FailureData = new object[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "HELLO THERE", "\"HELLO THERE\"" ),
            new TestCaseData( "I said hello", "\"I said hello\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }

    [TestFixture]
    public class StartsWithConstraintTestsIgnoringCase : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new StartsWithConstraint("hello").IgnoreCase;
            expectedDescription = "String starting with \"hello\", ignoring case";
            stringRepresentation = "<startswith \"hello\">";
        }

        internal object[] SuccessData = new object[] { "Hello", "HELLO there" };

        internal object[] FailureData = new object[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( "I said hello", "\"I said hello\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }
}
