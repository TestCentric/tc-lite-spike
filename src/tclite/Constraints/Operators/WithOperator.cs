// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that simply wraps the
    /// constraint provided as an argument, without any
    /// further functionality, but which modifes the
    /// order of evaluation because of its precedence.
    /// </summary>
    public class WithOperator : PrefixOperator
    {
        /// <summary>
        /// Constructor for the WithOperator
        /// </summary>
        public WithOperator()
        {
            this.left_precedence = 1;
            this.right_precedence = 4;
        }

        /// <summary>
        /// Returns a constraint that wraps its argument
        /// </summary>
        public override Constraint ApplyPrefix(Constraint constraint)
        {
            return constraint;
        }
    }
}