// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal.Commands;
using TCLite.Framework.Internal.WorkItems;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// The Test abstract class represents a test within the framework.
	/// </summary>
	public abstract class Test : ITest, IComparable
    {
        /// <summary>
        /// Static value to seed ids. It's started at 1000 so any
        /// uninitialized ids will stand out.
        /// </summary>
        private static int _nextID = 1000;

        /// <summary>
		/// Constructs a test given its name
		/// </summary>
		/// <param name="name">The name of the test</param>
		protected Test( string name )
		{
            Guard.ArgumentNotNullOrEmpty(name, nameof(name));

            Initialize(name);
		}

		/// <summary>
		/// Constructs a test given the path through the
		/// test hierarchy to its parent and a name.
		/// </summary>
		/// <param name="pathName">The parent tests full name</param>
		/// <param name="name">The name of the test</param>
		protected Test( string pathName, string name ) 
            : this(name)
		{ 
            Guard.ArgumentNotNullOrEmpty(pathName, nameof(pathName));
            Guard.ArgumentNotNullOrEmpty(name, nameof(name));

            Initialize(name);

            FullName = $"{pathName}.{name}";
		}

        /// <summary>
        /// Constructs a test for a specific type.
        /// </summary>
        protected Test(Type type)
        {
            Initialize(type.Name);
            FixtureType = type;
            FullName = type.FullName;
        }

        private void Initialize(string name)
        {
            FullName = Name = name;
            Id = GetNextId();
            RunState = RunState.Runnable;
        }

        private static string GetNextId()
        {
            return IdPrefix + unchecked(_nextID++);
        }

        /// <summary>
        /// Static prefix used for ids in this AppDomain.
        /// Set by FrameworkController.
        /// </summary>
        public static string IdPrefix { get; set; }

		#region ITest Members

        /// <summary>
        /// Gets or sets the id of the test
        /// </summary>
        /// <value></value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the test
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the fully qualified name of the test
        /// </summary>
        /// <value></value>
        public string FullName { get; set; }

                /// <summary>
        /// Gets the name of the class where this test was declared.
        /// Returns null if the test is not associated with a class.
        /// </summary>
        public string ClassName
        {
            get
            {
                return FixtureType.IsGenericType
                    ? FixtureType.GetGenericTypeDefinition().FullName
                    : FixtureType.FullName;
            }
        }

        /// <summary>
        /// Gets the name of the method implementing this test.
        /// Returns null if the test is not implemented as a method.
        /// </summary>
        public virtual string MethodName
        {
            get { return null; }
        }

        /// <summary>
        /// The arguments to use in creating the test or empty array if none required.
        /// </summary>
        public abstract object[] Arguments { get; }

        /// <summary>
        /// Gets a MethodInfo for the method implementing this test.
        /// Returns null if the test is not implemented as a method.
        /// </summary>
        public MethodInfo Method { get; set; } // public setter needed by NUnitTestCaseBuilder

        /// <summary>
        /// Gets the Type of the fixture used in running this test
        /// or null if no fixture type is associated with it.
        /// </summary>
        public Type FixtureType { get; }

        /// <summary>
		/// Whether or not the test should be run
		/// </summary>
        public RunState RunState { get; set; } = RunState.Runnable;

        /// <summary>
        /// Gets the name used for the top-level element in the
        /// XML representation of this test
        /// </summary>
        public abstract string XmlElementName { get; }

        /// <summary>
        /// Gets a string representing the type of test. Used as an attribute
        /// value in the XML representation of a test and has no other
        /// function in the framework.
        /// </summary>
        public virtual string TestType
        {
            get { return GetType().Name; }
        }

        /// <summary>
		/// Gets a count of test cases represented by
		/// or contained under this test.
		/// </summary>
		public virtual int TestCaseCount 
		{ 
			get { return 1; } 
		}

		/// <summary>
		/// A dictionary of properties, used to add information
		/// to tests without requiring the class to change.
		/// </summary>
		public IPropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Returns true if this is a TestSuite
        /// </summary>
        public bool IsSuite
        {
            get { return this is TestSuite; }
        }

        /// <summary>
        /// Gets a bool indicating whether the current test
        /// has any descendant tests.
        /// </summary>
        public bool HasChildren => Tests.Count > 0;

        /// <summary>
        /// Gets the parent as a Test object.
        /// Used by the core to set the parent.
        /// </summary>
        public ITest Parent { get; set; }

        /// <summary>
        /// Gets or Sets the Int value representing the seed for the RandomGenerator
        /// </summary>
        /// <value></value>
        public int Seed { get; set; }

        /// <summary>
        /// Gets this test's child tests
        /// </summary>
        /// <value>A list of child tests</value>
        
        public IList<ITest> Tests { get; } = new List<ITest>();

        /// <summary>
        /// Gets the set up methods.
        /// </summary>
        /// <returns></returns>
        public MethodInfo[] SetUpMethods
        { 
            get { return _setUpMethods ?? (_setUpMethods = Parent?.SetUpMethods); }
            protected set { _setUpMethods = value; }
        }
        private MethodInfo[] _setUpMethods;

        /// <summary>
        /// Gets the tear down methods.
        /// </summary>
        /// <returns></returns>
        public MethodInfo[] TearDownMethods
        {
            get { return _tearDownMethods ?? (_tearDownMethods = Parent?.TearDownMethods); }
            protected set { _tearDownMethods = value; }
        }
        private MethodInfo[] _tearDownMethods;

        #endregion

        #region IXmlNodeBuilder Members

        /// <summary>
        /// Returns the Xml representation of the test
        /// </summary>
        /// <param name="recursive">If true, include child tests recursively</param>
        /// <returns></returns>
        public XmlNode ToXml(bool recursive)
        {
            XmlNode topNode = XmlHelper.CreateTopLevelElement("dummy");

            XmlNode thisNode = AddToXml(topNode, recursive);

            return thisNode;
        }

        /// <summary>
        /// Returns an XmlNode representing the current result after
        /// adding it as a child of the supplied parent node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="recursive">If true, descendant results are included</param>
        /// <returns></returns>
        public abstract XmlNode AddToXml(XmlNode parentNode, bool recursive);

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this test to another test for sorting purposes
        /// </summary>
        /// <param name="obj">The other test</param>
        /// <returns>Value of -1, 0 or +1 depending on whether the current test is less than, equal to or greater than the other test</returns>
        public int CompareTo(object obj)
        {
            Test other = obj as Test;

            if (other == null)
                return -1;

            return FullName.CompareTo(other.FullName);
        }

        #endregion

        #region Other Public Methods

        /// <summary>
        /// Creates a TestResult for this test.
        /// </summary>
        /// <returns>A TestResult suitable for this type of test.</returns>
        public abstract TestResult MakeTestResult();

        /// <summary>
        /// Creates a WorkItem for executing this test.
        /// </summary>
        /// <param name="childFilter">A filter to be used in selecting child tests</param>
        /// <returns>A new WorkItem</returns>
        public abstract WorkItem CreateWorkItem(ITestFilter childFilter);

        /// <summary>
        /// Modify a newly constructed test by applying any of NUnit's common
        /// attributes, based on a supplied ICustomAttributeProvider, which is
        /// usually the reflection element from which the test was constructed,
        /// but may not be in some instances. The attributes retrieved are 
        /// saved for use in subsequent operations.
        /// </summary>
        /// <param name="provider">An object implementing ICustomAttributeProvider</param>
        public void ApplyAttributesToTest(ICustomAttributeProvider provider)
        {
            foreach (IApplyToTest iApply in provider.GetCustomAttributes(typeof(IApplyToTest), true))
                iApply.ApplyToTest(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Add standard attributes and members to a test node.
        /// </summary>
        /// <param name="thisNode"></param>
        /// <param name="recursive"></param>
        protected void PopulateTestNode(XmlNode thisNode, bool recursive)
        {
            thisNode.AddAttribute("id", Id.ToString());
            thisNode.AddAttribute("name", Name);
            thisNode.AddAttribute("fullname", FullName);

            if (Properties.Count > 0)
                Properties.AddToXml(thisNode, recursive);
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// The fixture object for running this test,
        /// if it has been created. Public setter
        /// provided for use by LegacySuiteBuilder.
        /// </summary>
        public object Fixture { get; set; }

        #endregion
    }
}
