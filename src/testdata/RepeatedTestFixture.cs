// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************
#if false
using System;
using TCLite.Framework;

namespace TCLite.TestData.RepeatedTestFixture
{
	[TestFixture]
	public class RepeatingTestsBase
	{
		private int fixtureSetupCount;
		private int fixtureTeardownCount;
		private int setupCount;
		private int teardownCount;
		protected int count;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			fixtureSetupCount++;
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			fixtureTeardownCount++;
		}

		[SetUp]
		public void SetUp()
		{
			setupCount++;
		}

		[TearDown]
		public void TearDown()
		{
			teardownCount++;
		}

		public int FixtureSetupCount
		{
			get { return fixtureSetupCount; }
		}
		public int FixtureTeardownCount
		{
			get { return fixtureTeardownCount; }
		}
		public int SetupCount
		{
			get { return setupCount; }
		}
		public int TeardownCount
		{
			get { return teardownCount; }
		}
		public int Count
		{
			get { return count; }
		}
	}

	public class RepeatSuccessFixture : RepeatingTestsBase
	{
		[Test, Repeat(3)]
		public void RepeatSuccess()
		{
			count++;
			Assert.True (true);
		}
	}

	public class RepeatFailOnFirstFixture : RepeatingTestsBase
	{
		[Test, Repeat(3)]
		public void RepeatFailOnFirst()
		{
			count++;
			Assert.False (true);
		}
	}

	public class RepeatFailOnThirdFixture : RepeatingTestsBase
	{
		[Test, Repeat(3)]
		public void RepeatFailOnThird()
		{
			count++;

			if (count == 3)
				Assert.True (false);
		}
	}

    public class RepeatedTestWithIgnore : RepeatingTestsBase
    {
        [Test, Repeat(3), Ignore("Ignore this test")]
        public void RepeatShouldIgnore()
        {
            Assert.Fail("Ignored test executed");
        }
    }

    public class RepeatedTestWithCategory : RepeatingTestsBase
    {
        [Test, Repeat(3), Category("SAMPLE")]
        public void TestWithCategory()
        {
            count++;
            Assert.True(true);
        }
    }
}
#endif