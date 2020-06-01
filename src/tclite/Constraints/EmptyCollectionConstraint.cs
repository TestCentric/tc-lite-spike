// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyCollectionConstraint tests whether a collection is empty. 
    /// </summary>
    public class EmptyCollectionConstraint : CollectionConstraint
    {
        /// <summary>
        /// Check that the collection is empty
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected override bool doMatch(IEnumerable collection)
        {
            return IsEmpty(collection);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write("<empty>");
        }
    }
}