// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !SILVERLIGHT

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class XmlSerializableTest : ConstraintTestBaseWithArgumentException
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new XmlSerializableConstraint();
            expectedDescription = "xml serializable";
            stringRepresentation = "<xmlserializable>";
        }

        internal object[] SuccessData = new object[] { 1, "a", new ArrayList() };

        internal object[] FailureData = new object[] { 
            new TestCaseData( new Dictionary<string, string>(), "<Dictionary`2>" ),
            new TestCaseData( new InternalClass(), "<InternalClass>" ),
            new TestCaseData( new InternalWithSerializableAttributeClass(), "<InternalWithSerializableAttributeClass>" )
        };

        internal object[] InvalidData = new object[] { null };
    }

    internal class InternalClass
    { }

    [Serializable]
    internal class InternalWithSerializableAttributeClass
    { }
}
#endif