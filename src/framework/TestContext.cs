// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Provide the context information of the current test.
    /// This is an adapter for the internal ExecutionContext
    /// class, hiding the internals from the user test.
    /// </summary>
    public class TestContext
    {
        private TestExecutionContext ec;
        private TestAdapter test;
        private ResultAdapter result;

        #region Constructor

        /// <summary>
        /// Construct a TestContext for an ExecutionContext
        /// </summary>
        /// <param name="ec">The ExecutionContext to adapt</param>
        public TestContext(TestExecutionContext ec)
        {
            this.ec = ec;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the current test context. This is created
        /// as needed. The user may save the context for
        /// use within a test, but it should not be used
        /// outside the test for which it is created.
        /// </summary>
        public static TestContext CurrentContext
        {
            get { return new TestContext(TestExecutionContext.CurrentContext); }
        }

        /// <summary>
        /// Get a representation of the current test.
        /// </summary>
        public TestAdapter Test
        {
            get
            {
                if (test == null)
                    test = new TestAdapter(ec.CurrentTest);

                return test;
            }
        }

        /// <summary>
        /// Gets a Representation of the TestResult for the current test. 
        /// </summary>
        public ResultAdapter Result
        {
            get
            {
                if (result == null)
                    result = new ResultAdapter(ec.CurrentResult);

                return result;
            }
        }

#if !NETCF
        /// <summary>
        /// Gets the directory containing the current test assembly.
        /// </summary>
        public string TestDirectory
        {
            get
            {
                return AssemblyHelper.GetDirectoryName(ec.CurrentTest.FixtureType.Assembly);
            }
        }
#endif

        /// <summary>
        /// Gets the directory to be used for outputing files created
        /// by this test run.
        /// </summary>
        public string WorkDirectory
        {
            get
            {
				return ec.WorkDirectory;
        	}
        }

        public RandomGenerator Random
        {
            get
            {
                return ec.RandomGenerator;
            }
        }

        #endregion

        #region Nested TestAdapter Class

        /// <summary>
        /// TestAdapter adapts a Test for consumption by
        /// the user test code.
        /// </summary>
        public class TestAdapter
        {
            private Test test;

            #region Constructor

            /// <summary>
            /// Construct a TestAdapter for a Test
            /// </summary>
            /// <param name="test">The Test to be adapted</param>
            public TestAdapter(Test test)
            {
                this.test = test;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the unique Id of a test
            /// </summary>
            public string ID
            {
                get { return test.Id; }
            }

            /// <summary>
            /// The name of the test, which may or may not be 
            /// the same as the method name.
            /// </summary>
            public string Name
            {
                get
                {
                    return test.Name;
                }
            }
			
            /// <summary>
            /// The name of the method representing the test.
            /// </summary>
			public string MethodName
			{
				get
				{
					return test is TestMethod
						? ((TestMethod)test).Method.Name
						: null;
				}
			}

            /// <summary>
            /// The FullName of the test
            /// </summary>
            public string FullName
            {
                get
                {
                    return test.FullName;
                }
            }

            /// <summary>
            /// The properties of the test.
            /// </summary>
            public IPropertyBag Properties
            {
                get
                {
                    return test.Properties;
                }
            }

            #endregion
        }

        #endregion

        #region Nested ResultAdapter Class

        /// <summary>
        /// ResultAdapter adapts a TestResult for consumption by
        /// the user test code.
        /// </summary>
        public class ResultAdapter
        {
            private TestResult result;

            #region Constructor

            /// <summary>
            /// Construct a ResultAdapter for a TestResult
            /// </summary>
            /// <param name="result">The TestResult to be adapted</param>
            public ResultAdapter(TestResult result)
            {
                this.result = result;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets a ResultState representing the outcome of the test.
            /// </summary>
            public ResultState Outcome
            {
                get
                {
                    return result.ResultState;
                }
            }

            #endregion
        }
        
        #endregion
    }
}
