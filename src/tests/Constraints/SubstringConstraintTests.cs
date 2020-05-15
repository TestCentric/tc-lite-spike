// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class SubstringConstraintTests : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            theConstraint = new SubstringConstraint("hello");
            expectedDescription = "String containing \"hello\"";
            stringRepresentation = "<substring \"hello\">";
        }

        internal object[] SuccessData = new object[] { "hello", "hello there", "I said hello", "say hello to fred" };

        internal object[] FailureData = new object[] { 
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "HELLO", "\"HELLO\"" ),
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null, "null" ) };

        public void HandleException(Exception ex)
        {
            string NL = Environment.NewLine;

            Assert.That(ex.Message, new EqualConstraint(
                TextMessageWriter.Pfx_Expected + "String containing \"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa...\"" + NL +
                TextMessageWriter.Pfx_Actual   + "\"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx...\"" + NL));
        }
    }

    [TestFixture]
    public class SubstringConstraintTestsIgnoringCase : ConstraintTestBase
    {
		[SetUp]
        public void SetUp()
        {
            theConstraint = new SubstringConstraint("hello").IgnoreCase;
            expectedDescription = "String containing \"hello\", ignoring case";
            stringRepresentation = "<substring \"hello\">";
        }

        internal object[] SuccessData = new object[] { "Hello", "HellO there", "I said HELLO", "say hello to fred" };

        internal object[] FailureData = new object[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null, "null" ) };
    }

    //[TestFixture]
    //public class EqualIgnoringCaseTest : ConstraintTest
    //{
    //    [SetUp]
    //    public void SetUp()
    //    {
    //        Matcher = new EqualConstraint("Hello World!").IgnoreCase;
    //        Description = "\"Hello World!\", ignoring case";
    //    }

    //    internal object[] SuccessData = new object[] { "hello world!", "Hello World!", "HELLO world!" };

    //    internal object[] FailureData = new object[] { "goodbye", "Hello Friends!", string.Empty, null };


    //    internal string[] ActualValues = new string[] { "\"goodbye\"", "\"Hello Friends!\"", "<string.Empty>", "null" };
    //}
}
