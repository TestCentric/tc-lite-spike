// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyDirectoryConstraint is used to test that a directory is empty
    /// </summary>
    public class EmptyDirectoryConstraint : Constraint
    {
        private int files = 0;
        private int subdirs = 0;

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;

            DirectoryInfo dirInfo = actual as DirectoryInfo;
            if (dirInfo == null)
                throw new ArgumentException("The actual value must be a DirectoryInfo", "actual");

#if SL_4_0 || SL_5_0
            foreach (FileInfo file in dirInfo.EnumerateFiles())
                files++;
            foreach (DirectoryInfo dir in dirInfo.EnumerateDirectories())
                subdirs++;
#else
            files = dirInfo.GetFiles().Length;
            subdirs = dirInfo.GetDirectories().Length;
#endif

            return files == 0 && subdirs == 0;
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write( "An empty directory" );
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            DirectoryInfo dir = actual as DirectoryInfo;
            if (dir == null)
                base.WriteActualValueTo(writer);
            else
            {
                writer.WriteActualValue(dir);
                writer.Write(" with {0} files and {1} directories", files, subdirs);
            }
        }
    }
}