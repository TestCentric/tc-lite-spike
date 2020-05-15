// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
{
    public class TestDummy : Test
    {
        public TestDummy() : base("TestDummy") { }

        #region Overrides

        public string TestKind
        {
            get { return "dummy-test"; }
        }

        public override Internal.WorkItems.WorkItem CreateWorkItem(ITestFilter childFilter)
        {
            throw new NotImplementedException();
        }

        public override bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public override System.Collections.Generic.IList<ITest> Tests
        {
            get
            {
                return new ITest[0];
            }
        }

        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            throw new NotImplementedException();
        }

        public override TestResult MakeTestResult()
        {
            throw new NotImplementedException();
        }

        public override string XmlElementName
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
