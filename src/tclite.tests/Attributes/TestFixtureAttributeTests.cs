// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Attributes
{
    public class TestFixtureAttributeTests
    {
        static object[] fixtureArgs = new object[] { 10, 20, "Charlie" };
        static Type[] typeArgs = new Type[] { typeof(int), typeof(string) };
        static object[] combinedArgs = new object[] { typeof(int), typeof(string), 10, 20, "Charlie" };

        [Test]
        public void ConstructWithoutArguments()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute();
            Assert.That(attr.Arguments.Length == 0);
            Assert.That(attr.TypeArgs.Length == 0);
        }

        [Test]
        public void ConstructWithFixtureArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(fixtureArgs);
            Assert.That(attr.Arguments, Is.EqualTo( fixtureArgs ) );
            Assert.That(attr.TypeArgs.Length == 0 );
        }

        [Test]
        public void ConstructWithJustTypeArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(typeArgs);
            Assert.That(attr.Arguments.Length == 0);
            Assert.That(attr.TypeArgs, Is.EqualTo(typeArgs));
        }

        [Test]
        public void ConstructWithNoArgumentsAndSetTypeArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute();
            attr.TypeArgs = typeArgs;
            Assert.That(attr.Arguments.Length == 0);
            Assert.That(attr.TypeArgs, Is.EqualTo(typeArgs));
        }

        [Test]
        public void ConstructWithFixtureArgsAndSetTypeArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(fixtureArgs);
            attr.TypeArgs = typeArgs;
            Assert.That(attr.Arguments, Is.EqualTo(fixtureArgs));
            Assert.That(attr.TypeArgs, Is.EqualTo(typeArgs));
        }

        [Test]
        public void ConstructWithCombinedArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(combinedArgs);
            Assert.That(attr.Arguments, Is.EqualTo(fixtureArgs));
            Assert.That(attr.TypeArgs, Is.EqualTo(typeArgs));
        }
	}
}
