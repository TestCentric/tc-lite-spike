// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Predicate constraint wraps a Predicate in a constraint,
    /// returning success if the predicate is true.
    /// </summary>
    public class PredicateConstraint<T> : Constraint
    {
        readonly Predicate<T> predicate;

        /// <summary>
        /// Construct a PredicateConstraint from a predicate
        /// </summary>
        public PredicateConstraint(Predicate<T> predicate)
        {
            this.predicate = predicate;
        }

        /// <summary>
        /// Determines whether the predicate succeeds when applied
        /// to the actual value.
        /// </summary>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            if (!(actual is T))
                throw new ArgumentException("The actual value is not of type " + typeof(T).Name, "actual");

            return predicate((T)actual);
        }

        /// <summary>
        /// Writes the description to a MessageWriter
        /// </summary>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
#if NETCF_2_0
            writer.Write("value matching predicate");
#else
            writer.WritePredicate("value matching");
            writer.Write(predicate.Method.Name.StartsWith("<")
                ? "lambda expression"
                : predicate.Method.Name);
#endif
        }
    }
}
