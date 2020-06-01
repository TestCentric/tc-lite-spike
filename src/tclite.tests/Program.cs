// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using TCLite.Runner;
using TCLite.Framework.Internal;

namespace TCLite.Tests
{
    public class Program
    {
        // The main program executes the tests. Output may be routed to
        // various locations, depending on the arguments passed.
        public static void Main(string[] args)
        {
            new TestRunner().Execute(args);
        }
    }
}