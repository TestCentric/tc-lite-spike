// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework
{
    /// <summary>
    /// RangeAttribute is used to supply a range of values to an
    /// individual parameter of a parameterized test.
    /// </summary>
    public class RangeAttribute : ValuesAttribute
    {
        /// <summary>
        /// Construct a range of ints using default step of 1
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public RangeAttribute(int from, int to) : this(from, to, 1) { }

        /// <summary>
        /// Construct a range of ints specifying the step size 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(int from, int to, int step)
        {
            int count = (to - from) / step + 1;
            this.data = new object[count];
            int index = 0;
            for (int val = from; index < count; val += step)
                this.data[index++] = val;
        }

        /// <summary>
        /// Construct a range of longs
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(long from, long to, long step)
        {
            long count = (to - from) / step + 1;
            this.data = new object[count];
            int index = 0;
            for (long val = from; index < count; val += step)
                this.data[index++] = val;
        }

        /// <summary>
        /// Construct a range of doubles
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(double from, double to, double step)
        {
            double tol = step / 1000;
            int count = (int)((to - from) / step + tol + 1);
            this.data = new object[count];
            int index = 0;
            for (double val = from; index < count; val += step)
                this.data[index++] = val;
        }

        /// <summary>
        /// Construct a range of floats
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="step"></param>
        public RangeAttribute(float from, float to, float step)
        {
            float tol = step / 1000;
            int count = (int)((to - from) / step + tol + 1);
            this.data = new object[count];
            int index = 0;
            for (float val = from; index < count; val += step)
                this.data[index++] = val;
        }
    }
}
