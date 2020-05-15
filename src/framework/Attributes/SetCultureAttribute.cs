// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
	/// <summary>
	/// Summary description for SetCultureAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=false, Inherited=true)]
	public class SetCultureAttribute : PropertyAttribute, IApplyToContext
	{
        private string _culture;

		/// <summary>
		/// Construct given the name of a culture
		/// </summary>
		/// <param name="culture"></param>
		public SetCultureAttribute( string culture ) : base( PropertyNames.SetCulture, culture ) 
        {
            _culture = culture;
        }

        #region IApplyToContext Members

        void IApplyToContext.ApplyToContext(TestExecutionContext context)
        {
            context.CurrentCulture = new System.Globalization.CultureInfo(_culture);
        }

        #endregion
    }
}
