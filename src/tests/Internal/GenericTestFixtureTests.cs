// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TCLite.TestUtilities;

namespace TCLite.Framework.Internal
{
    [TestFixture(typeof(List<int>))]
    [TestFixture(TypeArgs = new Type[] { typeof(List<object>) })]
#if !SILVERLIGHT
    [TestFixture(typeof(ArrayList))]
#endif
    // TODO: Why doesn't this work?
    //[TestFixture(TypeArgs = new Type[] { typeof(SimpleObjectList) })]
    public class GenericTestFixture_IList<T> where T : IList, new()
    {
        [Test]
        public void TestCollectionCount()
        {
            IList list = new T();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }
    }

    [TestFixture(typeof(double))]
    public class GenericTestFixture_Numeric<T>
    {
        [TestCase(5)]
        [TestCase(1.23)]
        public void TestMyArgType(T x)
        {
            Assert.That(x, Is.TypeOf(typeof(T)));
        }
    }
}
