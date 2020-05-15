// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.TheoryFixture
{
    [TestFixture]
    public class TheoryFixture
    {
        [Datapoint]
        internal int i0 = 0;
        [Datapoint]
        internal static int i1 = 1;
        [Datapoint]
        public int i100 = 100;

        private void Dummy()
        {
            int x = i0; // Suppress Compiler Warnings
            int y = i1; //
        }

        [Theory]
        public void TheoryWithNoArguments()
        {
        }

        [Theory]
        public void TheoryWithArgumentsButNoDatapoints(decimal x, decimal y)
        {
        }

        [Theory]
        public void TheoryWithArgumentsAndDatapoints(int x, int y)
        {
        }

        [TestCase(5, 10)]
        [TestCase(3, 12)]
        public void TestWithArguments(int x, int y)
        {
        }

        [Theory]
        public void TestWithBooleanArguments(bool a, bool b)
        {
        }

        [Theory]
        public void TestWithEnumAsArgument(System.AttributeTargets targets)
        {
        }

        [Theory]
        public void TestWithAllBadValues(
            [Values(-12.0, -4.0, -9.0)] double d)
        {
            Assume.That(d > 0);
            Assert.Pass();
        }
    }
}
