// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Commands
{
    /// <summary>
    /// TheoryResultCommand adjusts the result of a Theory so that
    /// it fails if all the results were inconclusive.
    /// </summary>
    public class TheoryResultCommand : DelegatingTestCommand
    {
        /// <summary>
        /// Constructs a TheoryResultCommand 
        /// </summary>
        /// <param name="command">The command to be wrapped by this one</param>
        public TheoryResultCommand(TestCommand command) : base(command) { }

        /// <summary>
        /// Overridden to call the inner command and adjust the result
        /// in case all chlid results were inconclusive.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            TestResult theoryResult = innerCommand.Execute(context);

            if (theoryResult.ResultState == ResultState.Success)
            {
                if (!theoryResult.HasChildren)
                    theoryResult.SetResult(ResultState.Failure, "No test cases were provided", null);
                else
                {
                    bool wasInconclusive = true;
                    foreach (TestResult childResult in theoryResult.Children)
                        if (childResult.ResultState == ResultState.Success)
                        {
                            wasInconclusive = false;
                            break;
                        }

                    if (wasInconclusive)
                        theoryResult.SetResult(ResultState.Failure, "All test cases were inconclusive", null);
                }
            }

            return theoryResult;
        }
    }
}
