// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;

namespace TCLite.Framework.Internal.Tests
{
    public class TestMethodTests
    {
        private TestMethod _testMethod;

        [SetUp]
        public void CreateTestMethod()
        {
            var method = GetType().GetMethod(nameof(MyTestMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            _testMethod = new TestMethod(method, null);
         
            Assert.NotNull(_testMethod);
        }

        [Test]
        public void TestType()
        {
            Assert.That(_testMethod.TestType, Is.EqualTo("TestMethod"));
        }

        [Test]
        public void MethodName()
        {
            Assert.That(_testMethod.MethodName, Is.EqualTo(nameof(MyTestMethod)));
        }

        private void MyTestMethod()
        {

        }
    }
}