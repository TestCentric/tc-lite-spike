// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

#if !NETCF
using System.Text.RegularExpressions;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// RegexConstraint can test whether a string matches
    /// the pattern provided.
    /// </summary>
    public class RegexConstraint : StringConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RegexConstraint"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public RegexConstraint(string pattern) : base(pattern) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches(string actual)
        {
            return Regex.IsMatch(
                    actual,
                    this.expected,
                    this.caseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("String matching");
            writer.WriteExpectedValue(this.expected);
            if (this.caseInsensitive)
                writer.WriteModifier("ignoring case");
        }
    }
}
#endif
