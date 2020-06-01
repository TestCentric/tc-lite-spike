// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Collections;
using TCLite.Framework.Extensibility;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// Provides data from fields marked with the DatapointAttribute or the
    /// DatapointsAttribute.
    /// </summary>
    public class DatapointProvider : IParameterDataProvider
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
        public bool HasDataFor(System.Reflection.ParameterInfo parameter)
        {
            Type parameterType = parameter.ParameterType;
            MemberInfo method = parameter.Member;
            Type fixtureType = method.ReflectedType;

            if (!method.IsDefined(typeof(TheoryAttribute), true))
                return false;

            if (parameterType == typeof(bool) || parameterType.IsEnum)
                return true;

            foreach (MemberInfo member in fixtureType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (member.IsDefined(typeof(DatapointAttribute), true) &&
                    GetTypeFromMemberInfo(member) == parameterType)
                        return true;
                else if (member.IsDefined(typeof(DatapointSourceAttribute), true) &&
                    GetElementTypeFromMemberInfo(member) == parameterType)
                    return true;
            }

            return false;
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
        public System.Collections.IEnumerable GetDataFor(System.Reflection.ParameterInfo parameter)
        {
            ArrayList datapoints = new ArrayList();

            Type parameterType = parameter.ParameterType;
            Type fixtureType = parameter.Member.ReflectedType;

            foreach (MemberInfo member in fixtureType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                if (member.IsDefined(typeof(DatapointAttribute), true))
                {
                    if (GetTypeFromMemberInfo(member) == parameterType &&
                        member.MemberType == MemberTypes.Field)
                    {
                        FieldInfo field = member as FieldInfo;
                        if (field.IsStatic)
                            datapoints.Add(field.GetValue(null));
                        else
                            datapoints.Add(field.GetValue(ProviderCache.GetInstanceOf(fixtureType)));
                    }
                }
                else if (member.IsDefined(typeof(DatapointSourceAttribute), true))
                {
                    if (GetElementTypeFromMemberInfo(member) == parameterType)
                    {
                        object instance;

                        switch(member.MemberType)
                        {
                            case MemberTypes.Field:
                                FieldInfo field = member as FieldInfo;
                                instance = field.IsStatic ? null : ProviderCache.GetInstanceOf(fixtureType);
                                foreach (object data in (IEnumerable)field.GetValue(instance))
                                    datapoints.Add(data);
                                break;
                            case MemberTypes.Property:
                                PropertyInfo property = member as PropertyInfo;
                                MethodInfo getMethod = property.GetGetMethod(true);
                                instance = getMethod.IsStatic ? null : ProviderCache.GetInstanceOf(fixtureType);
                                foreach (object data in (IEnumerable)property.GetValue(instance,null))
                                    datapoints.Add(data);
                                break;
                            case MemberTypes.Method:
                                MethodInfo method = member as MethodInfo;
                                instance = method.IsStatic ? null : ProviderCache.GetInstanceOf(fixtureType);
                                foreach (object data in (IEnumerable)method.Invoke(instance, new Type[0]))
                                    datapoints.Add(data);
                                break;
                        }
                    }
                }
            }

            if (datapoints.Count == 0)
            {
                if (parameterType == typeof(bool))
                {
                    datapoints.Add(true);
                    datapoints.Add(false);
                }
                else if (parameterType.IsEnum)
                {
                    datapoints.AddRange(TypeHelper.GetEnumValues(parameterType));
                }
            }

            return datapoints;
        }

        private Type GetTypeFromMemberInfo(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                default:
                    return null;
            }
        }

        private Type GetElementTypeFromMemberInfo(MemberInfo member)
        {
            Type type = GetTypeFromMemberInfo(member);

            if (type == null)
                return null;

            if (type.IsArray)
                return type.GetElementType();

            if (type.IsGenericType && type.Name == "IEnumerable`1")
                return type.GetGenericArguments()[0];

            return null;
        }

        #endregion
    }
}
