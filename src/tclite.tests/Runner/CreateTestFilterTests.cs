// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework;
using TCLite.Framework.Internal.Filters;

namespace TCLite.Runner
{
    public class CreateTestFilterTests
    {
        [TestCase("cat=Urgent", "<cat>Urgent</cat>")]
        [TestCase("cat==Urgent", "<cat>Urgent</cat>")]
        [TestCase("cat=='Urgent,+-!'", "<cat>Urgent,+-!</cat>")]
        [TestCase("cat!=Urgent", "<not><cat>Urgent</cat></not>")]
        [TestCase("cat!='Urgent,+-!'", "<not><cat>Urgent,+-!</cat></not>")]
        [TestCase("cat =~ Urgent", "<cat re=\"1\">Urgent</cat>")]
        [TestCase("cat =~ 'Urgent,+-!'", "<cat re=\"1\">Urgent,+-!</cat>")]
        [TestCase("cat =~ 'Urgent,\\+-!'", "<cat re=\"1\">Urgent,+-!</cat>")]
        [TestCase("cat =~ 'Urgent,\\\\+-!'", "<cat re=\"1\">Urgent,\\+-!</cat>")]
        [TestCase("cat !~ Urgent", "<not><cat re=\"1\">Urgent</cat></not>")]
        [TestCase("cat !~ 'Urgent,+-!'", "<not><cat re=\"1\">Urgent,+-!</cat></not>")]
        [TestCase("cat !~ 'Urgent,\\+-!'", "<not><cat re=\"1\">Urgent,+-!</cat></not>")]
        [TestCase("cat !~ 'Urgent,\\\\+-!'", "<not><cat re=\"1\">Urgent,\\+-!</cat></not>")]
        [TestCase("cat = Urgent || cat = High", "<or><cat>Urgent</cat><cat>High</cat></or>")]
        [TestCase("Priority == High", "<prop name=\"Priority\">High</prop>")]
        [TestCase("Priority != Urgent", "<not><prop name=\"Priority\">Urgent</prop></not>")]
        [TestCase("Author =~ Jones", "<prop re=\"1\" name=\"Author\">Jones</prop>")]
        [TestCase("Author !~ Jones", "<not><prop re=\"1\" name=\"Author\">Jones</prop></not>")]
        [TestCase("method=TestMethod", "<method>TestMethod</method>")]
        [TestCase("method=Test1||method=Test2||method=Test3", "<or><method>Test1</method><method>Test2</method><method>Test3</method></or>")]
        [TestCase("test='My.Test.Fixture.Method(42)'", "<test>My.Test.Fixture.Method(42)</test>")]
        [TestCase("test='My.Test.Fixture.Method(\"xyz\")'", "<test>My.Test.Fixture.Method(\"xyz\")</test>")]
        [TestCase("test='My.Test.Fixture.Method(\"abc\\'s\")'", "<test>My.Test.Fixture.Method(\"abc's\")</test>")]
        [TestCase("test='My.Test.Fixture.Method(\"x&y&z\")'", "<test>My.Test.Fixture.Method(\"x&amp;y&amp;z\")</test>")]
        [TestCase("test='My.Test.Fixture.Method(\"<xyz>\")'", "<test>My.Test.Fixture.Method(\"&lt;xyz&gt;\")</test>")]
        [TestCase("cat==Urgent && test=='My.Tests'", "<and><cat>Urgent</cat><test>My.Tests</test></and>")]
        [TestCase("cat==Urgent and test=='My.Tests'", "<and><cat>Urgent</cat><test>My.Tests</test></and>")]
        [TestCase("cat==Urgent || test=='My.Tests'", "<or><cat>Urgent</cat><test>My.Tests</test></or>")]
        [TestCase("cat==Urgent or test=='My.Tests'", "<or><cat>Urgent</cat><test>My.Tests</test></or>")]
        [TestCase("cat==Urgent || test=='My.Tests' && cat == high", "<or><cat>Urgent</cat><and><test>My.Tests</test><cat>high</cat></and></or>")]
        [TestCase("cat==Urgent && test=='My.Tests' || cat == high", "<or><and><cat>Urgent</cat><test>My.Tests</test></and><cat>high</cat></or>")]
        [TestCase("cat==Urgent && (test=='My.Tests' || cat == high)", "<and><cat>Urgent</cat><or><test>My.Tests</test><cat>high</cat></or></and>")]
        [TestCase("cat==Urgent && !(test=='My.Tests' || cat == high)", "<and><cat>Urgent</cat><not><or><test>My.Tests</test><cat>high</cat></or></not></and>")]
        public void FilterCreation(string tslText, string expectedXml)
        {
            var filter = new TestRunner().CreateTestFilter(tslText);
            var xmlText = filter.ToXml(true).OuterXml;
            Assert.That(xmlText, Is.EqualTo(expectedXml));
        }
    }
}