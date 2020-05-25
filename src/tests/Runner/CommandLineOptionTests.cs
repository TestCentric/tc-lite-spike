// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.IO;
using System.Reflection;
using TCLite.Framework;

namespace TCLite.Runner.Tests
{
    [TestFixture]
    class CommandLineOptionTests
    {
        private CommandLineOptions _options;

        [SetUp]
        public void CreateOptions()
        {
            _options = new CommandLineOptions();
        }

        [TestCase("ShowHelp", "--help")]
        [TestCase("ShowHelp", "-h")]
        [TestCase("ShowVersion", "--version")]
        [TestCase("ShowVersion", "-V")]
        [TestCase("StopOnError", "--stoponerror")]
        [TestCase("WaitBeforeExit", "--wait")]
        [TestCase("NoHeader", "--noheader")]
        [TestCase("NoHeader", "--noh")]
        // NYI: [TestCase("Full", "--full")]
        // NYI: [TestCase("TeamCity", "teamcity")]
        public void BooleanOptions(string propertyName, string option)
        {
            PropertyInfo property = GetPropertyInfo(propertyName);
            Assert.AreEqual(typeof(bool), property.PropertyType, "Property '{0}' is wrong type", propertyName);

            _options.Parse(option);
            Assert.AreEqual(true, (bool)property.GetValue(_options, null), "Didn't recognize -" + option);
        }

        [TestCase("WhereClause", "--where", "cat==Fast")]
        [TestCase("DisplayTestLabels", "--labels", "Off")]
        [TestCase("DisplayTestLabels", "--labels", "On")]
        [TestCase("DisplayTestLabels", "--labels", "Before")]
        [TestCase("DisplayTestLabels", "--labels", "After")]
        [TestCase("DisplayTestLabels", "--labels", "All")]
        [TestCase("OutFile", "--out", "output.txt")]
        [TestCase("ErrFile", "--err", "error.txt")]
        [TestCase("WorkDirectory", "--work", "results")]
        [TestCase("InternalTraceLevel", "--trace", "Off")]
        [TestCase("InternalTraceLevel", "--trace", "Error")]
        [TestCase("InternalTraceLevel", "--trace", "Warning")]
        [TestCase("InternalTraceLevel", "--trace", "Info")]
        [TestCase("InternalTraceLevel", "--trace", "Debug")]
        [TestCase("InternalTraceLevel", "--trace", "Verbose")]
        [TestCase("ResultFile", "--result", "MyOwnResult.xml")]
        [TestCase("ResultFormat", "--format", "nunit2")]
        [TestCase("ResultFormat", "--format", "nunit3")]
        [TestCase("RandomSeed", "--seed", 123456789)]
        [TestCase("DefaultTimeout", "--timeout", 2000)]
        [TestCase("NumberOfTestWorkers", "--workers", 32)]
        public void ValidOptionValues<T>(string propertyName, string option, T value)
        {
            PropertyInfo property = GetPropertyInfo(propertyName);
            Assert.AreEqual(typeof(T), property.PropertyType);

            string optionPlusValue = $"{option}={value}";
            _options.Parse(optionPlusValue);
            if (_options.Error)
                Assert.Fail("Unexpected Error: " + _options.ErrorMessages[0]);
            Assert.AreEqual(value, (T)property.GetValue(_options, null), "Didn't recognize " + optionPlusValue);
       }

        // NYI: [TestCase("--where")]
        [TestCase("--labels", "JUNK")]
        // NYI: [TestCase("--out")]
        // NYI: [TestCase("--err")]
        // NYI: [TestCase("--work")]
        [TestCase("--trace", "JUNK")]
        // NYI: [TestCase("--result")]
        [TestCase("--format", "xyz")]
        [TestCase("--seed", "xxx")]
        [TestCase("--timeout", "ABC")]
        [TestCase("--workers", "Yes")]
        public void InvalidOptionValues(string option, string value)
        {
            string optionPlusValue = $"{option}={value}";
            _options.Parse(optionPlusValue);
            Assert.True(_options.Error, "Should not be valid: " + optionPlusValue);
            Assert.That(_options.ErrorMessages.Count, Is.EqualTo(1));
            Assert.That(_options.ErrorMessages[0], Is.EqualTo($"The value {value} is not valid for option {option}."));
        }

