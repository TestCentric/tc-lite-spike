// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// ValueSourceAttribute indicates the source to be used to
    /// provide data for one parameter of a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class ValueSourceAttribute : DataAttribute, Api.IParameterDataSource
    {
        private readonly string sourceName;
        private readonly Type sourceType;

        /// <summary>
        /// Construct with the name of the factory - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="sourceName">The name of the data source to be used</param>
        public ValueSourceAttribute(string sourceName)
        {
            this.sourceName = sourceName;
        }

        /// <summary>
        /// Construct with a Type and name - for use with languages
        /// that don't support params arrays.
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public ValueSourceAttribute(Type sourceType, string sourceName)
        {
            this.sourceType = sourceType;
            this.sourceName = sourceName;
        }

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source
        /// </summary>
        public string SourceName
        {
            get { return sourceName; }
        }

        /// <summary>
        /// A Type to be used as a source
        /// </summary>
        public Type SourceType
        {
            get { return sourceType; }
        }

        #region IParameterDataSource Members

        /// <summary>
        /// Gets an enumeration of data items for use as arguments
        /// for a test method parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which data is needed</param>
        /// <returns>
        /// An enumeration containing individual data items
        /// </returns>
        public IEnumerable GetData(ParameterInfo parameter)
        {
            var data = new ArrayList();
            IEnumerable source = GetDataSource(parameter);

            if (source != null)
                foreach (object item in source)
                    data.Add(item);

            return source;
        }

        #endregion

        #region Helper Methods

        private IEnumerable GetDataSource(ParameterInfo parameter)
        {
            IEnumerable source = null;

            Type sourceType = this.sourceType;
            if (sourceType == null)
                sourceType = parameter.Member.ReflectedType;

            // TODO: Test this
            if (this.sourceName == null)
            {
                return Reflect.Construct(sourceType) as IEnumerable;
            }

            MemberInfo[] members = sourceType.GetMember(sourceName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (members.Length == 1)
            {
                MemberInfo member = members[0];
                object sourceobject = Internal.Reflect.Construct(sourceType);
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo field = member as FieldInfo;
                        source = (IEnumerable)field.GetValue(sourceobject);
                        break;
                    case MemberTypes.Property:
                        PropertyInfo property = member as PropertyInfo;
                        source = (IEnumerable)property.GetValue(sourceobject, null);
                        break;
                    case MemberTypes.Method:
                        MethodInfo m = member as MethodInfo;
                        source = (IEnumerable)m.Invoke(sourceobject, null);
                        break;
                }
            }
            return source;
        }

        #endregion
    }
}
