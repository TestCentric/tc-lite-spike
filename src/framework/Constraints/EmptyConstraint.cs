// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
	/// <summary>
	/// EmptyConstraint tests a whether a string or collection is empty,
	/// postponing the decision about which test is applied until the
	/// type of the actual argument is known.
	/// </summary>
	public class EmptyConstraint : Constraint
	{
		private Constraint RealConstraint
		{
			get 
			{
                if (actual is string)
                    return new EmptyStringConstraint();
                else if (actual is System.IO.DirectoryInfo)
                    return new EmptyDirectoryConstraint();
                else
                    return new EmptyCollectionConstraint();
			}
		}
		
		/// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
		public override bool Matches(object actual)
		{
			this.actual = actual;

            if (actual == null)
                throw new ArgumentException("The actual value must be a non-null string, IEnumerable or DirectoryInfo", "actual");

			return this.RealConstraint.Matches( actual );
		}

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
		public override void WriteDescriptionTo(MessageWriter writer)
		{
			this.RealConstraint.WriteDescriptionTo( writer );
		}
	}
}
