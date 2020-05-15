// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Filters;

namespace TCLite.Runner
{
    /// <summary>
    /// TextUI is a general purpose class that runs tests and
    /// outputs to a TextWriter.
    /// 
    /// Call it from your Main like this:
    ///   new TextUI(textWriter).Execute(args);
    ///     OR
    ///   new TextUI().Execute(args);
    /// The provided TextWriter is used by default, unless the
    /// arguments to Execute override it using -out. The second
    /// form uses the Console, provided it exists on the platform.
    /// 
    /// NOTE: When running on a platform without a Console, such
    /// as Windows Phone, the results will simply not appear if
    /// you fail to specify a file in the call itself or as an option.
    /// </summary>
    public class TextUI : ITestListener
    {
        private CommandLineOptions _options;

        private List<Assembly> _assemblies = new List<Assembly>();

        private ExtendedTextWriter _writer;

        private ITestAssemblyRunner _runner;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        public TextUI() : this(new ColorConsoleWriter(true), TestListener.NULL) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        /// <param name="writer">The TextWriter to use.</param>
        public TextUI(ExtendedTextWriter writer) : this(writer, TestListener.NULL) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        /// <param name="writer">The TextWriter to use.</param>
        /// <param name="listener">The Test listener to use.</param>
        public TextUI(ExtendedTextWriter writer, ITestListener listener)
        {
            // Set the default writer - may be overridden by the args specified
            _writer = writer;
            _runner = new NUnitLiteTestAssemblyRunner(new NUnitLiteTestAssemblyBuilder());
        }

