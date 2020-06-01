// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class CollectionSubsetConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new CollectionSubsetConstraint(new int[] { 1, 2, 3, 4, 5 });
            stringRepresentation = "<subsetof System.Int32[]>";
            expectedDescription = "subset of < 1, 2, 3, 4, 5 >";
        }

        internal object[] SuccessData = new object[] { new int[] { 1, 3, 5 }, new int[] { 1, 2, 3, 4, 5 } };
        internal object[] FailureData = new object[] { 
            new object[] { new int[] { 1, 3, 7 }, "< 1, 3, 7 >" },
            new object[] { new int[] { 1, 2, 2, 2, 5 }, "< 1, 2, 2, 2, 5 >" } };
    }
}
