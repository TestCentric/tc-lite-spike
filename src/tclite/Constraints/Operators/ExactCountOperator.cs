// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that succeeds if the specified 
    /// count of members of a collection match a base constraint.
    /// </summary>
    public class ExactCountOperator : CollectionOperator
    {
        private int expectedCount;

        /// <summary>
        /// Construct an ExactCountOperator for a specified count
        /// </summary>
        /// <param name="expectedCount">The expected count</param>
        public ExactCountOperator(int expectedCount)
        {
            this.expectedCount = expectedCount;
        }

        /// <summary>
        /// Returns a constraint that will apply the argument
        /// to the members of a collection, succeeding if
        /// none of them succeed.
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new ExactCountConstraint(expectedCount, constraint);
        }
    }
}

