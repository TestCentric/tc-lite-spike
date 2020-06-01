// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;
using TCLite.TestData.AssertFailFixture;

namespace TCLite.Framework.AssertionTests
{
    [TestFixture]
    public class AssertFailTests : AssertionTestBase
    {
        [Test]
        public void ThrowsAssertionException()
        {
            ThrowsAssertionException(() => Assert.Fail());
        }

        [Test]
        public void ThrowsAssertionExceptionWithMessage()
        {
            ThrowsAssertionException(() => Assert.Fail("MESSAGE"), "MESSAGE");
        }

        [Test]
        public void ThrowsAssertionExceptionWithMessageAndArgs()
        {
            ThrowsAssertionException(() => Assert.Fail("MESSAGE: {0}+{1}={2}", 2, 2, 4), "MESSAGE: 2+2=4");
        }
    }
}
