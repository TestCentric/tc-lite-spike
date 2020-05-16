// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData.TestCaseAttributeFixture;
using TCLite.TestUtilities;
using System.Collections;

namespace TCLite.Framework.Tests
{
    [TestFixture]
    public class TestCaseAttributeTests
    {
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void IntegerDivisionWithResultPassedToTest(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCase(12, 3, ExpectedResult = 4)]
        [TestCase(12, 2, ExpectedResult = 6)]
        [TestCase(12, 4, ExpectedResult = 3)]
        public int IntegerDivisionWithResultCheckedByNUnit(int n, int d)
        {
            return n / d;
        }

        [TestCase(2, 2, ExpectedResult=4)]
        public double CanConvertIntToDouble(double x, double y)
        {
            return x + y;
        }

        [TestCase("2.2", "3.3", ExpectedResult = 5.5)]
        public decimal CanConvertStringToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(2.2, 3.3, ExpectedResult = 5.5)]
        public decimal CanConvertDoubleToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public decimal CanConvertIntToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public short CanConvertSmallIntsToShort(short x, short y)
        {
            return (short)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public byte CanConvertSmallIntsToByte(byte x, byte y)
        {
            return (byte)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public sbyte CanConvertSmallIntsToSByte(sbyte x, sbyte y)
        {
            return (sbyte)(x + y);
        }

        [Test]
		public void ConversionOverflowMakesTestNotRunnable()
		{
			Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
				typeof(TestCaseAttributeFixture), "MethodCausesConversionOverflow").Tests[0];
			Assert.AreEqual(RunState.NotRunnable, test.RunState);
		}

        [TestCase("12-October-1942")]
        public void CanConvertStringToDateTime(DateTime dt)
        {
            Assert.AreEqual(1942, dt.Year);
        }

        [TestCase(null)]
        public void CanPassNullAsFirstArgument(object a)
        {
        	Assert.Null(a);
        }

        [TestCase(new object[] { 1, "two", 3.0 })]
        [TestCase(new object[] { "zip" })]
        public void CanPassObjectArrayAsFirstArgument(object[] a)
        {
        }
  
        [TestCase(new object[] { "a", "b" })]
        public void CanPassArrayAsArgument(object[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase("a", "b")]
        public void ArgumentsAreCoalescedInObjectArray(object[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase(1, "b")]
        public void ArgumentsOfDifferentTypeAreCoalescedInObjectArray(object[] array)
        {
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase(ExpectedResult = null)]
        public object ResultCanBeNull()
        {
            return null;
        }

        [TestCase("a", "b")]
        public void HandlesParamsArrayAsSoleArgument(params string[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase("a")]
        public void HandlesParamsArrayWithOneItemAsSoleArgument(params string[] array)
        {
            Assert.AreEqual("a", array[0]);
        }

        [TestCase("a", "b", "c", "d")]
        public void HandlesParamsArrayAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual("c", array[0]);
            Assert.AreEqual("d", array[1]);
        }

        [TestCase("a", "b")]
        public void HandlesParamsArrayWithNoItemsAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual(0, array.Length);
        }

        [TestCase("a", "b", "c")]
        public void HandlesParamsArrayWithOneItemAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual("c", array[0]);
        }

        [Test]
        public void CanSpecifyDescription()
        {
			Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
				typeof(TestCaseAttributeFixture), "MethodHasDescriptionSpecified").Tests[0];
			Assert.AreEqual("My Description", test.Properties.Get(PropertyNames.Description));
		}

        [Test]
        public void CanSpecifyTestName()
        {
            Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), "MethodHasTestNameSpecified").Tests[0];
            Assert.AreEqual("XYZ", test.Name);
            Assert.AreEqual("TCLite.TestData.TestCaseAttributeFixture.TestCaseAttributeFixture.XYZ", test.FullName);
        }

        [Test]
        public void CanSpecifyCategory()
        {
            Test test = (Test)TestBuilder.MakeTestCase(
                typeof(TestCaseAttributeFixture), "MethodHasSingleCategory").Tests[0];
			IList categories = test.Properties["Category"];
            Assert.AreEqual(new string[] { "XYZ" }, categories);
        }
 
        [Test]
        public void CanSpecifyMultipleCategories()
        {
            Test test = (Test)TestBuilder.MakeTestCase(
                typeof(TestCaseAttributeFixture), "MethodHasMultipleCategories").Tests[0];
			IList categories = test.Properties["Category"];
            Assert.AreEqual(new string[] { "X", "Y", "Z" }, categories);
        }
 
        [Test]
        public void CanIgnoreIndividualTestCases()
        {
            TestSuite test = (TestSuite)TestBuilder.MakeTestCase(
                typeof(TestCaseAttributeFixture), "MethodWithIgnoredTestCases");

            Test testCase = TestFinder.Find("MethodWithIgnoredTestCases(1)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));
 
            testCase = TestFinder.Find("MethodWithIgnoredTestCases(2)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
 
			testCase = TestFinder.Find("MethodWithIgnoredTestCases(3)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(testCase.Properties.GetSetting(PropertyNames.SkipReason, ""), Is.EqualTo("Don't Run Me!"));
		}

        [Test]
        public void CanMarkIndividualTestCasesExplicit()
        {
            TestSuite test = (TestSuite)TestBuilder.MakeTestCase(
                typeof(TestCaseAttributeFixture), "MethodWithExplicitTestCases");

            Test testCase = TestFinder.Find("MethodWithExplicitTestCases(1)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));
 
            testCase = TestFinder.Find("MethodWithExplicitTestCases(2)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));
 
			testCase = TestFinder.Find("MethodWithExplicitTestCases(3)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));
            Assert.That(testCase.Properties.GetSetting(PropertyNames.SkipReason, ""), Is.EqualTo("Connection failing"));
		}
    }
}
