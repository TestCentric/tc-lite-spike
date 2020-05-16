// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.TestUtilities;

namespace TCLite.Framework.AssertionTests
{
	public class AssertThrowsTests : AssertionTestBase
	{
        [Test]
		public void CorrectExceptionThrown()
		{
            Assert.Throws(typeof(ArgumentException), new TestDelegate(TestDelegates.ThrowsArgumentException));

            Assert.Throws(typeof(ArgumentException), TestDelegates.ThrowsArgumentException);

            Assert.Throws(typeof(ArgumentException),
                delegate { throw new ArgumentException(); });

            Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException(); });
            Assert.Throws<ArgumentException>(TestDelegates.ThrowsArgumentException);

            // Without cast, delegate is ambiguous before C# 3.0.
            Assert.That((TestDelegate)delegate { throw new ArgumentException(); },
                    Throws.Exception.TypeOf<ArgumentException>() );
            //Assert.Throws( Is.TypeOf(typeof(ArgumentException)),
            //        delegate { throw new ArgumentException(); } );
        }

        [Test]
		public void CorrectExceptionIsReturnedToMethod()
		{
			ArgumentException ex = Assert.Throws(typeof(ArgumentException),
                new TestDelegate(TestDelegates.ThrowsArgumentException)) as ArgumentException;

            Assert.NotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

            ex = Assert.Throws<ArgumentException>(
                delegate { throw new ArgumentException("myMessage", "myParam"); }) as ArgumentException;

            Assert.NotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

			ex = Assert.Throws(typeof(ArgumentException), 
                delegate { throw new ArgumentException("myMessage", "myParam"); } ) as ArgumentException;

            Assert.NotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif

            ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsArgumentException) as ArgumentException;

            Assert.NotNull(ex, "No ArgumentException thrown");
            Assert.That(ex.Message, Is.StringStarting("myMessage"));
#if !NETCF && !SILVERLIGHT
            Assert.That(ex.ParamName, Is.EqualTo("myParam"));
#endif
		}

		[Test]
		public void NoExceptionThrown()
		{
            try
            {
                ArgumentException ex = Assert.Throws<ArgumentException>(TestDelegates.ThrowsNothing);
            }
            catch(AssertionException ex)
            {
                Assert.That(ex.Message, Is.EqualTo(
				    "  Expected: <System.ArgumentException>" + NL +
				    "  But was:  null" + NL));
            }
		}

        [Test]
        public void UnrelatedExceptionThrown()
        {
            try
            {
                Assert.Throws<ArgumentException>(TestDelegates.ThrowsCustomException);
            }
            catch(AssertionException ex)
            {
                Assert.That(ex.Message, Is.StringStarting(
                    "  Expected: <System.ArgumentException>" + NL +
                    "  But was:  <TCLite.TestUtilities.TestDelegates+CustomException> (Exception of type 'TCLite.TestUtilities.TestDelegates+CustomException' was thrown.)" + NL));
                Assert.That(ex.Message, Contains.Substring("  at "));
            }
        }

        [Test]
        public void BaseExceptionThrown()
        {
            try
            {
                Assert.Throws<ArgumentException>(TestDelegates.ThrowsSystemException);
            }
            catch(AssertionException ex)
            {
                Assert.That(ex.Message, Is.StringStarting(
                    "  Expected: <System.ArgumentException>" + NL +
                    "  But was:  <System.Exception> (my message)" + NL));
                Assert.That(ex.Message, Contains.Substring("  at "));
            }
        }

        [Test]
        public void DerivedExceptionThrown()
        {
            try
            {
                Assert.Throws<Exception>(TestDelegates.ThrowsArgumentException);
            }
            catch(AssertionException ex)
            {
                Assert.That(ex.Message, Is.StringStarting(
                    "  Expected: <System.Exception>" + NL +
                    "  But was:  <System.ArgumentException> (myMessage (Parameter 'myParam'))" + NL));
                Assert.That(ex.Message, Contains.Substring("  at "));
            }
        }

        [Test]
        public void DoesNotThrowSucceeds()
        {
            Assert.DoesNotThrow(new TestDelegate(TestDelegates.ThrowsNothing));
        }

        [Test]
        public void DoesNotThrowFails()
        {
            try
            {
                Assert.DoesNotThrow(new TestDelegate(TestDelegates.ThrowsArgumentException));
            }
            catch(AssertionException ex)
            {
                Assert.That(ex.Message, Is.StringStarting(
                    "  Expected: No Exception to be thrown" + NL +
                    "  But was:  <System.ArgumentException> (myMessage (Parameter 'myParam'))" + NL));
                Assert.That(ex.Message, Contains.Substring("  at "));
            }
        }
    }
}
