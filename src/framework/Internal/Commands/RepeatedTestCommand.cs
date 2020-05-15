// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************
#if NYI
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Commands
{
    /// <summary>
    /// TODO: Documentation needed for class
    /// </summary>
    public class RepeatedTestCommand : DelegatingTestCommand
    {
        private int repeatCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatedTestCommand"/> class.
        /// TODO: Add a comment about where the repeat count is retrieved. 
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        public RepeatedTestCommand(TestCommand innerCommand)
            : base(innerCommand)
        {
            this.repeatCount = Test.Properties.GetSetting(PropertyNames.RepeatCount, 1);
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            int count = repeatCount;

            while (count-- > 0)
            {
                context.CurrentResult = innerCommand.Execute(context);

                // TODO: We may want to change this so that all iterations are run
                if (context.CurrentResult.ResultState == ResultState.Failure ||
                    context.CurrentResult.ResultState == ResultState.Error ||
                    context.CurrentResult.ResultState == ResultState.Cancelled)
                {
                    break;
                }
            }

            return context.CurrentResult;
        }
    }
}
#endif