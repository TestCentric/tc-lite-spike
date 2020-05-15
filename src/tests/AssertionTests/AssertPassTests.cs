// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.AssertionTests
{
    public class AssertPassTests : AssertionTestBase
    {
        [Test]
        public void ThrowsSuccessException()
        {
            ThrowsSuccessException(() => Assert.Pass());
        }

        [Test,]
        public void ThrowsSuccessExceptionWithMessage()
        {
            ThrowsSuccessException(() => Assert.Pass("MESSAGE"), "MESSAGE");
        }

        [Test]
        public void AssertPassReturnsSuccess()
        {
            Assert.Pass("This test is OK!");
        }

        [Test]
        public void SubsequentFailureIsIrrelevant()
        {
            Assert.Pass("This test is OK!");
            Assert.Fail("No it's NOT!");
        }
    }
}
