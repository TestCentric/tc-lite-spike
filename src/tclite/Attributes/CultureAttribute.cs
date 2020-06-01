// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// CultureAttribute is used to mark a test fixture or an
    /// individual method as applying to a particular Culture only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public class CultureAttribute : IncludeExcludeAttribute, IApplyToTest
    {
        private CultureDetector cultureDetector = new CultureDetector();
        private CultureInfo currentCulture = CultureInfo.CurrentCulture;

        /// <summary>
        /// Constructor with no cultures specified, for use
        /// with named property syntax.
        /// </summary>
        public CultureAttribute() { }

        /// <summary>
        /// Constructor taking one or more cultures
        /// </summary>
        /// <param name="cultures">Comma-deliminted list of cultures</param>
        public CultureAttribute(string cultures) : base(cultures) { }

        #region IApplyToTest members

        /// <summary>
        /// Causes a test to be skipped if this CultureAttribute is not satisfied.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (test.RunState != RunState.NotRunnable && !IsCultureSupported())
            {
                test.RunState = RunState.Skipped;
                test.Properties.Set(PropertyNames.SkipReason, Reason);
            }
        }

        #endregion

        /// <summary>
        /// Tests to determine if the current culture is supported
        /// based on the properties of this attribute.
        /// </summary>
        /// <returns>True, if the current culture is supported</returns>
        private bool IsCultureSupported()
        {
            if (Include != null && !cultureDetector.IsCultureSupported(Include))
            {
                Reason = string.Format("Only supported under culture {0}", Include);
                return false;
            }

            if (Exclude != null && cultureDetector.IsCultureSupported(Exclude))
            {
                Reason = string.Format("Not supported under culture {0}", Exclude);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Test to determine if the a particular culture or comma-
        /// delimited set of cultures is in use.
        /// </summary>
        /// <param name="culture">Name of the culture or comma-separated list of culture names</param>
        /// <returns>True if the culture is in use on the system</returns>
        public bool IsCultureSupported(string culture)
        {
            culture = culture.Trim();

            if (culture.IndexOf(',') >= 0)
            {
                if (IsCultureSupported(culture.Split(new char[] { ',' })))
                    return true;
            }
            else
            {
                if (currentCulture.Name == culture || currentCulture.TwoLetterISOLanguageName == culture)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Test to determine if one of a collection of culturess
        /// is being used currently.
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns></returns>
        public bool IsCultureSupported(string[] cultures)
        {
            foreach (string culture in cultures)
                if (IsCultureSupported(culture))
                    return true;

            return false;
        }
    }
}
