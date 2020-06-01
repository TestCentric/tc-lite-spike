// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Assertions;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    public class ExactCountConstraintTests
    {
        private static readonly string NL = Environment.NewLine;
        private static readonly string[] names = new string[] { "Charlie", "Fred", "Joe", "Charlie" };

        [Test]
        public void ZeroItemsMatch()
        {
            Assert.That(names, new ExactCountConstraint(0, Is.EqualTo("Sam")));
            Assert.That(names, Has.Exactly(0).EqualTo("Sam"));
        }

        [Test]
        public void ZeroItemsMatchFails()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(names, new ExactCountConstraint(0, Is.EqualTo("Charlie")));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "no item \"Charlie\"" + NL +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + NL));
        }

        [Test]
        public void ExactlyOneItemMatches()
        {
            Assert.That(names, new ExactCountConstraint(1, Is.EqualTo("Fred")));
            Assert.That(names, Has.Exactly(1).EqualTo("Fred"));
        }

        [Test]
        public void ExactlyOneItemMatchFails()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(names, new ExactCountConstraint(1, Is.EqualTo("Charlie")));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "exactly one item \"Charlie\"" + NL +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + NL ));
        }

        [Test]
        public void ExactlyTwoItemsMatch()
        {
            Assert.That(names, new ExactCountConstraint(2, Is.EqualTo("Charlie")));
            Assert.That(names, Has.Exactly(2).EqualTo("Charlie"));
        }

        [Test]
        public void ExactlyTwoItemsMatchFails()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That(names, new ExactCountConstraint(2, Is.EqualTo("Fred")));
            });

            Assert.That(ex.Message, Is.EqualTo(
                TextMessageWriter.Pfx_Expected + "exactly 2 items \"Fred\"" + NL +
                TextMessageWriter.Pfx_Actual + "< \"Charlie\", \"Fred\", \"Joe\", \"Charlie\" >" + NL));
        }
    }
}