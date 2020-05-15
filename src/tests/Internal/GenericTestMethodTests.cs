// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCF
using System;
using System.Collections.Generic;

namespace TCLite.Framework.Internal
{
    [TestFixture]
    class GenericTestMethodTests
    {
        [TestCase(5, 2, "ABC")]
        [TestCase(5.0, 2.0, "ABC")]
        [TestCase(5, 2.0, "ABC")]
        [TestCase(5.0, 2L, "ABC")]
        public void GenericTestMethodWithOneTypeParameter<T>(T x, T y, string label)
        {
            Assert.AreEqual(5, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual("ABC", label);
        }

        [TestCase(5, 2, "ABC")]
        [TestCase(5.0, 2.0, "ABC")]
        [TestCase(5, 2.0, "ABC")]
        [TestCase(5.0, 2L, "ABC")]
        public void GenericTestMethodWithTwoTypeParameters<T1, T2>(T1 x, T2 y, string label)
        {
            Assert.AreEqual(5, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual("ABC", label);
        }

        [TestCase(5, 2, "ABC")]
        [TestCase(5.0, 2.0, "ABC")]
        [TestCase(5, 2.0, "ABC")]
        [TestCase(5.0, 2L, "ABC")]
        public void GenericTestMethodWithTwoTypeParameters_Reversed<T1, T2>(T2 x, T1 y, string label)
        {
            Assert.AreEqual(5, x);
            Assert.AreEqual(2, y);
            Assert.AreEqual("ABC", label);
        }
    }
}
#endif
