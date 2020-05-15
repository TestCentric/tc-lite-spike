// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Runner
{
    /// <summary>
    /// ResultReporter writes the NUnitLite results to a TextWriter.
    /// </summary>
    public class ResultReporter
    {
        private ExtendedTextWriter writer;
        private ITestResult result;
        private ResultSummary summary;
        private int reportCount = 0;

        /// <summary>
        /// Constructs an instance of ResultReporter
        /// </summary>
        /// <param name="result">The top-level result being reported</param>
        /// <param name="writer">A TextWriter to which the report is written</param>
        public ResultReporter(ITestResult result, ExtendedTextWriter writer)
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
            DisplaySummaryReport();

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
        public void DisplaySummaryReport()
        {
            var status = summary.ResultState.Status;

            var overallResult = status.ToString();
            if (overallResult == "Skipped")
                overallResult = "Warning";

            ColorStyle overallStyle = status == TestStatus.Passed
                ? ColorStyle.Pass
                : status == TestStatus.Failed
                    ? ColorStyle.Failure
                    : status == TestStatus.Skipped
                        ? ColorStyle.Warning
                        : ColorStyle.Output;

            // if (_testCreatedOutput)
            //     Writer.WriteLine();

            writer.WriteLine(ColorStyle.SectionHeader, "Test Run Summary");
            writer.WriteLabelLine("  Overall result: ", overallResult, overallStyle);

            WriteSummaryCount("  Test Count: ", summary.TestCount);
            WriteSummaryCount(", Passed: ", summary.PassCount);
            WriteSummaryCount(", Failed: ", summary.FailedCount, ColorStyle.Failure);
            //WriteSummaryCount(", Warnings: ", summary.WarningCount, ColorStyle.Warning);
            WriteSummaryCount(", Inconclusive: ", summary.InconclusiveCount);
            //WriteSummaryCount(", Skipped: ", summary.TotalSkipCount);
            writer.WriteLine();

            if (summary.FailedCount > 0)
            {
                WriteSummaryCount("    Failed Tests - Failures: ", summary.FailureCount, ColorStyle.Failure);
                WriteSummaryCount(", Errors: ", summary.ErrorCount, ColorStyle.Error);
                WriteSummaryCount(", Invalid: ", summary.InvalidCount, ColorStyle.Error);
                writer.WriteLine();
            }
            if (summary.NotRunCount > 0)
            {
                WriteSummaryCount("    Skipped Tests - Ignored: ", summary.IgnoreCount, ColorStyle.Warning);
                //WriteSummaryCount(", Explicit: ", summary.ExplicitCount);
                WriteSummaryCount(", Other: ", summary.SkipCount);
                writer.WriteLine();
            }

            //writer.WriteLabelLine("  Start time: ", summary.StartTime.ToString("u"));
            //writer.WriteLabelLine("    End time: ", summary.EndTime.ToString("u"));
            writer.WriteLabelLine("    Duration: ", string.Format(NumberFormatInfo.InvariantInfo, "{0:0.000} seconds", summary.Duration));
            writer.WriteLine();
        }

        private void WriteSummaryCount(string label, int count)
        {
            writer.WriteLabel(label, count.ToString(CultureInfo.CurrentUICulture));
        }

        private void WriteSummaryCount(string label, int count, ColorStyle color)
        {
            writer.WriteLabel(label, count.ToString(CultureInfo.CurrentUICulture), count > 0 ? color : ColorStyle.Value);
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
