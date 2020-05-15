// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading.Tasks;
using TCLite.Framework.Constraints;


namespace TCLite.Framework.AssertionTests
{
    public class AssumeThatTests : AssertionTestBase
    {
        [Test]
        public void AssumptionPasses_Boolean()
        {
            Assume.That(2 + 2 == 4);
        }

        [Test]
        public void AssumptionPasses_BooleanWithMessage()
        {
            Assume.That(2 + 2 == 4, "Not Equal");
        }

        [Test]
        public void AssumptionPasses_BooleanWithMessageAndArgs()
        {
            Assume.That(2 + 2 == 4, "Not Equal to {0}", 4);
        }

        [Test]
        public void AssumptionPasses_ActualAndConstraint()
        {
            Assume.That(2 + 2, Is.EqualTo(4));
        }

        [Test]
        public void AssumptionPasses_ActualAndConstraintWithMessage()
        {
            Assume.That(2 + 2, Is.EqualTo(4), "Should be 4");
        }

        [Test]
        public void AssumptionPasses_ActualAndConstraintWithMessageAndArgs()
        {
            Assume.That(2 + 2, Is.EqualTo(4), "Should be {0}", 4);
        }

        [Test]
        public void AssumptionPasses_ReferenceAndConstraint()
        {
            bool value = true;
            Assume.That(ref value, Is.True);
        }

        [Test]
        public void AssumptionPasses_ReferenceAndConstraintWithMessage()
        {
            bool value = true;
            Assume.That(ref value, Is.True, "Message");
        }

        [Test]
        public void AssumptionPasses_ReferenceAndConstraintWithMessageAndArgs()
        {
            bool value = true;
            Assume.That(ref value, Is.True, "Message", 42);
        }

        [Test]
        public void AssumptionPasses_DelegateAndConstraint()
        {
            Assume.That(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4));
        }

        [Test]
        public void AssumptionPasses_DelegateAndConstraintWithMessage()
        {
            Assume.That(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4), "Message");
        }

        [Test]
        public void AssumptionPasses_DelegateAndConstraintWithMessageAndArgs()
        {
            Assume.That(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4), "Should be {0}", 4);
        }

        private int ReturnsFour()
        {
            return 4;
        }

        [Test]
        public void FailureThrowsInconclusiveException_Boolean()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2 == 5));
        }

        [Test]
        public void FailureThrowsInconclusiveException_BooleanWithMessage()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2 == 5, "message"),
                "  message" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_BooleanWithMessageAndArgs()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2 == 5, "got {0}", 5),
                "  got 5" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_ActualAndConstraint()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2, Is.EqualTo(5)));
        }

        [Test]
        public void FailureThrowsInconclusiveException_ActualAndConstraintWithMessage()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2, Is.EqualTo(5), "Error"),
                "  Error" + NL +
                "  Expected: 5" + NL +
                "  But was:  4" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_ActualAndConstraintWithMessageAndArgs()
        {
            ThrowsInconclusiveException(() => Assume.That(2 + 2, Is.EqualTo(5), "Should be {0}", 5),
                "  Should be 5" + NL +
                "  Expected: 5" + NL +
                "  But was:  4" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_ReferenceAndConstraint()
        {
            bool value = false;
            ThrowsInconclusiveException(() => Assume.That(ref value, Is.True));
        }

        [Test]
        public void FailureThrowsInconclusiveException_ReferenceAndConstraintWithMessage()
        {
            bool value = false;
            ThrowsInconclusiveException(() => Assume.That(ref value, Is.True, "message"),
                "  message" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_ReferenceAndConstraintWithMessageAndArgs()
        {
            bool value = false;
            ThrowsInconclusiveException(() => Assume.That(ref value, Is.True, "message is {0}", 42),
                "  message is 42" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_DelegateAndConstraint()
        {
            ThrowsInconclusiveException(() => Assume.That(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4)));
        }

        [Test]
        public void FailureThrowsInconclusiveException_DelegateAndConstraintWithMessage()
        {
            ThrowsInconclusiveException(() => Assume.That(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), "Error"),
                "  Error" + NL +
                "  Expected: 4" + NL +
                "  But was:  5" + NL);
        }

        [Test]
        public void FailureThrowsInconclusiveException_DelegateAndConstraintWithMessageAndArgs()
        {
            ThrowsInconclusiveException(() => Assume.That(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), "Should be {0}", 4),
                "  Should be 4" + NL +
                "  Expected: 4" + NL +
                "  But was:  5" + NL);
        }

        private int ReturnsFive()
        {
            return 5;
        }

#if NET_4_5
        [Test]
        public void AssumeThatSuccess()
        {
            Assume.That(async () => await One(), Is.EqualTo(1));
        }

        [Test]
        public void AssumeThatFailure()
        {
            var exception = Assert.Throws<InconclusiveException>(() =>
                Assume.That(async () => await One(), Is.EqualTo(2)));
        }

        [Test]
        public void AssumeThatError()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assume.That(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
        }

        private static Task<int> One()
        {
            return Task.Run(() => 1);
        }

        private static async Task<int> ThrowExceptionGenericTask()
        {
            await One();
            throw new InvalidOperationException();
        }
#endif
    }
}
