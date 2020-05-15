// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// ParameterizedFixtureSuite serves as a container for the set of test 
    /// fixtures created from a given Type using various parameters.
    /// </summary>
    public class ParameterizedFixtureSuite : TestSuite
    {
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterizedFixtureSuite"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ParameterizedFixtureSuite(Type type) : base(type.Namespace, TypeHelper.GetDisplayName(type)) 
        {
            this.type = type;
        }

        /// <summary>
        /// Gets the Type represented by this suite.
        /// </summary>
        /// <value>A Sysetm.Type.</value>
        public Type ParameterizedType
        {
            get { return type; }
        }

        /// <summary>
        /// Gets a string representing the type of test
        /// </summary>
        /// <value></value>
        public override string TestType
        {
            get
            {
                if (this.ParameterizedType.ContainsGenericParameters)
                    return "GenericFixture";
                
                return "ParameterizedFixture";
            }
        }
    }
}
