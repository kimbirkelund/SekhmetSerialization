using System;
using System.Threading;

namespace Sekhmet.Serialization.Utility
{
	/// <summary>
	/// A scope ensuring read lock access.
	/// </summary>
	public sealed class UpgradeableReadLockScope : IDisposable
	{
		/// <summary>
		/// The reader/writer lock used.
		/// </summary>
		private readonly ReaderWriterLockSlim _rwLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadLockScope"/> class.
		/// </summary>
		/// <param name="rwLock">The reader/writer lock.</param>
		internal UpgradeableReadLockScope(ReaderWriterLockSlim rwLock)
		{
			_rwLock = rwLock;
			rwLock.EnterUpgradeableReadLock();
		}

		/// <summary>
		/// Releases the held upgradeable reader lock.
		/// </summary>
		public void Dispose()
		{
			_rwLock.ExitUpgradeableReadLock();
		}
	}
}