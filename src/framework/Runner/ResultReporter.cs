// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Runner
{
    /// <summary>
    /// ResultReporter writes the NUnitLite results to a TextWriter.
    /// </summary>
    public class ResultReporter
    {
        private TextWriter writer;
        private ITestResult result;
        private ResultSummary summary;
        private int reportCount = 0;

        /// <summary>
        /// Constructs an instance of ResultReporter
        /// </summary>
        /// <param name="result">The top-level result being reported</param>
        /// <param name="writer">A TextWriter to which the report is written</param>
        public ResultReporter(ITestResult result, TextWriter writer)
        {
            this.result = result;
            this.writer = writer;

            this.summary = new ResultSummary(this.result);
        }

        /// <summary>
        /// Gets the ResultSummary created by the ResultReporter
        /// </summary>
        public ResultSummary Summary
        {
            get { return summary; }
        }

        /// <summary>
        /// Produces the standard output reports.
        /// </summary>
        public void ReportResults()
        {
            PrintSummaryReport();

            if (summary.FailureCount > 0 || summary.ErrorCount > 0)
                PrintErrorReport();

            if (summary.NotRunCount > 0)
                PrintNotRunReport();

            //if (commandLineOptions.Full)
            //    PrintFullReport(result);
        }

        /// <summary>
        /// Prints the Summary Report
        /// </summary>
        public void PrintSummaryReport()
        {
            writer.WriteLine(
                "Tests run: {0}, Passed: {1}, Errors: {2}, Failures: {3}, Inconclusive: {4}",
                summary.TestCount, summary.PassCount, summary.ErrorCount, summary.FailureCount, summary.InconclusiveCount);
            writer.WriteLine(
                "  Not run: {0}, Invalid: {1}, Ignored: {2}, Skipped: {3}",
                summary.NotRunCount, summary.InvalidCount, summary.IgnoreCount, summary.SkipCount);
            writer.WriteLine("Elapsed time: {0}", result.Duration);
        }

        /// <summary>
        /// Prints the Error Report
        /// </summary>
        public void PrintErrorReport()
        {
            reportCount = 0;
            writer.WriteLine();
            writer.WriteLine("Errors and Failures:");
            PrintErrorResults(this.result);
        }

        /// <summary>
        /// Prints the Not Run Report
        /// </summary>
        public void PrintNotRunReport()
        {
            reportCount = 0;
            writer.WriteLine();
            writer.WriteLine("Tests Not Run:");
            PrintNotRunResults(this.result);
        }

        /// <summary>
        /// Prints a full report of all results
        /// </summary>
        public void PrintFullReport()
        {
            writer.WriteLine();
            writer.WriteLine("All Test Results:");
            PrintAllResults(this.result, " ");
        }

        #region Helper Methods

        private void PrintErrorResults(ITestResult result)
        {
            if (result.ResultState.Status == TestStatus.Failed)
                if (!result.HasChildren)
                    WriteSingleResult(result);

            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    PrintErrorResults(childResult);
        }

        private void PrintNotRunResults(ITestResult result)
        {
            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    PrintNotRunResults(childResult);
            else if (result.ResultState.Status == TestStatus.Skipped)
                WriteSingleResult(result);
        }

        private void PrintTestProperties(ITest test)
        {
            foreach (PropertyEntry entry in test.Properties)
                writer.WriteLine("  {0}: {1}", entry.Name, entry.Value);
        }

        private void PrintAllResults(ITestResult result, string indent)
        {
            string status = null;
            switch (result.ResultState.Status)
            {
                case TestStatus.Failed:
                    status = "FAIL";
                    break;
                case TestStatus.Skipped:
                    status = "SKIP";
                    break;
                case TestStatus.Inconclusive:
                    status = "INC ";
                    break;
                case TestStatus.Passed:
                    status = "OK  ";
                    break;
            }

            writer.Write(status);
            writer.Write(indent);
            writer.WriteLine(result.Name);

            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    PrintAllResults(childResult, indent + "  ");
        }

        private void WriteSingleResult(ITestResult result)
        {
            writer.WriteLine();
            writer.WriteLine("{0}) {1} ({2})", ++reportCount, result.Name, result.FullName);

            if (result.Message != null && result.Message != string.Empty)
                writer.WriteLine("   {0}", result.Message);

            if (result.StackTrace != null && result.StackTrace != string.Empty)
                writer.WriteLine(result.ResultState == ResultState.Failure
                    ? StackFilter.Filter(result.StackTrace)
                    : result.StackTrace + Environment.NewLine);
        }

        #endregion
    }
}
