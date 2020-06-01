// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// RangeConstraint tests whether two values are within a 
    /// specified range.
    /// </summary>
    public class RangeConstraint<T> : ComparisonConstraint where T : IComparable<T>
    {
        private readonly T from;
        private readonly T to;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RangeConstraint"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public RangeConstraint(T from, T to)
            : base(from, to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            if (from == null || to == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference", "actual");

            return comparer.Compare(from, actual) <= 0 &&
                   comparer.Compare(to, actual) >= 0;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {

            writer.Write("in range ({0},{1})", from, to);
        }
    }
}
