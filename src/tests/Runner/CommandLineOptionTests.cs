// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System;
using System.IO;
using TCLite.Framework;

namespace TCLite.Runner.Tests
{
    [TestFixture]
    class CommandLineOptionTests
    {
        private static readonly string NL = Environment.NewLine;

        private CommandLineOptions options;

        [SetUp]
        public void CreateOptions()
        {
            options = new CommandLineOptions();
        }

        [Test]
        public void TestWaitOption()
        {
            options.Parse( "--wait" );
            Assert.That(options.Error, Is.False);
            Assert.That(options.WaitBeforeExit, Is.True);
        }

        [Test]
        public void TestNoheaderOption()
        {
            options.Parse("--noheader");
            Assert.That(options.Error, Is.False);
            Assert.That(options.NoHeader, Is.True);
        }

        [Test]
        public void TestHelpOption()
        {
            options.Parse("--help");
            Assert.That(options.Error, Is.False);
            Assert.That(options.ShowHelp, Is.True);
        }

        [Test, Ignore("NYI")]
        public void TestFullOption()
        {
            options.Parse("--full");
            Assert.That(options.Error, Is.False);
            Assert.That(options.Full, Is.True);
        }

#if !SILVERLIGHT && !NETCF
        [Test, Ignore("NYI")]
        public void TestExploreOptionWithNoFileName()
        {
            options.Parse("-explore");
            Assert.That(options.Error, Is.False);
            Assert.That(options.Explore, Is.True);
            Assert.That(options.ExploreFile, Is.EqualTo(Path.GetFullPath("tests.xml")));
        }

        [Test, Ignore("NYI")]
        public void TestExploreOptionWithGoodFileName()
        {
            options.Parse("-explore=MyFile.xml");
            Assert.That(options.Error, Is.False);
            Assert.That(options.Explore, Is.True);
            Assert.That(options.ExploreFile, Is.EqualTo(Path.GetFullPath("MyFile.xml")));
        }

        [Test, Ignore("NYI")]
        public void TestExploreOptionWithBadFileName()
        {
            options.Parse("--explore=MyFile*.xml");
            Assert.That(options.Error, Is.True);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: -explore=MyFile*.xml" + NL));
        }

        [Test]
        public void TestResultOptionWithNoFileName()
        {
            options.Parse("--result");
            Assert.That(options.Error, Is.True);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Missing required value for option '--result'."));
        }

        [Test, Ignore("NYI")]
        public void TestResultOptionWithGoodFileName()
        {
            options.Parse("-result=MyResult.xml");
            Assert.That(options.Error, Is.False);
            Assert.That(options.ResultFile, Is.EqualTo(Path.GetFullPath("MyResult.xml")));
        }

        [Test, Ignore("NYI")]
        public void TestResultOptionWithBadFileName()
        {
            options.Parse("-result=MyResult*.xml");
            Assert.That(options.Error, Is.True);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: -result=MyResult*.xml"));
        }
#endif

        [Test, Ignore("NYI")]
        public void TestNUnit2FormatOption()
        {
            options.Parse("--format=nunit2");
            Assert.That(options.Error, Is.False);
            Assert.That(options.ResultFormat, Is.EqualTo("nunit2"));
        }

        [Test, Ignore("NYI")]
        public void TestNUnit3FormatOption()
        {
            options.Parse("-format=nunit3");
            Assert.That(options.Error, Is.False);
            Assert.That(options.ResultFormat, Is.EqualTo("nunit3"));
        }

        [Test]
        public void TestBadFormatOption()
        {
            options.Parse("--format=xyz");
            Assert.That(options.Error, Is.True);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: --format=xyz"));
        }

        [Test]
        public void TestMissingFormatOption()
        {
            options.Parse("--format");
            Assert.That(options.Error, Is.True);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: --format"));
        }

#if !SILVERLIGHT && !NETCF
        [Test, Ignore("NYI")]
        public void TestOutOptionWithGoodFileName()
        {
            options.Parse("--out=myfile.txt");
            Assert.False(options.Error);
            Assert.That(options.OutFile, Is.EqualTo(Path.GetFullPath("myfile.txt")));
        }

        [Test]
        public void TestOutOptionWithNoFileName()
        {
            options.Parse("--out=");
            Assert.True(options.Error);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Missing required value for option '--output'."));
        }

        [Test, Ignore("NYI")]
        public void TestOutOptionWithBadFileName()
        {
            options.Parse("-out=my*file.txt");
            Assert.True(options.Error);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: -out=my*file.txt" + NL));
        }
#endif

        [Test, Ignore("NYI")]
        public void TestLabelsOption()
        {
            options.Parse("--labels");
            Assert.That(options.Error, Is.False);
            Assert.That(options.DisplayTestLabels, Is.True);
        }

        [Test]
        public void TestSeedOption()
        {
            options.Parse("--seed=123456789");
            Assert.False(options.Error);
            Assert.That(options.RandomSeed, Is.EqualTo(123456789));
        }

        [Test]
        public void InvalidOptionProducesError()
        {
            options.Parse( "--junk" );
            Assert.That(options.Error);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: --junk"));
        }

        [Test]
        public void MultipleInvalidOptionsAreListedInErrorMessage()
        {
            options.Parse( "--junk", "--trash", "something", "--garbage" );
            Assert.That(options.Error);
            Assert.That(options.ErrorMessages[0], Is.EqualTo("Invalid option: --junk"));
                // "Invalid option: -junk" + NL +
                // "Invalid option: -trash" + NL +
                // "Invalid option: -garbage" + NL));
        }
    }
}
