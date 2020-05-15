// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Api
{
	using System;

    /// <summary>
    /// The TestOutput class holds a unit of output from 
    /// a test to either stdOut or stdErr
    /// </summary>
	public class TestOutput
	{
		string text;
		TestOutputType type;

        /// <summary>
        /// Construct with text and an output destination type
        /// </summary>
        /// <param name="text">Text to be output</param>
        /// <param name="type">Destination of output</param>
		public TestOutput(string text, TestOutputType type)
		{
			this.text = text;
			this.type = type;
		}

        /// <summary>
        /// Return string representation of the object for debugging
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return type + ": " + text;
		}

        /// <summary>
        /// Get the text 
        /// </summary>
		public string Text
		{
			get
			{
				return this.text;
			}
		}

        /// <summary>
        /// Get the output type
        /// </summary>
		public TestOutputType Type
		{
			get
			{
				return this.type;
			}
		}
	}

    /// <summary>
    /// Enum representing the output destination
    /// It uses combinable flags so that a given
    /// output control can accept multiple types
    /// of output. Normally, each individual
    /// output uses a single flag value.
    /// </summary>
	public enum TestOutputType
	{
        /// <summary>
        /// Send output to stdOut
        /// </summary>
		Out, 
        
        /// <summary>
        /// Send output to stdErr
        /// </summary>
        Error,

		/// <summary>
		/// Send output to Trace
		/// </summary>
		Trace,

		/// <summary>
		/// Send output to Log
		/// </summary>
		Log
	}
}
