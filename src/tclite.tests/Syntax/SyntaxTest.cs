// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;

namespace TCLite.Framework.Syntax
{
    public abstract class SyntaxTest
    {
        protected string parseTree;
        protected IResolveConstraint staticSyntax;
        protected IResolveConstraint inheritedSyntax;
        protected IResolveConstraint builderSyntax;

        protected ConstraintExpression Builder()
        {
            return new ConstraintExpression();
        }

        [Test]
        public void SupportedByStaticSyntax()
        {
            Assert.That(
                staticSyntax.Resolve().ToString(),
                Is.EqualTo(parseTree).NoClip);
        }

        [Test]
        public void SupportedByConstraintBuilder()
        {
            Assert.That(
                builderSyntax.Resolve().ToString(),
                Is.EqualTo(parseTree).NoClip);
        }
    }
}
