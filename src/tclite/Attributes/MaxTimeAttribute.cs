// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal.Commands;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
	/// <summary>
	/// Summary description for MaxTimeAttribute.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method, AllowMultiple=false, Inherited=false )]
	public sealed class MaxTimeAttribute : PropertyAttribute, ICommandDecorator
	{
        /// <summary>
        /// Construct a MaxTimeAttribute, given a time in milliseconds.
        /// </summary>
        /// <param name="milliseconds">The maximum elapsed time in milliseconds</param>
		public MaxTimeAttribute( int milliseconds )
            : base( milliseconds ) { }

        #region ICommandDecorator Members

        CommandStage ICommandDecorator.Stage
        {
            get { return CommandStage.AboveSetUpTearDown; }
        }

        int ICommandDecorator.Priority
        {
            get { return 0; }
        }

        TestCommand ICommandDecorator.Decorate(TestCommand command)
        {
            return new MaxTimeCommand(command);
        }

        #endregion
    }
}
