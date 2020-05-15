// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCF && !SILVERLIGHT
using System;
using System.Collections;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class BinarySerializableTest : ConstraintTestBaseWithArgumentException
    {
		[SetUp]
        public void SetUp()
        {
            theConstraint = new BinarySerializableConstraint();
            expectedDescription = "binary serializable";
            stringRepresentation = "<binaryserializable>";
        }

        object[] SuccessData = new object[] { 1, "a", new ArrayList(), new InternalWithSerializableAttributeClass() };
        
        object[] FailureData = new object[] { new TestCaseData( new InternalClass(), "<InternalClass>" ) };

        object[] InvalidData = new object[] { null };
    }
}
#endif