        [TestCase("--where")]
        [TestCase("--labels")]
        [TestCase("--out")]
        [TestCase("--err")]
        [TestCase("--work")]
        [TestCase("--trace")]
        [TestCase("--result")]
        [TestCase("--format")]
        [TestCase("--seed")]
        [TestCase("--timeout")]
        [TestCase("--workers")]
        public void MissingOptionValues(string option)
        {
            _options.Parse(option);
            Assert.True(_options.Error, "Should not be valid: " + option);
            Assert.That(_options.ErrorMessages.Count, Is.EqualTo(1));
            Assert.That(_options.ErrorMessages[0], Is.EqualTo($"Missing required value for option '{option}'."));
        }

#if !SILVERLIGHT && !NETCF
        [Test, Ignore("NYI")]
        public void TestExploreOptionWithNoFileName()
        {
            _options.Parse("-explore");
            Assert.That(_options.Error, Is.False);
            Assert.That(_options.Explore, Is.True);
            Assert.That(_options.ExploreFile, Is.EqualTo(Path.GetFullPath("tests.xml")));
        }

        [Test, Ignore("NYI")]
        public void TestExploreOptionWithGoodFileName()
        {
            _options.Parse("-explore=MyFile.xml");
            Assert.That(_options.Error, Is.False);
            Assert.That(_options.Explore, Is.True);
            Assert.That(_options.ExploreFile, Is.EqualTo(Path.GetFullPath("MyFile.xml")));
        }

        [Test, Ignore("NYI")]
        public void TestExploreOptionWithBadFileName()
        {
            _options.Parse("--explore=MyFile*.xml");
            Assert.That(_options.Error, Is.True);
            Assert.That(_options.ErrorMessages[0], Is.EqualTo("Invalid option: -explore=MyFile*.xml"));
        }

        [Test, Ignore("NYI")]
        public void TestResultOptionWithBadFileName()
        {
            _options.Parse("-result=MyResult*.xml");
            Assert.That(_options.Error, Is.True);
            Assert.That(_options.ErrorMessages[0], Is.EqualTo("Invalid option: -result=MyResult*.xml"));
        }
#endif

#if !SILVERLIGHT && !NETCF
        [Test, Ignore("NYI")]
        public void TestOutOptionWithBadFileName()
        {
            _options.Parse("-out=my*file.txt");
            Assert.True(_options.Error);
            Assert.That(_options.ErrorMessages[0], Is.EqualTo("Invalid option: -out=my*file.txt"));
        }
#endif

        [Test]
        public void InvalidOptionProducesError()
        {
            _options.Parse( "--junk" );
            Assert.That(_options.Error);
            Assert.That(_options.ErrorMessages.Count, Is.EqualTo(1));
            Assert.That(_options.ErrorMessages[0], Is.EqualTo("Invalid option: --junk"));
        }

        [Test]
        public void MultipleInvalidOptionsAreListedInErrorMessage()
        {
            _options.Parse( "--junk", "--trash", "something", "--garbage" );
            Assert.That(_options.Error);
            Assert.That(_options.ErrorMessages[0], Is.EqualTo("Invalid option: --junk"));
                // "Invalid option: -junk" + NL +
                // "Invalid option: -trash" + NL +
                // "Invalid option: -garbage" + NL));
        }
    
        private static PropertyInfo GetPropertyInfo(string propertyName)
        {
            PropertyInfo property = typeof(CommandLineOptions).GetProperty(propertyName);
            Assert.NotNull(property, "The property '{0}' is not defined", propertyName);
            return property;
        }
    }
}
