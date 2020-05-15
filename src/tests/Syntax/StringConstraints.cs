// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Syntax
{
    public class SubstringTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<substring ""X"">";
            staticSyntax = Is.StringContaining("X");
            builderSyntax = Builder().ContainsSubstring("X");
        }
    }

    public class ContainsSubstringTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<substring ""X"">";
            staticSyntax = Contains.Substring("X");
            builderSyntax = Builder().ContainsSubstring("X");
        }
    }

    public class SubstringTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<substring ""X"">";
            staticSyntax = Is.StringContaining("X").IgnoreCase;
            builderSyntax = Builder().ContainsSubstring("X").IgnoreCase;
        }
    }

    public class StartsWithTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<startswith ""X"">";
            staticSyntax = Is.StringStarting("X");
            builderSyntax = Builder().StartsWith("X");
        }
    }

    public class StartsWithTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<startswith ""X"">";
            staticSyntax = Is.StringStarting("X").IgnoreCase;
            builderSyntax = Builder().StartsWith("X").IgnoreCase;
        }
    }

    public class EndsWithTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<endswith ""X"">";
            staticSyntax = Is.StringEnding("X");
            builderSyntax = Builder().EndsWith("X");
        }
    }

    public class EndsWithTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<endswith ""X"">";
            staticSyntax = Is.StringEnding("X").IgnoreCase;
            builderSyntax = Builder().EndsWith("X").IgnoreCase;
        }
    }

#if !NETCF
    public class RegexTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<regex ""X"">";
            staticSyntax = Is.StringMatching("X");
            builderSyntax = Builder().Matches("X");
        }
    }

    public class RegexTest_IgnoreCase : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            parseTree = @"<regex ""X"">";
            staticSyntax = Is.StringMatching("X").IgnoreCase;
            builderSyntax = Builder().Matches("X").IgnoreCase;
        }
    }
#endif
}
