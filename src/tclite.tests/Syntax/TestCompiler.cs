// ****************************************************************
// Copyright 2007, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

#if NYI
using System;
using System.CodeDom.Compiler;
using System.IO;

namespace TCLite.Framework.Syntax
{
    class TestCompiler
    {
        Microsoft.CSharp.CSharpCodeProvider provider;
        CompilerParameters options;

		public TestCompiler() : this( null, null ) { }

		public TestCompiler( string[] assemblyNames ) : this( assemblyNames, null ) { }

		public TestCompiler( string[] assemblyNames, string outputName )
		{
			this.provider = new Microsoft.CSharp.CSharpCodeProvider();
            this.options = new CompilerParameters();

			if ( assemblyNames != null && assemblyNames.Length > 0 )
				options.ReferencedAssemblies.AddRange( assemblyNames );
			if ( outputName != null )
				options.OutputAssembly = outputName;

			options.IncludeDebugInformation = false;
			options.TempFiles = new TempFileCollection( Path.GetTempPath(), false );
			options.GenerateInMemory = false;
		}

		public CompilerParameters Options
		{
			get { return options; }
		}

		public CompilerResults CompileCode( string code )
        {
            return provider.CompileAssemblyFromSource( options, code );
        }
    }
}
#endif
