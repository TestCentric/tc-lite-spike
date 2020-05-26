// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.IO;
using TCLite.Framework;

namespace TCLite.Runner.Tests
{
    public class MakeRunSettingsTests
    {
        private static TestCaseData[] SettingsData =
        {
            new TestCaseData("--timeout=50", "DefaultTimeout", 50),
            new TestCaseData("--work=results", "WorkDirectory", Path.GetFullPath("results")),
            new TestCaseData("--seed=1234", "RandomSeed", 1234),
            new TestCaseData("--workers=8", "NumberOfTestWorkers", 8),
            new TestCaseData("--trace=Debug", "InternalTraceLevel", "Debug"),
            new TestCaseData("--stoponerror", "StopOnError", true)
            //new TestCaseData("--prefilter=A.B.C", "LOAD", new string[] { "A.B.C" }),
            //new TestCaseData("--test=A.B.C", "LOAD", new string[] { "A.B.C" }),
            //new TestCaseData("--test=A.B.C(arg)", "LOAD", new string[] { "A.B.C" }),
            //new TestCaseData("--test=A.B<type>.C(arg)", "LOAD", new string[] { "A.B" })
       };

        [TestCaseSource(nameof(SettingsData))]
        public void CheckSetting<T>(string option, string key, T value)
        {
            var options = new CommandLineOptions();
            options.Parse("test.dll", option);
            var settings = TestRunner.MakeRunSettings(options);

            Assert.That(settings.ContainsKey(key));
            Assert.That(settings[key], Is.EqualTo(value));
        }
    }
}
