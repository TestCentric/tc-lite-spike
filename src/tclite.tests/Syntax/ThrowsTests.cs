// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;

namespace TCLite.Framework.Syntax
{
    [TestFixture]
    public class ThrowsTests
    {
        [Test]
        public void ThrowsException()
        {
            IResolveConstraint expr = Throws.Exception;
            Assert.AreEqual(
                "<throws>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsExceptionWithConstraint()
        {
            IResolveConstraint expr = Throws.Exception.With.Property("ParamName").EqualTo("myParam");
            Assert.AreEqual(
                @"<throws <property ParamName <equal ""myParam"">>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsExceptionTypeOf()
        {
            IResolveConstraint expr = Throws.Exception.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTypeOf()
        {
            IResolveConstraint expr = Throws.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTypeOfAndConstraint()
        {
            IResolveConstraint expr = Throws.TypeOf(typeof(ArgumentException)).And.Property("ParamName").EqualTo("myParam");
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsExceptionTypeOfAndConstraint()
        {
            IResolveConstraint expr = Throws.Exception.TypeOf(typeof(ArgumentException)).And.Property("ParamName").EqualTo("myParam");
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTypeOfWithConstraint()
        {
            IResolveConstraint expr = Throws.TypeOf(typeof(ArgumentException)).With.Property("ParamName").EqualTo("myParam");
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property ParamName <equal ""myParam"">>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTypeofWithMessage()
        {
            IResolveConstraint expr = Throws.TypeOf(typeof(ArgumentException)).With.Message.EqualTo("my message");
            Assert.AreEqual(
                @"<throws <and <typeof System.ArgumentException> <property Message <equal ""my message"">>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsInstanceOf()
        {
            IResolveConstraint expr = Throws.InstanceOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <instanceof System.ArgumentException>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsExceptionInstanceOf()
        {
            IResolveConstraint expr = Throws.Exception.InstanceOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <instanceof System.ArgumentException>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsInnerException()
        {
            IResolveConstraint expr = Throws.InnerException.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <property InnerException <typeof System.ArgumentException>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsExceptionWithInnerException()
        {
            IResolveConstraint expr = Throws.Exception.With.InnerException.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <property InnerException <typeof System.ArgumentException>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTypeOfWithInnerException()
        {
            IResolveConstraint expr = Throws.TypeOf(typeof(System.Reflection.TargetInvocationException))
                .With.InnerException.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <and <typeof System.Reflection.TargetInvocationException> <property InnerException <typeof System.ArgumentException>>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsTargetInvocationExceptionWithInnerException()
        {
            IResolveConstraint expr = Throws.TargetInvocationException
                .With.InnerException.TypeOf(typeof(ArgumentException));
            Assert.AreEqual(
                "<throws <and <typeof System.Reflection.TargetInvocationException> <property InnerException <typeof System.ArgumentException>>>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsArgumentException()
        {
            IResolveConstraint expr = Throws.ArgumentException;
            Assert.AreEqual(
                "<throws <typeof System.ArgumentException>>",
                expr.Resolve().ToString());
        }

        [Test]
        public void ThrowsInvalidOperationException()
        {
            IResolveConstraint expr = Throws.InvalidOperationException;
            Assert.AreEqual(
                "<throws <typeof System.InvalidOperationException>>",
                expr.Resolve().ToString());
        }

#if !NETCF
        [Test]
        public void DelegateThrowsException()
        {
            Assert.That(
                delegate { throw new ArgumentException(); },
                Throws.Exception);
        }

        [Test]
        public void LambdaThrowsExcepton()
        {
            Assert.That(
                () => new MyClass(null),
                Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void LambdaThrowsExceptionWithMessage()
        {
            Assert.That(
                () => new MyClass(null),
                Throws.InstanceOf<ArgumentNullException>()
                .And.Message.Matches("null"));
        }

        internal class MyClass
        {
            public MyClass(string s)
            {
                if (s == null)
                {
                    throw new ArgumentNullException();
                }
            }
        }

        [Test]
        public void LambdaThrowsNothing()
        {
            Assert.That(() => (object)null, Throws.Nothing);
        }
#endif
    }
}
