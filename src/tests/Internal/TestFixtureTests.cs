// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.TestData.FixtureSetUpTearDownData;
using TCLite.TestUtilities;
using TCLite.TestData.TestFixtureData;
using IgnoredFixture = TCLite.TestData.TestFixtureData.IgnoredFixture;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// Tests of the NUnitTestFixture class
	/// </summary>
	[TestFixture]
	public class TestFixtureTests
	{
        private static void CanConstructFrom(Type fixtureType)
        {
            CanConstructFrom(fixtureType, fixtureType.Name);
        }

        private static void CanConstructFrom(Type fixtureType, string expectedName)
        {
            TestSuite fixture = TestBuilder.MakeFixture(fixtureType);
            Assert.AreEqual(expectedName, fixture.Name);
            Assert.AreEqual(fixtureType.FullName, fixture.FullName);
        }

        [Test]
		public void ConstructFromType()
		{
            CanConstructFrom(typeof(FixtureWithTestFixtureAttribute));
		}

		[Test]
		public void ConstructFromNestedType()
		{
            CanConstructFrom(typeof(OuterClass.NestedTestFixture), "OuterClass+NestedTestFixture");
		}

		[Test]
		public void ConstructFromDoublyNestedType()
		{
            CanConstructFrom(typeof(OuterClass.NestedTestFixture.DoublyNestedTestFixture),
                "OuterClass+NestedTestFixture+DoublyNestedTestFixture");
		}

        public void ConstructFromTypeWithoutTestFixtureAttributeContainingTest()
        {
            CanConstructFrom(typeof(FixtureWithoutTestFixtureAttributeContainingTest));
        }
 
        [Test]
        public void ConstructFromTypeWithoutTestFixtureAttributeContainingTestCase()
        {
            CanConstructFrom(typeof(FixtureWithoutTestFixtureAttributeContainingTestCase));
        }
 
        [Test]
        public void ConstructFromTypeWithoutTestFixtureAttributeContainingTestCaseSource()
        {
            CanConstructFrom(typeof(FixtureWithoutTestFixtureAttributeContainingTestCaseSource));
        }
 
        [Test]
        public void CannotRunConstructorWithArgsNotSupplied()
        {
            TestAssert.IsNotRunnable(typeof(NoDefaultCtorFixture));
        }

        [Test]
        public void CanRunConstructorWithArgsSupplied()
        {
            TestAssert.IsRunnable(typeof(FixtureWithArgsSupplied), ResultState.Success);
        }

        [Test]
		public void CannotRunBadConstructor()
		{
            TestAssert.IsNotRunnable(typeof(BadCtorFixture));
		}

		[Test] 
		public void CanRunMultipleSetUp()
		{
            TestAssert.IsRunnable(typeof(MultipleSetUpAttributes), ResultState.Success);
		}

		[Test] 
		public void CanRunMultipleTearDown()
		{
            TestAssert.IsRunnable(typeof(MultipleTearDownAttributes), ResultState.Success);
		}

		[Test]
		public void CannotRunIgnoredFixture()
		{
			TestSuite suite = TestBuilder.MakeFixture( typeof( IgnoredFixture ) );
			Assert.AreEqual( RunState.Ignored, suite.RunState );
			Assert.AreEqual( "testing ignore a fixture", suite.Properties.Get(PropertyNames.SkipReason) );
		}

//		[Test]
//		public void CannotRunAbstractFixture()
//		{
//            TestAssert.IsNotRunnable(typeof(AbstractTestFixture));
//		}

        [Test]
        public void CanRunFixtureDerivedFromAbstractTestFixture()
        {
            TestAssert.IsRunnable(typeof(DerivedFromAbstractTestFixture), ResultState.Success);
        }

        [Test]
        public void CanRunFixtureDerivedFromAbstractDerivedTestFixture()
        {
            TestAssert.IsRunnable(typeof(DerivedFromAbstractDerivedTestFixture), ResultState.Success);
        }

//		[Test]
//		public void CannotRunAbstractDerivedFixture()
//		{
//            TestAssert.IsNotRunnable(typeof(AbstractDerivedTestFixture));
//		}

        [Test]
        public void FixtureInheritingTwoTestFixtureAttributesIsLoadedOnlyOnce()
        {
            TestSuite suite = TestBuilder.MakeFixture(typeof(DoubleDerivedClassWithTwoInheritedAttributes));
            Assert.That(suite, Is.TypeOf(typeof(TestFixture)));
            Assert.That(suite.Tests.Count, Is.EqualTo(0));
        }

        [Test] 
		public void CanRunMultipleTestFixtureSetUp()
		{
            TestAssert.IsRunnable(typeof(MultipleFixtureSetUpAttributes), ResultState.Success);
		}

		[Test] 
		public void CanRunMultipleTestFixtureTearDown()
		{
            TestAssert.IsRunnable(typeof(MultipleFixtureTearDownAttributes), ResultState.Success);
		}

        [Test]
        public void CanRunTestFixtureWithNoTests()
        {
            TestAssert.IsRunnable(typeof(FixtureWithNoTests), ResultState.Success);
        }

        [Test]
        public void ConstructFromStaticTypeWithoutTestFixtureAttribute()
        {
            CanConstructFrom(typeof(StaticFixtureWithoutTestFixtureAttribute));
        }

        [Test]
        public void CanRunStaticFixture()
        {
            TestAssert.IsRunnable(typeof(StaticFixtureWithoutTestFixtureAttribute), ResultState.Success);
        }

#if !NETCF
        [Test, Platform(Exclude = "NETCF", Reason = "NYI")]
        public void CanRunGenericFixtureWithProperArgsProvided()
        {
            TestSuite suite = TestBuilder.MakeFixture(
                typeof(TCLite.TestData.TestFixtureData.GenericFixtureWithProperArgsProvided<>));
            Assert.That(suite.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(suite is ParameterizedFixtureSuite);
            Assert.That(suite.Tests.Count, Is.EqualTo(2));
        }

//        [Test]
//        public void CannotRunGenericFixtureWithNoTestFixtureAttribute()
//        {
//            TestSuite suite = TestBuilder.MakeFixture(
//                GetTestDataType("NUnit.TestData.TestFixtureData.GenericFixtureWithNoTestFixtureAttribute`1"));
//
//            Assert.That(suite.RunState, Is.EqualTo(RunState.NotRunnable));
//            Assert.That(suite.Properties.Get(PropertyNames.SkipReason), 
//                Is.StringStarting("Fixture type contains generic parameters"));
//        }

        [Test, Platform(Exclude = "NETCF", Reason = "NYI")]
        public void CannotRunGenericFixtureWithNoArgsProvided()
        {
            TestSuite suite = TestBuilder.MakeFixture(
                typeof(TCLite.TestData.TestFixtureData.GenericFixtureWithNoArgsProvided<>));

            Test fixture = (Test)suite.Tests[0];
            Assert.That(fixture.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That((string)fixture.Properties.Get(PropertyNames.SkipReason), Is.StringStarting("Fixture type contains generic parameters"));
        }

        [Test, Platform(Exclude = "NETCF", Reason = "NYI")]
        public void CannotRunGenericFixtureDerivedFromAbstractFixtureWithNoArgsProvided()
        {
            TestSuite suite = TestBuilder.MakeFixture(
                typeof(TCLite.TestData.TestFixtureData.GenericFixtureDerivedFromAbstractFixtureWithNoArgsProvided<>));
            TestAssert.IsNotRunnable((Test)suite.Tests[0]);
        }

        [Test, Platform(Exclude = "NETCF", Reason = "NYI")]
        public void CanRunGenericFixtureDerivedFromAbstractFixtureWithArgsProvided()
        {
            TestSuite suite = TestBuilder.MakeFixture(
                typeof(TCLite.TestData.TestFixtureData.GenericFixtureDerivedFromAbstractFixtureWithArgsProvided<>));
            Assert.That(suite.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(suite is ParameterizedFixtureSuite);
            Assert.That(suite.Tests.Count, Is.EqualTo(2));
        }
#endif

        #region SetUp Signature
        [Test] 
		public void CannotRunPrivateSetUp()
		{
            TestAssert.IsNotRunnable(typeof(PrivateSetUp));
		}

#if !SILVERLIGHT
        [Test] 
		public void CanRunProtectedSetUp()
		{
            TestAssert.IsRunnable(typeof(ProtectedSetUp), ResultState.Success);
		}
#endif

        /// <summary>
        /// Determines whether this instance [can run static set up].
        /// </summary>
		[Test] 
		public void CanRunStaticSetUp()
		{
            TestAssert.IsRunnable(typeof(StaticSetUp), ResultState.Success);
		}

		[Test]
		public void CannotRunSetupWithReturnValue()
		{
            TestAssert.IsNotRunnable(typeof(SetUpWithReturnValue));
		}

		[Test]
		public void CannotRunSetupWithParameters()
		{
            TestAssert.IsNotRunnable(typeof(SetUpWithParameters));
		}
		#endregion

		#region TearDown Signature
		[Test] 
		public void CannotRunPrivateTearDown()
		{
            TestAssert.IsNotRunnable(typeof(PrivateTearDown));
		}

#if !SILVERLIGHT
        [Test]
        public void CanRunProtectedTearDown()
        {
            TestAssert.IsRunnable(typeof(ProtectedTearDown), ResultState.Success);
        }
#endif

		[Test] 
		public void CanRunStaticTearDown()
		{
            TestAssert.IsRunnable(typeof(StaticTearDown), ResultState.Success);
		}

		[Test]
		public void CannotRunTearDownWithReturnValue()
		{
            TestAssert.IsNotRunnable(typeof(TearDownWithReturnValue));
		}

		[Test]
		public void CannotRunTearDownWithParameters()
		{
            TestAssert.IsNotRunnable(typeof(TearDownWithParameters));
		}
		#endregion

		#region TestFixtureSetUp Signature
		[Test] 
		public void CannotRunPrivateFixtureSetUp()
		{
            TestAssert.IsNotRunnable(typeof(PrivateFixtureSetUp));
		}

#if !SILVERLIGHT
        [Test]
        public void CanRunProtectedFixtureSetUp()
        {
            TestAssert.IsRunnable(typeof(ProtectedFixtureSetUp), ResultState.Success);
        }
#endif

		[Test] 
		public void CanRunStaticFixtureSetUp()
		{
            TestAssert.IsRunnable(typeof(StaticFixtureSetUp), ResultState.Success);
		}

		[Test]
		public void CannotRunFixtureSetupWithReturnValue()
		{
            TestAssert.IsNotRunnable(typeof(FixtureSetUpWithReturnValue));
		}

		[Test]
		public void CannotRunFixtureSetupWithParameters()
		{
            TestAssert.IsNotRunnable(typeof(FixtureSetUpWithParameters));
		}
		#endregion

		#region TestFixtureTearDown Signature

		[Test] 
		public void CannotRunPrivateFixtureTearDown()
		{
            TestAssert.IsNotRunnable(typeof(PrivateFixtureTearDown));
		}

#if !SILVERLIGHT
        [Test]
        public void CanRunProtectedFixtureTearDown()
        {
            TestAssert.IsRunnable(typeof(ProtectedFixtureTearDown), ResultState.Success);
        }
#endif

		[Test] 
		public void CanRunStaticFixtureTearDown()
		{
            TestAssert.IsRunnable(typeof(StaticFixtureTearDown), ResultState.Success);
		}

//		[TestFixture]
//			[Category("fixture category")]
//			[Category("second")]
//			private class HasCategories 
//		{
//			[Test] public void OneTest()
//			{}
//		}
//
//		[Test]
//		public void LoadCategories() 
//		{
//			TestSuite fixture = LoadFixture("NUnit.Core.Tests.TestFixtureBuilderTests+HasCategories");
//			Assert.IsNotNull(fixture);
//			Assert.AreEqual(2, fixture.Categories.Count);
//		}

		[Test]
		public void CannotRunFixtureTearDownWithReturnValue()
		{
            TestAssert.IsNotRunnable(typeof(FixtureTearDownWithReturnValue));
		}

		[Test]
		public void CannotRunFixtureTearDownWithParameters()
		{
            TestAssert.IsNotRunnable(typeof(FixtureTearDownWithParameters));
		}
		#endregion
	}
}
