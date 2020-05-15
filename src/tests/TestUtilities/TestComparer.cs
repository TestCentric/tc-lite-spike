// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.TestUtilities
{
    internal class TestComparer : IComparer
    {
        public bool Called = false;

        #region IComparer Members
        public int Compare(object x, object y)
        {
            Called = true;

            if (x == null && y == null)
                return 0;

            if (x == null || y == null)
                return -1;

            if (x.Equals(y))
                return 0;

            return -1;
        }
        #endregion
    }
}
