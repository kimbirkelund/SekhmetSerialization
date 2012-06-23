using System;
using System.Threading;

namespace Sekhmet.Serialization.Utility
{
	/// <summary>
	/// A scope ensuring read lock access.
	/// </summary>
	public sealed class WriteLockScope : IDisposable
	{
		/// <summary>
		/// The reader/writer lock used.
		/// </summary>
		private readonly ReaderWriterLockSlim _rwLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadLockScope"/> class.
		/// </summary>
		/// <param name="rwLock">The reader/writer lock.</param>
		internal WriteLockScope(ReaderWriterLockSlim rwLock)
		{
			_rwLock = rwLock;
			rwLock.EnterWriteLock();
		}

		/// <summary>
		/// Releases the held writer lock.
		/// </summary>
		public void Dispose()
		{
			_rwLock.ExitWriteLock();
		}
	}
}