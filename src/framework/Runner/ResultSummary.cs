// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;

namespace TCLite.Runner
{
    /// <summary>
    /// Helper class used to summarize the result of a test run
    /// </summary>
    public class ResultSummary
    {
        private int testCount;
        private int passCount;
        private int errorCount;
        private int failureCount;
        private int notRunCount;
        private int inconclusiveCount;
        private int ignoreCount;
        private int skipCount;
        private int invalidCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultSummary"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ResultSummary(ITestResult result)
        {
            Visit(result);
        }

        /// <summary>
        /// Gets the test count.
        /// </summary>
        /// <value>The test count.</value>
        public int TestCount
        {
            get { return testCount; }
        }

        /// <summary>
        /// Gets the count of passed tests
        /// </summary>
        public int PassCount
        {
            get { return passCount; }
        }

        /// <summary>
        /// Gets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount
        {
            get { return errorCount; }
        }

        /// <summary>
        /// Gets the failure count.
        /// </summary>
        /// <value>The failure count.</value>
        public int FailureCount
        {
            get { return failureCount; }
        }

        /// <summary>
        /// Gets the not run count.
        /// </summary>
        /// <value>The not run count.</value>
        public int NotRunCount
        {
            get { return notRunCount; }
        }

        /// <summary>
        /// Gets the ignore count
        /// </summary>
        public int IgnoreCount
        {
            get { return ignoreCount; }
        }

        /// <summary>
        /// Gets the skip count
        /// </summary>
        public int SkipCount
        {
            get { return skipCount; }
        }

        /// <summary>
        /// Gets the invalid count
        /// </summary>
        public int InvalidCount
        {
            get { return invalidCount; }
        }

        /// <summary>
        /// Gets the count of inconclusive results
        /// </summary>
        public int InconclusiveCount
        {
            get { return inconclusiveCount; }
        }

        private void Visit(ITestResult result)
        {
            if (result.Test.IsSuite)
            {
                foreach (ITestResult r in result.Children)
                    Visit(r);
            }
            else
            {
                testCount++;

                switch (result.ResultState.Status)
                {
                    case TestStatus.Passed:
                        passCount++;
                        break;
                    case TestStatus.Skipped:
                        if (result.ResultState == ResultState.Ignored)
                            ignoreCount++;
                        else if (result.ResultState == ResultState.Skipped)
                            skipCount++;
                        else if (result.ResultState == ResultState.NotRunnable)
                            invalidCount++;
                        notRunCount++;
                        break;
                    case TestStatus.Failed:
                        if (result.ResultState == ResultState.Failure)
                            failureCount++;
                        else
                            errorCount++;
                        break;
                    case TestStatus.Inconclusive:
                        inconclusiveCount++;
                        break;
                }

                return;
            }
        }
    }
}
