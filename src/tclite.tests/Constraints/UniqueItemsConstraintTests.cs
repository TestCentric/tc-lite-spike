// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if NYI
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class UniqueItemsTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new UniqueItemsConstraint();
            stringRepresentation = "<uniqueitems>";
            expectedDescription = "all items unique";
        }

        internal object[] SuccessData = new object[] { new int[] { 1, 3, 17, -2, 34 }, new object[0] };
        internal object[] FailureData = new object[] { new object[] { new int[] { 1, 3, 17, 3, 34 }, "< 1, 3, 17, 3, 34 >" } };
    }
}
#endif
