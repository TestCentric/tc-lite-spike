// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// TestCaseSourceAttribute indicates the source to be used to
    /// provide test cases for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestCaseSourceAttribute : DataAttribute, ITestCaseSource
    {
        private readonly string sourceName;
        private readonly Type sourceType;

        /// <summary>
        /// Construct with the name of the method, property or field that will prvide data
        /// </summary>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public TestCaseSourceAttribute(string sourceName)
        {
            this.sourceName = sourceName;
        }

        /// <summary>
        /// Construct with a Type and name
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public TestCaseSourceAttribute(Type sourceType, string sourceName)
        {
            this.sourceType = sourceType;
            this.sourceName = sourceName;
        }
        
        /// <summary>
        /// Construct with a Type
        /// </summary>
        /// <param name="sourceType">The type that will provide data</param>
        public TestCaseSourceAttribute(Type sourceType)
        {
            this.sourceType = sourceType;
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
            get { return sourceType;  }
        }

        private string category;
        /// <summary>
        /// Gets or sets the category associated with this test.
        /// May be a single category or a comma-separated list.
        /// </summary>
        public string Category 
        {
            get { return category; }
            set { category = value; }
        }

        #region ITestCaseSource Members
        /// <summary>
        /// Returns a set of ITestCaseDataItems for use as arguments
        /// to a parameterized test method.
        /// </summary>
        /// <param name="method">The method for which data is needed.</param>
        /// <returns></returns>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> data = new List<ITestCaseData>();
            IEnumerable source = GetTestCaseSource(method);

            if (source != null)
            {
                ParameterInfo[] parameters = method.GetParameters();

                foreach (object item in source)
                {
                    ParameterSet parms = new ParameterSet();
                    ITestCaseData testCaseData = item as ITestCaseData;
					
					if (testCaseData != null)
						parms = new ParameterSet(testCaseData);
                    else if (item is object[])
                    {
                        object[] array = item as object[];
                        parms.Arguments = array.Length == parameters.Length
                            ? array
                            : new object[] { item };
                    }
                    //else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(item.GetType()))
                    //{
                    //    parms.Arguments = new object[] { item };
                    //}
                    else if (item is Array)
                    {
                        Array array = item as Array;

                        if (array.Rank == 1 && array.Length == parameters.Length)
                        {
                            parms.Arguments = new object[array.Length];
                            for (int i = 0; i < array.Length; i++)
                                parms.Arguments[i] = (object)array.GetValue(i);
                        }
                        else
                        {
                            parms.Arguments = new object[] { item };
                        }
                    }
                    else
                    {
                        parms.Arguments = new object[] { item };
                    }

                    if (this.Category != null)
                        foreach (string cat in this.Category.Split(new char[] { ',' }))
                            parms.Properties.Add(PropertyNames.Category, cat);

                    data.Add(parms);
                }
            }

            return data;
        }

        private IEnumerable GetTestCaseSource(MethodInfo method)
        {
            IEnumerable source = null;

            Type sourceType = this.sourceType;
            if (sourceType == null)
                sourceType = method.ReflectedType;

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
