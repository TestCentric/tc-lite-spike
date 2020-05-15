// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Tests
{
    [TestFixture]
    class StackFilterTest
    {
        private static readonly string NL = Environment.NewLine;

        private static readonly string rawTrace1 =
    @"   at TCLite.Framework.Assert.Fail(String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 56" + NL +
    @"   at TCLite.Framework.Assert.That(String label, Object actual, Matcher expectation, String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 50" + NL +
    @"   at TCLite.Framework.Assert.That(Object actual, Matcher expectation) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 19" + NL +
    @"   at TCLite.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + NL;

        private static readonly string filteredTrace1 =
    @"   at TCLite.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + NL;

        private static readonly string rawTrace2 =
    @"  at TCLite.Framework.Assert.Fail(String message, Object[] args)" + NL +
    @"  at MyNamespace.MyAppsTests.AssertFailTest()" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)" + NL +
    @"  at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)" + NL +
    @"  at TCLite.ProxyTestCase.InvokeMethod(MethodInfo method, Object[] args)" + NL +
    @"  at TCLite.Framework.TestCase.RunTest()" + NL +
    @"  at TCLite.Framework.TestCase.RunBare()" + NL +
    @"  at TCLite.Framework.TestCase.Run(TestResult result, TestListener listener)" + NL +
    @"  at TCLite.Framework.TestCase.Run(TestListener listener)" + NL +
    @"  at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"  at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"  at TCLite.Runner.TestRunner.Run(ITest test)" + NL +
    @"  at TCLite.Runner.ConsoleUI.Run(ITest test)" + NL +
    @"  at TCLite.Runner.TestRunner.Run(Assembly assembly)" + NL +
    @"  at TCLite.Runner.ConsoleUI.Run()" + NL +
    @"  at TCLite.Runner.ConsoleUI.Main(String[] args)" + NL +
    @"  at OpenNETCF.Linq.Demo.Program.Main(String[] args)" + NL;

        private static readonly string filteredTrace2 =
    @"  at MyNamespace.MyAppsTests.AssertFailTest()" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"  at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)" + NL +
    @"  at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)" + NL +
    @"  at TCLite.ProxyTestCase.InvokeMethod(MethodInfo method, Object[] args)" + NL +
    @"  at TCLite.Framework.TestCase.RunTest()" + NL +
    @"  at TCLite.Framework.TestCase.RunBare()" + NL +
    @"  at TCLite.Framework.TestCase.Run(TestResult result, TestListener listener)" + NL +
    @"  at TCLite.Framework.TestCase.Run(TestListener listener)" + NL +
    @"  at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"  at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"  at TCLite.Runner.TestRunner.Run(ITest test)" + NL +
    @"  at TCLite.Runner.ConsoleUI.Run(ITest test)" + NL +
    @"  at TCLite.Runner.TestRunner.Run(Assembly assembly)" + NL +
    @"  at TCLite.Runner.ConsoleUI.Run()" + NL +
    @"  at TCLite.Runner.ConsoleUI.Main(String[] args)" + NL +
    @"  at OpenNETCF.Linq.Demo.Program.Main(String[] args)" + NL;

        [Test]
        public void FilterFailureTrace1()
        {
            Assert.That(StackFilter.Filter(rawTrace1), Is.EqualTo(filteredTrace1));
        }

        [Test]
        public void FilterFailureTrace2()
        {
            Assert.That(StackFilter.Filter(rawTrace2), Is.EqualTo(filteredTrace2));
        }
    }
}
