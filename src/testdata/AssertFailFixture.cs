// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.AssertFailFixture
{
	[TestFixture]
	public class AssertFailFixture
	{
        [Test]
        public void CallAssertFail()
        {
            Assert.Fail();
        }

        [Test]
        public void CallAssertFailWithMessage()
        {
            Assert.Fail("MESSAGE");
        }

        [Test]
        public void CallAssertFailWithMessageAndArgs()
        {
            Assert.Fail("MESSAGE: {0}+{1}={2}", 2, 2, 4);
        }
    }
}
