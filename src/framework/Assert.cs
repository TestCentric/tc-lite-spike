// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using TCLite.Framework.Constraints;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void TestDelegate();

    /// <summary>
    /// The Assert class contains a collection of static methods that
    /// implement the most common assertions used in TCLite.
    /// </summary>
    public class Assert
    {
        #region Constructor

        /// <summary>
        /// We don't actually want any instances of this object, but some people
        /// like to inherit from it to add other static methods. Hence, the
        /// protected constructor disallows any instances of this object. 
        /// </summary>
        protected Assert() { }

        #endregion

        #region Equals and ReferenceEquals

#if !NETCF
        /// <summary>
        /// The Equals method throws an AssertionException. This is done 
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw new InvalidOperationException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// override the default ReferenceEquals to throw an AssertionException. This 
        /// implementation makes sure there is no mistake in calling this function 
        /// as part of Assert. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw new InvalidOperationException("Assert.ReferenceEquals should not be used for Assertions");
        }
#endif

        #endregion

        #region Utility Asserts

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to TCLite.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Pass(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new SuccessException(message);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message and arguments 
        /// that are passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Fail(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Ignore(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new IgnoreException(message);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as inconclusive.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Inconclusive(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new InconclusiveException(message);
        }

        #endregion

        #region Assert.That

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(object actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True, message, args);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(ref bool actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ActualValueDelegate<T> del, IResolveConstraint expr, string message=null, params object[] args)
        {
            Constraint constraint = expr.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(del))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ref T actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Asserts that the code represented by a delegate throws an exception
        /// that satisfies the constraint provided.
        /// </summary>
        /// <param name="code">A TestDelegate to be executed</param>
        /// <param name="constraint">A ThrowsConstraint used in the test</param>
        static public void That(TestDelegate code, IResolveConstraint constraint, string message=null, params object[] args)
        {
            Assert.That((object)code, constraint, message, args);
        }

        #endregion

        #region Assert.ByVal

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure. 
        /// Used as a synonym for That in rare cases where a private setter 
        /// causes a Visual Basic compilation error.
        /// </summary>
        /// <remarks>
        /// This method is provided for use by VB developers needing to test
        /// the value of properties with private setters.
        /// </remarks>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void ByVal(object actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Assert.That(actual, expression, message, args);
        }

        #endregion

        #region Throws, Catch and DoesNotThrow

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <param name="expression">A constraint to be satisfied by the exception</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Throws(IResolveConstraint expression, TestDelegate code, string message=null, params object[] args)
        {
            Exception caughtException = null;

#if NYI
            if (AsyncInvocationRegion.IsAsyncOperation(code))
            {
                using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(code))
                {
                    code();

                    try
                    {
                        region.WaitForPendingOperationsToComplete(null);
                    }
                    catch (Exception e)
                    {
                        caughtException = e;
                    }
                }
            }
            else
#endif
                try
            {
                code();
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.That(caughtException, expression, message, args);

            return caughtException;
        }

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <param name="expectedExceptionType">The exception Type expected</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Throws(Type expectedExceptionType, TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new ExceptionTypeConstraint(expectedExceptionType), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <typeparam name="T">Type of the expected exception</typeparam>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static T Throws<T>(TestDelegate code, string message=null, params object[] args) where T : Exception
        {
            return (T)Throws(typeof(T), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception when called
        /// and returns it.
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Catch(TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new InstanceOfTypeConstraint(typeof(Exception)), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception of a certain Type
        /// or one derived from it when called and returns it.
        /// </summary>
        /// <param name="expectedExceptionType">The expected Exception Type</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Catch(Type expectedExceptionType, TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new InstanceOfTypeConstraint(expectedExceptionType), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception of a certain Type
        /// or one derived from it when called and returns it.
        /// </summary>
        /// <typeparam name="T">The expected Exception Type</typeparam>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static T Catch<T>(TestDelegate code, string message=null, params object[] args) where T : System.Exception
        {
            return (T)Throws(new InstanceOfTypeConstraint(typeof(T)), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate does not throw an exception
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void DoesNotThrow(TestDelegate code, string message=null, params object[] args)
        {
            Assert.That(code, new ThrowsNothingConstraint(), message, args);
        }

        #endregion

        #region True

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void True(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True ,message, args);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsTrue(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True ,message, args);
        }

        #endregion

        #region False

        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void False(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.False ,message, args);
        }

        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsFalse(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.False ,message, args);
        }

        #endregion

        #region NotNull

        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void NotNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Not.Null ,message, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Not.Null ,message, args);
        }

        #endregion

        #region Null

        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Null(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Null ,message, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Null ,message, args);
        }

        #endregion

        #region AreEqual

        /// <summary>
        /// Verifies that two ints are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(int expected, int actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two longs are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(long expected, long actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two unsigned ints are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(uint expected, uint actual, string message, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message=null, args);
        }

        /// <summary>
        /// Verifies that two unsigned longs are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(ulong expected, ulong actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two decimals are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(decimal expected, decimal actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double actual, double delta, string message, params object[] args)
        {
            AssertDoublesAreEqual(expected, actual, delta, message=null, args);
        }

        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double? actual, double delta, string message=null, params object[] args)
        {
            AssertDoublesAreEqual(expected, (double)actual, delta, message, args);
        }

        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are not equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        #endregion

        #region AreNotEqual

        /// <summary>
        /// Verifies that two ints are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(int expected, int actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two longs are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(long expected, long actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two unsigned ints are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(uint expected, uint actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two unsigned longs are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(ulong expected, ulong actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two decimals are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(decimal expected, decimal actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message=null, args);
        }

        /// <summary>
        /// Verifies that two floats are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(float expected, float actual, string message, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message=null, args);
        }

        /// <summary>
        /// Verifies that two doubles are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(double expected, double actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two objects are not equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. NUnit
        /// has special semantics for some object types.
        /// If they are equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        #endregion

        #region Same / NotSame

        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreSame(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.SameAs(expected), message, args);
        }

        /// <summary>
        /// Asserts that two objects do not refer to the same object. If they
        /// are the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotSame(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.SameAs(expected), message, args);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper for Assert.AreEqual(double expected, double actual, ...)
        /// allowing code generation to work consistently.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        protected static void AssertDoublesAreEqual(double expected, double actual, double delta, string message, object[] args)
        {
            if (double.IsNaN(expected) || double.IsInfinity(expected))
                Assert.That(actual, Is.EqualTo(expected), message, args);
            else
                Assert.That(actual, Is.EqualTo(expected).Within(delta), message, args);
        }

        private static void IncrementAssertCount()
        {
            TestExecutionContext.CurrentContext.IncrementAssertCount();
        }

        #endregion
    }
}
