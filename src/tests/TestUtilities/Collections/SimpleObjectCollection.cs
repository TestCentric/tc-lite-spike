// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.TestUtilities
{
	/// <summary>
	/// SimpleObjectCollection is used in testing to wrap an array or
	/// other collection, ensuring that only methods of the ICollection
	/// interface are accessible.
	/// </summary>
	class SimpleObjectCollection : ICollection
	{
		private readonly ICollection inner;

		public SimpleObjectCollection(ICollection inner)
		{
			this.inner = inner;
		}

		public SimpleObjectCollection(params object[] inner)
		{
			this.inner = inner;
		}

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			inner.CopyTo(array, index);
		}

		public int Count
		{
			get { return inner.Count; }
		}

		public bool IsSynchronized
		{
			get { return  inner.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return inner.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		#endregion
	}
}
