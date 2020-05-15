// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using System.Globalization;

namespace TCLite.Framework.Internal
{
    [TestFixture]
    public class TextMessageWriterTests
    {
        private static readonly string NL = Environment.NewLine;

        private TextMessageWriter writer;

		[SetUp]
		public void SetUp()
        {
            writer = new TextMessageWriter();
        }

        [Test]
        public void ConnectorIsWrittenWithSurroundingSpaces()
        {
            writer.WriteConnector("and");
            Assert.That(writer.ToString(), Is.EqualTo(" and "));
        }

        [Test]
        public void PredicateIsWrittenWithTrailingSpace()
        {
            writer.WritePredicate("contains");
            Assert.That(writer.ToString(), Is.EqualTo("contains "));
        }

        [Test]
        public void IntegerIsWrittenAsIs()
        {
            writer.WriteValue(42);
            Assert.That(writer.ToString(), Is.EqualTo("42"));
        }

        [Test]
        public void StringIsWrittenWithQuotes()
        {
            writer.WriteValue("Hello");
            Assert.That(writer.ToString(), Is.EqualTo("\"Hello\""));
        }

		// This test currently fails because control character replacement is
		// done at a higher level...
		// TODO: See if we should do it at a lower level
//            [Test]
//            public void ControlCharactersInStringsAreEscaped()
//            {
//                WriteValue("Best Wishes,\r\n\tCharlie\r\n");
//                Assert.That(writer.ToString(), Is.EqualTo("\"Best Wishes,\\r\\n\\tCharlie\\r\\n\""));
//            }

        [Test]
        public void FloatIsWrittenWithTrailingF()
        {
            writer.WriteValue(0.5f);
            Assert.That(writer.ToString(), Is.EqualTo("0.5f"));
        }

        [Test]
        public void FloatIsWrittenToNineDigits()
        {
            writer.WriteValue(0.33333333333333f);
            int digits = writer.ToString().Length - 3;   // 0.dddddddddf
            Assert.That(digits, Is.EqualTo(9));
            Assert.That(writer.ToString().Length, Is.EqualTo(12));
        }

        [Test]
        public void DoubleIsWrittenWithTrailingD()
        {
            writer.WriteValue(0.5d);
            Assert.That(writer.ToString(), Is.EqualTo("0.5d"));
        }

        [Test]
        public void DoubleIsWrittenToSeventeenDigits()
        {
            writer.WriteValue(0.33333333333333333333333333333333333333333333d);
            Assert.That(writer.ToString().Length, Is.EqualTo(20)); // add 3 for leading 0, decimal and trailing d
        }

        [Test]
        public void DecimalIsWrittenWithTrailingM()
        {
            writer.WriteValue(0.5m);
            Assert.That(writer.ToString(), Is.EqualTo("0.5m"));
        }

        [Test]
        public void DecimalIsWrittenToTwentyNineDigits()
        {
            writer.WriteValue(12345678901234567890123456789m);
            Assert.That(writer.ToString(), Is.EqualTo("12345678901234567890123456789m"));
        }

		[Test]
		public void DateTimeTest()
		{
            writer.WriteValue(new DateTime(2007, 7, 4, 9, 15, 30, 123));
            Assert.That(writer.ToString(), Is.EqualTo("2007-07-04 09:15:30.123"));
		}

        [Test]
        public void DisplayStringDifferences()
        {
            string s72 = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string exp = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXY...";

            writer.DisplayStringDifferences(s72, "abcde", 5, false, true);
            string message = writer.ToString();
            Assert.That(message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + Q(exp) + NL +
                TextMessageWriter.Pfx_Actual + Q("abcde") + NL +
                "  ----------------^" + NL));
        }

        [Test]
        public void DisplayStringDifferences_NoClipping()
        {
            string s72 = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            writer.DisplayStringDifferences(s72, "abcde", 5, false, false);
            string message = writer.ToString();
            Assert.That(message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + Q(s72) + NL +
                TextMessageWriter.Pfx_Actual + Q("abcde") + NL +
                "  ----------------^" + NL));
        }

        private string Q(string s)
        {
            return "\"" + s + "\"";
        }
    }
}
