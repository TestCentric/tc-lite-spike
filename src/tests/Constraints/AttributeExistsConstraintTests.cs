// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class AttributeExistsConstraintTests : ConstraintTestBase
    {
        [SetUp]
        public void SetUp()
        {
            theConstraint = new AttributeExistsConstraint(typeof(TestFixtureAttribute));
            expectedDescription = "type with attribute <TCLite.Framework.TestFixtureAttribute>";
            stringRepresentation = "<attributeexists TCLite.Framework.TestFixtureAttribute>";
        }

        internal object[] SuccessData = new object[] { typeof(AttributeExistsConstraintTests) };

        internal object[] FailureData = new object[] { 
            new TestCaseData( typeof(D2), "<TCLite.Framework.Constraints.Tests.AttributeExistsConstraintTests+D2>" ) };

        [Test]
        public void NonAttributeThrowsException()
        {
            Assert.Throws<System.ArgumentException>(() =>
            {
                new AttributeExistsConstraint(typeof(string));
            });
        }

        [Test]
        public void AttributeExistsOnMethodInfo()
        {
            Assert.That(
                GetType().GetMethod("AttributeExistsOnMethodInfo"),
                new AttributeExistsConstraint(typeof(TestAttribute)));
        }

        [Test, Description("my description")]
        public void AttributeTestPropertyValueOnMethodInfo()
        {
            Assert.That(
                GetType().GetMethod("AttributeTestPropertyValueOnMethodInfo"),
                Has.Attribute(typeof(DescriptionAttribute)).Property("Properties").Property("Keys").Contains("Description"));
        }

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}