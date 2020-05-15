// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;

namespace TCLite.TestData.CategoryAttributeData
{
	[TestFixture, InheritableCategory("MyCategory")]
	public abstract class AbstractBase { }
	
	[TestFixture, Category( "DataBase" )]
	public class FixtureWithCategories : AbstractBase
	{
		[Test, Category("Long")]
		public void Test1() { }

		[Test, Critical]
		public void Test2() { }

        [Test, Category("Top")]
        [TestCaseSource("Test3Data")]
        public void Test3(int x) { }

        [Test, Category("A-B")]
        public void Test4() { }

        internal TestCaseData[] Test3Data = new TestCaseData[] {
            new TestCaseData(5).SetCategory("Bottom")
        };
    }

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
	public class CriticalAttribute : CategoryAttribute { }
	
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
	public class InheritableCategoryAttribute : CategoryAttribute
	{
		public InheritableCategoryAttribute(string name) : base(name) { }
	}
}