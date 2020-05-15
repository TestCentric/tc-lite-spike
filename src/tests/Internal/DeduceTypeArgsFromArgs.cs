// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCF
using System;

namespace TCLite.Framework.Internal
{
    [Category("Generics")]
    [TestFixture(100.0, 42)]
    [TestFixture(42, 100.0)]
    public class DeduceTypeArgsFromArgs<T1, T2>
    {
        T1 t1;
        T2 t2;

        public DeduceTypeArgsFromArgs(T1 t1, T2 t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }

        [TestCase(5, 7)]
        public void TestMyArgTypes(T1 t1, T2 t2)
        {
            Assert.That(t1, Is.TypeOf<T1>());
            Assert.That(t2, Is.TypeOf<T2>());
        }
    }
}
#endif
