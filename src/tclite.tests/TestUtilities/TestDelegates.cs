// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.TestUtilities
{
    public class TestDelegates
    {
        public static void ThrowsArgumentException()
        {
            throw new ArgumentException("myMessage", "myParam");
        }

        public static void ThrowsSystemException()
        {
            throw new Exception("my message");
        }

        public static void ThrowsNothing()
        {
        }

        public static void ThrowsCustomException()
        {
            throw new CustomException();
        }

        public static void ThrowsDerivedCustomException()
        {
            throw new DerivedCustomException();
        }

        public class CustomException : Exception
        {
        }

        public class DerivedCustomException : CustomException
        {
        }

        public class DerivedException : Exception
        {
        }
    }
}
