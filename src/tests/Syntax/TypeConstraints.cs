// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Syntax
{
    [TestFixture]
    public class ExactTypeTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<typeof System.String>";
            staticSyntax = Is.TypeOf(typeof(string));
            builderSyntax = Builder().TypeOf(typeof(string));
        }
    }

    [TestFixture]
    public class InstanceOfTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<instanceof System.String>";
            staticSyntax = Is.InstanceOf(typeof(string));
            builderSyntax = Builder().InstanceOf(typeof(string));
        }
    }

    [TestFixture]
    public class AssignableFromTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignablefrom System.String>";
            staticSyntax = Is.AssignableFrom(typeof(string));
            builderSyntax = Builder().AssignableFrom(typeof(string));
        }
    }

    [TestFixture]
    public class AssignableToTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignableto System.String>";
            staticSyntax = Is.AssignableTo(typeof(string));
            builderSyntax = Builder().AssignableTo(typeof(string));
        }
    }

    [TestFixture]
    public class AttributeTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<attributeexists TCLite.Framework.TestFixtureAttribute>";
            staticSyntax = Has.Attribute(typeof(TestFixtureAttribute));
            builderSyntax = Builder().Attribute(typeof(TestFixtureAttribute));
        }
    }

    [TestFixture]
    public class AttributeTestWithFollowingConstraint : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<attribute TCLite.Framework.TestFixtureAttribute <property Description <not <null>>>>";
            staticSyntax = Has.Attribute(typeof(TestFixtureAttribute)).Property("Description").Not.Null;
            builderSyntax = Builder().Attribute(typeof(TestFixtureAttribute)).Property("Description").Not.Null;
        }
    }

    [TestFixture]
    public class ExactTypeTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<typeof System.String>";
            staticSyntax = Is.TypeOf<string>();
            builderSyntax = Builder().TypeOf<string>();
        }
    }

    [TestFixture]
    public class InstanceOfTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<instanceof System.String>";
            staticSyntax = Is.InstanceOf<string>();
            builderSyntax = Builder().InstanceOf<string>();
        }
    }

    [TestFixture]
    public class AssignableFromTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignablefrom System.String>";
            staticSyntax = Is.AssignableFrom<string>();
            builderSyntax = Builder().AssignableFrom<string>();
        }
    }

    [TestFixture]
    public class AssignableToTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<assignableto System.String>";
            staticSyntax = Is.AssignableTo<string>();
            builderSyntax = Builder().AssignableTo<string>();
        }
    }

    [TestFixture]
    public class AttributeTest_Generic : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<attributeexists TCLite.Framework.TestFixtureAttribute>";
            staticSyntax = Has.Attribute<TestFixtureAttribute>();
            builderSyntax = Builder().Attribute<TestFixtureAttribute>();
        }
    }
}
