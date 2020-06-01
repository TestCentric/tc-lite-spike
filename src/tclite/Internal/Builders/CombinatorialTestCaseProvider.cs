// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Collections;
using TCLite.Framework.Api;
using TCLite.Framework.Extensibility;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// CombinatorialTestCaseProvider creates test cases from individual
    /// parameter data values, combining them using the CombiningStrategy
    /// indicated by an Attribute used on the test method.
    /// </summary>
    public class CombinatorialTestCaseProvider : ITestCaseProvider
    {
        #region Static Members
        static IParameterDataProvider dataPointProvider = new ParameterDataProviders();

        #endregion

        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>
        /// True if any cases are available, otherwise false.
        /// </returns>
        public bool HasTestCasesFor(System.Reflection.MethodInfo method)
        {
            if (method.GetParameters().Length == 0)
                return false;

            foreach (ParameterInfo parameter in method.GetParameters())
                if (!dataPointProvider.HasDataFor(parameter))
                    return false;

            return true;
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            return GetStrategy(method).GetTestCases();
        }
        #endregion

        #region GetStrategy

        /// <summary>
        /// Gets the strategy to be used in building test cases for this test.
        /// </summary>
        /// <param name="method">The method for which test cases are being built.</param>
        /// <returns></returns>
        private CombiningStrategy GetStrategy(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            IEnumerable[] sources = new IEnumerable[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                sources[i] = dataPointProvider.GetDataFor(parameters[i]);

            if (method.IsDefined(typeof(TCLite.Framework.SequentialAttribute), false))
                return new SequentialStrategy(sources);

            if (method.IsDefined(typeof(TCLite.Framework.PairwiseAttribute), false) &&
                method.GetParameters().Length > 2)
                    return new PairwiseStrategy(sources);

            return new CombinatorialStrategy(sources);
        }

        #endregion
    }
}