        /// <summary>
        /// Execute a test run based on the arguments passed
        /// from Main.
        /// </summary>
        /// <param name="args">An array of arguments</param>
        public void Execute(string[] args)
        {
            // NOTE: Execute must be directly called from the
            // test assembly in order for the mechanism to work.
            Assembly callingAssembly = Assembly.GetCallingAssembly();

            _options = new CommandLineOptions();
            _options.Parse(args);

            if (_options.OutFile != null)
                _writer = new ExtendedTextWrapper(new StreamWriter(_options.OutFile));

            if (!_options.NoHeader)
                DisplayHeader();

            if (_options.ShowHelp)
                _writer.Write(_options.HelpText);
            else if (_options.Error)
            {
                _writer.WriteLine(_options.ErrorMessage);
                _writer.WriteLine(_options.HelpText);
            }
            else
            {
                DisplayRuntimeEnvironment();

                DisplayTestFiles(_options.Parameters);

                if (_options.Wait && _options.OutFile != null)
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

                ITestFilter filter = _options.TestCount > 0
                    ? new SimpleNameFilter(_options.Tests)
                    : TestFilter.Empty;

                try
                {
                    foreach (string name in _options.Parameters)
                        _assemblies.Add(Assembly.Load(name));

                    if (_assemblies.Count == 0)
                        _assemblies.Add(callingAssembly);

                    // TODO: For now, ignore all but first assembly
                    Assembly assembly = _assemblies[0] as Assembly;

                    Randomizer.InitialSeed = _options.InitialSeed;

                    if (!_runner.Load(assembly, loadOptions))
                    {
                        AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(assembly);
                        Console.WriteLine("No tests found in assembly {0}", assemblyName.Name);
                        return;
                    }

                    if (_options.Explore)
                        ExploreTests();
                    else
                    {
                        if (_options.Include != null && _options.Include != string.Empty)
                        {
                            TestFilter includeFilter = new SimpleCategoryExpression(_options.Include).Filter;

                            if (filter.IsEmpty)
                                filter = includeFilter;
                            else
                                filter = new AndFilter(filter, includeFilter);
                        }

                        if (_options.Exclude != null && _options.Exclude != string.Empty)
                        {
                            TestFilter excludeFilter = new NotFilter(new SimpleCategoryExpression(_options.Exclude).Filter);

                            if (filter.IsEmpty)
                                filter = excludeFilter;
                            else if (filter is AndFilter)
                                ((AndFilter)filter).Add(excludeFilter);
                            else
                                filter = new AndFilter(filter, excludeFilter);
                        }

                        RunTests(filter);
                    }
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
                        if (_options.Wait)
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
        }

        /// <summary>
        /// Write the standard header information to a TextWriter.
        /// </summary>
        /// <param name="writer">The TextWriter to use</param>
        public void DisplayHeader()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string title = "TcLite";
            AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(executingAssembly);
            Version version = assemblyName.Version;
            string copyright = "Copyright (C) 2020, Charlie Poole";
            string build = "";

            var titleAttr = executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (titleAttr != null)
                title = titleAttr.Title;

            var copyrightAttr = executingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            if (copyrightAttr != null)
                copyright = copyrightAttr.Copyright;

            var configAttr = executingAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
            if (configAttr != null)
                build = $"({configAttr.Configuration})";

            WriteHeader($"{title} {version.ToString(3)} {build}");
            WriteSubHeader(copyright);
            _writer.WriteLine();
        }

        /// <summary>
        /// Write information about the current runtime environment
        /// </summary>
        /// <param name="writer">The TextWriter to be used</param>
        public void DisplayRuntimeEnvironment()
        {
            string clrPlatform = Type.GetType("Mono.Runtime", false) == null ? ".NET" : "Mono";

            WriteSectionHeader("Runtime Environment -");
            _writer.WriteLabelLine("    OS Version: ", Environment.OSVersion);
            _writer.WriteLabelLine($"  {clrPlatform} Version: ", Environment.Version);
            _writer.WriteLine();
        }

        public void DisplayTestFiles(IEnumerable<string> testFiles)
        {
            WriteSectionHeader("Test Files -");

            foreach (string testFile in testFiles)
                _writer.WriteLine(ColorStyle.Default, "    " + testFile);

            _writer.WriteLine();
        }

        #region Helper Methods

        private void RunTests(ITestFilter filter)
        {
            DateTime startTime = DateTime.Now;

            ITestResult result = _runner.Run(this, filter);
            new ResultReporter(result, _writer).ReportResults();
            string resultFile = _options.ResultFile;
            string resultFormat = _options.ResultFormat;
                    
            if (resultFile != null || _options.ResultFormat != null)
            {
                if (resultFile == null)
                    resultFile = "TestResult.xml";

                if (resultFormat == "nunit2")
                    new NUnit2XmlOutputWriter(startTime).WriteResultFile(result, resultFile);
                else
                    new NUnit3XmlOutputWriter(startTime).WriteResultFile(result, resultFile);

                Console.WriteLine();
                Console.WriteLine("Results saved as {0}.", resultFile);
            }
        }

        private void ExploreTests()
        {
            XmlNode testNode = _runner.LoadedTest.ToXml(true);

            string listFile = _options.ExploreFile;
            TextWriter textWriter = listFile != null && listFile.Length > 0
                ? new StreamWriter(listFile)
                : Console.Out;

            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            System.Xml.XmlWriter testWriter = System.Xml.XmlWriter.Create(textWriter, settings);

            testNode.WriteTo(testWriter);
            testWriter.Close();

            Console.WriteLine();
            Console.WriteLine("Test info saved as {0}.", listFile);
        }

        private void WriteHeader(string text)
        {
            _writer.WriteLine(ColorStyle.Header, text);
        }

        private void WriteSubHeader(string text)
        {
            _writer.WriteLine(ColorStyle.SubHeader, text);
        }

        private void WriteSectionHeader(string text)
        {
            _writer.WriteLine(ColorStyle.SectionHeader, text);
        }

        #endregion

        #region ITestListener Members

        /// <summary>
        /// A test has just started
        /// </summary>
        /// <param name="test">The test</param>
        public void TestStarted(ITest test)
        {
            if (_options.LabelTestsInOutput)
                _writer.WriteLine("***** {0}", test.Name);
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
        }

        #endregion
    }
}
