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
    /// Summary description for ResultSummary.
    /// </summary>
    public class ResultSummary
    {
        private int resultCount = 0;
        private int testsRun = 0;
        private int failureCount = 0;
        private int errorCount = 0;
        private int successCount = 0;
        private int inconclusiveCount = 0;
        private int skipCount = 0;
        private int ignoreCount = 0;
        private int notRunnable = 0;

        private TimeSpan duration = TimeSpan.Zero;
        private string name;

        public ResultSummary() { }

        public ResultSummary(ITestResult result)
        {
            Summarize(result);
        }

        private void Summarize(ITestResult result)
        {
            if (this.name == null)
            {
                this.name = result.Name;
                this.duration = result.Duration;
            }

            if (result.HasChildren)
            {
                foreach (TestResult childResult in result.Children)
                    Summarize(childResult);
            }
            else
            {
                resultCount++;

                switch (result.ResultState.Status)
                {
                    case TestStatus.Passed:
                        successCount++;
                        testsRun++;
                        break;
                    case TestStatus.Failed:
                        failureCount++;
                        testsRun++;
                        break;
                    //case TestStatus.Error:
                    //case TestStatus.Cancelled:
                    //errorCount++;
                    //testsRun++;
                    //break;
                    case TestStatus.Inconclusive:
                        inconclusiveCount++;
                        testsRun++;
                        break;
                    //case TestStatus.NotRunnable:
                    //    notRunnable++;
                    //    //errorCount++;
                    //    break;
                    //case TestStatus.Ignored:
                    //    ignoreCount++;
                    //    break;
                    case TestStatus.Skipped:
                    default:
                        skipCount++;
                        break;
                }
            }
        }

        public string Name
        {
            get { return name; }
        }

        public bool Success
        {
            get { return failureCount == 0; }
        }

        /// <summary>
        /// Returns the number of test cases for which results
        /// have been summarized. Any tests excluded by use of
        /// Category or Explicit attributes are not counted.
        /// </summary>
        public int ResultCount
        {
            get { return resultCount; }
        }

        /// <summary>
        /// Returns the number of test cases actually run, which
        /// is the same as ResultCount, less any Skipped, Ignored
        /// or NonRunnable tests.
        /// </summary>
        public int TestsRun
        {
            get { return testsRun; }
        }

        /// <summary>
        /// Returns the number of tests that passed
        /// </summary>
        public int Passed
        {
            get { return successCount; }
        }

        /// <summary>
        /// Returns the number of test cases that had an error.
        /// </summary>
        public int Errors
        {
            get { return errorCount; }
        }

        /// <summary>
        /// Returns the number of test cases that failed.
        /// </summary>
        public int Failures
        {
            get { return failureCount; }
        }

        /// <summary>
        /// Returns the number of test cases that failed.
        /// </summary>
        public int Inconclusive
        {
            get { return inconclusiveCount; }
        }

        /// <summary>
        /// Returns the number of test cases that were not runnable
        /// due to errors in the signature of the class or method.
        /// Such tests are also counted as Errors.
        /// </summary>
        public int NotRunnable
        {
            get { return notRunnable; }
        }

        /// <summary>
        /// Returns the number of test cases that were skipped.
        /// </summary>
        public int Skipped
        {
            get { return skipCount; }
        }

        public int Ignored
        {
            get { return ignoreCount; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
        }

        public int TestsNotRun
        {
            get { return skipCount + ignoreCount + notRunnable; }
        }

        public int ErrorsAndFailures
        {
            get { return errorCount + failureCount; }
        }
    }
}
