// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class NumericsTest
    {
        private Tolerance tenPercent, zeroTolerance;

        [SetUp]
        public void SetUp()
        {
            tenPercent = new Tolerance(10.0).Percent;
            zeroTolerance = new Tolerance(0);
        }

        [TestCase(123456789)]
        [TestCase(123456789U)]
        [TestCase(123456789L)]
        [TestCase(123456789UL)]
        [TestCase(1234.5678f)]
        [TestCase(1234.5678)]
        [Test]
        public void CanMatchWithoutToleranceMode(object value)
        {
            Assert.True(Numerics.AreEqual(value, value, ref zeroTolerance));
        }

        // Separate test case because you can't use decimal in an attribute (24.1.3)
        [Test]
        public void CanMatchDecimalWithoutToleranceMode()
        {
            Assert.True(Numerics.AreEqual(123m, 123m, ref zeroTolerance));
        }

        [TestCase((int)9500)]
        [TestCase((int)10000)]
        [TestCase((int)10500)]
        [TestCase((uint)9500)]
        [TestCase((uint)10000)]
        [TestCase((uint)10500)]
        [TestCase((long)9500)]
        [TestCase((long)10000)]
        [TestCase((long)10500)]
        [TestCase((ulong)9500)]
        [TestCase((ulong)10000)]
        [TestCase((ulong)10500)]
        [Test]
        public void CanMatchIntegralsWithPercentage(object value)
        {
            Assert.True(Numerics.AreEqual(10000, value, ref tenPercent));
        }

        [Test]
        public void CanMatchDecimalWithPercentage()
        {
            Assert.True(Numerics.AreEqual(10000m, 9500m, ref tenPercent));
            Assert.True(Numerics.AreEqual(10000m, 10000m, ref tenPercent));
            Assert.True(Numerics.AreEqual(10000m, 10500m, ref tenPercent));
        }

        [TestCase((int)8500)]
        [TestCase((int)11500)]
        [TestCase((uint)8500)]
        [TestCase((uint)11500)]
        [TestCase((long)8500)]
        [TestCase((long)11500)]
        [TestCase((ulong)8500)]
        [TestCase((ulong)11500)]
        public void FailsOnIntegralsOutsideOfPercentage(object value)
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.True(Numerics.AreEqual(10000, value, ref tenPercent));
            });
        }

        [Test]
        public void FailsOnDecimalBelowPercentage()
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.True(Numerics.AreEqual(10000m, 8500m, ref tenPercent));
            });
        }

        [Test]
        public void FailsOnDecimalAbovePercentage()
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.True(Numerics.AreEqual(10000m, 11500m, ref tenPercent));
            });
        }
    }
}