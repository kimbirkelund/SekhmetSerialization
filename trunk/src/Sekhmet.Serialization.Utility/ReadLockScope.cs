using System;
using System.Threading;

namespace Sekhmet.Serialization.Utility
{
	/// <summary>
	/// A scope ensuring read lock access.
	/// </summary>
	public sealed class ReadLockScope : IDisposable
	{
		/// <summary>
		/// The reader/writer lock used.
		/// </summary>
		private readonly ReaderWriterLockSlim _rwLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadLockScope"/> class.
		/// </summary>
		/// <param name="rwLock">The rw lock.</param>
		internal ReadLockScope(ReaderWriterLockSlim rwLock)
		{
			_rwLock = rwLock;
			rwLock.EnterReadLock();
		}

		/// <summary>
		/// Releases the held reader lock.
		/// </summary>
		public void Dispose()
		{
			_rwLock.ExitReadLock();
		}
	}
}