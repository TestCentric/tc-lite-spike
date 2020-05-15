// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using TCLite.Framework;
using TCLite.Framework.Api;

namespace TCLite.Framework.Tests
{
    public class RecordingTestListener : ITestListener
    {
        public string Events = string.Empty;

        public void TestStarted(ITest test)
        {
            Events += string.Format("<{0}:", test.Name);
        }

        public void TestFinished(ITestResult result)
        {
            Events += string.Format(":{0}>", result.ResultState);
        }

        public void TestOutput(TestOutput output) { }
    }
}
