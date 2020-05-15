// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework;

namespace TCLite.TestData.TestCaseSourceAttributeFixture
{
    [TestFixture]
    public class TestCaseSourceAttributeFixture
    {
        [TestCaseSource("ignored_source")]
        public void MethodWithIgnoredTestCases(int num)
        {
        }

        [TestCaseSource("explicit_source")]
        public void MethodWithExplicitTestCases(int num)
        {
        }

        internal static IEnumerable ignored_source
        {
            get
            {
                return new object[] {
                    new TestCaseData(1),
                    new TestCaseData(2).Ignore(),
                    new TestCaseData(3).Ignore("Don't Run Me!")
                };
            }
        }

        internal static IEnumerable explicit_source
        {
            get
            {
                return new object[] {
                    new TestCaseData(1),
                    new TestCaseData(2).Explicit(),
                    new TestCaseData(3).Explicit("Connection failing")
                };
            }
        }

        [TestCaseSource("exception_source")]
        public void MethodWithSourceThrowingException(string lhs, string rhs)
        {
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
    }
}
