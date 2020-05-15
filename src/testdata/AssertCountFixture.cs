﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework;

namespace TCLite.TestData
{
    public class AssertCountFixture
    {
        public static readonly int ExpectedAssertCount = 5;

        [Test]
        public void BooleanAssert()
        {
            Assert.That(2 + 2 == 4);
        }
        [Test]
        public void ConstraintAssert()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }
        [Test]
        public void ThreeAsserts()
        {
            Assert.That(2 + 2 == 4);
            Assert.That(2 + 2, Is.EqualTo(4));
            Assert.That(2 + 2, Is.EqualTo(5));
        }
    }
}
