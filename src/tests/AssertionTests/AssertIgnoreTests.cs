// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
//using TCLite.TestData.AssertIgnoreData;
using TCLite.TestUtilities;

namespace TCLite.Framework.AssertionTests
{
    /// <summary>
    /// Tests of IgnoreException and Assert.Ignore
    /// </summary>
    [TestFixture]
    public class AssertIgnoreTests : AssertionTestBase
    {
        [Test]
        public void ThrowsIgnoreException()
        {
            ThrowsIgnoreException(() => Assert.Ignore());
        }

        [Test]
        public void ThrowsIgnoreExceptionWithMessage()
        {
            ThrowsIgnoreException(() => Assert.Ignore("MESSAGE"), "MESSAGE");
        }

        [Test]
        public void ThrowsIgnoreExceptionWithMessageAndArgs()
        {
            ThrowsIgnoreException(() => Assert.Ignore("MESSAGE: {0}+{1}={2}", 2, 2, 4), "MESSAGE: 2+2=4");
        }
    }
}
