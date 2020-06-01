﻿using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.TestData.DatapointFixture;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
    public class DatapointTests
    {
        private void RunTestOnFixture(Type fixtureType)
        {
            TestResult result = TestBuilder.RunTestFixture(fixtureType);
            ResultSummary summary = new ResultSummary(result);
            Assert.That(summary.Passed, Is.EqualTo(2));
            Assert.That(summary.Inconclusive, Is.EqualTo(3));
            Assert.That(result.ResultState, Is.EqualTo(ResultState.Success));
        }

        [Test]
        public void WorksOnField()
        {
            RunTestOnFixture(typeof(SquareRootTest_Field_Double));
        }

        [Test]
        public void WorksOnArray()
        {
            RunTestOnFixture(typeof(SquareRootTest_Field_ArrayOfDouble));
        }

        [Test]
        public void WorksOnPropertyReturningArray()
        {
            RunTestOnFixture(typeof(SquareRootTest_Property_ArrayOfDouble));
        }

        [Test]
        public void WorksOnMethodReturningArray()
        {
            RunTestOnFixture(typeof(SquareRootTest_Method_ArrayOfDouble));
        }

        [Test]
        public void WorksOnIEnumerableOfT()
        {
            RunTestOnFixture(typeof(SquareRootTest_Field_IEnumerableOfDouble));
        }

        [Test]
        public void WorksOnPropertyReturningIEnumerableOfT()
        {
            RunTestOnFixture(typeof(SquareRootTest_Property_IEnumerableOfDouble));
        }

        [Test]
        public void WorksOnMethodReturningIEnumerableOfT()
        {
            RunTestOnFixture(typeof(SquareRootTest_Method_IEnumerableOfDouble));
        }

        [Test]
        public void WorksOnEnumeratorReturningIEnumerableOfT()
        {
            RunTestOnFixture(typeof(SquareRootTest_Iterator_IEnumerableOfDouble));
        }
    }
}
