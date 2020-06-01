// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
	using System;

	/// <summary>
	/// Attribute used to identify a method that is 
	/// called before any tests in a fixture are run.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class OneTimeSetUpAttribute : TCLiteAttribute
	{
	}
}
