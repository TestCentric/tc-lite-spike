// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TCLite.Framework.Builders;

namespace TCLite.Framework.Extensibility
{
    class ParameterDataProviders : IParameterDataProvider
    {
        private List<IParameterDataProvider> Extensions = new List<IParameterDataProvider>();

        public ParameterDataProviders()
        {
            Extensions.Add(new ParameterDataProvider());
            Extensions.Add(new DatapointProvider());
        }

        #region IDataPointProvider Members

        /// <summary>
        /// Determine whether any data is available for a parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>True if any data is available, otherwise false.</returns>
        public bool HasDataFor(ParameterInfo parameter)
        {
            foreach (IParameterDataProvider provider in Extensions)
                if (provider.HasDataFor(parameter))
                    return true;

            return false;
        }

        /// <summary>
        /// Return an IEnumerable providing data for use with the
        /// supplied parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>An IEnumerable providing the required data</returns>
        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
            var list = new List<object>();

            foreach (IParameterDataProvider provider in Extensions)
                if (provider.HasDataFor(parameter))
                    foreach (object o in provider.GetDataFor(parameter))
                        list.Add(o);

            return list;
        }
        #endregion
    }
}
