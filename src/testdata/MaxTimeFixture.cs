// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData
{
    [TestFixture]
    public class MaxTimeFixture
    {
        [Test, MaxTime(1)]
        public void MaxTimeExceeded()
        {
#if NETCF
            long endTime = DateTime.Now.Ticks + TimeSpan.TicksPerMillisecond * 20;
            while (endTime > DateTime.Now.Ticks) ;
#else
            System.Threading.Thread.Sleep(20);
#endif
        }
    }

    [TestFixture]
    public class MaxTimeFixtureWithFailure
    {
        [Test, MaxTime(1)]
        public void MaxTimeExceeded()
        {
            System.Threading.Thread.Sleep(20);
            Assert.Fail("Intentional Failure");
        }
    }

    [TestFixture]
    public class MaxTimeFixtureWithError
    {
        [Test, MaxTime(1)]
        public void MaxTimeExceeded()
        {
            System.Threading.Thread.Sleep(20);
            throw new Exception("Exception message");
        }
    }
}
