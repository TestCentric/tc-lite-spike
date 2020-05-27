// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
	using System;
    using TCLite.Framework.Api;
    using TCLite.Framework.Internal;

	/// <summary>
	/// Adding this attribute to a method within a <seealso cref="TestFixtureAttribute"/> 
	/// class makes the method callable from the NUnit test runner. There is a property 
	/// called Description which is optional which you can provide a more detailed test
	/// description. This class cannot be inherited.
	/// </summary>
	/// 
	/// <example>
	/// [TestFixture]
	/// public class Fixture
	/// {
	///   [Test]
	///   public void MethodToTest()
	///   {}
	///   
	///   [Test(Description = "more detailed description")]
	///   publc void TestDescriptionMethod()
	///   {}
	/// }
	/// </example>
	/// 
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class TestAttribute : TCLiteAttribute, IApplyToTest
	{
		private string description;

		/// <summary>
		/// Descriptive text for this test
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a description, if not already set.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (!test.Properties.ContainsKey(PropertyNames.Description) && description != null)
                test.Properties.Set(PropertyNames.Description, description);
        }

        #endregion
    }
}
