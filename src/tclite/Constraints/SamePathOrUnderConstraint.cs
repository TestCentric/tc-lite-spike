// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************
#if NYI
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// SamePathOrUnderConstraint tests that one path is under another
    /// </summary>
    public class SamePathOrUnderConstraint : PathConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SamePathOrUnderConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected path</param>
        public SamePathOrUnderConstraint(string expected) : base(expected) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="expectedPath">The expected path</param>
        /// <param name="actualPath">The actual path</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool IsMatch(string expectedPath, string actualPath)
        {
            string path1 = Canonicalize(expectedPath);
            string path2 = Canonicalize(actualPath);
            return StringUtil.StringsEqual(path1, path2, caseInsensitive) || IsSubPath(path1, path2, caseInsensitive);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("Path under or matching");
            writer.WriteExpectedValue(expectedPath);
        }
    }
}
#endif
