// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Builders
{
    class ProviderCache
    {
        private static Dictionary<CacheEntry, object> instances = new Dictionary<CacheEntry, object>();

        public static object GetInstanceOf(Type providerType)
        {
            return GetInstanceOf(providerType, null);
        }

        public static object GetInstanceOf(Type providerType, object[] providerArgs)
        {
            CacheEntry entry = new CacheEntry(providerType, providerArgs);

            object instance = instances.ContainsKey(entry)
                ?instances[entry]
                : null;

            if (instance == null)
                instances[entry] = instance = Reflect.Construct(providerType, providerArgs);

            return instance;
        }

        public static void Clear()
        {
            foreach (CacheEntry key in instances.Keys)
            {
                IDisposable provider = instances[key] as IDisposable;
                if (provider != null)
                    provider.Dispose();
            }

            instances.Clear();
        }

        class CacheEntry
        {
            private Type providerType;
            private object[] providerArgs;

            public CacheEntry(Type providerType, object[] providerArgs)
            {
                this.providerType = providerType;
                this.providerArgs = providerArgs;
            }

            public override bool Equals(object obj)
            {
                CacheEntry other = obj as CacheEntry;
                if (other == null) return false;

                return this.providerType == other.providerType;
            }

            public override int GetHashCode()
            {
                return providerType.GetHashCode();
            }
        }
    }
}
