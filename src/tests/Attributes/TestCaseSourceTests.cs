// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData.TestCaseSourceAttributeFixture;
using TCLite.TestUtilities;

namespace TCLite.Framework.Tests
{
    [TestFixture]
    public class TestCaseSourceTests
    {
        [Test, TestCaseSource("StaticProperty")]
        public void SourceCanBeStaticProperty(string source)
        {
            Assert.AreEqual("StaticProperty", source);
        }

        internal static IEnumerable StaticProperty
        {
            get { return new object[] { new object[] { "StaticProperty" } }; }
        }

        [Test, TestCaseSource("InstanceProperty")]
        public void SourceCanBeInstanceProperty(string source)
        {
            Assert.AreEqual("InstanceProperty", source);
        }

        internal IEnumerable InstanceProperty
        {
            get { return new object[] { new object[] { "InstanceProperty" } }; }
        }

        [Test, TestCaseSource("StaticMethod")]
        public void SourceCanBeStaticMethod(string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        internal static IEnumerable StaticMethod()
        {
            return new object[] { new object[] { "StaticMethod" } };
        }

        [Test, TestCaseSource("InstanceMethod")]
        public void SourceCanBeInstanceMethod(string source)
        {
            Assert.AreEqual("InstanceMethod", source);
        }

        internal IEnumerable InstanceMethod()
        {
            return new object[] { new object[] { "InstanceMethod" } };
        }

        [Test, TestCaseSource("StaticField")]
        public void SourceCanBeStaticField(string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        internal static object[] StaticField =
            { new object[] { "StaticField" } };

        [Test, TestCaseSource("InstanceField")]
        public void SourceCanBeInstanceField(string source)
        {
            Assert.AreEqual("InstanceField", source);
        }

        internal static object[] InstanceField =
            { new object[] { "InstanceField" } };

        [Test, TestCaseSource(typeof(DataSourceClass))]
        public void SourceCanBeInstanceOfIEnumerable(string source)
        {
            Assert.AreEqual("DataSourceClass", source);
        }

        internal class DataSourceClass : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return "DataSourceClass";
            }
        }

        [Test, TestCaseSource("MyData")]
        public void SourceMayReturnArgumentsAsObjectArray(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCaseSource("MyData")]
        public void TestAttributeIsOptional(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [Test, TestCaseSource("MyIntData")]
        public void SourceMayReturnArgumentsAsIntArray(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [Test, TestCaseSource("EvenNumbers")]
        public void SourceMayReturnSinglePrimitiveArgumentAlone(int n)
        {
            Assert.AreEqual(0, n % 2);
        }

        [Test, TestCaseSource("Params")]
        public int SourceMayReturnArgumentsAsParamSet(int n, int d)
        {
            return n / d;
        }

        [Test]
        [TestCaseSource("MyData")]
        [TestCaseSource("MoreData", Category="Extra")]
        public void TestMayUseMultipleSourceAttributes(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [Test, TestCaseSource("FourArgs")]
        public void TestWithFourArguments(int n, int d, int q, int r)
        {
            Assert.AreEqual(q, n / d);
            Assert.AreEqual(r, n % d);
        }

        [Test, TestCaseSource(typeof(DivideDataProvider), "HereIsTheData")]
        //[Category("Top")]
        public void SourceMayBeInAnotherClass(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [Test, TestCaseSource(typeof(DivideDataProviderWithReturnValue), "TestCases")]
        public int SourceMayBeInAnotherClassWithReturn(int n, int d)
        {
            return n / d;
        }

        [Test]
        public void CanIgnoreIndividualTestCases()
        {
            TestSuite test = (TestSuite)TestBuilder.MakeTestCase(
                typeof(TestCaseSourceAttributeFixture), "MethodWithIgnoredTestCases");

            Test testCase = TestFinder.MustFind("MethodWithIgnoredTestCases(1)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));
 
            testCase = TestFinder.MustFind("MethodWithIgnoredTestCases(2)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
 
			testCase = TestFinder.MustFind("MethodWithIgnoredTestCases(3)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(testCase.Properties.GetSetting(PropertyNames.SkipReason, ""), Is.EqualTo("Don't Run Me!"));
        }

        [Test]
        public void CanMarkIndividualTestCasesExplicit()
        {
            TestSuite test = (TestSuite)TestBuilder.MakeTestCase(
                typeof(TestCaseSourceAttributeFixture), "MethodWithExplicitTestCases");

            Test testCase = TestFinder.Find("MethodWithExplicitTestCases(1)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));
 
            testCase = TestFinder.Find("MethodWithExplicitTestCases(2)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));
 
			testCase = TestFinder.Find("MethodWithExplicitTestCases(3)", test, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));
            Assert.That(testCase.Properties.GetSetting(PropertyNames.SkipReason, ""), Is.EqualTo("Connection failing"));
		}

		[Test]
        public void HandlesExceptionInTestCaseSource()
        {
            Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), "MethodWithSourceThrowingException").Tests[0];
            Assert.AreEqual(RunState.NotRunnable, test.RunState);
            ITestResult result = TestBuilder.RunTest(test, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
            Assert.AreEqual("System.Exception : my message", result.Message);
        }

        internal object[] testCases =
        {
            new TestCaseData(
                new string[] { "A" },
                new string[] { "B" })
        };

        [Test, TestCaseSource("testCases")]
        public void MethodTakingTwoStringArrays(string[] a, string[] b)
        {
            Assert.That(a, Is.TypeOf(typeof(string[])));
            Assert.That(b, Is.TypeOf(typeof(string[])));
        }

        #region Sources used by the tests
        internal static object[] MyData = new object[] {
            new object[] { 12, 3, 4 },
            new object[] { 12, 4, 3 },
            new object[] { 12, 6, 2 } };

        internal static object[] MyIntData = new object[] {
            new int[] { 12, 3, 4 },
            new int[] { 12, 4, 3 },
            new int[] { 12, 6, 2 } };

        internal static object[] FourArgs = new object[] {
            new TestCaseData( 12, 3, 4, 0 ),
            new TestCaseData( 12, 4, 3, 0 ),
            new TestCaseData( 12, 5, 2, 2 ) };

        internal static int[] EvenNumbers = new int[] { 2, 4, 6, 8 };

        internal static object[] MoreData = new object[] {
            new object[] { 12, 1, 12 },
            new object[] { 12, 2, 6 } };

        internal static object[] Params = new object[] {
            new TestCaseData(24, 3).Returns(8),
            new TestCaseData(24, 2).Returns(12) };

        public class DivideDataProvider
        {
            public static IEnumerable HereIsTheData
            {
                get
                {
                    yield return new object[] { 100, 20, 5 };
                    yield return new object[] { 100, 4, 25 };
                }
            }
        }

        public class DivideDataProviderWithReturnValue
        {
            public static IEnumerable TestCases
            {
                get
                {
                    return new object[] {
                        new TestCaseData(12, 2).Returns(6).SetName("TC2"),
                        new TestCaseData(12, 4).Returns(3).SetName("TC3")
                    };
                }
            }
        }

        internal static IEnumerable exception_source
        {
            get
            {
                yield return new TestCaseData("a", "a");
                yield return new TestCaseData("b", "b");

                throw new System.Exception("my message");
            }
        }

        #endregion
    }
}
