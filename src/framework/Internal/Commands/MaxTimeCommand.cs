// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Diagnostics;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Commands
{
    /// <summary>
    /// TODO: Documentation needed for class
    /// </summary>
    public class MaxTimeCommand : DelegatingTestCommand
    {
        private int maxTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxTimeCommand"/> class.
        /// TODO: Add a comment about where the max time is retrieved.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        public MaxTimeCommand(TestCommand innerCommand)
            : base(innerCommand)
        {
            this.maxTime = Test.Properties.GetSetting(PropertyNames.MaxTime, 0);
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            // TODO: This command duplicates the calculation of the
            // duration of the test because that calculation is 
            // normally performed at a higher level. Most likely,
            // we should move the maxtime calculation to the
            // higher level eventually.
#if !SILVERLIGHT && !NETCF_2_0
            long startTicks = Stopwatch.GetTimestamp();
#endif

            TestResult testResult = innerCommand.Execute(context);

#if !SILVERLIGHT && !NETCF_2_0
            long tickCount = Stopwatch.GetTimestamp() - startTicks;
            double seconds = (double)tickCount / Stopwatch.Frequency;
            testResult.Duration = TimeSpan.FromSeconds(seconds);
#else
            testResult.Duration = DateTime.Now - context.StartTime;
#endif

            if (testResult.ResultState == ResultState.Success)
            {
                //int elapsedTime = (int)Math.Round(testResult.Time * 1000.0);
                double elapsedTime = testResult.Duration.TotalMilliseconds;

                if (elapsedTime > maxTime)
                    testResult.SetResult(ResultState.Failure,
                        string.Format("Elapsed time of {0}ms exceeds maximum of {1}ms",
                            elapsedTime, maxTime));
            }

            return testResult;
        }
    }
}