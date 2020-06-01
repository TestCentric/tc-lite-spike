// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Syntax
{
#if !NETCF && !SILVERLIGHT
    [TestFixture]
    public class BinarySerializableTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<binaryserializable>";
            staticSyntax = Is.BinarySerializable;
            builderSyntax = Builder().BinarySerializable;
        }
    }
#endif

#if !SILVERLIGHT
    [TestFixture]
    public class XmlSerializableTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = "<xmlserializable>";
            staticSyntax = Is.XmlSerializable;
            builderSyntax = Builder().XmlSerializable;
        }
    }
#endif
}
