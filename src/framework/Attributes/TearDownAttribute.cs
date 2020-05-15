// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
	using System;

	/// <summary>
	/// Attribute used to identify a method that is called 
	/// immediately after each test is run. The method is 
	/// guaranteed to be called, even if an exception is thrown.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class TearDownAttribute : NUnitAttribute
	{}
}
