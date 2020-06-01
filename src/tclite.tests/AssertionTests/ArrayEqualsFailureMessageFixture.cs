// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;

namespace TCLite.Framework.AssertionTests 
{
    public class ArrayEqualsFailureMessageFixture : AssertionTestBase
    {
        [Test]
        public void ArraysHaveDifferentRanks()
        {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 4 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected is <System.Int32[4]>, actual is <System.Int32[2,2]>" + NL);
        }

        [Test]
        public void ExpectedArrayIsLonger()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 5 };
            int[] actual = new int[] { 1, 2, 3 };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected is <System.Int32[5]>, actual is <System.Int32[3]>" + NL +
                "  Values differ at index [3]" + NL +
                "  Missing:  < 4, 5 >");
        }

        [Test]
        public void ActualArrayIsLonger()
        {
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = new int[] { 1, 2, 3, 4, 5, 6, 7 };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected is <System.Int32[3]>, actual is <System.Int32[7]>" + NL +
                "  Values differ at index [3]" + NL +
                "  Extra:    < 4, 5, 6... >");
        }

        [Test]
        public void FailureOnSingleDimensionedArrays()
        {
            int[] expected = new int[] { 1, 2, 3 };
            int[] actual = new int[] { 1, 5, 3 };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected and actual are both <System.Int32[3]>" + NL +
                "  Values differ at index [1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual + "5" + NL);
        }

        [Test]
        public void DoubleDimensionedArrays()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 3, 2 }, { 4, 0, 6 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected and actual are both <System.Int32[2,3]>" + NL +
                "  Values differ at index [0,1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual + "3" + NL);
        }

        [Test]
        public void TripleDimensionedArrays()
        {
            int[, ,] expected = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };
            int[, ,] actual = new int[,,] { { { 1, 2 }, { 3, 4 } }, { { 0, 6 }, { 7, 8 } } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected and actual are both <System.Int32[2,2,2]>" + NL +
                "  Values differ at index [1,0,0]" + NL +
                TextMessageWriter.Pfx_Expected + "5" + NL +
                TextMessageWriter.Pfx_Actual + "0" + NL);
        }

        [Test]
        public void FiveDimensionedArrays()
        {
            int[, , , ,] expected = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };
            int[, , , ,] actual = new int[2, 2, 2, 2, 2] { { { { { 1, 2 }, { 4, 3 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } }, { { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } }, { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } } } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected and actual are both <System.Int32[2,2,2,2,2]>" + NL +
                "  Values differ at index [0,0,0,1,0]" + NL +
                TextMessageWriter.Pfx_Expected + "3" + NL +
                TextMessageWriter.Pfx_Actual + "4" + NL);
        }

        [Test]
        public void JaggedArrays()
        {
            int[][] expected = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6, 7 }, new int[] { 8, 9 } };
            int[][] actual = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 0, 7 }, new int[] { 8, 9 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected and actual are both <System.Int32[3][]>" + NL +
                "  Values differ at index [1]" + NL +
                "    Expected and actual are both <System.Int32[4]>" + NL +
                "    Values differ at index [2]" + NL +
                TextMessageWriter.Pfx_Expected + "6" + NL +
                TextMessageWriter.Pfx_Actual + "0" + NL);
        }

        [Test]
        public void JaggedArrayComparedToSimpleArray()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[][] actual = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 0, 7 }, new int[] { 8, 9 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected)),
                "  Expected is <System.Int32[9]>, actual is <System.Int32[3][]>" + NL +
                "  Values differ at index [0]" + NL +
                TextMessageWriter.Pfx_Expected + "1" + NL +
                TextMessageWriter.Pfx_Actual + "< 1, 2, 3 >" + NL);
        }

        [Test]
        public void ArraysWithDifferentRanksAsCollection()
        {
            int[] expected = new int[] { 1, 2, 3, 4 };
            int[,] actual = new int[,] { { 1, 0 }, { 3, 4 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected).AsCollection),
                "  Expected is <System.Int32[4]>, actual is <System.Int32[2,2]>" + NL +
                "  Values differ at expected index [1], actual index [0,1]" + NL +
                TextMessageWriter.Pfx_Expected + "2" + NL +
                TextMessageWriter.Pfx_Actual + "0" + NL);
        }

        [Test]
        public void ArraysWithDifferentDimensionsAsCollection()
        {
            int[,] expected = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] actual = new int[,] { { 1, 2 }, { 3, 0 }, { 5, 6 } };

            ThrowsAssertionException(() => Assert.That(actual, Is.EqualTo(expected).AsCollection),
                "  Expected is <System.Int32[2,3]>, actual is <System.Int32[3,2]>" + NL +
                "  Values differ at expected index [1,0], actual index [1,1]" + NL +
                TextMessageWriter.Pfx_Expected + "4" + NL +
                TextMessageWriter.Pfx_Actual + "0" + NL);
        }

        [Test]
        public void SameLengthDifferentContent()
        {
            string[] array1 = { "one", "two", "three" };
            string[] array2 = { "one", "two", "ten" };

            ThrowsAssertionException(() => Assert.That(array1, Is.EqualTo(array2)),
                "  Expected and actual are both <System.String[3]>" + NL +
                "  Values differ at index [2]" + NL +
                "  Expected string length 3 but was 5. Strings differ at index 1." + NL +
                "  Expected: \"ten\"" + NL +
                "  But was:  \"three\"" + NL +
                "  ------------^" + NL);
        }

        [Test]
        public void ArraysDeclaredAsDifferentTypes()
        {
            string[] array1 = { "one", "two", "three" };
            object[] array2 = { "one", "three", "two" };

            ThrowsAssertionException(() => Assert.That(array1, Is.EqualTo(array2)),
                "  Expected is <System.Object[3]>, actual is <System.String[3]>" + NL +
                "  Values differ at index [1]" + NL +
                "  Expected string length 5 but was 3. Strings differ at index 1." + NL +
                "  Expected: \"three\"" + NL +
                "  But was:  \"two\"" + NL +
                "  ------------^" + NL);
        }

        [Test]
        public void ArrayAndCollection_Failure()
        {
            int[] a = new int[] { 1, 2, 3 };
            ICollection b = new SimpleObjectCollection(1, 3);
            ThrowsAssertionException(() => Assert.AreEqual(a, b));
        }

        [Test]
        public void DifferentArrayTypesEqualFails()
        {
            string[] array1 = { "one", "two", "three" };
            object[] array2 = { "one", "three", "two" };

            ThrowsAssertionException(() => Assert.AreEqual(array1, array2),
                "  Expected is <System.String[3]>, actual is <System.Object[3]>" + NL +
                "  Values differ at index [1]" + NL +
                "  Expected string length 3 but was 5. Strings differ at index 1." + NL +
                "  Expected: \"two\"" + NL +
                "  But was:  \"three\"" + NL +
                "  ------------^" + NL);
        }
    }
}
