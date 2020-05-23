// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Mono.Options;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Filters;

namespace TCLite.Runner
{
    public class TestRunner : ITestListener
    {
        private Assembly _testAssembly;
        private CommandLineOptions _options;
        private ExtendedTextWriter _writer;
        private TextUI _textUI;
        private ITestAssemblyRunner _runner;

        private bool ShowHelp;

        public TestRunner(Assembly testAssembly)
        {
            _testAssembly = testAssembly;
            _runner = new NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());
        }

        public void Execute(string[] args)
        {
            _options = new CommandLineOptions();
            _options.Parse(args);

            _writer = _options.OutFile != null
                ? new ExtendedTextWrapper(new StreamWriter(_options.OutFile))
                : new ColorConsoleWriter();
            _textUI = new TextUI(_writer);

            if (!_options.NoHeader)
                _textUI.DisplayHeader();

            if (_options.ShowHelp)
            {
                _textUI.DisplayHelp(_options._monoOptions);
                return;
            }

            if (_options.Error)
            {
                foreach (string msg in _options.ErrorMessages)
                    _writer.WriteLine(msg);
                _textUI.DisplayHelp(_options._monoOptions);
                return;
            }

            _textUI.DisplayRuntimeEnvironment();

            _textUI.DisplayTestFile(AssemblyHelper.GetAssemblyPath(_testAssembly));

            if (_options.WaitBeforeExit && _options.OutFile != null)
                _writer.WriteLine("Ignoring /wait option - only valid for Console");

#if SILVERLIGHT
            IDictionary loadOptions = new System.Collections.Generic.Dictionary<string, string>();
#else
            IDictionary loadOptions = new Hashtable();
#endif
            //if (options.Load.Count > 0)
            //    loadOptions["LOAD"] = options.Load;

            //IDictionary runOptions = new Hashtable();
            //if (commandLineOptions.TestCount > 0)
            //    runOptions["RUN"] = commandLineOptions.Tests;

            ITestFilter filter = TestFilter.Empty;

            try
            {
                Randomizer.InitialSeed = _options.RandomSeed;

                if (!_runner.Load(_testAssembly, loadOptions))
                {
                    AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(_testAssembly);
                    Console.WriteLine("No tests found in assembly {0}", assemblyName.Name);
                    return;
                }

                if (_options.Explore)
                    ExploreTests();
                else
                    RunTests(filter);
            }
            catch (FileNotFoundException ex)
            {
                _writer.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                _writer.WriteLine(ex.ToString());
            }
            finally
            {
                if (_options.OutFile == null)
                {
                    if (_options.WaitBeforeExit)
                    {
                        Console.WriteLine("Press Enter key to continue . . .");
                        Console.ReadLine();
                    }
                }
                else
                {
                    _writer.Close();
                }
            }
        }

        private void RunTests(ITestFilter filter)
        {
            DateTime startTime = DateTime.Now;

            ITestResult result = _runner.Run(this, filter);

            var summary = new ResultSummary(result);

            if (summary.FailureCount > 0 || summary.ErrorCount > 0)
                _textUI.DisplayErrorReport(result);

            if (summary.NotRunCount > 0)
                _textUI.DisplayNotRunReport(result);

            //if (commandLineOptions.Full)
            //    PrintFullReport(result);

            _textUI.DisplaySummaryReport(summary);

            // string resultFile = _options.ResultFile;
            // string resultFormat = _options.ResultFormat;
                    
            // if (resultFile != null || _options.ResultFormat != null)
            // {
            //     if (resultFile == null)
            //         resultFile = "TestResult.xml";

            //     if (resultFormat == "nunit2")
            //         new NUnit2XmlOutputWriter(startTime).WriteResultFile(result, resultFile);
            //     else
            //         new NUnit3XmlOutputWriter(startTime).WriteResultFile(result, resultFile);

            //     Console.WriteLine();
            //     Console.WriteLine("Results saved as {0}.", resultFile);
            // }
        }

        private void ExploreTests()
        {
            XmlNode testNode = _runner.LoadedTest.ToXml(true);

            // string listFile = _options.ExploreFile;
            // TextWriter textWriter = listFile != null && listFile.Length > 0
            //     ? new StreamWriter(listFile)
            //     : Console.Out;

            TextWriter textWriter = Console.Out;

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            System.Xml.XmlWriter testWriter = System.Xml.XmlWriter.Create(textWriter, settings);

            testNode.WriteTo(testWriter);
            testWriter.Close();

            Console.WriteLine();
            // Console.WriteLine("Test info saved as {0}.", listFile);
        }

        #region ITestListener Members

        private string _currentTestName;

        /// <summary>
        /// A test has just started
        /// </summary>
        /// <param name="test">The test</param>
        public void TestStarted(ITest test)
        {
            _currentTestName = test.Name;

            // if (_options.LabelTestsInOutput)
            //     _writer.WriteLine("***** {0}", _currentTestName);
        }

        /// <summary>
        /// A test has just finished
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void TestFinished(ITestResult result)
        {
        }

        /// <summary>
        /// A test has produced some text output
        /// </summary>
        /// <param name="testOutput">A TestOutput object holding the text that was written</param>
        public void TestOutput(TestOutput testOutput)
        {
            _writer.WriteLine("***** {0}", _currentTestName);
        }

        #endregion
    }
}
