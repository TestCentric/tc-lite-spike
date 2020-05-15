// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;

namespace TCLite.Framework
{
    /// <summary>
    /// Helper class with properties and methods that supply
    /// constraints that operate on exceptions.
    /// </summary>
    public class Throws
    {
        #region Exception

        /// <summary>
        /// Creates a constraint specifying an expected exception
        /// </summary>
        public static ResolvableConstraintExpression Exception
        {
            get { return new ConstraintExpression().Append(new ThrowsOperator()); }
        }

        #endregion

        #region InnerException

        /// <summary>
        /// Creates a constraint specifying an exception with a given InnerException
        /// </summary>
        public static ResolvableConstraintExpression InnerException
        {
            get { return Exception.InnerException; }
        }

        #endregion

        #region TargetInvocationException

        /// <summary>
        /// Creates a constraint specifying an expected TargetInvocationException
        /// </summary>
        public static ExactTypeConstraint TargetInvocationException
        {
            get { return TypeOf(typeof(System.Reflection.TargetInvocationException)); }
        }

        #endregion

        #region ArgumentException

        /// <summary>
        /// Creates a constraint specifying an expected TargetInvocationException
        /// </summary>
        public static ExactTypeConstraint ArgumentException
        {
            get { return TypeOf(typeof(System.ArgumentException)); }
        }

        #endregion

        #region InvalidOperationException

        /// <summary>
        /// Creates a constraint specifying an expected TargetInvocationException
        /// </summary>
        public static ExactTypeConstraint InvalidOperationException
        {
            get { return TypeOf(typeof(System.InvalidOperationException)); }
        }

        #endregion

        #region Nothing

        /// <summary>
        /// Creates a constraint specifying that no exception is thrown
        /// </summary>
        public static ThrowsNothingConstraint Nothing
        {
            get { return new ThrowsNothingConstraint(); }
        }

        #endregion

        #region TypeOf

        /// <summary>
        /// Creates a constraint specifying the exact type of exception expected
        /// </summary>
        public static ExactTypeConstraint TypeOf(Type expectedType)
        {
            return Exception.TypeOf(expectedType);
        }

        /// <summary>
        /// Creates a constraint specifying the exact type of exception expected
        /// </summary>
        public static ExactTypeConstraint TypeOf<T>()
        {
            return TypeOf(typeof(T));
        }

        #endregion

        #region InstanceOf

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return Exception.InstanceOf(expectedType);
        }

        /// <summary>
        /// Creates a constraint specifying the type of exception expected
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf<T>()
        {
            return InstanceOf(typeof(T));
        }

        #endregion

    }
}
