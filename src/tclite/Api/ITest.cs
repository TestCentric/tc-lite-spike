// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Framework.Api
{
	/// <summary>
	/// Common interface supported by all representations
	/// of a test. Only includes informational fields.
	/// The Run method is specifically excluded to allow
	/// for data-only representations of a test.
	/// </summary>
	public interface ITest : IXmlNodeBuilder
    {
        /// <summary>
        /// Gets or sets the id of the test
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets the name of the test
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the test
        /// </summary>
        string TestType { get; }
        /// <summary>
        /// Gets the fully qualified name of the test
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the name of the class containing this test. Returns
        /// null if the test is not associated with a class.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets the name of the method implementing this test.
        /// Returns null if the test is not implemented as a method.
        /// </summary>
        string MethodName { get; }

        /// <summary>
        /// Indicates whether the test can be run using
        /// the RunState enum.
        /// </summary>
		RunState RunState { get; set; }

        /// <summary>
        /// Count of the test cases ( 1 if this is a test case )
        /// </summary>
		int TestCaseCount { get; }

		/// <summary>
		/// Gets the properties of the test
		/// </summary>
		IPropertyBag Properties { get; }

        /// <summary>
        /// Gets the parent test, if any.
        /// </summary>
        /// <value>The parent test or null if none exists.</value>
        ITest Parent { get; }

        MethodInfo[] SetUpMethods { get; }

        MethodInfo[] TearDownMethods { get; }

        /// <summary>
        /// Returns true if this is a test suite
        /// </summary>
        bool IsSuite { get; }

        /// <summary>
        /// Gets a bool indicating whether the current test
        /// has any descendant tests.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets the Int value representing the seed for the RandomGenerator
        /// </summary>
        /// <value></value>
        int Seed { get; }

        /// <summary>
        /// Gets this test's child tests
        /// </summary>
        /// <value>A list of child tests</value>
        System.Collections.Generic.IList<ITest> Tests { get; }

        /// <summary>
        /// Gets a fixture object for running this test.
        /// </summary>
        object Fixture { get; }

        /// <summary>
        /// The arguments to use in creating the test or empty array if none are required.
        /// </summary>
        object[] Arguments { get; }
    }
}

