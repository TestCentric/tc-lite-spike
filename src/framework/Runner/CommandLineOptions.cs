// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TCLite.Framework;

namespace TCLite.Runner
{
    /// <summary>
    /// The CommandLineOptions class parses and holds the values of
    /// any options entered at the command line.
    /// </summary>
    public class CommandLineOptions
    {
        private string _optionChars;
        private static string NL = Environment.NewLine;

        private bool error = false;

        private List<string> tests = new List<string>();
        private List<string> invalidOptions = new List<string>();
        private List<string> parameters = new List<string>();

        private int randomSeed = -1;

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the 'wait' option was used.
        /// </summary>
        public bool Wait { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the 'nologo' option was used.
        /// </summary>
        public bool NoHeader { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the 'help' option was used.
        /// </summary>
        public bool ShowHelp { get; private set; }

        /// <summary>
        /// Just show the header with version and exit
        /// </summary>
        public bool ShowVersion{ get; private set; }

        /// <summary>
        /// Gets a list of all tests specified on the command line
        /// </summary>
        public string[] Tests
        {
            get { return (string[])tests.ToArray(); }
        }

        /// <summary>
        /// Gets a value indicating whether a full report should be displayed
        /// </summary>
        public bool Full { get; private set; }

        /// <summary>
        /// Gets a value indicating whether tests should be listed
        /// rather than run.
        /// </summary>
        public bool Explore { get; private set; }

        /// <summary>
        /// Gets the name of the file to be used for listing tests
        /// </summary>
        public string ExploreFile { get; private set; }

        /// <summary>
        /// Gets the name of the file to be used for test results
        /// </summary>
        public string ResultFile { get; private set; }

        /// <summary>
        /// Gets the format to be used for test results
        /// </summary>
        public string ResultFormat { get; private set; }

        /// <summary>
        /// Gets the full path of the file to be used for output
        /// </summary>
        public string OutFile { get; private set; }

        /// <summary>
        /// Gets the list of categories to include
        /// </summary>
        public string Include { get; private set; }

        /// <summary>
        /// Gets the list of categories to exclude
        /// </summary>
        public string Exclude { get; private set; }

        /// <summary>
        /// Gets a flag indicating whether each test should
        /// be labeled in the output.
        /// </summary>
        public bool LabelTestsInOutput { get; private set; }

        private string ExpandToFullPath(string path)
        {
            if (path == null) return null;

#if NETCF
            return Path.Combine(TCLite.Env.DocumentFolder, path);
#else
            return Path.GetFullPath(path); 
#endif
        }

        /// <summary>
        /// Gets the test count
        /// </summary>
        public int TestCount
        {
            get { return tests.Count; }
        }

        /// <summary>
        /// Gets the seed to be used for generating random values
        /// </summary>
        public int InitialSeed
        {
            get 
            {
                if (randomSeed < 0)
                    randomSeed = new Random().Next();

                return randomSeed; 
            }
        }

        #endregion

        /// <summary>
        /// Construct a CommandLineOptions object using default option chars
        /// </summary>
        public CommandLineOptions()
        {
            _optionChars = System.IO.Path.DirectorySeparatorChar == '/' ? "-" : "/-";
        }

        /// <summary>
        /// Construct a CommandLineOptions object using specified option chars
        /// </summary>
        /// <param name="optionChars"></param>
        public CommandLineOptions(string optionChars)
        {
            _optionChars = optionChars;
        }

        /// <summary>
        /// Parse command arguments and initialize option settings accordingly
        /// </summary>
        /// <param name="args">The argument list</param>
        public void Parse(params string[] args)
        {
            foreach( string arg in args )
            {
                if (_optionChars.IndexOf(arg[0]) >= 0 )
                    ProcessOption(arg);
                else
                    ProcessParameter(arg);
            }
        }

        /// <summary>
        ///  Gets the parameters provided on the commandline
        /// </summary>
        public string[] Parameters
        {
            get { return (string[])parameters.ToArray(); }
        }

        private void ProcessOption(string option)
        {
            string opt = option;
            int pos = opt.IndexOfAny( new char[] { ':', '=' } );
            string val = string.Empty;

            if (pos >= 0)
            {
                val = opt.Substring(pos + 1);
                opt = opt.Substring(0, pos);
            }

            switch (opt.Substring(1))
            {
                case "wait":
                    Wait = true;
                    break;
                case "noheader":
                case "noh":
                    NoHeader = true;
                    break;
                case "help":
                case "h":
                    ShowHelp = true;
                    break;
                case "version":
                    ShowVersion = true;
                    break;
                case "test":
                    tests.Add(val);
                    break;
                case "full":
                    Full = true;
                    break;
                case "explore":
                    Explore = true;
                    if (val == null || val.Length == 0)
                        val = "tests.xml";
                    try
                    {
                        ExploreFile = ExpandToFullPath(val);
                    }
                    catch
                    {
                        InvalidOption(option);
                    }
                    break;
                case "result":
                    if (val == null || val.Length == 0)
                        val = "TestResult.xml";
                    try
                    {
                        ResultFile = ExpandToFullPath(val);
                    }
                    catch
                    {
                        InvalidOption(option);
                    }
                    break;
                case "format":
                    ResultFormat = val;
                    if (ResultFormat != "nunit3" && ResultFormat != "nunit2")
                        InvalidOption(option);
                    break;
                case "out":
                    try
                    {
                        OutFile = ExpandToFullPath(val);
                    }
                    catch
                    {
                        InvalidOption(option);
                    }
                    break;
                case "labels":
                    LabelTestsInOutput = true;
                    break;
                case "include":
                    Include = val;
                    break;
                case "exclude":
                    Exclude = val;
                    break;
                case "seed":
                    try
                    {
                        randomSeed = int.Parse(val);
                    }
                    catch
                    {
                        InvalidOption(option);
                    }
                    break;
                default:
                    InvalidOption(option);
                    break;
            }
        }

        private void InvalidOption(string option)
        {
            error = true;
            invalidOptions.Add(option);
        }

        private void ProcessParameter(string param)
        {
            parameters.Add(param);
        }

        /// <summary>
        /// Gets a value indicating whether there was an error in parsing the options.
        /// </summary>
        /// <value><c>true</c> if error; otherwise, <c>false</c>.</value>
        public bool Error
        {
            get { return error; }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                foreach (string opt in invalidOptions)
                    sb.Append( "Invalid option: " + opt + NL );
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the help text.
        /// </summary>
        /// <value>The help text.</value>
        public string HelpText
        {
            get
            {
                StringBuilder sb = new StringBuilder();

#if PocketPC || WindowsCE || NETCF || SILVERLIGHT
                string name = "NUnitLite";
#else
                string name = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
#endif

                sb.Append("Usage: " + name + " [assemblies] [options]" + NL + NL);
                sb.Append("Runs a set of NUnitLite tests from the console." + NL + NL);
                sb.Append("You may specify one or more test assemblies by name, without a path or" + NL);
                sb.Append("extension. They must be in the same in the same directory as the exe" + NL);
                sb.Append("or on the probing path. If no assemblies are provided, tests in the" + NL);
                sb.Append("executing assembly itself are run." + NL + NL);
                sb.Append("Options:" + NL);
                sb.Append("  -test:testname  Provides the name of a test to run. This option may be" + NL);
                sb.Append("                  repeated. If no test names are given, all tests are run." + NL + NL);
                sb.Append("  -out:FILE       File to which output is redirected. If this option is not" + NL);
                sb.Append("                  used, output is to the Console, which means it is lost" + NL);
                sb.Append("                  on devices without a Console." + NL + NL);
                sb.Append("  -full           Prints full report of all test results." + NL + NL);
                sb.Append("  -result:FILE    File to which the xml test result is written." + NL + NL);
                sb.Append("  -format:FORMAT  Format in which the result is to be written. FORMAT must be" + NL);
                sb.Append("                  either nunit3 or nunit2. The default is nunit3." + NL + NL);
                sb.Append("  -explore:FILE  If provided, this option indicates that the tests" + NL);
                sb.Append("                  should be listed rather than executed. They are listed" + NL);
                sb.Append("                  to the specified file in XML format." + NL);
                sb.Append("  -help,-h        Displays this help" + NL + NL);
                sb.Append("  -noheader,-noh  Suppresses display of the initial message" + NL + NL);
                sb.Append("  -labels         Displays the name of each test when it starts" + NL + NL);
                sb.Append("  -seed:SEED      If provided, this option allows you to set the seed for the" + NL + NL);
                sb.Append("                  random generator in the test context." + NL + NL);
                sb.Append("  -include:CAT    List of categories to include" + NL + NL);
                sb.Append("  -exclude:CAT    List of categories to exclude" + NL + NL);
                sb.Append("  -wait           Waits for a key press before exiting" + NL + NL);

                sb.Append("Notes:" + NL);
                sb.Append(" * File names may be listed by themselves, with a relative path or " + NL);
                sb.Append("   using an absolute path. Any relative path is based on the current " + NL);
                sb.Append("   directory or on the Documents folder if running on a under the " +NL);
                sb.Append("   compact framework." + NL + NL);
                if (System.IO.Path.DirectorySeparatorChar != '/')
                    sb.Append(" * On Windows, options may be prefixed by a '/' character if desired" + NL + NL);
                sb.Append(" * Options that take values may use an equal sign or a colon" + NL);
                sb.Append("   to separate the option from its value." + NL + NL);

                return sb.ToString();
            }
        }
    }
}
