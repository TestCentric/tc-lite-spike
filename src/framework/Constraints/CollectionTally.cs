// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// CollectionTally counts (tallies) the number of
    /// occurences of each object in one or more enumerations.
    /// </summary>
    public class CollectionTally
    {
        // Internal list used to track occurences
        private readonly List<object> list = new List<object>();

        private readonly NUnitEqualityComparer comparer;

        /// <summary>
        /// Construct a CollectionTally object from a comparer and a collection
        /// </summary>
        public CollectionTally(NUnitEqualityComparer comparer, IEnumerable c)
        {
            this.comparer = comparer;

            foreach (object o in c)
                list.Add(o);
        }

        /// <summary>
        /// The number of objects remaining in the tally
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }

        private bool ItemsEqual(object expected, object actual)
        {
            Tolerance tolerance = Tolerance.Zero;
            return comparer.AreEqual(expected, actual, ref tolerance);
        }

        /// <summary>
        /// Try to remove an object from the tally
        /// </summary>
        /// <param name="o">The object to remove</param>
        /// <returns>True if successful, false if the object was not found</returns>
        public bool TryRemove(object o)
        {
            for (int index = 0; index < list.Count; index++)
                if (ItemsEqual(list[index], o))
                {
                    list.RemoveAt(index);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Try to remove a set of objects from the tally
        /// </summary>
        /// <param name="c">The objects to remove</param>
        /// <returns>True if successful, false if any object was not found</returns>
        public bool TryRemove(IEnumerable c)
        {
            foreach (object o in c)
                if (!TryRemove(o))
                    return false;

            return true;
        }
    }
}
