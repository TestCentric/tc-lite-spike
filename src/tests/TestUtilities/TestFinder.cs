// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.TestUtilities
{
    /// <summary>
    /// Utility class used to locate tests by name in a test suite
    /// </summary>
    public class TestFinder
    {
        public static Test MustFind(string name, TestSuite suite, bool recursive)
        {
            Test test = Find(name, suite, recursive);

            Assert.NotNull(test, "Unable to find test {0}", name);

            return test;
        }

        public static Test Find(string name, TestSuite suite, bool recursive)
        {
            foreach (Test child in suite.Tests)
            {
                if (child.Name == name)
                    return child;
                if (recursive)
                {
                    TestSuite childSuite = child as TestSuite;
                    if (childSuite != null)
                    {
                        Test grandchild = Find(name, childSuite, true);
                        if (grandchild != null)
                            return grandchild;
                    }
                }
            }

            return null;
        }

        public static ITestResult MustFind(string name, TestResult result, bool recursive)
        {
            ITestResult foundResult = Find(name, result, recursive);

            Assert.NotNull(foundResult, "Unable to find result for {0}", name);

            return foundResult;
        }

        public static ITestResult Find(string name, TestResult result, bool recursive)
        {
            if (result.HasChildren)
            {
                foreach (TestResult childResult in result.Children)
                {
                    if (childResult.Name == name)
                        return childResult;

                    if (recursive && childResult.HasChildren)
                    {
                        ITestResult r = Find(name, childResult, true);
                        if (r != null)
                            return r;
                    }
                }
            }

            return null;
        }

        private TestFinder() { }
    }
}
