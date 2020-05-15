// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.AssertionTests
{
    public class AssertInconclusiveTests : AssertionTestBase
    {
        [Test]
        public void ThrowsInconclusiveException()
        {
            ThrowsInconclusiveException(() => Assert.Inconclusive());
        }

        [Test]
        public void ThrowsInconclusiveExceptionWithMessage()
        {
            ThrowsInconclusiveException(() => Assert.Inconclusive("MESSAGE"), "MESSAGE");
        }

        [Test]
        public void ThrowsInconclusiveExceptionWithMessageAndArgs()
        {
            ThrowsInconclusiveException(() => Assert.Inconclusive("MESSAGE: {0}+{1}={2}", 2, 2, 4), "MESSAGE: 2+2=4");
        }
    }
}
