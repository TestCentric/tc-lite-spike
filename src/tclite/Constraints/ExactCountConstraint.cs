// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// ExactCountConstraint applies another constraint to each
    /// item in a collection, succeeding only if a specified
    /// number of items succeed.
    /// </summary>
    public class ExactCountConstraint : PrefixConstraint
    {
        private int expectedCount;

        /// <summary>
        /// Construct an ExactCountConstraint on top of an existing constraint
        /// </summary>
        /// <param name="expectedCount"></param>
        /// <param name="itemConstraint"></param>
        public ExactCountConstraint(int expectedCount, Constraint itemConstraint)
            : base(itemConstraint)
        {
            this.DisplayName = "one";
            this.expectedCount = expectedCount;
        }

        /// <summary>
        /// Apply the item constraint to each item in the collection,
        /// succeeding only if the expected number of items pass.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            if (!(actual is IEnumerable))
                throw new ArgumentException("The actual value must be an IEnumerable", "actual");

            int count = 0;
            foreach (object item in (IEnumerable)actual)
                if (baseConstraint.Matches(item))
                    count++;

            return count == expectedCount;
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            switch(expectedCount)
            {
                case 0:
                    writer.WritePredicate("no item");
                    break;
                case 1:
                    writer.WritePredicate("exactly one item");
                    break;
                default:
                    writer.WritePredicate("exactly " + expectedCount.ToString() + " items");
                    break;
            }

            baseConstraint.WriteDescriptionTo(writer);
        }
    }
}

