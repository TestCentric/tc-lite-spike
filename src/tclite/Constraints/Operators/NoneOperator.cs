﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that succeeds if none of the 
    /// members of a collection match a base constraint.
    /// </summary>
    public class NoneOperator : CollectionOperator
    {
        /// <summary>
        /// Returns a constraint that will apply the argument
        /// to the members of a collection, succeeding if
        /// none of them succeed.
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return new NoItemConstraint(constraint);
        }
    }
}
