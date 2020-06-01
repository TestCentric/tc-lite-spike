// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.TestUtilities;
using TCLite.TestData.SetUpData;

namespace TCLite.Framework.Internal
{
	[TestFixture]
	public class SetUpTest
	{	
		[Test]
		public void SetUpAndTearDownCounter()
		{
			SetUpAndTearDownCounterFixture fixture = new SetUpAndTearDownCounterFixture();
			TestBuilder.RunTestFixture( fixture );

			Assert.AreEqual(3, fixture.setUpCounter);
			Assert.AreEqual(3, fixture.tearDownCounter);
		}

		
		[Test]
		public void MakeSureSetUpAndTearDownAreCalled()
		{
			SetUpAndTearDownFixture fixture = new SetUpAndTearDownFixture();
			TestBuilder.RunTestFixture( fixture );

			Assert.True(fixture.wasSetUpCalled);
			Assert.True(fixture.wasTearDownCalled);
		}

		[Test]
		public void CheckInheritedSetUpAndTearDownAreCalled()
		{
			InheritSetUpAndTearDown fixture = new InheritSetUpAndTearDown();
			TestBuilder.RunTestFixture( fixture );

			Assert.True(fixture.wasSetUpCalled);
			Assert.True(fixture.wasTearDownCalled);
		}

		[Test]
		public void CheckOverriddenSetUpAndTearDownAreNotCalled()
		{
			DefineInheritSetUpAndTearDown fixture = new DefineInheritSetUpAndTearDown();
			TestBuilder.RunTestFixture( fixture );

			Assert.False(fixture.wasSetUpCalled);
			Assert.False(fixture.wasTearDownCalled);
			Assert.True(fixture.derivedSetUpCalled);
			Assert.True(fixture.derivedTearDownCalled);
		}

        [Test]
        public void MultipleSetUpAndTearDownMethodsAreCalled()
        {
            MultipleSetUpTearDownFixture fixture = new MultipleSetUpTearDownFixture();
            TestBuilder.RunTestFixture(fixture);

            Assert.True(fixture.wasSetUp1Called, "SetUp1");
            Assert.True(fixture.wasSetUp2Called, "SetUp2");
            Assert.True(fixture.wasSetUp3Called, "SetUp3");
            Assert.True(fixture.wasTearDown1Called, "TearDown1");
            Assert.True(fixture.wasTearDown2Called, "TearDown2");
        }

        [Test]
        public void BaseSetUpIsCalledFirstTearDownLast()
        {
            DerivedClassWithSeparateSetUp fixture = new DerivedClassWithSeparateSetUp();
            TestBuilder.RunTestFixture(fixture);

            Assert.True(fixture.wasSetUpCalled, "Base SetUp Called");
            Assert.True(fixture.wasTearDownCalled, "Base TearDown Called");
            Assert.True(fixture.wasDerivedSetUpCalled, "Derived SetUp Called");
            Assert.True(fixture.wasDerivedTearDownCalled, "Derived TearDown Called");
            Assert.True(fixture.wasBaseSetUpCalledFirst, "SetUp Order");
            Assert.True(fixture.wasBaseTearDownCalledLast, "TearDown Order");
        }

        [Test]
        public void SetupRecordsOriginalExceptionThownByTestCase()
        {
            Exception e = new Exception("Test message for exception thrown from setup");
            SetupAndTearDownExceptionFixture fixture = new SetupAndTearDownExceptionFixture();
            fixture.setupException = e;
            TestResult suiteResult = TestBuilder.RunTestFixture(fixture);
            Assert.True(suiteResult.HasChildren, "Fixture test should have child result.");
            TestResult result = (TestResult)suiteResult.Children[0];
            Assert.AreEqual(result.ResultState, ResultState.Error, "Test should be in error state");
            string expected = string.Format("{0} : {1}", e.GetType().FullName, e.Message);
            Assert.AreEqual(expected, result.Message);
        }

        [Test]
        public void TearDownRecordsOriginalExceptionThownByTestCase()
        {
            Exception e = new Exception("Test message for exception thrown from tear down");
            SetupAndTearDownExceptionFixture fixture = new SetupAndTearDownExceptionFixture();
            fixture.tearDownException = e;
            TestResult suiteResult = TestBuilder.RunTestFixture(fixture);
            Assert.That(suiteResult.HasChildren, "Fixture test should have child result.");
            TestResult result = (TestResult)suiteResult.Children[0];
            Assert.AreEqual(result.ResultState, ResultState.Error, "Test should be in error state");
            string expected = string.Format("TearDown : {0} : {1}", e.GetType().FullName, e.Message);
            Assert.AreEqual(expected, result.Message);
        }

        public class SetupCallBase
        {
            protected int setupCount = 0;
            public virtual void Init()
            {
                setupCount++;
            }
            public virtual void AssertCount()
            {
            }
        }

        [TestFixture]
        // Test for bug 441022
        public class SetupCallDerived : SetupCallBase
        {
            [SetUp]
            public override void Init()
            {
                setupCount++;
                base.Init();
            }
            [Test]
            public override void AssertCount()
            {
                Assert.AreEqual(2, setupCount);
            }
        }
    }
}
