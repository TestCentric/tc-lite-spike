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
    /// ParameterDataProvider supplies individual argument values for
    /// single parameters using attributes derived from DataAttribute.
    /// </summary>
    public class ParameterDataProvider : IParameterDataProvider
    {
        #region IDataPointProvider Members

        /// <summary>
        /// Determine whether any data is available for a parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>
        /// True if any data is available, otherwise false.
        /// </returns>
        public bool HasDataFor(ParameterInfo parameter)
        {
            return parameter.IsDefined(typeof(DataAttribute), false);
        }

        /// <summary>
        /// Return an IEnumerable providing data for use with the
        /// supplied parameter.
        /// </summary>
        /// <param name="parameter">A ParameterInfo representing one
        /// argument to a parameterized test</param>
        /// <returns>
        /// An IEnumerable providing the required data
        /// </returns>
        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
            var data = new ArrayList();

            foreach (Attribute attr in parameter.GetCustomAttributes(typeof(DataAttribute), false))
            {
                IParameterDataSource source = attr as IParameterDataSource;
                if (source != null)
                    foreach (object item in source.GetData(parameter))
                        data.Add(item);
            }

            return data;
        }
        #endregion
    }
}
