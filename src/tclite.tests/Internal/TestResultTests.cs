// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.TestUtilities;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// Summary description for TestResultTests.
	/// </summary>
	[TestFixture]
	public abstract class TestResultTests
	{
		protected TestResult testResult;
        protected TestResult suiteResult;
        protected TestMethod test;

        protected string ignoredChildMessage = "One or more child tests were ignored";
        protected string failingChildMessage = "One or more child tests had errors";

		[SetUp]
		public void SetUp()
		{
            TestSuite suite = new TestSuite(typeof(DummySuite));
            suite.Properties.Set(PropertyNames.Description, "Suite description");
            suite.Properties.Add(PropertyNames.Category, "Fast");
            suite.Properties.Add("Value", 3);
            suiteResult = suite.MakeTestResult();

            test = new TestMethod(typeof(DummySuite).GetMethod("DummyMethod"), suite);
            test.Properties.Set(PropertyNames.Description, "Test description");
            test.Properties.Add(PropertyNames.Category, "Dubious");
            test.Properties.Set("Priority", "low");
			testResult = test.MakeTestResult();

            SimulateTestRun();
        }

        [Test]
        public void TestResultBasicInfo()
        {
            Assert.AreEqual("DummyMethod", testResult.Name);
            Assert.AreEqual("TCLite.Framework.Internal.TestResultTests+DummySuite.DummyMethod", testResult.FullName);
        }

        [Test]
        public void SuiteResultBasicInfo()
        {
            Assert.AreEqual("TestResultTests+DummySuite", suiteResult.Name);
            Assert.AreEqual("TCLite.Framework.Internal.TestResultTests+DummySuite", suiteResult.FullName);
        }

        [Test]
        public void TestResultBasicInfo_XmlNode()
        {
            XmlNode testNode = testResult.ToXml(true);

            //Assert.True(testNode is XmlElement);
            Assert.NotNull(testNode.Attributes["id"]);
            Assert.AreEqual("test-case", testNode.Name);
            Assert.AreEqual("DummyMethod", testNode.Attributes["name"]?.Value);
            Assert.AreEqual("TCLite.Framework.Internal.TestResultTests+DummySuite.DummyMethod", testNode.Attributes["fullname"]?.Value);

            Assert.AreEqual("Test description", testNode.SelectSingleNode("properties/property[@name='Description']").Attributes["value"]?.Value);
            Assert.AreEqual("Dubious", testNode.SelectSingleNode("properties/property[@name='Category']").Attributes["value"]?.Value);
            Assert.AreEqual("low", testNode.SelectSingleNode("properties/property[@name='Priority']").Attributes["value"]?.Value);

            Assert.AreEqual(0, testNode.SelectNodes("test-case").Count);
        }

        [Test]
        public void TestResultBasicInfo_WriteXml()
        {
            XmlNode testNode = testResult.ToXml(true);

            string expected = GenerateExpectedXml(testResult);

            StringBuilder actual = new StringBuilder();
            StringWriter sw = new StringWriter(actual);
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.CloseOutput = true;
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sw, settings);
            testNode.WriteTo(writer);
            writer.Close();

            Assert.That(actual.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void SuiteResultBasicInfo_XmlNode()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            //Assert.True(suiteNode is XmlElement);
            Assert.NotNull(suiteNode.Attributes["id"]);
            Assert.AreEqual("test-suite", suiteNode.Name);
            Assert.AreEqual("TestResultTests+DummySuite", suiteNode.Attributes["name"]?.Value);
            Assert.AreEqual("TCLite.Framework.Internal.TestResultTests+DummySuite", suiteNode.Attributes["fullname"]?.Value);

            Assert.AreEqual("Suite description", suiteNode.SelectSingleNode("properties/property[@name='Description']").Attributes["value"]?.Value);
            Assert.AreEqual("Fast", suiteNode.SelectSingleNode("properties/property[@name='Category']").Attributes["value"]?.Value);
            Assert.AreEqual("3", suiteNode.SelectSingleNode("properties/property[@name='Value']").Attributes["value"]?.Value);
        }

        [Test]
        public void SuiteResultBasicInfo_WriteXml()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            string expected = GenerateExpectedXml(suiteResult);

            StringBuilder actual = new StringBuilder();
            StringWriter sw = new StringWriter(actual);
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.CloseOutput = true;
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sw, settings);
            suiteNode.WriteTo(writer);
            writer.Close();

            Assert.That(actual.ToString(), Is.EqualTo(expected));
        }

        protected abstract void SimulateTestRun();

        private static string GenerateExpectedXml(TestResult result)
        {
            StringBuilder expected = new StringBuilder();
            TestSuiteResult suiteResult = result as TestSuiteResult;

            if(suiteResult != null)
                expected.Append("<test-suite type=" + Quoted("TestSuite"));
            else
                expected.Append("<test-case");

            expected.Append(" id=" + Quoted(result.Test.Id));
            expected.Append(" name=" + Quoted(result.Name));
            expected.Append(" fullname=" + Quoted(result.FullName));

            if (suiteResult == null)
                expected.Append(" seed=" + Quoted(result.Test.Seed));

            if (suiteResult != null)
                expected.Append(" testcasecount=" + Quoted(result.Test.TestCaseCount));

            expected.Append(" result=" + Quoted(result.ResultState.Status));
            if (result.ResultState.Label != null && result.ResultState.Label != "")
                expected.Append(" label=" + Quoted(result.ResultState.Label));

            expected.Append(" time=" + Quoted(result.Duration.ToString()));

            if (suiteResult != null)
            {
                ResultSummary summary = new ResultSummary(suiteResult);
                expected.Append(" total=" + Quoted(suiteResult.PassCount+suiteResult.FailCount+suiteResult.InconclusiveCount+suiteResult.SkipCount));
                expected.Append(" passed=" + Quoted(suiteResult.PassCount));
                expected.Append(" failed=" + Quoted(suiteResult.FailCount));
                expected.Append(" inconclusive=" + Quoted(suiteResult.InconclusiveCount));
                expected.Append(" skipped=" + Quoted(suiteResult.SkipCount));
            }

            expected.Append(" asserts=" + Quoted(result.AssertCount) + ">");

            if (result.Test.Properties.Count > 0)
            {
                expected.Append("<properties>");
                foreach (string key in result.Test.Properties.Keys)
                    foreach (object value in result.Test.Properties[key])
                        expected.Append("<property name=" + Quoted(key) + " value=" + Quoted(value) + " />");
                expected.Append("</properties>");
            }

            if (result.ResultState.Status == TestStatus.Failed)
            {
                expected.Append("<failure>");
                if (result.Message != null)
                    expected.Append("<message>" + Escape(result.Message) + "</message>");

                if (result.StackTrace != null)
                    expected.Append("<stack-trace>" + Escape(result.StackTrace) + "</stack-trace>");

                expected.Append("</failure>");
            }
            else if (result.Message != null)
            {
                expected.Append("<reason><message>" + Escape(result.Message) + "</message></reason>");
            }

            if (suiteResult != null)
            {
                foreach (TestResult childResult in suiteResult.Children)
                    expected.Append(GenerateExpectedXml(childResult));

                expected.Append("</test-suite>");
            }
            else
                expected.Append("</test-case>");

            return expected.ToString();
        }

        private static string Quoted(object o)
        {
            return "\"" + o.ToString() + "\"";
        }

        private static string Escape(string s)
        {
            return s
                .Replace("&", "&amp;")
                .Replace(">", "&gt;")
                .Replace("<", "&lt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        public class DummySuite
        {
            public void DummyMethod() { }
        }
    }

    public class DefaultResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void TestResultIsInconclusive()
        {
            Assert.AreEqual(ResultState.Inconclusive, testResult.ResultState);
            Assert.AreEqual(TestStatus.Inconclusive, testResult.ResultState.Status);
            Assert.That(testResult.ResultState.Label, Is.Empty);
            Assert.That(testResult.Duration, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void SuiteResultIsInconclusive()
        {
            Assert.AreEqual(ResultState.Inconclusive, suiteResult.ResultState);
            Assert.AreEqual(0, suiteResult.AssertCount);
        }

        [Test]
        public void TestResultXmlNodeIsInconclusive()
        {
            XmlNode testNode = testResult.ToXml(true);

            Assert.AreEqual("Inconclusive", testNode.Attributes["result"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeIsInconclusive()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Inconclusive", suiteNode.Attributes["result"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasOneChildTest()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(1, suiteNode.SelectNodes("test-case").Count);
        }
    }

    public class SuccessResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            testResult.SetResult(ResultState.Success, "Test passed!");
            testResult.Duration = TimeSpan.FromSeconds(0.125);
            suiteResult.Duration = TimeSpan.FromSeconds(0.125);
            testResult.AssertCount = 2;
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void TestResultIsSuccess()
        {
            Assert.True(testResult.ResultState == ResultState.Success);
            Assert.AreEqual(TestStatus.Passed, testResult.ResultState.Status);
            Assert.That(testResult.ResultState.Label, Is.Empty);
            Assert.AreEqual("Test passed!", testResult.Message);
            Assert.That(testResult.Duration.TotalSeconds, Is.EqualTo(0.125));
        }

        [Test]
        public void SuiteResultIsSuccess()
        {
            Assert.True(suiteResult.ResultState == ResultState.Success);
            Assert.AreEqual(TestStatus.Passed, suiteResult.ResultState.Status);
            Assert.That(suiteResult.ResultState.Label, Is.Empty);

            Assert.AreEqual(1, suiteResult.PassCount);
            Assert.AreEqual(0, suiteResult.FailCount);
            Assert.AreEqual(0, suiteResult.SkipCount);
            Assert.AreEqual(0, suiteResult.InconclusiveCount);
            Assert.AreEqual(2, suiteResult.AssertCount);
        }

        [Test]
        public void TestResultXmlNodeIsSuccess()
        {
            XmlNode testNode = testResult.ToXml(true);

            Assert.AreEqual("Passed", testNode.Attributes["result"]?.Value);
            Assert.AreEqual("00:00:00.1250000", testNode.Attributes["time"]?.Value);
            Assert.AreEqual("2", testNode.Attributes["asserts"]?.Value);

            XmlNode reason = testNode.SelectSingleNode("reason");
            Assert.NotNull(reason);
            Assert.NotNull(reason.SelectSingleNode("message"));
            Assert.AreEqual("Test passed!", reason.SelectSingleNode("message").InnerText);
            //Assert.AreEqual("Test passed!", reason.SelectSingleNode("message").EscapedTextContent);
            Assert.Null(reason.SelectSingleNode("stack-trace"));
        }

        [Test]
        public void SuiteResultXmlNodeIsSuccess()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Passed", suiteNode.Attributes["result"]?.Value);
            Assert.AreEqual("00:00:00.1250000", suiteNode.Attributes["time"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("2", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasOneChildTest()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(1, suiteNode.SelectNodes("test-case").Count);
        }
    }

    public class IgnoredResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            testResult.SetResult(ResultState.Ignored, "because");
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void TestResultIsIgnored()
        {
            Assert.AreEqual(ResultState.Ignored, testResult.ResultState);
            Assert.AreEqual(TestStatus.Skipped, testResult.ResultState.Status);
            Assert.AreEqual("Ignored", testResult.ResultState.Label);
            Assert.AreEqual("because", testResult.Message);
        }

        [Test]
        public void SuiteResultIsIgnored()
        {
            Assert.AreEqual(ResultState.Ignored, suiteResult.ResultState);
            Assert.AreEqual(TestStatus.Skipped, suiteResult.ResultState.Status);
            Assert.AreEqual(ignoredChildMessage, suiteResult.Message);

            Assert.AreEqual(0, suiteResult.PassCount);
            Assert.AreEqual(0, suiteResult.FailCount);
            Assert.AreEqual(1, suiteResult.SkipCount);
            Assert.AreEqual(0, suiteResult.InconclusiveCount);
            Assert.AreEqual(0, suiteResult.AssertCount);
        }

        [Test]
        public void TestResultXmlNodeIsIgnored()
        {
            XmlNode testNode = testResult.ToXml(true);

            Assert.AreEqual("Skipped", testNode.Attributes["result"]?.Value);
            Assert.AreEqual("Ignored", testNode.Attributes["label"]?.Value);
            XmlNode reason = testNode.SelectSingleNode("reason");
            Assert.NotNull(reason);
            Assert.NotNull(reason.SelectSingleNode("message"));
            Assert.AreEqual("because", reason.SelectSingleNode("message").InnerText);
            //Assert.AreEqual("because", reason.SelectSingleNode("message").EscapedTextContent);
            Assert.Null(reason.SelectSingleNode("stack-trace"));
        }

        [Test]
        public void SuiteResultXmlNodeIsIgnored()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Skipped", suiteNode.Attributes["result"]?.Value);
            Assert.AreEqual("Ignored", suiteNode.Attributes["label"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasOneChildTest()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(1, suiteNode.SelectNodes("test-case").Count);
        }
    }

    public class FailedResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            testResult.SetResult(ResultState.Failure, "message with <xml> & straight text", "stack trace");
            testResult.Duration = TimeSpan.FromSeconds(0.125);
            suiteResult.Duration = TimeSpan.FromSeconds(0.125);
            testResult.AssertCount = 3;
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void TestResultIsFailure()
        {
            Assert.AreEqual(ResultState.Failure, testResult.ResultState);
            Assert.AreEqual(TestStatus.Failed, testResult.ResultState.Status);
            Assert.AreEqual("message with <xml> & straight text", testResult.Message);
            Assert.AreEqual("stack trace", testResult.StackTrace);
            Assert.AreEqual(0.125, testResult.Duration.TotalSeconds);
        }

        [Test]
        public void SuiteResultIsFailure()
        {
            Assert.AreEqual(ResultState.Failure, suiteResult.ResultState);
            Assert.AreEqual(TestStatus.Failed, suiteResult.ResultState.Status);
            Assert.AreEqual(failingChildMessage, suiteResult.Message);
            Assert.Null(suiteResult.StackTrace);

            Assert.AreEqual(0, suiteResult.PassCount);
            Assert.AreEqual(1, suiteResult.FailCount);
            Assert.AreEqual(0, suiteResult.SkipCount);
            Assert.AreEqual(0, suiteResult.InconclusiveCount);
            Assert.AreEqual(3, suiteResult.AssertCount);
        }

        [Test]
        public void TestResultXmlNodeIsFailure()
        {
            XmlNode testNode = testResult.ToXml(true);

            Assert.AreEqual("Failed", testNode.Attributes["result"]?.Value);
            Assert.AreEqual("00:00:00.1250000", testNode.Attributes["time"]?.Value);

            XmlNode failureNode = testNode.SelectSingleNode("failure");
            Assert.NotNull(failureNode, "No <failure> element found");

            XmlNode messageNode = failureNode.SelectSingleNode("message");
            Assert.NotNull(messageNode, "No <message> element found");
            Assert.AreEqual("message with <xml> & straight text", messageNode.InnerText);
            //Assert.AreEqual("message with &lt;xml&gt; &amp; straight text", messageNode.EscapedTextContent);

            XmlNode stacktraceNode = failureNode.SelectSingleNode("stack-trace");
            Assert.NotNull(stacktraceNode, "No <stack-trace> element found");
            Assert.AreEqual("stack trace", stacktraceNode.InnerText);
            //Assert.AreEqual("stack trace", stacktraceNode.EscapedTextContent);
        }

        [Test]
        public void SuiteResultXmlNodeIsFailure()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Failed", suiteNode.Attributes["result"]?.Value);
            Assert.AreEqual("00:00:00.1250000", suiteNode.Attributes["time"]?.Value);

            XmlNode failureNode = suiteNode.SelectSingleNode("failure");
            Assert.NotNull(failureNode, "No <failure> element found");

            XmlNode messageNode = failureNode.SelectSingleNode("message");
            Assert.NotNull(messageNode, "No <message> element found");
            Assert.AreEqual(failingChildMessage, messageNode.InnerText);
            //Assert.AreEqual(failingChildMessage, messageNode.EscapedTextContent);

            XmlNode stacktraceNode = failureNode.SelectSingleNode("stacktrace");
            Assert.Null(stacktraceNode, "Unexpected <stack-trace> element found");

            Assert.AreEqual("0", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("3", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasOneChildTest()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(1, suiteNode.SelectNodes("test-case").Count);
        }
    }

    public class InconclusiveResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            testResult.SetResult(ResultState.Inconclusive, "because");
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void TestResultIsInconclusive()
        {
            Assert.AreEqual(ResultState.Inconclusive, testResult.ResultState);
            Assert.AreEqual(TestStatus.Inconclusive, testResult.ResultState.Status);
            Assert.That(testResult.ResultState.Label, Is.Empty);
            Assert.AreEqual("because", testResult.Message);
        }

        [Test]
        public void SuiteResultIsInconclusive()
        {
            Assert.AreEqual(ResultState.Inconclusive, suiteResult.ResultState);
            Assert.AreEqual(TestStatus.Inconclusive, suiteResult.ResultState.Status);
            Assert.Null(suiteResult.Message);

            Assert.AreEqual(0, suiteResult.PassCount);
            Assert.AreEqual(0, suiteResult.FailCount);
            Assert.AreEqual(0, suiteResult.SkipCount);
            Assert.AreEqual(1, suiteResult.InconclusiveCount);
            Assert.AreEqual(0, suiteResult.AssertCount);
        }

        [Test]
        public void TestResultXmlNodeIsInconclusive()
        {
            XmlNode testNode = testResult.ToXml(true);

            Assert.AreEqual("Inconclusive", testNode.Attributes["result"]?.Value);
            Assert.Null(testNode.Attributes["label"], "Unexpected attribute 'label' found");
            XmlNode reason = testNode.SelectSingleNode("reason");
            Assert.NotNull(reason);
            Assert.NotNull(reason.SelectSingleNode("message"));
            Assert.AreEqual("because", reason.SelectSingleNode("message").InnerText);
            //Assert.AreEqual("because", reason.SelectSingleNode("message").EscapedTextContent);
            Assert.Null(reason.SelectSingleNode("stack-trace"));
        }

        [Test]
        public void SuiteResultXmlNodeIsInconclusive()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Inconclusive", suiteNode.Attributes["result"]?.Value);
            Assert.Null(suiteNode.Attributes["label"], "Unexpected 'label' attribute found");
            Assert.AreEqual("0", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasOneChildTest()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(1, suiteNode.SelectNodes("test-case").Count);
        }
    }

    public class MixedResultTests : TestResultTests
    {
        protected override void SimulateTestRun()
        {
            testResult.SetResult(ResultState.Success);
            testResult.AssertCount = 2;
            suiteResult.AddResult(testResult);

            testResult.SetResult(ResultState.Failure, "message", "stack trace");
            testResult.AssertCount = 1;
            suiteResult.AddResult(testResult);

            testResult.SetResult(ResultState.Success);
            testResult.AssertCount = 3;
            suiteResult.AddResult(testResult);

            testResult.SetResult(ResultState.Inconclusive, "inconclusive reason", "stacktrace");
            testResult.AssertCount = 0;
            suiteResult.AddResult(testResult);
        }

        [Test]
        public void SuiteResultIsFailure()
        {
            Assert.AreEqual(ResultState.Failure, suiteResult.ResultState);
            Assert.AreEqual(TestStatus.Failed, suiteResult.ResultState.Status);
            Assert.AreEqual(failingChildMessage, suiteResult.Message);
            Assert.Null(suiteResult.StackTrace, "There should be no stacktrace");

            Assert.AreEqual(2, suiteResult.PassCount);
            Assert.AreEqual(1, suiteResult.FailCount);
            Assert.AreEqual(0, suiteResult.SkipCount);
            Assert.AreEqual(1, suiteResult.InconclusiveCount);
            Assert.AreEqual(6, suiteResult.AssertCount);
        }

        [Test]
        public void SuiteResultXmlNodeIsFailure()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual("Failed", suiteNode.Attributes["result"]?.Value);
            XmlNode failureNode = suiteNode.SelectSingleNode("failure");
            Assert.NotNull(failureNode, "No failure element found");

            XmlNode messageNode = failureNode.SelectSingleNode("message");
            Assert.NotNull(messageNode, "No message element found");
            Assert.AreEqual(failingChildMessage, messageNode.InnerText);
            //Assert.AreEqual(failingChildMessage, messageNode.EscapedTextContent);

            XmlNode stacktraceNode = failureNode.SelectSingleNode("stacktrace");
            Assert.Null(stacktraceNode, "There should be no stacktrace");

            Assert.AreEqual("2", suiteNode.Attributes["passed"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["failed"]?.Value);
            Assert.AreEqual("0", suiteNode.Attributes["skipped"]?.Value);
            Assert.AreEqual("1", suiteNode.Attributes["inconclusive"]?.Value);
            Assert.AreEqual("6", suiteNode.Attributes["asserts"]?.Value);
        }

        [Test]
        public void SuiteResultXmlNodeHasFourChildTests()
        {
            XmlNode suiteNode = suiteResult.ToXml(true);

            Assert.AreEqual(4, suiteNode.SelectNodes("test-case").Count);
        }
    }
}
