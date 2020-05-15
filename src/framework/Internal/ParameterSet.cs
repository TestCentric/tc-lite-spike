// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// ParameterSet encapsulates method arguments and
    /// other selected parameters needed for constructing
    /// a parameterized test case.
    /// </summary>
    public class ParameterSet : ITestCaseData, IApplyToTest
    {
        #region Instance Fields

        private object[] arguments;
        private object[] originalArguments;
        private object result;
        private bool hasExpectedResult;

        /// <summary>
        /// A dictionary of properties, used to add information
        /// to tests without requiring the class to change.
        /// </summary>
        private IPropertyBag properties;

        #endregion

        #region Properties

        private RunState runState;
        /// <summary>
        /// The RunState for this set of parameters.
        /// </summary>
        public RunState RunState 
        {
            get { return runState; }
            set { runState = value; }
        }

        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
            set 
            { 
                arguments = value;

                if (originalArguments == null)
                    originalArguments = value;
            }
        }

        /// <summary>
        /// The original arguments provided by the user,
        /// used for display purposes.
        /// </summary>
        public object[] OriginalArguments
        {
            get { return originalArguments; }
        }

        /// <summary>
        /// The expected result of the test, which
        /// must match the method return type.
        /// </summary>
        public object ExpectedResult
        {
            get { return result; }
            set
            {
                result = value;
                hasExpectedResult = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an expected result was specified.
        /// </summary>
        public bool HasExpectedResult
        {
            get { return hasExpectedResult; }
        }

        private string testName;
        /// <summary>
        /// A name to be used for this test case in lieu
        /// of the standard generated name containing
        /// the argument list.
        /// </summary>
        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        public IPropertyBag Properties
        {
            get
            {
                if (properties == null)
                    properties = new PropertyBag();

                return properties;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a non-runnable ParameterSet, specifying
        /// the provider exception that made it invalid.
        /// </summary>
        public ParameterSet(Exception exception)
        {
            this.RunState = RunState.NotRunnable;
            this.Properties.Set(PropertyNames.SkipReason, ExceptionHelper.BuildMessage(exception));
            this.Properties.Set(PropertyNames.ProviderStackTrace, ExceptionHelper.BuildStackTrace(exception));
        }

        /// <summary>
        /// Construct an empty parameter set, which
        /// defaults to being Runnable.
        /// </summary>
        public ParameterSet()
        {
            this.RunState = RunState.Runnable;
        }

        /// <summary>
        /// Construct a ParameterSet from an object implementing ITestCaseData
        /// </summary>
        /// <param name="data"></param>
        public ParameterSet(ITestCaseData data)
        {
            this.TestName = data.TestName;
            this.RunState = data.RunState;
            this.Arguments = data.Arguments;
            
			if (data.HasExpectedResult)
                this.ExpectedResult = data.ExpectedResult;
			
            foreach (string key in data.Properties.Keys)
                this.Properties[key] = data.Properties[key];
        }

        #endregion

        #region IApplyToTest Members

        /// <summary>
        /// Applies ParameterSet values to the test itself.
        /// </summary>
        /// <param name="test">A test.</param>
        public void ApplyToTest(Test test)
        {
            if (this.RunState != RunState.Runnable)
				test.RunState = this.RunState;

            foreach (string key in Properties.Keys)
                foreach (object value in Properties[key])
                    test.Properties.Add(key, value);
        }

        #endregion
    }
}
