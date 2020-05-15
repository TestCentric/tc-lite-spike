// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData
{
    [TestFixture]
    public class TimeoutFixture
    {
        public bool TearDownWasRun;

        [SetUp]
        public void SetUp()
        {
            TearDownWasRun = false;
        }

        [TearDown]
        public void TearDown()
        {
            TearDownWasRun = true;
        }

        [Test, Timeout(50)]
        public void InfiniteLoopWith50msTimeout()
        {
            while (true) { }
        }
    }

    [TestFixture, Timeout(50)]
    public class ThreadingFixtureWithTimeout
    {
        [Test]
        public void Test1() { }
        [Test]
        public void Test2WithInfiniteLoop() { while (true) { } }
        [Test]
        public void Test3() { }
    }
}
